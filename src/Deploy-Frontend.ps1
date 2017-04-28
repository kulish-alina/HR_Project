Param(
   [Parameter(Mandatory)]
   [String]$FrontendLocation,
   [Parameter(Mandatory)]
   [String]$BuildLocation,
   [Parameter(Mandatory)]
   [String]$Destination
)

. "./Common.ps1"

Write-Host "Checking your frontend environment.." -ForegroundColor DarkYellow
CheckEnvironment -Command "npm"
CheckEnvironment -Command "bower"

## Start building bundle
## Build using webpack
Push-Location

Write-Host "Start with frontend.." -ForegroundColor DarkYellow
Set-Location $FrontendLocation
Write-Output " Downloading node modules.."
npm i --loglevel=error
Write-Output " Downloading bower components.."
bower i --loglevel=error --no-colors

Write-Output " Frontend build running.."
npm run dist-build

Pop-Location
Get-ChildItem $BuildLocation -Recurse -Force | Move-Item -Destination $Destination