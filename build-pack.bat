@echo off  
echo ����Ŀ¼   %~dp0

if {%1} neq {} echo �汾��׺�� %~1

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

echo "������ɣ���ʼ����ԭ"
cd %~dp0
@REM dotnet restore --no-cache

echo ����ԭ��ɣ���ʼ������

set d=%~dp0src\Framework
set f=*.csproj

set last=1
:PACK
echo pack... %f% in %d%
for /r %d% %%a in (%f%) do (  
  if exist %%a (  
	  echo ��ʼ��� %%~na
	  @echo on
	  if {%1} neq {} (
	  dotnet pack %%a -c Release --version-suffix %~1 --output ..\..\..\Output
	  ) else (
	  echo ���%%a
	  dotnet pack %%a -c Release --output ..\..\..\Output
	  )
	  @echo off
 )  
)
if %last% == 3 goto quit


if %last% == 1 (
	echo ��ʼ��������
	set last=2
	set d=%~dp0src\Cloud
	set f=*.csproj
	goto PACK
)
:Tools
if %last% == 2 (
	echo ��ʼ�������
	set last=3
	set d=%~dp0src\Tooling
	set f=Sherlock.Framework.Modularity.Tools.Vs2017.csproj
	goto PACK
)


:quit 
