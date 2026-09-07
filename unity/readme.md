# Unity developer guide

The user reviews Windows builds, not Unity scenes. Produce the build at:

`builds/windows/SomethingDownThere.exe`

From the repository root:

```powershell
./tools/build-windows.ps1
```

The build starts from `Assets/Scenes/MainGame.unity`. It is the evolving production game scene. Do not replace it when running its editor builder.

## Source map

- `Assets/Runtime/Player` - movement, input, battery
- `Assets/Runtime/Interaction` - target/station contracts and inventory
- `Assets/Runtime/Terrain` - excavation grid, mesh, collision, boundaries
- `Assets/Runtime/UI` - HUD and menus
- `Assets/Runtime/Validation` - temporary fixtures only
- `Assets/Editor` - scene and build tooling
- `Assets/Tests` - repository-owned checks

## Validation commands

- `./tools/test-fps.ps1` - FPS EditMode/PlayMode checks
- `./tools/test-terrain.ps1` - excavation plus FPS checks in an isolated project

Unity is pinned to `6000.6.0f1` with URP `17.6.0`. Direct dependencies are authoritative in `Packages/manifest.json`; generated logs stay under ignored `unity/Logs/`.
