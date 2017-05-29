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
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

namespace SEPlugin
{
    [Serializable]
    public class Config
    {
        public bool WriteLog = true;
        public bool ExtendedLog = true;

        public string schema = ConfigurationManager.AppSettings["SpamExpertsSchema"];
        public string url = ConfigurationManager.AppSettings["SpamExpertsUrl"];
        public string user = ConfigurationManager.AppSettings["SpamExpertsUser"];
        public string password = ConfigurationManager.AppSettings["SpamExpertsPassword"];

        public string MailServer = "";
        public string MailTo = "";
        public string MailFrom = "";

        public string ErrorMailBody = "{0}. Error: {1}";
        public string ErrorMailSubject = "SEPlugin error";

        // default 

        public static string SettingsFileName
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + @"SEPlugin.config";
            }
        }


        public static object Deserialize(Config s)
        {
            XmlSerializer myXmlSer = null;
            FileStream fs = null;

            try
            {
                myXmlSer = new XmlSerializer(s.GetType());
                fs = new FileStream(SettingsFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception exc)
            {
                return s;
            }

            try
            {
                return (object)myXmlSer.Deserialize(fs);
            }
            catch (Exception exc)
            {
                return s;
            }
            finally
            {
                fs.Close();
            }
        }

        public bool Serialize()
        {
            XmlSerializer myXmlSer = null;
            FileStream fs = null;

            try
            {
                myXmlSer = new XmlSerializer(GetType());
                fs = new FileStream(SettingsFileName, FileMode.Create, FileAccess.Write, FileShare.Read);

            }
            catch
            {
                return false;
            }

            try
            {
                myXmlSer.Serialize(fs, this);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                fs.Close();
            }
        }

        private static Config instance = null;
        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    Load();
                    //if (!File.Exists(SettingsFileName)) 
                    Save();
                }
                return instance;
            }
        }


        public static void Load()
        {
            instance = (Config)Deserialize(new Config());
        }

        public static void Save()
        {
            if (instance == null) return;
            instance.Serialize();
        }


    }
}
