<?php

class guacamole {
	public $guacusername;
	public $guacpassword;
	public $guacconname;
	private $conf;
	private $authjson;

	function __construct($conf) {
		$this->conf = $conf;
	}

	function addconnection($sql, $name = "") {

		$qsalt = 'select UNHEX(SHA2(UUID(), 256)) As SALT';
		$quser = 'INSERT INTO guacamole_user (username, password_salt, password_hash) VALUES (:username, :salt, UNHEX(SHA2(CONCAT(:password, HEX(:salt2)), 256)))';
		$quserid = 'SELECT user_id FROM guacamole_user WHERE username=:username';
		$qconn = 'INSERT INTO guacamole_connection (connection_name, protocol) VALUES (:name, :protocol)';
		$qconnid = 'SELECT connection_id FROM guacamole_connection WHERE connection_name=:name';
		$qprop = 'INSERT INTO guacamole_connection_parameter VALUES (:connid, :name, :value)';
		$qconperm = 'INSERT INTO guacamole_connection_permission (user_id, connection_id, permission) VALUES (:userid, :connid, "READ")';

		$random = new randomgen();
		$this->guacusername = $random->generateStrongPassword();
		$this->guacpassword = $random->generateStrongPassword();
		$this->guacconname = $random->generateStrongPassword();
		$this->guacconname = $name."-vm-connect-".$this->guacconname;

		try {
			$servername = $sql["servername"];
			$dbname = $sql["dbname"];
			$username = $sql["username"];
			$password = $sql["password"];

			$conn = new PDO("mysql:host=$servername;dbname=$dbname", $username, $password);
			$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

			//generate salt
			$stmt = $conn->prepare($qsalt);
			$stmt->execute();
			$result = $stmt->setFetchMode(PDO::FETCH_ASSOC);
			$result = $stmt->fetchAll();
			$salt = $result[0]['SALT'];

			//add user
			$stmt = $conn->prepare($quser);
			$stmt->bindParam(':username', $this->guacusername, PDO::PARAM_STR);
			$stmt->bindParam(':salt', $salt, PDO::PARAM_STR);
			$stmt->bindParam(':salt2', $salt, PDO::PARAM_STR);
			$stmt->bindParam(':password', $this->guacpassword, PDO::PARAM_STR);
			$stmt->execute();

			//get userid
			$stmt = $conn->prepare($quserid);
			$stmt->bindParam(':username', $this->guacusername);
			$stmt->execute();
			$result = $stmt->setFetchMode(PDO::FETCH_ASSOC);
			$result = $stmt->fetchAll();
			$user_id = $result[0]['user_id'];

			//add connection
			$stmt = $conn->prepare($qconn);
			$stmt->bindParam(':name', $this->guacconname, PDO::PARAM_STR);
			$stmt->bindParam(':protocol', $this->conf['protocol'], PDO::PARAM_STR);
			$stmt->execute();

			//get connectionid
			$stmt = $conn->prepare($qconnid);
			$stmt->bindParam(':name', $this->guacconname);
			$stmt->execute();
			$result = $stmt->setFetchMode(PDO::FETCH_ASSOC);
			$result = $stmt->fetchAll();
			$conn_id = $result[0]['connection_id'];

			//add connectionpermission
			$stmt = $conn->prepare($qconperm);
			$stmt->bindParam(':userid', $user_id, PDO::PARAM_STR);
			$stmt->bindParam(':connid', $conn_id, PDO::PARAM_STR);
			$stmt->execute();

			//add connection propertys
			foreach ($this->conf as $name => $property) {
                		$stmt = $conn->prepare($qprop);
                		$stmt->bindParam(':connid', $conn_id);
                		$stmt->bindParam(':name', $name);
       	 	        	$stmt->bindParam(':value', $property);
		                $stmt->execute();
			}

		}
		catch(PDOException $e) {
			echo "Error: " . $e->getMessage();
			die();
		}
	}


	function gettoken($localguacaurl) {
		// Get Auth Token from guacamole api
		$post_vars = array("username" => $this->guacusername, "password" => $this->guacpassword);
		$post_content = http_build_query($post_vars);
		$clength = strlen($post_content);
		$stream_opts = array(
			'http' => array('method' => "POST",
					'timeout' => 5,
					'header' => "Accept-language: en\r\nContent-type: application/x-www-form-urlencoded\r\nUser-Agent: Mozilla/5.0 (X11; Linux x86_64; rv:12.0) Gecko/20100101 Firefox  /21.0\r\nContent-Length: $clength\r\n",
					'content' => $post_content));
		$context = stream_context_create($stream_opts);
		$contents = file_get_contents($localguacaurl.'/api/tokens', false, $context);
		$this->authjson = json_decode($contents);
	}

	function addcookie() {
		// Build and SET Cookie
		$cookie = array(
			'authToken' => $this->authjson->{'authToken'},
			'username' => $this->authjson->{'username'},
			'dataSource' => $this->authjson->{'dataSource'},
			'availableDataSources' => $this->authjson->{'availableDataSources'}
		);

		$cookie = json_encode($cookie);
		setcookie("GUAC_AUTH", $cookie, time()+3600, "/");
	}
}
?>
