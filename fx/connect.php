<?php
// cTrader connect api retrn Uri
error_reporting('E_ALL');
// App client id
$cID = '';
// App secret 
$cSecret = '';
// Your App return url 
$folder = 'fx';
$cUri = 'https://fxstar.eu/'.$folder.'/connect.php';

if (isset($_GET['code'])) {
	// mysql connect include
	require('pdo.php');
	// connect database
	$db = Conn();
	// Secure input data
	$cReceive = htmlentities($_GET['code'], ENT_QUOTES, 'utf-8');
	//$cReceive = preg_replace("/[^a-zA-Z0-9_-]/", "", $_GET['code']);

	$cGetUserAccessToken = 'https://connect.spotware.com/apps/token?grant_type=authorization_code&code='.$cReceive.'&redirect_uri='.$cUri.'&client_id='.$cID.'&client_secret='.$cSecret;
	$res = file_get_contents($cGetUserAccessToken);	

	if ($res != false) {
		// save to folder
		file_put_contents('token/token-'.time().'.secret', $res);		
		$tokens = json_decode($res, true);

		$accessToken = $tokens['accessToken'];
		$refreshToken = $tokens['refreshToken'];
		$expiresIn = $tokens['expiresIn'];
		$errorCode = $tokens['errorCode'];
		$tokenType = $tokens['tokenType'];
		// refresh token timestamp
		$refreshTime = time() + $expiresIn - (60 * 60 * 6);
		$ip = IP();
		$time= time();
		if (empty($errorCode)) {			
			try{
				$res = $db->query("INSERT INTO accessTokens(code,accessToken,refreshToken,tokenType,expiresIn,refreshTime,ip,time) VALUES('$cReceive', '$accessToken','$refreshToken','$tokenType','$expiresIn',$refreshTime,'$ip', $time)");
				if ($db->lastInsertId() > 0) {
					//echo "Thank You, your account has been added to our aplication.";
					echo '
					<style>
					.alert-dismissable, .alert-dismissible {
					    padding-right: 35px;
					}
					.alert {
					    padding: 15px;
					    margin-bottom: 20px;
					    border: 1px solid #d6e9c6;
					    border-radius: 4px;
					}
					.alert-success {
					    color: #3c763d;
					    background-color: #dff0d8;
					    border-color: #d6e9c6;
					}
					.alert-warning {
					    color: #8a6d3b;
					    background-color: #fcf8e3;
					    border-color: #faebcc;
					}		
					</style>
					<div class="alert alert-success">
  						<strong>Success!</strong> Thank You, your account has been added to our aplication.
  					</div>
					';
				}else{
					//echo "Upss...error! Sorry, try again leater.";
					echo '
					<style>
					.alert-dismissable, .alert-dismissible {
					    padding-right: 35px;
					}
					.alert {
					    padding: 15px;
					    margin-bottom: 20px;
					    border: 1px solid #d6e9c6;
					    border-radius: 4px;
					}
					.alert-success {
					    color: #3c763d;
					    background-color: #dff0d8;
					    border-color: #d6e9c6;
					}
					.alert-warning {
					    color: #8a6d3b;
					    background-color: #fcf8e3;
					    border-color: #faebcc;
					}		
					</style>
					<div class="alert alert-warning">
  						<strong>Warning!</strong> Oj...error! Sorry something went wrong. Try again leater. 
  					</div>
					';					
				}
			} catch (Exception $e) {
				echo $e->getMessage();
				echo $e->getCode();
				// return 0;
			}
			//return $this->db->lastInsertId();
		}else{
			echo '<h1 style="color: #f00; width: 100%; text-align: center">[ACCESS_DENIED] <br> Make sure the credentials are valid</h1>';
		}
	}

}else{
	echo '<h1 style="color: #000; width: 100%; text-align: center">[ERROR_INPUT_DATA]</h1>';
}

// REFRESH TOKEN
// $refresh = "https://sandbox-connect.spotware.com/apps/token?grant_type=refresh_token&refresh_token={your Refresh Token previously received with Access Token}&redirect_uri={your Redirection URI}&client_id={your Partner's Public Client ID}&client_secret={your Partner's Client Secret}";

//$json = '{"accessToken":"THfMH7jgo7PMKOp0_3dQu1kLI8WB8eU5Fq00K_mDYLs","tokenType":"bearer","expiresIn":2628000,"refreshToken":"M8GEUvt4G2YQmotoWwRLgT1Hg0mi2qOO-v8qU0mxmVg","errorCode":null,"access_token":"THfMH7jgo7PMKOp0_3dQu1kLI8WB8eU5Fq00K_mDYLs","refresh_token":"M8GEUvt4G2YQmotoWwRLgT1Hg0mi2qOO-v8qU0mxmVg","expires_in":2628000}';
//$d = json_decode($json, true);
//echo "<pre>";
//print_r($d);
// die();
?>
