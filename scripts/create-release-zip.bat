call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat"

REM build RestCake
REM msbuild ..\src\RestCake.sln

REM zip up the bin folder
.\7-zip\7za.exe a -r -tzip ..\RestCake-MAJ-MIN-REV.zip ..\src\RestCake\bin
