
## Script function-helpers
function PSCoalesce($a, $b) {
    if ($a -ne "" -and $null -ne $a)
    { 
        $a 
    } 
    else 
    {
        $b 
    }
}