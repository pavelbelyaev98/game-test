# Unity developer guide

Unity is pinned to `6000.6.0f1` with URP `17.6.0`. Runtime work belongs in `Assets/`; generated evidence stays under ignored `Logs/`.

## Official Unity CLI and Pipeline

Use Unity's official CLI directly from the terminal with `com.unity.pipeline` for live editor work. No additional Unity server or bridge is configured or needed for this workflow.

Verified setup: CLI `1.0.0-beta.8`, Pipeline `0.6.0-exp.1`, and Unity Technologies' `unity-agent-plugin` (`unity` plugin `0.1.0-beta`). These are the official beta/experimental releases required for the requested workflow; no gameplay dependency was upgraded.

From the repository root, setup and verification commands are:

```powershell
unity --version
unity upgrade --check --channel beta --format json
unity pipeline install --project-path ./unity --format json
unity pipeline list --format json
codex plugin marketplace add Unity-Technologies/unity-agent-plugin
codex plugin add unity@unity-agent-plugin
codex plugin list --marketplace unity-agent-plugin --json
```

The official Unity agent plugin is installed and enabled in Codex. Keep this project open in Unity for live commands, or open it with `unity open ./unity`. Pass `--project-path ./unity` from this repository root to target the correct Editor. Direct CLI calls already work in the current session.

```powershell
unity status --format json
unity command --project-path ./unity --format json
unity command editor_status --project-path ./unity --format json
unity command get_scene_hierarchy --project-path ./unity --format json
unity command console --tail 20 --project-path ./unity --format json
unity command eval 'return 2 + 2;' --project-path ./unity --format json
```

Discover command parameters from the connected Editor before use. Prefer direct C# file edits and CLI inspection of live scene/editor state. After script changes, use `recompile`, then poll `recompile_status` until successful. Do not start a second Editor on the same open project.

Sources: [official agent plugin](https://github.com/Unity-Technologies/unity-agent-plugin), [official CLI skill](https://github.com/Unity-Technologies/skills/tree/main/skills/unity-cli). Current audit: `Logs/Task18Audit/audit.md`.

## Windows build and validation

The user reviews `builds/windows/SomethingDownThere.exe`. With this project open, build the existing `Assets/Scenes/MainGame.unity` using:

```powershell
unity command menu --path 'Tools/Something Down There/Build Windows Player' --project-path ./unity --format json
```

With the Editor closed, use `./tools/build-windows.ps1`. Task `16` implements a bordered, resizable review window (1920x1080 on 2560x1440), Space to jump and a 0.22-second hold to engage the jetpack; final desktop review is pending. Escape releases the mouse and focus loss pauses.

With the Editor closed, `./tools/test-fps.ps1` runs batch validation; `./tools/test-terrain.ps1` uses an isolated copy. For the open Editor, use `unity command run_tests --mode editor --filter SomethingDownThere.EditModeTests --filter_type assembly --async_tests true --project-path ./unity --format json`, then poll `test_status`. Repeat with `--mode playmode --filter SomethingDownThere.PlayModeTests`. Save scene edits first. The status payload can be a JSON string requiring a second parse.

For HUD/menu inspection, use `capture_game_view --source screen` in Play Mode; `screenshot` renders the camera and omits overlay UI. Save the returned base64 PNG under `unity/Logs/`; the capture command's `save_path` is normalized into `Assets/` and imports the image. Capture evidence must not remain in game assets.

## Source map

- `Assets/Runtime/Player` - movement, input, battery, desktop window
- `Assets/Runtime/Interaction` - target/station contracts and inventory
- `Assets/Runtime/Terrain` - excavation grid, mesh, collision, boundaries
- `Assets/Runtime/UI` - HUD and menus
- `Assets/Runtime/Validation` - temporary fixtures only
- `Assets/Editor` - scene and build tooling
- `Assets/Tests` - repository-owned checks
