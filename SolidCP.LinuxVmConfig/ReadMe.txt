SolidCP.LinuxVmConfig - Service for Linux-VM configuration (Hostname, IPs, root-password), uses KVP-Exchange.

Tested on Ubuntu-16.04-LTS, 18.04-LTS, 18.04.2-LTS (Live)

Installing:

1 - SolidCP.LinuxVmConfig must be installed from root user, KVP-Exchange must be enabled in Hyper-V (default enabled).
2 - If not installed then:
		apt-get update
		apt-get install --install-recommends linux-virtual
		apt-get install linux-tools-virtual linux-cloud-tools-virtual
3 - Copy "Compilled\ubuntu" on your server.
4 - cd /{Your path}/ubuntu
5 - chmod 774 SolidCP.LinuxVmConfig
6 - ./SolidCP.LinuxVmConfig install
7 - "SolidCP.service successfully installed." message - Done



Compiling:

1 - Install NetCore SDK on Your server:
		Manual: https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-current
2 - Copy source folder on Your server.
3 - cd /{Your path}/SolidCP.LinuxVmConfig/SolidCP.LinuxVmConfig
4 - dotnet publish -c Release --self-contained -r ubuntu.18.04-x64