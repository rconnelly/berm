@echo off
c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild ..\Source\Quad.Berm.Migrations\Quad.Berm.Migrations.csproj /nologo /v:m
        if %ERRORLEVEL% NEQ 0 (
            goto:end
        )

PUSHD ..\Source\Quad.Berm.Migrations\bin\
    Migrate -db SqlServer2008 -conn "Data Source=(local);Initial Catalog=bermdb;Integrated Security=True" -a Quad.Berm.Migrations.dll
POPD
:end