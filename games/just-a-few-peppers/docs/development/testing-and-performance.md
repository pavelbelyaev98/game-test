# Testing, performance, and verification

Status: foundation, raw/finished handling, processing and complete stored-food loop checks are installed; save coverage remains planned. [Implementation status](status.md) records delivered results.

## Check the changed work

Follow [AGENTS.md](../../../../AGENTS.md#definition-of-done-and-records) for verification requirements. Select checks for changed behavior using the commands and coverage below; record results/limitations in the task's delivery record. Documentation-only work does not require Unity, and Stage0 has no standing current regression gate.

## Historical baseline

The old standalone Stage0 harness passed **10/10** after relocation on September 5, 2026 using .NET SDK 9.0.314. It tests roast/steam/peel rules, not bulk gameplay. This is retained history, not an instruction to rerun it. Its old scene builder/probe can regenerate Stage0 art and must not be used on new authored content.

## current automated checks to add

Task 1_01 installed Unity Test Framework 1.8.0 and separate runtime/editor/EditMode/PlayMode assemblies, with Input System 1.20.0 and uGUI 2.6.0 on Unity 6000.6.0f1. The saved scene, gathering, conserved quantities, carrier ownership, input interruption/recovery, tipping/processing and 1_04's finished-food handoff use the same commands below. Save coverage remains future work.

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

### Windows editor launch context

On this development host, restricted-sandbox editor launches have repeatedly failed during startup, including failure to connect to Package Manager's local IPC server. The same pinned editor and wrapper completed authoring, tests and builds through approved execution outside the sandbox. For wrapper modes that launch **Unity.exe**, request the tool's narrowly scoped outside-sandbox execution from the first attempt; do not repeat the known failing launch just because a new chat began. Continue ordinary file inspection and the packaged `Smoke`/`SmokeDevelopment` modes in the normal context unless their own evidence shows a problem. Outside-sandbox execution does not mean Windows administrator elevation, and the wrapper already launches hidden.

September 6 user report: a `Unity.Licensing.Client.exe` application-error dialog appeared shortly after the implementation prompt, showing `0xe0434352`. That code identifies a [CLR exception](https://learn.microsoft.com/en-us/shows/inside/e0434352), not its underlying cause. The failed sandbox launch and subsequent successful editor runs make the launch context a suspected cause; no matching Windows crash event/dump was found in the available checks, so the dialog's exact exception remains unconfirmed. Later logs resolve the existing entitlement and complete verification. This is tooling feedback, not failed gameplay acceptance or evidence that Unity needs reinstalling. If it recurs outside the sandbox or during an ordinary Hub launch, inspect the matching editor and [licensing-client logs](https://docs.unity3d.com/6000.6/Documentation/Manual/log-files.html) before choosing a repair. Do not rerun Unity merely to reproduce a disruptive startup dialog.

### Command behavior and evidence

Invoke diagnostic modes with the same wrapper, for example `-Mode BuildDevelopment`. They must not overwrite the ordinary player or become a required extra build for every task. Neither mode implies publishing or final release readiness. No firewall rules are changed by the wrapper.

EditMode/PlayMode use `-batchmode -nographics -runTests -testPlatform <mode> -assemblyNames JustAFewPeppers.<mode>Tests -testResults <absolute XML path>` plus project/log paths. They omit `-quit` so tests finish. Input simulation uses Unity's isolated `InputTestFixture` in PlayMode. The package's entire test suite and Stage0 are not run.

**Automated play is muted.** All PlayMode fixtures set `AudioListener.volume` to zero before loading the yard and restore its previous value in teardown, including failed checks. This also applies when running them through Unity's Test Runner. The opt-in packaged smoke probe mutes before scene `Awake` for both build kinds; ordinary interactive play keeps its audio. Hidden/batch execution alone does not mute the game. Cue dispatch and rate limits remain checked, but audible quality/volume requires human play. EditMode checks do not play the scene. New scene tests should preserve this mute/restore pattern. References: [Unity 6.6 listener volume](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/AudioListener-volume.html) and [before-scene initialization](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/RuntimeInitializeLoadType.BeforeSceneLoad.html).

Build calls `FoundationSceneBuilder.BuildWindows` (`BuildOptions.None`); diagnostics call `BuildWindowsDevelopment` (`BuildOptions.Development`). Both build the exact saved PepperYard scene. `CreateScene` remains available only when that scene is missing; it refuses to overwrite authored work.

Smoke launches the corresponding executable hidden with `-batchmode -foundationSmoke <output directory>`; diagnostics also pass `-expectDevelopment`. The dormant local verification component supports both build kinds and requires batch mode plus the explicit flag. It checks `Debug.isDebugBuild`, scene/menu/input, walking/sprint speed, jump/landing without held repeat, midair pause, simulated focus callbacks, and reset. Task 1_02 adds input-driven crate pickup, local scooping/filling, full-cue limits, loaded sprint/jump, free placement/drop/recovery, and prototype restart. The probe places the player at deterministic approach points and then uses actual input actions; it does not establish human navigation or enjoyment. It writes fresh results/images and exits. Ordinary interactive launches do not instantiate it. Physical Alt-Tab, OS cursor behavior, and comfort still need human checks. This is not a performance measurement.

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

### Finished-food handoff coverage

`FinishedFoodStateTests` verifies complete 1/25/107-unit jobs, final partial loads, exact-once deposits, active reservations, output accumulating while the carrier is away, tier capacity, invalid poses, recovery/reset and 4,000 mixed commands. It retains the model's atomic optional-pose hand-switch checks, although current gameplay requires explicit release. `FinishedFoodSceneTests` uses actual RMB/E/G/Z/X actions for grabbing/releasing, occupied-hand collection refusal, ground/worktop placement, jar/fixture targets, movement interruption, drop/pause/recovery, preservation of raw arrangements, casual set-down beside the rack and repeated deposits. A grab aimed at another carrier first releases the held one without automatically taking the target or transferring food. Public gathering/time commands prepare its batches; the existing suites retain real scoop/tip and wall/contact checks.

The packaged probe retains its real scooping/processing checks, then completes nine tip/receive/handoff cycles for all 107 units, including the last eleven. This complete-job section prepares raw loads through public gathering commands but uses ordinary input and unaccelerated station time for tipping, receiving and handoff. It checks finished-carrier sprint/jump, quiet rotated ground/worktop placement, drop/focus/contact/recovery, exact-once credit, automatic empty return and final partial stored fill. Its original timeout was 150 seconds for these cycles (1_06 extends it to 210); this is not a measured human route or full-cycle balance claim. Results and captures belong in the [1_04 delivery record](tasks/1_04_finished-carrier-and-storage-rack.md#delivery-record--september-6-2026).

`-Mode AuthorFinishedFood` applies the guarded one-time saved-scene addition; normal play/tests/builds do not run it. Its target/reference checks distinguish the empty docked carrier's disabled collider from the permanent receiving fixture. The fixture reserves automatic-return space. Official matching references consulted: [Unity 6.6 scene saving](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/SceneManagement.EditorSceneManager.SaveScene.html), [penetration queries](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.ComputePenetration.html), and [Input System 1.20 press/release semantics](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.InputAction.html). Pins and the ordinary/development build separation remain unchanged.

### Raw-crate coverage

The [1_02 quiet-placement delivery record](tasks/1_02_scooping-and-crate-carrying.md#quiet-placement-delivery-record--september-6-2026) owns current raw-crate evidence, following the earlier free-placement delivery. Finished carriers, loose props and saves retain their future checks below. Historical fixed-mat checks are superseded; the useful scoop/processing and muted-automation checks remain.

`-Mode AuthorFreePlacement` applies the guarded one-time saved-scene migration; `TunePlacementPreview` updates its now-dormant preview material. Normal tests/builds load the supplied scene without running it. `HandlingSceneTests` covers actual placement/rotation/drop input, supported settling, toppling, blocked recovery and input priority. The ordinary packaged probe covers full-load ground/worktop/support placement, regrab, drop/pause/contact/settling and automatic recovery before running the retained processing checks. Both check that valid and invalid placement aim stays free of outlines and continuous validity text, while rejected E placement retains a brief explanation.

The revision uses pinned Unity 6.6 [Rigidbody motion](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Rigidbody.html), [penetration queries](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.ComputePenetration.html), and [interpolation ownership](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Rigidbody-interpolation.html), plus the existing Input System 1.20 actions. Nonalloc geometry queries have bounded buffers and reject saturated results. Held bodies use an enabled kinematic trigger so collision queries remain usable without pushing the holder; direct holding disables interpolation. Released bodies use gravity, continuous collision, interpolation and zero-bounce contacts. No editor/package upgrade was made.

| Work | Required evidence |
| --- | --- |
| 1_02 crate revision / 1_04 finished carrier | Actual input places/rotates a loaded carrier at multiple chosen ground/worktop/support poses, releases it to fall/collide/settle, then regrabs and tips/deposits. Obstructed careful placement gives clear guidance while deliberate release remains available. Contents survive toppling/recovery; no mat whitelist, accidental food transfer, held-object player launch, or duplicate body. |
| 1_06 loose objects / 2_01 wheelbarrow | Shared grab priority, careful stacks where supported, small-prop drop/toss, movable temporary clutter and lost-object recovery; wheelbarrow release/regrab at chosen positions with easy turning/reversing. Check pause/focus during physical motion without a resume burst or unintended input. |
| M3 saving | Stable IDs, free poses/rotations, arranged stacks, held and falling objects round-trip coherently. Valid arrangements remain; invalid/blocked positions recover without duplicate bodies, reset food or stack explosions. |
| M5–M6 presentation | Real assets retain consistent portable/fixed affordances, matching collision, useful staging space and physically available shortcuts. Stored-food displays do not masquerade as unimplemented loose jars. |
| M8 reliability/performance | Repeated manipulation and reconstruction near boundaries, active/sleeping body counts, disturbed stacks, physics cost, and save/load behavior on recorded hardware. Separate loose bodies from decorative pepper proxies. |

Packaged players must exercise changed handling. Human checks ask which objects/positions/routes worked as expected and which refusals or collisions felt arbitrary; automate conservation/recovery and inspect the actual contact/settling behavior without inventing comfort or fun. Optional prop play cannot excuse excessive forced waiting.

## First-playable comfort checks

Task 1_05's [feedback revision](tasks/1_05_first-playable-comfort-and-handoff.md#human-feedback-and-quiet-help-revision--september-6-2026) moves instructions to optional F1 help and retains session sensitivity. `AuthorQuietHelp` applies the guarded saved-scene correction; ordinary play/builds do not run migrations. Actual input checks cover opening/closing help from play or pause, focus loss, hidden instructions during gameplay, keyboard/pointer sensitivity and food recovery/reset preserving the setting.

### Loose-prop checks

The [1_06 physical-handling revision](tasks/1_06_loose-yard-objects-and-playful-handling.md#physical-handling-revision-delivery--september-6-2026) owns current results and limitations. `AuthorLooseProps` adds four reusable bodies through editor authoring; `TuneLooseBall` tunes the sample's ball/basin fit. `AuthorPhysicalHandling` preserves input IDs while renaming the shared Use/Grab actions, updates F1 help and adds a static sloped board; `MovePlayBoard` records its subsequent relocation clear of the pepper pile. Normal runs use the saved scene without migrations.

`LoosePropSceneTests` covers actual RMB grab/release, LMB charged throwing, optional rotation/E set-down, worktop placement, stacks including the basin interior, quiet aim, pause/focus/fresh input and recovery. Added cases prove falling without a placement target, bounded moving release, board rolling, support removal and contact moving a resting ball. A gathering press cannot turn into throwing after losing its target or changing holders, and pause cancels charge. Food checks require explicit carrier release and preserve loaded poses/quantities through prop play and handoff. Existing carrier/food tests remain relevant to the shared component; boxes can settle, while ordinary ball release is never required to become motionless immediately.

The packaged smoke also manipulates all four props, then runs its retained food-loop checks. Its timeout is 210 seconds to cover these cases. An isolated five-second sample records total/awake/sleeping bodies and observed batch-update intervals with hardware/configuration in `Logs/FoundationSmoke/loose-props-observation.txt`; captures and disk writes are outside that interval. Batch mode can skip rendering: this sample does not measure rendered FPS, human navigation, minimum-hardware acceptance or a full performance profile. Automation stays muted.

## Machine and equipment-budget coverage

These checks are planned; historical 1_03 passes cover its automatic backend only.

- **1_07:** exactly-one targeted pickup, previewed/capacity-capped bulk sets, occlusion and contents/container priority, physical container filling/full/partial pours, spills/recovery and matched representation measurements. Both candidates retain representative nearby physical interaction; repeated contacts or representation changes cannot duplicate food.
- **1_08:** input-driven mechanism plus the provisional grouping guide within output collection, queued-to-active and output-to-carrier commit/reservation, interrupted strokes before/after commit, partial final batches, output blocking, pause/focus/recovery and full food handoff in the ordinary player.
- **2_01–2_02:** proportional credit, split-load equivalence, empty/repeated deposit, separate stored food, unaffordable/repeated purchase, either order, paid pending installation, combined effects and finite affordability with useful work left.
- **M3 onward:** coherent single/bulk/spill/operation/grouping/food/Coins/purchase/pose restore, duplicate pepper owners/stale absorbed IDs, invalid grouping selections, malformed balances, backup/write failures and no replayed collection/earnings/charges. Completion stays independent of spending or yard tidiness.

Human checks observe the operator action and the reason for a purchase. A bigger model, larger capacity number or click-to-wait animation alone does not establish a useful or enjoyable improvement.

### Purchased attachment checks

Follow the [installation lifecycle](state-and-saving.md#attachment-ownership-and-installation). These are future requirements, not passing results from this documentation refinement.

| Owner | Required evidence |
| --- | --- |
| 2_01 model | Available → paid/awaiting installation → installed; one charge, stable kit identity and one effect. Interrupted placement, recovery and repeated mount requests cannot duplicate kits or capabilities. Both purchase orders remain viable. |
| 2_01 scene/player | Complete kit appears beside one large mount; shared physical grab/place/drop, forgiving fitting and clear snap/sound/mechanism response. Cards show workflow effect, full price, lifecycle state and location. Old machine works while unmounted; fitting during work signals the safe boundary and preserves active/queued food, reservations and output. Pause/focus needs fresh input. |
| 2_02–2_03 observation | Finding/understanding time, placement attempts/rejections and safe-boundary wait recorded as one-time installation cost. Recurring equal-work handling/travel measured separately before/after, with useful food left. No compulsory individual-item haul, remembered shopping list, artificial timers, slowed gathering or extra mandatory supply. Optional single-pepper play remains supported. |
| 3_01–3_03 save/recovery | Round-trip unowned, paid loose, held/dropped/recovered, fitted-pending and installed states in both purchase orders. Duplicate IDs, contradictory ownership/mount state and blocked/lost kit poses do not create an extra body, charge, effect or food loss. Exercise actual restart and valid-backup recovery. |
| 5_02 / 5_05 presentation | Real kit/mount art keeps fitting forgiving and instructions visible, including muted play. One-time fitting remains distinct from the recurring machine benefit. |

Prototype comic content is limited to at most one optional inexpensive static gag. Observe the ordinary batch loop separately from liking it. Later 5_04–5_05/6_02 checks cover selected messages remaining readable later, no required dismissal/reply, no obscured operating instructions or stacked joke replay after load/skipped milestones, quiet gaps and muted dialogue. No new mail/chat service, random-event scheduler or day-management test system is implied.

### Finished-batch and accumulation checks

These are planned checks for the [finished-batch contract](../core-loop-and-mechanics.md#make-the-finished-batch-worth-handling), not new passing results.

- **1_04:** use actual input to receive a recognizable automatically arranged jar group, carefully place/regrab it and hand it off. Check full/partial food, no item-by-item confirmations, stable supported settling and correct carrier prompt among overlapping jars/machine geometry. Deliberate dropping still works without food loss.
- **1_08:** replace passive receiving with the small player-controlled grouping action. The guide responds to input as loose prepared material becomes a neat batch; no extra per-jar confirmations. Pause/cancel/recovery before commit keeps selected units at the output, after commit in the carrier. Repeated gestures and the final partial batch neither lose food nor recollect it.
- **1_04 / 5_03:** several deposits within one household milestone band visibly grow/fill the nearby food group, including partial units. Empty/repeated deposits, recovery and spending do not change stored appearance. The group remains a derived bounded view; no retrievable duplicate food or per-jar physics/save state. M3/later reconstruction selects its current appearance without replaying handoffs.
- **5_01–5_03:** assess prepared-pepper/jar readability, substantial receiving/contact feedback, dependable deliberate placement and a work-area sightline to accumulation. Preserve exact state and short routes with representative assets.
- **2_03 / 5_05:** record individual handling, bulk handling, packaging and control clarity separately, including intended versus affected selections. Also record transfer preference, interest in the finished food/visible order and motivation to repeat after an upgrade separately. Prototype shapes can answer initial readability questions; representative materials need later evidence. Liking a joke or seeing a counter increase does not establish food appeal.

## Gates before content production

M2 must meet the [revised scope/feel gate](../scope-and-validation.md#next-experiment-one-pile-one-carrier-one-discovery), with actual sample limits. Compare baseline, each initial purchase and both on equivalent finite food jobs. Coins and both useful choices belong in the tested prototype.

Record the nine current prototype questions separately: single/bulk handling, creating an orderly finished batch, direct operation, clear controls/comfortable cycle, food/Coins handoff, meaningful purchase choice, improved-apparatus repetition, natural yard play and finite-food/budget reliability. Measure operator strokes/held effort, transfer/trip counts, internal waiting and stored units separately from one-time installation. The small work area includes single/bulk physical pepper handling with affected-set preview, player-controlled grouping into recognizable finished batches, a nearby graybox food group, props, direct operation, the two-offer bench and one complete attachment snap; polished household art, a Grandpa dialogue system, final conversion and disk saves remain later. At most one optional static gag may accompany M2.

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

The earlier 64 moving decorative proxies are an interim representation guardrail, not a cap on all physical gameplay. 1_07 compares manageable physical pepper batches and grouped resting/distant views on the same job, measuring active/sleeping bodies, single/bulk selection, container contact, recoverable spills and frame/allocations. Both retain the required nearby physical gameplay sample and explicit registered food ownership. Profile actual carriers, material, mechanism and props separately; do not freeze important objects to preserve a cosmetic-only rule.

Capture frame timing and allocation behavior during repeated gathering and dumping, plus startup/save/load times once those exist. Investigate sustained frame times above the target, repeatable spikes, per-action memory growth, and save stalls. Prefer bounded effects, authored depletion, and reusable assets before engine-scale optimization. Define tighter measured budgets only when a real problem needs them.

## Regression records

| Observed failure | Cause and contract | Regression evidence | Status |
| --- | --- | --- | --- |
| Human 1_06 feedback found ordinary ball interaction rigid and drop controls awkward. | E used supported placement with forced sleep; physical release was a separate key. | Ordinary-input falling, moving release, slope, support-removal/contact and intent-cancellation checks in PlayMode and the packaged probe; human feel remains pending. | RMB now grabs/releases at the hand with bounded carry motion; E provides secondary set-down into active simulation and LMB deliberately charges a prop throw. |
| The revision's first board position blocked a pepper region and interrupted the ball's roll. | The board overlapped pile geometry. | Existing local-gathering check, authored-board roll and an EditMode board/pile bounds check. | Editor-authored board moved to clear space beside the prop area. |
| 1_04's first package showed the rack label over the HUD and first deposits low behind the machine. | The old floating label location and bottom-first filling made the handoff and early accumulation harder to see. | Fresh packaged rack and receiving-work-area captures at 12, 24 and 107 stored units. | Mount the existing label between shelves and fill upper shelves first; exact food and bounded group ownership are unchanged. |
| 1_02 free-placement tests found held-wall overlap and a low pouring edge. | A disabled held collider did not supply reliable penetration correction; a torso-height sweep also crossed the worktop during the raised tip. | Held-wall and existing raised-cascade PlayMode assertions. | Enabled held trigger supports queries without solid contact; tipping sweeps from an elevated origin. |
| Pausing a physical crate mid-tip retained a small stale rotation. | Rigidbody interpolation competed with direct held transforms after physics paused. | Existing focus/pause tip test checks the restored carry orientation and no transaction replay. | Disable interpolation while held; enable it for released motion. |
| Historical 1_02 package captures showed no wire preview despite valid enabled line geometry. | The sprite material did not render the then-required outline in the ordinary player's captures. | The earlier free-placement delivery verified enabled geometry and rendered ground/worktop previews. | Originally fixed with an opaque unlit material and a property block allocated during initialization. Superseded by human feedback: the quiet-placement revision deliberately removes preview drawing and its allocation; current checks require hidden indicators. |
| 1_03's first package capture showed an upward cascade and a floating stage label over the HUD. | The low scoop carry pose was reused for tipping; the stage label floated at the HUD's projected height. A dump needs a visible downward handoff and readable status. | Processing PlayMode and packaged smoke check the actual raised pouring edge above the intake; final package captures inspect the cascade and labels. | Fixed: brief raised side pose during the cosmetic tilt; printed tray/station labels moved onto their surfaces. Food still commits before presentation. |
| 1_01 walking barely moved in very fast headless frames despite the Move action reading W. | CharacterController's default 0.001 m minimum discarded small motion steps. Walking must remain responsive across frame rates. | `Assets/JustAFewPeppers/Tests/PlayMode/FoundationSceneTests.cs`, `MovementLookFocusLossAndResetUseActualInput`, failed before the fix and passed afterward. | Fixed: `YardPlayer.ResetTo` sets `body.minMoveDistance = 0`; the saved scene has the same value. |
| 1_01 movement revision's focus test stopped the player when simulated W/Space input activated Quit while unfocused. | Gameplay paused but UI actions still accepted navigation/Submit. An unfocused window must not execute menu commands. | `FoundationSceneTests.MenuSpaceFocusPauseAndResetDoNotLeakJumpInput` reproduces focus loss with movement/jump held; the initial run aborted, then the full suite passed with UI disabled during focus loss. | Fixed: `YardInput.SetPaused` gates UI by focus, and `YardSession` restores only paused-menu input on focus return. |
| 1_02 package capture showed the held crate covering the aimed scoop region when looking down. | The carry anchor followed body yaw but not camera pitch. Broad scoop targeting needs a clear view of the touched face. | `HandlingSceneTests.LocalDepletionInterruptionFullLoadAndDenialCuesUseActualMouseInput` now checks the projected crate rim stays below the target; final packaged scoop/load captures also inspected. | Fixed: authored lower camera-relative carry pose, with scenery tucking. The capture helper also places its temporary HUD in front of scene geometry to match ordinary overlay behavior. |
| Automated handling checks played audible game sounds despite hidden execution. | Tests and the packaged probe exercised real audio sources without muting output. Automation should be silent while retaining cue checks and ordinary play audio. | Existing handling integration and packaged smoke assert zero listener volume after actual scoop cues, and that both sources respect listener volume. See [quiet-test evidence](tasks/1_02_scooping-and-crate-carrying.md#quiet-test-follow-up--september-6-2026). | Fixed: PlayMode fixtures mute before scene loading and restore in teardown; the explicitly requested packaged probe mutes before scene Awake. |

The 1_06 interior-stack check exposed a real ball/rim approach refusal after the first broad stack check only established a supported placement. `LoosePropSceneTests.BasinStacksOnStoolAndBallFitsInsideBasinWithRealCollisions` now checks the ball's bottom against the basin base and revalidates recovery with an interior object present. Matching sphere sweeps, a roomier basin and actual compound penetration checks fix the refusal and preserve the stack; the packaged probe checks the same interior height. See the [task's delivery evidence](tasks/1_06_loose-yard-objects-and-playful-handling.md#delivery-record--september-6-2026).

Early input-test failures came from the batch editor's input routing and test setup; using Unity's isolated fixture and moving all simulated-input checks to PlayMode resolved them. No runtime exception filter was introduced. World labels obscured the route in the first package capture; their authored scale was reduced, and the final package capture was inspected again.

Record future important failures with root cause, violated contract, preventing test, and fixing change. Reproduce with a failing test first when practical, and retain it afterward.

Investigate unexpected errors from the current game/editor run. Do not carry over exception filters from the old Stage0 probe or suppress unrelated errors to report a pass.

## Release-candidate check

Test the exact candidate artifact on a clean user-data path: launch, new game, save, exit, continue, controls/settings, full completion, continued completed-yard control, and relaunch into the completed property. Check supported display/input configurations, backup recovery, credits/licenses, and runtime logs. Record the artifact version and unresolved issues. The selected distribution channel's current requirements must be checked when that release task is performed.
