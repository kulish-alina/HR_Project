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
  [String]$DestinationPath,
  [bool]$Deploy
)

## Parameter to stop script execution on error
#$ErrorActionPreference = "Stop"

if(-Not ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole(`
   [Security.Principal.WindowsBuiltInRole] "Administrator")) {
   Write-Warning "You do not have Administrator rights to run this script!`nPlease re-run this script as an Administrator!"
   Break
}

## Script function-helpers
function PSCoalesce($a, $b) { if ($a -ne "" -and $null -ne $a) { $a } else { $b } }

function CheckEnvironment ([string]$Command) {
  if ($null -eq (Get-Command $Command -ErrorAction SilentlyContinue)) { 
    Write-Error "Unable to find $Command in your PATH"
  }
}

## Paths
$deployConfigPath          = Join-Path $PSScriptRoot "./deploy.json"
$localFrontendConfigPath   = Join-Path $PSScriptRoot "./wwwroot/config/config.context/local.json"
$frontendBuildResultPath   = Join-Path $PSScriptRoot "./wwwroot/dist/*"
$solutionDir               = Join-Path $PSScriptRoot "./BaseOfTalents"
$solutionPath              = Join-Path $solutionDir "./BaseOfTalents.sln"

## Starting preparations to the build
## Getting build parameters
Write-Host "Build started..." -ForegroundColor DarkYellow

if(test-path $deployConfigPath) {
   $config = Get-Content -Raw -Path $deployConfigPath | ConvertFrom-Json
   . "./Validate-Config.ps1" -Configuration $config
} else {
   Write-Error "No configuration file exists, please create deploy.json and try again"   
}

## Build environment initialization
## Script variables
$url = "$($config.url):$($config.port)/"
$rel = "release-$($config.port)"
$dest = PSCoalesce $DestinationPath "."
$releaseDir = Join-Path $dest       $rel
$wwwrootDir = Join-Path $releaseDir $config.wwwroot
$uploadsDir = Join-Path $releaseDir $config.uploads
$releasePath = Join-Path $PSScriptRoot $releaseDir

## Checking execution environment
## First of all it is npm, bower and msbuild   
Write-Host "1. Checking your environment.." -ForegroundColor DarkYellow
CheckEnvironment -Command "npm"
CheckEnvironment -Command "bower"
CheckEnvironment -Command "msbuild"

## Create frontend build configuration
## Create a local config storage to translate it to file
$localFrontendConfig = New-Object -TypeName PSObject
$localFrontendConfig | Add-Member -MemberType NoteProperty -name "logLevel"   -value $config.frLogLevel
$localFrontendConfig | Add-Member -MemberType NoteProperty -name "logPattern" -value $config.frLogPattern
$localFrontendConfig | Add-Member -MemberType NoteProperty -name "serverUrl"  -value "$($config.frAccessUrl):$($config.port)/"

if(test-path $localFrontendConfigPath) {
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
Set-Location ..

## Collecting all the data together
if(Test-Path $releaseDir) {
   Remove-Item $releaseDir -Recurse -Force
}

New-Item $releaseDir -Type Directory
New-Item $wwwrootDir -Type Directory
New-Item $uploadsDir -Type Directory

Write-Host "3. Start serving backend.." -ForegroundColor DarkYellow

Write-Output " Restoring nuget packages"
& (Join-Path $solutionDir "./.nuget/Nuget.exe") restore $solutionPath

Write-Output " Building backend"
msbuild.exe $solutionPath /t:Build /p:Configuration=Release /p:DebugSymbols=false /p:DebugType=None /p:ExcludeGeneratedDebugSymbol=true /p:AllowedReferenceRelatedFileExtensions=none /p:OutputPath=$releasePath /nologo /m /v:m 

Move-Item -Path $frontendBuildResultPath  -Destination $wwwrootDir
Copy-Item -Path $deployConfigPath         -Destination $releaseDir

Write-Host "Build finished!" -ForegroundColor DarkYellow

if($Deploy -eq $true) {
   ## Open client access to the daemon (server)
   $result = netsh http show urlacl url=$url 2>$1
   if($result.length -lt 6) {
      netsh http add urlacl url=$url user=everyone
   }

  & (Join-Path $releaseDir "ApiHost.exe")
}
