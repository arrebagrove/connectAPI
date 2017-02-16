<?php
// header('Content-Type: text/html; charset=utf-8');
// header('Access-Control-Allow-Origin: *');
// error_reporting('E_ALL');

// PDO
function Conn(){
	$c = new PDO('mysql:host=localhost;port=3306;dbname=NAME;charset=utf8', 'root', 'toor');
	// don't cache query
	$c->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
	// show warning text
	$c->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_WARNING);
	// throw error exception
	$c->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
	// don't colose connecion on script end
	$c->setAttribute(PDO::ATTR_PERSISTENT, false);
	// set utf for connection utf8_general_ci or utf8_unicode_ci 
	$c->setAttribute(PDO::MYSQL_ATTR_INIT_COMMAND, "SET NAMES 'utf8mb4' COLLATE 'utf8mb4_unicode_ci'");
	return $c;
}

// Function to get the client IP address
function IP() {
    $ipaddress = '';
    if (isset($_SERVER['HTTP_CLIENT_IP']))
        $ipaddress = $_SERVER['HTTP_CLIENT_IP'];
    else if(isset($_SERVER['HTTP_X_FORWARDED_FOR']))
        $ipaddress = $_SERVER['HTTP_X_FORWARDED_FOR'];
    else if(isset($_SERVER['HTTP_X_FORWARDED']))
        $ipaddress = $_SERVER['HTTP_X_FORWARDED'];
    else if(isset($_SERVER['HTTP_FORWARDED_FOR']))
        $ipaddress = $_SERVER['HTTP_FORWARDED_FOR'];
    else if(isset($_SERVER['HTTP_FORWARDED']))
        $ipaddress = $_SERVER['HTTP_FORWARDED'];
    else if(isset($_SERVER['REMOTE_ADDR']))
        $ipaddress = $_SERVER['REMOTE_ADDR'];
    else
        $ipaddress = $_SERVER['REMOTE_ADDR'];
    return $ipaddress;
}

// secure string
// $nick = htmlentities($_GET['nick'], ENT_QUOTES, 'utf-8');
// $nick = preg_replace("/[^a-zA-Z0-9.@-]/", "", $_GET['nick']);
// $pass = md5($_GET['pass']);
// $email = htmlentities($_GET['email'], ENT_QUOTES, 'utf-8');

?>
