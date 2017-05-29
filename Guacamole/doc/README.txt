To use the Guacamole Feature for VPS2012 some additional Steps are required:

1) Install a seperate Server with Linux Apache Mysql PHP as Virtual Console Proxy Server
2) Install Guacamole https://guacamole.incubator.apache.org/
3) Configure Guacamole with Database Authentication: https://guacamole.incubator.apache.org/doc/gug/jdbc-auth.html
4) Configure Apache Proxy to Guacamole like:
<VirtualHost *:443>
        ServerName vcproxy.yourdomain.com
        ProxyPreserveHost on
        ProxyPass /gc/ ajp://localhost:8009/guacamole/
        ProxyPassReverse /gc/ ajp://localhost:8009/guacamole/

        SSLEngine on
        SSLCertificateFile /etc/apache2/ssl/cert
        SSLCertificateKeyFile /etc/apache2/ssl/cert.key
        SSLCACertificateFile /etc/apache2/ssl/ca-bundle
</VirtualHost>

<VirtualHost *:80>
        ServerName localhost

        ProxyPreserveHost on
        ProxyPass /gc/ ajp://localhost:8009/guacamole/
        ProxyPassReverse /gc/ ajp://localhost:8009/guacamole/
</VirtualHost>

5) copy the Files of html directory to your Webserver vcproxy.yourdomain.com /
6) Edit the config.inc.php
7) Edit the SolidCP -> Servers -> VPS2012 -> Guacamole Section

