# Testing, performance, and verification

Status: task 1_01 foundation checks are installed and verified; later v4 gameplay checks remain planned. [Implementation status](status.md) records delivered results.

## Check the changed work

Documentation-only work needs relevant link and consistency checks. Do not launch Unity, regenerate Stage0, or run gameplay tests for ordinary document edits. The old prototype is disposable and has no standing regression gate for v4.

For new gameplay, run focused rules and integration checks for the changed behavior. Check the player build when the milestone changes packaged behavior. Once appropriate checks pass, repeat or broaden only for changed code/content, a new failure, or an unresolved concern. Preserve verified commands/results in this document as new tooling is implemented.

The AI handles technical verification and gives Pavel an integrated scene/build with a short play checklist. Pavel judges responsiveness, clarity, repetition, and enjoyment. Record what was actually exercised and what remains untested; compile success alone is not a playable handoff. See [Start here](start-here.md).

## Historical baseline

The old standalone Stage0 harness passed **10/10** after relocation on September 5, 2026 using .NET SDK 9.0.314. It tests roast/steam/peel rules, not bulk gameplay. This is retained history, not an instruction to rerun it. Its old scene builder/probe can regenerate Stage0 art and must not be used on new authored content.

## V4 automated checks to add

Task 1_01 installed Unity Test Framework 1.8.0 and separate runtime/editor/EditMode/PlayMode assemblies, with Input System 1.20.0 and uGUI 2.6.0 on Unity 6000.6.0f1. The saved foundation scene is covered by the verified commands below. Quantity, transfer, save, and complete-loop coverage in this table remains future work.

## Verified foundation commands

From the repository root, use the checked-in [PowerShell wrapper](../../unity/tools/Verify-Foundation.ps1). It resolves the editor version from `ProjectVersion.txt`; `-EditorPath` can supply the matching editor elsewhere. Close this project's interactive editor before starting a batch run.

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

Verified September 6, 2026: **2/2 EditMode** tests (scene references/settings and broad targeting occlusion/range), **5/5 PlayMode** tests (normalized bindings, actual menu/movement/look, pause/focus, collision/recovery, and authored target feedback), Windows x64 development build, and its opt-in packaged smoke. Tests filter to `JustAFewPeppers.EditModeTests` / `JustAFewPeppers.PlayModeTests`; the Input System package's own full suite and Stage0 are not run. Input tests use the package's isolated `InputTestFixture` in PlayMode, with synthetic keyboard/mouse devices.

The wrapper's editor test invocations use `-batchmode -nographics -runTests -testPlatform EditMode|PlayMode -assemblyNames <assembly> -testResults <absolute XML path>` with `-projectPath` and `-logFile`; they deliberately omit `-quit` so the runner completes. Build invokes `JustAFewPeppers.Editor.FoundationSceneBuilder.BuildWindows` with `-batchmode -quit` and graphics enabled. Smoke launches the actual `Builds/JustAFewPeppers/JustAFewPeppers.exe` with `-batchmode -foundationSmoke <absolute output directory>`, writes a fresh pass/fail report, captures images, and exits. This flag is development-only; it permits synthetic input in a hidden player and invokes focus callbacks explicitly. Physical Alt-Tab, OS cursor behavior, and comfort still need human observation. No timing/FPS target is established by these checks.

Scene authoring was executed through `JustAFewPeppers.Editor.FoundationSceneBuilder.CreateScene` in the pinned editor. The equivalent wrapper mode is `CreateScene` (available for reconstruction, not rerun after delivery). It refuses an existing `Assets/JustAFewPeppers/Scenes/PepperYard.unity`; builds and subsequent tasks use that saved scene. Do not delete it to regenerate later authored work.

Local evidence lives in ignored `unity/Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, `Foundation-Build.log`, `Foundation-Smoke.log`, and `FoundationSmoke/`. The task's [delivery record](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026) retains results and limitations. The initial sandbox editor failed Package Manager IPC; authorized unsandboxed runs completed. An editor licensing notice was followed by successful entitlement resolution, and the player rendered after a D3D12 debug info-queue notice. Neither was suppressed. Final runs have no new C# warning or game error/exception.

Official version-matched references consulted: [Unity 6.6 input/package selection](https://docs.unity3d.com/6000.6/Documentation/Manual/com.unity.inputsystem.html), [Input System 1.20 actions](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Actions.html), [input testing](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/manual/Testing.html), [Unity 6.6 test command line](https://docs.unity3d.com/6000.6/Documentation/Manual/test-framework/run-tests-from-command-line.html), [CharacterController.Move](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/CharacterController.Move.html), [SphereCast](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.SphereCast.html), and [cursor state](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Cursor-lockState.html). The installed package's fixture documentation/source clarified the need for isolated PlayMode input processing. Package versions and dependencies are locked in the project manifest/lockfile.

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

Early input-test failures came from the batch editor's input routing and test setup; using Unity's isolated fixture and moving all simulated-input checks to PlayMode resolved them. No runtime exception filter was introduced. World labels obscured the route in the first package capture; their authored scale was reduced, and the final package capture was inspected again.

Record future important failures with root cause, violated contract, preventing test, and fixing change. Reproduce with a failing test first when practical, and retain it afterward.

Investigate unexpected errors from the current game/editor run. Do not carry over exception filters from the old Stage0 probe or suppress unrelated errors to report a pass.

## Release-candidate check

Test the exact candidate artifact on a clean user-data path: launch, new game, save, exit, continue, controls/settings, full completion, and relaunch after the ending. Check supported display/input configurations, backup recovery, credits/licenses, and runtime logs. Record the artifact version and unresolved issues. The selected distribution channel's current requirements must be checked when that release task is performed.
