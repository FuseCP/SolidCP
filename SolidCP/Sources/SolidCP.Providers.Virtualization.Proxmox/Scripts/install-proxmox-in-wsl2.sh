#!/bin/bash

# This script installs Proxmox in WSL2 on Debian 12 Bookworm
# Inspired by https://github.com/gaudat/gauup/blob/master/homelab/pve-wsl2.md

# Setup Systemd
if [ ! -f ".wslconfmodified"]
then
echo "Setup Systemd"
touch .wslconfmodified
# Setup WSL
echo "[boot]
systemd=true
[network]
generateHosts = false" >> /etc/wsl.conf

echo "Modified /etc/wsl.conf:"
cat /etc/wsl.conf

echo -e "/etc/wsl.conf has been updated to support Systemd and a custom /etc/hosts file.
If the script got your wsl.conf wrong, please correct it by hand.
Exit your WSL shell and back in Windows execute wsl --shutdown to restart wsl.
Then go back to WSL and execute this script again."
exit
fi

# Set root password. Need this to log in PVE web UI.
echo "Set a root password. Need this to log in PVE web UI."
sudo passwd

# Confirm which Debian version it is (This script only supports Debian 12 Bookworm)
echo "Confirm which Debian version it is (This script only supports Debian 12 Bookworm)"
cat /etc/os-release

# Install prerequiste packages
echo "Install prerequiste packages"
sudo apt -y install apt-transport-https python3-requests lsb-release

# https://pve.proxmox.com/wiki/Install_Proxmox_VE_on_Debian_12_Bookworm

sudo wget https://enterprise.proxmox.com/debian/proxmox-release-bookworm.gpg -O /etc/apt/trusted.gpg.d/proxmox-release-bookworm.gpg 

sudo apt update

# Install Proxmox
sudo apt -y install proxmox-ve

sudo apt remove os-prober

# Setup set ip in /etc/hosts service
echo "Setup system service to update /etc/hosts for Proxmox"

echo -e "#!/bin/bash

ip4addr=`ip -4 a | awk '/inet .* scope global/ {split($2,out,"/");print out[1]}'`

awk "/^127\.0\.0\.1.*`hostname`/ {next} /`hostname`/ {\$1 = \"$ip4addr\"} // {print}" /etc/hosts > /etc/hosts.new && mv /etc/hosts.new /etc/hosts" > /usr/local/bin/pvepreup.sh

echo -e "[Unit]
Description=Fix /etc/hosts before starting PVE
Before=pve-cluster.service
After=network.target
DefaultDependencies=no
Before=shutdown.target
Conflicts=shutdown.target

[Service]
ExecStart=/usr/local/bin/pvepreup.sh
KillMode=mixed
TimeoutStopSec=10
Type=oneshot

[Install]
WantedBy=multi-user.target" > /etc/systemd/system/pvepreup.service

chmod +x /usr/local/bin/pvepreup.sh
systemctl daemon-reload
systemctl enable pvepreup
systemctl start pvepreup
systemctl status pvepreup

echo "Opening Proxmox Web GUI"
xdg-open https://localhost:8006
