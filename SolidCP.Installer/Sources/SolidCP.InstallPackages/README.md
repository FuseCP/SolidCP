# Build Setup

For this project to fully build you need to have a WSL distro with rpmbuild installed. If not, the Fedora rpm package won't be built. This has been tested with Ubuntu & Fedora Remix.

To install WSL Ubuntu run:

wsl --install Ubuntu

and inside Ubuntu run

sudo apt install rpm rpmlint

To install Fedora Remix run:

winget install -e --id whitewaterfoundry.fedora-remix-for-wsl

and inside Fedora run

sudo dnf install rpmdevtools rpmlint

# Structure of the Folders

* The content of the Debian package resides in the Debian folder.
* The content of the RPM package resides in the Fedora folder. For Fedora, the .specs file must reside in
  the Fedora\SPECS folder.
* In the src folder there are the some common files that will be copied to the Debian and Fedora
  folder before build.
