<?php
$yesterday = 188.666;
$today = 177.666;

// różnica kursowa w procentach dla wzrosu ile procent w plus, dla spadku ile procent trzeba odrobić
kurs($today,$yesterday);

// różnica kursowa w procentach dla wzrosu ile procent w plus, dla spadku ile procent trzeba odrobić
function kurs($today,$yesterday){
	if ($today>$yesterday) {
		echo "+".$p = ($today/$yesterday)."%";		
		// $yesterday * $p;		
	}else{
		echo "-".$p = ($yesterday/$today)."%";		
		// echo $today * $p;		
	}
}
?>
