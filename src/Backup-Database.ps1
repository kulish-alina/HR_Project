param(
    [Parameter(Mandatory=$true,Position=1)]
    [ValidateNotNullOrEmpty()]
    $Database,
    [Parameter(Mandatory=$true,Position=2)]
    [ValidateNotNullOrEmpty()]
    $Server,
    [Parameter(Mandatory=$true, Position=3)]
    [ValidateNotNullOrEmpty()]
    $BackupDir,
    [Parameter(Mandatory=$false, Position=4)]
    [int]$Port
)
try {
    Write-Host "Checking your environment.."
    if (-not (Get-Module -ListAvailable -Name sqlps)) {
        Import-Module SQLPS    
        Write-Error "Module does not exist in the environment, unable to do the back up"
    }
    if($Server -match "localdb") {
        $remotePath = $BackupDir
    } else {
        $remotePath = "\\$Server\$($BackupDir -replace 'c:', 'c$')"
    }
    
    if(-not $Port -eq 0) {
        $sqlServer = "$Server, $Port"
    } else {
        $sqlServer = $Server
    }
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