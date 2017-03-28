<?php
// redirect uri file example save token to file
error_reporting('E_ALL');
// App client id
$cID = '235_oC5GEbxWgmAAhcYiWzxmw1eu8l73phcKcXeaEKflqMYqwYq';
// App secret 
$cSecret = 'Yxh4wQRbIrGUfbGyiQ8G00gRJgHlHiT8kc1DFqaOui4PMVU';
// App redirect uri
$cUri = 'https://fxstar.eu/fx/connect.php';

// refreshtoken for token which you need or want refresh
$refreshToken = '';

// url
$refresh = "https://connect.spotware.com/apps/token?grant_type=refresh_token&refresh_token=".$refreshToken."&redirect_uri=".$redirectUri."&client_id=".$cID."&client_secret=".$cSecret;
$res = file_get_contents($refresh);	
echo "New token: <br>";
print_r($res);
?>
