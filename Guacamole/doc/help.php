<?php
/*** CONFIG ***/
/*
// URLs
$localguacaurl = "http://localhost:8080/guacamole";
$publicguacaurl = "http://guacamole-tomcat:8080/guacamole";

// guacamole mysql
$servername = 'localhost';
$username = 'guacamoleuser';
$password = 'guacamolepass';
$dbname = 'guacamoledatabase';

// Connect to
$guacconf = array(
        "protocol" =>  "rdp",
        "hostname" => "hostname",
        "username" => "Administrator",
        "password" => "password",
        "domain" => "hypercloud",
        "port" => "2179",
        "security" => "nla",
        "ignore-cert" => "true",
        "preconnection-id" => "",
        "preconnection-blob" => "7638ed56-d4c4-4abe-a95d-a035093cfa30",
);
*/


/**

<authorize username="325656" password="Abc123">
        <protocol>rdp</protocol>
        <param name="hostname">hypervhostname</param>
        <param name="port">2179</param>
        <param name="domain">hypervdomain</param>
        <param name="username">Administrator</param>
        <param name="password">guacamole-pass</param>
        <param name="security">nla</param>
        <param name="ignore-cert">true</param>
        <param name="preconnection-id"></param>
        <param name="preconnection-blob">7638ed56-d4c4-4abe-a95d-a035093cfa30</param>
</authorize>


-- Generate salt
SET @salt = UNHEX(SHA2(UUID(), 256));

-- Create user and hash password with salt INSERT INTO guacamole_user (username, password_salt, password_hash)
     VALUES ('myuser', @salt, UNHEX(SHA2(CONCAT('mypassword', HEX(@salt)), 256)));

-- Create connection
INSERT INTO guacamole_connection (connection_name, protocol) VALUES ('test', 'vnc');

-- Determine the connection_id
SELECT * FROM guacamole_connection WHERE connection_name = 'test' AND parent_id IS NULL;

-- Add parameters to the new connection
INSERT INTO guacamole_connection_parameter VALUES (1, 'hostname', 'localhost');
INSERT INTO guacamole_connection_parameter VALUES (1, 'port', '5901');

**/
?>
