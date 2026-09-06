# Testing, performance, and verification

Status: task 1_01 foundation checks are installed and verified; later current gameplay checks remain planned. [Implementation status](status.md) records delivered results.

## Check the changed work

Follow [AGENTS.md](../../../../AGENTS.md#definition-of-done-and-records) for verification requirements. Select checks for changed behavior using the commands and coverage below; record results/limitations in the task's delivery record. Documentation-only work does not require Unity, and Stage0 has no standing current regression gate.

## Historical baseline

The old standalone Stage0 harness passed **10/10** after relocation on September 5, 2026 using .NET SDK 9.0.314. It tests roast/steam/peel rules, not bulk gameplay. This is retained history, not an instruction to rerun it. Its old scene builder/probe can regenerate Stage0 art and must not be used on new authored content.

## current automated checks to add

Task 1_01 installed Unity Test Framework 1.8.0 and separate runtime/editor/EditMode/PlayMode assemblies, with Input System 1.20.0 and uGUI 2.6.0 on Unity 6000.6.0f1. The saved foundation scene is covered by the verified commands below. Quantity, transfer, save, and complete-loop coverage in this table remains future work.

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

Build calls `FoundationSceneBuilder.BuildWindows` (`BuildOptions.None`); diagnostics call `BuildWindowsDevelopment` (`BuildOptions.Development`). Both build the exact saved PepperYard scene. `CreateScene` remains available only when that scene is missing; it refuses to overwrite authored work.

Smoke launches the corresponding executable hidden with `-batchmode -foundationSmoke <output directory>`; diagnostics also pass `-expectDevelopment`. The dormant local verification component supports both build kinds and requires batch mode plus the explicit flag. It checks `Debug.isDebugBuild`, scene/menu/input, walking/sprint speed, jump/landing without held repeat, midair pause, simulated focus callbacks, and reset; writes fresh results/images; and exits. Ordinary interactive launches do not instantiate it. Physical Alt-Tab, OS cursor behavior, and comfort still need human checks. This is not a performance measurement.

Local ignored evidence: `unity/Logs/Foundation-<mode>.log`, test XMLs, `FoundationSmoke/` for ordinary-player results/captures, and `FoundationDevelopmentSmoke/` for diagnostics. The wrapper checks exit codes and fresh test/smoke results. Actual counts, troubleshooting history, and original package verification remain in the [1_01 delivery record](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026); [build follow-up evidence](tasks/1_01_unity-foundation-and-walkable-scene.md#process-and-build-follow-up--september-6-2026) records the split. Do not copy those changing results into entry pages.

Official version-matched references consulted: [Unity 6.6 input/package selection](https://docs.unity3d.com/6000.6/Documentation/Manual/com.unity.inputsystem.html), [Input System 1.20 actions](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Actions.html), [input testing](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Testing.html), [Unity 6.6 test command line](https://docs.unity3d.com/6000.6/Documentation/Manual/test-framework/run-tests-from-command-line.html), [CharacterController.Move](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/CharacterController.Move.html), [SphereCast](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.SphereCast.html), and [cursor state](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Cursor-lockState.html). The installed package's fixture documentation/source clarified the need for isolated PlayMode input processing. Package versions and dependencies are locked in the project manifest/lockfile. Build-kind references: [Development](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/BuildOptions.Development.html), [None](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/BuildOptions.None.html), and [Debug.isDebugBuild](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Debug-isDebugBuild.html).

## Remaining gameplay coverage

| Layer | Meaningful coverage |
| --- | --- |
| EditMode / pure rules | Conservation, carrier limits, partial transfers, output reservation/accumulation, exactly-once deposits, monotonic upgrades, completion, and content IDs. Add snapshot validation/round trips in M3. |
| PlayMode | Actual targets and component wiring, scoop/tip commits, pause/focus, carrier recovery, output pickup/deposit, discovery, and later save reconstruction/ending. |
| Packaged Windows build | New scene starts, input works, a load finishes, pause/resume works, saves persist when introduced, and the complete game reaches its ending. |
| Human observation | Responsiveness, local depletion continuity, sound, comfort, useful upgrade gain, navigation, and desire to continue. |

Use deterministic tests for quantities and state. Avoid brittle pixel comparisons or tests that merely repeat a private method. Add regression coverage for significant observed failures. A source compile does not establish targeting, scene wiring, or fun.

## Gates before content production

M2 must meet the current [scope/feel gates](../scope-and-validation.md#next-experiment-one-pile-one-carrier-one-discovery), with results and sample limits recorded. Compare equal quantities before and after the wheelbarrow and count the entire workflow.

Record each of the eight interaction-prototype questions separately in 2_03's observation evidence: scoop, local depletion, filling, dumping, visible-wheel motivation, equal-work improvement, finished-batch handoff, and reliable complete storage. Keep the one-corner content boundary; no household display, dialogue, final machine, ending scene, or disk-save requirement enters that gate.

In 4_02, follow the [combined final-upgrade comparison](../scope-and-validation.md#later-checks-for-the-complete-game): use the same final supply, 96-unit quantity, wheelbarrow, and handoff rack, while comparing the original intake route/tier 48 against the revealed nearby intake/tier 96. Record matched repeated trials, loaded travel, output collections, empty walking, waits, total scoop-to-storage time, absolute/percentage changes, and remaining cycles after reveal. Require a substantial loaded-route reduction and a complete-workflow improvement beyond timing variability. A same-path capacity test is diagnostic only; neither capacity nor spectacle replaces this evidence.

For M4 onward, check that the last accepted deposit automatically commits completion with every other conserved term empty. A partial final batch must work. Repeated deposits, pause/focus during the transition, and loading a completed snapshot must not replay the gift or require a Finish/table action. The 100% cellar/family display reads the same stored total and never adds a transfer or completion prerequisite.

Before expanding the representative slice in M5 into full content:

- The ordinary loop and upgrade have acceptable play observations; no unresolved progression blockers.
- Current EditMode and PlayMode checks pass, including save/reload where relevant.
- A player build exercises the intended scene and the short arc through its ending.
- Pausing, partial final batches, output accumulation, safe recovery, and skipped visual milestones work.
- Representative assets and pile density have a measured performance record.
- The tested build, hardware, content configuration, and known limitations are recorded in status.

Run checks appropriate to the change. After they pass, repeat or broaden only for a new failure, changed behavior, or an unresolved concern.

## Performance targets and measurement

Initial target: smooth 60 FPS at 1920×1080 on a recorded development/test PC. This is a planning target, not an established minimum specification. Record CPU, GPU, memory, resolution, build settings, and representative pile/cascade conditions before claiming a pass or setting shipping requirements.

Start with at most 64 simultaneously moving decorative pepper proxies as a tunable guardrail, independent of total harvest. They carry zero authoritative food state. Profile the largest local pile, a full wheelbarrow dump, machine output, and food-display update. If this visual budget cannot sell the action, compare a revised representation before increasing the pool.

Capture frame timing and allocation behavior during repeated gathering and dumping, plus startup/save/load times once those exist. Investigate sustained frame times above the target, repeatable spikes, per-action memory growth, and save stalls. Prefer bounded effects, authored depletion, and reusable assets before engine-scale optimization. Define tighter measured budgets only when a real problem needs them.

## Regression records

| Observed failure | Cause and contract | Regression evidence | Status |
| --- | --- | --- | --- |
| 1_01 walking barely moved in very fast headless frames despite the Move action reading W. | CharacterController's default 0.001 m minimum discarded small motion steps. Walking must remain responsive across frame rates. | `Assets/JustAFewPeppers/Tests/PlayMode/FoundationSceneTests.cs`, `MovementLookFocusLossAndResetUseActualInput`, failed before the fix and passed afterward. | Fixed: `YardPlayer.ResetTo` sets `body.minMoveDistance = 0`; the saved scene has the same value. |
| 1_01 movement revision's focus test stopped the player when simulated W/Space input activated Quit while unfocused. | Gameplay paused but UI actions still accepted navigation/Submit. An unfocused window must not execute menu commands. | `FoundationSceneTests.MenuSpaceFocusPauseAndResetDoNotLeakJumpInput` reproduces focus loss with movement/jump held; the initial run aborted, then the full suite passed with UI disabled during focus loss. | Fixed: `YardInput.SetPaused` gates UI by focus, and `YardSession` restores only paused-menu input on focus return. |

Early input-test failures came from the batch editor's input routing and test setup; using Unity's isolated fixture and moving all simulated-input checks to PlayMode resolved them. No runtime exception filter was introduced. World labels obscured the route in the first package capture; their authored scale was reduced, and the final package capture was inspected again.

Record future important failures with root cause, violated contract, preventing test, and fixing change. Reproduce with a failing test first when practical, and retain it afterward.

Investigate unexpected errors from the current game/editor run. Do not carry over exception filters from the old Stage0 probe or suppress unrelated errors to report a pass.

## Release-candidate check

Test the exact candidate artifact on a clean user-data path: launch, new game, save, exit, continue, controls/settings, full completion, and relaunch after the ending. Check supported display/input configurations, backup recovery, credits/licenses, and runtime logs. Record the artifact version and unresolved issues. The selected distribution channel's current requirements must be checked when that release task is performed.



