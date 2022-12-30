**********************************************************************************************************************************
  Guacamole for SolidCP

  INTRO

  Apache Guacamole allows users to access computers with a web browser without any other special software or plugins. Deploying 
  Guacamole with SolidCP provides users with console access to virtual machines directly through the SolidCP client portal. 

  This project includes an authentication extension for Guacamole located in the auth-ext folder. The authentication extension
  is the means by which users securely authenticate console access to a specific virtual machine (vm).  The extension instructs
  Guacamole to establish a virtual machine connection (vmconnect) to the Hyper-V host where the vm resides. VMConnect is a tool
  used for interacting with a virtual machine at the hypervisor level.  Therefore, console access can be established to virtual
  machines regardless of operating system or network accessibility.

  BENEFITS

  VMConnect will not provide the same elegant user experience as regular RDP for Windows or SSH for Linux.  Therefore, it is 
  not a replacement for those protocols.  However, the benefits of providing console access with vmconnect are immense.  Users
  can quickly access the console from any computer without regard for special software or firewall restrictions.  It simply 
  works over an https url in a browser.  Users will be able to access vm's to resolve boot issues, fix network misconfigurations,
  fix firewall issues if they lock themselves out, boot into rescue mode and much more. Some users may prefer to use the console
  over other methods of remote access simply for its convenience and ease of use.

  SECURITY

  As always, you should consider security as it applies to your unique environment. We are providing information to help you
  understand how Guacamole works with SolidCP.  We are also providing a configuration guide for deploying Guacamole in a
  production environment which uses Nginx as a reverse proxy and SSL.  However, the information that we are providing is
  not an end all be all for configuration and security.  Things such as network configuration, firewalls, vlans, group
  policies and other considerations should be taken into account before production deployment. Reference links are provided
  so that you can learn more directly from the software providers being used.

  PREP

  The configuration guide uses two dedicated vm's running Ubuntu 22.04.1 to serve Guacamole/Tomcat and Nginx/SSL.  It is 
  certainly possible to use other versions of Linux but our guide is specifically for Ubuntu.  Ubuntu 22.04.1 is the most
  current release as of this writing.  Newer releases are encouraged and will likely work without much or any trouble.
  However, we do not recommend using earlier versions due to security improvements.  Additional requirements will be detailed
  as you work your way through the configuration steps.  The guide assumes you already have SolidCP and Hyper-V setup and are
  successfully deploying vm's with SolidCP.  If not, you are in the wrong place.

  The guide does not cover the topic of load balancing Guacamole servers with Nginx.  However, you may wish to consider doing
  so. Some benefits include the enhanced ability to make future updates without console downtime, reducing the load on each of
  the Guacamole servers, improving overall uptime and delivering a higher quality service to users.

  If you are new to Guacamole with SolidCP then a good approach is to configure Guacamole using our guide and then continue
  to improve on your deployment by making additional enhancements.

  TECHNICAL

  The following is a network diagram for the deployment outlined in the configuration guide.  It shows the protocol and port
  numbers required for communication.
  

               SolidCP Users
                     |
                 internet
                     |
        ----<--- (tcp/443) ---->----
        |                          |
  SolidCP Portal               Nginx/SSL --> (tcp/8080) --> Guacamole/Tomcat --> (tcp/2179) --> Hyper-V Host
        |                                                                                            |
    (tcp/9002)                                                                                       |
        |                                                                                            |
 SolidCP Enterprise                                                                                  |
        |                                                                                            |
        ------------------>---------------------  (tcp/9003)  ------------------>---------------------


  Nginx will run in a dedicated vm and will serve as a reverse proxy to Tomcat. Nginx also serves as the SSL termination
  point on port 443 and will maintain the SSL certificate. Guacamole and Tomcat will run together in a separate dedicated vm.
  Guacamole is a web application that is served by Tomcat on port 8080.  Guacamole establishes vmconnect to Hyper-V on port
  2179. Not shown, the Guacamole service runs on port 4822 however it is only accessed locally.

  Communication between Nginx and Tomcat is not encrypted.  It is technically possible but not favored for proxied 
  communications. Instead, consider isolating the network for Nginx and Tomcat by placing those two servers into
  their own unique vlan using vlan routing and a firewall to manage traffic.  The configuration guide includes software
  firewall configuration on both servers, however it does not cover network design in detail.
  
  When a SolidCP user signs into the portal and navigates to their vm, they will be presented with an 'Open Console' button
  that sits just below the vm thumbnail image.  Additionally, the page will contain links to open the console in different
  resolutions.  Establishing a console connection is as easy as clicking the 'Open Console' button.  A browser window will
  open and display the vm console.

  What happens behind the scenes is a little more technical. SolidCP creates an encrypted connection string that contains
  all of the information needed to establish the connection and opens the browser window with an https url that contains the
  encrypted string. The url points to the Nginx server.  The request is then proxied to Tomcat and processed by Guacamole
  using the SolidCP authentication extension.  The extension will tell Guacamole to accept or reject the request.  If
  rejected, the user will be presented with the Guacamole home page.  This is typically the result of an expired link which
  can occur if the clocks and timezones of the SolidCP server and the Guacamole server are not in sync. If accepted, the
  extension provides Guacamole with the information required to establish a connection.  Guacamole then attempts to
  establish vmconnect with Hyper-V.  Hyper-V will accept or reject the request.  It's also possible that Guacamole cannot
  reach the Hyper-V host at all.  In all of these cases, the user will be presented with a 'login failed' screen.  This is
  typically the result of incorrect Hyper-V credentials or firewall/policy restrictions on the Hyper-V host.  If Hyper-V 
  accepts the connection then a console session will be established and displayed to the user.

  The user will maintain the console session with a direct connection to Nginx from the internet.  Guacamole will consider 
  the session active as long as it's open, regardless of mouse or keyboard movement.  If you wish to implement an idle
  timeout then this would be part of the group policy applied to your Hyper-V hosts or individual VMs.  The console can be
  terminated by the user either by closing the browser window or using the Guacamole menu. Either way, Guacamole will 
  destroy the session within about a minute after it is terminated. There is a caveat to this. The shortest amount of time
  that we can configure for Guacamole session timeouts is 1 minute. As a result, users can re-establish console connections
  without reauthentication before the session is destroyed.  Further, users may have trouble establishing a console to a
  different vm until the original session is destroyed.  Waiting a minute or two after closing a console resolves these
  issues.
  
  The Guacamole menu is opened by pressing Alt + Ctrl + Shift when the focus on the console window.

  For Windows logins, users will need to open the Guacamole menu, select the option to use the on-screen keyboard, and then 
  press Alt + Ctrl + Del using the on-screen keyboard.  The user can then close the on-screen keyboard and use their own
  keyboard from that point forward.

  FINAL NOTE
  
  It is recommended that this Guacamole deployment is dedicated to SolidCP.  It is possible to install additional
  authentication providers and use it for other purposes but this is strongly discouraged.  In those cases, you should spin
  up a separate Guacamole deployment.  The configuration guide only installs the SolidCP authentication provider.
  Therefore, there is little value in displaying the login which appears on the Guacamole home page even though no
  credentials will be configured that will allow login through the home page.  Because of this, we have created another
  extension which displays the SolidCP logo instead of the login.  The configuration guide will have you install this 
  extension.  However, it is optional.  The extension can be found in the brand-ext folder and, optionally, be customized 
  with your own logo and other content.

  REFERENCE LINKS

  SolidCP Provider      https://github.com/FuseCP/SolidCP/tree/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider
  Guacamole Manual      https://guacamole.apache.org/doc/gug/introduction.html
  Guacamole Server      https://guacamole.apache.org/releases/1.4.0/
  Guacamole Client      https://guacamole.apache.org/releases/1.4.0/
  Tomcat Manual         https://tomcat.apache.org/tomcat-9.0-doc/index.html
  Bouncy Castle         https://mvnrepository.com/artifact/org.bouncycastle/bcprov-jdk15on/1.70
  JSON in Java          https://mvnrepository.com/artifact/org.json/json/20211205
  SLF4J Logger          https://mvnrepository.com/artifact/org.slf4j/slf4j-api
  Nginx Manual          https://docs.nginx.com/nginx/admin-guide

