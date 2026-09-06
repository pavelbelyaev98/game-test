# 1_02 — Scooping and crate carrying

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make taking an ordinary load visibly satisfying, with a crate the player can freely place and drop. The current revision extends existing handling while preserving delivered processing.

**Depends on:** [1_01 — Unity foundation and walkable scene](1_01_unity-foundation-and-walkable-scene.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Look, sound, and comfort](../../look-sound-and-comfort.md) · [Architecture](../../../ARCHITECTURE.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

For the reopened revision, also read the [free handling contract](../../core-loop-and-mechanics.md#pick-up-place-and-play), [feedback below](#free-placement-feedback-and-revision--september-6-2026), and the **full [1_03 delivery record](1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026)**. That later delivered integration is required preservation context, not a new circular prerequisite. Resume the existing scene/model/input and mute policy; do not regenerate earlier authoring or restart scooping from scratch.

## Work

- Add the authoritative finite pile/raw-carrier state and validated gathering commands, using a 12-unit crate available from the start.
- Connect broad hold-only scooping to local authored depletion, increasing carried volume, short action audio, and clear full/invalid-target feedback.
- Tune the load rhythm as part of the physical food-processing loop. Avoid instant filling followed by long walking/servicing; pleasant handling and later direct machine work matter together, without requiring clearing to dominate.
- Replace the two-mat placement restriction with geometric reach/support/clearance checks, rotation, careful placement on ground/worktops or stable supporting objects, and physical release/drop with gravity and collisions. Add a reachable test worktop and stackable support in the existing corner; normal placement cannot depend on named pads or surface whitelists. Keep holding collision-aware and ordinary sprint/jump available. Physically accessible shortcuts are valid; boundaries retain the yard.
- Preserve one crate identity and its exact contents while held, placed, falling, toppled, paused, or recovered. Track free pose and last safe recovery pose instead of treating an authored mat index as every legal location. Recovery preserves existing processing quantities/timers. Decorative pepper motion remains bounded and never owns harvest units.
- Wire clear grab/place/rotate/drop input and contextual guidance alongside E tipping and hold-only scooping; document the actual bindings. Call the source Pepper pile / Peppers left in player guidance. Remove mats' visual implication that they are compulsory parking spots; keep a clear fallback recovery location.
- Apply the later quiet-placement feedback: hide the wire outline and continuous valid/blocked placement guidance. Keep a brief explanation after a rejected E attempt. Rotation remains optional; record its reported limited usefulness without inventing a replacement mechanic.

## Acceptance

- Scooping changes the touched pile region immediately; accepted units leave that pocket and enter the crate once, up to capacity.
- Gathering requires the scoop input to remain held. Release stops new transfers; no mode key can latch gathering on. Pause/focus recovery requires release and a fresh hold.
- Meaningful checks cover full/partial scoops, invalid targets, interruption, reset, and recovery without loss or duplication. Verify full/invalid feedback is rate-limited (one soft cue, no spam while the trigger is held). The scene exposes these actions to the human playtester.
- In the ordinary player, place a loaded crate at several player-chosen positions/rotations on ground and a worktop, on a stable support, then regrab and tip it. Release from a height and watch collision/settling. Blocked careful placement must not prevent deliberate dropping or corrupt contents; narrow/obstructed support gives understandable guidance.
- Pause/focus while dropping and resume without a motion burst or accidental transfer. Recover an inaccessible loaded crate with the same food while station work exists; preserve other valid arrangements. Verify held-wall contact cannot propel the player, duplicate a body, or transfer food. Retain relevant 1_03 conservation, tip interruption, and output-reservation checks plus muted automation.

## Human playtest check

Fill and carry the crate, choose where to place it on ground and a worktop, rotate and stack it on the test support, drop it, then pick it up and tip. Try pause/recovery with food in both crate and station. Report arbitrary refusals, unclear controls, unstable placement, or handling that feels cumbersome.

**Outside this task:** Finished-output collection (1_04), the loose-prop sample/tossing (1_06), per-pepper food simulation, wheelbarrow, disk persistence, manual cooking, or stamina. Small support geometry needed to demonstrate placement belongs here.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

After this revision, apply queue selection: 1_03 is already technically delivered, so the expected next unfinished task is [1_04 — Finished carrier and handoff rack](1_04_finished-carrier-and-storage-rack.md). Stop after this task's handoff unless the developer explicitly requested a larger range.

## Delivery record — September 6, 2026

Historical first delivery. The [hold-only revision](#hold-only-revision--september-6-2026) below supersedes its mode-switching controls and build evidence.

**Ready for human playtest / Not tested.** NEXT selected 1_02 from the queue. No Partial/Needs revision work was pending. The earlier walking acceptance does not establish acceptance of the subsequent sprint/jump revision or this handling build. Exactly one task was implemented; **1_03** is next. M1 remains in progress.

### Playable artifact and behavior

- **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity` relative to `games/just-a-few-peppers/unity/`. **Windows player:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe` from the repository root, with all adjacent files. Ordinary Windows x64 Mono build, Development off; no Inspector work is required.
- The existing mound is now **107 finite units** in nine authored local regions. A shallow three-unit edge can be cleared within a load. Other regions retain their own quantities/silhouettes. The 12-unit crate fills visibly, and three reusable decorative proxies show accepted scoops. They have no food ownership or Rigidbody simulation.
- [Current controls and scoop cadence](../../core-loop-and-mechanics.md#current-crate-prototype-controls) specify the delivered interaction. E picks up or parks at a clear marked mat; left mouse scoops, T switches hold/toggle, and the existing movement/sprint/jump/pause controls remain. R/Return to gate/automatic fall recovery preserve the exact load and local depletion. F8/Restart scoop test explicitly restores initial state; this is how to try another load before 1_03 adds tipping.
- Grounded, nearby, unobstructed parking on the two fixed mats prevents stacking or throwing the crate into inaccessible places. Invalid parking leaves ownership/contents unchanged. Recovery restores the same crate to the gate mat. The held view follows a low camera-relative pose and tucks toward the torso near scenery; it cannot propel the player. Full/empty/invalid feedback is restrained and does not repeat merely because the trigger remains held or is repressed.
- Three source audio clips from Kenney's CC0 Impact Sounds are imported, wired, and documented in the [asset register](../asset-register.md). Primitive pile/crate materials remain deliberate graybox assets.

### Changed implementation and authoring

`Assets/JustAFewPeppers/Runtime/HarvestState.cs` owns quantities and crate ownership; `YardHandling.cs` validates input/targets and requests transactions. `PileRegion.cs`, `RawCarrierView.cs`, and `ScoopPresentation.cs` render accepted state. Existing session, Input System actions, targeting, HUD, and packaged probe are integrated with these components. `Tests/EditMode/HarvestStateTests.cs`, `Tests/PlayMode/HandlingSceneTests.cs`, and the foundation asset/movement checks cover the saved scene and rules.

`Editor/HandlingSceneAuthoring.cs` applied a focused editor migration, preserving the scene and input asset GUIDs; it refuses to recreate already authored handling. `TuneHandlingView` subsequently adjusted existing view/HUD references. Both are available in the [wrapper](../../../unity/tools/Verify-Foundation.ps1); normal tests/builds load the saved scene and do not run either migration. Content additions are four placeholder materials, the three audio clips, and their included license, all with Unity-generated `.meta` files. Editor **6000.6.0f1**, Input System **1.20.0**, Test Framework **1.8.0**, uGUI **2.6.0**, Built-in rendering, and existing build settings remain pinned. Matching official API references are in the [verification guide](../testing-and-performance.md#remaining-gameplay-coverage).

### Verification evidence

Commands from the repository root:

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **EditMode 6/6 passed:** saved references, content IDs/quantity, Input System bindings, aligned pile colliders, crate/audio/UI wiring, and broad-target occlusion/range; full/partial accepted scoops, invalid commands, repeated ownership/recovery, configuration rejection, copied configuration, and explicit reset conservation.
- **PlayMode 15/15 passed:** eleven foundation movement/input checks plus four handling integrations. Actual keyboard/mouse tests cover immediate local depletion and increasing contents, interruption, empty/full cue limits, rapid-click cadence, invalid/occluded targets, hold/toggle, focus/pause/resume with held input, loaded sprint/jump, rejected airborne/occupied-mat parking, valid parking, corrupted view/fall recovery, boundary collision, and paused prototype reset. The final-clump size and projected crate rim are checked for visibility. The last UI-only parked-full prompt correction is additionally asserted by the final packaged smoke run.
- **Ordinary Windows build and smoke passed.** The exact executable verifies startup/menu, movement/sprint/jump, pause/focus/reset, E pickup, immediate scooping/local depletion, full load and conservation, quiet held-full feedback, loaded sprint/jump, parking/pickup guidance, recovery, and F8 restart. The probe uses deterministic approach positions followed by actual input actions; this does not count as human navigation evidence.
- The packaged run measured **5.501 seconds** for 12 committed units from first input to full crate: first scoop immediate, subsequent scoops at the configured cadence. There is no catch-up burst or click-speed shortcut. This measures gathering only, not a completed processing/storage cycle; the complete gather/travel/service ratio remains a tuning check as 1_03–1_04 arrive.
- Actual ordinary-player captures at 1440 × 900 were inspected for scoop-region visibility, readable controls, full load, and parked contents. Raw evidence remains local/ignored under `unity/Logs/`: `Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, `Foundation-Build.log`, `Foundation-Smoke.log`, and `FoundationSmoke/result.txt` plus `01-menu.png` through `06-parked.png`. The final build reports **98,011,159 bytes**; runtime DLL SHA-256: `547D504F92A695C6710B007FD0A27C59B06E1E1367A3E84CF892A47749BC38AA`.
- No C# warnings/errors or unexpected gameplay exceptions in the final checks. The sandboxed editor initially exited during startup; authorized runs outside the sandbox completed. The editor retains its pre-existing licensing access-token/signature notices and resolves its local entitlement. No Stage0 tests, editor upgrade, new package, or development-player rebuild was required.
- Documentation checks passed across 56 Markdown files, 575 local links and 166 heading targets, code fences, and 31 unique ordered queue IDs. Asset/meta pairing and whitespace checks passed; all three imported audio files match their pack originals. Scene whitespace was normalized after editor authoring without changing serialized values. The pre-existing status-history edit was preserved.

One earlier PlayMode result was **14/15**: a foundation landing test hard-coded the old mound height and failed against the newly authored surface by 0.005 m beyond its old tolerance. It now compares the real surface within controller skin width; all checks passed afterward. The first package capture exposed an occluding body-relative carry pose, fixed with the lower camera-relative pose and a retained projected-rim check. Its capture-only HUD plane was moved in front of scene geometry to represent the ordinary overlay correctly. A parked full crate's prompt was also corrected to offer pickup. These corrections were verified; no failed check is presented as passing evidence.

### Human play checklist and limitations

1. Start with Enter/click Walk. Approach the gate crate, press E, then hold left mouse at the mound. Sweep across different clumps; clear the shallow left-front edge and check that only touched regions change.
2. Fill to 12, keep holding, then release/repress. Check that full feedback stays quiet and the carried volume leaves the scoop region visible. Try empty ground, another target, and moving out of reach.
3. Hold left mouse to scoop, then release and check that gathering stops. Pause or Alt-Tab mid-action; resume and check that a held/menu click does not resume gathering until released and held again.
4. Carry a partial/full load while sprinting/jumping, press E beside a crate mat to park, and pick it up again. Try parking while airborne/standing too close. Press R and confirm the same load remains on the gate mat and the mound stays depleted.
5. Use F8 or Restart scoop test in the pause menu to try a fresh load. Check that the crate empties, the mound returns, and a paused restart leaves the menu open.

**Limitations:** processing, tipping, finished food, wheelbarrow, saves, and whole-yard completion are not part of this task. Consequently the current crate cannot be emptied into the processor or clear the full mound through repeated production loads. Graybox pile appearance, audio suitability/volume, physical Alt-Tab/cursor behavior, and handling enjoyment need human review. Full-cycle balance and performance are not established by the automated timing or screenshots. At delivery, feedback was **Not tested**; subsequent observations are recorded below.

## Human playtester feedback — September 6, 2026

Historical clarification before the hold-only revision below; the removed mode no longer needs confirmation.

The playtester reported, “as for other things it works fine,” and asked what toggle does because they did not see a difference. This is positive feedback for the other handling behavior, with a remaining control-clarity question; it does not establish individual checklist results or a fun rating.

The actual input code was inspected: T switches the scoop mode but does not start scooping. Hold mode stops on left-mouse release; toggle mode continues after a click/release and stops on the next click. Both require a held crate with free capacity and a reachable nonempty region. The suggested check is to release T and the mouse, aim at peppers, then click and release once. No reproducible input failure has been reported yet.

Technical delivery remains Ready for human playtest. The queue's coarse Not tested feedback now refers to the unconfirmed toggle check; the positive partial feedback above is retained. No gameplay change, new task selection, Unity test rerun, or rebuild was performed for this clarification.

## Hold-only revision — September 6, 2026

The developer requested removing automatic/toggle scooping because the extra mode complicates the controls. This directly revises 1_02 and the related future specifications; no new gameplay task was selected. The earlier positive feedback for other handling remains recorded above. **Ready for human playtest / Not tested** applies to this revised build; confirmation of the removed mode is no longer pending.

Delivered behavior: **hold left mouse to scoop; release to stop**. Removed the latched runtime state, T action/binding, mode-switch logic, mode status, and mode instructions. The saved scene and scene-authoring helper now show only the hold control. The first accepted scoop, cadence, volume/depletion, full/invalid cues, carrying, and recovery are retained. Pause/focus still requires releasing the mouse before starting a fresh hold. Already committed decorative motion may finish after release; it cannot transfer additional units.

Updated the core mechanics, comfort specification, architecture, roadmap, play guide, this brief, and future 1_05/7_02 briefs. Historical delivery evidence above retains the old behavior only as explicitly superseded history. `HandlingSceneAuthoring.ApplyHoldOnlyScooping` / wrapper mode `HoldOnlyScooping` updated the input asset and saved HUD through the pinned editor APIs, preserving other actions and existing asset GUIDs.

**Play:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`, with its adjacent files. **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity` relative to the Unity root. Ordinary Windows x64 Mono player, Development off. Build reports **98,010,967 bytes**; runtime DLL SHA-256: `A72C186B860E7ECD985056EEFAF24E8525797D2A5F92C7D9523B705BBDF84F69`.

Verification used the same wrapper's `EditMode`, `PlayMode`, `Build`, and `Smoke` commands:

- **EditMode 6/6 passed**, including no saved scoop-mode action/T binding and hold-only HUD guidance, alongside scene/quantity checks.
- **PlayMode 15/15 passed.** The former toggle test now verifies a press/release stops gathering even after T, holding continues gathering, and focus/pause/resume require fresh hold input. Existing depletion, full/invalid cue, movement, parking, recovery, and reset tests pass.
- **Ordinary build and packaged smoke passed.** The final executable explicitly checks the missing mode action, a held scoop, and no extra units after release following T. Existing packaged handling/movement checks pass. The updated HUD capture `Logs/FoundationSmoke/05-loaded.png` was inspected at 1440 × 900.
- No C# warnings/errors or unexpected game exceptions in the final runs. Editor/package pins, assets, and quantities were retained; no Stage0 run or development build was needed. Local raw evidence remains in the `Logs/Foundation-*` files and `FoundationSmoke/` directory described above. Documentation links, task order, asset/meta pairing, and whitespace were checked; only serialized trailing whitespace was normalized after editor authoring.

Quick check:

1. Pick up the crate with E, hold left mouse over peppers, then release. The count should stop increasing.
2. Press T, then repeat a brief hold/release; it should behave the same, with no mode label or prompt.
3. Pause or Alt-Tab during a hold; resume, release, and hold again to continue. R still recovers the same contents; F8 still restores the test mound.

Human testing of this revision is pending. Processing/tipping/saves and full-cycle balance remain outside this revision. **Next task: 1_03.**

## Quiet-test follow-up — September 6, 2026

The developer reported hearing game audio during automated tests and requested muting it. This is verification maintenance for the current 1_02 handoff; no new gameplay task was selected. Gameplay delivery/feedback remains **Ready for human playtest / Not tested** for the hold-only revision. Tooling feedback is N/A; no new human gameplay acceptance was supplied.

`Tests/PlayMode/FoundationSceneTests.cs` and `HandlingSceneTests.cs` now mute `AudioListener.volume` before loading the yard and restore the previous value in teardown's `finally` block. This covers wrapper runs and the same tests launched through Unity's Test Runner. Audio cues still execute, so dispatch counts and denial limits remain checked. `Runtime/FoundationBuildSmoke.cs` mutes only when batch mode and the explicit smoke flag are present, before scene Awake. Ordinary interactive launches return before any mute is applied. No scene, asset, input, OS volume, or saved audio setting was changed. The verification guide records the mute policy, version-matched API references, and regression.

**Latest ordinary player:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`, with adjacent files. **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity` relative to the Unity root. Windows x64 Mono, Development off; **98,011,479 bytes** reported. Runtime DLL SHA-256: `BAE221FC76B4B6084B5C0107248E76476C6AB1B2AD3289974FDB42A9654BB3FA`. This rebuild replaces the earlier hold-only artifact at the same path.

Verification through `tools/Verify-Foundation.ps1`:

- **PlayMode: 15/15 passed.** The existing full-load integration additionally verifies zero listener volume after twelve dispatched scoop cues and that both audio sources respect listener volume.
- **Build and Smoke: passed.** The exact ordinary player completed its existing input/handling checks, with logged checks `Scoop and feedback audio respect the probe mute` and `Automated player audio stays muted`.
- No C# warnings/errors or unexpected game exceptions in these runs. The editor retains the previously recorded licensing access-token notice without preventing completion. Evidence: local ignored `Logs/Foundation-PlayMode.xml`, `Foundation-PlayMode.log`, `Foundation-Build.log`, `Foundation-Smoke.log`, and `FoundationSmoke/result.txt`.
- Documentation links, queue order, asset/meta pairing, and whitespace checks passed. EditMode rules/assets were unaffected and were not rerun. The separate diagnostic player was not rebuilt; future builds use the same updated smoke component. Interactive Test Runner UI and physical speaker output were not separately observed; automated assertions establish the mute state, not an audio-quality assessment.

Controls remain WASD/arrows, mouse look, Shift sprint, Space jump, E pickup/park, **hold left mouse to scoop**, Esc pause, R recover, F8 restart.

Short check:

1. Run wrapper mode `PlayMode`: the scene checks should finish without audible scoop/handling sounds.
2. Run wrapper mode `Smoke`: the hidden ordinary player should also finish silently.
3. Launch the executable normally (or Play PepperYard), press Enter, pick up with E, and hold left mouse at the mound: normal handling audio should remain audible; release stops gathering.

Human sound-quality/comfort review and the hold-only revision's gameplay confirmation remain pending. Processing/tipping/saves remain outside this handoff. **Next task: 1_03.**

## Free-placement feedback and revision — September 6, 2026

The developer played the processing build and reported that they could not grab/place objects wherever they wanted, questioned the absence of physics and the word “mound,” and could not use the jars accumulating beside the machine. They explicitly rejected the handling restrictions, then requested revised documentation/tasks and a review of `research/case-studies` for more freedom. These observations supersede the earlier positive partial handling feedback for placement; they do not supply a new scoop rating or acceptance of processing.

**Partial / Needs revision.** Reopen 1_02 for free crate placement/rotation/drop, collision and recovery, and clearer pile guidance under the revised brief above. The existing hold-only scoop behavior, quiet automation, scene authoring, and delivered 1_03 processing remain useful partial work. Fixed mats were an implementation convenience, not a necessary consequence of conserved food state. Historical mat-based tests prove the old contract only.

Finished jars are currently a grouped output view. Making them collectable as the one reusable carrier is still 1_04, now explicitly requiring free set-down/regrab before the rack deposit. That known missing feature is not proof of a broken 1_03 transaction. Loose household-object play is separately queued as 1_06. Keep those implementation scopes distinct when resuming one task.

This revision record is documentation/planning only. The current playable artifact and processing evidence remain in [1_03's delivery record](1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026); no new scene, executable, Unity checks, or human acceptance was produced here. NEXT resumes 1_02; after its correction the expected unfinished successor is 1_04. The revised physical interaction still requires implementation and a new ordinary-player handoff.

## Free-placement delivery record — September 6, 2026

The later [quiet-placement delivery](#quiet-placement-delivery-record--september-6-2026) supersedes this record's visible preview and current build evidence. Its physical handling remains in use.

**Ready for human playtest / Not tested.** NEXT resumed the reopened 1_02 placement feedback. The full brief, predecessor movement records, full 1_03 preservation record, linked handling/state/comfort/architecture contracts, saved scene and pinned packages were inspected before editing. This revision replaces compulsory mats and absent physical release, retains hold-only scooping and 1_03 processing, and implements exactly one task. Earlier negative feedback remains above; no human acceptance of the revision is inferred. **Next task: 1_04.**

### Playable artifact and behavior

- **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity` under `games/just-a-few-peppers/unity/`. **Ordinary Windows x64 Mono player:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe` from the repository root, with its complete adjacent folder. Development is off; no Inspector assembly is needed.
- E grabs, carefully places at the aimed geometry, or tips at the intake. Z/X continuously rotates the chosen orientation; G deliberately drops. A green/orange wire volume and contextual text explain valid placement, reach, steepness, clearance, narrow/uneven/moving support, or an obstructed approach. Center/corner support and swept clearance replace mat permission. The new clear worktop and low support left of the opening route demonstrate useful staging; ordinary suitable ground/worktops/supports use the same checks.
- One crate Rigidbody falls, collides and settles. Careful placement starts at rest with zero launch/angular velocity and a non-bouncing friction material. Held geometry cannot push the controller; it sweeps and resolves against scenery. Loaded sprint/jump and the low scoop view remain available. Toppling keeps the exact bulk contents. Contact cues are restrained and automation remains muted.
- `HarvestState` retains one stable `raw-crate` identity, held/released state, finite world pose and last safe pose. R returns the player and recovers held/lost/moving crates without changing food or station progress. Valid supported arrangements remain. Invalid/out-of-bounds poses recover at a revalidated last safe pose or clear fallback; blocked recovery never moves another valid object. F8 explicitly restores all initial food and the initial crate pose. Pause/focus stops physics and processing; fresh input requires releasing the handling controls. E at intake has transfer priority; G wins over simultaneous E. Handling cannot overlap the short tip presentation.
- Player guidance now says **Pepper pile / Peppers left**. The 107-unit local supply, 12-unit crate, first-immediate/0.5-second hold scoop, 0.75-second tip and four-second automatic backend remain. Jars are still output views pending 1_04.

### Implementation and authoring

Under `Assets/JustAFewPeppers/`, new `Runtime/CarrierPose.cs` and `PortableBody.cs` supply value poses, geometry queries, motion and feedback. Updated `HarvestState`, `RawCarrierView`, `YardHandling`, `YardInput`, `YardSession` and `FoundationBuildSmoke` integrate ownership, input, recovery and verification. The existing EditMode/PlayMode suites retain scooping, movement, conservation and processing coverage while replacing mat-specific assumptions. New tests cover free poses, toppling, safe/fallback recovery and input priority.

`Editor/FreePlacementAuthoring.Apply` / wrapper mode `AuthorFreePlacement` migrated the existing scene once and refuses to replace already authored free handling. `TunePlacementPreview` updated the existing preview material through editor APIs. Normal tests/builds use the saved scene. The migration retained **1,253 of 1,272** serialized objects, removed 19 old mat/label/collider blocks and added 47. All prior input map/action/binding IDs and values were preserved. Existing scene and asset GUIDs remain. The five old crate colliders became one matching bulk collision box; pepper proxies still own no food and have no bodies.

New authoring is graybox worktop/support geometry, `Placement outline.mat`, `Crate contact.physicMaterial`, and the physical crate's reuse of the existing Kenney CC0 plank impact. No external pack, purchase, editor/package upgrade or Stage0 regeneration was used. Pins remain **6000.6.0f1 / Input System 1.20.0 / Test Framework 1.8.0 / uGUI 2.6.0**, Built-in rendering. Updated behavior/state/architecture/play guidance and [verification references/regressions](../testing-and-performance.md#free-handling-coverage) describe the actual boundaries.

### Verification evidence

Final commands from the repository root:

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **EditMode 14/14 passed:** retained gathering/processing rules and saved references, new physical/input/preview wiring, finite pose/rotation validation, held/released ownership and distinct last-safe/falling poses. Invalid/repeated commands preserve food; the retained 2,000-command conservation/reservation check passes.
- **PlayMode 21/21 passed:** retained movement, actual hold scooping, denial limits and processing/tip interruption; actual rotated placements at two ground positions, worktop and low support; stable settling/regrab/tip; narrow-support refusal with deliberate drop; gravity/contact/pause/resume; held-wall collision; sprint/jump; fresh G after menus; simultaneous G/E priority; toppled-load regrab; out-of-bounds and blocked-safe-pose recovery; station timer/quantities and other valid arrangements preserved; paused restart. Additional physics scenarios prepare loads through public model commands and deliberately perturb a released pose for the side-landing case. They do not establish ordinary human navigation or subjective feel.
- **Final ordinary Build and Smoke passed.** Unity reports **98,075,463 bytes**. Runtime DLL SHA-256: **`7E5CE103CAC3368BF285D6111C76814BBA58324D07446BDC97D22EA742F79F4A`**. The exact executable uses actual input to scoop twelve units, place/rotate/regrab them on ground/worktop/support, drop despite invalid careful placement, freeze/resume/settle and recover them, then exercises retained full/partial tipping, output reservation and all-food recovery/reset. Deterministic approach positions are probe setup. Final gathering measurement: **5.502 seconds** for twelve units; no complete stored-loop timing or performance claim is made.
- Fresh ordinary-player captures at 1440 × 900 were inspected for menu/control readability, visible placement outlines, chosen supported crate poses, scoop visibility and downward tipping. Raw evidence is local/ignored: `Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, corresponding logs, `Foundation-Build.log`, `Foundation-Smoke.log`, and `FoundationSmoke/result.txt`, `01-menu.png` through `11-full-input.png`, plus `placement-0-*` through `placement-4-dropped.png`. The separate development player was not rebuilt.
- Final runs contain no C# compilation/deprecation warnings or unexpected gameplay errors/exceptions. Sandbox authoring could not connect to Package Manager IPC; authorized editor runs outside the sandbox completed. Existing licensing notices resolve local entitlement; the packaged D3D12 info-queue notice remains. Documentation links/anchors, task order, source/meta pairing, GUID uniqueness and whitespace are checked with the final repository edits.

Real failures were fixed before final evidence: initial scene checks found held-wall overlap and a low pouring edge; enabling queryable held trigger geometry and an elevated tip sweep corrected them. A subsequent pause check found stale Rigidbody interpolation competing with held transforms; holding now disables interpolation. Package captures then exposed an invisible sprite-shader outline despite valid geometry; an opaque unlit material with per-renderer color made it visible. The rendering allocation initially caused an editor constructor exception and was moved into scene initialization. The final suites/build/smoke above follow these corrections; earlier failed runs are not counted as passes.

### Human play checklist and limitations

1. Enter/click Walk, E grabs the gate crate, and hold left mouse scoops the pepper pile. Release stops. Carry a partial/full load while sprinting and jumping.
2. Aim at several chosen ground positions, hold Z/X to rotate, and E places the green preview. Try the clear worktop and low support left of the opening route; regrab each placement.
3. Try a narrow edge or obstructed position, then G to drop the held crate. Watch contact/settling, look down and regrab it; contents must remain. Bring it to the intake and E tips.
4. Pause/Alt-Tab during a drop or station work, then explicitly resume and release controls. R should keep food/progress and valid supported placements. Use F8/the labelled pause-menu restart only for a fresh test.

**Limitations:** graybox art and bulk cosmetic peppers; only the raw crate is portable in this task, with the worktop/support fixed test geometry. Collectable finished jars/carrier and handoff remain **1_04**, loose props **1_06**, physical pepper comparison **1_07**, direct machine operation **1_08**, Coins/purchases **2_01**, and disk saving M3. Full output/input still require F8 to start another test. Physical OS focus/cursor behavior, sound balance, placement/camera comfort and enjoyment need human play; no acceptance or fun rating is claimed. Stop after this handoff.

## Quiet-placement delivery record — September 6, 2026

**Human feedback:** the developer tested the free-placement build and reported that everything works fine, that rotation currently feels unnecessary, and that the placement-allowed/blocked indicator is distracting and should be removed for now. This supplies positive feedback for the tested handling and a specific presentation revision; it does not establish individual checklist results or a fun rating. The requested change stays within 1_02. Rotation remains available as an optional control; no removal or replacement mechanic was requested.

**Delivered behavior:** no green/orange outline and no continuously changing placement-validity text while aiming with the crate. E still validates the chosen surface; a rejected attempt retains the crate and gives the existing brief explanation. Scoop/intake prompts, the aiming dot, bottom control legend, optional Z/X rotation, G drop, physics, food ownership and recovery remain. `Runtime/PortableBody.cs` removes preview drawing/buffers/material-property allocation and keeps the existing scene renderer disabled. `Runtime/YardHandling.cs` stops continuous placement guidance. The saved scene already disables that renderer, so its references/material/meta files are retained without scene regeneration or Inspector work. Existing PlayMode and packaged checks now assert quiet valid/invalid aiming and explanatory rejection while continuing to exercise placement and processing.

**Artifact:** ordinary Windows x64 Mono player, Development off, at `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe` with adjacent files. Scene: `Assets/JustAFewPeppers/Scenes/PepperYard.unity` relative to the Unity root. The rebuilt player replaces the previous artifact at that path: **98,074,439 bytes**, runtime DLL SHA-256 **`FC36EEFB494D91AA6425FA8123A18279AA5E3D0A5D19A641F835FAEC5F9E8AD0`**.

Verification from the repository root:

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **PlayMode 21/21 passed.** Existing rotated ground/worktop/support and narrow-support checks now require hidden preview/continuous guidance; rejected E still explains refusal. Retained handling, physics, recovery, scooping and processing checks pass.
- **Ordinary Build and Smoke passed.** The exact player verifies hidden indicators at supported and unsupported aim, explicit rejection feedback, actual placement/regrab/drop inputs and the retained full/partial processing checks. Automation remains muted. Approach positions and simulated focus callbacks are automated setup, not human navigation/focus evidence.
- Inspected fresh 1440 × 900 packaged captures `Logs/FoundationSmoke/placement-0-aim.png`, `placement-2-aim.png` and `placement-4-unsupported-aim.png`: the ground, worktop and unsupported view have no placement outline or continuous validity label. Existing brief pickup notices and other HUD guidance remain visible where applicable. Raw evidence is local/ignored in `Logs/Foundation-PlayMode.xml`, its log, `Foundation-Build.log`, `Foundation-Smoke.log` and `FoundationSmoke/result.txt`. Older `*-preview.png` files in that directory belong to the preceding build, not this evidence.
- No C# warnings/errors or unexpected gameplay exceptions in these runs. Editor launches used the documented working context outside the restricted sandbox on the first attempt. Existing licensing access-token and packaged D3D12 notices did not prevent completion. EditMode was not rerun because this revision changes presentation only; the earlier 14/14 result is historical. Editor/package pins, authored scene and input bindings were unchanged by this follow-up. Documentation links/anchors, queue IDs/order, asset/meta pairing and whitespace were checked after the final edits.

**Controls:** Enter starts; WASD/arrows move, mouse looks, Shift sprints, Space jumps. E grabs/places/tips, hold left mouse scoops, Z/X optionally rotates, G drops. Esc pauses/resumes, R returns/recovers with food kept, F8 explicitly restarts the food test.

Quick check:

1. Grab and fill the crate, then aim around the ground and worktop. Confirm the distracting outline and placement-validity label stay hidden.
2. E places on a suitable surface; regrab and try an unsuitable position. Only the attempted refusal should explain why it cannot place. G still drops and preserves the load.
3. Regrab the loaded crate and E at the intake to tip. Check that scoop/intake prompts remain useful with the quieter placement view.

**Feedback status:** the earlier free-handling test has the positive feedback above. The queue's **Ready for human playtest / Not tested** refers to this new presentation revision only; its visual comfort has not yet been confirmed by the developer. Rotation's usefulness remains an observation for later tuning. Finished-output pickup/handoff still belongs to 1_04, so full output still requires F8 for a fresh test; disk saving is not implemented. No other task was selected. **Next task: 1_04.** Stop after this handoff.
