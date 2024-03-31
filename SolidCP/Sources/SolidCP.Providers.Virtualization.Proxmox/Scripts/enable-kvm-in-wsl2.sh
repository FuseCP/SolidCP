# If you have Proxmox installed inside WSL2, you can execute this script to enable KVM support in Proxmox

# Fully based on https://boxofcables.dev/accelerated-kvm-guests-on-wsl-2/

if [ -z "$1" ]
  then
    echo "Must supply your Windows 10 username"
    exit
fi
WIN_USERNAME=$1

# package updates and installations
sudo apt update && sudo apt -y install build-essential libncurses-dev bison flex libssl-dev libelf-dev cpu-checker qemu-kvm aria2 bc python3 pahole git

# download WSL2 Kernel and backup config file
cd ~
git clone https://github.com/microsoft/WSL2-Linux-Kernel.git --depth=1 -b linux-msft-wsl-6.1.y
cd ./WSL2-Linux-Kernel

# add properties to avoid using make menuconfig command (CONFIG_VHOST_NET and CONFIG_VHOST are already in the file)
echo "Enable kvm, as described here https://wiki.gentoo.org/wiki/QEMU#Kernel"
xdg-open https://wiki.gentoo.org/wiki/QEMU#Kernel &
echo "Save your edited configuration in the Microsoft folder when running make menuconfig"
echo "Press any key to continue..."
read -s -n 1

make menuconfig KCONFIG_CONFIG=Microsoft/config-wsl

# copy newest config in Microsoft to .config
cd Microsoft
cp "$(find . -maxdepth 1 -type f -exec ls -t {} + | head -1)" ../.config
cd ..

# build the kernel
make -j8

# install kernel modules
make modules_install

# install the new wsl2 kernel
cp arch/x86/boot/bzImage /mnt/c/Users/$WIN_USERNAME/bzImage
echo -e "[wsl2]
nestedVirtualization=true
kernel=C:\\Users\\$WIN_USERNAME\\bzImage" > /mnt/c/Users/$WIN_USERNAME/.wslconfig
echo "Modified C:\\Users\\$WIN_USERNAME\\.wslconfig:"
cat /mnt/c/Users/$WIN_USERNAME/.wslconfig

# create kvm-nested.conf file and moves to /etc/modprobe.d/ folder
echo 'options kvm-intel nested=1
options kvm-intel enable_shadow_vmcs=1
options kvm-intel enable_apicv=1
options kvm-intel ept=1' > kvm-nested.conf
sudo mv kvm-nested.conf /etc/modprobe.d/

echo "Finished. Please restart wsl by exiting your shell and running wsl --shutdown in Windows and returning to wsl"
exit
