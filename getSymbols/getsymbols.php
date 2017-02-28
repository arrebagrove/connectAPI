<?php 
// header('Content-Type: text/html; charset=utf-8');
// header("Content-Type: application/json;charset=utf-8");
// header('Access-Control-Allow-Origin: *');
// error_reporting('E_ALL');
// session_id() || session_start();

echo exec('hostname -f') . " ";
echo "Works... ".getenv('OPENSHIFT_PHP_LOG_DIR')." ".getenv('OPENSHIFT_REPO_DIR');
for ($i=0; $i < 28; $i++) { 
	//file_put_contents(getenv('OPENSHIFT_REPO_DIR').'www/loop.txt', time()."\r\n", FILE_APPEND | LOCK_EX));		
	// access_token
  $j = file_get_contents('https://api.spotware.com/connect/tradingaccounts/431708/symbols?access_token=hjsdhfjkhsdjkfhskdjfhksdj');
	addSymbols($j);
	//file_put_contents('loop.txt', time()." ".$j."\r\n", FILE_APPEND | LOCK_EX);
	file_put_contents(getenv('OPENSHIFT_PHP_LOG_DIR').'loop.txt', time()." ".$j."\r\n");	
	usleep(2002);	
	echo $i;
}

// PDO
function Conn(){
$connection = new PDO('mysql:host=127.12.109.130;port=3306;dbname=www;charset=utf8', 'username', 'password');
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

function addSymbols($json){
	// https://api.spotware.com/connect/tradingaccounts/431708/symbols?access_token=jalkdjaskdjlasjdlsjadljasldjasdjlasjdla'
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
			$sym = $v['symbolName'];
			$dig = $v['digits'];
			$pip = $v['pipPosition'];
			$base = $v['baseAsset'];
			$quote = $v['quoteAsset'];
			$desc = $v['description'];
			$bid = (float)$v['lastBid'];
			$ask = (float)$v['lastAsk'];
			// create database
			$database = 'CREATE TABLE IF NOT EXISTS `symbolssmall` (  `id` bigint(20) NOT NULL AUTO_INCREMENT,  `symbolName` varchar(20) DEFAULT NULL,  `digits` int(11) NOT NULL,  `pipPosition` int(11) NOT NULL,  `baseAsset` varchar(100) DEFAULT NULL,  `quoteAsset` varchar(100) DEFAULT NULL,  `description` varchar(100) DEFAULT NULL,  `lastBid` decimal(10,6),  `lastAsk` decimal(10,6),  `minute` int(11) NOT NULL,  `hour` int(11) NOT NULL,  `day` int(11) NOT NULL,  `month` int(11) NOT NULL,  `year` int(11) NOT NULL,    `time` bigint(22) DEFAULT 0,  `unique` varchar(250) DEFAULT NULL,  PRIMARY KEY (`id`),  UNIQUE KEY `unique` (`symbolName`,`hour`,`day`,`month`,`year`)) ENGINE=InnoDB DEFAULT CHARSET=utf8;';		
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
