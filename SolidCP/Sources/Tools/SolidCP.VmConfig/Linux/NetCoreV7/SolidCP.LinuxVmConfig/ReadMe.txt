SolidCP.LinuxVmConfig - Service for Linux-VM configuration (Hostname, IPs, root-password), uses KVP-Exchange.

Tested on Ubuntu-16.04-LTS, 18.04-LTS, 18.04.2-LTS (Live), CentOS-7, FreeBSD-12

SolidCP.LinuxVmConfig must be installed from root user, KVP-Exchange must be enabled in Hyper-V (default enabled), Hyper-V kvp-daemon muss be running on the guest system.

***Installing - Ubuntu:
1 - If not installed then:
		apt-get update
		apt-get install --install-recommends linux-virtual
		apt-get install linux-tools-virtual linux-cloud-tools-virtual
2 - Copy "Compiled\SCPLinux-x64" on your server.
3 - cd /{Your path}/SCPLinux-x64
4 - chmod 774 SolidCP.LinuxVmConfig
5 - ./SolidCP.LinuxVmConfig install
6 - "SolidCP.service successfully installed." message - Done

***Installing - CentOS:
1 - Copy "Compiled\SCPLinux-x64" on your server.
2 - cd /{Your path}/SCPLinux-x64
3 - chmod 774 SolidCP.LinuxVmConfig
4 - ./SolidCP.LinuxVmConfig install
5 - "SolidCP.service successfully installed." message - Done

***Installing - FreeBSD:
1 - kldload linux64
2 - pkg install linux_base-c7 linux-c7-openssl-libs
3 - add to "/etc/rc.conf"
	linux_enable="YES"
4 - add following entries to "/etc/fstab"
	linprocfs   /compat/linux/proc  linprocfs   rw  0   0
5 - reboot
6 - Copy "Compiled\SCPLinux-x64" on your server.
7 - cd /{Your path}/SCPLinux-x64
8 - chmod 774 SolidCP.LinuxVmConfig
9 - chmod 774 sh
10 - ./SolidCP.LinuxVmConfig install
11 - "solidcp service successfully installed." message - Done


***Compiling:
1 - Install NetCore SDK on Your server:
		Manual: https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-current
2 - Copy source folder on Your server.
3 - cd /{Your path}/SolidCP.LinuxVmConfig/SolidCP.LinuxVmConfig
4 - dotnet publish -c Release --self-contained -r linux-x64

-------------------------------

sudo mkdir /etc/solidcp
sudo mkdir /etc/solidcp/SCPLinux-x64

sudo apt update
sudo apt upgrade

sudo apt update
sudo apt-get install dotnet-runtime-7.0 net-tools linux-image-virtual linux-tools-virtual linux-cloud-tools-virtual

sudo nano /etc/default/grub

[Update grub file as follows]
GRUB_CMDLINE_LINUX_DEFAULT="elevator=noop"

sudo update-grub

sudo apt update
sudo apt upgrade

sudo mkdir /dev/vmbus/hv_fcopy
sudo systemctl restart hv-fcopy-daemon.service

[Verify the following services are running properly]
systemctl status hv-kvp-daemon.service
systemctl status hv-fcopy-daemon.service
systemctl status hv-vss-daemon.service

sudo chmod 777 /etc/solidcp/SCPLinux-x64

[Copy the LinuxVMConfig files to the /etc/solidcp/SCPLinux-x64 folder using SFTP or some other means]

sudo chmod 755 -R /etc/solidcp/SCPLinux-x64
sudo chmod 774 /etc/solidcp/SCPLinux-x64/SolidCP.LinuxVmConfig

sudo su
cd /etc/solidcp/SCPLinux-x64
./SolidCP.LinuxVmConfig install

[Verify the solidcp service is running properly]
systemctl status solidcp

exit
logout

