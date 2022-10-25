Guacamole custom authentication provider for SolidCP (No need SolidCP php-script and mysql database)

***Configuration***

1) Install Guacamole Server on Linux-VM (Ubuntu 20.04.03 in my case). Full manual: http://guacamole.apache.org/doc/gug/installing-guacamole.html
	- apt-get update
	- Install GCC-Compiler: apt-get install gcc
	- Install required libraries: apt-get install libcairo2-dev libjpeg-turbo8-dev libpng-dev libossp-uuid-dev freerdp2-dev freerdp2-x11 libtool-bin libwebsockets-dev libavformat-dev
	- Give a write permission for current user to "/usr/sbin" ( https://www.mail-archive.com/user@guacamole.apache.org/msg08613.html )
	- Download guacamole-server-1.3.0.tar.gz from https://guacamole.apache.org/releases/1.3.0/
	- Unpack sources: tar -xzf guacamole-server-1.3.0.tar.gz
	- cd guacamole-server-1.3.0/
	- Run build script: ./configure --with-init-dir=/etc/init.d
	- Compile guacamole server: make
	- Install guacamole server: make install
	- ldconfig
	- systemctl enable guacd
	- systemctl start guacd

2) Install last version of Apache and Tomcat9

3) Install Guacamole Client:
	- Download Guacamole client - guacamole-1.3.0.war from https://guacamole.apache.org/releases/1.3.0/
	- Copy Guacamole client to Your tomcat directory:  cp guacamole-1.3.0.war /{tomcat_path}/webapps/guacamole.war
	- Restart Tomcat: systemctl restart tomcat
	- If Guacamole installed successfully You should see login site on: http://{your_server}:8080/guacamole
	- For HTTPS bindings use Apache reverse proxy or configure Tomcat for SSL

4) Create Guacamole Home Directory
	- mkdir /etc/guacamole /etc/guacamole/extensions /etc/guacamole/lib
	- touch /etc/guacamole/guacamole.properties
	- chown -R {tomcat_user} /etc/guacamole/

5) Configure Guacamole to use SolidCP authentication provider
	- Copy SolidCPAuthenticationProvider\target\guacamole-auth-solidcp-1.3.0.jar to /etc/guacamole/extensions
	- SolidCP authentication provider need two external jar files:
			bcprov-jdk15on-1.70.jar (https://mvnrepository.com/artifact/org.bouncycastle/bcprov-jdk15on/1.70)
			json-20211205.jar (https://mvnrepository.com/artifact/org.json/json/20211205)
		Download libraries or copy it from SolidCPAuthenticationProvider\target\lib to /etc/guacamole/lib
	- In SolidCP go to Configuration - Servers - HyperV - Guacamole and generate Guacamole Encryption Password
	- Set "Guacamole Connect Script URL" to Your Guacamole login site
	- Set Hyper-V IP, domain, user and password (User should be in Domain-admins group)
	- Edit Your guacamole.properties file:

guacd-hostname: localhost
guacd-port:    4822
#Your Guacamole Encryption Password from SolidCP
solidcp-key: {Guacamole Encryption Password}
#Connection link lifetime in seconds
solidcp-link-exp-time: 60
#Server keyboard layout (http://guacamole.apache.org/doc/gug/configuring-guacamole.html#rdp-session-settings)
server-layout: en-us-qwerty

	- Restart Tomcat: systemctl restart tomcat

6) Disable browser local storage authentication in Guacamole-client for auto-LogOff
	- Open /{tomcat_path}/webapps/guacamole/guacamole.min.js
	- Find getItem("GUAC_AUTH") and replace it to: getItem("DUMMY")
	- Save changes and restart Tomcat: systemctl restart tomcat


***Compilation***

	- Install Maven: apt-get install maven
	- Copy SolidCPAuthenticationProvider to Your server
	- cd SolidCPAuthenticationProvider
	- mvn package


***Guacamole User Manual***
http://guacamole.apache.org/doc/gug/using-guacamole.html
