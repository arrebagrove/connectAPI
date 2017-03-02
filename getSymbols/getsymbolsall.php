<?php 
/*
array = 0  - show json format
array = 1  - show array format
*/

if (!empty($_GET['array'])) {
	$arr = (int)$_GET['array'];
	getSymbolAll($arr);
}else{
	getSymbolAll();
}

// PDO
function Conn(){
$connection = new PDO('mysql:host=127.12.109.130;port=3306;dbname=www;charset=utf8', 'User', 'Password');
// don't cache query
$connection->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
// show warning text
$connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_WARNING);
// throw error exception
$connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
// don't colose connecion on script end
$connection->setAttribute(PDO::ATTR_PERSISTENT, false);
// set utf for connection utf8_general_ci or utf8_unicode_ci 
$connection->setAttribute(PDO::MYSQL_ATTR_INIT_COMMAND, "SET NAMES 'utf8mb4' COLLATE 'utf8mb4_unicode_ci'");
return $connection;
}

function getSymbolAll($array = 0){

	try{
		$m = date('i');
		$h = date('H');
		$d = date('d');
		$month = date('m');
		$y = date('Y');
		$time = time();

		// mysql
		$db = Conn();							
		$res = $db->query("SELECT symbolName,lastAsk,lastBid,digits,description,time,from_unixtime(time) as data from symbolssmall WHERE hour=$h AND day = $d AND month = $month AND year = $y ORDER BY id ASC");
		$x = $res->fetchAll(PDO::FETCH_ASSOC);
		
		// replace ! to # if you need
		// foreach ($x as $k => $v) { $x[$k] = str_replace("!", "#", $v); }
		
		if ($array > 0) {
			echo "<pre>";
			print_r($x);
		}else{
			// return json string
			echo json_encode($x);
		}

	}catch(Exception $e){
		echo $e;
	}	
}
?>
