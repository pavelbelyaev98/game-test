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

Development play: launch `builds/windows/SomethingDownThere.exe` for **New Game, Load Game, Settings and Quit**. New Game confirms before replacing an existing slot, retaining its files under `Save/PreviousGames/<timestamp-id>/`; Load Game validates/restores the current slot and is disabled when absent. Settings exposes the existing FOV/crosshair controls and returns to startup. In play, **Ctrl+Shift+F10** opens Developer admin (also **Esc > Developer admin**). Hold **Ctrl+Shift** with **1-6** (number row or numpad) to select a shovel, **R** to refill, **X** to toggle buried-find X-ray, or **Home** to return to the rim. The panel also provides **Unlimited battery**, **Restore normal rules**, and confirmed **Reset ground...**. Plain keys have no admin effect. Overrides last for the session and preserve owned progression; no special launcher or launch flag exists.

Aim down and hold **LMB**; aim at walls to widen or tunnel. Cuts follow the struck surface and leave broad angled floors with uneven edges; overlapping strokes deepen and widen them. Shovel reach increases through **3.0 / 3.2 / 3.4 / 3.6 / 3.8 / 4.0 m**, with a hard 4 m maximum shown on the HUD. Nominal cut widths increase gently from 0.82 to 1.92 m; separate speed/strength upgrades are planned under Task `25`. Hold **Space** to fly; after thrust in this flight, release to fall and press again to resume immediately. Excavation persists across quit/reload.

There are 96 buried finds, including 24 shallow finds toward the entrance. **X-ray** shows markers only. Current finds use the larger dimensions from Task `30`, gentler shovels from `31`, and **40%** required exposure from `72`. Hold **LMB** directly on a visible small find: ordinary strokes clear its nearby covering soil until 40% exposure, then collect within 3 m without releasing. Large finds still require aiming at surrounding soil. After a short pickup recovery, the same hold continues digging. E remains for stations. Pickup messages name the find; **Tab** lists collected items. X-ray cannot bypass covering soil, exposure or capacity. Reset ground reburies uncollected items and preserves collected ones. Task `09` replaces the simple user-requested shapes with final art; Task `22` verifies release content/access.

The normal build command uses `BuildOptions.Development` for in-game admin access. **Build Windows Release Player** in the same Tools menu writes the same executable path without development access. Both use `DesktopInstance` before scene loading: repeated Windows launches request the existing window and exit quietly, independently of Steam. Keep Unity's `forceSingleInstance` disabled because it displays a fatal-error dialog. Profile contention still offers a clear retry/back/quit screen, with OS details retained only in logs. `Debug.isDebugBuild` gates admin; it requires no Windows elevation. Task `22` must verify release exclusion and ordinary progression before production; [release gate](../docs/features/backlog/excavation-terrain.md#task-22---production-release-gate). Keep durable tooling; do not add disposable review launchers/files.

With the Editor closed, use `./tools/build-windows.ps1`. Task `16` verifies a bordered 1920x1080 startup window on the user's 2560x1440 desktop, player resizing to 960x540 and 1280x800, and native Space tap/hold/restart/depletion behavior. Space jumps freely; the first 0.22-second hold engages the jetpack. Final fuel burns proportionately to zero. Escape releases the mouse and focus loss pauses. Evidence: `Logs/Task16/`; production HUD/art and subjective tuning retain their separate acceptance. Allow 300 seconds for the live build command; its default 30-second request timeout can expire while Unity continues building.

With the Editor closed, `./tools/test-fps.ps1` runs batch validation; `./tools/test-terrain.ps1` uses an isolated copy. For the open Editor, use `unity command run_tests --mode editor --filter SomethingDownThere.EditModeTests --filter_type assembly --async_tests true --project-path "$projectPath" --format json`, then poll `test_status`. Repeat with `--mode playmode --filter SomethingDownThere.PlayModeTests`. Save scene edits first. The status payload can be a JSON string requiring a second parse.

Use isolated Input System devices for automated regression tests. Native Windows reviews share the user's desktop: announce brief input-control periods, verify game focus and repeat checks interrupted by user input. [Validation policy](../docs/scope-and-validation.md#validation-policy). The user cancelled Sandbox setup; Windows repair is not a development prerequisite.

For HUD/menu inspection, use `capture_game_view --source screen` in Play Mode; `screenshot` renders the camera and omits overlay UI. Save the returned base64 PNG under `unity/Logs/`; the capture command's `save_path` is normalized into `Assets/` and imports the image. Capture evidence must not remain in game assets.

Surface recharge is free and instant between the two surface stations. Step into the rim zone to refill; underground overlap cannot recharge. The battery HUD shows risky reserve at 35% and critical at 15%, plus empty-charge and refill titles without subtitles. These are charge bands, not a guaranteed return estimate.

**E at the SELL machine** opens explicit sale rows and Sell All. Each row sells that exact find; opening the menu and Tab inspection never sell anything. **E at the UPGRADES workbench** compares the next owned shovel's scoop width, reach and stroke time. Levels 2-6 cost 10 / 25 / 55 / 100 / 180 credits; purchases cannot skip levels or charge twice for an old offer. Arrows/Enter navigate, mouse wheel scrolls the find list, and Escape closes. The HUD shows credits. Successful trades checkpoint the entire matching world. Approved station art/source and removal: [asset ledger](../docs/asset-ledger.md).

**Fuel reaching zero automatically triggers rescue**, returning to the clear surface anchor with a full battery. Pause has no rescue action or confirmation. Rescue loses carried ordinary finds and charges up to 10 credits, limited to your balance; the HUD reports both. Held digging/thrust must be released after return; excavation, shovel upgrades and collected-find identities are preserved. Lost finds never respawn. Rescue works with an empty battery/wallet and does not require Developer admin; its result is checkpointed. Evidence: `Logs/Task14/` and `Logs/Task35/`.

**Saving:** changes autosave every 10 seconds; trades/rescue request an immediate whole-world checkpoint. **Esc > Save and quit** and the window close button wait for the latest write. Resume appears after terrain collision and discoveries are restored. Saves are outside the build at `%USERPROFILE%/AppData/LocalLow/Something Down There/Something Down There/Save/`; `world.previous.sav` retains the prior checkpoint. Damaged/unsupported saves show recovery options and are never silently reset. Editor play uses a separate `EditorSave/` profile; Editor Stop cannot wait for a new exit checkpoint, so use Save and quit when preserving its very latest changes. Additive tests use isolated temporary profiles. Future save schema/content changes must preserve/migrate existing versions and content keys.

**Esc > Camera comfort:** adjust vertical FOV from 55–90° (default 75°), toggle the steady crosshair (default On), or reset only camera settings. Back/Escape keeps changes and returns to Pause. Arrow keys/Enter and mouse controls work while gameplay stays paused. Preferences live separately in `Preferences/camera-v1.ini` (`EditorPreferences` in the Editor); failed writes retain session values and offer Retry in this panel.

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
