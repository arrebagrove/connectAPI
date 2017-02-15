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

// how to use
echo $res = file_get_contents($cGetUserAccessToken);	
print_r(parseHeaders($http_response_header));
  
