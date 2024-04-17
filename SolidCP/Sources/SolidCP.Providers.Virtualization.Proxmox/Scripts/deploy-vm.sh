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
# [FQDN] [OSTEMPLATENAME] [OSTEMPLATEFILE] [ADMINPASS] [IP] [GATEWAY] [NETMASK]
#
# In the OS Templates, specify as Additional Deploy Script Parameters:
#
# <Guest OS flavor (Windows or Linux or Container)> <ID or name of the template VM> <Administartor username (root for Linux guests, Adminstrator for Windows guests)>
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

debug=

if [[ $1 = '-d' ]]
then
    echo "Running in Debug mode"
    echo ""
    debug=true
    shift 1
fi

fullyQualifiedDomainName=$1
templateName=$2
templateFile=$3
adminPassword=$4
ip=$5
gateway=$6
netmask=$7
osflavor=$8
templateVm=$9
adminUsername=$10

if [[ ! $templateVm =~ ^[0-9]+$ ]]
then
    # templateVm file is not a number (VM ID), lookup VM ID by VM Name
    vmname=$templateVm
    vmnameEscaped=$(sed 's/[^^ ]/[&]/g; s/\^/\\^/g' <<<"$vmname") # escape it.
    vmlist=$(sudo -n qm list | sed -e '1d')
    regex="^[ ]*([0-9]+)[ ]+$vmnameEscaped[ ]+[a-zA-Z]+[ ]+[0-9]+[ ]+[0-9.]+[ ]+[0-9]+[ ]*$"
    templateVm=$(sed -r "s/$regex/\1/g" <<< $vmlist)
fi

hostname=`echo $fullyQualifiedDomainName | sed 's/\..*//'` 

echo "Usage:
./deploy-vm.sh [-d] <Guest OS flavor ('Windows', 'Linux' or 'Container')> <ID or name of the template VM> <Adminstrator username> <Fully qualified domain name> <OS Template Name> <OS Template File> <Administrator password> [<IP address>] [<Gateway>] [<Netmask>]" 
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
newVm="$(sudo -n pvesh get /cluster/nextid)"
echo "New VM Name: $templateName"
echo "New VM ID: $newVm"
echo ""

if [[ $osflavor = 'Container' ]]
then

    # Setup LXC container
    echo "Setup LXC container"

    # check if $tempalteFile is set
    if [[ ! -z $templateFile ]]
    then
        # Clone container VM
        echo "Clone container VM"
        echo "> pct clone $templateVm $newVm -hostname $hostname"
        if [[ -z $debug ]]
        then
            pct clone $templateVm $newVm -hostname $hostname
        fi

        # Start the new VM
        echo "Start the new VM"
        echo "> pct start $newVm"
        if [[ -z $debug ]]
        then
            pct start $newVm
        fi

        # Change administrator password of the new VM
        echo "Change administrator password of the new VM"
        echo "> pct exec $newVm bash -c \"echo \\\"$adminPassword\\\\n$adminPassword\\\\n\\\" | passwd\""
        if [[ -z $debug ]]
        then
            pct exec $newVm "bash -c \"echo -e \\\"$adminPassword\\\\\n$adminPassword\\\\\n\\\" | passwd"
        fi

        # TODO Setup Network
        echo "Setup Network"

        # Stop the new VM
        echo "Stop the new VM"
        echo "> pct stop $newVm"
        if [[ -z $debug ]]
        then
            pct stop $newVm
        fi

    else
        # Create container VM
        echo "Create container VM"
        echo "> pct create $newVm $templateFile -hostname $hostname -password $adminPassword"
        if [[ ! $debug ]]
        then
            pct create $newVm $templateFile -hostname $hostname -password $adminPassword
        fi

        # Start the new VM
        echo "Start the new VM"
        echo "> pct start $newVm"
        if [[ -z $debug ]]
        then
            pct start $newVm
        fi

        # TODO Setup Network
        echo "Setup Network"

        # Stop the new VM
        echo "Stop the new VM"
        echo "> pct stop $newVm"
        if [[ -z $debug ]]
        then
            pct stop $newVm
        fi
    fi

    exit
else

    # Clone the template VM
    echo "Clone the template VM"
    echo "> qm clone $templateVm $newVm --name $templateName"
    if [[ -z $debug ]]
    then
        sudo -n qm clone $templateVm $newVm --name "$hostname"
    fi

    # Start the new VM
    echo "Start the new VM"
    echo "> qm start $newVM"
    if [[ -z $debug && $osflavor != 'Debug' ]]
    then
        qm start $newVm
    fi

    # Change administrator password of the new VM
    echo "Change administrator password of the new VM"
    echo "> echo -e \"$adminPassword\\n$adminPassword\\n\" | qm guest passwd $newVm $adminUsername -pass-stdin"
    if [[ -z $debug && $osflavor != 'Debug' ]]
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
    if [[ -z $debug ]]
    then
        qm guest exe $newVm powershell Rename-Computer -NewName $hostname
    fi

    # TODO Setup Network
    echo "Setup Network"

fi

# If guest is Linux
if [ $osflavor = 'Linux' ]
then

    # set hostname
    echo "Set hostname to $hostname"
    echo "> qm guest exec $newVm hostnamectl set-hostname $hostname"
    if [[ -z $debug ]]
    then
        qm guest exec $newVm hostnamectl set-hostname $hostname
    fi

    # TODO Setup Network
    echo "Setup Network"

fi

# Stop the new VM
echo "Stop the new VM"
echo "> qm stop $newVm"
if [[ -z $debug && $osflavor != 'Debug' ]]
then
    qm stop $newVm
fi
