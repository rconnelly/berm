SETUP:
1. Prepare environment
    1.1 install "Tools\WindowsAzureTools.vs110.exe"
    1.2 install "Tools\WindowsAzureAuthoringTools-x64.msi" or "Tools\WindowsAzureAuthoringTools-x86.msi"
    1.3 install Windows PowerShell cmdlets from "Tools\WindowsAzurePowerShell.3f.3f.3fnew.exe"
    1.4 import "Cloud\certificate\berm.azure.pfx" into CurrentUser Personal Location
2. run "setup.cmd"

**********************************************************************************************************
NOTE:
- useful info: https://www.windowsazure.com/en-us/develop/net/common-tasks/continuous-delivery/
- run ps_execute.cmd [full path to ps1 file] #full path is required for ps1 file even if it's in the same directory

**********************************************************************************************************