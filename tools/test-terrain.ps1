param(
    [string]$UnityEditor = 'C:/Program Files/Unity/Hub/Editor/6000.6.0f1/Editor/Unity.exe'
)

$ErrorActionPreference = 'Stop'
$taskSource = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../unity'))
$taskStamp = Get-Date -Format 'yyyyMMdd-HHmmss-fff'
$taskRun = Join-Path $taskSource ('Logs/TerrainValidation-' + $taskStamp)
# Keep package extraction paths below Windows' legacy path limit.
$taskSnapshot = Join-Path ([IO.Path]::GetTempPath()) ('sdt06-' + $taskStamp)
if (-not (Test-Path -LiteralPath $UnityEditor -PathType Leaf)) { throw 'Pinned Unity editor not found.' }
New-Item -ItemType Directory -Path $taskSnapshot -Force | Out-Null
New-Item -ItemType Directory -Path $taskRun -Force | Out-Null
foreach ($taskFolder in @('Assets', 'Packages', 'ProjectSettings')) {
    Copy-Item -LiteralPath (Join-Path $taskSource $taskFolder) -Destination $taskSnapshot -Recurse -Force
}
# A real copy avoids modifying the open editor's imported package cache.
$taskCache = Join-Path $taskSource 'Library/PackageCache'
if (Test-Path -LiteralPath $taskCache -PathType Container) {
    New-Item -ItemType Directory -Path (Join-Path $taskSnapshot 'Library') -Force | Out-Null
    Copy-Item -LiteralPath $taskCache -Destination (Join-Path $taskSnapshot 'Library') -Recurse -Force
}
$taskLog = Join-Path $taskRun 'SceneSetup.log'
$taskArguments = @('-batchmode', '-nographics', '-quit', '-projectPath', ('"' + $taskSnapshot + '"'),
    '-executeMethod', 'SomethingDownThere.Editor.MainGameSceneBuilder.CreateIfMissing',
    '-logFile', ('"' + $taskLog + '"'))
Write-Output "Preparing isolated terrain project. Log: $taskLog"
$taskProcess = Start-Process -FilePath $UnityEditor -ArgumentList $taskArguments -WindowStyle Hidden -PassThru
$taskProcess.WaitForExit()
if ($taskProcess.ExitCode -ne 0) { throw "Scene setup failed (exit $($taskProcess.ExitCode)). Read $taskLog" }
if (-not (Test-Path -LiteralPath (Join-Path $taskSnapshot 'Assets/Scenes/MainGame.unity'))) {
    throw "Scene setup produced no MainGame scene. Read $taskLog"
}
try {
    & (Join-Path $PSScriptRoot 'test-fps.ps1') -UnityEditor $UnityEditor -ProjectPath $taskSnapshot
} finally {
    Get-ChildItem -LiteralPath (Join-Path $taskSnapshot 'Logs') -Directory -Filter 'FpsValidation-*' |
        ForEach-Object { Copy-Item -LiteralPath $_.FullName -Destination $taskRun -Recurse }
}
Write-Output "Terrain and FPS checks passed in $taskSnapshot"
