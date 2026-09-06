# Repository audit and process adaptation

Audited September 5, 2026 after the grouping change. This records the existing project, not the proposed current implementation.

Historical baseline: the table below describes September 5. On September 6, [task 1_01](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026) added the walkable current scene, Input System-only configuration, input/UI/test packages, assembly definitions, and Windows foundation build. Consult that delivery and the [current status](status.md) for today's setup. Stage0 generation/build is now guarded; its gameplay was not retained as a playable current dependency.

## What exists

| Topic | Observed state |
| --- | --- |
| Unity | `6000.6.0f1`, revision `f7f8ed4d1e24`, in [ProjectVersion.txt](../../unity/ProjectSettings/ProjectVersion.txt). Matching editor is installed at the standard Unity Hub path on this machine. |
| Render pipeline | Built-in: GraphicsSettings and all inspected quality levels reference no custom render-pipeline asset. |
| Packages | [Manifest](../../unity/Packages/manifest.json) contains built-in audio, image conversion, IMGUI, particle-system, and physics modules only; matching lockfile. No Input System, URP/HDRP, or Unity Test Framework package. |
| Input | Legacy input, `activeInputHandler: 0`; Stage0 directly uses UnityEngine.Input. |
| Scenes | One authored scene, [ChushkopekStage0.unity](../../unity/Assets/Stage0/Scenes/ChushkopekStage0.unity), also the sole enabled build scene. |
| Prefabs/assemblies | No authored prefab or assembly-definition files found in Assets. Stage0 code uses the default assemblies. |
| Runtime | Plain `PepperState` owned by `Stage0Session`, local station/interaction/presentation components, synthesized sound, IMGUI HUD. Namespace `Chushkopek.Stage0`. |
| Persistence | No gameplay save/load implementation found. Pause and cycle reset exist. |
| Editor tools | Stage0 scene/art generator, Windows build menu, and custom scene probe. These target the historical scene. |
| Tests | Standalone [Stage0.StateTests.csproj](../../unity/tests/Stage0.StateTests/Stage0.StateTests.csproj), target net9.0, links PepperState.cs by project-relative path. No Unity EditMode/PlayMode test assemblies. |
| Local test tooling | .NET SDKs 6.0.428 and 9.0.314 installed; relocated state tests passed using 9.0.314. |
| Build identity | Existing product is Chushkopek - One Pepper, company Interaction Spikes; this is historical identity, not the final game setup. |
| Builds/caches | Library, Logs, Builds and UserSettings moved with the Unity project and remain ignored. |
| Other process examples | No Rust sources, Rust architecture package, or prior AGENTS.md was present in the inspected repository. |

The audit read project files and relevant scripts. It did not launch the editor or rebuild generated assets. The 10/10 state-test rerun confirms the moved standalone project can compile its linked source; it does not verify Unity import or current gameplay.

## Reuse and separation

The old design archives and prototype reports were removed on September 6, 2026 at the user's request. The existing scene/code remains disposable reference material. Reuse targeting, camera comfort, material/audio ideas, or code only where it fits the bulk loop; retire obsolete pieces during implementation after checking retained references. Do not force roasting timers and peel states into the new model or keep retesting the discarded loop.

The [architecture](../../ARCHITECTURE.md) proposes a separate current scene/content area in this same Unity project. No runtime namespace, asset GUID, package, or setting was changed by the move.

## How the old bootstrap was applied

The [original bootstrap](<../../../../instructions/Unity Game Repository Documentation Bootstrap Prompt.md>) was read in full and retained unchanged.

| Practice | Adaptation |
| --- | --- |
| Audit before choosing technology | This document records actual version, pipeline, input, tests, scenes, and saves. |
| Short persistent agent map | Root AGENTS.md links scope, status, architecture, and verification. |
| Clear ownership and persistence | One architecture map and one state/save contract. |
| Incremental delivery with honest tracking | Ordered roadmap, one status file, and one concrete first task. |
| Tests and regressions | One testing document with existing commands, future gates, and a regression section. |
| Documentation synchronized with code | Required in the agent instructions and task exit gates. |
| Full product/feature template trees | Existing current design docs already cover gameplay; no duplicates or empty feature plans are added. |
| Separate ADR for every system | Add a small ADR only if a consequential decision actually needs one. |
| Networking, economy, collection, generic frameworks | Excluded by the current small-game scope. |

Planning defaults are Windows x64, keyboard/mouse, the installed Unity version, Built-in rendering, Input System for new code, one scene, offline saving, and simple C#. M1 installs/configures the compatible input and test packages; the observed legacy setup above has not yet changed. Follow the [Unity and free asset policy](unity-and-assets.md). Test hardware, final content quantities, and shipping requirements are resolved with evidence in later milestones. No missing Rust reference or speculative choice blocks the first prototype.

