$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath; 
iex "cd ../"; 
Write-Host "Working directory is $PSScriptRoot";
$command = 'dotnet test RowaLogUnitTests'; 
Write-Host  $command; 
iex $command; 
Write-Host -NoNewLine 'Press enter to continue...';
Read-Host