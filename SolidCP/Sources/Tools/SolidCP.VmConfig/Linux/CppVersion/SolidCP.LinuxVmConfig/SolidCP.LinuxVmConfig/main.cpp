// Copyright (c) 2016, SolidCP
// SolidCP is distributed under the Creative Commons Share-alike license
// 
// SolidCP is a fork of WebsitePanel:
// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#include "OsVersion.h"
#include "ShellHelper.h"
#include "StrHelper.h"
#include "Log.h"
#include "AppInfo.h"
#include "KvpUtils.h"
#include "Task.h"
#include "ExecutionContext.h"
#include "ChangeComputerName.h"
#include "SetupNetworkAdapter.h"
#include "ChangeAdministratorPassword.h"
#include <thread>
#include <sstream>

using namespace std;

void InstallService();
void InstallService_Linux();
void InstallService_FreeBSD();
void InstallService_PfSense();
void ProcessTasks();
void ProcessIdleTimer();
void DeleteOldResults();
void RebootSystem();
void StartIdleTimer();
void InitializeSupportedTasks();
void LoadConfig();
void ParseParameters(map<string, string>& dictionary, string parameters);
void SaveExecutionResult(string name, const ExecutionResult& res, string started, string ended);
void Start();

const string TaskPrefix = "SCP-";
const string CurrentTaskName = "SCP-CurrentTask";

const string DEFAULT_KVP_DIRECTORY_LINUX = "/var/lib/hyperv/";
const string DEFAULT_KVP_DIRECTORY_FREEBSD = "/var/db/hyperv/pool/";
const string KVP_BASEFILENAME = ".kvp_pool_";
string InputKVP = DEFAULT_KVP_DIRECTORY_LINUX + KVP_BASEFILENAME + "0";
string OutputKVP = DEFAULT_KVP_DIRECTORY_LINUX + KVP_BASEFILENAME + "1";

vector<string> supportedTasks;

int pollInterval = 30000;
int idleInterval = 0;//value in seconds / 0 - disabled
int delayOnStart = 5000;
int idleTimer = 0;
bool rebootRequired = false;
bool stopService = false;

int main(int argcount, char* args[])
{
	if (OsVersion::IsFreeBsdOs())
	{
		InputKVP = DEFAULT_KVP_DIRECTORY_FREEBSD + KVP_BASEFILENAME + "0";
		OutputKVP = DEFAULT_KVP_DIRECTORY_FREEBSD + KVP_BASEFILENAME + "1";
	}

	AppInfo::InitAppInfo(argcount, args);
	LoadConfig();

	for (int i = 1; i < argcount; i++)
	{
		string arg(args[i]);
		if (arg == "install") {
			InstallService();
			return 0;
		}
	}

	std::thread mainThread(Start);
	mainThread.join();
	return 0;
}

void LoadConfig()
{
	const string cfgFile = AppInfo::appPath + "/config.cfg";
	try
	{
		ifstream in(cfgFile);
		if (!in.good()) return;
		string config((istreambuf_iterator<char>(in)), istreambuf_iterator<char>());
		in.close();
		istringstream file(config);
		string line;
		while (getline(file, line))
		{
			if (line.find_first_of('#') == 0) continue;
			istringstream inLine(line);
			string key;
			if (getline(inLine, key, '='))
			{
				string value;
				if (getline(inLine, value))
				{
					if (key == "pollInterval")
					{
						pollInterval = stoi(value);
					}
					else if (key == "idleInterval")
					{
						idleInterval = stoi(value);
					}
					else if (key == "delayOnStart")
					{
						delayOnStart = stoi(value);
					}
					else if (key == "pfSenseAdmin")
					{
						AppInfo::pfSenseAdmin = value;
					}
				}
			}
		}
	}catch (exception ex) {}
}

void Start()
{
	Log::WriteApplicationStart();
	
	this_thread::sleep_for(chrono::milliseconds(delayOnStart));

	InitializeSupportedTasks();
	StartIdleTimer();

	while (!stopService)
	{
		ProcessTasks();
		this_thread::sleep_for(chrono::milliseconds(pollInterval));
	}
}

