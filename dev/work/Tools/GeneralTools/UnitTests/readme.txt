
Unit Test's aus 64 Bit Programmen mit 32 Bit SAP .NET Modulen ("ErpConnect35.dll")

Matthias Jenzen, 21.08.2013 



Um 32 Bit DLL's (wie "ErpConnect35.dll") in den Unit Tests erfolgreich verwenden zu k�nnen 
muss das "32BIT" Flag in den entsprechenden Unit-Test Utilities mit dem "corflags" utility gesetzt werden

Zuerst, �ffnet das Visual Studio Command Prompt:  
(On the taskbar click Start, click All Programs, click Visual Studio, click Visual Studio Tools, and then click Visual Studio Command Prompt) 



�ber das Visual Studio Command Prompt die Aktionen 1 + 2 durchf�hren:




1. F�r das externe Unit Test Programm "NUnit.exe":

1.a) corflags utility ausf�hren auf NUnit.exe

=> corflags "C:\Program Files (x86)\NUnit 2.6.2\bin\nunit.exe" /32BIT+ /Force




2. F�r die Visual Studio integrierten Unit Tests mit Resharper und NUnit:


2.a) Installiert das NUnit plugin f�r Visual Studio:

=> Men� "Extras" / "Erweiterungs-Manager", sucht nach "Visual Nunit 2010", und dieses Plugin dann installieren


2.b) Im Visual Studio Command Prompt in folgendes Verzeichnis wechseln:

=> cd "\Program Files (x86)\JetBrains\ReSharper\v7.1\Bin"


2.c) corflags utility ausf�hren auf den "Resharper TaskRunner"

=> corflags JetBrains.ReSharper.TaskRunner.CLR4.MSIL.exe /32BIT+ /Force



