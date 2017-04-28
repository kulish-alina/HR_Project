Param(
   [String]$SolutionLocation,
   [String]$Destination
)

. "./Common.ps1"

CheckEnvironment -Command "msbuild"
CheckEnvironment -Command "nuget"

Write-Host "Start serving backend.." -ForegroundColor DarkYellow

Write-Output " Restoring nuget packages"
nuget restore $SolutionLocation

Write-Output " Building backend"
msbuild $SolutionLocation /t:Build /p:OutDir=$Destination /p:Configuration=Release /p:DebugSymbols=false /p:DebugType=None /p:ExcludeGeneratedDebugSymbol=true /p:AllowedReferenceRelatedFileExtensions=none  /nologo /m /v:m 


