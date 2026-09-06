# 1_07 — Physical pepper batch comparison

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Deliver deliberate single-pepper and predictable bulk handling of a representative scattered physical batch, then choose measured simulation limits without losing food.

**Depends on:** [1_06 — Loose yard objects](1_06_loose-yard-objects-and-playful-handling.md). All earlier play gates must be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), dependency delivery/feedback records, and [core mechanics](../../core-loop-and-mechanics.md), [scope/validation](../../scope-and-validation.md), [state/saving](../state-and-saving.md), [architecture](../../../ARCHITECTURE.md), and relevant [verification](../testing-and-performance.md) / [asset rules](../unity-and-assets.md). Inspect actual source, scene/input wiring and pinned versions before implementation; the brief is not delivery evidence.

## Work

- Reuse the completed crate/handoff loop and shared handling. Add a representative scattered group of physical gameplay peppers in the same work area: select one intended pepper, pick it up, move/place it, put it into a container, deliberately gather several and pour them. Individual handling is optional; the harvest must remain convenient in bulk. Use the existing raw carrier as the minimal bulk destination; a separate held-group system is not required.
- Compare bounded physical batches with grouped resting/distant representation under equal quantities and timing during pickup, filling and pouring. Both candidates retain the nearby gameplay above; disappearing scenery plus cosmetic particles/counts cannot be the chosen replacement.
- Implement the [single/bulk/placement/pouring control contract](../../core-loop-and-mechanics.md#single-bulk-placement-and-pouring-controls): a visible single target, previewed bulk affected set and destination before commit, capacity-capped selection, occlusion and stable contents/container priority. Teach each action briefly at its target; no hidden modes/unexplained modifiers. Preserve hold-left-mouse bulk gathering/release-to-stop, quiet denial cues and hidden placement indicators; a bulk selection preview does not restore placement outlines. Document exact delivered bindings.
- Let physical peppers contact the carrier/intake and settle convincingly. Choose active-body limits, sleeping/reuse and grouped resting contents from measured behavior rather than a blanket no-physics rule. Use placeholders or suitable licensed assets; no full-harvest simulation requirement.
- Register live interactable pepper IDs, exact units, owner/membership and recovery poses under the state contract. Single/bulk pickup, taking targeted contents out of a container, partial acceptance and representation changes move the same units once. A gameplay spill/off-target pour creates recoverable loose ownership, never silent loss or a second copy. Provide forgiving recovery for inaccessible strays without requiring a tiny-item scavenger hunt.
- Verify exactly-one selection, capacity-limited previewed bulk pickup, occluded/overlapping contents, partial container transfers, pause/focus, interrupted pours, off-target spills, body recovery, repeated loads and partial final amounts in the actual scene/player. Record active/sleeping body counts, frame behavior, visual contact and limitations.
- Choose and integrate a technically viable default, retain useful comparison evidence and report pending human preference honestly. No fun rating is inferred from physics or screenshots.

## Acceptance

- Pick exactly one intended physical pepper, deliberately gather several without surprising selections, place them into a container and pour with convincing contact. The affected set is visible before bulk commitment and respects capacity/occlusion. Single pickup never grabs the container when its contents are targeted.
- The chosen representation preserves exact ownership through single/bulk pickup, filling, full/partial pours, spills, interrupted motion and recovery. Repeated contact/recovery cannot duplicate material; decorative proxies never own another copy.
- Packaged input checks and observed contacts cover repeated use, physical interruption and the final partial batch. No routine tunnelling, growing body leak or unrecoverable food remains.
- The decision states what is simulated, what owns food, measured limits, tradeoffs and untested feel. It retains required nearby physical gameplay without claiming the entire harvest needs active rigidbodies. Record individual handling, bulk handling and control clarity separately; screenshots cannot prove enjoyment.

## Human playtest check

Pick exactly one pepper from a scattered group and from exposed container contents. Preview and deliberately gather several, including near capacity and an obstacle; check that only intended peppers move. Fill/pour full and partial loads, miss the intake or drop a carrier, pause during motion, then recover and finish the same food. Compare individual handling, bulk handling and control clarity separately using the recorded representation alternative.

**Outside this task:** new machine operations (1_08), Coins, all-yard pepper rigidbodies, cooking judgment, disk saves or second products.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Own scene/prefab, input, UI, assets and build wiring; record exact scene/build, controls, short checklist, checks, limitations and actual feedback. Update queue/milestone records and stop after this handoff.

Next in order: [1_08 — Direct machine operation](1_08_direct-machine-operation.md).

## Planning record — September 6, 2026

Added for the supplied processing-and-inventions direction. Todo / Not tested; no new gameplay or measured comparison is delivered by this documentation pass.

## Delivery record — September 6, 2026

**Selection and feedback:** selected NEXT after the developer's “looks better now implement next task.” The [1_06 feedback record](1_06_loose-yard-objects-and-playful-handling.md#human-feedback-after-the-handling-revision--september-6-2026) records acceptance to continue for that revision only. This delivery implements 1_07; direct operation and output grouping remain 1_08. Existing placement indicators remain hidden and general instructions remain in F1.

**Delivered:** a scattered 107-unit work patch with stable one-unit IDs, precise single pickup (including exposed crate contents), previewed capacity-limited bulk gathering into the existing raw crate, physical insertion/settling, full/partial contact-based intake pouring, recoverable missed pours/toppling and one-gesture stray recovery. A finite physical batch is the default; F9 retains the grouped-rest comparison with the same nearby actions and food ownership. [Exact controls/behavior](../../core-loop-and-mechanics.md#current-physical-pepper-batch--1_07), [ownership/recovery](../state-and-saving.md#transactions-and-reconstruction) and [component boundary](../../../ARCHITECTURE.md#implemented-physical-peppers--1_07) are updated.

Bulk preview highlights only reachable, unobstructed IDs within 0.65 m of the intended target, up to three and capped to crate space. A changed set is displayed for at least 0.12 seconds before commit, with a 0.5-second transfer cadence and release-to-stop. The crate marker and count show the destination. A single contents target never grabs the whole container. Held cargo uses stable slots; after release it contacts actual hollow walls/floor. Each intake-directed ID reserves available space, then actual intake contact commits it once. The 1.15-second pour window closes before the accepted batch auto-starts. Missed/interrupted unaccepted material remains loose; a rejected capacity remainder stays in the crate. R regroups raw strays at their original supply without resetting station or stored food.

**Changed paths:** under `Assets/JustAFewPeppers/`, new `Runtime/HarvestPeppers.cs`, `PepperBody.cs`, `PepperBatch.cs`, `PepperBatchBuildSmoke.cs`, `Editor/PepperBatchAuthoring.cs`, `Content/Physical pepper.prefab`, and `Tests/EditMode/PepperStateTests.cs` / `Tests/PlayMode/PepperBatchSceneTests.cs`, each with its meta. Existing model/handling/carrier/input/HUD/session and packaged probe files integrate them. `Scenes/PepperYard.unity` and `Content/YardControls.inputactions` are editor-authored; retained crate meshes match the hollow collision geometry. Shared scene/asset tests and `tools/Verify-Foundation.ps1` cover the new path. The asset register, play guide, behavior/technical contracts, queue and milestone summary are updated. No new external assets, package upgrade, Inspector assembly or Stage0 regeneration is required.

**Playable artifact:** ordinary Windows x64 Mono, Unity 6000.6.0f1, Built-in pipeline, Input System 1.20.0; Development disabled. Scene: `Assets/JustAFewPeppers/Scenes/PepperYard.unity`. Build: `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`, keeping adjacent files together. Build report: **98,374,899 bytes**. `JustAFewPeppers_Data/Managed/JustAFewPeppers.Runtime.dll` SHA-256: `D2EB6FB2C3DF5F3D3A261B4735737C4E2D92DF1BD20008DF2FA6B913FBD81C0B`.

**Controls:** Enter/click Walk; WASD/arrows move, Shift sprint, Space jump, mouse look. RMB grabs one targeted pepper/container/prop and releases a held object; G also releases. With the raw crate, hold LMB to gather the highlighted set and release to stop. With one pepper or a loose prop, hold/release LMB to charge/throw. E sets down/puts a pepper into the crate, pours at the intake, collects finished food or hands it off at the rack. F explicitly pours the raw crate toward your aim, including off-target spills. Z/X optionally rotate carriers/props. Esc pauses; F1 shows all keys/current-work help; F9 compares representations and reports the mode in F1. R recovers objects/regroups raw strays with food kept. F8 explicitly restarts the food job, clearing stored food too.

**Short play check:**

1. Pick exactly one highlighted pepper, put it into the orange crate with E, then target that visible content and pick it back out. Drop/toss it and regrab it.
2. Grab the crate and hold LMB at the supply. Watch the highlighted set and destination, release to stop, then fill the final two spaces. Try beside an obstruction; only visible highlighted peppers should move.
3. Pour a partial and a full load at the intake. Set the crate down, collect finished food with E and hand it off at the rack. Repeat to include the final eleven-unit load.
4. Use F away from the intake or topple a loaded crate. Pause/Alt-Tab during motion, resume with fresh input, then R to regroup strays and finish the same food. R keeps station/stored progress; F8 clears the test.
5. Repeat single/bulk handling with F9's grouped-rest alternative. Assess **individual handling, bulk handling and control clarity separately**; use F1 only when wanted.

### Verification and measured decision

Editor commands ran in the documented working Windows execution context outside the restricted sandbox from the first launch; no editor administrator mode or license repair was used. Commands use `& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode <mode>`. Authoring modes: `AuthorPepperBatch`, `ConfigurePepperComparison`, `ScatterPeppers`, `AlignPepperCrate`. Builds use the saved scene without authoring reruns.

- **EditMode: 26/26 passed**, including six new pepper state cases and retained rules/asset checks; `Logs/Foundation-EditMode.xml` (0.213 seconds).
- **Final PlayMode: 42/42 passed**, including six new pepper integration cases and the strengthened visible-contents regression; `Logs/Foundation-PlayMode.xml` (106.540 seconds). Shared movement, food, carrier and four-prop cases remain covered under the current physical contract.
- **Build and final Smoke passed** with the artifact above. The ordinary player verified actual single/contents pickup and visible crate contact in both modes, previewed two-space bulk filling, full intake contact, off-target spill/pause/automatic recovery, and all 107 units through nine physical pours and exact-once handoffs (last load eleven). Full-job raw loads use public gathering preparation; transfers use actual input and unaccelerated processing. Menu/F1/sensitivity, movement, simulated focus and four-prop physics checks also passed. Evidence: `Logs/FoundationSmoke/result.txt`, `Foundation-Smoke.log`, comparison files and captures. No game exception or C# warning was found in final logs; the editor still logs its known nonfatal unavailable licensing access-token refresh, then builds successfully.

Earlier failures are retained honestly: the first focused pepper run passed 5/6 and exposed the rim-insertion issue (`Task1_07-PlayMode-initial.xml`). The first full run passed 26/42 (`Task1_07-PlayMode-regressions.xml`): obsolete cosmetic scoop/tip timing, body-count/mound assertions and a real cargo-release pose bug needed correction. The subsequent 41/42 run (`Task1_07-PlayMode-cargo-fixed.xml`) left one obsolete “toppled raw crate still holds six” assertion; the new contract verifies retained plus loose food and same-ID recovery. An initial EditMode help-wording assertion was also updated. After full scene and package passes, visual capture inspection found the hidden contents problem; the strengthened assertion failed 0/1 (`Task1_07-PlayMode-hidden-contents.xml`) before editor-authored mesh alignment fixed it. The alignment utility first failed compilation because its shared Collider array needed a BoxCollider cast; the corrected command and final build compile successfully. No exception suppression was added.

**Representation decision:** keep `PhysicalBatch` as the technically viable prototype default. Its finite 107 pepper actors are allocated once and reused; ordinary source/released contents use dynamic capsule collision, held cargo uses arranged kinematic slots, and processed actors deactivate. Alongside six existing portable bodies, the authored ceiling is 113 reusable bodies; actions do not grow that pool. `GroupedRest` still allocates the same 107 actors but activates at most the nearest 36 Source bodies within 5 m, plus all held/carrier/loose/transit food. Thus 36 is a nearby-source limit, not a global body cap; spills can raise its active count. Nine non-owning coarse proxies stand in for inactive sources.

Both modes support the same nearby single/contents/bulk/pour checks. Grouped rest reduces physics work but spends more CPU sorting/selecting and switching representation, while showing coarse proxy changes. The current small patch benefits from the simpler default's persistent physical appearance and lower measured handling cost. This is a technical choice for human comparison, not an enjoyment result or permission to simulate a full production harvest.

Final packaged samples: three alternating-order trials, 200 fixed 0.02-second steps per phase (four simulated seconds), equal 107-unit starting supply and 12-unit fill/pour. “Scattered” measures nearby source/targeting; filling measures a prepared loaded crate settling; pouring measures actual E-initiated transit/contact. Table timings are the **median of three trial medians / median of three trial p95s**, separately for physics and handling, in milliseconds. Active/awake counts are taken at phase start; sleeping is the end-of-phase range.

| Candidate | Phase | Active / awake at start | Sleeping at end | Physics median / p95 ms | Handling median / p95 ms |
| --- | --- | --- | --- | --- | --- |
| PhysicalBatch | Scattered | 107 / 107 | 98–101 | 0.0511 / 0.1045 | 0.0292 / 0.0359 |
| PhysicalBatch | Filling | 107 / 107 | 103–104 | 0.0412 / 0.1339 | 0.0296 / 0.0386 |
| PhysicalBatch | Pouring | 107 / 107 | 91 | 0.0367 / 0.1192 | 0.0272 / 0.0358 |
| GroupedRest | Scattered | 36 / 36 | 26–31 | 0.0370 / 0.0802 | 0.1118 / 0.1281 |
| GroupedRest | Filling | 48 / 48 | 45–46 | 0.0265 / 0.0936 | 0.0987 / 0.1154 |
| GroupedRest | Pouring | 27 / 27 | 15 | 0.0193 / 0.0648 | 0.1024 / 0.1171 |

All measured loops reported **0 managed bytes per step**. This brackets only `Physics.Simulate` and `PepperBatch.Tick`, excluding input-command/UI allocation, file writes and captures. Other scene bodies participate in physics; synchronous measurement does not advance game time or other gameplay Update methods. Separate normal-time input checks prove pour completion. Hardware/configuration: AMD Ryzen 7 9700X, Radeon RX 9060 XT, 63,033 MB reported RAM, 1902×963 window, vSync 1, targetFrameRate -1, ordinary non-development batch player. Batch mode may skip rendering: these are simulation/handling CPU samples, **not rendered FPS, full-frame allocation or minimum-hardware acceptance**. Raw evidence: `Logs/FoundationSmoke/pepper-comparison.csv` and `pepper-comparison-context.txt`.

Fresh captures inspected: `PhysicalBatch-single.png`, both `*-contents.png` (now visibly above the crate floor), both bulk previews, `PhysicalBatch-physical-pour.png`, F1 help and final stored-food view. Bodies visibly fall into the tray and settle inside the crate; no repeated-load tunnelling, growing body pool or unrecoverable food was observed by the contact/conservation checks. Captures establish static readability; ongoing feel still needs human play.

The decision uses the pinned official [Unity 6000.6 Physics.Simulate contract](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/Physics.Simulate.html) for fixed-step measurement and [collision callback contract](https://docs.unity3d.com/6000.6/Documentation/ScriptReference/MonoBehaviour.OnCollisionEnter.html) for eligible physical contacts. Measurements cannot establish enjoyment or per-input/full-frame allocation costs.

**Documentation/preservation checks:** 60 Markdown files, 829 local links, 324 heading targets, balanced fences, 89 JustAFewPeppers asset/folder meta pairs and 34 unchanged queue IDs/order passed. `git -c core.safecrlf=false diff --check` passed after removing Unity-generated trailing whitespace without changing serialized tokens. Against this turn's 331-file starting snapshot, 28 existing files changed and 303 remained byte-identical; eight assets/source files plus their eight metas were added. All 119 pre-existing metas, 2,949 prior scene object IDs and 61 prior input IDs remain; the scene now has 2,994 IDs and input 65. Packages, project settings, AGENTS.md, Stage0, other task briefs and user design drafts remain unchanged apart from the selected delivery and prior-feedback record. The local `Logs/Task1_07-preservation.json` records exact paths. No commit or publication was performed.

**Limitations and human feedback:** **Not tested** for this new task. Individual handling, bulk handling, representation preference and control clarity each need firsthand review; the prior positive feedback supplies none of those ratings. Temporary primitive art, reused muted-in-automation audio and a guided pour pose remain provisional. Grouped proxies are coarse and can visibly switch to nearby actors. Held crate contents are arranged; released contents/intake contact use physics. R intentionally regroups raw pepper arrangements, while preserving other valid prop/carrier arrangements. The work-patch ceiling is not a production-yard body budget. Actual OS focus/cursor behavior, audible quality, rendered frame rate, low-end hardware and human navigation/pacing remain unverified. Direct apparatus operation/provisional output grouping, Coins and disk saves are outside this task.

**Next task: 1_08 — Direct machine operation.** Stop after this handoff.
