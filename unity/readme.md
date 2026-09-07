# Unity folder

1. Keep this folder focused: assets, scripts, tests, packages, and settings.
1. Use `Packages/manifest.json` as source of truth for package versions.
1. Preserve `.meta` files on asset moves and copies.
1. Prefer editor-side authoring for references over manual scene YAML edits.
1. FPS baseline: create/track player movement, digging input, camera rig, and interaction anchors before mechanics complexity.

## Run the FPS validation scene

1. Open this folder with Unity `6000.6.0f1` and let package import/compilation finish.
2. Open `Assets/Scenes/FpsValidation.unity` and press Play.
3. Use WASD + mouse, LMB dig, E interact, hold Space jetpack, Tab inventory, Escape pause/back. Menus support pointer clicks, arrows and Enter.
4. The block ahead takes three hits; the sphere behind it is collectible. Extra spheres to the right exercise inventory capacity. Two stations to the left exercise sale/upgrade menus; their nearby trigger recharges the battery.

This is a primitive control-validation yard. Its disposable blocks, exposed-find flags, sale/removal commands and one free cadence upgrade are test adapters. It has no production terrain, detector, money, save data, rescue or full tutorial. A scene reload resets fixture state. The production loop remains in `docs/idea-at-a-glance.md` (full detail in `docs/idea.md`).

If the scene is missing, use `Tools > Something Down There > Create FPS Validation Scene`. The builder creates a new scene only if the destination does not exist; otherwise the menu opens the existing scene. The batch setup entrypoint is `SomethingDownThere.Editor.FpsValidationSceneBuilder.InitializeProject`.

## Source map and wiring

- `Assets/Runtime/Player/`: `FpsPlayer`, inspector tuning, action reader and battery.
- `Assets/Runtime/Interaction/`: target/station contracts and session inventory.
- `Assets/Runtime/UI/`: HUD and pause/inventory/station menus.
- `Assets/Runtime/Validation/`: primitive target and station adapters.
- `Assets/Editor/`: deterministic scene/project setup.
- `Assets/Tests/EditMode/` and `Assets/Tests/PlayMode/`: automated checks.
- `Assets/Scenes/FpsValidation.unity`: generated validation scene with stable `.meta` references.

For a new player, use a yaw-only root with CharacterController (height 1.8, radius 0.3, center Y 0.9), a child Camera at eye height, `FpsPlayer`, and `FpsHud`. Put player colliders on Ignore Raycast; include all world blockers in the player's mask. Target components may be on the hit collider or its ancestors. The HUD creates its own Input System EventSystem when none exists; additional scenes must use a compatible Input System UI module, with Enter Submit and player-owned Escape handling. One active player/HUD/menu owner is supported.

## Validation

Run from the repository root while this Unity project is closed:

```powershell
./tools/test-fps.ps1
```

Pass `-Mode EditMode` or `-Mode PlayMode` for a targeted rerun, and `-UnityEditor 'C:/path/to/Unity.exe'` for a different installation path of the pinned editor. Logs and NUnit XML go into ignored `unity/Logs/FpsValidation-*` directories. Active Input Handling is serialized as Input System only. `Packages/manifest.json` and `packages-lock.json` record the compatible dependency set.

## Rendering and package baseline (M01)

Addressables `4.0.1`, Cinemachine/Timeline `6.6.0`, URP `17.6.0`, and IDE integration `2.0.28` are installed at the user's request. Input System `1.20.0`, Test Framework `1.8.0`, and Unity UI `2.6.0` remain current for this editor; TextMeshPro comes through Unity UI. Exact version decisions and test evidence are in `docs/development/status.md` at the repository root.

URP `17.6.0` is active through `Assets/Settings/SomethingDownThereURP.asset` and its Universal Renderer. Graphics settings assign that asset globally, while all quality levels inherit it. Package import also generated `Assets/UniversalRenderPipelineGlobalSettings.asset`, an empty `Assets/DefaultVolumeProfile.asset`, their metadata, and `ProjectSettings/ShaderGraphSettings.asset`; preserve those settings/references. The validation scene's primitive mesh renderers use URP Lit. No Addressables content or Timeline sequence was added in this refresh.
