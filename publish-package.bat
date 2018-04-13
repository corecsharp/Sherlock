SET key=oy2jhwrkgo4db4u4bpealoeiejyqfifublf2gydfxtidma 
SET version=3.0.1
SET source=https://api.nuget.org/v3/index.json



dotnet nuget push Output\Sherlock.Framework.%version%.nupkg -k %key% -s %source%
dotnet nuget push Output\Sherlock.Framework.Caching.Redis.%version%.nupkg -k %key%a -s %source%
dotnet nuget push Output\Sherlock.Framework.Data.Dapper.%version%.nupkg -k %key% -s %source%
dotnet nuget push Output\Sherlock.Framework.Scheduling.%version%.nupkg -k %key% -s %source%
dotnet nuget push Output\Sherlock.Framework.Web.%version%.nupkg -k %key% -s %source%
dotnet nuget push Output\Sherlock.Framework.Web.Dapper.%version%.nupkg -k %key% -s %source%
dotnet nuget push Output\Sherlock.Framework.Web.FluentValidation.%version%.nupkg -k %key% -s %source%
dotnet nuget push Output\Sherlock.Framework.Modularity.Tools.Vs2017.%version%.nupkg -k %key% -s %source%
cmd /k echo.

