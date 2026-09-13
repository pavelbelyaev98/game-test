$ErrorActionPreference = 'Stop'
$entries = @(& git ls-files --stage)
if ($LASTEXITCODE -ne 0) { throw 'Run this check inside a Git repository.' }
$objects = @($entries | ForEach-Object {
    $parts = $_ -split "`t", 2
    $metadata = $parts[0] -split ' '
    $metadata[1] + ' ' + $parts[1]
})
if ($objects.Count -eq 0) { Write-Output 'No staged files to check.'; exit 0 }
$sizes = @($objects | & git cat-file '--batch-check=%(objecttype) %(objectsize) %(rest)')
if ($LASTEXITCODE -ne 0) { throw 'Could not inspect staged Git objects.' }
$large = @($sizes | ForEach-Object {
    if ($_ -match '^blob (\d+) (.*)$' -and [long]$Matches[1] -ge 10MB) {
        '{0:N2} MiB  {1}' -f ([long]$Matches[1] / 1MB), $Matches[2]
    }
})
if ($large.Count -gt 0) {
    Write-Output 'Large files remain in ordinary Git. Track required binaries with Git LFS and stage them again:'
    $large | Write-Output
    Write-Output 'See docs/development/large-files.md.'
    exit 1
}
Write-Output 'OK: all staged Git blobs are below 10 MiB; LFS pointers are small.'