**********************************************************************************************************************************

*** CONFIGURATION GUIDE ***

- denotes a comment
# denotes ubuntu command

1) CREATE TWO NEW VMs - Nginx and Guacamole
	- The new VMs can be created with whatever settings you choose
	- In my case, I used Hyper-V Manager to create both VMs the same way with the following settings...
		- Generation 2
		- Secure Boot enabled using template MicrosoftUEFICertificateAuthority
		- 4 procs, 8192 MB RAM and 25 GB dynamically expanding disk
			- The procs and memory really depend on your usage (need at least 4096 MB RAM for ubuntu installer)
			- 25GB is probably the right amount of disk for most cases (plenty enough for software and log growth) 
			  if space is a concern, you can get away with less
		- Synthetic NIC - both servers on same vlan (access to internet required)
		- Attach Ubuntu 22.04.1 installation DVD

2) INSTALL UBUNTU (Nginx VM and Guacamole VM)
	- Start the VM and begin the Ubuntu installation from the installation DVD
	- The installation can proceed however you see fit. You really only need to assign it one static ip that can reach
	  the internet and install openssh server to complete the configuration steps
	- In my case, I chose the following options for both VMs...
		- After selecting the language and layout options, I chose the default install option
		- Configured a static ip that can reach the internet (private ip protected by firewall recommended)
		- Chose the default storage option
			- On review of the storage layout, I modified the ubuntu-lv logical volume to use all of the disk space 
			  Ubuntu installer defaults to using about half the available space
		- Assigned a hostname and username/password - this is the username/password you will use to SSH into the server
		- Selected the option to install openssh server
		- No featured snaps installed		
		- Reboot when the installation completes