void InitializeSupportedTasks()
{
	supportedTasks.push_back("ChangeComputerName");
	supportedTasks.push_back("ChangeAdministratorPassword");
	supportedTasks.push_back("SetupNetworkAdapter");
}

void StartIdleTimer()
{
	if (idleInterval <= 0) return;
	std::thread idleTimerThread(ProcessIdleTimer);
	idleTimerThread.detach();
}

void ProcessIdleTimer()
{
	while (!stopService)
	{
		idleTimer++;
		this_thread::sleep_for(chrono::milliseconds(1000));
		if (idleTimer >= idleInterval) stopService = true;
	}
}

void ProcessTasks()
{
	DeleteOldResults();

	//process all tasks
	while (!stopService)
	{
		//load completed tasks results
		vector<string> strResults = KvpUtils::GetKvpKeys(OutputKVP);
		vector<string> results;
		for (int i = 0; i < strResults.size(); i++)
		{
			if (strResults[i].length() > 0 && strResults[i].find(TaskPrefix) == 0 && strResults[i] != CurrentTaskName)
			{
				//save only SolidCP tasks
				results.push_back(strResults[i]);
			}
		}

		// sorted list of tasks - will be sorted by the TaskID (time in ticks)
		vector<Task> tasks;

		//load task definitions from input registry key 
		vector<string> strTasks = KvpUtils::GetKvpKeys(InputKVP);
		for (int i = 0; i < strTasks.size(); i++)
		{
			if (find(results.begin(), results.end(), strTasks[i]) != results.end()) continue; //skip completed tasks
			
			if (strTasks[i].length() > 0 && strTasks[i].find(TaskPrefix) == 0)
			{
				// extract Task ID parameter
				int idx = strTasks[i].find_last_of('-');
				if (idx == string::npos) continue;

				string strTaskId = strTasks[i].substr(idx + 1);
				long taskId = 0;
				try
				{
					taskId = stol(strTaskId);
				}
				catch (exception ex)
				{
					continue; // wrong task format
				}

				bool add = true;
				for (int i = 0; i < tasks.size(); i++)
				{
					if (tasks[i].taskId == taskId) {
						add = false;
						break;
					}
				}
				if (add) {
					Task task(taskId, strTasks[i]);
					tasks.push_back(task);
				}
			}
		}
		if (tasks.size() == 0)
		{
			if (rebootRequired)
			{
				Log::WriteInfo("Reboot required");
				RebootSystem();
				return;
			}
			return;
		}
		else
		{
			idleTimer = 0;
		}

		ExecutionContext context;
		for (int i = 0; i < tasks.size(); i++)
		{
			//find first correct task 
			string taskDefinition = tasks[i].taskName;
			string taskParameters = KvpUtils::GetKvpStringValue(InputKVP, taskDefinition);
			if (taskDefinition.find_last_of("-") == string::npos || taskDefinition.find_last_of('-') == taskDefinition.length() - 1)
			{
				Log::WriteError("Task was deleted from queue as its definition is invalid : " + taskDefinition);
				//go to next task
				continue;

			}
			string taskName = taskDefinition.substr(0, taskDefinition.find_last_of("-")).substr(TaskPrefix.length());
			string taskId = taskDefinition.substr(taskDefinition.find_last_of('-') + 1);

			if (find(supportedTasks.begin(), supportedTasks.end(), taskName) == supportedTasks.end())
			{
				Log::WriteError("Task was deleted from queue as its definition was not found : " + taskName);
				//go to next task
				continue;
			}
			//prepare execution context for correct task
			context.ActivityID = taskId;
			context.ActivityName = taskName;
			ParseParameters(context.Parameters, taskParameters);
			context.ActivityDefinition = taskDefinition;
			break;
		}
		if (context.ActivityName.length() > 0)
		{
			ExecutionResult res;
			string start = Log::GetDateTime();

			try
			{
				//load module and run task
				Log::WriteStart("Starting " + context.ActivityName + " module...");
				context.Progress = 0;
				if (context.ActivityName == "ChangeComputerName")
				{
					res = ChangeComputerName::Run(context);
				}
				else if (context.ActivityName == "ChangeAdministratorPassword")
				{
					res = ChangeAdministratorPassword::Run(context);
				}
				else if (context.ActivityName == "SetupNetworkAdapter")
				{
					res = SetupNetworkAdapter::Run(context);
				}
				context.Progress = 100;
				Log::WriteEnd(context.ActivityName + " module finished.");
			}
			catch (exception ex)
			{
				Log::WriteError("Unhandled exception:", ex);
				res.ResultCode = -1;
				res.ErrorMessage = "Unhandled exception: " + (string)ex.what();
			}
			string end = Log::GetDateTime();
			SaveExecutionResult(context.ActivityDefinition, res, start, end);

			if (res.RebootRequired) rebootRequired = true;
		}
		this_thread::sleep_for(chrono::milliseconds(1000));
	}
}

