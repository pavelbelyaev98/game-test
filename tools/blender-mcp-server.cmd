@echo off
setlocal
set "BLENDER_MCP_EXE=%~dp0..\.tools\blender-mcp\Scripts\blender-mcp.exe"
if not exist "%BLENDER_MCP_EXE%" (
  echo Blender MCP is not installed. Run tools\setup-blender-mcp-for-codex.ps1 -Install. 1>&2
  exit /b 1
)
"%BLENDER_MCP_EXE%" %*
