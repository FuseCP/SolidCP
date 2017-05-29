<?php

// Include Config & Encryption
include("config.inc.php");
include("inc/encryption.php");
include("inc/guacamole.php");

if (!$_REQUEST['e'] || $_REQUEST['e'] == "") {
	die();
}

// Encrypt GET Variable
$crypt = new encryption();
$enc = $crypt->urlsafe_b64decode($_REQUEST['e']);
$confstring = $crypt->decrypt($key, $enc);
$guaccookieconf = json_decode($confstring, true);


// Check required Parameters
if (!$guaccookieconf || !$guaccookieconf["hostname"] || $guaccookieconf["hostname"] == "" || !$guaccookieconf["username"] || $guaccookieconf["username"] == "" || !$guaccookieconf["password"] || $guaccookieconf["password"] == "") {
        echo 'Authentifizierungs Fehler. Bitte wenden Sie sich an den Support';
        die();
}

//$guaccookieconf["preconnectionblob"] = "7638ed56-d4c4-4abe-a95d-a035093cfa30";

$guacconf = array(
        "protocol" =>  $guaccookieconf["protocol"],
        "hostname" => $guaccookieconf["hostname"],
        "username" => $guaccookieconf["username"],
        "password" => $guaccookieconf["password"],
        "domain" => $guaccookieconf["domain"],
        "port" => $guaccookieconf["port"],
        "security" => $guaccookieconf["security"],
        "ignore-cert" => "true",
        "preconnection-id" => "",
        "preconnection-blob" => $guaccookieconf["preconnectionblob"],
);


$guacamole = new guacamole($guacconf);
$guacamole->addconnection($sql, $guaccookieconf["vmhostname"]);
$guacamole->gettoken($localguacaurl);
$guacamole->addcookie();

// Redirect to Guacamole
header("Location: ".$publicguacaurl."/#/client/");

// IFRAME
//echo '<html><head><title>VPS '.$guaccookieconf["vmhostname"].'</title><head><body style="overflow: hidden;">';
//echo '<iframe src="'.$publicguacaurl.'/#/client/" style="border: 0; width: 100%; height: 100%; overflow: hidden;">Your browser does not support iFrames. Please Click <a href="'.$publicguacaurl.'">here</a></iframe></body></html>';
?>