void SaveExecutionResult(string name, const ExecutionResult& res, string started, string ended)
{
	ostringstream stringStream;
	stringStream << "ResultCode=" << res.ResultCode << "|";
	stringStream << "RebootRequired=" << res.RebootRequired << "|";
	stringStream << "ErrorMessage=" << res.ErrorMessage << "|";
	stringStream << "Value=" << res.Value << "|";
	stringStream << "Started=" << started << "|";
	stringStream << "Ended=" << ended;
	KvpUtils::SetKvpStringValue(OutputKVP, name, stringStream.str());
}

void ParseParameters(map<string, string>& dictionary, string parameters) 
{
	if (parameters.length() == 0) return;
	vector<string> pairs = StrHelper::Split(parameters, '|');
	
	for (int i = 0; i < pairs.size(); i++)
	{
		vector<string> parts = StrHelper::Split(pairs[i], '=' , 2);
		if (parts.size() != 2) continue;
		if (dictionary.count(parts[0]) == 0) dictionary.insert(pair<string, string>(parts[0], parts[1]));
	}
}

void RebootSystem()
{
	try
	{
		Log::WriteStart("RebootSystem");
		ShellHelper::RunCmd("reboot");
		Log::WriteEnd("RebootSystem");
	}
	catch (exception ex)
	{
		Log::WriteError("Reboot System error:", ex);
	}
}

void DeleteOldResults()
{
	// get the list of input tasks
	vector<string> strTasks = KvpUtils::GetKvpKeys(InputKVP);
	vector<string> tasks;
	for (int i = 0; i < strTasks.size(); i++) 
	{
		if (strTasks[i].length() > 0 && strTasks[i].find(TaskPrefix) == 0 && strTasks[i] != CurrentTaskName)
		{
			//save only SolidCP tasks
			tasks.push_back(strTasks[i]);
		}
	}
	// get the list of task results
	int deletedResults = 0;
	vector<string> strResults = KvpUtils::GetKvpKeys(OutputKVP);
	for (int i = 0; i < strResults.size(); i++) 
	{
		if (strResults[i].length() > 0 && strResults[i].find(TaskPrefix) == 0 && strResults[i] != CurrentTaskName)
		{
			// check if task result exists in the input tasks
			if (find(tasks.begin(), tasks.end(), strResults[i]) == tasks.end())
			{
				KvpUtils::DeleteKvpKey(OutputKVP, strResults[i]);
				Log::WriteInfo("Deleted activity result: " + strResults[i]);
				deletedResults++;
			}
		}
	}
	if (deletedResults > 0) Log::WriteEnd(to_string(deletedResults) + " result(s) deleted");
}

void InstallService()
{
	switch (OsVersion::GetOsVersion())
	{
	case FreeBSD:
		InstallService_FreeBSD();
		break;
	case PfSense:
		InstallService_PfSense();
		break;
	default:
		InstallService_Linux();
		break;
	}
}

