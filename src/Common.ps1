function CleanUp ($Path) {
   if (Test-Path $path) {
      Remove-Item $path -Recurse -Force
   }
}

function CheckEnvironment ([string]$Command) {
   if ($null -eq (Get-Command $Command -ErrorAction SilentlyContinue)) { 
      Write-Error "Unable to find $Command in your PATH"
   }
}