3) INITIAL PREP (Guacamole VM)
	- SSH into Guacamole VM (all commands are run from the home directory of the user unless otherwise stated)
	- Enable Ubuntu firewall. We will revisit this section later to make the rules more restrictive.
		# sudo nano /etc/default/ufw
		- In nano, modify line IPV6=yes to IPV6=no, then save by ctrl+x, y, enter
		# sudo systemctl restart ufw
		# sudo ufw allow proto tcp from any to any port 22
		# sudo ufw allow proto tcp from any to any port 8080
		# sudo ufw enable
		# sudo ufw status
		# sudo apt update
		# sudo apt upgrade -y
	- Set timezone to match the same timezone configured on your solidcp server - replace <region/city> with your timezone
		# timedatectl
		# timedatectl list-timezones
		# sudo timedatectl set-timezone <region/city> 
		# sudo reboot

4) INSTALL GUACAMOLE AND TOMCAT SERVER (Guacamole VM)
	- SSH into Guacamole VM
		# sudo apt update
		# sudo apt install gcc make -y
		# sudo apt install libcairo2-dev libjpeg-turbo8-dev libpng-dev libtool-bin uuid-dev freerdp2-dev libssl-dev libvorbis-dev libwebp-dev -y
		# sudo apt install libavcodec-dev libavformat-dev libavutil-dev libswscale-dev libossp-uuid-dev libwebsockets-dev build-essential -y
		# wget -O guacamole-server-1.4.0.tar.gz https://apache.org/dyn/closer.lua/guacamole/1.4.0/source/guacamole-server-1.4.0.tar.gz?action=download
		# tar -xzf guacamole-server-1.4.0.tar.gz
		# cd guacamole-server-1.4.0/
		# CFLAGS=-Wno-error ./configure --with-init-dir=/etc/init.d
		# sudo make
		# sudo make install
		# sudo ldconfig
		# cd ~
		# sudo systemctl enable guacd
		# sudo systemctl start guacd
		# sudo apt install tomcat9 tomcat9-admin tomcat9-common tomcat9-user -y

5) INSTALL GUACAMOLE CLIENT (Guacamole VM)
		# sudo mkdir /etc/guacamole
		# sudo mkdir /etc/guacamole/lib
		# sudo mkdir /etc/guacamole/extensions
		# sudo wget -O /etc/guacamole/guacamole.war https://apache.org/dyn/closer.lua/guacamole/1.4.0/binary/guacamole-1.4.0.war?action=download
		# sudo ln -s /etc/guacamole/guacamole.war /var/lib/tomcat9/webapps/
		# sudo chown -R tomcat:tomcat /etc/guacamole
		# sudo systemctl restart guacd tomcat9
	- Verify guacamole is running - you should be presented with the Guacamole login screen - Debugging instructions are in step 8
		- Open a browser and navigate to http://<guacip>:8080/guacamole replacing <guacip> with the ip address assigned to the Guacamole VM 

