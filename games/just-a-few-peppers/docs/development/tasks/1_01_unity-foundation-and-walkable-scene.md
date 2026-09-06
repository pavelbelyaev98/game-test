# 1_01 — Unity foundation and walkable scene

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Give the human playtester a new outdoor test scene they can open and walk around immediately.

**Depends on:** [0_01 — Repository and design baseline](0_01_repository-and-design-baseline.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [M1 contract](../first-playable-task.md) · [Architecture](../../../ARCHITECTURE.md) · [Repository audit](../repository-audit.md) · [Unity and assets](../unity-and-assets.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Inspect actual editor/package versions and current source. Add compatible Input System and Unity test tooling; record installed versions and verified commands. Reuse or retire Stage0 pieces after checking references.
- Create the current scene, camera, collision, keyboard/mouse movement, gameplay/UI input actions, and broad targeting foundation. Use placeholders for mound, crate, station, and rack in a short outdoor route.
- Wire basic pause, focus loss, cursor capture/release, and a reset to safe spawn. Keep new scene/build configuration reproducible and prevent old generators from overwriting it.
- Include on-foot sprint and jump from the foundation: normalized movement, forgiving jump timing, safe collision/landing, and no held menu action causing a jump. Follow the [movement contract](../../look-sound-and-comfort.md#on-foot-movement).

## Acceptance

- The supplied scene opens and runs without missing references or new unexplained errors; movement/look and pause/resume work with the new input setup.
- Compatible test assemblies can run a meaningful scene/input check; record actual evidence and limitations. Do not install a general gameplay framework.
- Sprint and jump use the saved Input System actions and HUD guidance. Check speed, landing, no double/held-repeat jump, jump timing, collision, pause/focus, reset, and the ordinary packaged player.

## Human playtest check

Walk and sprint the route, jump in place and while moving, turn, pause, switch focus, and resume; identify any movement or camera discomfort.

**Outside this task:** Pepper handling, wheelbarrow, saves, polished art, or a full property.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_02 — Scooping and crate carrying](1_02_scooping-and-crate-carrying.md). Stop after this task's handoff unless the developer explicitly requested a larger range.

## Delivery record — September 6, 2026

**Ready for human playtest. Feedback: Not tested.** Task 1_01 is technically complete; M1's pepper-handling loop remains unfinished. Next eligible task is 1_02, with no technical dependency blocker.

### Delivered artifact and behavior

- Scene: `unity/Assets/JustAFewPeppers/Scenes/PepperYard.unity` from the game folder. Windows development player: `unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`; keep its complete adjacent build folder. Both are ready to open without Inspector work.
- An enclosed 18 × 18 m outdoor graybox, safe spawn at the gate, short connected paths, and integrated mound/crate/processor/rack placeholders with colliders, materials, labels, targeting highlights, and HUD descriptions. These objects do not yet transfer peppers.
- CharacterController walking at 3.2 m/s, normalized diagonal input, mouse look at 0.1 degrees/pixel, 72° vertical FOV, pitch limited to ±80°, no head bob or camera shake. Broad selection uses a 0.25 m sphere cast with 3 m reach; the first solid hit blocks targets behind scenery.
- Start in a menu; Enter/click Walk captures the cursor. Esc toggles pause/resume. Pausing disables Gameplay actions, freezes scaled time, clears targeting, and enables UI actions. Focus loss releases the cursor and pauses; focus return requires explicit resume. Recapture discards stale mouse delta. R or the menu returns the player to the gate; falling outside the test area recovers automatically. Reset while paused leaves the menu open. There is no inventory/save state to reset in this task.
- Controls: WASD or arrows walk; mouse looks; Esc pauses/resumes; R returns to gate while walking. In menus, W/S or Up/Down selects, Enter/Space activates, or use the mouse. Quit exits the player (stops Play in the editor).

### Implementation and authoring

New files live under `Assets/JustAFewPeppers/`: `Runtime/` holds session/input, movement, targeting and HUD components; `Content/` holds the `.inputactions` asset and 10 placeholder materials; `Editor/FoundationSceneBuilder.cs` authors the scene and builds the exact saved scene; `Tests/EditMode/` and `Tests/PlayMode/` hold scoped checks. All Unity assets retain editor-generated `.meta` files.

Editor remains **6000.6.0f1** with Built-in rendering. Installed/locked **Input System 1.20.0**, **Test Framework 1.8.0**, **uGUI 2.6.0**, and resolved module dependencies. `activeInputHandler` is Input System only. Gameplay, System, and UI maps use a runtime clone of the authored asset; `InputSystemUIInputModule` drives the actual menu buttons. `testables` includes Input System for its isolated test fixture. UI uses Unity's built-in font; no external asset pack was imported. Primitive placeholders follow the M1–M4 asset policy; an external-asset register is not yet warranted.

The scene creator refuses to overwrite an existing PepperYard. Normal builds never regenerate it. Stage0 has no references from the new scene or runtime; its generator/build entry points now refuse to alter this current project when PepperYard exists. Three deprecated object-search calls in its editor probe were updated to supported unsorted APIs after import exposed the warnings. Stage0 gameplay was neither migrated nor retested and is incompatible with Input System-only play; it remains disposable reference material.

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

### the tester's play checklist and limitations

1. Open PepperYard and press Play, or run the Windows executable; use Enter/click Walk.
2. Walk from the gate past the crate/mound to the processor and rack; approach each and check that its name/highlight appears.
3. Turn, strafe, and walk diagonally; report camera sensitivity, FOV, speed, or collision discomfort.
4. Press Esc, try movement while paused, switch applications and return, then explicitly resume. Check cursor release/capture and camera stability.
5. Press R after walking away; also try Return to gate in the pause menu, then Quit.

Physical Alt-Tab/window focus behavior and subjective camera comfort remain untested by the human playtester. The automated focus evidence invokes Unity callbacks and does not substitute for that check. No pepper handling, processing, saves, audio pass, finished art, or full-property gameplay is present; those remain in subsequent tasks. Human feedback is **Not tested**.

## Process and build follow-up — September 6, 2026

The developer authorized simplifying repeated workflow documentation while preserving discoverable features, and separating ordinary playtest builds from diagnostics. This is maintenance of the 1_01 handoff; no new gameplay task was selected and no play acceptance was supplied.

- **Current handoff:** the same `unity/Builds/JustAFewPeppers/JustAFewPeppers.exe` is now an ordinary Windows player (`BuildOptions.None`), 97,897,371 bytes reported for the complete build. Controls and the saved PepperYard scene are unchanged.
- `BuildWindowsDevelopment` / wrapper mode `BuildDevelopment` writes to `unity/Builds/JustAFewPeppers-Development/JustAFewPeppers.exe`, 160,125,006 bytes reported. Diagnostic builds can still use Unity's development connection and prompt for firewall access; default playtest builds do not use that connection. No firewall rules were changed.
- Verified the wrapper's `Build`, `Smoke`, `BuildDevelopment`, and `SmokeDevelopment` modes. Each player passed the startup/menu, input-driven movement, pause freeze, simulated focus return, resume, and safe-spawn reset checks, including an explicit assertion of its expected `Debug.isDebugBuild` value. The ordinary-player log has zero development-player discovery entries; its rendered yard capture was inspected. There were no C# warnings/errors or game exceptions. The editor build log label was then clarified to print the build kind instead of Unity's ambiguous enum formatting; player contents were unaffected.
- The smoke component is now available in both local build kinds and instantiates only with batch mode plus `-foundationSmoke`. It has no network code and stays dormant in ordinary interactive play. This permits checking the exact ordinary executable handed to the human playtester. Evidence: ignored `unity/Logs/Foundation-Build.log`, `Foundation-Smoke.log`, `FoundationSmoke/result.txt`, `Foundation-BuildDevelopment.log`, `Foundation-SmokeDevelopment.log`, and `FoundationDevelopmentSmoke/result.txt`. [Current commands](../testing-and-performance.md#verified-foundation-commands) describe both paths.
- Workflow entrypoints now route task-relevant reading instead of requiring every common document in full. AGENTS owns working rules, the queue owns task/feedback state, task records own execution evidence, and status owns milestone summaries/history. The design index and feature-to-task table remain the navigation routes for requirements. Shared workflow guidance across AGENTS, queue, start guide, fresh-chat prompt, and milestone status decreased from 4,837 to 3,761 whitespace-delimited words (22%).
- Verified documentation links/anchors, unchanged task order/status and review gates, preservation of all existing task-brief content, and unchanged gameplay specifications, roadmap, state/asset/architecture contracts, scene/material assets, and package versions. No files or features were deleted. Existing EditMode/PlayMode results remain the original foundation evidence; they were not rerun because gameplay was unchanged.

Task status remains **Ready for human playtest**, feedback **Not tested**; **1_02** remains next. Physical focus/cursor comfort still needs the play checklist above. This build split addresses the game's development connection; Unity/editor/helper executables can have separate firewall prompts.

## Human playtester feedback — September 6, 2026

The human playtester reported, “i tested the game so far so good,” and asked whether sprinting and jumping were future updates. This quotation is preserved as historical feedback and contains no personal name. Record the current foundation as **Done / Accepted to continue** on this positive playtest feedback. No specific failed behavior or requested revision was reported; individual checklist steps and comfort ratings were not supplied, so this does not establish separate focus-test or fun-gate results.

Sprinting and jumping are not explicitly specified or scheduled in the existing current briefs. Task 1_01 supplies walking/look; later comfort tasks cover settings and handling, not a promised sprint/jump feature. The question is recorded as a movement design discussion, not authorization to implement or silently expand a future task. Neither mechanic was removed by the documentation cleanup. Task **1_02** remains next; no gameplay work was started by this feedback update.

## Movement revision — September 6, 2026

The developer subsequently requested, “yes please add them from the start and if u see any other common dev tasks please add them as well.” This quotation is preserved as historical feedback and contains no personal name. This explicitly reopens **1_01** for sprint/jump and directly related movement safeguards. Previous acceptance applies to the walking build; the revised movement has not been played by the human playtester. Crate handling and task 1_02 remain outside this revision.

**Ready for human playtest / Not tested:** revised technical criteria are complete. M1 remains in progress, and **1_02** is next; no handling task was implemented.

### Delivered movement and scene

- **Play:** `unity/Builds/JustAFewPeppers/JustAFewPeppers.exe` from the game folder, with its adjacent files. This is the ordinary Windows x64 Mono player, Development off. **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity` relative to the Unity project. No Inspector assembly is needed.
- WASD/arrows walk at 3.2 m/s; either Shift key holds sprint at 5.4 m/s; Space requests a 0.8 m jump. Directional input stays normalized and works during jumps. No stamina, held-repeat jump, midair second jump, sprint FOV change, head bob, or landing shake.
- Small timing aids accept a jump 0.10 s after losing support or 0.12 s before landing. CharacterController motion is subdivided to at most 1/60 s per collision step; gravity integration preserves the jump arc across frame rates. Grounded stepping remains available, ascent stops on ceiling contact, and landing consumes the buffered request once.
- Pause/focus loss freezes an airborne arc; resume continues it. Pending jumps clear on pause/reset, and Space must be released after menu/resume/reset before another jump. R takes priority over a simultaneous jump and clears vertical velocity at the gate. UI actions are also disabled while the window is unfocused; focus return enables the menu and still requires explicit resume.
- Added authored Sprint/Jump actions and a two-line HUD control guide. The editor's `ApplyMovementUpdate` command migrated the existing scene/input through Unity APIs, retaining existing action IDs, asset GUIDs, materials, layout, and scene references. It does not regenerate the yard. The creator also includes these changes for a missing-scene bootstrap.
- Corrected the flattened mound and two appliance shapes to static mesh colliders matching their visible geometry. Yard wall collision extends to 4 m so jumps from props cannot leave the playable area; visible walls retain their existing height. All art remains the existing primitive graybox. No packages, third-party assets, or licenses were added; editor/package pins are unchanged.

### Revision verification

Ran the [wrapper](../testing-and-performance.md#verified-foundation-commands) in `PlayMode`, `EditMode`, `Build`, and `Smoke` modes:

- **PlayMode 11/11 passed.** Existing movement/look/menu/targeting/recovery checks remain, with actual Shift/Space input, speed and diagonal normalization, held/repeated/midair jump rejection, menu-submit isolation, airborne focus/pause/reset, deterministic 30/60/144 FPS jump arcs, grace/buffer expiry, ceiling contact, mound landing, and elevated boundary collision. Tests load the saved scene; controlled collision checks drive its real CharacterController.
- **EditMode 2/2 passed.** Saved scene references/build entry and targeting checks now also verify authored movement actions, HUD instructions, and the three corrected mesh colliders.
- **Ordinary Windows build and smoke passed.** Unity reported **97,900,379 bytes** for the complete build. The exact executable verified nondevelopment configuration, menu resume, walking, configured sprint speed, jump/landing without held repeat, another fresh jump, midair pause freeze, simulated focus return, resume, and gate reset. Runtime DLL SHA-256: `FAD857947D3FB86E0973350CE718CC9C5F74D834D8598CD7B7AC54E11DA5C90E`.
- Final package captures `unity/Logs/FoundationSmoke/01-menu.png` and `03-jump.png` inspected at 1440 × 900; the updated two-line controls are readable. `02-yard.png` is also produced. Raw test XMLs, build/smoke logs, and `FoundationSmoke/result.txt` are local/ignored evidence. The separate development diagnostic build was not rebuilt for this revision.
- The first PlayMode run aborted when simulated input activated a menu button while unfocused. Disabling unfocused UI input fixed it; the retained focus test and the full suite passed afterward. An initial packaged probe deliberately pressed menu Submit while navigating upward, which selected Quit; the probe now tests paused movement without issuing that valid menu command. The corrected probe passed on the rebuilt executable.
- No C# warnings/errors or unexpected gameplay exceptions in the final runs. The editor retains its previously recorded licensing access-token notice and successfully resolves its local entitlement. Source/scene diffs, unchanged input IDs/metas/package pins, documentation links, and whitespace checked. No Stage0 runs or human movement acceptance claimed.

### Revised play checklist

1. Start with Enter/click; walk, hold/release Shift, and sprint diagonally. Check speed and camera comfort.
2. Jump standing still and while sprinting. Hold Space through landing, then release/press again; confirm one jump per press.
3. Jump onto/around the crate and mound; check for floating, snagging, or getting outside the walls.
4. Pause or Alt-Tab during a jump, return, and resume. Confirm the yard freezes and Space used in the menu does not trigger a jump.
5. Press R during a jump, then try Return to gate while paused. Confirm a clean gate reset and normal movement afterward.

Remaining judgement: physical Alt-Tab/cursor behavior and movement comfort need the tester's playtest. There is still no pepper handling or save state; future carrier tasks must preserve contents through movement/recovery. Settings/rebinding remain in 7_02. Stop after this revision's handoff.



