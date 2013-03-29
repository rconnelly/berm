$subscriptionName = "${Azure_subName}"
$subscriptionId = "${Azure_subID}"

$deploymentThumbprint = "${Azure_thumbprint}"
$deploymentLocation = "West Europe"
$deploymentSlot = "${Azure_slot}"
$deploymentLabel = "v${ProductInformationalVersion}"

$defaultStorageName = "${Azure_storage}"

$defaultDatabaseName = "${Azure_sqldb}"
$defaultDatabaseUser = "bermuser"
$defaultDatabasePassword = "RuMbA52eTAs*"

$defaultServiceName = "${Azure_service}"

$waitWhenStarted = ("${Azure_waitRunning}" -eq "true")
$swapWhenStaging = ("${Azure_swap}" -ne "false")

$rootpath = Split-Path -parent $MyInvocation.MyCommand.Path
$contentPath = "$rootpath\Cloud"
$migratorPath = "$rootpath\Database"
$deploymentPackage = "$contentPath\Quad.Berm.Cloud.cspkg"
$deploymentPackageConfig = "$contentPath\ServiceConfiguration.${Azure_profile}.cscfg"
$defaultDatabaseMigratorAssembly = "$migratorPath\Quad.Berm.Migrations.dll";
$defaultCertificatePath = "$contentPath\certificate\berm.azure.pfx"
$defaultCertificatePassword = "1"
