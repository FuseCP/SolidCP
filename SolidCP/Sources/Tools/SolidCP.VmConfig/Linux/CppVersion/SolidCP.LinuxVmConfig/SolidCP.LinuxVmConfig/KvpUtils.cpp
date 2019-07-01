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

#include "KvpUtils.h"

const int KvpUtils::KVP_MAX_KEY_SIZE = 512;
const int KvpUtils::KVP_MAX_VALUE_SIZE = 2048;

vector<string> KvpUtils::GetKvpKeys(string pool)
{
	char bKey[KVP_MAX_KEY_SIZE];
	char bValue[KVP_MAX_VALUE_SIZE];
	vector<string> result;
	try
	{
		ifstream input(pool, ios::in | ios::binary);
		while (input.good() && input.peek() != EOF)
		{
			input.read(bKey, KVP_MAX_KEY_SIZE);
			input.read(bValue, KVP_MAX_VALUE_SIZE);
			int endPos = 0;
			for (int i = 0; i < KVP_MAX_KEY_SIZE; i++)
			{
				if (bKey[i] == 0 ) 
				{
					endPos = i + 1;
					break;
				}
			}
			if (endPos > 0)
			{
				char* chKey = new char[endPos];
				copy(bKey, bKey + endPos, chKey);
				string strKey(chKey);
				delete[] chKey;
				result.push_back(strKey);
			}
		}
		input.close();
		return result;
	}
	catch (exception ex)
	{
		Log::WriteError("GetKvpKeys error: ", ex);
	}
	return result;
}

string KvpUtils::GetKvpStringValue(string pool, string key)
{
	string sValue = "";
	char bKey[KVP_MAX_KEY_SIZE];
	char bValue[KVP_MAX_VALUE_SIZE];

	try
	{
		ifstream input(pool, ios::in | ios::binary);
		while (input.good() && input.peek() != EOF)
		{
			input.read(bKey, KVP_MAX_KEY_SIZE);
			input.read(bValue, KVP_MAX_VALUE_SIZE);
			int endPos = 0;
			for (int i = 0; i < KVP_MAX_KEY_SIZE; i++)
			{
				if (bKey[i] == 0)
				{
					endPos = i + 1;
					break;
				}
			}
			if (endPos > 0)
			{
				char* chKey = new char[endPos];
				copy(bKey, bKey + endPos, chKey);
				string strKey(chKey);
				delete[] chKey;
				if (strKey == key)
				{
					endPos = 0;
					for (int i = 0; i < KVP_MAX_VALUE_SIZE; i++)
					{
						if (bValue[i] == 0)
						{
							endPos = i + 1;
							break;
						}
					}
					if (endPos > 0)
					{
						char* chValue = new char[endPos];
						copy(bValue, bValue + endPos, chValue);
						sValue = string(chValue);
						delete[] chValue;
						input.close();
						return sValue;
					}
				}
			}
		}
		input.close();
	}
	catch (exception ex)
	{
		Log::WriteError("GetKvpStringValue error: ", ex);
	}
	return sValue;
}

void KvpUtils::SetKvpStringValue(string pool, string key, string value)
{
	char bKey[KVP_MAX_KEY_SIZE];
	char bValue[KVP_MAX_VALUE_SIZE];

	try
	{
		char data[KVP_MAX_VALUE_SIZE];
		fill_n(data, KVP_MAX_VALUE_SIZE, 0);
		strcpy(data, value.c_str());

		bool edit = false;
		long offset = 0;

		ifstream input(pool, ios::in | ios::binary);
		while (input.good() && input.peek() != EOF)
		{
			input.read(bKey, KVP_MAX_KEY_SIZE);
			offset += KVP_MAX_KEY_SIZE;
			int endPos = 0;
			for (int i = 0; i < KVP_MAX_KEY_SIZE; i++)
			{
				if (bKey[i] == 0)
				{
					endPos = i + 1;
					break;
				}
			}
			if (endPos > 0)
			{
				char* chKey = new char[endPos];
				copy(bKey, bKey + endPos, chKey);
				string strKey(chKey);
				delete[] chKey;
				if (strKey == key)
				{
					edit = true;
					break;
				}
				input.read(bValue, KVP_MAX_VALUE_SIZE);
				offset += KVP_MAX_VALUE_SIZE;
			}
		}
		input.close();
		if (edit)
		{
			EditKvpValue(pool, offset, data);
		}
		else
		{
			AddKvp(pool, key, value);
		}
	}
	catch (exception ex)
	{
		Log::WriteError("SetKvpStringValue error:", ex);
	}
}

