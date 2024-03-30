#Fully based on https://boxofcables.dev/accelerated-kvm-guests-on-wsl-2/
if [ -z "$1" ]
  then
    echo "Must supply your Windows 10 username"
    exit
fi
WIN_USERNAME=$1

#package updates and installations

sudo apt update && sudo apt -y upgrade
sudo apt -y install build-essential libncurses-dev bison flex libssl-dev libelf-dev cpu-checker qemu-kvm aria2 

#download WSL2 Kernel and backup config file
cd ~
aria2c -x 10 https://github.com/microsoft/WSL2-Linux-Kernel/archive/4.19.104-microsoft-standard.tar.gz
tar -xf WSL2-Linux-Kernel-4.19.104-microsoft-standard.tar.gz
cd ./WSL2-Linux-Kernel-4.19.104-microsoft-standard/
cp ./Microsoft/config-wsl .config

#add properties to avoid using make menuconfig command (CONFIG_VHOST_NET and CONFIG_VHOST are already in the file)
echo 'KVM_GUEST=y
CONFIG_KVM=y
CONFIG_KVM_INTEL=m' >> .config

#build the kernel
make -j 8

#install kernel modules
sudo make modules_install

#install the new wsl2 kernel
cp arch/x86/boot/bzImage /mnt/c/Users/$WIN_USERNAME/bzImage
echo '[wsl2]
nestedVirtualization=true
kernel=C:\\Users\\'$WIN_USERNAME'\\bzImage' > /mnt/c/Users/$WIN_USERNAME/.wslconfig

#create kvm-nested.conf file and moves to /etc/modprobe.d/ folder
echo 'options kvm-intel nested=1
options kvm-intel enable_shadow_vmcs=1
options kvm-intel enable_apicv=1
options kvm-intel ept=1' > kvm-nested.conf
sudo mv kvm-nested.conf /etc/modprobe.d/
exit