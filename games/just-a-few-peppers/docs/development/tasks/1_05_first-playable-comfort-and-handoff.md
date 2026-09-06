# 1_05 — First playable comfort and handoff

Milestone: M1 · Type: Milestone handoff · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Deliver the complete crate loop as an inspectable scene and Windows smoke build, with an interim developer checkpoint before loose props, physical-material comparison and direct machine operation in 1_06–1_08, then M2 purchases.

**Depends on:** [1_04 — Finished carrier and handoff rack](1_04_finished-carrier-and-storage-rack.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [M1 contract](../first-playable-task.md) · [Core mechanics](../../core-loop-and-mechanics.md) · [Look, sound, and comfort](../../look-sound-and-comfort.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Close remaining crate-loop behavior gaps: session reset, pause/focus including moving bodies, sensitivity, hold-to-scoop gathering, free placement/rotation/drop and recovery for both carriers, readable Pepper pile guidance, and scoop/tip/deposit feedback.
- Keep general instructions and debug keys behind F1 following the supplied feedback; teach delivered actions with only brief relevant target feedback; keep careful placement distinct from pouring/drop, target priority stable and full/invalid sounds quiet. Preserve hidden placement indicators. Single/bulk pepper selection and its affected-set preview arrive in 1_07; output grouping arrives in 1_08, each with its own controls/guidance. This checkpoint does not claim those later actions.
- Exercise the integrated ordinary-load and partial-final-load flow, repairing relevant failures. Produce a Windows build that starts the new scene.
- Record scene/build paths, exact useful commands, observed checks, and known limitations. Explain what the developer can play; keep fun claims pending actual feedback. This is the interim complete-transfer-loop checkpoint; 1_07–1_08 finish the revised physical processing loop and 2_01–2_03 add/evaluate chosen improvements. A formal multi-person study is not required here.

## Acceptance

- The first-playable contract's 1_01–1_05 criteria are met or explicitly left incomplete; 1_06's loose-prop sample follows this checkpoint. A compile alone does not qualify.
- The packaged scene completes a small harvest with functional input, pause/reset, and no unexplained runtime error or lost units.

## Human playtest check

Play repeated loads for a few minutes; report weak scooping, awkward carrying, unclear output, or boring walking/waits. Record whether the crate loop is at least promising before wheelbarrow work. A formal six-person study is not required here; the stronger human-evidence decision remains 2_03.

**Outside this task:** Wheelbarrow, full yard, saving, household scenes, and retesting Stage0.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_06 — Loose yard objects and playful handling](1_06_loose-yard-objects-and-playful-handling.md). Stop after this task's handoff unless the developer explicitly requested a larger range.

## Delivery record — September 6, 2026

**Ready for human playtest / Not tested.** NEXT selected 1_05 after the developer accepted the last playable 1_04 delivery as working fine. That supplied feedback is recorded in 1_04 and the queue; it does not accept the new comfort changes. Read the complete task and predecessor delivery, current M1/mechanics/comfort/state/architecture/verification and asset contracts, then inspected the saved HUD, input, handling, tests, wrapper and pinned packages. No earlier Partial/Needs revision task or review gate blocked selection. Exactly one task was implemented. **Next: 1_06.**

### Artifact and delivered behavior

- **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity`, relative to `games/just-a-few-peppers/unity/`.
- **Ordinary Windows x64 Mono build:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`, with its complete adjacent directory. Development is off. Final build reports **98,277,567 bytes**; runtime DLL SHA-256 **`5A6DB570E5B431C3B700BF51F5D9A07FABE331268CD361D322A5678CFA667182`**. No Inspector assembly is needed.
- One short contextual hint replaces the full controls list during play. It follows the current food/hands: find the orange crate, hold-to-gather/release-to-stop, tip a full or partial load, collect at the right receiving tray, retrieve a parked finished carrier and hand off at the rack. It recognizes remaining processing and all food stored, and reconstructs after recovery/restart. It neither owns food nor forces a tutorial sequence.
- Esc shows the full control reference, explicit recovery versus food-reset effects and a mouse-sensitivity slider. Mouse dragging or keyboard Left/Right adjusts 0.25x to 2.50x, default 1.00x. Actual look input uses the changed value. It survives food reset/recovery within the session, returning to the authored default on relaunch; M3 will persist currently implemented settings, while broader camera/rebinding options remain M7.
- Brief raw pickup feedback distinguishes E careful placement from G dropping. Existing target priority, hold-only scoop cadence, quiet full/invalid cues, optional rotation, hidden placement indicators, both carriers' recovery, automatic processing and stored-food ownership remain intact. The complete 107-unit job still finishes through nine deposits, including eleven units in the final load. Normal yard control remains available.

### Implementation and authoring

`Runtime/YardHud.cs` owns derived guidance and its pause slider wiring; `YardSession.cs` refreshes guidance after handling/resume/recovery/restart. `YardHandling.cs` shortens the raw status bar and clarifies pickup feedback. `Runtime/FoundationBuildSmoke.cs`, `Tests/EditMode/FoundationAssetTests.cs`, and the existing foundation/finished-food PlayMode fixtures verify the changed behavior. No new gameplay binding, food model, package or physics behavior was introduced.

`Editor/ComfortAuthoring.cs` and its Unity-generated meta edit only the existing HUD through editor APIs. Wrapper `AuthorComfort` is guarded against replacing already-authored comfort work; `TuneComfortPresentation` supplies the subsequent menu correction. Normal builds load the saved scene. All **2,780 prior serialized scene IDs** remain and **26 IDs** were added. Full controls retain their existing text object, now parented under pause. Only trailing scene whitespace was normalized after saving, with identical serialized tokens. Existing package pins, input asset, build settings and prior artwork/food code were preserved. No Stage0 builder, external asset import, purchase or development build was used; UI reuses the built-in font and simple uGUI graphics.

Pinned editor **6000.6.0f1**, Input System **1.20.0**, uGUI **2.6.0** and Test Framework **1.8.0** remain. Consulted official [Input System UI module](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.20/api/UnityEngine.InputSystem.UI.InputSystemUIInputModule.html) and [Unity scene saving](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/SceneManagement.EditorSceneManager.SaveScene.html) references. The hosted uGUI 2.6 page was unavailable; the installed 2.6.0 package's `Slider.cs` documentation/source confirmed callback-free initialization and keyboard movement behavior. Editor commands used the documented working context outside the restricted sandbox from the first launch.

### Verification and honest limitations

Commands from the repository root:

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **EditMode 20/20 passed:** saved HUD/slider/navigation/input/build wiring and retained food/conservation tests.
- **PlayMode 27/27 passed:** retained movement, scoop/tip, full/partial food, free physical carrier handling, pause/focus, switching, recovery and complete-job checks. New actual-input coverage exercises keyboard navigation and pointer sensitivity changes, unfocused UI isolation, resume without stale look, changed mouse response, and recovery/reset preserving the setting. A two-unit load checks contextual gather/tip/output, parked-food recovery and hidden placement indicators; the complete job checks final guidance and explicit restart.
- **Final ordinary Build and Smoke passed.** The exact packaged player checks keyboard/pointer sensitivity, actual look, recovery, full controls in pause and hidden live hint/backdrop, then the retained scoop/processing/physical-carrier checks and all 107 units through nine handoffs. The complete-job segment prepares loads with public gathering commands, then uses ordinary transfer inputs and unaccelerated station time. Deterministic approaches and simulated focus callbacks do not establish human navigation or physical Alt-Tab behavior.
- The first authoring attempt found a `Text`/`Transform` sibling-index compile mistake; it was corrected before scene authoring succeeded. The initial PlayMode run passed 26/27: a new assertion assumed the deliberately absent finished-carrier preview renderer existed. The corrected check accepts no renderer or a disabled renderer; the subsequent 27/27 run passed. Its initial XML is retained as `Logs/Task1_05-PlayMode-initial.xml`.
- The first package passed behavior, but capture review exposed the slider handle stretching into its label and unnecessary HUD visibility behind pause. The final saved menu uses a bounded handle, opaque panel and a hidden guidance backdrop while paused. Final Build/Smoke and fresh captures cover that presentation correction; the already-passing rules/PlayMode suites were not repeated for the layout/visibility-only change.
- Inspected fresh 1440 x 900 ordinary-player menu, sensitivity and gathering captures. Existing smoke captures also cover intake/output, free placement and final food accumulation. Local evidence: `Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, authoring/test/build/smoke logs, `FoundationSmoke/result.txt`, `01-menu.png`, `01-sensitivity.png`, `04-scoop.png` and the existing full-loop captures. Final runs have no C# compiler/deprecation warnings or unexpected gameplay exceptions. Documentation links/anchors, whitespace, task ordering and preservation are checked at handoff.

**Limitations:** graybox food/yard; only raw and finished carriers are portable here. No loose-prop sample (1_06), single/bulk physical peppers (1_07), direct operation/grouping (1_08), Coins/purchases (M2), disk saving (M3) or expanded camera/binding settings (M7). Automated tests and smoke remain muted. Audible quality, physical Alt-Tab/cursor behavior, whether the hints are helpful, and the complete loop's human pacing/appeal need actual feedback. The developer's positive report covers 1_04, not these new changes; no fun score is inferred.

### Controls and short play check

Enter/click Walk starts. WASD/arrows move; mouse looks; Shift sprints; Space jumps. E picks up, places, tips, collects or hands off at the indicated target. Hold left mouse scoops, release stops. Optional Z/X rotates; G drops. Esc pauses and shows controls/sensitivity. R returns/recovers while preserving food; F8 explicitly clears the food test including stored food.

1. Follow the opening hint to the crate; gather a small partial load, release the mouse and tip it into the round intake.
2. Follow the output hint to the receiving tray on the right, collect, park/regrab the carrier, then hand it off at the rack behind the processor.
3. Pause, change sensitivity using mouse or keyboard, and resume. Check comfortable look, no unintended movement/handling and unobstructed play guidance.
4. Drop a loaded carrier, pause/Alt-Tab, resume and use R. Confirm the same food remains and other valid placements stay where left.
5. Complete several loads and the final partial one, reaching 107/107 with control retained. R keeps food; use F8 only to restart, confirming sensitivity stays for this session.

**Next task: 1_06 — Loose yard objects and playful handling.** Stop after this handoff.

## Human feedback and quiet-help revision — September 6, 2026

The developer said this task was fine and requested NEXT, while asking to remove most movement/control instructions and show the full reference only through an uncommon hotkey. This accepts the prior loop/checkpoint overall; it does not establish detailed checklist coverage or a fun score. The new quiet presentation has not yet received human feedback.

Full bindings, recovery/restart tools and optional current-work help now live behind F1. Normal play removes the instruction footer; Esc uses a compact pause menu with existing session sensitivity. F1 pauses and returns to the previous mode when closed; focus loss requires explicit resume. Pickup no longer advertises a list of placement/rotation/drop keys. Short relevant target prompts and food status remain. `QuietHelpAuthoring.Apply` migrated the existing HUD/input through editor APIs. The [1_06 delivery record](1_06_loose-yard-objects-and-playful-handling.md#delivery-record--september-6-2026) owns the combined artifact and fresh verification, including actual F1/menu/focus and packaged checks.

Options/settings were already queued: [7_02](7_02_input-and-camera-options.md) covers input/camera and [7_03](7_03_audio-display-and-guidance.md) covers audio/display/guidance. Their briefs now explicitly preserve the quiet default and optional reference. No additional task or settings system was added. The queue's Not tested applies to this latest presentation correction only.
