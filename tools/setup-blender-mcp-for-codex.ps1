param(
    [string]$Workspace = (Get-Location).Path,
    [string]$UvPath = "uv",
    [string]$BlenderServerPort = "9876",
    [string]$UvCacheDir = "",
    [string]$UvToolDir = "",
    [switch]$Install = $false
)

if (-not (Test-Path (Join-Path $Workspace ".vscode"))) {
    New-Item -ItemType Directory -Path (Join-Path $Workspace ".vscode") | Out-Null
}

$Workspace = (Resolve-Path $Workspace).Path

if ([string]::IsNullOrWhiteSpace($UvCacheDir)) {
    $UvCacheDir = Join-Path $Workspace ".tmp\uv-cache"
}
if ([string]::IsNullOrWhiteSpace($UvToolDir)) {
    $UvToolDir = Join-Path $Workspace ".tmp\uv-tools"
}

$mcpConfig = @"
{
  "servers": {
    "blender": {
      "type": "stdio",
      "command": "tools\\blender-mcp-server.cmd",
      "args": [
        "--transport",
        "stdio"
      ],
      "env": {
        "BLENDER_MCP_HOST": "127.0.0.1",
        "BLENDER_MCP_PORT": "$BlenderServerPort"
      }
    }
  }
}
"@

$mcpConfig | Set-Content -Encoding UTF8 (Join-Path $Workspace ".vscode\mcp.json")
Write-Host "Wrote .vscode\mcp.json"

if ($Install) {
    $resolvedUvPath = $UvPath
    if ($UvPath -eq "uv" -and -not (Get-Command $UvPath -ErrorAction SilentlyContinue)) {
        $candidates = @(
            Join-Path $Workspace ".tmp\uv\uv.exe",
            Join-Path (Split-Path $Workspace -Parent) ".tmp\uv\uv.exe",
            Join-Path (Get-Location).Path ".tmp\uv\uv.exe"
        )
        foreach ($candidate in $candidates) {
            if (Test-Path $candidate) {
                $resolvedUvPath = $candidate
                break
            }
        }
    }

    if (-not (Test-Path $resolvedUvPath)) {
        throw "uv was not found. Install uv and pass a valid -UvPath."
    }

    $env:UV_CACHE_DIR = $UvCacheDir
    $env:UV_TOOL_DIR = $UvToolDir

    Write-Host "Installing blender-mcp server package..."
    & $resolvedUvPath tool install "git+https://projects.blender.org/lab/blender_mcp" --with "mcp<2" --force
    if ($LASTEXITCODE -ne 0) { throw "uv install failed. Check console output above." }
    Write-Host "Install done. Verify with: blender-mcp --help"
} else {
    Write-Host "Install flag not set. Run this script with -Install to run uv tool install."
}

Write-Host "Reminder: In Blender, enable MCP add-on (host localhost, port $BlenderServerPort) before opening MCP calls."
