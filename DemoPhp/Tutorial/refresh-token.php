<?php
// redirect uri file example save token to file
error_reporting('E_ALL');
// App client id
$cID = '235_oC5GEbxWgmAAhcYiWzxmw1eu8l73phcKcXeaEKflqMYqwYqHcB';
// App secret 
$cSecret = 'Yxh4wQRbIrGUfbGyiQ8G00gRJgHlHiT8kc1DFqaOui4PMVULaz';
// App redirect uri
$cUri = 'https://fxstar.eu/fx/connect.php';

// refreshtoken for token which you need or want refresh
$refreshToken = 'TCzwxEH7d3M45CNhY4c2d4xQSQx8Mn14MRQOEW_qewA';

// url
$refresh = "https://connect.spotware.com/apps/token?grant_type=refresh_token&refresh_token=".$refreshToken."&redirect_uri=".$redirectUri."&client_id=".$cID."&client_secret=".$cSecret;
// get data from url
$res = file_get_contents($refresh);	
// save to file
file_put_contents('token-'.time().'.secret', $res); 
// convert from json to array
$tokens = json_decode($res, true);
// display array in browser
echo "New token: <br>";
print_r($tokens);
?>
