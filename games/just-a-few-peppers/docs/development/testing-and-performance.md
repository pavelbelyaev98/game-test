# Testing, performance, and verification

Status: foundation, crate-handling, and processing checks are installed; finished-food handoff, complete-loop, and save coverage remain planned. [Implementation status](status.md) records delivered results.

## Check the changed work

Follow [AGENTS.md](../../../../AGENTS.md#definition-of-done-and-records) for verification requirements. Select checks for changed behavior using the commands and coverage below; record results/limitations in the task's delivery record. Documentation-only work does not require Unity, and Stage0 has no standing current regression gate.

## Historical baseline

The old standalone Stage0 harness passed **10/10** after relocation on September 5, 2026 using .NET SDK 9.0.314. It tests roast/steam/peel rules, not bulk gameplay. This is retained history, not an instruction to rerun it. Its old scene builder/probe can regenerate Stage0 art and must not be used on new authored content.

## current automated checks to add

Task 1_01 installed Unity Test Framework 1.8.0 and separate runtime/editor/EditMode/PlayMode assemblies, with Input System 1.20.0 and uGUI 2.6.0 on Unity 6000.6.0f1. The saved scene, local gathering, conserved quantities, crate ownership, input interruption/recovery, and task 1_03's tipping/processing use the same commands below. Finished-food handoff, save, and complete-loop coverage remain future work.

## Verified foundation commands

From the repository root, use the checked-in [PowerShell wrapper](../../unity/tools/Verify-Foundation.ps1). It resolves the editor from `ProjectVersion.txt`; `-EditorPath` accepts another installation of the pinned version. Close this project's interactive editor before a batch run.

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

Run only the modes relevant to the change. **Build/Smoke are the default packaged handoff**, using an ordinary Windows x64 Mono player with Unity's Development flag off. Diagnostic variants are opt-in:

| Modes | Player path relative to `unity/` | Purpose |
| --- | --- | --- |
| `Build`, `Smoke` | `Builds/JustAFewPeppers/JustAFewPeppers.exe` | the tester's ordinary playtest; no Unity development profiler/discovery connection. |
| `BuildDevelopment`, `SmokeDevelopment` | `Builds/JustAFewPeppers-Development/JustAFewPeppers.exe` | Development diagnostics when needed; may prompt for firewall access. |

Invoke diagnostic modes with the same wrapper, for example `-Mode BuildDevelopment`. They must not overwrite the ordinary player or become a required extra build for every task. Neither mode implies publishing or final release readiness. No firewall rules are changed by the wrapper.

EditMode/PlayMode use `-batchmode -nographics -runTests -testPlatform <mode> -assemblyNames JustAFewPeppers.<mode>Tests -testResults <absolute XML path>` plus project/log paths. They omit `-quit` so tests finish. Input simulation uses Unity's isolated `InputTestFixture` in PlayMode. The package's entire test suite and Stage0 are not run.

**Automated play is muted.** All PlayMode fixtures set `AudioListener.volume` to zero before loading the yard and restore its previous value in teardown, including failed checks. This also applies when running them through Unity's Test Runner. The opt-in packaged smoke probe mutes before scene `Awake` for both build kinds; ordinary interactive play keeps its audio. Hidden/batch execution alone does not mute the game. Cue dispatch and rate limits remain checked, but audible quality/volume requires human play. EditMode checks do not play the scene. New scene tests should preserve this mute/restore pattern. References: [Unity 6.6 listener volume](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/AudioListener-volume.html) and [before-scene initialization](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/RuntimeInitializeLoadType.BeforeSceneLoad.html).

Build calls `FoundationSceneBuilder.BuildWindows` (`BuildOptions.None`); diagnostics call `BuildWindowsDevelopment` (`BuildOptions.Development`). Both build the exact saved PepperYard scene. `CreateScene` remains available only when that scene is missing; it refuses to overwrite authored work.

Smoke launches the corresponding executable hidden with `-batchmode -foundationSmoke <output directory>`; diagnostics also pass `-expectDevelopment`. The dormant local verification component supports both build kinds and requires batch mode plus the explicit flag. It checks `Debug.isDebugBuild`, scene/menu/input, walking/sprint speed, jump/landing without held repeat, midair pause, simulated focus callbacks, and reset. Task 1_02 adds input-driven crate pickup, local scooping/filling, full-cue limits, loaded sprint/jump, safe parking/recovery, and prototype restart. The probe places the player at deterministic approach points and then uses actual input actions; it does not establish human navigation or enjoyment. It writes fresh results/images and exits. Ordinary interactive launches do not instantiate it. Physical Alt-Tab, OS cursor behavior, and comfort still need human checks. This is not a performance measurement.

`-Mode AuthorHandling` applies the one-time `HandlingSceneAuthoring.Apply` migration to a foundation without handling. It refuses to replace existing handling and is **not** a routine test/build prerequisite. Use the supplied saved scene for play and builds. [1_02 evidence](tasks/1_02_scooping-and-crate-carrying.md#delivery-record--september-6-2026) records the current handling tests, captures, and limitations.

The later `-Mode HoldOnlyScooping` migration removes the former mode action and updates the saved HUD through editor APIs. Normal builds already use that authored scene. Hold-only verification covers release stopping transfers, the former T key having no effect, and pause/focus requiring fresh input; see the [revision evidence](tasks/1_02_scooping-and-crate-carrying.md#hold-only-revision--september-6-2026). The migration uses the pinned [Input System 1.20 RemoveAction API](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.InputActionSetupExtensions.html#UnityEngine_InputSystem_InputActionSetupExtensions_RemoveAction_UnityEngine_InputSystem_InputAction_).

Local ignored evidence: `unity/Logs/Foundation-<mode>.log`, test XMLs, `FoundationSmoke/` for ordinary-player results/captures, and `FoundationDevelopmentSmoke/` for diagnostics. The wrapper checks exit codes and fresh test/smoke results. Actual counts, troubleshooting history, and original package verification remain in the [1_01 delivery record](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026); [build follow-up evidence](tasks/1_01_unity-foundation-and-walkable-scene.md#process-and-build-follow-up--september-6-2026) records the split. Do not copy those changing results into entry pages.

Official version-matched references consulted: [Unity 6.6 input/package selection](https://docs.unity3d.com/6000.6/Documentation/Manual/com.unity.inputsystem.html), [Input System 1.20 actions](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Actions.html), [input testing](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Testing.html), [Unity 6.6 test command line](https://docs.unity3d.com/6000.6/Documentation/Manual/test-framework/run-tests-from-command-line.html), [CharacterController.Move](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/CharacterController.Move.html), [SphereCast](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.SphereCast.html), and [cursor state](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Cursor-lockState.html). The installed package's fixture documentation/source clarified the need for isolated PlayMode input processing. Package versions and dependencies are locked in the project manifest/lockfile. Build-kind references: [Development](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/BuildOptions.Development.html), [None](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/BuildOptions.None.html), and [Debug.isDebugBuild](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Debug-isDebugBuild.html).

## Remaining gameplay coverage

Task 1_03 adds `ProcessingStateTests` (accepted transfers, final partial batches, reservations/accumulation, waiting, tick partitioning, recovery/reset, mixed-command conservation) and `ProcessingSceneTests` (actual E input, raised cascade pose, pause/focus, fresh input, partial jar fill, full-input cue limits, occlusion/range and recovery). The packaged probe follows its existing handling checks with actual scoop/tip loads through partial output, full output, limited intake acceptance, and whole-test recovery/reset. It remains muted. [Task 1_03 evidence](tasks/1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026) owns results and capture details.

`-Mode AuthorProcessing` runs the guarded one-time scene migration; `-Mode TuneProcessingLabels` applies the subsequent label placement correction. Neither is required for normal tests/builds of the supplied scene. Official version-matched references consulted for 1_03: [Unity 6.6 frame delta](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Time-deltaTime.html), [transform pose](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Transform.SetPositionAndRotation.html), [editor scene saving](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/SceneManagement.EditorSceneManager.SaveScene.html), and [Input System 1.20 action press/release semantics](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.InputAction.html). Editor/packages/rendering remain pinned; no additional package was needed.

Task 1_02 consulted the pinned [sphere-cast](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.SphereCast.html), [box-cast](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.BoxCast.html), [clearance check](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.CheckBox.html), [AudioSource](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/AudioSource.html), [audio preload settings](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/AudioImporterSampleSettings-preloadAudioData.html), and [Input System 1.20 actions](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Actions.html). Editor/package pins and rendering remain unchanged.

| Layer | Meaningful coverage |
| --- | --- |
| EditMode / pure rules | Conservation, carrier limits, partial transfers, output reservation/accumulation, exactly-once deposits, monotonic upgrades, completion, and content IDs. Add snapshot validation/round trips in M3. |
| PlayMode | Actual targets and component wiring, scoop/tip commits, pause/focus, carrier recovery, output pickup/deposit, discovery, and later completed-property reconstruction. |
| Packaged Windows build | New scene starts, input works, a load finishes, pause/resume works, saves persist when introduced, and the complete game reaches harvest completion with normal control retained. |
| Human observation | Responsiveness, local depletion continuity, sound, comfort, useful upgrade gain, navigation, and desire to continue. |

Use deterministic tests for quantities and state. Avoid brittle pixel comparisons or tests that merely repeat a private method. Add regression coverage for significant observed failures. A source compile does not establish targeting, scene wiring, or fun.

## Free handling coverage

These are **planned checks**, not new passing results. The [1_02 feedback/revision](tasks/1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026) supersedes fixed-mat acceptance. Preserve useful scoop/processing and muted-automation checks while replacing tests that merely enforce the rejected placement rule.

| Work | Required evidence |
| --- | --- |
| 1_02 crate revision / 1_04 finished carrier | Actual input places/rotates a loaded carrier at multiple chosen ground/worktop/support poses, releases it to fall/collide/settle, then regrabs and tips/deposits. Obstructed careful placement gives clear guidance while deliberate release remains available. Contents survive toppling/recovery; no mat whitelist, accidental food transfer, held-object player launch, or duplicate body. |
| 1_06 loose objects / 2_01 wheelbarrow | Shared grab priority, careful stacks where supported, small-prop drop/toss, movable temporary clutter and lost-object recovery; wheelbarrow release/regrab at chosen positions with easy turning/reversing. Check pause/focus during physical motion without a resume burst or unintended input. |
| M3 saving | Stable IDs, free poses/rotations, arranged stacks, held and falling objects round-trip coherently. Valid arrangements remain; invalid/blocked positions recover without duplicate bodies, reset food or stack explosions. |
| M5–M6 presentation | Real assets retain consistent portable/fixed affordances, matching collision, useful staging space and physically available shortcuts. Stored-food displays do not masquerade as unimplemented loose jars. |
| M8 reliability/performance | Repeated manipulation and reconstruction near boundaries, active/sleeping body counts, disturbed stacks, physics cost, and save/load behavior on recorded hardware. Separate loose bodies from decorative pepper proxies. |

Packaged players must exercise changed handling. Human checks ask which objects/positions/routes worked as expected and which refusals or collisions felt arbitrary; automate conservation/recovery and inspect the actual contact/settling behavior without inventing comfort or fun. Optional prop play cannot excuse excessive forced waiting.

## Machine and equipment-budget coverage

These checks are planned; historical 1_03 passes cover its automatic backend only.

- **1_07:** matched physical/grouped pours, contacts/settling, off-target material recovery, partial final food, bounded body use and measured representation choice.
- **1_08:** actual input-driven mechanism, queued-to-active commit/reservation, interrupted strokes, output blocking, pause/focus/recovery and full food handoff in the ordinary player.
- **2_01–2_02:** proportional credit, split-load equivalence, empty/repeated deposit, separate stored food, unaffordable/repeated purchase, either order, paid pending installation, combined effects and finite affordability with useful work left.
- **M3 onward:** coherent operation/food/Coins/purchase/pose restore, malformed IDs/ownership/balance, backup/write failures, no replayed earnings/charges and completion independent of spending or yard tidiness.

Human checks observe the operator action and the reason for a purchase. A bigger model, larger capacity number or click-to-wait animation alone does not establish a useful or enjoyable improvement.

### Purchased attachment checks

Follow the [installation lifecycle](state-and-saving.md#attachment-ownership-and-installation). These are future requirements, not passing results from this documentation refinement.

| Owner | Required evidence |
| --- | --- |
| 2_01 model | Available → paid/awaiting installation → installed; one charge, stable kit identity and one effect. Interrupted placement, recovery and repeated mount requests cannot duplicate kits or capabilities. Both purchase orders remain viable. |
| 2_01 scene/player | Complete kit appears beside one large mount; shared physical grab/place/drop, forgiving fitting and clear snap/sound/mechanism response. Cards show workflow effect, full price, lifecycle state and location. Old machine works while unmounted; fitting during work signals the safe boundary and preserves active/queued food, reservations and output. Pause/focus needs fresh input. |
| 2_02–2_03 observation | Finding/understanding time, placement attempts/rejections and safe-boundary wait recorded as one-time installation cost. Recurring equal-work handling/travel measured separately before/after, with useful food left. No individual-item haul, remembered shopping list, artificial timers, slowed gathering or extra mandatory supply. |
| 3_01–3_03 save/recovery | Round-trip unowned, paid loose, held/dropped/recovered, fitted-pending and installed states in both purchase orders. Duplicate IDs, contradictory ownership/mount state and blocked/lost kit poses do not create an extra body, charge, effect or food loss. Exercise actual restart and valid-backup recovery. |
| 5_02 / 5_05 presentation | Real kit/mount art keeps fitting forgiving and instructions visible, including muted play. One-time fitting remains distinct from the recurring machine benefit. |

Prototype comic content is limited to at most one optional inexpensive static gag. Observe the ordinary batch loop separately from liking it. Later 5_04–5_05/6_02 checks cover selected messages remaining readable later, no required dismissal/reply, no obscured operating instructions or stacked joke replay after load/skipped milestones, quiet gaps and muted dialogue. No new mail/chat service, random-event scheduler or day-management test system is implied.

## Gates before content production

M2 must meet the [revised scope/feel gate](../scope-and-validation.md#next-experiment-one-pile-one-carrier-one-discovery), with actual sample limits. Compare baseline, each initial purchase and both on equivalent finite food jobs. Coins and both useful choices belong in the tested prototype.

Record the nine current prototype questions separately: physical batch handling, readable food transformation, direct operation, comfortable cycle, food/Coins handoff, meaningful purchase choice, improved-apparatus repetition, natural yard play and finite-food/budget reliability. Measure operator strokes/held effort, transfer/trip counts, internal waiting and stored units separately from one-time installation. The small work area includes physical material, props, direct operation, the two-offer bench and one complete attachment snap; household art, a Grandpa dialogue system, final conversion and disk saves remain later. At most one optional static gag may accompany M2.

In 4_02, follow the [powered-apparatus comparison](../scope-and-validation.md#later-checks-for-the-complete-game): same 96-unit reference job, carrier, nearby work area and handoff before/after the conversion. Record useful operation/material changes, strokes/actions, output collections, travel, wait and complete work with repeated trials. Require improvement beyond variability and repeated food remaining at purchase. The former mandatory distant intake/shorter-haul reveal is retired.

For 2_03, record one internal throughput pass across baseline, each purchase and both. Include direct operator work and budget/purchase timing with gathering, travel, machine wait and output handling. These are diagnostic measurements, not a new shipping statistic or a substitute for actual choice/feel observations.

For M4 onward, check that the last accepted deposit automatically commits completion with every other conserved term empty. A partial final batch must work. Repeated deposits, pause/focus during the transition, and loading a completed snapshot must not replay the gift or require a Finish/table action. The 100% cellar/family display reads the same stored total and never adds a transfer or completion prerequisite.

Before expanding the representative slice in M5 into full content:

- The ordinary loop and upgrade have acceptable play observations; no unresolved progression blockers.
- Current EditMode and PlayMode checks pass, including save/reload where relevant.
- A player build exercises the intended scene and the short arc through harvest completion, including continued control afterward.
- Pausing, partial final batches, output accumulation, safe recovery, and skipped visual milestones work.
- Representative assets and pile density have a measured performance record.
- The tested build, hardware, content configuration, and known limitations are recorded in status.

Run checks appropriate to the change. After they pass, repeat or broaden only for a new failure, changed behavior, or an unresolved concern.

## Performance targets and measurement

Initial target: smooth 60 FPS at 1920×1080 on a recorded development/test PC. This is a planning target, not an established minimum specification. Record CPU, GPU, memory, resolution, build settings, and representative pile/cascade conditions before claiming a pass or setting shipping requirements.

The earlier 64 moving decorative proxies are an interim representation guardrail, not a cap on all physical gameplay. 1_07 compares manageable physical pepper batches and grouped views on the same job, measuring active/sleeping bodies, contact, recovery and frame/allocations. Define ownership explicitly if loose material becomes authoritative. Profile actual carriers, material, mechanism and props separately; do not freeze important objects to preserve a cosmetic-only rule.

Capture frame timing and allocation behavior during repeated gathering and dumping, plus startup/save/load times once those exist. Investigate sustained frame times above the target, repeatable spikes, per-action memory growth, and save stalls. Prefer bounded effects, authored depletion, and reusable assets before engine-scale optimization. Define tighter measured budgets only when a real problem needs them.

## Regression records

| Observed failure | Cause and contract | Regression evidence | Status |
| --- | --- | --- | --- |
| 1_03's first package capture showed an upward cascade and a floating stage label over the HUD. | The low scoop carry pose was reused for tipping; the stage label floated at the HUD's projected height. A dump needs a visible downward handoff and readable status. | Processing PlayMode and packaged smoke check the actual raised pouring edge above the intake; final package captures inspect the cascade and labels. | Fixed: brief raised side pose during the cosmetic tilt; printed tray/station labels moved onto their surfaces. Food still commits before presentation. |
| 1_01 walking barely moved in very fast headless frames despite the Move action reading W. | CharacterController's default 0.001 m minimum discarded small motion steps. Walking must remain responsive across frame rates. | `Assets/JustAFewPeppers/Tests/PlayMode/FoundationSceneTests.cs`, `MovementLookFocusLossAndResetUseActualInput`, failed before the fix and passed afterward. | Fixed: `YardPlayer.ResetTo` sets `body.minMoveDistance = 0`; the saved scene has the same value. |
| 1_01 movement revision's focus test stopped the player when simulated W/Space input activated Quit while unfocused. | Gameplay paused but UI actions still accepted navigation/Submit. An unfocused window must not execute menu commands. | `FoundationSceneTests.MenuSpaceFocusPauseAndResetDoNotLeakJumpInput` reproduces focus loss with movement/jump held; the initial run aborted, then the full suite passed with UI disabled during focus loss. | Fixed: `YardInput.SetPaused` gates UI by focus, and `YardSession` restores only paused-menu input on focus return. |
| 1_02 package capture showed the held crate covering the aimed scoop region when looking down. | The carry anchor followed body yaw but not camera pitch. Broad scoop targeting needs a clear view of the touched face. | `HandlingSceneTests.LocalDepletionInterruptionFullLoadAndDenialCuesUseActualMouseInput` now checks the projected crate rim stays below the target; final packaged scoop/load captures also inspected. | Fixed: authored lower camera-relative carry pose, with scenery tucking. The capture helper also places its temporary HUD in front of scene geometry to match ordinary overlay behavior. |
| Automated handling checks played audible game sounds despite hidden execution. | Tests and the packaged probe exercised real audio sources without muting output. Automation should be silent while retaining cue checks and ordinary play audio. | Existing handling integration and packaged smoke assert zero listener volume after actual scoop cues, and that both sources respect listener volume. See [quiet-test evidence](tasks/1_02_scooping-and-crate-carrying.md#quiet-test-follow-up--september-6-2026). | Fixed: PlayMode fixtures mute before scene loading and restore in teardown; the explicitly requested packaged probe mutes before scene Awake. |

Early input-test failures came from the batch editor's input routing and test setup; using Unity's isolated fixture and moving all simulated-input checks to PlayMode resolved them. No runtime exception filter was introduced. World labels obscured the route in the first package capture; their authored scale was reduced, and the final package capture was inspected again.

Record future important failures with root cause, violated contract, preventing test, and fixing change. Reproduce with a failing test first when practical, and retain it afterward.

Investigate unexpected errors from the current game/editor run. Do not carry over exception filters from the old Stage0 probe or suppress unrelated errors to report a pass.

## Release-candidate check

Test the exact candidate artifact on a clean user-data path: launch, new game, save, exit, continue, controls/settings, full completion, continued completed-yard control, and relaunch into the completed property. Check supported display/input configurations, backup recovery, credits/licenses, and runtime logs. Record the artifact version and unresolved issues. The selected distribution channel's current requirements must be checked when that release task is performed.
