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
using System.Runtime.InteropServices;
using System.Management;
using System.Security.Principal;
using Microsoft.Win32;

namespace SolidCP.Server.Utils
{
	public sealed class OS
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct OSVERSIONINFO
		{
			public Int32 dwOSVersionInfoSize;
			public Int32 dwMajorVersion;
			public Int32 dwMinorVersion;
			public Int32 dwBuildNumber;
			public Int32 dwPlatformID;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct OSVERSIONINFOEX
		{
			public Int32 dwOSVersionInfoSize;
			public Int32 dwMajorVersion;
			public Int32 dwMinorVersion;
			public Int32 dwBuildNumber;
			public Int32 dwPlatformID;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;
			public short wServicePackMajor;
			public short wServicePackMinor;
			public short wSuiteMask;
			public byte wProductType;
			public byte wReserved;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct SYSTEM_INFO
		{
			public Int32 dwOemID;
			public Int32 dwPageSize;
			public Int32 wProcessorArchitecture;
			public Int32 lpMinimumApplicationAddress;
			public Int32 lpMaximumApplicationAddress;
			public Int32 dwActiveProcessorMask;
			public Int32 dwNumberOrfProcessors;
			public Int32 dwProcessorType;
			public Int32 dwAllocationGranularity;
			public Int32 dwReserved;
		}
		public enum WinSuiteMask : int
		{
			VER_SUITE_SMALLBUSINESS = 1,
			VER_SUITE_ENTERPRISE = 2,
			VER_SUITE_BACKOFFICE = 4,
			VER_SUITE_COMMUNICATIONS = 8,
			VER_SUITE_TERMINAL = 16,
			VER_SUITE_SMALLBUSINESS_RESTRICTED = 32,
			VER_SUITE_EMBEDDEDNT = 64,
			VER_SUITE_DATACENTER = 128,
			VER_SUITE_SINGLEUSERTS = 256,
			VER_SUITE_PERSONAL = 512,
			VER_SUITE_BLADE = 1024,
			VER_SUITE_STORAGE_SERVER = 8192,
			VER_SUITE_COMPUTE_SERVER = 16384
		}
		public enum WinPlatform : byte
		{
			VER_NT_WORKSTATION = 1,
			VER_NT_DOMAIN_CONTROLLER = 2,
			VER_NT_SERVER = 3
		}
		public enum OSMajorVersion : byte
		{
			VER_OS_NT4 = 4,
			VER_OS_2K_XP_2K3 = 5,
			VER_OS_VISTA_LONGHORN = 6
		}

		private const Int32 SM_SERVERR2 = 89;
		private const Int32 SM_MEDIACENTER = 87;
		private const Int32 SM_TABLETPC = 86;

		[DllImport("kernel32")]
		private static extern int GetSystemInfo(ref SYSTEM_INFO lpSystemInfo);
		[DllImport("user32")]
		private static extern int GetSystemMetrics(int nIndex);
		[DllImport("kernel32", EntryPoint = "GetVersion")]
		private static extern int GetVersionAdv(ref OSVERSIONINFO lpVersionInformation);
		[DllImport("kernel32")]
		private static extern int GetVersionEx(ref OSVERSIONINFOEX lpVersionInformation);

	
		/*public static string GetVersionEx()
		{
			OSVERSIONINFO osvi = new OSVERSIONINFO();
			OSVERSIONINFOEX xosvi = new OSVERSIONINFOEX();
			Int32 iRet = 0;
			string strDetails = string.Empty;
			osvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFO));
			xosvi.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
			try
			{
				iRet = (int)System.Environment.OSVersion.Platform;
				if (iRet == 1)
				{
					iRet = GetVersionAdv(ref osvi);
					strDetails = Environment.NewLine + "Version: " + osvi.dwMajorVersion + "." + osvi.dwMinorVersion + "." + osvi.dwBuildNumber + Environment.NewLine + osvi.szCSDVersion;
					if (Len(osvi) == 0)
					{
						return "Windows 95" + strDetails;
					}
					else if (Len(osvi) == 10)
					{
						return "Windows 98" + strDetails;
					}
					else if (Len(osvi) == 9)
					{
						return "Windows ME" + strDetails;
					}
				}
				else
				{
					iRet = GetVersionEx(xosvi);
					strDetails = Environment.NewLine + "Version: " + xosvi.dwMajorVersion + "." + xosvi.dwMinorVersion + "." + xosvi.dwBuildNumber + Environment.NewLine + xosvi.szCSDVersion + " (" + xosvi.wServicePackMajor + "." + xosvi.wServicePackMinor + ")";
					if (xosvi.dwMajorVersion == (byte)OSMajorVersion.VER_OS_NT4)
					{
						return "Windows NT 4" + strDetails;
					}
					else if (xosvi.dwMajorVersion == OSMajorVersion.VER_OS_2K_XP_2K3)
					{
						if (xosvi.dwMinorVersion == 0)
						{
							if (xosvi.wProductType == WinPlatform.VER_NT_WORKSTATION)
							{
								return "Windows 2000 Pro" + strDetails;
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_SERVER)
							{
								if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_DATACENTER) == WinSuiteMask.VER_SUITE_DATACENTER)
								{
									return "Windows 2000 Datacenter Server" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_ENTERPRISE) == WinSuiteMask.VER_SUITE_ENTERPRISE)
								{
									return "Windows 2000 Advanced Server" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_SMALLBUSINESS) == WinSuiteMask.VER_SUITE_SMALLBUSINESS)
								{
									return "Windows 2000 Small Business Server" + strDetails;
								}
								else
								{
									return "Windows 2000 Server" + strDetails;
								}
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_DOMAIN_CONTROLLER)
							{
								if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_DATACENTER) == WinSuiteMask.VER_SUITE_DATACENTER)
								{
									return "Windows 2000 Datacenter Server Domain Controller" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_ENTERPRISE) == WinSuiteMask.VER_SUITE_ENTERPRISE)
								{
									return "Windows 2000 Advanced Server Domain Controller" + strDetails;
								}
								else if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_SMALLBUSINESS) == WinSuiteMask.VER_SUITE_SMALLBUSINESS)
								{
									return "Windows 2000 Small Business Server Domain Controller" + strDetails;
								}
								else
								{
									return "Windows 2000 Server Domain Controller" + strDetails;
								}
							}
						}
						else if (xosvi.dwMinorVersion == 1)
						{
							if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_PERSONAL) == WinSuiteMask.VER_SUITE_PERSONAL)
							{
								return "Windows XP Home Edition" + strDetails;
							}
							else
							{
								return "Windows XP Professional Edition" + strDetails;
							}
						}
						else if (xosvi.dwMinorVersion == 2)
						{
							if (xosvi.wProductType == WinPlatform.VER_NT_WORKSTATION)
							{
								return "Windows XP Professional x64 Edition" + strDetails;
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_SERVER)
							{
								if (GetSystemMetrics(SM_SERVERR2) == 1)
								{
									return "Windows Server 2003 R2" + strDetails;
								}
								else
								{
									return "Windows Server 2003" + strDetails;
								}
							}
							else if (xosvi.wProductType == WinPlatform.VER_NT_DOMAIN_CONTROLLER)
							{
								if (GetSystemMetrics(SM_SERVERR2) == 1)
								{
									return "Windows Server 2003 R2 Domain Controller" + strDetails;
								}
								else
								{
									return "Windows Server 2003 Domain Controller" + strDetails;
								}
							}
						}
					}
					else if (xosvi.dwMajorVersion == OSMajorVersion.VER_OS_VISTA_LONGHORN)
					{
						if (xosvi.wProductType == WinPlatform.VER_NT_WORKSTATION)
						{
							if ((xosvi.wSuiteMask & WinSuiteMask.VER_SUITE_PERSONAL) == WinSuiteMask.VER_SUITE_PERSONAL)
							{
								return "Windows Vista (Home Premium, Home Basic, or Home Ultimate) Edition";
							}
							else
							{
								return "Windows Vista (Enterprize or Business)" + strDetails;
							}
						}
						else
						{
							return "Windows Server (Longhorn)" + strDetails;
						}
					}
				}
			}
			catch
			{
				MessageBox.Show(GetLastError.ToString);
				return string.Empty;
			}
		}*/

		public enum WindowsVersion
		{
			Unknown = 0,
			Windows95,
			Windows98,
			WindowsMe,
			WindowsNT351,
			WindowsNT4,
			Windows2000,
			WindowsXP,
			WindowsServer2003,
			Vista,
			WindowsServer2008,
            Windows7,
            WindowsServer2008R2,
            Windows8,
            WindowsServer2012,
            Windows81,
            WindowsServer2012R2,
            WindowsServer2016,
            Windows10,
            WindowsServer2019
        }

		/// <summary>
		/// Determine OS version
		/// </summary>
		/// <returns></returns>
		public static WindowsVersion GetVersion()
		{
			WindowsVersion ret = WindowsVersion.Unknown;

			OSVERSIONINFOEX info = new OSVERSIONINFOEX();
			info.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));
			GetVersionEx(ref info);

			// Get OperatingSystem information from the system namespace.
			System.OperatingSystem osInfo = System.Environment.OSVersion;

			// Determine the platform.
			switch (osInfo.Platform)
			{
				// Platform is Windows 95, Windows 98, Windows 98 Second Edition, or Windows Me.
				case System.PlatformID.Win32Windows:
					switch (osInfo.Version.Minor)
					{
						case 0:
							ret = WindowsVersion.Windows95;
							break;
						case 10:
							ret = WindowsVersion.Windows98;
							break;
						case 90:
							ret = WindowsVersion.WindowsMe;
							break;
					}
					break;

				// Platform is Windows NT 3.51, Windows NT 4.0, Windows 2000, or Windows XP.
				case System.PlatformID.Win32NT:
					switch (osInfo.Version.Major)
					{
						case 3:
							ret = WindowsVersion.WindowsNT351;
							break;
						case 4:
							ret = WindowsVersion.WindowsNT4;
							break;
						case 5:
							switch (osInfo.Version.Minor)
							{
								case 0:
									ret = WindowsVersion.Windows2000;
									break;
								case 1:
									ret = WindowsVersion.WindowsXP;
									break;
								case 2:
									int i = GetSystemMetrics(SM_SERVERR2);
									if (i != 0)
									{
										//Server 2003 R2
										ret = WindowsVersion.WindowsServer2003;
									}
									else
									{
										if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
										{
											//XP Pro x64
											ret = WindowsVersion.WindowsXP;
										}
										else
										{
											ret = WindowsVersion.WindowsServer2003;
										}
										break;
									}
									break;
							}
							break;
						case 6:
                            switch (osInfo.Version.Minor)
                            {
                                case 0:
                                    if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                        ret = WindowsVersion.Vista;
                                    else
                                        ret = WindowsVersion.WindowsServer2008;
                                    break;
                                case 1:
                                    if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                        ret = WindowsVersion.Windows7;
                                    else
                                        ret = WindowsVersion.WindowsServer2008R2;
                                    break;
                                case 2:
                                    if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                        ret = WindowsVersion.Windows8;
                                    else
                                        ret = WindowsVersion.WindowsServer2012;
                                    break;
                                case 3:
                                    if (info.wProductType == (byte)WinPlatform.VER_NT_WORKSTATION)
                                        ret = WindowsVersion.Windows81;
                                    else
                                        ret = WindowsVersion.WindowsServer2012R2;
                                    break;
                            }
                            break;
                        case 10:
                            int ReleaseId = GetReleaseId();
                            // Server 2016
                            if (ReleaseId == 1607 || ReleaseId == 1803 || ReleaseId == 1709 || ReleaseId == 1803) ret = WindowsVersion.WindowsServer2016;
                            // Windows 10 below 1903
                            else if (ReleaseId == 1507 || ReleaseId == 1511 || ReleaseId == 1607 || ReleaseId == 1703 || ReleaseId == 1709 || ReleaseId == 1803) ret = WindowsVersion.Windows10;
                            // Server 2019 and Windows 10 above 1903
                            else if (ReleaseId == 1809 || ReleaseId >= 1903) ret = WindowsVersion.WindowsServer2019;
                            break;
					}
					break;
			}
			return ret;
		}

