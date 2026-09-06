# 1_03 — Tipping and automatic processing

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Supply the reliable automatic processing backend with partial transfers. This delivered step is retained; [1_08](1_08_direct-machine-operation.md) adds the revised directly operated batch cycle.

**Depends on:** [1_02 — Scooping and crate carrying](1_02_scooping-and-crate-carrying.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Look, sound, and comfort](../../look-sound-and-comfort.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Wire one broad intake target and a short tilt/cascade. Transfer only the accepted amount into the input buffer; interrupted motion cannot repeat or erase a committed transfer.
- Add one input queue, one active batch, and accumulating output at the starting 12/12 tier. Reserve output room before beginning work and start partial batches automatically.
- Show simple working/output-full feedback and placeholder roast/rest/preparation/packing stages. Pause processing with gameplay; waiting never spoils or burns food.
- Keep transfers and output readable without repeated blocked-input feedback. Food preparation is the current objective; the automatic start behavior recorded below is an interim backend, superseded as the final interaction by 1_08.

## Acceptance

- Test full, limited-space, cancelled, and final partial transfers; conservation holds across pile, crate, queue, active batch, and output.
- Full output safely pauses the line; output plus reserved active work stays within capacity. No minimum load or full-jar rule strands the last peppers.

## Human playtest check

Tip a full crate, inspect the cascade and output, then try a partial load and pause during processing. Output collection is added in 1_04.

**Outside this task:** Manual peeling, separate intermediate inventories, quality timers, fuel, recipes, or helper AI.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_04 — Finished carrier and handoff rack](1_04_finished-carrier-and-storage-rack.md). Stop after this task's handoff unless the developer explicitly requested a larger range.

## Delivery record — September 6, 2026

**Ready for human playtest / Not tested.** NEXT selected 1_03: no earlier Partial/Needs revision entry or pending review gate blocked it. Read the full brief, linked behavior/state/comfort contracts, architecture, Unity/verification guidance, and predecessor delivery/feedback records, then inspected the saved scene, runtime, authoring, tests, and package pins. Earlier requests for hold-only scooping and muted automation remain applied. No new human feedback was supplied. Exactly one task was implemented; **1_04** is next. M1 remains in progress.

### Playable artifact and behavior

- **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity` relative to `games/just-a-few-peppers/unity/`. **Player:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe` from the repository root, with its complete adjacent folder. Ordinary Windows x64 Mono, Development off; no Inspector work required.
- E at the broad intake commits only the accepted raw load, then shows a 0.75-second raised side tilt and downward cascade using nine reusable proxies. Looking away, pause/focus loss, or recovery interrupts the visual without undoing/repeating the food transfer. Held E cannot repeat; fresh input requires release after interruption. Scooping/parking cannot overlap the cascade.
- The same `HarvestState` now owns a 12-unit input queue, one active batch/remaining time, and 12-unit accumulating output. Active food also reserves its output destination. Batches start automatically, including a single unit and the amount that fits beside existing output. A four-second batch shows roasting, covered rest, preparation, then packing/cooling. Output plus active reservation never exceeds capacity. Full output safely leaves queued food waiting; full intake retains any remainder in the crate.
- Queued peppers, moving feeder/stage markers, partial-fill jar groups, persistent input/working/finished counts, and restrained tip/completion/denial cues give feedback. One jar represents three pepper units, with exact partial fill. Waiting has no spoilage or quality penalty. Repeated full-input E presses do not repeat the soft cue.
- R/Return to gate/fall recovery preserve the raw load, local depletion, all station food, and batch progress. F8 or **Restart processing test (clears food)** restores the entire authored test, including station quantities/timer, while keeping pause state. Output collection and the handoff rack remain 1_04, so this build safely reaches full output/input and then needs a test restart.

Controls: Enter/click Walk starts; WASD/arrows move; mouse looks; either Shift sprints; Space jumps; E picks up/tips at intake/parks beside a mat; **hold left mouse to scoop, release to stop**; Esc pauses/resumes; R recovers; F8 restarts. Menu navigation remains Up/Down and Enter/Space or mouse. Focus return requires explicit resume.

### Implementation and authoring

Changed code/assets are under `Assets/JustAFewPeppers/`: extended `Runtime/HarvestState.cs`, `YardHandling.cs`, `RawCarrierView.cs`, `YardSession.cs`, and `FoundationBuildSmoke.cs`; added `Runtime/StationView.cs`, `TipPresentation.cs`, `Editor/ProcessingSceneAuthoring.cs`, `Tests/EditMode/ProcessingStateTests.cs`, and `Tests/PlayMode/ProcessingSceneTests.cs`; extended `Tests/EditMode/FoundationAssetTests.cs`. The existing `Scenes/PepperYard.unity` integrates the intake, stage/output views, feedback audio, and revised HUD/menu labels. `Content/Jar body.mat` is one new graybox material; all new assets have Unity-generated metas.

`ProcessingSceneAuthoring.Apply` / wrapper mode `AuthorProcessing` is a guarded one-time addition to handling and refuses to replace existing processing. `TuneLabels` / `TuneProcessingLabels` applies the focused label correction. Normal builds use the saved scene without regeneration. Serialized-object comparison retained all 1,043 other original scene fileIDs; only the old machine-wide target component was removed, replaced by the explicit intake target. Eleven retained objects changed for wiring/labels; 229 serialized objects were added. Final whitespace normalization changed no serialized tokens.

