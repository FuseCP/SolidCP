#!/bin/bash

# Example Proxmox Deploy Script

# First prepare a Template VM you want to base clones on.
# Then set this script in SolidCP Proxmox Virtualization Server Settings VM Deploy Script like this (replace ./ with the actual path to the file):
#
# ./deploy-vm.sh
#
# and as VM Deploy Script Parameters:
#
# [FQDN] [ADMINPASS] [IP] [GATEWAY] [NETMASK]
#
# In the OS Templates, specify as Additional Deploy Script Parameters:
#
# <Guest OS flavor (Windows or Linux)> <ID of the template VM> <Administartor Username (root for Linux guests, Adminstrator for Windows guests)>
#
# The template VM will then be cloned via the qm clone command.
#
# You might want to cusomize the script to configure the network inside the
# guest with the 'qm guest exec' command

osflavor=$1
templateVmId=$2
adminUsername=$3
fullyQualifiedDomainName=$4
adminPassword=$5
ip=$6
gateway=$7
netmask=$8

hostname=`echo "$fullyQuanlifiedDomainName" | sed 's/\..*//'^` 

echo "Deploy Proxmox VPS"
echo ""
echo "Usage:
./deploy-vm.sh <Guest OS flavor ('Windows' or 'Linux')> <Template VM ID> <Adminstrator username> <Fully qualified domain name> <Administrator password> <IP address> <Gateway> <Netmask>" 
echo ""
echo "OS Template Name: $templateName"
echo "OS Template VM ID: $templateVmId"
echo "Administrator Username: $adminUsername"
echo "Administrator Password $adminPassword"
echo "Domain: $fullyQualifiedDomainName"
echo "Hostname: $hostname"
echp "IP: $ip"
echo "Gateway: $gateway"
echo "Netmask: $netmask"

# Get next free VM ID
newVmId="$(pvesh get /cluster/nextid)"
echo "New VM Name: $hostname"
echo "New VM ID: $newVmId"

# Clone the template VM
echo "> qm clone $templateVmId $newId --name $hostname"
# qm clone $templateVmId $newId --name "$hostname"

# Start the new VM
echo "> qm start"
# qm start

# Change administrator password of the new VM
echo "> echo -e \"$adminPassword\\n$adminPassword\\n\" | qm guest passwd $newVmId $adminUsername"
# echo -e "$adminPassword\n$adminPassword\n" | qm guest passwd $newVmId $adminUsername

# Change nework configuration of the new guest VM

# If guest is Windows
if [ $osflavor = 'Windows' ]
then

# set hostname
# qm guest exe $newVmId powershell Rename-Computer -NewName $hostname

# ...

fi

# If guest is Linux
if [ $osflavor = 'Linux' ]
then

# set hostname
#   qm guest exec hostnamectl set-hostname $hostname

# ...

fi