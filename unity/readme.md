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

Development play: launch `builds/windows/SomethingDownThere.exe` for **Continue, New Game, Settings and Quit**, in that order. Continue validates/restores the current slot and is disabled when absent; initial focus then moves to New Game. New Game confirms before replacing an existing slot, retaining its files under `Save/PreviousGames/<timestamp-id>/`. Settings opens Display, followed by Graphics, Audio, Controls and Accessibility; Back returns directly to startup. In play, **Ctrl+Shift+F10** opens Developer admin (also **Esc > Developer admin**). Hold **Ctrl+Shift** with **1-6** (number row or numpad) to select a shovel, **R** to refill, **X** to toggle buried-find X-ray, or **Home** to return to the rim. The panel also provides **Unlimited battery**, **Restore normal rules**, and confirmed **Reset ground...**. Plain keys have no admin effect. Overrides last for the session and preserve owned progression; no special launcher or launch flag exists.

The approved grass covers the whole original land patch above continuous turf. Taller curved blades have travelling wind and anchored roots; grass clears with its soil support and regenerates from reset/restored terrain. Distant grass keeps every blade with simpler bends and compact instanced rendering. The clear bright cyan sky retains the approved sun. Clouds, scenery trees, river/water and decorative ground rocks are removed; buried collectible rocks remain. Aim down and hold **LMB**; aim at walls to widen or tunnel. Cuts follow the struck surface and leave broad angled floors with uneven edges; overlapping strokes deepen and widen them. Shovel reach increases through **3.0 / 3.2 / 3.4 / 3.6 / 3.8 / 4.0 m**, with a hard 4 m maximum shown on the HUD. Nominal cut widths increase gently from 0.692 to 1.619 m (127: about 40% less soil per stroke, with reach/cadence unchanged); separate speed/strength upgrades are planned under Task `25`. Hold **Space** to fly; after thrust in this flight, release to fall and press again to resume immediately. Excavation persists across quit/reload.

New games have only 336 buried rocks (one common Rock item with three appearances). Task `140` puts 264 at 0.65-1.1 m centre depth across the top layer, including the starting rim; 72 rocks remain deeper. Bottle spawning is temporarily disabled, with their assets and save compatibility retained. New rocks sell for 2 credits: five fund the first shovel upgrade, and the shallow pool can fund the entire existing shovel track. Saved item prices remain historical. Existing saves keep their population; choose **New Game** for this rock-only distribution. New Game archives the previous slot after its confirmation. Rock A has one hollow; B/C have solid uneven tops. Rocks use seeded full 3D rotation; saved appearance keys and poses persist. The [rock source guide](../art/photo-rock/README.md) owns dimensions/value/counts, model replacement and the Sync Rock Models menu. Existing legacy saves retain their 96 identities, positions and historical values, mapped to current visuals. These are replaceable trial models, not final art; use the [model/reference/spec replacement guide](../docs/development/replacing-find-models.md). **X-ray** shows markers only. Current trial finds use catalog-authored metre dimensions and real-mesh exposure samples, gentler shovels from `31`, and **60%** required exposure from `110`. Hold **LMB** directly on a visible ineligible bottle or rock to clear its covering soil. A successful stroke that finishes uncovering the same already-aimed item now collects it immediately (`131`), after checking 60% exposure, visibility, 3 m reach and inventory space. Held/toggle Dig also collects an already eligible aimed item immediately, even while the shovel is cooling down (`136`); the old recognition delay is removed. A fresh deliberate press uses the same eligibility rules. A wide shovel scoop or physical drop leaves off-aim finds in the world. Aimed pickup uses the centre crosshair independently of scoop radius. Grounded walking also collects fully uncovered nearby floor finds, with obstruction/full-bag checks; intentionally dropped/thrown finds are excluded until leaving their vicinity and returning. Successful pickup pulls a nonphysical copy directly toward the player over 0.18 seconds with at most 15% shrink and no spin/bob; inventory commits immediately. Active toggle digging also collects directly aimed eligible finds and continues afterwards; a stop press performs no action. RMB lifts/drops an eligible physical item; a fresh LMB throws it (bottles 8 m/s, rocks 4 m/s, editable source `throw_speed`). Handling keeps the object in the world, works with a full bag and blocks digging until released. A throw clears held/toggle intent; saved held objects restore released at their saved world pose. After short pickup recovery, the same hold/toggle can continue digging or collect the next directly aimed eligible find. E remains for stations. Pickup messages name the find; **Tab** lists collected items. X-ray cannot bypass covering soil, exposure or capacity. Reset ground reburies uncollected items and preserves collected ones. Task `110` removes cans/bricks and lets detached bottles fall and settle; `114` also integrates large ordinary rock physics; saves retain their current pose/released state, and ground reset reanchors reburied bottles. Task `109` supplied the stable catalog; `09`/`105` retain final model/style acceptance, and `22` verifies release content/access.

