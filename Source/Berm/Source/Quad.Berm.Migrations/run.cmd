@echo off
Migrate -db SqlServer2008 -conn "Data Source=(local);Initial Catalog=bermdb;Integrated Security=True;Application Name=Quad.Berm.Migrations" -a Quad.Berm.Migrations.dll

