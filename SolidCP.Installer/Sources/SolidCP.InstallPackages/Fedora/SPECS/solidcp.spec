Name: solidcp
Version:1.5.0
Release: 1%{?dist}
Summary: This is the SolidCP Server component
License: Creative Commons Share-alike    
URL: https://www.solidcp.com
Requires: /bin/sh, sed, mono-complete >= 5.0.1
AutoReqProv: no
BuildArch: noarch
Source0: %{name}-%{version}.tar.gz

%description
SolidCP is a complete management portal for Cloud Computing Companies
and IT Providers to automate the provisioning of a full suite of Multi-Tenant
services on servers. The powerful, flexible and fully open source SolidCP platform
gives users simple point-and-click control over Server applications including IIS 10,
Microsoft SQL Server 2022, MySQL, MariaDB, Active Directory, Microsoft Exchange 2019,
Microsoft Sharepoint 2019, Microsoft RemoteApp/RDS, Hyper-v and Proxmox Deployments.

%prep
%autosetup

%install
rm -rf $RPM_BUILD_ROOT
mkdir -p $RPM_BUILD_ROOT%{_bindir}
mkdir -p $RPM_BUILD_ROOT/usr/share
cp -rp usr/bin/* $RPM_BUILD_ROOT%{_bindir}
cp -rp usr/share/* $RPM_BUILD_ROOT/usr/share

%post
if [ $1 -ge 1 ];then
    sed -i 's|/usr/bin/solidcp-universalinstaller|%{_bindir}/solidcp-universalinstaller|g' /usr/share/applications/solidcp-universalinstaller.desktop
#  %{_bindir}/solidcp-universalinstaller
    echo "Please type 'sudo solidcp-universalinstaller' to install SolidCP"
fi

%clean
rm -rf $RPM_BUILD_ROOT

%files
/usr/bin/solidcp-universalinstaller
/usr/share/solidcp/SolidCP.UniversalInstaller.exe
/usr/share/pixmaps/SolidCP.png
/usr/share/applications/solidcp-universalinstaller.desktop
/usr/share/doc/solidcp/copyright
/usr/share/doc/solidcp/ChangeLog
/usr/share/doc/solidcp/README
/usr/share/man/man1/solidcp-universalinstaller.1.gz

%changelog
* Fri Mar 22 2024 Simon Egli <simon.jakob.egli@gmail.com> - 1.5.0
- First version being packaged