void InstallService_Linux()
{
	const string serviceName = "solidcp.service";
	const string servicesPath = "/etc/systemd/system/";
	string userName = AppInfo::userName;
	string appPath = AppInfo::appPath;
	string appExe = AppInfo::appExeName;

	ShellHelper::RunCmd("systemctl stop " + serviceName);
	ShellHelper::RunCmd("systemctl disable " + serviceName);

	vector<string> config;
	config.push_back("[Unit]");
	config.push_back("Description=SolidCP LinuxVmConfig Service");
	config.push_back("[Service]");
	config.push_back("User=" + userName);
	config.push_back("WorkingDirectory=" + appPath);
	config.push_back("ExecStart=" + appPath + "/" + appExe);
	config.push_back("SuccessExitStatus=0");
	config.push_back("TimeoutStopSec=infinity");
	config.push_back("Restart=on-failure");
	config.push_back("RestartSec=5");
	config.push_back("[Install]");
	config.push_back("WantedBy=multi-user.target");

	ofstream out(servicesPath + serviceName, ios::out | ios::trunc);
	ostream_iterator<string> iter(out, "\n");
	copy(config.begin(), config.end(), iter);
	out.close();

	ExecutionResult res = ShellHelper::RunCmd("systemctl daemon-reload");
	if (res.ResultCode == 1)
	{
		Log::WriteError("Service install error: " + res.ErrorMessage);
		return;
	}
	res = ShellHelper::RunCmd("systemctl enable " + serviceName);
	if (res.ResultCode == 1)
	{
		Log::WriteError("Service install error: " + res.ErrorMessage);
		return;
	}
	res = ShellHelper::RunCmd("systemctl start " + serviceName);
	if (res.ResultCode == 1)
	{
		Log::WriteError("Service install error: " + res.ErrorMessage);
		return;
	}
	Log::WriteInfo(serviceName + " successfully installed.");
}

void InstallService_FreeBSD()
{
	const string rcConf = "/etc/rc.conf";
	const string serviceName = "solidcp";
	const string servicesPath = "/etc/rc.d/";
	string userName = AppInfo::userName;
	string appPath = AppInfo::appPath;
	string appExe = AppInfo::appExeName;

	ShellHelper::RunCmd("service " + serviceName + " stop");

	vector<string> config;

	config.push_back("#!/bin/sh");
	config.push_back("");
	config.push_back("# SolidCP LinuxVmConfig Service");
	config.push_back("# PROVIDE: " + serviceName);
	config.push_back("# REQUIRE: DAEMON networking");
	config.push_back("# BEFORE:  LOGIN");
	config.push_back("");
	config.push_back(". /etc/rc.subr");
	config.push_back("");
	config.push_back("name=" + serviceName);
	config.push_back("rcvar=" + serviceName + "_enable");
	config.push_back(serviceName + "_user=\"" + userName + "\"");
	config.push_back("command=\"" + appPath + "/" + appExe + "\"");
	config.push_back("pidfile=\"/var/run/" + serviceName + ".pid\"");
	config.push_back("");
	config.push_back("start_cmd=\"" + serviceName + "_start\"");
	config.push_back("stop_cmd=\"" + serviceName + "_stop\"");
	config.push_back("status_cmd=\"" + serviceName + "_status\"");
	config.push_back("");
	config.push_back(serviceName + "_start() {");
	config.push_back("   /usr/sbin/daemon -P ${pidfile} -r -f -u $" + serviceName + "_user $command");
	config.push_back("}");
	config.push_back("");
	config.push_back(serviceName + "_stop() {");
	config.push_back("   if [ -e \"${pidfile}\" ]; then");
	config.push_back("      kill -s TERM `cat ${pidfile}`");
	config.push_back("   else");
	config.push_back("      echo \"SolidCP.VmConfig is not running\"");
	config.push_back("   fi");
	config.push_back("}");
	config.push_back("");
	config.push_back(serviceName + "_status() {");
	config.push_back("   if [ -e \"${pidfile}\" ]; then");
	config.push_back("      echo \"SolidCP.VmConfig is running as pid `cat ${pidfile}`\"");
	config.push_back("   else");
	config.push_back("      echo \"SolidCP.VmConfig is not running\"");
	config.push_back("   fi");
	config.push_back("}");
	config.push_back("");
	config.push_back("load_rc_config $name");
	config.push_back("run_rc_command \"$1\"");

	ofstream out(servicesPath + serviceName, ios::out | ios::trunc);
	ostream_iterator<string> iter(out, "\n");
	copy(config.begin(), config.end(), iter);
	out.close();

	ShellHelper::RunCmd("chmod +x " + servicesPath + serviceName);
	int pos = TxtHelper::GetStrPos(rcConf, serviceName + "_enable", 0, -1);
	TxtHelper::ReplaceStr(rcConf, serviceName + "_enable=\"YES\"", pos);

	ExecutionResult res = ShellHelper::RunCmd("service " + serviceName + " start");
	if (res.ResultCode == 1)
	{
		Log::WriteError("Service install error: " + res.ErrorMessage);
		return;
	}
	Log::WriteInfo(serviceName + " service successfully installed.");
}

