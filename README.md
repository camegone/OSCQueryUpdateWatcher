# OSCQueryUpdateWatcher

 Codes to detect change of VRCOSCQuery tree and output them

## How to run

 This code references VRCOSCQuery NuGet package that not published yet, so you need to make locally.

### Local build and installation the NuGet Package

If you have not created local NuGet source, do

```
dotnet nuget add <Path to store local sources> -n local_store
```

In the directory you want to build, run

```
git pull https://github.com/vrchat-community/vrc-oscquery-lib.git
cd vrc-oscquery-lib
dotnet pack -c Release .\
```

Then, push the package to your local NuGet source

```
dotnet nuget push .\vrc-oscquery-lib\vrc-oscquery-lib\bin\Release\vrc-oscquery-lib.1.0.0.nupkg
```
