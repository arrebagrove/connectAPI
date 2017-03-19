### C# Windows service
### Create console project
Add references to 
using System.ServiceProcess; 
using System.Configuration.Install;

## Install service
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe /i c:\WindowsService.exe

## Uninstall service
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe /u c:\WindowsService.exe

### Links
http://www.csharp-examples.net/install-net-service/
https://msdn.microsoft.com/pl-pl/library/zt39148a(v=vs.110).aspx
https://msdn.microsoft.com/en-us/library/zt39148a(v=vs.110).aspx
https://www.codeproject.com/Articles/14353/Creating-a-Basic-Windows-Service-in-C
https://www.youtube.com/watch?v=DubiAulO5eI
