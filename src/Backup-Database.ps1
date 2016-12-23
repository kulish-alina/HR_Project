param(
    [Parameter(Mandatory=$true,Position=1)]
    [ValidateNotNullOrEmpty()]
    $Database,
    [Parameter(Mandatory=$false,Position=2)]
    [ValidateNotNullOrEmpty()]
    $Server = "ihrbot.isd.dp.ua",
    [Parameter(Mandatory=$false, Position=3)]
    [ValidateNotNullOrEmpty()]
    [int]$Port =1433,
    [Parameter(Mandatory=$false, Position=4)]
    [ValidateNotNullOrEmpty()]
    $BackupDir = "C:\Backup"
)
try {
    Write-Host "Checking your environment.."
    if (-not (Get-Module -ListAvailable -Name sqlps)) {
        Import-Module SQLPS    
        Write-Error "Module does not exist in the environment, unable to do the back up"
    }

    $remotePath = "\\$Server\$($BackupDir -replace 'c:', 'c$')"
    $sqlServer = "$Server, $Port"
    $fileName = Get-Date -Format "ddMMyyyyHHmmss"
    $backUpFile = Join-Path $BackupDir "$Database\$fileName.bak"
    Write-Verbose $remotePath
    Write-Verbose $sqlServer
    Write-Verbose $backUpFile

    if(-not (Test-Path $remotePath)) {
        Write-Warning "$BackupDir doesn't exist on $remotePC"
        Pause
        Break
    }

    if(-not (Test-Path "$remotePath\$Database")) {
        New-Item -ItemType Directory -Path "$remotePath\$Database"
    }

    Write-Verbose "The path for backup is fine"
    Write-Host "Everything is fine!"
    Write-Host "Start doing backup"
    Invoke-Sqlcmd -ServerInstance $sqlServer -Database $Database -Query "BACKUP DATABASE [$Database] TO DISK='$backUpFile' WITH STATS = 10"
    Write-Host "Backup successfully completed. Check your backup file"
} finally {
    Write-Host "Exiting.."
}