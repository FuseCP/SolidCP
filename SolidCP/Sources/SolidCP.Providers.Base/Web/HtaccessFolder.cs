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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using SolidCP.Providers.Common;
using System.Linq;

namespace SolidCP.Providers.Web
{
	public class HtaccessFolder : IComparable
	{
		#region Constants

		public const string HTTPD_CONF_FILE = "httpd.conf";
		public const string HTACCESS_FILE = ".htaccess";
		public const string HTPASSWDS_FILE = ".htpasswds";
		public const string HTGROUPS_FILE = ".htgroups";


		public const string AUTH_NAME_DIRECTIVE = "AuthName";

		public const string AUTH_TYPE_DIRECTIVE = "AuthType";
		public const string AUTH_TYPE_BASIC = "Basic";
		public const string AUTH_TYPE_DIGEST = "Digest";
		public const string DEFAULT_AUTH_TYPE = AUTH_TYPE_BASIC;

		public static readonly string[] PASSWORD_ENCODING_TYPES = new string[]
        {
            "Apache MD5",
            "Unix Crypt",
            "SHA1"
        };

		public static readonly string[] AUTH_TYPES = new string[]
        {
            AUTH_TYPE_BASIC,
            AUTH_TYPE_DIGEST
        };

		public const string REQUIRE_DIRECTIVE = "Require";
		public const string AUTH_BASIC_PROVIDER_FILE = "AuthBasicProvider file";
		public const string AUTH_DIGEST_PROVIDER_FILE = "AuthDigestProvider file";
		public const string VALID_USER = "valid-user";
		public const string REQUIRE_USER = "user";
		public const string REQUIRE_GROUP = "group";
		public const string AUTH_USER_FILE_DIRECTIVE = "AuthUserFile";
		public const string AUTH_GROUP_FILE_DIRECTIVE = "AuthGroupFile";

		protected static string LinesSeparator = System.Environment.NewLine;
		#endregion

