param(
    [string]$Workspace = (Split-Path $PSScriptRoot -Parent),
    [string]$PythonPath = 'python',
    [int]$BlenderServerPort = 9876,
    [switch]$Install
)
$ErrorActionPreference = 'Stop'
$Workspace = (Resolve-Path -LiteralPath $Workspace).Path
$server = Join-Path $Workspace '.tools/blender-mcp/Scripts/blender-mcp.exe'
$environmentPython = Join-Path $Workspace '.tools/blender-mcp/Scripts/python.exe'
# Official Blender Lab source, pinned to the version validated with the installed add-on.
$revision = '4309a39646e644261624bfcd2bca669b343b7621'
if ($Install) {
    if (-not (Test-Path -LiteralPath $environmentPython)) {
        & $PythonPath -m venv (Join-Path $Workspace '.tools/blender-mcp')
        if ($LASTEXITCODE -ne 0) { throw 'Could not create the isolated Python environment.' }
    }
    & $environmentPython -m pip install --disable-pip-version-check "git+https://projects.blender.org/lab/blender_mcp.git@$revision#subdirectory=mcp" 'mcp<2'
    if ($LASTEXITCODE -ne 0) { throw 'Official Blender MCP installation failed.' }
}
if (-not (Test-Path -LiteralPath $server)) { throw 'Run this script with -Install first.' }
& $server --help | Out-Null
if ($LASTEXITCODE -ne 0) { throw 'The Blender MCP executable failed its startup check.' }
& codex mcp add blender --env BLENDER_MCP_HOST=127.0.0.1 --env "BLENDER_MCP_PORT=$BlenderServerPort" -- $server --transport stdio
if ($LASTEXITCODE -ne 0) { throw 'Codex MCP registration failed.' }
$configPath = Join-Path $Workspace '.vscode/mcp.json'
if (Test-Path -LiteralPath $configPath) { $config = Get-Content -LiteralPath $configPath -Raw | ConvertFrom-Json }
else { $config = [PSCustomObject]@{} }
if (-not $config.PSObject.Properties['servers']) { $config | Add-Member -NotePropertyName servers -NotePropertyValue ([PSCustomObject]@{}) }
$entry = [PSCustomObject]@{
    type = 'stdio'; command = $server; args = @('--transport', 'stdio')
    env = [PSCustomObject]@{ BLENDER_MCP_HOST='127.0.0.1'; BLENDER_MCP_PORT="$BlenderServerPort" }
}
$config.servers | Add-Member -NotePropertyName blender -NotePropertyValue $entry -Force
New-Item -ItemType Directory -Force (Split-Path $configPath -Parent) | Out-Null
$config | ConvertTo-Json -Depth 12 | Set-Content -LiteralPath $configPath -Encoding UTF8
Write-Host 'Configured Blender MCP for Codex and VS Code. Keep the Blender MCP add-on running on localhost.'
Write-Host 'Restart the Codex extension to load the new server. Undo: codex mcp remove blender; see unity/readme.md.'
