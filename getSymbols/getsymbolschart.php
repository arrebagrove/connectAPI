<?php 
/*
s = SymbolName
array = 0  - show json format
array = 1  - show array format
*/

if (!empty($_GET['s'])) {
	$txt = htmlentities($_GET['s'], ENT_QUOTES, "UTF-8");	
	$arr = (int)$_GET['array'];
	getSymbolChart($txt,$arr);
}else{
	getSymbolChart();
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

function getSymbolChart($str = "GBPJPY", $array = 0){

	try{
		$m = (int)date('i');
		$h = (int)date('H');
		$d = (int)date('d');
		$month = (int)date('m');
		$y = (int)date('Y');
		$time = time();

		if (!empty($str)) {
			// mysql
			$db = Conn();				
			$res = $db->query("SELECT symbolName,lastAsk,lastBid,digits,description,time,from_unixtime(time) as data from symbolssmall WHERE symbolName = '$str' ORDER BY time DESC LIMIT 750");
			$x = $res->fetchAll(PDO::FETCH_ASSOC);
		}else{			
			die("Invalid instrument!");
		}

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

function showChart($x){
			// foreach
		foreach ($x as $v) {
			$sym = $v['symbolName'];
			$dig = $v['digits'];
			$desc = $v['description'];
			$bid = (float)$v['lastBid'];
			$ask = (float)$v['lastAsk'] . "<br>";
		}
}

?>
