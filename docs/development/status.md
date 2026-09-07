# Current status

## Current result

Tasks `01`-`07` are complete. The main game scene has FPS controls, finite excavation, permanent boundaries, surface anchors and session-persistent cuts. Its 10-slot inventory now carries immutable identities and sale values, with inspection-only Tab UI. Collectible finds and the economy are pending.

## Next task

Task `08` is ready: [terrain-exposed authored finds](../features/backlog/discovery-collection.md).

## Playable build

- Windows executable: [SomethingDownThere.exe](../../builds/windows/SomethingDownThere.exe)
- Rebuild command: `./tools/build-windows.ps1`
- Task `07` Windows build succeeded from `Assets/Scenes/MainGame.unity`; log: `unity/Logs/WindowsBuild.log`.

## Evidence and limitations

- Unity `6000.6.0f1` / URP `17.6.0`: 21 EditMode and 22 PlayMode checks passed, including inventory identity/capacity, UI/input and FPS/terrain regressions. Results: `unity/Logs/FpsValidation-20260907-214931-124/`.
- Main-scene inventory remains empty until Task `08`; populated inspection and collection/removal are covered through validation fixtures. All state remains session-only.
- Unity MCP remains unavailable; no scene edits were needed. Manual feel/visual review of this build remains pending.
