del *.nupkg
xcopy ..\LICENSE bin ..\deploy\ /y
xcopy ..\README.md bin ..\deploy\ /y
NuGet.exe pack RestCake.nuspec -BasePath ..\deploy\RestCake\ -Verbosity detailed
