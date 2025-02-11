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

using System;
using System.IO;
using System.Collections.Generic;

namespace SolidCP.LinuxVmConfig
{
    public class TxtHelper
    {
        public static void ReplaceStr(string filePath, string findStr, string replaceStr)
        {
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> list = new List<string>();
                foreach (string str in listIE)
                {
                    string res = str.Replace(findStr, replaceStr, StringComparison.Ordinal);
                    list.Add(res);
                }
                File.WriteAllLines(filePath, list);
            }
            catch (Exception ex) {
                Log.WriteError("ReplaceStr error: " + ex.ToString());
            }
        }

        public static void ReplaceStr(string filePath, string newStr, int pos)
        {
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> list = new List<string>();
                int count = -1;
                foreach (string str in listIE)
                {
                    count++;
                    if (count == pos)
                    {
                        list.Add(newStr);
                    }
                    else
                    {
                        list.Add(str);
                    }
                }
                if (pos == -1) list.Add(newStr);
                File.WriteAllLines(filePath, list);
            }
            catch (Exception ex) {
                Log.WriteError("ReplaceStr error: " + ex.ToString());
            }
        }

        public static void DelStr(string filePath, int pos)
        {
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> list = new List<string>();
                int count = -1;
                foreach (string str in listIE)
                {
                    count++;
                    if (count != pos) list.Add(str);
                }
                File.WriteAllLines(filePath, list);
            }
            catch (Exception ex) {
                Log.WriteError("DelStr error: " + ex.ToString());
            }
        }

        public static int GetStrPos(string filePath, string findStr, int startPos, int endPos)
        {
            int count = -1;
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                foreach (string str in listIE)
                {
                    count++;
                    if (count < startPos) continue;
                    if (endPos != -1 && count > endPos) break;
                    if (str.Contains(findStr))
                    {
                        return count;
                    }
                }
            }
            catch (Exception ex) {
                Log.WriteError("GetStrPos error: " + ex.ToString());
            }
            return -1;
        }

        public static string GetStr(string filePath, string findStr, int startPos, int endPos)
        {
            int count = -1;
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                foreach (string str in listIE)
                {
                    count++;
                    if (count < startPos) continue;
                    if (endPos != -1 && count > endPos) break;
                    if (str.Contains(findStr))
                    {
                        return str;
                    }
                }
            }
            catch (Exception ex) {
                Log.WriteError("GetStr error: " + ex.ToString());
            }
            return null;
        }

        public static void ReplaceAllStr(string filePath, List<string> strList, int startPos, int endPos)
        {
            int count = -1;
            bool added = false;
            try
            {
                System.Collections.Generic.IEnumerable<string> listIE = File.ReadLines(filePath);
                List<string> newList = new List<string>();
                foreach (string str in listIE)
                {
                    count++;
                    if (count < startPos || (count > endPos && endPos != -1))
                    {
                        newList.Add(str);
                    }
                    else
                    {
                        if (!added)
                        {
                            added = true;
                            foreach (string newStr in strList)
                            {
                                newList.Add(newStr);
                            }

                            // if start and end the same then insert text on startPos instead of replace
                            if (startPos == endPos)
                            {
                                newList.Add(str);
                            }

                        }
                    }
                }
                File.WriteAllLines(filePath, newList);
            }
            catch (Exception ex) {
                Log.WriteError("ReplaceAllStr error: " + ex.ToString());
            }
        }

        public static void AddAllStr(string filePath, List<string> strList)
        {
            try
            {
                File.WriteAllLines(filePath, strList);

            }
            catch (Exception ex)
            {
                Log.WriteError("AddAllStr error: " + ex.ToString());
            }
        }

    }
}
