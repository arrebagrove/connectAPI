<?php
// CURL GET SSL force cert or how add cacert.pem http://stackoverflow.com/questions/4372710/php-curl-https
function curl($url){
	$ch = curl_init();
	curl_setopt($ch, CURLOPT_URL, $url);
	curl_setopt($ch, CURLOPT_HEADER, 0);
	curl_setopt($ch,CURLOPT_SSL_VERIFYPEER, 0);
	curl_setopt($ch, CURLOPT_FOLLOWLOCATION, 1);
	curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
	$out = curl_exec($ch);
	// Check HTTP status code
	if (!curl_errno($ch)) {
	  switch ($http_code = curl_getinfo($ch, CURLINFO_HTTP_CODE)) {
	    case 200:  # OK
	      break;
	    default:
	      //return $out = '['.$http_code.']';
              return '[ERROR]';
	  }
	}
	curl_close($ch);
	return $out;
}
// how to use
echo curl('https://fxstar.eu');
?>

<?php
function parseHeaders( $headers )
{
    $head = array();
    foreach( $headers as $k=>$v )
    {
        $t = explode( ':', $v, 2 );
        if( isset( $t[1] ) )
            $head[ trim($t[0]) ] = trim( $t[1] );
        else
        {
            $head[] = $v;
            if( preg_match( "#HTTP/[0-9\.]+\s+([0-9]+)#",$v, $out ) )
                $head['reponse_code'] = intval($out[1]);
        }
    }
    return $head;
}

// Spotware Connect API 
// User must login first on this page: 
$cLoginFirst = 'https://connect.spotware.com/auth';
// after login open this link (get user access token)
$cRedirectUserTo = 'https://connect.spotware.com/apps/auth?client_id=203_2hosJUozrOVxXNWyvAh8ftmp6Ug3vjmTP1SPBg3ytinkoiF4Te&redirect_uri=https://fxstar.eu/forex/connect.php&scope=accounts';
// after redirect to this !!! script !!! you can get access code in json format
$cGetUserAccessToken = 'https://connect.spotware.com/apps/token?grant_type=authorization_code&code='.$cReceive.'&redirect_uri='.$cUri.'&client_id='.$cID.'&client_secret='.$cSecret;

// how to use
echo $res = file_get_contents($cGetUserAccessToken);	
print_r(parseHeaders($http_response_header));

// or
$res = file_get_contents($cGetUserAccessToken);	
$rescode = parseHeaders($http_response_header)['reponse_code'];
if ($rescode == 200) {
      echo $res;
}else{
      echo "['Spotware_page_error']";
}