The normal build command uses `BuildOptions.Development` for in-game admin access. **Build Windows Release Player** in the same Tools menu writes the same executable path without development access. Both use `DesktopInstance` before scene loading: repeated Windows launches request the existing window and exit quietly, independently of Steam. Keep Unity's `forceSingleInstance` disabled because it displays a fatal-error dialog. Profile contention still offers a clear retry/back/quit screen, with OS details retained only in logs. `Debug.isDebugBuild` gates admin; it requires no Windows elevation. Task `22` must verify release exclusion and ordinary progression before production; [release gate](../docs/features/backlog/excavation-terrain.md#task-22---production-release-gate). Keep durable tooling; do not add disposable review launchers/files.

With the Editor closed, use `./tools/build-windows.ps1`. Task `16` covered resizing and native Space tap/hold/restart/depletion behavior; `123` now owns native-borderless startup and saved display choices. Space jumps freely; the first 0.22-second hold engages the jetpack. Final fuel burns proportionately to zero. Escape releases the mouse and focus loss pauses. Evidence: `Logs/Task16/`; production HUD/art and subjective tuning retain their separate acceptance. Allow 300 seconds for the live build command; its default 30-second request timeout can expire while Unity continues building.

With the Editor closed, `./tools/test-fps.ps1` runs batch validation; `./tools/test-terrain.ps1` uses an isolated copy. For the open Editor, use `unity command run_tests --mode editor --filter SomethingDownThere.EditModeTests --filter_type assembly --async_tests true --project-path "$projectPath" --format json`, then poll `test_status`. Repeat with `--mode playmode --filter SomethingDownThere.PlayModeTests`. Save scene edits first. The status payload can be a JSON string requiring a second parse.

Use isolated Input System devices for automated regression tests. Native Windows reviews share the user's desktop: announce brief input-control periods, verify game focus and repeat checks interrupted by user input. [Validation policy](../docs/scope-and-validation.md#validation-policy). The user cancelled Sandbox setup; Windows repair is not a development prerequisite.

For HUD/menu inspection, use `capture_game_view --source screen` in Play Mode; `screenshot` renders the camera and omits overlay UI. Save the returned base64 PNG under `unity/Logs/`; the capture command's `save_path` is normalized into `Assets/` and imports the image. Capture evidence must not remain in game assets.

Saving performance: **Tools > Something Down There > Validation > Build Save Performance Player** builds a separate release fixture at `builds/validation/saving/SavePerformance.exe`. The generated validation scene is removed after building; the normal Windows executable is untouched. This fixture loads MainGame with isolated saves under its own `Evidence/`, measures fresh 0.41 m and late 0.96 m cuts with overlapping checkpoints/purchases for 10 minutes each after 60-second warm-ups, then saves and exits. Activate its initialized window before measurement; reject runs with unfocused/menu frames. Reports include Stopwatch frame intervals, capture, grid/mesh/discovery costs, encoding, write/flush/replacement, durability and private memory. Unsupported allocation counters are unavailable, not evidence of zero allocation. `-saveProfileSeconds 5` runs a short fixture smoke check; it is not a qualification run. The normal player has no benchmark launch flag.

Aim at the upgrade station and press **E** to open **Workshop**. Hover or select a Shovel, Backpack or Fuel tank row to see its effects on the right, then use the purchase button. Backpack levels hold 10 / 15 / 20 / 30 / 40 finds; shared fuel capacity is 100 / 150 / 200 / 300 / 400. Both new tracks cost $6 / $14 / $28 / $48 independently. Buying capacity keeps current fuel. Choose **Refill fuel** for explicit instant service: $1 per 100 fuel added, rounded up to whole amounts with a $1 minimum; when money is short, the quote offers the affordable partial fill. Digging uses 1 fuel per accepted stroke: a starter tank supports 100 digs before thrust, versus 50 previously. Both cheaper service and lower consumption apply immediately to Continue. Walking through the surface area never refills or bills. The battery HUD retains risky reserve at 35% and critical at 15%. Bottom-center **LOW FUEL** appears yellow at 35%, then **FUEL CRITICAL** red at 15%; these use your owned tank capacity and remain visible near the workshop until refilled. They describe charge, not a guaranteed return estimate. Save v5 preserves whole balances, owned capacity levels, slots and exact charge; Continue supports v1–v4 saves without resetting their world. Legacy fractional balances round up once and future saves contain no fraction. At critical fractional fuel, successful service reaches the displayed final tank level before charging; 25/85/100 fuel costs $1; 150 costs $2. The HUD and all money values use `$` with whole numbers.

