# Current status

Task `19` is complete: the requested asset/audio rollback, existing-rendering cleanup, excavation-rim fix and Blender MCP repair. [Completion](completed/19-presentation-rollback.md); [asset inventory and reversal](../asset-ledger.md).

## Current result

- All assistant-added art, fonts, UI sprites, audio, notices and their integration are removed from active assets and the build. User-authorized TMP Essentials/Examples are removed too. Recoverable copies and a complete original-path manifest are outside Unity imports under ignored `unity/Logs/Task19/`.
- Existing scene art is restored. Cast shadows are disabled; 4x MSAA, linear color rendering and clearer compact HUD labels improve the current view. Bedrock walls now meet the green rim underside without overlapping faces.
- Rules require explaining every proposed new asset/audio addition and waiting for explicit user approval before it enters the project, plus a complete file/integration/rollback inventory. General feature requests and assumed necessity are not approval. The user owns terrain art; new art/audio is deferred.
- Blender Lab MCP is configured and verified against the running Blender instance. Restart the Codex extension to expose its tools in a new session; [setup and uninstall footprint](../../unity/readme.md#blender-mcp). Unity continues through the official CLI and Pipeline.

## Latest evidence

- 28/28 EditMode and 26/26 PlayMode tests passed; all five UI input tests passed again after removing TMP resources. The rim/wall regression checks cover all four sides.
- Live excavation: 17 successful cuts beside the rim, inspected from three camera positions; wall top and rim underside both at y=-1. Scene audit: zero missing scripts, imported audio clips or removed-presentation dependencies.
- Windows build succeeded with zero errors and launched in a bordered, resizable 1920x1080 client on the 2560x1440 desktop. No game exceptions appeared in the player log. [Run the executable](../../builds/windows/SomethingDownThere.exe).
- Blender initialization, listing 26 tools, file state, datablocks, hierarchy and screenshot checks passed without changing or saving the Blender scene.
- Evidence lives under `unity/Logs/Task19/`: test JSON, `rim-*.png`, `final-build-report.json`, `window-check.json`, `windows-review.png` and `blender-check.json`.

## Next task and limitations

No task is active. Task `16` remains ready for movement/jetpack feel review; its desktop window behavior is verified. Task `08` is deferred until a new scoped art request. Tasks `05`-`07` remain reopened for production presentation acceptance; restoring the original primitive art does not satisfy that bar. Discovery population, economy, recharge/rescue and disk persistence remain later work. No commit was made.
