# Current status

## Current result

Tasks `01`-`06` are complete. The evolving main game scene has FPS controls, an untouched finite excavation volume, permanent boundaries, surface anchors, primitive scenery, and session-persistent cuts. Inventory/discoveries/economy are not integrated yet.

## Next task

Task `07` is ready: [session inventory identities and values](../features/backlog/inventory.md).

## Playable build

- Windows executable: [SomethingDownThere.exe](../../builds/windows/SomethingDownThere.exe)
- Rebuild command: `./tools/build-windows.ps1`
- Latest build completed successfully from `Assets/Scenes/MainGame.unity` after Task `06`.

## Evidence and limitations

- Unity `6000.6.0f1` / URP `17.6.0`: 11 EditMode and 20 PlayMode checks passed for FPS and terrain integration.
- Terrain uses stepped 0.5 m cells and resets when the application closes. Finds, detector, transactions, recharge, rescue, and saving are pending.
- Unity MCP remains unavailable; automated checks do not replace manual feel/visual review of the Windows build.
