SolidCP.LinuxVmConfig - Service for Linux and FreeBSD VM configuration (Hostname, IPs, root-password), uses KVP-Exchange.

Tested on: Ubuntu-16.04, 18.04 | CentOS-7 | FreeBSD-12,11.2 | PfSense 2.4.4

SolidCP.LinuxVmConfig must be installed from root user, KVP-Exchange must be enabled in Hyper-V (default enabled), Hyper-V kvp-daemon muss be running on the guest system.

***Install***

1 - Copy "Compiled\{Your OS}\VmConfig" on Your server.
2 - cd /{Your path}/VmConfig
3 - chmod +x install.sh
4 - ./install.sh
5 - "solidcp service successfully installed." message - Done


***Compiling***

1 - install "gcc" version 7 or above on Your server
2 - cd /{Your path}/SolidCP.LinuxVmConfig/SolidCP.LinuxVmConfig
3 - g++ -std=c++11 -pthread *.cpp -o SolidCP.LinuxVmConfig