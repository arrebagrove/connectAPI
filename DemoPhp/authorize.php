<?php
// redirect uri file example save token to file
error_reporting('E_ALL');
// App client id
$cID = '235_oC5GEbxWgmAAhcYiWzxmw1eu8l73phcKcXeaEKflqMYqwYq';
// App secret 
$cSecret = 'Yxh4wQRbIrGUfbGyiQ8G00gRJgHlHiT8kc1DFqaOui4PMVU';
// App redirect uri
$cUri = 'https://fxstar.eu/fx/connect.php';

// Authorize user redirect user to this links
// Allow only accounts positions 
$urlAccountsAuth = 'https://connect.spotware.com/apps/auth?client_id='.$cID.'&redirect_uri='.$cUri.'&scope=accounts';
// Allow Positions and Trading
$urlTradingAuth = 'https://connect.spotware.com/apps/auth?client_id='.$cID.'&redirect_uri='.$cUri.'&scope=trading';

// Example returning code after redirects
// https://fxstar.eu/fx/connect.php?code=094e3bf848909069e79605b7fc5f5adda15fd1e496473fb95d97a278022c07a45c97e83180b00d335f65b6

// GET Accounts connected to token
// https://api.spotware.com/connect/tradingaccounts?access_token=SdVYws4iBY4r3HQmaGg8J-CYNW7g2XeUzEy56H2QtUM

// GET Positions from account id
// https://api.spotware.com/connect/tradingaccounts/467809/positions?access_token=SdVYws4iBY4r3HQmaGg8J-CYNW7g2XeUzEy56H2QtUM

/*
// Trading
// Token to second account connect1@fxstar.eu 
If ok we got token in file, lets test it!Array ( [accessToken] => bMkBlg0567Z4p6h5oe97E_zgiY7S5jqgvKJMPkmg2KQ [tokenType] => bearer [expiresIn] => 2628000 [refreshToken] => Ge4MMu_w3lncqDNeyrJGLk8tXhMcP_Wr7AWKm1T8-2Y [errorCode] => [access_token] => bMkBlg0567Z4p6h5oe97E_zgiY7S5jqgvKJMPkmg2KQ [refresh_token] => Ge4MMu_w3lncqDNeyrJGLk8tXhMcP_Wr7AWKm1T8-2Y [expires_in] => 2628000 ) 
// Accounts
If ok we got token in file, lets test it!Array ( [accessToken] => M6rxbPkLpT1svnGOEsto49qIBkdEohSiF-Kidy7i_Ls [tokenType] => bearer [expiresIn] => 2628000 [refreshToken] => Qudqq0WqAL6atrU3hssAXTBl7MLs_86uKyl8t7haFB0 [errorCode] => [access_token] => M6rxbPkLpT1svnGOEsto49qIBkdEohSiF-Kidy7i_Ls [refresh_token] => Qudqq0WqAL6atrU3hssAXTBl7MLs_86uKyl8t7haFB0 [expires_in] => 2628000 ) 
*/
?>
<!DOCTYPE html>
<html>
<head>
	<title>Authorize account (Allow access to account)</title>
	<style type="text/css">
		.btn1, .btn2{float: left; width: auto; padding: 10px; background: #202033; color: #fff; margin: 1%; border-radius: 0px; text-decoration: none; font-family: Tahoma; width: 98%; box-sizing: border-box;}
		.btn2{background: #ff3d00}
		.btn1:hover, .btn2:hover{background: #0275d8; background: #009933}
		label{float: left; width: 100%; padding: 10px; color: #202033; font-weight: bold; font-family: Arial}
		.center{width: 350px; margin: 110px auto; border: 1px solid #202033; padding: 10px; overflow: hidden;}
	</style>
</head>
<body>
<div class="center">
<label>Allow access to cTrader account</label>
<a class="btn1" href="<?php echo $urlAccountsAuth; ?>">Allow Access Positions</a>
<a class="btn2" href="<?php echo $urlTradingAuth; ?>">Allow Access Trading</a>
</div>
</body>
</html>
