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
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace ChangeCopyrightInfo
{
    class Program
    {
        static string Path
        {
            get { return ConfigurationManager.AppSettings["path"]; }
        }

        static string Files
        {
            get { return ConfigurationManager.AppSettings["files"]; }
        }

        static List<string> copyrigth = null;
        static List<string> Copyrigth
        {
            get {
                if (copyrigth != null) return copyrigth;
                copyrigth = new List<string>();
                string text = ConfigurationManager.AppSettings["copyrigth"];
                if (text == null) return copyrigth;
                copyrigth.AddRange( text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) );
                return copyrigth;
            }
        }
        static string startcopyrigth
        {
            get { return ConfigurationManager.AppSettings["startcopyrigth"]; }
        }

        static void Main(string[] args)
        {
            Log.Initialize(ConfigurationManager.AppSettings["LogFile"]);
            Log.WriteApplicationStart();

            if (Path != null)
                CheckDirectory(Path);

            Log.WriteApplicationEnd();
        }

        static void CheckDirectory(string directory)
        {
            Log.WriteLine("Check " + directory);

            string[] dirFiles = Directory.GetFiles(directory, Files);

            foreach (string file in dirFiles)
                CheckFile(file);

            string[] dirDirs = Directory.GetDirectories(directory);

            foreach(string dir in dirDirs)
                CheckDirectory(dir);
        }

        static void CheckFile(string file)
        {
            List<string> lines = new List<string>();
            lines.AddRange(File.ReadAllLines(file));

            bool copyrigthExist = false;
            if (Copyrigth.Count <= lines.Count)
            {
                copyrigthExist = true;
                for (int i = 0; i < Copyrigth.Count; i++)
                {
                    if (Copyrigth[i].Trim().ToLower() != lines[i].Trim().ToLower())
                    {
                        copyrigthExist = false;
                        break;
                    }
                }
            }

            if (copyrigthExist)
                return;

            Log.WriteLine("Change copyrigth : " + file);

            // skip copyrigth
            int skip = 0;
            if (lines.Count>0)
            {
                if (lines[0].StartsWith(startcopyrigth, StringComparison.CurrentCultureIgnoreCase))
                {
                    for (int i = 1; i < lines.Count; i++)
                    {
                        if (!lines[i].StartsWith("//"))
                            break;
                        skip = i+1;
                    }
                }
            }

            // skip WhiteSpace
            for (int i = skip; i < lines.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                    break;
                skip = i+1;
            }

            lines.RemoveRange(0, skip);

            File.WriteAllLines(file, Copyrigth);
            File.AppendAllText(file, Environment.NewLine);
            File.AppendAllLines(file, lines);
        }
    }
}
