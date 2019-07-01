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

#include "Log.h"

void Log::WriteError(string message, exception ex)
{
	string msg = "[" + GetDateTime() + "] ERROR: " + message + ex.what();
	cout << msg << endl;
	WriteLogFile(msg);
}

void Log::WriteError(string message)
{
	string msg = "[" + GetDateTime() + "] ERROR: " + message;
	cout << msg << endl;
	WriteLogFile(msg);
}

void Log::WriteInfo(string message)
{
	string msg = "[" + GetDateTime() + "] INFO: " + message;
	cout << msg << endl;
	WriteLogFile(msg);
}

void Log::WriteStart(string message)
{
	string msg = "[" + GetDateTime() + "] START: " + message;
	cout << msg << endl;
	WriteLogFile(msg);
}

void Log::WriteEnd(string message)
{
	string msg = "[" + GetDateTime() + "] END: " + message;
	cout << msg << endl;
	WriteLogFile(msg);
}

void Log::WriteApplicationStart()
{
	string msg = "[" + GetDateTime() + "] APP: " + AppInfo::appExeName + " " + AppInfo::appVersion + " started successfully";
	cout << msg << endl;
	WriteLogFile(msg);
}

string Log::GetDateTime()
{
	time_t now = chrono::system_clock::to_time_t(chrono::system_clock::now());
	string time(ctime(&now));
	return time.substr(0, time.length() - 1);
}

void Log::WriteLogFile(string message)
{
	try {
		string logPath = AppInfo::appPath + "/" + AppInfo::appExeName + ".log";
		ifstream f(logPath);
		bool exists = f.good();
		f.close();
		if (!exists) {
			ofstream logFile(logPath);
			logFile << message << endl;
			logFile.close();
		}
		else
		{
			ofstream logFile;
			logFile.open(logPath, ios_base::app);
			logFile << message << endl;
			logFile.close();
		}
	}
	catch (exception) {}
}