The editor remains **6000.6.0f1**, Input System **1.20.0**, Test Framework **1.8.0**, uGUI **2.6.0**, with Built-in rendering and existing ordinary/development build separation. No package, input binding, editor version, or production asset pack was added. Existing Kenney CC0 impacts serve tipping/finish/completion; reuse is recorded in the [asset register](../asset-register.md). Matching official API references and verification coverage are in the [verification guide](../testing-and-performance.md#remaining-gameplay-coverage). Player/state contracts, architecture, play guide, queue, and milestone summary were updated.

### Verification evidence

Commands from the repository root:

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **EditMode 13/13 passed:** saved scene/reference/material/input/build wiring; original gathering/ownership checks; full and limited-space tips; output reservation/accumulation; final 1/7/11-unit harvests; long versus small time steps; invalid time/configuration; 2,000 deterministic mixed commands with conservation/capacity assertions; reset of all station owners. Pure rules were unchanged by the later presentation correction and were not rerun for it.
- **PlayMode 18/18 passed:** the 15 existing movement/handling checks plus three processing integrations. These exercise actual E input, tilt/cascade and its raised pouring edge, all four stage cues, pause/focus freezing batch time, held-E resume rejection with a new available load, partial jar fill, output accumulation/reservation, limited acceptance, full-input cue limits, occlusion/range rejection, looking away mid-motion, recovery during queued/active work, and paused restart. Processing tests prepare loads through public model transactions; the existing gathering checks and packaged probe use actual scoop input. Tests are muted and restore listener volume in teardown.
- **Ordinary Build and Smoke passed** on the final artifact: **98,053,207 bytes** reported by Unity; runtime DLL SHA-256 **`909625110C5E6DDE5B105A68F3FDBE1A1C67A8587A784BCD1B16A390DD471058`**. The exact executable checks the original menu/movement/handling behavior, then actual scoop/tip loads: one unit to a partial jar; a 12-unit load reserves eleven output spaces and queues one; another load fills the eleven free input spaces and keeps one in the crate. It verifies quiet blocked input, pause/focus, all-food recovery/reset, conservation, and muted processing audio. Approach positions are deterministic probe placements, not human navigation evidence.
- The final packaged run measured **5.501 seconds** for twelve actual scoops, with the original first-immediate/0.5-second cadence. Configured tip motion is 0.75 seconds and automatic work is four unpaused seconds, so processing duration is shorter than gathering another full crate when output space is available. Loaded travel, output service/return travel, unavoidable waiting over a complete stored loop, enjoyment, and performance remain unmeasured; collection is not implemented yet. No scoop delay was added to improve a ratio.
- Actual ordinary-player captures at 1440 × 900 were inspected: menu, full cascade, one-unit output, and full input/output. The first capture exposed an upward cascade from the low carry pose and a world label overlapping the HUD; the raised side pose and mounted label correction were followed by a passing PlayMode run, rebuild, smoke, and fresh capture review. The [regression record](../testing-and-performance.md#regression-records) retains the cause and check. No failed result is claimed as a pass.
- Raw local/ignored evidence: `unity/Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, their logs, `Foundation-Build.log`, `Foundation-Smoke.log`, and `FoundationSmoke/result.txt` with `01-menu.png` through `11-full-input.png`. The ordinary player was rebuilt; the separate development artifact was not.
- Documentation checks passed across 56 Markdown files, 587 local links, 177 heading targets, balanced fences, and all 31 unique queue IDs in unchanged order; only 1_03's task row changed. Asset/meta pairing and GUID uniqueness passed for 56 metas in the current game asset tree. Whitespace checks passed; scene trailing whitespace and mixed text line endings were normalized without changing serialized tokens or behavior.

Initial sandboxed authoring failed to connect to Unity Package Manager's local IPC stream. Authorized editor runs outside the sandbox completed authoring/tests/builds; the packaged probe ran through the normal wrapper. The editor retains its pre-existing licensing access-token/signature notices without preventing local entitlement or completion; the player retains its D3D12 info-queue notice. No C# compilation/deprecation warnings or unexpected gameplay errors/exceptions occurred in the final checks. No Stage0 test or build was run.

### Human play checklist and remaining acceptance

1. Start, pick up the crate, scoop a full load, then approach the broad tray and press E. Inspect the downward cascade and automatic stages; four full jar groups should accumulate without another command.
2. F8, scoop just one or a few units, and tip. Check that a partial jar appears. Then deliver a full load: only available output room starts processing, with the rest visibly queued.
3. After output fills, bring another full load. Check partial intake acceptance and retained crate contents; repeat/hold E at full input and check quiet, persistent guidance.
4. Pause or Alt-Tab during tipping/processing, then explicitly resume and release controls. Food/progress should remain consistent. Press R with station food present; confirm it stays while the crate returns to the gate.
5. Restart from the pause menu: all food returns to the mound, crate/station empty, and the menu stays open. Report target clarity, motion/camera comfort, sound balance, and whether dumping feels satisfying.

**Limitations:** output pickup, permanent deposits, complete storage, wheelbarrow, disk saves, and full-yard completion are not implemented in this task. Full output is deliberately uncollectable until 1_04; use the explicitly destructive prototype restart for another experiment. Graybox art and compressed automatic stages are provisional. Physical Alt-Tab/cursor behavior, subjective motion/audio comfort, ordinary route timing, full-cycle balance, and enjoyment need human play; no acceptance or fun rating is inferred from automation. **Human feedback: Not tested. Next task: 1_04.**
