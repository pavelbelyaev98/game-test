# 1_01 — Unity foundation and walkable scene

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Give Pavel a new outdoor test scene he can open and walk around immediately.

**Depends on:** [0_01 — Repository and design baseline](0_01_repository-and-design-baseline.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [M1 contract](../first-playable-task.md) · [Architecture](../../../ARCHITECTURE.md) · [Repository audit](../repository-audit.md) · [Unity and assets](../unity-and-assets.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Inspect actual editor/package versions and current source. Add compatible Input System and Unity test tooling; record installed versions and verified commands. Reuse or retire Stage0 pieces after checking references.
- Create the v4 scene, camera, collision, keyboard/mouse movement, gameplay/UI input actions, and broad targeting foundation. Use placeholders for mound, crate, station, and rack in a short outdoor route.
- Wire basic pause, focus loss, cursor capture/release, and a reset to safe spawn. Keep new scene/build configuration reproducible and prevent old generators from overwriting it.

## Acceptance

- The supplied scene opens and runs without missing references or new unexplained errors; movement/look and pause/resume work with the new input setup.
- Compatible test assemblies can run a meaningful scene/input check; record actual evidence and limitations. Do not install a general gameplay framework.

## Pavel's check

Walk the route, turn, pause, switch focus, and resume; identify any movement or camera discomfort.

**Outside this task:** Pepper handling, wheelbarrow, saves, polished art, or a full property.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_02 — Scooping and crate carrying](1_02_scooping-and-crate-carrying.md). Stop after this task's handoff unless the user explicitly requested a larger range.

## Delivery record — September 6, 2026

**Ready for Pavel. Feedback: Not tested.** Task 1_01 is technically complete; M1's pepper-handling loop remains unfinished. Next eligible task is 1_02, with no technical dependency blocker.

### Delivered artifact and behavior

- Scene: `unity/Assets/JustAFewPeppers/Scenes/PepperYard.unity` from the game folder. Windows development player: `unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`; keep its complete adjacent build folder. Both are ready to open without Inspector work.
- An enclosed 18 × 18 m outdoor graybox, safe spawn at the gate, short connected paths, and integrated mound/crate/processor/rack placeholders with colliders, materials, labels, targeting highlights, and HUD descriptions. These objects do not yet transfer peppers.
- CharacterController walking at 3.2 m/s, normalized diagonal input, mouse look at 0.1 degrees/pixel, 72° vertical FOV, pitch limited to ±80°, no head bob or camera shake. Broad selection uses a 0.25 m sphere cast with 3 m reach; the first solid hit blocks targets behind scenery.
- Start in a menu; Enter/click Walk captures the cursor. Esc toggles pause/resume. Pausing disables Gameplay actions, freezes scaled time, clears targeting, and enables UI actions. Focus loss releases the cursor and pauses; focus return requires explicit resume. Recapture discards stale mouse delta. R or the menu returns the player to the gate; falling outside the test area recovers automatically. Reset while paused leaves the menu open. There is no inventory/save state to reset in this task.
- Controls: WASD or arrows walk; mouse looks; Esc pauses/resumes; R returns to gate while walking. In menus, W/S or Up/Down selects, Enter/Space activates, or use the mouse. Quit exits the player (stops Play in the editor).

### Implementation and authoring

New files live under `Assets/JustAFewPeppers/`: `Runtime/` holds session/input, movement, targeting and HUD components; `Content/` holds the `.inputactions` asset and 10 placeholder materials; `Editor/FoundationSceneBuilder.cs` authors the scene and builds the exact saved scene; `Tests/EditMode/` and `Tests/PlayMode/` hold scoped checks. All Unity assets retain editor-generated `.meta` files.

Editor remains **6000.6.0f1** with Built-in rendering. Installed/locked **Input System 1.20.0**, **Test Framework 1.8.0**, **uGUI 2.6.0**, and resolved module dependencies. `activeInputHandler` is Input System only. Gameplay, System, and UI maps use a runtime clone of the authored asset; `InputSystemUIInputModule` drives the actual menu buttons. `testables` includes Input System for its isolated test fixture. UI uses Unity's built-in font; no external asset pack was imported. Primitive placeholders follow the M1–M4 asset policy; an external-asset register is not yet warranted.

The scene creator refuses to overwrite an existing PepperYard. Normal builds never regenerate it. Stage0 has no references from the new scene or runtime; its generator/build entry points now refuse to alter this v4 project when PepperYard exists. Three deprecated object-search calls in its editor probe were updated to supported unsorted APIs after import exposed the warnings. Stage0 gameplay was neither migrated nor retested and is incompatible with Input System-only play; it remains disposable reference material.

### Verification evidence

Verified PowerShell commands from the repository root (see [verification guide](../testing-and-performance.md#verified-foundation-commands)):

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **EditMode 2/2 passed:** saved scene/component/material/input/build references and Input System-only settings; broad near-miss targeting, wall occlusion, and range rejection.
- **PlayMode 5/5 passed:** authored normalized bindings/map isolation; keyboard and mouse menu activation; actual movement/look, paused-input rejection, simulated focus callbacks and recapture; wall collision, fall recovery and menu reset; each scene placeholder's reachable target/highlight/HUD feedback. Tests load the saved scene and use Unity's isolated input fixture, without hardware input or unexpected-log suppression.
- **Windows x64 build succeeded**, Mono development configuration, 160,124,434 bytes reported by Unity. The final build's opt-in smoke run passed startup/menu Submit, input-driven walking, pause freeze, simulated focus return, explicit resume, and reset. Runtime DLL SHA-256: `5DC0C7AD0B5FE8473AC1F3886CAEC9DF5647C7A39DD658E77F2E77AB36097E80`.
- Actual package captures `unity/Logs/FoundationSmoke/01-menu.png` and `02-yard.png` inspected at 1440 × 900. Labels were reduced after the first capture obscured the route. Captures render the same HUD through a temporary camera canvas; ordinary play uses an overlay canvas.
- Local ignored evidence: `unity/Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, `Foundation-Build.log`, `Foundation-Smoke.log`, and `FoundationSmoke/result.txt`. The build and raw logs remain local/ignored; this record preserves the results for future chats.
- Relevant source/meta, documentation-link and whitespace checks completed. No Stage0 test run and no human acceptance claimed.

Initial sandbox launch could not connect to Package Manager IPC. Authorized unsandboxed editor runs resolved packages and completed. Editor logs report an unavailable access token/signature validation notice, then successfully resolve the local Unity Personal entitlement; these did not prevent import, tests or builds. The player logs a D3D12 debug info-queue query notice, then renders on AMD Radeon RX 9060 XT. No game error/exception or C# compilation/deprecation warning remains in the final checks. This is a smoke check, not a measured performance result.

One real regression was found and fixed: the default CharacterController minimum movement threshold discarded tiny per-frame steps in very fast headless runs. Setting `minMoveDistance = 0` preserves movement; the actual-input movement test now passes. Early input-test failures were fixture setup errors; all input simulation was moved to PlayMode under the package fixture. See the [regression record](../testing-and-performance.md#regression-records).

### Pavel's play checklist and limitations

1. Open PepperYard and press Play, or run the Windows executable; use Enter/click Walk.
2. Walk from the gate past the crate/mound to the processor and rack; approach each and check that its name/highlight appears.
3. Turn, strafe, and walk diagonally; report camera sensitivity, FOV, speed, or collision discomfort.
4. Press Esc, try movement while paused, switch applications and return, then explicitly resume. Check cursor release/capture and camera stability.
5. Press R after walking away; also try Return to gate in the pause menu, then Quit.

Physical Alt-Tab/window focus behavior and subjective camera comfort remain untested by Pavel. The automated focus evidence invokes Unity callbacks and does not substitute for that check. No pepper handling, processing, saves, audio pass, finished art, or full-property gameplay is present; those remain in subsequent tasks. Human feedback is **Not tested**.
