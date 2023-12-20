@echo off

Rem -------------------------------------------------
Rem Create Template for the Next Day
Rem -------------------------------------------------

powershell -ExecutionPolicy Bypass -File %cd%/Calendar/create-template.ps1 %cd% 2023 %1 c-sharp