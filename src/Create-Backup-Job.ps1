Param (
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

$script = "$($PSScriptRoot)\Backup-Database.ps1 $($Database) $($Server) $($Port) $($BackupDir)"
Write-Verbose $script
$jobName = "Backup of $($Database) Task"
Write-Verbose $jobName

$interval = (New-TimeSpan -Hours 8)

$action = New-ScheduledTaskAction -Execute "$pshome\powershell.exe" -Argument "-NonInteractive -NoLogo -NoProfile -File $($script)"
$trigger = New-ScheduledTaskTrigger  -Once -At '3AM' -RepetitionInterval $interval -RepetitionDuration ([System.TimeSpan]::MaxValue)
$settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable -RunOnlyIfNetworkAvailable -DontStopOnIdleEnd

Register-ScheduledTask -TaskName $jobname -Action $action -Trigger $trigger -RunLevel Highest -Settings $settings