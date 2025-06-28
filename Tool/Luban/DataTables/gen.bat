set BAT_DIR=%~dp0
for %%I in ("%BAT_DIR%\..\..\..") do set WORKSPACE=%%~fI
set LUBAN_DLL="%WORKSPACE%\Tool\Luban\DataTables\Tools\Luban\Luban.dll"
set CONF_ROOT="%WORKSPACE%\Tool\Luban\DataTables"

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
    -c cs-simple-json ^
    --conf "%CONF_ROOT%\luban.conf" ^
    -x outputCodeDir="%WORKSPACE%\Unity\Assets/Scripts/Config/Gen/Luban" ^
    -x outputDataDir="%WORKSPACE%\Tool\Luban\DataTables\output"

pause