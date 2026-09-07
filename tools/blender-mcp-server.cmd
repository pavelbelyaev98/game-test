@echo off
setlocal

set "SCRIPT_DIR=%~dp0"
set "REPO_ROOT=%SCRIPT_DIR%.."
set "UV_BIN=%REPO_ROOT%\\.tmp\uv\uv.exe"
set "UV_CACHE_DIR=%REPO_ROOT%\\.tmp\uv-cache"
set "UV_TOOL_DIR=%REPO_ROOT%\\.tmp\uv-tools"

if not exist "%UV_BIN%" (
  echo UV binary not found at "%UV_BIN%"
  exit /b 1
)

set "UV_CACHE_DIR=%UV_CACHE_DIR%"
set "UV_TOOL_DIR=%UV_TOOL_DIR%"

"%UV_BIN%" tool run blender-mcp %*
