## Install from visual studio Tools > NuGet Package manager > package Manager Console
PM> Install-Package Newtonsoft.Json 

dynamic results = JsonConvert.DeserializeObject<dynamic>(json); 

or ClassName results = JsonConvert.DeserializeObject<ClassName>(json);

var id = results.Id;

var name= results.Name;


## Download and install Mysql Connector/.NET from:
https://dev.mysql.com/downloads/connector/net/6.9.html

And add as references to project

## Instal Json
PM> Install-Package microsoft-web-helpers

#### How use
using System.Web.Helpers.Json;

dynamic data = Json.Decode(json);
