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

#include "StrHelper.h"

bool StrHelper::Replace(string& str, string from, string to)
{
	size_t start_pos = str.find(from);
	if (start_pos == string::npos) return false;
	str.replace(start_pos, from.length(), to);
	return true;
}

void StrHelper::ReplaceAll(string& str, string from, string to)
{
	if (from.empty()) return;
	size_t start_pos = 0;
	while ((start_pos = str.find(from, start_pos)) != string::npos) {
		str.replace(start_pos, from.length(), to);
		start_pos += to.length();
	}
}

vector<string> StrHelper::Split(string str, char separator)
{
	vector<string> result;
	int pos = 0;
	int oldPos = 0;
	do
	{
		pos = str.find(separator, pos);
		if (oldPos != 0) oldPos++;
		string param = str.substr(oldPos, pos - oldPos);
		result.push_back(param);
		oldPos = pos;
		if (pos != string::npos) pos++;
	} while (pos != string::npos);
	return result;
}

vector<string> StrHelper::Split(string str, char separator, int maxCount)
{
	vector<string> result;
	int pos = 0;
	int oldPos = 0;
	int count = 0;
	do
	{
		pos = str.find(separator, pos);
		if (oldPos != 0) oldPos++;
		string param = str.substr(oldPos, pos - oldPos);
		result.push_back(param);
		oldPos = pos;
		count++;
		if (count >= maxCount) break;
		if (pos != string::npos) pos++;
	} while (pos != string::npos);
	return result;
}

void StrHelper::LTrim(string& str) {
	str.erase(str.begin(), find_if(str.begin(), str.end(), [](int ch) {
		return !isspace(ch);
	}));
}

void StrHelper::RTrim(string& str) {
	str.erase(find_if(str.rbegin(), str.rend(), [](int ch) {
		return !isspace(ch);
	}).base(), str.end());
}

void StrHelper::Trim(string& str) {
	LTrim(str);
	RTrim(str);
}