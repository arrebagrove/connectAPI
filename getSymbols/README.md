# Connect Api Accounts get Symbols, currency, metals, stocks

### getsymbols.php (server cron file save data to database and symbols.json)
get all symbols for account from connect server - script create database on start.

### getsymbolswith.php?s=GBP,USD 
get all symbols with text from our database.

### getsymbolschart.php?s=GBPJPY&array=0 or getsymbolschart.php?s=GBPJPY&array=1
get symbol chart data (json or array format)

### getsymbolsall.php or getsymbolsall.php?array=1
get all symbols json format (or get in array format)

### symbols.json (getsymbols.php save data to this file)
file contain all symbols with current prices - refresh interval 2 seconds

### symbols-hour.json (getsymbols.php save data to this file)
file contain all symbols with previous hour prices - refresh interval 2 seconds
