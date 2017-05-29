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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace WseClean
{
	class Program
	{
		// Methods
		private static void CleanXmlTypeDefinitions(string pathToFile)
		{
			Regex regex = new Regex(@"\[([^\]]{2,})\]");
			string[] strArray = ReadWseFileContent(pathToFile);
			StringBuilder builder = new StringBuilder();
			StringBuilder builder2 = new StringBuilder();
			for (int i = 0; i < strArray.Length; i++)
			{
				string str = strArray[i];
				builder2.AppendLine(str);
				if (!str.Contains("///"))
				{
					if (regex.IsMatch(str))
					{
						if (str.IndexOf("XmlTypeAttribute") > -1)
						{
                            int braces = 0;
                            bool started = false;
							while (!started || braces > 0)
							{
                                if (str.IndexOf("}") != -1)
                                {
                                    braces--;
                                    started = true;
                                }
                                else if (str.IndexOf("{") != -1)
                                {
                                    braces++;
                                    started = true;
                                }

								i++;
								str = strArray[i];
							}
							builder2.Remove(0, builder2.Length);
						}
					}
					else if (builder2.Length > 0)
					{
						builder.Append(builder2.ToString());
						builder2.Remove(0, builder2.Length);
					}
				}
			}
			File.WriteAllText(pathToFile, builder.ToString());
		}

		private static void Main(string[] args)
		{
			if ((args != null) && (args.Length > 0))
			{
				CleanXmlTypeDefinitions(args[0]);
			}
			else
			{
				ShowCleanerUsage();
			}
		}

		private static string[] ReadWseFileContent(string pathToFile)
		{
			if (!File.Exists(pathToFile))
			{
				throw new FileNotFoundException("Sorry but it seems that file you've specified doesn't exist");
			}
			return File.ReadAllLines(pathToFile);
		}

		private static void ShowCleanerUsage()
		{
			Console.WriteLine("WseCleaner usage guidelines...");
			Console.ReadKey();
		}
	}
}