void KvpUtils::EditKvpValue(string pool, int offset, char data[])
{
	try
	{
		ifstream input(pool, ios::in | ios::binary | ios::ate);
		if (input.is_open())
		{
			streampos size = input.tellg();
			char* fBytes = new char[size];
			input.seekg(0, ios::beg);
			input.read(fBytes, size);
			input.close();
			copy(data, data + KVP_MAX_VALUE_SIZE, fBytes + offset);
			ofstream output(pool, ios::out | ios::trunc | ios::binary);
			output.write(fBytes, size);
			output.close();
			delete[] fBytes;
		}
	}
	catch (exception ex)
	{
		Log::WriteError("EditKvpValue error: ", ex);
	}
}

void KvpUtils::AddKvp(string pool, string key, string value)
{
	try
	{
		const int dataLength = KVP_MAX_KEY_SIZE + KVP_MAX_VALUE_SIZE;
		char data[dataLength];
		fill_n(data, dataLength, 0);

		char bKey[KVP_MAX_KEY_SIZE] = {};
		strcpy(bKey, key.c_str());
		copy(bKey, bKey + KVP_MAX_KEY_SIZE, data);

		char bValue[KVP_MAX_VALUE_SIZE] = {};
		strcpy(bValue, value.c_str());
		copy(bValue, bValue + KVP_MAX_VALUE_SIZE, data + KVP_MAX_KEY_SIZE);

		ofstream output(pool, ios::out | ios::app | ios::binary);
		output.write(data, dataLength);
		output.close();
	}
	catch (exception ex)
	{
		Log::WriteError("AddKvp error:", ex);
	}
}

void KvpUtils::DeleteKvpKey(string pool, string key)
{
	char bKey[KVP_MAX_KEY_SIZE];
	char bValue[KVP_MAX_VALUE_SIZE];

	try
	{
		bool del = false;
		long offset = 0;

		ifstream input(pool, ios::in | ios::binary);
		while (input.good() && input.peek() != EOF)
		{
			input.read(bKey, KVP_MAX_KEY_SIZE);
			int endPos = 0;
			for (int i = 0; i < KVP_MAX_KEY_SIZE; i++)
			{
				if (bKey[i] == 0)
				{
					endPos = i + 1;
					break;
				}
			}
			if (endPos > 0)
			{
				char* chKey = new char[endPos];
				copy(bKey, bKey + endPos, chKey);
				string strKey(chKey);
				delete[] chKey;
				if (strKey == key)
				{
					del = true;
					break;
				}
				input.read(bValue, KVP_MAX_VALUE_SIZE);
				offset += KVP_MAX_VALUE_SIZE + KVP_MAX_KEY_SIZE;
			}
		}
		input.close();
		if (del) DeleteKvp(pool, offset);
	}
	catch (exception ex)
	{
		Log::WriteError("DeleteKvpKey error:", ex);
	}
}

void KvpUtils::DeleteKvp(string pool, int offset)
{
	try
	{
		const int delLength = KVP_MAX_KEY_SIZE + KVP_MAX_VALUE_SIZE;
		ifstream input(pool, ios::in | ios::binary | ios::ate);
		if (input.is_open())
		{
			streampos size = input.tellg();
			char* fBytes = new char[size];
			input.seekg(0, ios::beg);
			input.read(fBytes, size);
			input.close();

			long newSize = (long)size - delLength;
			if (newSize > 0) 
			{
				char resArray[newSize];
				if (offset > 0) copy(fBytes, fBytes + offset, resArray);
				if (newSize - offset > 0) copy(fBytes + offset + delLength, fBytes + (long)size, resArray + offset);
				ofstream output(pool, ios::out | ios::trunc | ios::binary);
				output.write(resArray, newSize);
				output.close();
			}
			else
			{
				ofstream output(pool, ios::out | ios::trunc | ios::binary);
				output.close();
			}
			delete[] fBytes;
		}
	}
	catch (exception ex)
	{
		Log::WriteError("DeleteKvp error: ", ex);
	}
}