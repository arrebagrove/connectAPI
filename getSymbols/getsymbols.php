<?php 
// header('Content-Type: text/html; charset=utf-8');
// header("Content-Type: application/json;charset=utf-8");
// header('Access-Control-Allow-Origin: *');
// error_reporting('E_ALL');
// session_id() || session_start();

hourClosePrice();

// echo exec('hostname -f');
// echo "Works...".getenv('OPENSHIFT_PHP_LOG_DIR')." ".getenv('OPENSHIFT_REPO_DIR');
for ($i=0; $i < 28; $i++) { 
	//file_put_contents(getenv('OPENSHIFT_REPO_DIR').'www/loop.txt', time()."\r\n", FILE_APPEND | LOCK_EX));	
	// $curl = exec('curl https://api.spotware.com/connect/tradingaccounts/442711/symbols?access_token=sdfsdfsdfsdfsdfsdfs');
	
	$j = file_get_contents('https://api.spotware.com/connect/tradingaccounts/442711/symbols?access_token=ksajdlsajdljsadlsadsadsa');
	addSymbols($j);
	
	// file_put_contents('loop.txt', time()." ".$j."\r\n", FILE_APPEND | LOCK_EX);
	// file_put_contents(getenv('OPENSHIFT_PHP_LOG_DIR').'loop.txt', time()." ".$j."\r\n");	
	file_put_contents(getenv('OPENSHIFT_REPO_DIR').'www/symbols/symbols.json', $j);	
	usleep(2002);		
}

// PDO
function Conn(){
$connection = new PDO('mysql:host=127.12.19.10;port=3306;dbname=www;charset=utf8', 'User', 'Password');
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


function hourClosePrice(){
	try{
		$m = (int)date('i');
		$h = (int)date('H');
		$d = (int)date('d');
		$month = (int)date('m');
		$y = (int)date('Y');
		$time = time();

		$db = Conn();		
		//$res = $db->query("SELECT symbolName,lastAsk,lastBid,digits,description from symbolssmall WHERE hour=$h-1 AND day = $d AND month = $month");
		$res = $db->query("SELECT symbolName,lastAsk,lastBid,digits,description,time,from_unixtime(time) as data from symbolssmall WHERE hour=$h-1 AND day = $d AND month = $month AND year = $y");
		$x = $res->fetchAll(PDO::FETCH_ASSOC);
		// last hour close prices
		file_put_contents(getenv('OPENSHIFT_REPO_DIR').'www/symbols/symbols-hour.json', json_encode($x));

		foreach ($x as $v) {

			$sym = $v['symbolName'];
			$dig = $v['digits'];
			$desc = $v['description'];
			$bid = (float)$v['lastBid'];
			$ask = (float)$v['lastAsk'] . "<br>";
		}
	}catch(Exception $e){
		echo $e;
	}	
}

function getSymbol($search = "GBP"){
	try{
		$str = '%'.$search.'%';

		$m = date('i');
		$h = date('H');
		$d = date('d');
		$month = date('m');
		$y = date('Y');
		$time = time();


		$db = Conn();				
		$res = $db->query("SELECT symbolName,lastAsk,lastBid,digits,description from symbolssmall WHERE hour=$h AND day = $d AND month = $month AND year = $y AND symbolName LIKE '$str'");
		$x = $res->fetchAll();
		// last hour close prices
		file_put_contents(getenv('OPENSHIFT_REPO_DIR').'www/symbols/symbols-'.$search.'.json', json_encode($x));

		foreach ($x as $v) {

			$sym = $v['symbolName'];
			$dig = $v['digits'];
			$desc = $v['description'];
			$bid = (float)$v['lastBid'];
			$ask = (float)$v['lastAsk'] . "<br>";
		}
	}catch(Exception $e){
		echo $e;
	}	
}

function addSymbols($json){
	// https://api.spotware.com/connect/tradingaccounts/442711/symbols?access_token=dsfdsfdfdsfdsfdsfsdf'
	try{
		$db = Conn();
		$x = json_decode($json,true);
		// $z = $x['data'][0]['tickSize'];
		// echo rtrim(sprintf('%.20f', $z), '0');
		foreach ($x['data'] as $v) {
			// data
			$m = date('i');
			$h = date('H');
			$d = date('d');
			$month = date('m');
			$y = date('Y');
			$time = time();
			// symbol
			$sym = str_replace("#", "!", $v['symbolName']);
			$dig = $v['digits'];
			$pip = $v['pipPosition'];
			$base = $v['baseAsset'];
			$quote = $v['quoteAsset'];
			$desc = $v['description'];
			$bid = (float)$v['lastBid'];
			$ask = (float)$v['lastAsk'];
			// create database
			$database = 'CREATE TABLE IF NOT EXISTS `symbolssmall` (  `id` bigint(20) NOT NULL AUTO_INCREMENT,  `symbolName` varchar(20) DEFAULT NULL,  `digits` int(11) NOT NULL,  `pipPosition` int(11) NOT NULL,  `baseAsset` varchar(100) DEFAULT NULL,  `quoteAsset` varchar(100) DEFAULT NULL,  `description` varchar(100) DEFAULT NULL,  `lastBid` decimal(50,6),  `lastAsk` decimal(50,6), `typ` int(11) DEFAULT 1,  `minute` int(11) NOT NULL,  `hour` int(11) NOT NULL,  `day` int(11) NOT NULL,  `month` int(11) NOT NULL,  `year` int(11) NOT NULL,    `time` bigint(22) DEFAULT 0,  `uniquehour` varchar(250) DEFAULT NULL,  PRIMARY KEY (`id`),  UNIQUE KEY `uniquehour` (`symbolName`,`hour`,`day`,`month`,`year`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;';		

			// Create database
			$res = $db->query($database);
			// Insert
			$res = $db->query("INSERT INTO symbolssmall(symbolName,digits,pipPosition,baseAsset,quoteAsset,description,lastBid,lastAsk,minute,hour,day,month,year,time) VALUES('$sym',$dig,$pip,'$base','$quote','$desc',$bid,$ask,$m,$h,$d,$month,$y,$time) ON DUPLICATE KEY UPDATE lastBid=$bid, lastAsk=$ask");
		}
	}catch(Exception $e){
		echo $e;
	}
}

?>
