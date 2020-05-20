Import-AzureRmContext -Path "E:\Projects\Visual Studio 2019\StockExchangeAdvisor\UtilityAzure\PowerShellScripts\azureprofile.json" 
 
$PowerState = ((Get-AzureRmVM -Name Centos -ResourceGroupName WebServices -Status).Statuses[1]).code 
 
If ( $PowerState -contains "PowerState/running") 
{ 
   Write-Host "PowerState12: $PowerState" 
 
   Stop-AzureRmVM -Name Centos -ResourceGroupName WebServices -Force 
   $PowerState = ((Get-AzureRmVM -Name Centos -ResourceGroupName WebServices -Status).Statuses[1]).code 
} 
Write-Host "PowerState13: $PowerState"