6) CONFIGURE GUACAMOLE (Guacamole VM)
	- Download project files. If you experience issues, you can also download the files manually and SFTP them onto the server
	- Each command should be pasted into SSH as one line
		# sudo wget -O /etc/guacamole/extensions/guacamole-auth-solidcp-1.4.0.jar https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/auth-ext/target/guacamole-auth-solidcp-1.4.0.jar
		# sudo wget -O /etc/guacamole/lib/bcprov-jdk15on-1.70.jar https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/auth-ext/target/lib/bcprov-jdk15on-1.70.jar
		# sudo wget -O /etc/guacamole/lib/json-20211205.jar https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/auth-ext/target/lib/json-20211205.jar
		# sudo wget -O /etc/guacamole/lib/slf4j-api-2.0.5.jar https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/auth-ext/target/lib/slf4j-api-2.0.5.jar
		# sudo wget -O /etc/guacamole/lib/slf4j-simple-2.0.5.jar https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/auth-ext/target/lib/slf4j-simple-2.0.5.jar
		# sudo wget -O /etc/guacamole/guacamole.properties https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/config/guacamole.properties
		# sudo wget -O /etc/guacamole/logback.xml https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/config/logback.xml
		# sudo nano /etc/guacamole/guacamole.properties
	- Create a user account for accessing Hyper-V from Guacamole
		- Create a new user in active directory
		- Make the user a member of the domain admins group
		- Alternatively, you can create a local administrator on the Hyper-V host but not recommended
	- Sign into your SolidCP portal
		- Go to Configuration -> Servers -> Hyper-V -> Guacamole
		- Set the Guacamole Script URL to http://<guacip>:8080/guacamole replacing <guacip> with the ip assigned to the Guacamole VM
		- Generate Guacamole encryption password
		- Set Hyper-V host IP
		- Set Hyper-V Domain (or name of Hyper-V host for local administrator)
		- Set Hyper-V User and Password (same as the new user you just created)
		- Copy the entire Guacamole encryption key (place cursor in box, press ctrl+a, ctrl+c)
		- Click Update to save the configuration
	- Provide Guacamole with the encryption key
		- In nano, move cursor to end of solidcp-key: and paste the encryption key (usually right-click) then ctrl+x, y, enter to save
		# sudo chown -R tomcat:tomcat /etc/guacamole
		# sudo systemctl restart guacd tomcat9

7) VERIFY GUACAMOLE (Guacamole VM)
	- When opening http://<ip>:8080/guacamole in a browser it should present the guacamole login screen
	- When clicking 'Open Console' for a VM in SolidCP it should present a console session to the server
	- If not working then the following may help you resolve issues
		- Verify the services are running
			# sudo systemctl status guacd
			# sudo systemctl status tomcat9
		- Enable guacd debugging
			# sudo nano /etc/guacamole/logback.xml
			- change <root level="info"> to <root level="debug">, to save press ctrl+x, y and enter 
			# sudo systemctl restart guacd tomcat9
		- Monitor debugging info
			# tail -f /var/lib/tomcat9/logs/catalina.out
			- Attempt to open a console session while you have your SSH session to guacamole open and it should display 
			  debugging info in real time
		- You may also scroll through all the logs instead of viewing in real time
		  This can be helpful to identify that the SolidCPAuthenticationProvider is getting loaded.
			# sudo nano /var/lib/tomcat9/logs/catalina.out
		- Other items to check
			- SolidCP generated key matches guacamole.properties solidcp-key value
			- User specified in SolidCP Guacamole configuration has administrative rights to Hyper-V host
			- Software firewall on Hyper-V host allows tcp/2179 traffic from the guacamole server ip
			- All of the files are valid and exist in /etc/guacamole and its subfolders
			- All of the files (including libs and extensions) in /etc/guacamole are owned by tomcat user
		- Once it is working, change the logback.xml root level back to "info" and restart services (See "Enable guacd debugging" section)

8) INSTALL BRANDING EXTENSION (Guacamole VM) - OPTIONAL BUT RECOMENDED
		# sudo wget -O /etc/guacamole/extensions/guacamole-brand-solidcp.jar https://raw.githubusercontent.com/FuseCP/SolidCP/master/SolidCP/Sources/Tools/Guacamole/SolidCPAuthenticationProvider/brand-ext/guacamole-brand-solidcp.jar
		# sudo chown tomcat:tomcat /etc/guacamole/extensions/guacamole-brand-solidcp.jar
		# sudo systemctl restart guacd tomcat9
	- Verify it's working
		- When opening http://<guacip>:8080/guacamole in a browser it should display only the SolidCP logo

9) GUACAMOLE ACCESS RESTRICTION (Guacamole VM)
	- Now we will begin to prepare Guacamole for nginx
	- Delete the existing rule for port 8080 - replace <rulenum> with the rule number associated with port 8080 (ie 2)
		# sudo ufw status numbered
		# sudo ufw delete <rulenum>
	- Add new rule for port 8080 - replace <nginxip> with the ip that you assigned to the Nginx VM
		# sudo ufw allow proto tcp from <nginxip> to any port 8080
	- You may also wish to further restrict openssh server on tcp port 22 at this point

