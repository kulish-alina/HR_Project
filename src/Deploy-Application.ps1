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
   [switch]$Deploy,
   [switch]$Backup,
   [switch]$Autostartup
)

function CheckEnvironment ([string]$Command) {
   if ($null -eq (Get-Command $Command -ErrorAction SilentlyContinue)) { 
      Write-Error "Unable to find $Command in your PATH"
   }
}

function IsNotAdmin () {   
   if (-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
      [Security.Principal.WindowsBuiltInRole] "Administrator")) {
      Write-Warning "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!"
      Pause
      Break
   }
}

function CleanUp ($Path) {
   if (Test-Path $path) {
      Remove-Item $path -Recurse -Force
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
   Push-Location
   IsNotAdmin

   ## Paths
   $deployConfigPath = Join-Path $PSScriptRoot "./deploy.json"
   $localFrontendConfigPath = Join-Path $PSScriptRoot "./wwwroot/config/config.context/local.json"
   $frontendBuildResultPath = Join-Path $PSScriptRoot "./wwwroot/dist/"
   $solutionDir = Join-Path $PSScriptRoot "./BaseOfTalents"
   $solutionPath = Join-Path $solutionDir "./BaseOfTalents.sln"
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
   $releasePath = Join-Path $PSScriptRoot $releaseDir

   $app = Join-Path $releaseDir $executable
   $ishttps = $url -match "^https"

   ## Checking execution environment
   ## First of all it is npm, bower and msbuild   
   Write-Host "1. Checking your environment.." -ForegroundColor DarkYellow
   CheckEnvironment -Command "npm"
   CheckEnvironment -Command "bower"
   CheckEnvironment -Command "msbuild"

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

   ## Start building bundle
   ## Build using webpack
   Write-Host "2. Start with frontend.." -ForegroundColor DarkYellow
   Set-Location ./wwwroot/
   Write-Output " Downloading node modules.."
   npm i --loglevel=error
   Write-Output " Downloading bower components.."
   bower i --loglevel=error --no-colors

   Write-Output " Frontend build running.."
   npm run dist-build
   Pop-Location

   ## Collecting all the data together
   CleanUp -Path $releasePath

   New-Item $releaseDir -Type Directory
   New-Item $wwwrootDir -Type Directory
   New-Item $uploadsDir -Type Directory

   Write-Host "3. Start serving backend.." -ForegroundColor DarkYellow

   Write-Output " Restoring nuget packages"
   & (Join-Path $solutionDir "./.nuget/Nuget.exe") restore $solutionPath

   Write-Output " Building backend"
   msbuild.exe $solutionPath /t:Build /p:Configuration=Release /p:DebugSymbols=false /p:DebugType=None /p:ExcludeGeneratedDebugSymbol=true /p:AllowedReferenceRelatedFileExtensions=none /p:OutputPath=$releasePath /nologo /m /v:m 

   Get-ChildItem $frontendBuildResultPath -Recurse -Force | Move-Item -Destination $wwwrootDir
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