**E at the SELL machine** opens explicit sale rows and Sell All. Each row sells that exact find; opening the menu and Tab inspection never sell anything. **E at the UPGRADES workbench** compares the next owned shovel's scoop width, reach and stroke time. Levels 2-6 cost 10 / 25 / 55 / 100 / 180 credits; purchases cannot skip levels or charge twice for an old offer. Arrows/Enter navigate, mouse wheel scrolls the find list, and Escape closes. The HUD shows credits. Successful trades checkpoint the entire matching world. Approved station art/source and removal: [asset ledger](../docs/asset-ledger.md).

**Fuel reaching zero automatically triggers rescue**, returning to the clear surface anchor with a full battery. Pause has no rescue action or confirmation. Rescue loses carried ordinary finds and charges up to 10 credits, limited to your balance; the HUD reports both. Held digging/thrust must be released after return; excavation, shovel upgrades and collected-find identities are preserved. Lost finds never respawn. Rescue works with an empty battery/wallet and does not require Developer admin; its result is checkpointed. Evidence: `Logs/Task14/` and `Logs/Task35/`.

**Saving:** changes autosave every 10 seconds; trades/rescue request an immediate whole-world checkpoint. **Esc > Save and quit** and the window close button wait for the latest write. Resume appears after terrain collision and discoveries are restored. Saves are outside the build at `%USERPROFILE%/AppData/LocalLow/Something Down There/Something Down There/Save/`; `world.previous.sav` retains the prior checkpoint. Damaged/unsupported saves show recovery options and are never silently reset. Editor play uses a separate `EditorSave/` profile; Editor Stop cannot wait for a new exit checkpoint, so use Save and quit when preserving its very latest changes. Additive tests use isolated temporary profiles. Future save schema/content changes must preserve/migrate existing versions and content keys.

**PC settings:** Display includes window mode/resolution, VSync, FPS cap (30–240 or Unlimited) and an FPS readout. Graphics shows only TBD pending individual review. Internal graphics defaults use 150%, MSAA 8×, full textures and forced filtering. Audio contains Master volume only. VSync disables the cap while active; the dropdown reads Automatic without an arrow. New preferences use native-resolution Borderless, VSync Off and a 144 FPS cap. Mode/resolution changes immediately open a standalone Keep changes dialog with a 15-second countdown; Escape, timeout or focus loss reverts. All ordinary changes apply immediately and save at settings boundaries in `Preferences/game-v1.json` (independent of world, input and camera files). Back and Reset category are matching compact buttons together at bottom-left. Reset is disabled on the empty Graphics page and otherwise local to its category; failed writes keep session values and offer Retry. [Values, research and review views](../docs/development/ui-review/common-settings.md).

**Startup Settings > Accessibility / Esc > Settings > Accessibility:** adjust vertical FOV from 55–90° (default 75°), toggle the steady crosshair (default On), or reset only camera settings. Back/Escape keeps changes and returns directly to the originating startup/Pause screen with Settings focused. Arrow keys/Enter and mouse controls work while gameplay stays paused. Preferences live separately in `Preferences/camera-v1.ini` (`EditorPreferences` in the Editor); failed writes retain session values and offer Retry in this panel.

**Startup Settings > Controls / Esc > Settings > Controls:** rebind movement, Dig/collect/throw, Lift/drop find, Jump/jetpack, held Crouch/Sprint, Interact, Inventory and Pause to keyboard/mouse buttons. Mouse sensitivity (0.10–3.00×) and horizontal/vertical inversion sit above the same scrolling list as Digging mode and the Movement/Actions bindings; Back/Escape returns directly to the originating startup/Pause screen. Select an action, release its opening button, then press the new control; Escape cancels. Conflicts offer Cancel or Swap bindings, exchanging the two assignments. Hold digging is default; Toggle starts/stops on fresh Dig presses. Menus, focus loss, rescue and world changes stop digging until release and a fresh press. Menu arrows/Enter and Escape remain available. Input preferences persist independently in `Preferences/input-v1.ini` (`EditorPreferences` in the Editor); Reset category restores bindings, Hold and mouse defaults while preserving camera/world state, and failed writes offer Retry. Hold Left Shift for modest sprinting (35% faster than walking); crouch takes priority. The controls above describe defaults; Pause and station prompts show active bindings.

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
