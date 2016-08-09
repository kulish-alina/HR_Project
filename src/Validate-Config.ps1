Param (
   [Parameter(Mandatory=$True,Position=1)]
   [object]$Configuration
)

$neededParameters =  @("url",
                     "port",
                     "dbInitialCatalog",
                     "dbDataSource"
                     "email",
                     "password",
                     "frAccessUrl",
                     "frLogLevel",
                     "frLogPattern")

$fieldsNotExistList = New-Object System.Collections.ArrayList

foreach($parameter in $neededParameters) {
   if( -not ($Configuration.PSobject.Properties.name -match $parameter)) {
      $fieldsNotExistList.Add($parameter)
   }
}

if($fieldsNotExistList.Count -gt 0) {
   $errorMessage = @"
The configuration file doesn't contains all needed fields, please check your configuration.
There is no property:`n$($fieldsNotExistList -join "`n")
"@
   Write-Error $errorMessage
   Pause
   Break
}
