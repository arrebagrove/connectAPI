<?php
$yesterday = 0.7675;
$today = 0.7596;

// różnica kursowa w procentach
kurs($today,$yesterday);

// różnica kursowa w procentach
function kurs($today,$yesterday){
	return $delta = (($today - $yesterday) / $yesterday)*100;
}
?>