void InstallService_PfSense()
{
	const string serviceName = "solidcp";
	const string servicesPath = "/usr/local/etc/rc.d/";
	const string configXmlPath = "/cf/conf/config.xml";
	string userName = AppInfo::userName;
	string appPath = AppInfo::appPath;
	string appExe = AppInfo::appExeName;

	ShellHelper::RunCmd("service " + serviceName + " onestop");

	vector<string> config;

	config.push_back("#!/bin/sh");
	config.push_back("");
	config.push_back("# SolidCP LinuxVmConfig Service");
	config.push_back("# PROVIDE: " + serviceName);
	config.push_back("# REQUIRE: DAEMON networking");
	config.push_back("# BEFORE:  LOGIN");
	config.push_back("");
	config.push_back(". /etc/rc.subr");
	config.push_back("");
	config.push_back("name=" + serviceName);
	config.push_back("rcvar=" + serviceName + "_enable");
	config.push_back(serviceName + "_user=\"" + userName + "\"");
	config.push_back("command=\"" + appPath + "/" + appExe + "\"");
	config.push_back("pidfile=\"/var/run/" + serviceName + ".pid\"");
	config.push_back("");
	config.push_back("start_cmd=\"" + serviceName + "_start\"");
	config.push_back("stop_cmd=\"" + serviceName + "_stop\"");
	config.push_back("status_cmd=\"" + serviceName + "_status\"");
	config.push_back("");
	config.push_back(serviceName + "_start() {");
	config.push_back("   /usr/sbin/daemon -P ${pidfile} -r -f -u $" + serviceName + "_user $command");
	config.push_back("}");
	config.push_back("");
	config.push_back(serviceName + "_stop() {");
	config.push_back("   if [ -e \"${pidfile}\" ]; then");
	config.push_back("      kill -s TERM `cat ${pidfile}`");
	config.push_back("   else");
	config.push_back("      echo \"SolidCP.VmConfig is not running\"");
	config.push_back("   fi");
	config.push_back("}");
	config.push_back("");
	config.push_back(serviceName + "_status() {");
	config.push_back("   if [ -e \"${pidfile}\" ]; then");
	config.push_back("      echo \"SolidCP.VmConfig is running as pid `cat ${pidfile}`\"");
	config.push_back("   else");
	config.push_back("      echo \"SolidCP.VmConfig is not running\"");
	config.push_back("   fi");
	config.push_back("}");
	config.push_back("");
	config.push_back("load_rc_config $name");
	config.push_back("run_rc_command \"$1\"");

	ofstream out(servicesPath + serviceName, ios::out | ios::trunc);
	ostream_iterator<string> iter(out, "\n");
	copy(config.begin(), config.end(), iter);
	out.close();

	ShellHelper::RunCmd("chmod +x " + servicesPath + serviceName);

	string shellcmd = "<shellcmd>service " + serviceName + " onestart</shellcmd>";
	int systemStart = TxtHelper::GetStrPos(configXmlPath, "<system>", 0, -1);
	int systemEnd = TxtHelper::GetStrPos(configXmlPath, "</system>", systemStart, -1);
	if (TxtHelper::GetStrPos(configXmlPath, shellcmd, systemStart, systemEnd) == -1)
	{
		TxtHelper::InsertStr(configXmlPath, shellcmd, systemEnd);
	}

	ExecutionResult res = ShellHelper::RunCmd("service " + serviceName + " onestart");
	if (res.ResultCode == 1)
	{
		Log::WriteError("Service install error: " + res.ErrorMessage);
		return;
	}
	Log::WriteInfo(serviceName + " service successfully installed.");
}