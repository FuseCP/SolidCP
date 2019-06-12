SolidCP.LinuxVmConfig - Service for Linux-VM configuration (Hostname, IPs, root-password), uses KVP-Exchange.

Tested on Ubuntu-16.04-LTS, 18.04-LTS, 18.04.2-LTS (Live), CentOS-7

SolidCP.LinuxVmConfig must be installed from root user, KVP-Exchange must be enabled in Hyper-V (default enabled).

***Installing - Ubuntu:
1 - If not installed then:
		apt-get update
		apt-get install --install-recommends linux-virtual
		apt-get install linux-tools-virtual linux-cloud-tools-virtual
2 - Copy "Compilled\SCPLinux-x64" on your server.
3 - cd /{Your path}/SCPLinux-x64
4 - chmod 774 SolidCP.LinuxVmConfig
5 - ./SolidCP.LinuxVmConfig install
6 - "SolidCP.service successfully installed." message - Done

***Installing - CentOS:
1 - If not installed then:
		yum install -y hyperv-daemons
2 - Copy "Compilled\SCPLinux-x64" on your server.
3 - cd /{Your path}/SCPLinux-x64
4 - chmod 774 SolidCP.LinuxVmConfig
5 - ./SolidCP.LinuxVmConfig install
6 - "SolidCP.service successfully installed." message - Done



***Compiling:
1 - Install NetCore SDK on Your server:
		Manual: https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-current
2 - Copy source folder on Your server.
3 - cd /{Your path}/SolidCP.LinuxVmConfig/SolidCP.LinuxVmConfig
4 - dotnet publish -c Release --self-contained -r linux-x64