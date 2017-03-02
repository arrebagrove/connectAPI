<?php 
// header('Content-Type: text/html; charset=utf-8');
// header("Content-Type: application/json;charset=utf-8");
// header('Access-Control-Allow-Origin: *');
// error_reporting('E_ALL');
// session_id() || session_start();
// echo exec('hostname -f');
// echo "Works...".getenv('OPENSHIFT_PHP_LOG_DIR')." ".getenv('OPENSHIFT_REPO_DIR');

// HOW TO USE
// https://example.com/getsymbolswith.php?s=GBPJPY,EURUSD
// https://example.com/getsymbolswith.php?s=GBP
// ======================================================

if (!empty($_GET['s'])) {
	$txt = htmlentities($_GET['s'], ENT_QUOTES, "UTF-8");
	getSymbol($txt);
}else{
	getSymbol();
}

// PDO
function Conn(){
$connection = new PDO('mysql:host=127.0.0.1;port=3306;dbname=www;charset=utf8', 'user', 'pass');
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

function getSymbol($search = "GBP", $save = false){

	try{
		$str = '%'.$search.'%';

		$m = (int)date('i');
		$h = (int)date('H');
		$d = (int)date('d');
		$month = (int)date('m');
		$y = (int)date('Y');
		$time = time();

		if (count(explode(',', $search)) > 1) {
			$exp = explode(',', $search);			
			$or = "";
			foreach ($exp as $v) {
				$v = '%'.$v.'%';
				$or .= " OR hour=$h AND day = $d AND month = $month AND year = $y AND symbolName LIKE '$v'";
			}
			// mysql
			$db = Conn();				
			$res = $db->query("SELECT symbolName,lastAsk,lastBid,digits,description,time,from_unixtime(time) as data from symbolssmall WHERE hour=$h AND day = $d AND month = $month AND year = $y AND symbolName = '#'". $or);
			$x = $res->fetchAll();			
		}else{
			// mysql
			$db = Conn();				
			$res = $db->query("SELECT symbolName,lastAsk,lastBid,digits,description,time,from_unixtime(time) as data from symbolssmall WHERE hour=$h AND day = $d AND month = $month AND year = $y AND symbolName LIKE '$str'");
			$x = $res->fetchAll();
		}

		// save to file if needed
		if ($save) {			
			file_put_contents(getenv('OPENSHIFT_REPO_DIR').'www/symbols/symbols-'.$search.'.json', json_encode($x));
		}		

		// foreach
		foreach ($x as $v) {
			$sym = $v['symbolName'];
			$dig = $v['digits'];
			$desc = $v['description'];
			$bid = (float)$v['lastBid'];
			$ask = (float)$v['lastAsk'] . "<br>";
		}

		// return json string
		echo json_encode($x);

	}catch(Exception $e){
		echo $e;
	}	
}

?>
