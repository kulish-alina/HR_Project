#######################################################################
##
## One script to find them all,
## One script to build them,
## One script to serve them all,
## And in one host run them
##
#######################################################################
## Script parameters
Param (
   [String]$DestinationPath = ".",
   [ValidateSet("Backend", "Frontend", "All")]
   [String]$DeployType = "All",
   [switch]$Deploy,
   [switch]$Backup,
   [switch]$Autostartup
)
. './Common.ps1'
function IsNotAdmin () {   
   if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
      [Security.Principal.WindowsBuiltInRole] "Administrator")) {
      Write-Warning "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!"
      Pause
      Break
   }
}

function AddSSLReservation ([int]$Port, [string]$AppId, [string]$CertHash) {
   Write-Verbose "Adding SSL certificate"
   $result = netsh http show sslcert ipport=0.0.0.0:$Port 2>$1
   if ($result.length -lt 8) {
      netsh http add sslcert ipport=0.0.0.0:$Port appid="{$AppId}" certhash=$CertHash clientcertnegotiation=enable certstorename=Root
   }
}

function AddTunelling([string]$url, [string]$application) {
   Write-Verbose "Tunelling started"
   $result = netsh http show urlacl url=$url 2>$1
   if ($result.length -lt 6) {
      netsh http add urlacl url=$url user=everyone
   }

   & $application
}

try {
   ## Parameter to stop script execution on error
   #$ErrorActionPreference = "Stop"
   IsNotAdmin

   ## Paths
   $deployConfigPath = Join-Path $PSScriptRoot "./deploy.json"
   $localFrontendConfigPath = Join-Path $PSScriptRoot "./wwwroot/config/config.context/local.json"
   $solutionPath = Join-Path $PSScriptRoot "./BaseOfTalents/BaseOfTalents.sln"
   $executable = "ApiHost.exe"

   ## Starting preparations to the build
   ## Getting build parameters
   Write-Host "Build started..." -ForegroundColor DarkYellow

   if (test-path $deployConfigPath) {
      $config = Get-Content -Raw -Path $deployConfigPath | ConvertFrom-Json
      . "./Validate-Config.ps1" -Configuration $config
   }
   else {
      Write-Error "No configuration file exists, please create deploy.json and try again"
      Pause
      Break   
   }

   ## Build environment initialization
   ## Script variables
   $url = "$($config.url):$($config.port)/"
   $rel = "release-$($config.port)"
   $releaseDir = Join-Path $DestinationPath $rel
   $wwwrootDir = Join-Path $releaseDir 'wwwroot'
   $uploadsDir = Join-Path $releaseDir 'wwwroot/uploads'

   $app = Join-Path $releaseDir $executable
   $ishttps = $url -match "^https"

   ## Create frontend build configuration
   ## Create a local config storage to translate it to file
   $localFrontendConfig = @{
      "logLevel" = $config.frLogLevel
      "logPattern" = $config.frLogPattern 
      "serverUrl" = "$($config.frAccessUrl):$($config.port)/"
   }

   if (test-path $localFrontendConfigPath) {
      Remove-Item $localFrontendConfigPath
   }
   $localFrontendConfig | ConvertTo-Json | Out-File -FilePath $localFrontendConfigPath -Encoding utf8   

   if ($DeployType -match "(All|Backend)") {
      . "./Deploy-Backend.ps1" -SolutionLocation $solutionPath -Destination $releaseDir 
   }

   if ($DeployType -match "(All|Frontend)") {
      . "./Deploy-Frontend.ps1" -FrontendLocation "./wwwroot/" -BuildLocation "./wwwroot/dist" -Destination $wwwrootDir
   }

   ## Preparing directory
   # CleanUp -Path $releaseDir

   # New-Item $releaseDir -Type Directory
   # New-Item $wwwrootDir -Type Directory
   # New-Item $uploadsDir -Type Directory

   
   Copy-Item -Path $deployConfigPath -Destination $releaseDir
   Write-Host "Build finished!" -ForegroundColor DarkYellow

   if ($Deploy -eq $true) {
      ## Open client access to the daemon (server)
      Write-Verbose "IsHttps : $ishttps"
      if ($ishttps -eq $true) {
         AddSSLReservation -Port $config.port -AppId $config.appid -CertHash $config.certhash
      }
      
      AddTunelling -url $url -application $app
   }

   if($Backup -eq $true) {
      Write-Verbose "Scheduling backup task"
      . './Create-Backup-Job.ps1' -Database $config.dbInitialCatalog
   }

   if($Autostartup -eq $true) {
      Write-Verbose "Scheduling autostartup"
      . './Create-Autostart-Job.ps1' -WebServerPath $app -JobName "Autostart of $(if(isHttps){'https'} else {'http'}) $executable on $Port port"
   }
}
finally {
   Pop-Location
}
