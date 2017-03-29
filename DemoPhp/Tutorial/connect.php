<?php
// redirect uri file example save token to file
error_reporting('E_ALL');
// App client id
$cID = '235_oC5GEbxWgmAAhcYiWzxmw1eu8l73phcKcXeaEKflqMYqwYqHcB';
// App secret 
$cSecret = 'Yxh4wQRbIrGUfbGyiQ8G00gRJgHlHiT8kc1DFqaOui4PMVULaz';
// App redirect uri
$cUri = 'https://fxstar.eu/fx/connect.php';
 
if (isset($_GET['code'])) {
 
    // Secure input data
    $cReceive = htmlentities($_GET['code'], ENT_QUOTES, 'utf-8');
    //$cReceive = preg_replace("/[^a-zA-Z0-9_-]/", "", $_GET['code']);
 
    $cGetUserAccessToken = 'https://connect.spotware.com/apps/token?grant_type=authorization_code&code='.$cReceive.'&redirect_uri='.$cUri.'&client_id='.$cID.'&client_secret='.$cSecret;
    // get response
    $res = file_get_contents($cGetUserAccessToken); 
 
    if ($res != false) {
        // save to file
        file_put_contents('token-'.time().'.secret', $res);     
        $tokens = json_decode($res, true);
        echo "If ok we got token in file, lets test it!";
        print_r($tokens);

    }
 
}else{
    echo '<h1 style="color: #000; width: 100%; text-align: center">[ERROR_INPUT_DATA]</h1>';
}
