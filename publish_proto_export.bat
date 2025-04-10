@echo off
set OUTPUT=bin
set PROJECT_DIR=..\GameServer\Protobuf\script
set PROJECT_APP=%PROJECT_DIR%\ProtoExport.exe

echo Publishing ProtoExport...
dotnet publish ProtoExport/ProtoExport.csproj -c Release -r win-x64 --self-contained false -o %OUTPUT%
if %ERRORLEVEL% neq 0 (
    pause
    goto :EOF
)

if not exist "%PROJECT_DIR%" goto :EOF

echo Copying ProtoExport.exe to destination...
copy /y "%OUTPUT%\ProtoExport.exe" "%PROJECT_APP%"
if %ERRORLEVEL% neq 0 pause
