param(
   [Parameter(Mandatory=$true,Position=1)]
   [ValidateNotNullOrEmpty()]
   $WebServerPath,
   [Parameter(Mandatory=$true,Position=2)]
   [ValidateNotNullOrEmpty()]
   $JobName
)

Write-Verbose "Start executing script..."
$action = New-ScheduledTaskAction -Execute "$pshome\powershell.exe" -Argument "-NonInteractive -NoLogo -Command '& $WebServerPath'"
$trigger = New-JobTrigger -AtStartup -RandomDelay 00:00:30
$settings = New-ScheduledTaskSettingsSet -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries -StartWhenAvailable -RunOnlyIfNetworkAvailable -DontStopOnIdleEnd
Write-Verbose "The task has been configured"

Register-ScheduledTask -TaskName $JobName -Action $action -Trigger $trigger -RunLevel Highest -Settings $settings
Write-Host "Task registered"