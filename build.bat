@echo off

Rem -------------------------------------------------
Rem Create Calendar Tiles and Update README.md
Rem -------------------------------------------------

CALL :ClearFolder %cd%/Calendar/Tiles/
powershell -ExecutionPolicy Bypass -File %cd%/Calendar/create-tiles.ps1 %cd%/README-raw.md %cd%/README.md

Rem -------------------------------------------------
Rem Copy necessary files into docs folder
Rem -------------------------------------------------

copy /y .\2021\src\test\javascript\06\index.html .\docs\2021-06.html
copy /y .\2021\src\test\javascript\06\resources\js\* .\docs\resources\js\

copy /y .\2021\src\test\javascript\11\index.html .\docs\2021-11.html
copy /y .\2021\src\test\javascript\11\resources\js\* .\docs\resources\js\

EXIT /B %ERRORLEVEL%

Rem -------------------------------------------------
Rem Delete and re-create folder to clear all old data
Rem -------------------------------------------------
:ClearFolder
set folder=%~1
if exist "%folder%" rmdir /s /q "%folder%"
if not exist "%folder%" mkdir "%folder%"
EXIT /B 0