		#region parsing regexps
		protected static readonly Regex RE_AUTH_NAME = new Regex("^\\s*AuthName\\s+\"?([^\"]+)\"?\\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		protected static readonly Regex RE_AUTH_TYPE = new Regex(@"^\s*AuthType\s+(basic|digest)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		protected static readonly Regex RE_REQUIRE = new Regex(@"^\s*Require\s+(.+)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		protected static readonly Regex RE_AUTH_PROVIDER = new Regex(@"^\s*Auth(Basic|Digest)Provider\s+(file)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		protected static readonly Regex RE_AUTH_USER_FILE = new Regex(@"^\s*AuthUserFile\s+(.+)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
		protected static readonly Regex RE_AUTH_GROUP_FILE = new Regex(@"^\s*AuthGroupFile\s+(.+)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

		public static readonly Regex RE_DIGEST_PASSWORD = new Regex(@"^[^:]*:[0-9a-f]{32}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		#endregion


		#region private fields
		private string siteRootPath;
		private string path;
		private string contentPath;
		private string htaccessContent;
		private string authName;
		private string authType = DEFAULT_AUTH_TYPE;
		private List<string> users;
		private List<string> groups;
		private bool validUser = false;
		private string authUserFile;
		private string authGroupFile;

		private bool doAuthUpdate = false;

		private string filename = HTACCESS_FILE;
		#endregion

		#region public properties
		public string Path
		{
			get { return path; }
			set
			{
				path = value;
				if (path.EndsWith("\\") && !path.Equals("\\"))
				{
					path = path.Substring(0, path.Length - 1);
				}
			}
		}

		public string ContentPath
		{
			get { return contentPath; }
			set { contentPath = value; }
		}

		public string HtaccessContent
		{
			get { return htaccessContent; }
			set { htaccessContent = value; }
		}

		public string AuthName
		{
			get { return authName; }
			set { authName = value; }
		}

		public string AuthType
		{
			get { return authType; }
			set { authType = value; }
		}

		public List<string> Users
		{
			get { return users; }
			set { users = value; }
		}

		public List<string> Groups
		{
			get { return groups; }
			set { groups = value; }
		}

		public bool ValidUser
		{
			get { return validUser; }
			set { validUser = value; }
		}

		public bool DoAuthUpdate
		{
			get { return doAuthUpdate; }
			set { doAuthUpdate = value; }
		}

		public string AuthUserFile
		{
			get { return authUserFile; }
			set { authUserFile = value; }
		}

		public string AuthGroupFile
		{
			get { return authGroupFile; }
			set { authGroupFile = value; }
		}

		public string SiteRootPath
		{
			get { return siteRootPath; }
			set { siteRootPath = value; }
		}

		#endregion

		public HtaccessFolder()
		{
			Users = new List<string>();
			Groups = new List<string>();
		}

		public HtaccessFolder(string siteRootPath, string path, string contentPath)
			: this()
		{
			this.SiteRootPath = siteRootPath;
			this.Path = path;
			this.ContentPath = contentPath;

			ReadHtaccess();
		}

		private void ReadHttpdConf()
		{
			filename = HTTPD_CONF_FILE;
			string htpath = System.IO.Path.Combine(ContentPath, filename);
			HtaccessContent = ReadFile(htpath);
		}

		public void ReadHtaccess()
		{
			filename = HTACCESS_FILE;
			string htpath = System.IO.Path.Combine(ContentPath, filename);
			HtaccessContent = ReadFile(htpath);

			ParseHtaccess();
		}

		private void ParseHtaccess()
		{
			validUser = false;


			Match mAuthName = RE_AUTH_NAME.Match(HtaccessContent);
			if (mAuthName.Success)
			{
				AuthName = mAuthName.Groups[1].Value;
			}

			Match mAuthType = RE_AUTH_TYPE.Match(HtaccessContent);
			if (mAuthType.Success)
			{
				AuthType = mAuthType.Groups[1].Value;
			}

			Match mRequire = RE_REQUIRE.Match(HtaccessContent);
			if (mRequire.Success)
			{
				ParseRequirements(mRequire.Groups[1].Value);
			}

			Match mAuthUserFile = RE_AUTH_USER_FILE.Match(HtaccessContent);
			if (mAuthUserFile.Success)
			{
				AuthUserFile = mAuthUserFile.Groups[1].Value;
			}
			else
			{
				string authUserFilePath = System.IO.Path.Combine(SiteRootPath, HTPASSWDS_FILE);
				if (File.Exists(authUserFilePath))
				{
					AuthUserFile = authUserFilePath;
				}
			}

			Match mAuthGroupFile = RE_AUTH_GROUP_FILE.Match(HtaccessContent);
			if (mAuthGroupFile.Success)
			{
				AuthGroupFile = mAuthGroupFile.Groups[1].Value;
			}
			else
			{
				string authGroupFilePath = System.IO.Path.Combine(SiteRootPath, HTGROUPS_FILE);
				if (File.Exists(authGroupFilePath))
				{
					AuthGroupFile = authGroupFilePath;
				}
			}
		}

		private void ParseRequirements(string requirementsLine)
		{
			bool acceptUsers = false, acceptGroups = false;
			foreach (string requirement in requirementsLine.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries))
			{
				string req = requirement.Trim();
				if (req.Equals(VALID_USER))
				{
					ValidUser = true;
					acceptUsers = acceptGroups = false;
				}
				else if (req.Equals(REQUIRE_USER))
				{
					acceptUsers = true;
					acceptGroups = false;
				}
				else if (req.Equals(REQUIRE_GROUP))
				{
					acceptUsers = false;
					acceptGroups = true;
				}
				else
				{
					if (acceptUsers)
					{
						Users.Add(req);
					}
					else if (acceptGroups)
					{
						Groups.Add(req);
					}
				}
			}
		}

		public void Update()
		{
			if (Directory.Exists(ContentPath))
			{
				if (doAuthUpdate)
				{
					UpdateAuthDirectives();
				}

				string htaccessPath = System.IO.Path.Combine(ContentPath, filename);
				WriteFile(htaccessPath, HtaccessContent);
			}
		}

		private void UpdateAuthDirectives()
		{
			if (Users.Contains(VALID_USER))
			{
				ValidUser = true;
				Users.Remove(VALID_USER);
			}
			else
			{
				ValidUser = false;
			}


			// update AuthName
			Match mAuthName = RE_AUTH_NAME.Match(HtaccessContent);
			if (!string.IsNullOrEmpty(AuthName))
			{
				string s = string.Format("{0} \"{1}\"", AUTH_NAME_DIRECTIVE, AuthName);
				if (mAuthName.Success)
				{
					HtaccessContent = RE_AUTH_NAME.Replace(HtaccessContent, s);
				}
				else
				{
					HtaccessContent += s + Environment.NewLine;
				}
			}
			else
			{
				if (mAuthName.Success)
				{
					HtaccessContent = RE_AUTH_NAME.Replace(HtaccessContent, "");
				}
			}


			// update AuthType
			Match mAuthType = RE_AUTH_TYPE.Match(HtaccessContent);
			if (!string.IsNullOrEmpty(AuthType))
			{
				string s = string.Format("{0} {1}", AUTH_TYPE_DIRECTIVE, AuthType);
				if (mAuthType.Success)
				{
					HtaccessContent = RE_AUTH_TYPE.Replace(HtaccessContent, s);
				}
				else
				{
					HtaccessContent += s + Environment.NewLine;
				}
			}
			else
			{
				if (mAuthType.Success)
				{
					HtaccessContent = RE_AUTH_TYPE.Replace(HtaccessContent, "");
				}
			}

			// update Auth(Basic|Digest)Provider
			Match mAuthProvider = RE_AUTH_PROVIDER.Match(HtaccessContent);
			string prov = AuthType == "Basic" ? AUTH_BASIC_PROVIDER_FILE : AUTH_DIGEST_PROVIDER_FILE;
			if (mAuthProvider.Success)
			{
				HtaccessContent = RE_AUTH_PROVIDER.Replace(HtaccessContent, prov);
			}
			else
			{
				HtaccessContent += prov + Environment.NewLine;
			}

			// update AuthUserFile
			Match mAuthUserFile = RE_AUTH_USER_FILE.Match(HtaccessContent);
			if (!string.IsNullOrEmpty(AuthUserFile))
			{
				string s = string.Format("{0} {1}", AUTH_USER_FILE_DIRECTIVE, AuthUserFile);
				if (mAuthUserFile.Success)
				{
					HtaccessContent = RE_AUTH_USER_FILE.Replace(HtaccessContent, s);
				}
				else
				{
					HtaccessContent += s + Environment.NewLine;
				}
			}
			else
			{
				if (mAuthUserFile.Success)
				{
					HtaccessContent = RE_AUTH_USER_FILE.Replace(HtaccessContent, "");
				}
			}

			// update AuthUserFile
			Match mAuthGroupFile = RE_AUTH_GROUP_FILE.Match(HtaccessContent);
			if (!string.IsNullOrEmpty(AuthGroupFile))
			{
				string s = string.Format("{0} {1}", AUTH_GROUP_FILE_DIRECTIVE, AuthGroupFile);
				if (mAuthGroupFile.Success)
				{
					HtaccessContent = RE_AUTH_GROUP_FILE.Replace(HtaccessContent, s);
				}
				else
				{
					HtaccessContent += s + Environment.NewLine;
				}
			}
			else
			{
				if (mAuthGroupFile.Success)
				{
					HtaccessContent = RE_AUTH_GROUP_FILE.Replace(HtaccessContent, "");
				}
			}

			// update Require
			Match mRequire = RE_REQUIRE.Match(HtaccessContent);
			if (ValidUser || Users.Count > 0 || Groups.Count > 0)
			{
				string s = GenerateReqiure();
				if (mRequire.Success)
				{
					HtaccessContent = RE_REQUIRE.Replace(HtaccessContent, s);
				}
				else
				{
					HtaccessContent += s + Environment.NewLine;
				}
			}
			else
			{
				if (mRequire.Success)
				{
					HtaccessContent = RE_AUTH_TYPE.Replace(HtaccessContent, "");
				}
			}
		}

		private string GenerateReqiure()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(REQUIRE_DIRECTIVE);

			if (ValidUser)
			{
				sb.Append(" ").Append(VALID_USER);
			}

			if (Users.Count > 0)
			{
				sb.Append(" ").Append(REQUIRE_USER);
				foreach (string user in Users)
				{
					sb.AppendFormat(" {0}", user);
				}
			}

			if (Groups.Count > 0)
			{
				sb.Append(" ").Append(REQUIRE_GROUP);
				foreach (string group in Groups)
				{
					sb.AppendFormat(" {0}", group);
				}
			}

			return sb.ToString();
		}

		#region Implementation of IComparable
		public int CompareTo(object obj)
		{
			HtaccessFolder folder = obj as HtaccessFolder;
			if (folder != null)
			{
				return String.CompareOrdinal(this.Path.ToLower(), folder.Path.ToLower());
			}

			return 0;
		}

		#endregion

		#region static helper members

		public static void GetDirectoriesWithHtaccess(string siteRootPath, string virtualRoot, string rootPath, string subPath, List<HtaccessFolder> folders)
		{
			if (Directory.Exists(subPath))
			{
				if (HasHtaccessInDirectory(subPath))
				{
					// Check this subPath is in folders list already
					bool included = folders.Any(x => String.Equals(subPath, x.ContentPath, StringComparison.OrdinalIgnoreCase));
					// Add only if it is not included
					if (included == false)
					{
						//
						folders.Add(new HtaccessFolder
						{
							SiteRootPath = siteRootPath,
							Path = virtualRoot + RelativePath(rootPath, subPath),
							ContentPath = subPath
						});
					}
				}
				// Process nested virtual directories if any
				Array.ForEach(Directory.GetDirectories(subPath), (x) =>
				{
					GetDirectoriesWithHtaccess(siteRootPath, virtualRoot, rootPath, x, folders);
				});
			}
		}

		private static bool HasHtaccessInDirectory(string path)
		{
			return File.Exists(System.IO.Path.Combine(path, HTACCESS_FILE));
		}

		private static string RelativePath(string basePath, string subPath)
		{
			if (string.Equals(basePath, subPath, StringComparison.OrdinalIgnoreCase))
			{
				return "\\";
			}

			if (subPath.StartsWith(subPath, StringComparison.OrdinalIgnoreCase))
			{
				return subPath.Substring(basePath.Length);
			}

			throw new ArgumentException("Paths do not have a common base");
		}

		public static HtaccessFolder CreateHtaccessFolder(string siteRootPath, string folderPath)
		{
			HtaccessFolder folder = new HtaccessFolder
			{
				SiteRootPath = siteRootPath,
				Path = folderPath,
				ContentPath = System.IO.Path.Combine(siteRootPath, folderPath)
			};

			folder.ReadHtaccess();

			return folder;
		}

		public static HtaccessFolder CreateHttpdConfFolder(string path)
		{
			HtaccessFolder folder = new HtaccessFolder
			{
				ContentPath = path,
				Path = HTTPD_CONF_FILE
			};

			folder.ReadHttpdConf();

			return folder;

		}

		public static string ReadFile(string path)
		{
			string result = string.Empty;

			if (!File.Exists(path))
				return result;

			using (StreamReader reader = new StreamReader(path))
			{
				result = reader.ReadToEnd();
				reader.Close();
			}

			return result;
		}

		public static List<string> ReadLinesFile(string path)
		{
			List<string> lines = new List<string>();

			string content = ReadFile(path);

			if (!string.IsNullOrEmpty(content))
			{
				foreach (string line in Regex.Split(content, LinesSeparator))
				{
					lines.Add(line);
				}
			}

			return lines;
		}

		public static void WriteFile(string path, string content)
		{
			//	remove 'hidden' attribute
			FileAttributes fileAttributes = FileAttributes.Normal;
			if (File.Exists(path))
			{
				fileAttributes = File.GetAttributes(path);
				if ((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
				{
					fileAttributes &= ~FileAttributes.Hidden;
					File.SetAttributes(path, fileAttributes);
				}
			}

			// check if folder exists
			string folder = System.IO.Path.GetDirectoryName(path);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			// write file
			using (StreamWriter writer = new StreamWriter(path))
			{
				writer.WriteLine(content);
				writer.Close();
			}

			// set 'hidden' attribute
			fileAttributes = File.GetAttributes(path);
			fileAttributes |= FileAttributes.Hidden;
			File.SetAttributes(path, fileAttributes);

		}

		public static void WriteLinesFile(string path, List<string> lines)
		{
			StringBuilder content = new StringBuilder();
			foreach (string line in lines)
			{
				if (!string.IsNullOrEmpty(line))
				{
					content.AppendLine(line);
				}
			}

			WriteFile(path, content.ToString());
		}

		#endregion
	}

	public class HtaccessUser : WebUser
	{
		public const string ENCODING_TYPE_APACHE_MD5 = "Apache MD5";
		public const string ENCODING_TYPE_UNIX_CRYPT = "Unix Crypt";
		public const string ENCODING_TYPE_SHA1 = "SHA1";

		public static readonly string[] ENCODING_TYPES = new string[]
        {
            ENCODING_TYPE_APACHE_MD5, ENCODING_TYPE_UNIX_CRYPT, ENCODING_TYPE_SHA1
        };

		private string authType;
		private string encType;
		private string realm;

		public string AuthType
		{
			get
			{
				if (string.IsNullOrEmpty(authType))
				{
					if (!string.IsNullOrEmpty(Password))
					{
						authType = HtaccessFolder.RE_DIGEST_PASSWORD.IsMatch(Password) ? HtaccessFolder.AUTH_TYPE_DIGEST : HtaccessFolder.AUTH_TYPE_BASIC;
					}
				}

				return authType;
			}
			set { authType = value; }
		}

		public string EncType
		{
			get
			{
				if (string.IsNullOrEmpty(encType))
				{
					if (HtaccessFolder.AUTH_TYPE_BASIC != AuthType)
					{
						encType = string.Empty;
					}
					else if (Password.StartsWith("{SHA"))
					{
						encType = ENCODING_TYPE_SHA1;
					}
					else if (Password.StartsWith("$apr1$"))
					{
						encType = ENCODING_TYPE_APACHE_MD5;
					}
					else
					{
						encType = ENCODING_TYPE_UNIX_CRYPT;
					}
				}

				return encType;
			}
			set { encType = value; }
		}

		public string Realm
		{
			get { return realm; }
			set { realm = value; }
		}
	}
}
