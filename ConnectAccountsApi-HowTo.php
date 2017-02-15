<?php
// Spotware Connect API Accounts
// 1] https://connect.spotware.com/docs/available-resources/playground
// 2] https://connect.spotware.com/docs/api-reference/accounts-api
// User must login (authhorize) first on this page with his cTID with connected cTrader accounts (Signin in cTrader with cTID): 
$cLoginFirst = 'https://connect.spotware.com/auth';
// after login open this link in new browser tab and click allow button (get user access token)
$cRedirectUserTo = 'https://connect.spotware.com/apps/auth?client_id=203_2hosJUozrOVxXNWyvAh8ftmp6Ug3vjmTP1SPBg3ytinkoiF4Te&redirect_uri=https://fxstar.eu/forex/connect.php&scope=accounts';
// REDIRECT after redirect to your ($redirectRequest) page !!! script !!! you can get access code in json format
$redirectRequest = 'https://fxstar.eu/forex/connect.php?code=asdsadsaa38e69cdee9db276sdfsdf34sfdsf';
// you need send GET code to this url file_get_contents or curl in php
$cGetUserAccessToken = 'https://connect.spotware.com/apps/token?grant_type=authorization_code&code='.$cReceive.'&redirect_uri='.$cUri.'&client_id='.$cID.'&client_secret='.$cSecret;
// RESPONSE then you got access token from spotware cTrader user account
{"accessToken":"a_dfsfsdfsdfdsLotTNWTSM","tokenType":"bearer","expiresIn":2628000,"refreshToken":"dfgdgdTTYRYymT1mFaQ","errorCode":null,"access_token":"a_dfsfsdfsdfdsLotTNWTSM","refresh_token":"dsfsdfsNVVoFwHymT1mFaQ","expires_in":2628000}
// Now you can get user account info with access token more https://connect.spotware.com/docs/api-reference/accounts-api
$requestExample = 'https://api.spotware.com/connect/tradingaccounts?access_token=a_dfsfsdfsdfdsLotTNWTSM';
// RESPONSE
{"data":[{"accountId":424862,"accountNumber":3157245,"live":false,"brokerName":"Spotware","brokerTitle":"Spotware","brokerCode":414,"depositCurrency":"USD","traderRegistrationTimestamp":1486632491403,"traderAccountType":"HEDGED","leverage":500,"leverageInCents":50000,"balance":100000000,"deleted":false,"accountStatus":"ACTIVE","swapFree":false},{"accountId":424860,"accountNumber":3157244,"live":false,"brokerName":"Spotware","brokerTitle":"Spotware","brokerCode":414,"depositCurrency":"EUR","traderRegistrationTimestamp":1486632406062,"traderAccountType":"HEDGED","leverage":100,"leverageInCents":10000,"balance":100000,"deleted":false,"accountStatus":"ACTIVE","swapFree":false}]}
// Now you can get accounts positions
$accountPositions = 'https://api.spotware.com/connect/tradingaccounts/424862/positions?access_token=a_dfsfsdfsdfdsLotTNWTSM';
// RESPONSE in json format
{"data":[{"positionId":7980418,"entryTimestamp":1487151223210,"utcLastUpdateTimestamp":1487151223210,"symbolName":"GBPUSD","tradeSide":"BUY","entryPrice":1.2428,"volume":100000,"stopLoss":null,"takeProfit":null,"profit":-62,"profitInPips":-6.6,"commission":-3,"marginRate":1.17765,"swap":0,"currentPrice":1.24214,"comment":null,"channel":"cAlgo","label":null},{"positionId":7980415,"entryTimestamp":1487101255700,"utcLastUpdateTimestamp":1487105959350,"symbolName":"GBPUSD","tradeSide":"SELL","entryPrice":1.24706,"volume":100000,"stopLoss":null,"takeProfit":null,"profit":460,"profitInPips":48.6,"commission":-3,"marginRate":1.17934,"swap":-14,"currentPrice":1.2422,"comment":null,"channel":"cAlgo","label":null},{"positionId":7980388,"entryTimestamp":1487100422974,"utcLastUpdateTimestamp":1487105954461,"symbolName":"GBPUSD","tradeSide":"BUY","entryPrice":1.24734,"volume":100000,"stopLoss":null,"takeProfit":null,"profit":-492,"profitInPips":-52.0,"commission":-3,"marginRate":1.17947,"swap":-16,"currentPrice":1.24214,"comment":null,"channel":"cAlgo","label":null},{"positionId":7979855,"entryTimestamp":1487089977549,"utcLastUpdateTimestamp":1487105944730,"symbolName":"GBPUSD","tradeSide":"BUY","entryPrice":1.24689,"volume":100000,"stopLoss":null,"takeProfit":null,"profit":-449,"profitInPips":-47.5,"commission":-3,"marginRate":1.17881,"swap":-16,"currentPrice":1.24214,"comment":null,"channel":"cAlgo","label":null},{"positionId":7975348,"entryTimestamp":1487009298935,"utcLastUpdateTimestamp":1487105966658,"symbolName":"EURUSD","tradeSide":"BUY","entryPrice":1.05986,"volume":100000,"stopLoss":null,"takeProfit":null,"profit":-404,"profitInPips":-42.7,"commission":-1,"marginRate":1.0,"swap":-46,"currentPrice":1.05559,"comment":null,"channel":"cAlgo","label":null}]}
?>
