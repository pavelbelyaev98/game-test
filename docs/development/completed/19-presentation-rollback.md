# Task 19 - presentation rollback and rendering cleanup

Status: done. [Scope](../../features/backlog/presentation-audio.md); [complete removal inventory and reversal](../../asset-ledger.md).

The user canceled the asset pass and reserved terrain art for separate work. Removed all added art/audio, fonts, UI sprites, notices, scene instances and runtime integration; also removed TMP sample resources after the user's follow-up. Recovery copies and every original file path remain under ignored `unity/Logs/Task19/`, outside imports/builds. Rules now prohibit unrelated decoration/audio and require independently reversible asset integrations.

The restored main scene uses no cast shadows, 4x MSAA, linear color rendering and more readable existing HUD labels. All four bedrock walls meet the rim underside at y=-1, eliminating overlapping vertical faces after excavation. Existing gameplay controls are preserved.

- Validation: 28/28 EditMode and 26/26 PlayMode tests passed; five UI tests passed again after TMP removal. Scene audit found zero missing scripts, imported audio clips or removed-presentation dependencies.
- Presentation: 17 successful live rim cuts inspected from three viewpoints; Windows launch verified a bordered, resizable 1920x1080 client on the user's 2560x1440 desktop with no game exceptions.
- [Windows build](../../../builds/windows/SomethingDownThere.exe) succeeded with zero errors. Evidence: `unity/Logs/Task19/` test results, rim captures, build report and window capture.
- Official Blender Lab bridge installed in isolated `.tools/blender-mcp/`; initialization, 26-tool listing, scene reads and screenshot passed without art changes. [Setup/reversal](../../../unity/readme.md#blender-mcp). Restart Codex to load the configured server.

Limitation: original primitive art remains by the user's rollback request; production art Task `08` is deferred and Tasks `05`-`07` retain their reopened acceptance status. Task `16` still needs movement/jetpack feel review. No commit.
