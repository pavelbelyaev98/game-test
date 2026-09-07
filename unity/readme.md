# Unity developer guide

Unity is pinned to `6000.6.0f1` with URP `17.6.0`. Runtime work belongs in `Assets/`; generated evidence stays under ignored `Logs/`.

## Official Unity CLI and Pipeline

Use Unity's official CLI directly from the terminal with `com.unity.pipeline` for live editor work. No additional Unity server or bridge is configured or needed for this workflow.

Verified setup: CLI `1.0.0-beta.8`, Pipeline `0.6.0-exp.1`, and Unity Technologies' `unity-agent-plugin` (`unity` plugin `0.1.0-beta`). These are the official beta/experimental releases required for the requested workflow; no gameplay dependency was upgraded.

From the repository root, setup and verification commands are:

```powershell
$projectPath = (Resolve-Path ./unity).Path
unity --version
unity upgrade --check --channel beta --format json
unity pipeline install --project-path "$projectPath" --format json
unity pipeline list --format json
codex plugin marketplace add Unity-Technologies/unity-agent-plugin
codex plugin add unity@unity-agent-plugin
codex plugin list --marketplace unity-agent-plugin --json
```

The official Unity agent plugin is installed and enabled in Codex. Keep this project open in Unity for live commands, or open it with `unity open ./unity`. Resolve the Unity directory to its full path for `--project-path`; this CLI version can fail to match an open Editor when given a relative path. Direct CLI calls already work in the current session.

```powershell
unity status --format json
unity command --project-path "$projectPath" --format json
unity command editor_status --project-path "$projectPath" --format json
unity command get_scene_hierarchy --project-path "$projectPath" --format json
unity command console --tail 20 --project-path "$projectPath" --format json
unity command eval 'return 2 + 2;' --project-path "$projectPath" --format json
```

Discover command parameters from the connected Editor before use. Prefer direct C# file edits and CLI inspection of live scene/editor state. After script changes, use `recompile`, then poll `recompile_status` until successful. Do not start a second Editor on the same open project.

Sources: [official agent plugin](https://github.com/Unity-Technologies/unity-agent-plugin), [official CLI skill](https://github.com/Unity-Technologies/skills/tree/main/skills/unity-cli). Current audit: `Logs/Task18Audit/audit.md`.

## Windows build and validation

The user reviews `builds/windows/SomethingDownThere.exe`. With this project open, build the existing `Assets/Scenes/MainGame.unity` using:

```powershell
unity command menu --path 'Tools/Something Down There/Build Windows Player' --timeout 300 --project-path "$projectPath" --format json
```

With the Editor closed, use `./tools/build-windows.ps1`. Task `16` implements a bordered, resizable review window, Space to jump and a 0.22-second hold to engage the jetpack. Task `19` verified a 1920x1080 client on the user's 2560x1440 desktop; movement/jetpack feel still needs user review. Escape releases the mouse and focus loss pauses. Allow 300 seconds for the live build command; its default 30-second request timeout can expire while Unity continues building.

With the Editor closed, `./tools/test-fps.ps1` runs batch validation; `./tools/test-terrain.ps1` uses an isolated copy. For the open Editor, use `unity command run_tests --mode editor --filter SomethingDownThere.EditModeTests --filter_type assembly --async_tests true --project-path "$projectPath" --format json`, then poll `test_status`. Repeat with `--mode playmode --filter SomethingDownThere.PlayModeTests`. Save scene edits first. The status payload can be a JSON string requiring a second parse.

For HUD/menu inspection, use `capture_game_view --source screen` in Play Mode; `screenshot` renders the camera and omits overlay UI. Save the returned base64 PNG under `unity/Logs/`; the capture command's `save_path` is normalized into `Assets/` and imports the image. Capture evidence must not remain in game assets.

## Blender MCP

The existing Blender Lab `MCP` extension 1.0.0 is running in Blender 5.2 on `127.0.0.1:9876`. Its official stdio bridge is installed in the isolated, ignored `.tools/blender-mcp/` environment: `blender-mcp` 1.0.0 from [Blender Lab](https://projects.blender.org/lab/blender_mcp), pinned to commit `4309a39646e644261624bfcd2bca669b343b7621`; MCP SDK 1.30.0 (`<2`). No art is created by setup.

Run `./tools/setup-blender-mcp-for-codex.ps1 -Install` to reproduce the installation; omit `-Install` to register an existing install. `tools/blender-mcp-server.cmd` launches the same isolated executable. Keep the Blender add-on running and restart the Codex extension after registration. [Codex MCP configuration](https://learn.chatgpt.com/docs/extend/mcp?surface=cli).

Verification: MCP initialize, 26-tool listing, blend-file state, datablock counts, scene hierarchy, window metadata and an actual screenshot all succeeded. Evidence: `Logs/Task19/blender-check.json` and `blender-window.png`. The existing scene was not changed or saved.

Complete setup footprint and reversal:

- `.tools/blender-mcp/` recursively contains the Python environment, installed bridge, SDK, dependencies and package metadata; it is excluded by `.gitignore`. Remove this isolated directory after closing its clients to uninstall local tooling.
- `C:/Users/pavel/.codex/config.toml`: only `[mcp_servers.blender]` and its environment table were added; `codex mcp remove blender` removes them.
- `.vscode/mcp.json`: only the `servers.blender` entry is managed; remove that entry to disconnect VS Code. Other server entries are preserved.
- `tools/setup-blender-mcp-for-codex.ps1` and `tools/blender-mcp-server.cmd` replace the broken `.tmp/uv` launcher. Their baseline is commit `6480694`; there is no uv install or global Python package modification.
- The user's pre-existing `%APPDATA%/Blender Foundation/Blender/5.2/extensions/user_default/mcp/` add-on was inspected and used, not installed or modified by this task.
- Ignored `Logs/Task19/tool-source/` is a downloaded official source checkout used for verification; pip also uses its ordinary user cache. Neither is game content.

## Source map

- `Assets/Runtime/Player` - movement, input, battery, desktop window
- `Assets/Runtime/Interaction` - target/station contracts and inventory
- `Assets/Runtime/Terrain` - excavation grid, mesh, collision, boundaries
- `Assets/Runtime/UI` - HUD and menus
- `Assets/Runtime/Validation` - temporary fixtures only
- `Assets/Editor` - scene and build tooling
- `Assets/Tests` - repository-owned checks
