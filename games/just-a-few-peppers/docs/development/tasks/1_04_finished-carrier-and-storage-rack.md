# 1_04 — Finished carrier and handoff rack

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Complete the scoop-to-stored-food loop in the same scene.

**Subsequent shared-handling correction:** The [1_06 revision](1_06_loose-yard-objects-and-playful-handling.md#physical-handling-revision-delivery--september-6-2026) supersedes this original brief's automatic nearby hand-switch clauses and old bindings. Current gameplay uses RMB grab/release, explicit set-down before collection and secondary E careful placement. Food ownership, handoff and the historical delivery/acceptance record below remain intact.

**Depends on:** [1_03 — Tipping and automatic processing](1_03_tipping-and-automatic-processing.md). All earlier play gates must also be resolved under the queue rules.

The reopened [1_02 free-placement revision](1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026) must be technically complete first; read its new delivery alongside 1_03. The developer reported unusable jars in the current processing build; this task makes that output actionable.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Household and ending](../../household-readiness-and-parcels.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Add the sole reusable finished-food carrier. Pick up all available output within its tier capacity while preserving active output reservations; new output can accumulate while it is away. Walking, sprinting, and jumping do not transfer ownership or lose its load; boundaries and recovery preserve the exact carried units.
- Make the [finished batch recognizable](../../core-loop-and-mechanics.md#make-the-finished-batch-worth-handling) using simple prepared-food/jar shapes and a readable receiving motion. Packing/arrangement is automatic; no individual jar alignment, lid action or confirmation per item. A partial last batch is visibly valid output, not an apparently incomplete task.
- Reuse the [free handling contract](../../core-loop-and-mechanics.md#pick-up-place-and-play): let the player rotate, place, drop and regrab the loaded finished carrier without depositing or losing its exact contents. Keep the raw carrier where placed; a convenient switch may place it in nearby clear space, never require a mat errand. If that space is blocked, keep ownership and explain what needs moving.
- Deposit the finished load once at the always-accessible **Finished Food Handoff Rack**, then return the empty carrier automatically to its dock. Update the existing foundation placeholder's label/target guidance during this implementation, preserving its asset references. Make output readiness/collection clear so visible finished jars do not appear inert.
- Show stored winter food, clear next-action prompts and one small nearby graybox food group whose volume/fill grows with each accepted deposit. Keep it readable from the work area and derive it from stored units, including partial amounts; counters alone are insufficient. All jar groups remain subordinate to exact pepper-unit state.
- Make the one handoff read as the player's final food-handling responsibility. The nearby group previews accumulation under the [display contract](../../household-readiness-and-parcels.md#progress-drives-presentation); M5 owns the four combined cellar/family compositions and polished food art. Add no distribution or ending in this task.

## Acceptance

- Every unit in a small non-multiple-of-12 harvest can reach the rack; no second output carrier or retrievable duplicate food appears.
- Check repeated/empty deposits, occupied/full output, interrupted pickup/deposit, and loaded-carrier recovery. There is no empty-container return trip.
- Place/drop the loaded finished carrier at chosen ground/worktop positions, pause while it moves, recover and regrab it, then deposit exactly once. Verify raw/finished switching with clear and obstructed nearby space, overlapping targets, and output accumulating while the loaded carrier is parked. Placing near the rack is not an accidental deposit.
- Careful placements settle at the chosen supported pose/orientation without a launch, prolonged bounce or repeated valid-target rejection. Decorative jars do not steal the carrier prompt; deliberate dropping remains available and preserves contents.
- The same permanent target accepts all finished food. Several successive deposits, including a partial amount, visibly grow/fill the nearby stored-food group without an extra collectible copy or secondary inventory. Empty/repeated deposits and recovery leave it unchanged; the empty carrier remains reusable. No rack-to-cellar transfer, household destination choice or helper.

## Human playtest check

Finish several loads and the partial last load. Place the carrier carefully somewhere you choose, resume other work, then regrab and deposit it. Watch the food group grow from the work area and collect again. Report which transfer felt best, whether the finished batch was worth looking at/handling, and whether placement or switching added needless effort.

**Outside this task:** Household display art, parcel allocation, temporary storage relocation, disk saves, or later completion presentation.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_05 — First playable comfort and handoff](1_05_first-playable-comfort-and-handoff.md). Stop after this task's handoff unless the developer explicitly requested a larger range.

## Delivery record — September 6, 2026

**Ready for human playtest / Not tested.** NEXT selected 1_04 after the technically ready 1_02 quiet-placement revision and 1_03 processing delivery. No earlier Partial/Needs revision task or human gate blocked selection. Read the full brief, dependency deliveries, handling/finished-batch/state/household contracts, architecture and Unity/verification guidance, then inspected the actual scene, source and pinned packages. Previous positive handling feedback is retained in 1_02; this request supplies no acceptance of its revised UI or this new task. Exactly one task was implemented. **Next: 1_05.**

### Playable artifact and behavior

- **Scene:** `Assets/JustAFewPeppers/Scenes/PepperYard.unity` under `games/just-a-few-peppers/unity/`. **Ordinary Windows x64 Mono player:** `games/just-a-few-peppers/unity/Builds/JustAFewPeppers/JustAFewPeppers.exe`, with its complete adjacent folder. Development is off; no Inspector work is needed. Final build reports **98,266,863 bytes**; runtime DLL SHA-256 **`49FF7C729B03153B96003617736C0115F664222B9A0047B89FDEB2EE28EE9070`**.
- E at the machine's right receiving tray collects available output, up to twelve units, into the sole reusable finished carrier. Prepared-pepper strips and automatically arranged jar groups show the load, including partial jars. A 0.45-second receiving motion follows the committed transfer. Interrupting it never repeats or erases food. Active reservation/time is preserved; new output can accumulate while the carrier is away.
- Raw and finished carriers share careful geometric placement, optional rotation, physical drop/contact and pose recovery. Placement outlines/continuous validity guidance stay hidden. A direct E hand switch first checks supported clear nearby space for the held carrier. Blocked switching keeps ownership; previously parked objects stay where placed. The finished carrier can be left on chosen ground/worktops, regrabbed or recovered with the same food.
- E aimed at the permanent **Finished Food Handoff Rack** deposits the held finished load once and returns the empty carrier automatically. G wins over E; casual placement beside the rack never deposits. The foundation rack target and label references are retained. Its 36 fixed jar groups derive from stored units, fill upper shelves first and represent the final partial amount. Deposits have a short settling/contact cue; empty/repeated deposits and recovery do not grow the group. There is no retrievable second food copy or empty-container trip.
- The same 107-unit supply, 12-unit raw crate, hold-only scoop cadence and four-second automatic backend now support all nine stored loads, including the last eleven units. R keeps food and valid arrangements; the explicitly labelled F8 restart also clears finished/stored food. Normal yard control remains after all food is stored; this task adds no ending or distribution action.

### Implementation and authoring

Under `Assets/JustAFewPeppers/`, `HarvestState` adds exact finished/stored ownership, docked/held/released poses and atomic collection/switch/deposit commands. `FinishedFoodHandling`, `FinishedCarrierView` and `FoodGroupView` handle explicit targets, shared physical motion and bounded presentation. `YardHandling`, `YardSession`, `PortableBody` and `RawCarrierView` integrate input priority, nearby set-down, shared recovery and restart. The permanent receiving fixture reserves automatic-return space; the empty docked carrier disables its own collider until collection. Decorative jars have no colliders or pickup commands.

`Editor/FinishedFoodAuthoring.Apply` / wrapper `AuthorFinishedFood` made a guarded one-time addition to the saved scene; `TuneFinishedFoodPresentation` then mounted the existing label and changed shelf fill order. Normal tests/builds do not regenerate authoring. All **1,300 prior serialized scene IDs** remain; **28 retained objects** changed and **1,480 serialized objects** were added. Existing output/jar and rack target/label references remain. Only trailing whitespace was normalized after editor saving, with identical serialized tokens.

One graybox `Content/Prepared peppers.mat` joins reused materials, jar shapes, wood/metal primitives and the existing Kenney CC0 plank impact. Asset use is in the [register](../asset-register.md). New source/assets have Unity-generated metas. No external pack, purchase, editor/package upgrade, new binding or Stage0 regeneration was used. Editor **6000.6.0f1**, Input System **1.20.0**, Test Framework **1.8.0**, uGUI **2.6.0**, Built-in rendering and the saved build entry remain unchanged.

### Verification evidence

Commands from the repository root:

```powershell
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode EditMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode PlayMode
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Build
& ./games/just-a-few-peppers/unity/tools/Verify-Foundation.ps1 -Mode Smoke
```

- **EditMode 20/20 passed:** retained gathering/processing rules and saved asset/input/build wiring; complete 1/25/107-unit jobs, final partial loads, active reservations, small carrier capacity, occupied output, away-carrier refusal, invalid/atomic switches, exact-once deposits, pose recovery/reset and 4,000 deterministic mixed commands with conservation and monotonic stored credit.
- **PlayMode 25/25 passed:** retained movement, scoop, tip and physical raw handling; new actual-input receiving/deposit interruption, blocked and clear switches in both directions, quiet rotated ground/worktop placement and settling, jar/fixture targeting, nearby-rack placement without credit, drop/focus freeze/contact/recovery, blocked-safe fallback, preserved raw arrangement, output accumulating while finished food is parked, nine successive deposits and final partial display, ordinary recovery and paused restart. New batch fixtures use public gathering/time commands; existing tests retain actual scoop input. Automation is muted.
- **Final ordinary Build and Smoke passed.** The exact player retains actual scoop/tip checks, then completes nine input-driven tip/receive/handoff cycles for all 107 units with unaccelerated station time. This added complete-job section prepares raw loads through public gathering commands. It verifies finished-carrier sprint/jump, rotated placement/regrab/drop, focus freeze/contact/loaded recovery, raw-arrangement preservation, exact-once credit, automatic dock return and the final two-unit jar fill. Deterministic approaches and simulated focus callbacks are probe setup, not human navigation or physical OS-focus evidence.
- Fresh 1440 × 900 player captures were inspected for receiving motion, loaded food, ground/worktop placement, rack signage and stored-food growth at 12/24/107 units, including views from the receiving work area. Local ignored evidence: `Logs/Foundation-EditMode.xml`, `Foundation-PlayMode.xml`, corresponding logs, authoring/tuning logs, `Foundation-Build.log`, `Foundation-Smoke.log`, and `FoundationSmoke/result.txt`. New captures: `12-receiving-food.png`, `13-finished-load.png`, `finished-placement-0/1.png`, `stored-food-12/24/107.png` and `stored-food-from-work-12/24/107.png`. The separate development player was not rebuilt.
- Initial EditMode **19/20** and PlayMode **24/25** results exposed old test assumptions about the deliberately disabled docked collider and its unavailable grab target. Explicit checks now cover the fixed receiving target instead. A missing LINQ import in that test update briefly prevented compilation and was fixed before the passing runs. The first package passed behavior but exposed the floating-label/low-first-shelf presentation issue. The final build/smoke and fresh captures follow its correction; rules/integration suites were not repeated for that label/shelf-order-only change. No failed run is counted as passing evidence.
- Final test/build runs contain no C# compilation/deprecation warnings or unexpected gameplay exceptions. Editor commands used the documented working context outside the restricted sandbox from the first launch; existing licensing notices did not prevent completion. Package pins/input IDs/build settings were compared with the baseline. Documentation links/anchors, task order/status scope, asset/meta pairing, GUID uniqueness and whitespace were checked with the final edits. No human acceptance or fun rating is inferred.

### Controls, play checklist and limitations

Enter/click Walk starts; WASD/arrows move, mouse looks, Shift sprints, Space jumps. E grabs/places/tips/collects/hands off according to the visible target; hold left mouse scoops; optional Z/X rotates and G drops. Esc pauses/resumes. R returns/recovers with food kept. F8/**Restart food test (clears stored food too)** explicitly resets every food owner.

1. Scoop and tip a load. Collect its jars at the receiving tray on the machine's right, including a small partial batch; inspect the receiving motion and visible food.
2. Place the finished carrier on ground/worktop, resume raw work, then regrab it. Try a direct switch between carriers with clear and obstructed nearby space. Placing beside the rack must not deposit.
3. Drop a loaded carrier, pause/Alt-Tab, resume and regrab/recover it. Check contents and other staged arrangements remain. Release controls after resuming.
4. E at the Finished Food Handoff Rack behind the machine. Watch the upper shelf gain food and the empty carrier return. Repeat deposits and inspect growth from the receiving work area; there is no further food-delivery errand.
5. Finish successive loads and the final partial load. Stored food should reach 107/107 with ordinary yard control available. Use R to keep progress; use F8 only for a fresh test.

**Limitations:** deliberately simple graybox jars, prepared-food strips and fixed rack display; only the two carriers are portable here. Loose props, physical-pepper comparison, direct operation, Coins/purchases, disk saves, polished household displays and completion presentation remain their later tasks. General hand placement is geometry-based; automatic receiving uses its fixed fixture. Full-cycle human travel/service balance, physical Alt-Tab/cursor behavior, sound quality, receiving/food appeal and whether switching feels useful still require play. **Human feedback: Not tested. Next task: 1_05.** Stop after this handoff.



## Human playtester feedback — September 6, 2026

The developer reported: "i tested the task works fine, implement next one". This accepts the last playable delivery, 1_04, to continue. Its implementation is complete; the intervening specification update did not change the build. No individual fun rating or detailed coverage of every checklist case was supplied. The new single/bulk pepper and grouping requirements remain unfinished 1_07/1_08 work. Queue delivery is Done / Accepted to continue; next implementation is 1_05.