        public static int GetReleaseId()
        {
            return Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "0"));
        }

		/// <summary>
		/// Returns Windows directory
		/// </summary>
		/// <returns></returns>
		public static string GetWindowsDirectory()
		{
			return Environment.GetEnvironmentVariable("windir");
		}
        /// <summary>
        /// Checks Whether the FSRM role services are installed
        /// </summary>
        /// <returns></returns>
        public static bool CheckFileServicesInstallation()
        {

            ManagementClass objMC = new ManagementClass("Win32_ServerFeature");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            // 01.09.2015 roland.breitschaft@x-company.de
            // Problem: Method not work on German Systems, because the searched Feature-Name does not exist
            // Fix: Add German String for FSRM-Feature            

            //foreach (ManagementObject objMO in objMOC)
            //    if (objMO.Properties["Name"].Value.ToString().ToLower().Contains("file server resource manager"))
            //        return true;
            foreach (ManagementObject objMO in objMOC)
            {
                var id = objMO.Properties["ID"].Value.ToString().ToLower();
                var name = objMO.Properties["Name"].Value.ToString().ToLower();
                if (id.Contains("72") || id.Contains("104"))
                    return true;
                else if (name.Contains("file server resource manager")
                    || name.Contains("ressourcen-manager für dateiserver"))
                    return true;
            }

            return false;
        }
    }
}

