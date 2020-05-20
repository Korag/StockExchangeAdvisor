Import-AzureRmContext -Path "E:\Projects\Visual Studio 2019\StockExchangeAdvisor\UtilityAzure\PowerShellScripts\azureprofile.json" 
 
$PowerState = ((Get-AzureRmVM -Name Centos -ResourceGroupName WebServices -Status).Statuses[1]).code 
 
If ( $PowerState -contains "PowerState/running") 
{ 
   Write-Host "PowerState1: running" 
} 
ElseIf ( $PowerState -contains "PowerState/deallocated") 
{ 
   Start-AzureRmVM -Name Centos -ResourceGroupName WebServices 
   $PowerState = ((Get-AzureRmVM -Name Centos -ResourceGroupName WebServices -Status).Statuses[1]).code 
} 
Write-Host "PowerState2: $PowerState"