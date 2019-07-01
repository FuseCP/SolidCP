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

#include "TxtHelper.h"

int TxtHelper::GetStrPos(string filePath, string findStr, int startPos, int endPos)
{
	string str;
	int count = -1;
	try {
		ifstream file(filePath);
		while (getline(file, str))
		{
			count++;
			if (count < startPos) continue;
			if (endPos != -1 && count > endPos) break;
			if (str.find(findStr) != string::npos) return count;
		}
		file.close();
	}
	catch (exception ex) {
		Log::WriteError("GetStrPos error: ", ex);
	}
	return -1;
}

void TxtHelper::ReplaceStr(string filePath, string oldStr, string newStr)
{
	try
	{
		ifstream in(filePath);
		if (!in.good()) return;
		string lines((istreambuf_iterator<char>(in)), istreambuf_iterator<char>());
		in.close();
		StrHelper::ReplaceAll(lines, oldStr, newStr);
		ofstream out(filePath, ios::out | ios::trunc);
		out << lines;
		out.close();
	}
	catch (exception ex) {
		Log::WriteError("ReplaceStr error: ", ex);
	}
}

void TxtHelper::ReplaceStr(string filePath, string newStr, int pos)
{
	try
	{
		vector<string> list;
		int count = -1;
		ifstream in(filePath);
		if (!in.good()) return;
		string line;
		while (getline(in, line))
		{
			count++;
			if (count == pos)
			{
				list.push_back(newStr);
			}
			else
			{
				list.push_back(line);
			}
		}
		in.close();
		if (pos == -1) list.push_back(newStr);
		ofstream out(filePath, ios::out | ios::trunc);
		ostream_iterator<string> iter(out, "\n");
		copy(list.begin(), list.end(), iter);
		out.close();
	}
	catch (exception ex) {
		Log::WriteError("ReplaceStr error: ", ex);
	}
}

void TxtHelper::InsertStr(string filePath, string str, int pos)
{
	try
	{
		vector<string> list;
		int count = -1;
		ifstream in(filePath);
		if (!in.good()) return;
		string line;
		while (getline(in, line))
		{
			count++;
			if (count == pos) list.push_back(str);
			list.push_back(line);
		}
		in.close();
		if (pos == -1) list.push_back(str);
		ofstream out(filePath, ios::out | ios::trunc);
		ostream_iterator<string> iter(out, "\n");
		copy(list.begin(), list.end(), iter);
		out.close();
	}
	catch (exception ex) {
		Log::WriteError("InsertStr error: ", ex);
	}
}

void TxtHelper::InsertLines(string filePath, vector<string> lines, int pos)
{
	try
	{
		vector<string> list;
		int count = -1;
		ifstream in(filePath);
		if (!in.good()) return;
		string line;
		while (getline(in, line))
		{
			count++;
			if (count == pos)
			{
				for (int i = 0; i < lines.size(); i++) list.push_back(lines[i]);
			}
			list.push_back(line);
		}
		in.close();
		if (pos == -1) for (int i = 0; i < lines.size(); i++) list.push_back(lines[i]);
		ofstream out(filePath, ios::out | ios::trunc);
		ostream_iterator<string> iter(out, "\n");
		copy(list.begin(), list.end(), iter);
		out.close();
	}
	catch (exception ex) {
		Log::WriteError("InsertLines error: ", ex);
	}
}

void TxtHelper::DelStr(string filePath, int pos)
{
	try
	{
		vector<string> list;
		int count = -1;
		ifstream in(filePath);
		if (!in.good()) return;
		string line;
		while (getline(in, line))
		{
			count++;
			if (count != pos) list.push_back(line);
		}
		in.close();
		ofstream out(filePath, ios::out | ios::trunc);
		ostream_iterator<string> iter(out, "\n");
		copy(list.begin(), list.end(), iter);
		out.close();
	}
	catch (exception ex) {
		Log::WriteError("DelStr error: ", ex);
	}
}

void TxtHelper::DelLines(string filePath, int startPos, int endPos)
{
	try
	{
		vector<string> list;
		int count = -1;
		ifstream in(filePath);
		if (!in.good()) return;
		string line;
		while (getline(in, line))
		{
			count++;
			if (count < startPos || count > endPos) list.push_back(line);
		}
		in.close();
		ofstream out(filePath, ios::out | ios::trunc);
		ostream_iterator<string> iter(out, "\n");
		copy(list.begin(), list.end(), iter);
		out.close();
	}
	catch (exception ex) {
		Log::WriteError("DelLines error: ", ex);
	}
}

string TxtHelper::GetStr(string filePath, string findStr, int startPos, int endPos)
{
	int count = -1;
	try
	{
		ifstream in(filePath);
		if (!in.good()) return "";
		string line;
		while (getline(in, line))
		{
			count++;
			if (count < startPos) continue;
			if (endPos != -1 && count > endPos) break;
			if (line.find(findStr) != string::npos)
			{
				in.close();
				return line;
			}
		}
		in.close();
	}
	catch (exception ex) {
		Log::WriteError("GetStr error: ", ex);
	}
	return "";
}

string TxtHelper::GetStr(string filePath, int pos)
{
	int count = -1;
	try
	{
		ifstream in(filePath);
		if (!in.good()) return "";
		string line;
		while (getline(in, line))
		{
			count++;
			if (count == pos)
			{
				in.close();
				return line;
			}
		}
		in.close();
	}
	catch (exception ex) {
		Log::WriteError("GetStr error: ", ex);
	}
	return "";
}

void TxtHelper::ReplaceLines(string filePath, vector<string> lines, int startPos, int endPos)
{
	int count = -1;
	bool added = false;
	try
	{
		vector<string> newList;
		ifstream in(filePath);
		if (!in.good()) return;
		string line;
		while (getline(in, line))
		{
			count++;
			if (count < startPos || (count > endPos && endPos != -1))
			{
				newList.push_back(line);
			}
			else
			{
				if (!added)
				{
					added = true;
					for (int i=0; i<lines.size(); i++)
					{
						newList.push_back(lines[i]);
					}
				}
			}
		}
		in.close();
		ofstream out(filePath, ios::out | ios::trunc);
		ostream_iterator<string> iter(out, "\n");
		copy(newList.begin(), newList.end(), iter);
		out.close();
	}
	catch (exception ex) {
		Log::WriteError("ReplaceLines error: ", ex);
	}
}