10) TOMCAT IP TRACKING (Guacamole VM)
	- Add a RemoteIPvalve in Tomcat - used to track actual source of guacamole requests
		# sudo nano /var/lib/tomcat9/conf/server.xml
		- Scroll to the <host> section and add the following lines replacing <nginxip> with the ip assigned to the Nginx VM
              <Valve className="org.apache.catalina.valves.RemoteIpValve"
		         internalProxies="<nginxip>"
				 remoteIpHeader="x-forwarded-for"
				 remoteIpProxiesHeader="x-forwarded-by"
				 protocolHeader="x-forwarded-proto" />
		- Press ctrl+x, y, enter to save
		# sudo systemctl restart guacd tomcat9
	- Verify services are running
		# sudo systemctl status guacd
		# sudo systemctl status tomcat9

11) INITIAL PREP (Nginx VM)
	- SSH into Nginx VM
	- Add firewall rules. You may wish to further restrict openssh on tcp port 22. Port 443 will remain open from any.
		# sudo nano /etc/default/ufw
		- in nano, modify line IPV6=yes to IPV6=no, then save by ctrl+x, y, enter 
		# sudo systemctl restart ufw
		# sudo ufw allow proto tcp from any to any port 22
		# sudo ufw allow proto tcp from any to any port 443
		# sudo ufw enable
		# sudo ufw status
		# sudo apt update
		# sudo apt upgrade -y
	- Set timezone to match the same timezone configured on your solidcp server - replace <region/city> with your timezone
		# timedatectl
		# timedatectl list-timezones
		# sudo timedatectl set-timezone <region/city> 
		# sudo reboot

12) INSTALL NGINX (Nginx VM)
	- SSH into Nginx VM
		# sudo apt update
		# sudo apt install nginx -y

13) PREPARING FOR NGINX (Nginx VM)
	- Decide on the fully qualified domain name you wish to use for accessing guacamole
	- You will need DNS access for the domain name
	- You will need an SSL certificate for the fully qualified domain that you use (wildcard will work)
	- The certificate should be issued by a trusted authority (let's encrypt will work)
	- This guide does not cover generating a certificate
	- I will be using the fqdn console.domain.com in this guide. Replace references to console.domain.com with your domain name.

14) CONFIGURE DNS AND SSL (Nginx VM)
	- Add a DNS A record for console.domain.name that resolves to the public ip address of the Nginx server
	- Create a file named console.domain.com.crt
		- Paste your issued certificate into the file (including beginning and end hyphenated statements)
		- You may also want to paste the intermediate and root certificates immediately following the issued certificate
		  to create a chained certificate file.  This is only really necessary if Ubuntu does not recognize the trusted 
		  authority that issued you the certificate.
	- Create a file named console.domain.com.key
		- Paste only your private key into the file (including beginning and end hyphenated statements)
	- Create a folder on the Nginx to store the certificate files
		# sudo mkdir /etc/nginx/ssl
	- Copy console.domain.com.crt and console.domain.com.key to /etc/nginx/ssl/
	- Secure the ssl folder
		# sudo chmod 600 /etc/nginx/ssl

15) CONFIGURE NGINX (Nginx VM)
	- Create a new Nginx server block file
		# sudo nano /etc/nginx/sites-available/console.domain.com
		- Copy the contents of SolidCPAuthenticationProvider\config\nginx.txt into the nano window
		- In nano, replace the following values
			- <nginxip> replace with the ip assigned to the nginx VM
			- <console.domain.com> replace all instances with your fully qualified domain name
			- <dns1> and <dns2> with the ip addresses of the dns servers you wish to use
			- <guacip> with the ip assigned to the guacamole VM
			- ** This is an example configuration. Feel free to modify it in other ways to suit your needs.
			- type ctrl+x, y, enter to save the file
	- Generate diffie hellman certificate for secure key exchange
		# sudo openssl dhparam -dsaparam -out /etc/nginx/dhparam.pem 4096
	- Link the new server block to nginx
		# sudo ln -s /etc/nginx/sites-available/console.domain.com /etc/nginx/sites-enabled/
	- Verify there are no syntax errors and resolve those errors if any exist before restarting service
		# sudo nginx -t
		# sudo systemctl restart nginx
	- Verify that it's working
		- open a browser and navigate to https://console.domain.com
		- You should see the Guacamole login or the SolidCP logo if the branding extension was installed

16) ALMOST DONE
	- Sign into your SolidCP portal
	- Go to Servers -> Hyper-V -> Guacamole
	- Change Guacamole Script URL to https://console.domain.com
	- Go to VM and click 'Open Console' and verify it is working within SolidCP

Congratulations! Your time and patience have paid off. You now have secure console access in SolidCP.

