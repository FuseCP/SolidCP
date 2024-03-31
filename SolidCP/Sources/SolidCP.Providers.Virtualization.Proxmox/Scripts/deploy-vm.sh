#!/bin/bash
#
# Example Proxmox Deploy Script
# (Version 1.0.0)
#
# First prepare a Template VM or TemplateFile (When using LXC Container) you want to base clones on.
# Then set this script in SolidCP Proxmox Virtualization Server Settings VM Deploy Script like this (replace ./ with the actual path to the file):
#
# ./deploy-vm.sh
#
# and as VM Deploy Script Parameters:
#
# [FQDN] [OSTEMPLATE] [OSTEMPLATEFILE] [ADMINPASS] [IP] [GATEWY] [NETMASK]
#
# In the OS Templates, specify as Additional Deploy Script Parameters:
#
# <Guest OS flavor (Windows or Linux or Container)> <ID of the template VM> <Administartor Username (root for Linux guests, Adminstrator for Windows guests)>
#
# When OS flavor is set to Container, specify the path to the distro tar.gz file in the OS Template File of the OS Template. 
# (For example 'local:vztmpl/debian-10.0-standard_10.0-1_amd64.tar.gz' for a Debian distro on the local storage)
#
# The template VM will then be cloned via the qm clone, the pct clone or the pct create command.
#
# You might want to cusomize the script to configure the network inside the
# guest with the 'qm guest exec' command

echo "Deploy Proxmox VPS"
echo ""

if [[ $1 = '-d' ]]
then
    echo "Running in Debug mode"
    echo ""
    debug=1
    shift 1
fi

osflavor=$1
templateVm=$2
adminUsername=$3
fullyQualifiedDomainName=$4
templateName=$5
tempalteFile=$6
adminPassword=$7
ip=$8
gateway=$9
netmask=$10

if [[ ! $templateVm =~ ^[0-9]+$ ]]
then
    # templateVm file is not a number (VM ID)
    templateFile=$templateVm
else
    templateFile=''
fi

hostname=`echo $fullyQualifiedDomainName | sed 's/\..*//'` 

echo "Usage:
./deploy-vm.sh [-d] <Guest OS flavor ('Windows', 'Linux' or 'Container')> <Template VM ID or template file> <Adminstrator username> <Fully qualified domain name> <OS Template Name> <OS Template File> <Administrator password> [<IP address>] [<Gateway>] [<Netmask>]" 
echo "With the -d switch the script is running in debug mode without executing any commands"
echo ""
echo "OS Template VM: $templateVm"
echo "OS Template Name: $templateName"
echo "Administrator Username: $adminUsername"
echo "Administrator Password $adminPassword"
echo "Domain: $fullyQualifiedDomainName"
echo "Hostname: $hostname"
echo "OS Template File: $templateFile"
echo "IP: $ip"
echo "Gateway: $gateway"
echo "Netmask: $netmask"

# Get next free VM ID
newVm="$(pvesh get /cluster/nextid)"
echo "New VM Name: $templateName"
echo "New VM ID: $newVm"
echo ""

if [[ $osflavor = 'Container' ]]
then

    # Setup LXC container
    echo "Setup LXC container"

    # check if $tempalteFile is set
    if [[ ! $templateFile ]]
    then
        # Clone container VM
        echo "Clone container VM"
        echo "> pct clone $templateVm $newVm -hostname $hostname"
        if [[ ! $debug ]]
        then
            pct clone $templateVm $newVm -hostname $hostname
        fi

        # Start the new VM
        echo "Start the new VM"
        echo "> pct start $newVm"
        if [[ ! $debug ]]
        then
            pct start $newVm
        fi

        # Change administrator password of the new VM
        echo "Change administrator password of the new VM"
        echo "> pct exec $newVm bash -c \"echo \\\"$adminPassword\\\\n$adminPassword\\\\n\\\" | passwd\""
        if [[ ! $debug ]]
        then
            pct exec $newVm bash -c "echo -e \"$adminPassword\\\n$adminPassword\\\n\" | passwd"
        fi

        # Setup Network
        echo "Setup Network"

        # Stop the new VM
        echo "Stop the new VM"
        echo "> pct stop $newVm"
        if [[ ! $debug ]]
        then
            pct stop $newVm
        fi

    else
        # Create container VM
        echo "Create container VM"
        echo "> pct create $newVm $templateVm -hostname $hostname -password $adminPassword"
        if [[ ! $debug ]]
        then
            pct create $newVm $templateVm  -hostname $hostname -password $adminPassword
        fi

        # Start the new VM
        echo "Start the new VM"
        echo "> pct start $newVm"
        if [[ ! $debug ]]
        then
            pct start $newVm
        fi

        # Setup Network

        # Stop the new VM
        echo "Stop the new VM"
        echo "> pct stop $newVm"
        if [[ ! $debug ]]
        then
            pct stop $newVm
        fi
    fi

    exit
else

    # Clone the template VM
    echo "Clone the template VM"
    echo "> qm clone $templateVm $newVm --name $templateName"
    if [[ ! $debug ]]
    then
        qm clone $templateVm $newVm --name "$hostname"
    fi

    # Start the new VM
    echo "Start the new VM"
    echo "> qm start $netVM"
    if [[ ! $debug ]]
    then
        qm start $netVm
    fi

    # Change administrator password of the new VM
    echo "Change administrator password of the new VM"
    echo "> echo -e \"$adminPassword\\n$adminPassword\\n\" | qm guest passwd $newVm $adminUsername -pass-stdin"
    if [[ ! $debug ]]
    then
        echo -e "$adminPassword\n$adminPassword\n" | qm guest passwd $newVm $adminUsername -pass-stdin
    fi

fi

# Change nework configuration of the new guest VM
echo "Change nework configuration of the new guest VM"

# If guest is Windows
if [ $osflavor = 'Windows' ]
then

    # set hostname
    echo "Set hostname to $hostname"
    echo "> qm guest exe $newVm powershell Rename-Computer -NewName $hostname"
    if [[ ! $debug ]]
    then
        qm guest exe $newVm powershell Rename-Computer -NewName $hostname
    fi

    # Setup Network
    # ...

fi

# If guest is Linux
if [ $osflavor = 'Linux' ]
then

    # set hostname
    echo "Set hostname to $hostname"
    echo "> qm guest exec $netVm hostnamectl set-hostname $hostname"
    if [[ ! $debug ]]
    then
        qm guest exec $netVm hostnamectl set-hostname $hostname
    fi

    # Setup Network
    # ...

fi

# Stop the new VM
echo "Stop the new VM"
echo "> qm stop $newVm"
if [[ ! $debug ]]
then
    qm stop $newVm
fi
