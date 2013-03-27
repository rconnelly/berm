Configure remote azure environment
	1. create subscription
		1.1 goto "Build\SetupParams\Output\Cloud\certificate"
		1.2 import "berm.azure.v2.cer" to cloud -> settings  

Prepare development environment
	1. download and install Windows Azure SDK for VS2012
	2. install Windows PowerShell cmdlets from "Tools\WindowsAzurePowerShell.3f.3f.3fnew.exe"
	3. in case of any deployment problems additionally install
		3.1 "Tools\WindowsAzureAuthoringTools-x64.msi" or "Tools\WindowsAzureAuthoringTools-x86.msi"
		3.2 "Tools\WindowsAzureTools.vs110.exe"
	4. import certificate
		6.1 goto "Build\SetupParams\Output\Cloud\certificate"
		6.2 import "berm.azure.v2.pfx" into CurrentUser Personal Location

Install application
	1. build application by running "Build\Build.Local.cmd"
	2. run "Build\Assembly\setup.cmd"

Tuning
	1. create new certificate (if we want to use different certificates for subscription and service - create different ones)
		1.1 upload ".cer" certificate to cloud -> settings
        1.2 update corresponding Config.Target with new thumbnail(s) 
        1.3 update ServiceConfiguration.Cloud.cscfg with new thumbnail
		1.4 import ".pfx" to CurrentUser Personal Location