@echo off  
echo 工作目录   %~dp0

if {%1} neq {} echo 版本后缀： %~1

:clear
set WHAT_SHOULD_BE_DELETED=bin 
set IN_LOOP=no 

:del
for /r %~dp0 %%a in (!WHAT_SHOULD_BE_DELETED!) do (  
  if exist %%a (   
  rd /s /q "%%a"  
 )  
)
if %IN_LOOP%  == yes goto build

set WHAT_SHOULD_BE_DELETED=obj 
set IN_LOOP=yes 
goto del


:build

echo "清理完成，开始包还原"
cd %~dp0
@REM dotnet restore --no-cache

echo 包还原完成，开始编译打包

set d=%~dp0src\Framework
set f=*.csproj

set last=1
:PACK
echo pack... %f% in %d%
for /r %d% %%a in (%f%) do (  
  if exist %%a (  
	  echo 开始打包 %%~na
	  @echo on
	  if {%1} neq {} (
	  dotnet pack %%a -c Release --version-suffix %~1 --output ..\..\..\Output
	  ) else (
	  echo 打包%%a
	  dotnet pack %%a -c Release --output ..\..\..\Output
	  )
	  @echo off
 )  
)
if %last% == 3 goto quit


if %last% == 1 (
	echo 开始打包云组件
	set last=2
	set d=%~dp0src\Cloud
	set f=*.csproj
	goto PACK
)
:Tools
if %last% == 2 (
	echo 开始打包工具
	set last=3
	set d=%~dp0src\Tooling
	set f=Sherlock.Framework.Modularity.Tools.Vs2017.csproj
	goto PACK
)


:quit 
