param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('CreateScene', 'MovePlayBoard', 'AuthorPhysicalHandling', 'AuthorQuietHelp', 'AuthorLooseProps', 'TuneLooseBall', 'AuthorComfort', 'TuneComfortPresentation', 'AuthorHandling', 'AuthorFreePlacement', 'TunePlacementPreview', 'AuthorProcessing', 'AuthorFinishedFood', 'TuneFinishedFoodPresentation', 'TuneProcessingLabels', 'TuneHandlingView', 'HoldOnlyScooping', 'EditMode', 'PlayMode', 'Build', 'Smoke', 'BuildDevelopment', 'SmokeDevelopment')]
    [string]$Mode,
    [string]$EditorPath
)

$ErrorActionPreference = 'Stop'
$projectRoot = (Resolve-Path -LiteralPath (Join-Path $PSScriptRoot '..')).Path
$logsPath = Join-Path $projectRoot 'Logs'
New-Item -ItemType Directory -Path $logsPath -Force | Out-Null
if (-not $EditorPath) {
    $versionLine = Get-Content -LiteralPath (Join-Path $projectRoot 'ProjectSettings/ProjectVersion.txt') |
        Where-Object { $_ -match '^m_EditorVersion: ' }
    $editorVersion = $versionLine.Substring('m_EditorVersion: '.Length).Trim()
    $EditorPath = "C:/Program Files/Unity/Hub/Editor/$editorVersion/Editor/Unity.exe"
}

$logPath = Join-Path $logsPath "Foundation-$Mode.log"
$arguments = @('-batchmode', '-projectPath', $projectRoot, '-logFile', $logPath)
$programPath = $EditorPath
$resultPath = $null
switch ($Mode) {
    'MovePlayBoard' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.PhysicalHandlingAuthoring.MoveBoard') }
    'AuthorPhysicalHandling' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.PhysicalHandlingAuthoring.Apply') }
    'TuneLooseBall' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.LoosePropsAuthoring.TuneBall') }
    'AuthorQuietHelp' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.QuietHelpAuthoring.Apply') }
    'AuthorLooseProps' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.LoosePropsAuthoring.Apply') }
    'CreateScene' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.FoundationSceneBuilder.CreateScene') }
    'TunePlacementPreview' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.FreePlacementAuthoring.TunePreview') }
    'AuthorFreePlacement' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.FreePlacementAuthoring.Apply') }
    'TuneComfortPresentation' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.ComfortAuthoring.TunePresentation') }
    'AuthorComfort' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.ComfortAuthoring.Apply') }
    'AuthorHandling' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.HandlingSceneAuthoring.Apply') }
    'AuthorProcessing' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.ProcessingSceneAuthoring.Apply') }
    'AuthorFinishedFood' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.FinishedFoodAuthoring.Apply') }
    'TuneFinishedFoodPresentation' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.FinishedFoodAuthoring.TunePresentation') }
    'TuneProcessingLabels' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.ProcessingSceneAuthoring.TuneLabels') }
    'TuneHandlingView' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.HandlingSceneAuthoring.ApplyVisibilityTuning') }
    'HoldOnlyScooping' { $arguments += @('-nographics', '-quit', '-executeMethod', 'JustAFewPeppers.Editor.HandlingSceneAuthoring.ApplyHoldOnlyScooping') }
    'Build' { $arguments += @('-quit', '-executeMethod', 'JustAFewPeppers.Editor.FoundationSceneBuilder.BuildWindows') }
    'BuildDevelopment' { $arguments += @('-quit', '-executeMethod', 'JustAFewPeppers.Editor.FoundationSceneBuilder.BuildWindowsDevelopment') }
    'Smoke' {
        $programPath = Join-Path $projectRoot 'Builds/JustAFewPeppers/JustAFewPeppers.exe'
        $smokePath = Join-Path $logsPath 'FoundationSmoke'
        $arguments = @('-batchmode', '-logFile', $logPath, '-foundationSmoke', $smokePath)
    }
    'SmokeDevelopment' {
        $programPath = Join-Path $projectRoot 'Builds/JustAFewPeppers-Development/JustAFewPeppers.exe'
        $smokePath = Join-Path $logsPath 'FoundationDevelopmentSmoke'
        $arguments = @('-batchmode', '-logFile', $logPath, '-foundationSmoke', $smokePath, '-expectDevelopment')
    }
    default {
        $resultPath = Join-Path $logsPath "Foundation-$Mode.xml"
        $arguments += @('-nographics', '-runTests', '-testPlatform', $Mode, '-assemblyNames', "JustAFewPeppers.${Mode}Tests", '-testResults', $resultPath)
    }
}
if (-not (Test-Path -LiteralPath $programPath)) { throw "Executable missing: $programPath" }
# Start-Process joins ArgumentList into a command line; quote each path/value explicitly.
$quotedArguments = $arguments | ForEach-Object { '"' + $_ + '"' }
$startedAt = Get-Date
$process = Start-Process -FilePath $programPath -ArgumentList $quotedArguments -WorkingDirectory $projectRoot -WindowStyle Hidden -PassThru
if (-not $process.WaitForExit(600000)) {
    Stop-Process -Id $process.Id
    throw "Unity $Mode timed out after 10 minutes. See $logPath"
}
$process.Refresh()
if ($process.ExitCode -ne 0) { throw "Unity $Mode exited with $($process.ExitCode). See $logPath" }
if ($resultPath) {
    if ((Get-Item -LiteralPath $resultPath).LastWriteTime -lt $startedAt) { throw 'No fresh test results.' }
    [xml]$results = Get-Content -LiteralPath $resultPath
    $run = $results.'test-run'
    if ($run.result -ne 'Passed' -or [int]$run.total -eq 0) { throw "Test run did not pass: $resultPath" }
    Write-Output "$Mode passed: $($run.passed)/$($run.total). Results: $resultPath"
} elseif ($Mode -in @('Smoke', 'SmokeDevelopment')) {
    $smokeResult = Join-Path $smokePath 'result.txt'
    if ((Get-Item -LiteralPath $smokeResult).LastWriteTime -lt $startedAt) { throw 'No fresh smoke result.' }
    $report = Get-Content -LiteralPath $smokeResult -Raw
    if (-not $report.StartsWith('PASS:')) { throw $report }
    Write-Output $report
} else {
    Write-Output "$Mode finished. Log: $logPath"
}
