# Development roadmap

Status: plan for the complete current game; task 1_01 foundation is delivered and the remaining gameplay is not implemented. The [numbered queue](tasks/readme.md) maps every milestone to concrete chronological task files and holds per-task progress. [Status and evidence](status.md) summarizes milestones. [Current scope](../scope-and-validation.md) limits every task.

Build in the order below. Each step delivers something inspectable and has an exit gate. Later content work does not compensate for a failed core interaction. For a fresh implementation chat, use the [reusable prompt](new-chat-prompt.md) and request NEXT or a numbered task such as 1_01. [Start here](start-here.md) explains the workflow; [Unity and asset rules](unity-and-assets.md) apply throughout.

## Milestones

| Step | Deliverable | Exit gate |
| --- | --- | --- |
| M0. Repository and plan | Grouped project, current specs, audited baseline, ownership/save/testing contracts. | Links and moved source verified; existing checks recorded honestly. |
| M1. First complete crate loop | One graybox mound → crate → automatic station → output carrier → Finished Food Handoff Rack, with local depletion and feedback. | Repeated and partial loads finish in-scene without lost units or exceptions; rules and integration checks pass. |
| M2. Discovery and wheelbarrow | Visible wheel, authored discovery pocket, 48-unit carrier and matching station support. | Equal-quantity upgrade comparison and first action-enjoyment evaluation pass. |
| M3. Save, resume, recovery | One versioned snapshot, safe restore, settings persistence, and a backup/error path. | Save/reload across all current transfer and machine states preserves the harvest; interrupted writes and invalid data recover honestly. |
| M4. Complete graybox yard | Measured pocket count within the five-area cap, useful shortcuts, final station with a nearby intake, finite harvest, automatic ending stand-in. | Different valid orders reach the ending after the last deposit; the final upgrade measurably improves the whole job including hauling/output handling. |
| M5. Representative vertical slice | A short playable arc with representative pile/carrier/station art, audio, UI, food displays, Grandpa and meal transition. | The core still feels good with real assets; build, save, performance and full-arc gates pass before broad asset production. |
| M6. Content and balance | Finished compact property, remaining authored piles, scenery and selected dialogue. | All content follows the same rules; no hidden leftovers; upgrades arrive while useful work remains; duration measured from play. |
| M7. Menus and comfort | New/continue flow, exit/save handling, persisted controls/audio/display options, final-target assistance. | Comfortable keyboard/mouse play and settings survive restart; UI and input remain usable at supported display settings. |
| M8. Hardening | Regression coverage, repeated complete runs, profiling, save failure checks, clean Windows release candidate. | No known progress blockers, duplicate food, corrupting saves, or unexplained runtime errors; performance evidence recorded. |
| M9. Shipping | Versioned build, credits/license inventory, accurate screenshots/trailer/store materials, release checklist and archived evidence. | The exact distributable passes install/start/continue/finish checks; published claims match measured gameplay and tested compatibility. |

M1 is in progress with task 1_01 Ready for the human; later milestones remain Todo. The historical Stage0 spike is separate evidence, not M1.

## M1 — prove a load

Follow tasks **1_01–1_05** in the [queue](tasks/readme.md), starting with [1_01](tasks/1_01_unity-foundation-and-walkable-scene.md). The [first-playable contract](first-playable-task.md) defines their combined result. Create only the necessary scene, runtime model/views, compatible Input System setup, and Unity test setup. Wire the scene and controls completely so the developer can press Play. Keep the starting crate available immediately. Use one 12-unit feed/output tier and enough finite supply for repeated loads plus a partial final batch.

The scope includes responsive scooping, stable carrying, broad tipping, automatic processing, collecting output, depositing once at the sole handoff rack, pause, restart, and a recoverable carrier. M1–M2 together stay within the [interaction prototype boundary and eight questions](../scope-and-validation.md#next-experiment-one-pile-one-carrier-one-discovery). No Grandpa dialogue, household display states, final machine, cellar, parcels, grinder, ending scene, or disk saves enter this prototype.

Immediate visual response and useful action sound belong in M1. A number changing behind placeholder UI alone is not a valid feel test. Record the first successful scene and packaged-player smoke evidence.

## M2 — prove discovery and increased capability

Expose the wheelbarrow through a small reachable region of the same authored mound and leave a comparable quantity available afterward. Change gathering width and carrier capacity together with station support. Compare the same 48 units before and after the upgrade, counting travel, waiting, and output work. Do not enlarge the one-corner prototype to demonstrate discovery.

Use the six-player approach where practical: median ordinary-action enjoyment at least 4/5, at least four of six choosing unrewarded continuation, median forced waiting/support friction at most 20%, and no unresolved completion blocker. Separate pleasure in the action from pleasure in the joke or reveal.

If the result fails, make at most two focused control/representation/layout revisions and retest. Do not fill the full yard or add recipes to rescue the result. Mark incomplete evidence inconclusive.

## M3 — make progress dependable

Implement the [state/save contract](state-and-saving.md) before increasing authored content. Exercise held raw food, mid-batch processing, an accumulating output, a carried finished load, pending upgrades, and stored progress.

Resume at committed logical state and reconstruct views. Add one local save slot with a previous valid backup, safe-write behavior, schema/content validation, and understandable error feedback. No cloud, slot browser, or old Stage0-save compatibility is needed.

## M4 — complete the functional game

Use the [yard design](../yard-and-progression.md) after recording core-prototype measurements. Choose the fewest areas that carry the full progression, with five as the maximum; three or four need no fifth. Preserve useful alternate approaches where the compact layout supports them without implementing the A–E sketch literally. The cellar is a view and shortcut, not a new inventory. All stock remains processable by the starting line.

Keep the final raw carrier as the 48-unit wheelbarrow. Combine two outputs at the 96-unit station and expose a fixed intake close to the final supply, using the same dump interaction and logical station. Follow the [combined upgrade measurement contract](../scope-and-validation.md#later-checks-for-the-complete-game): compare the same 96-unit job from the same supply before/after the authored intake route, including loaded travel, waiting, finished-output trips, and empty walking. Prove repeated useful cycles remain after reveal and test final-machine-before-wheelbarrow order. A same-path capacity test alone is not acceptance.

After all harvest is cleared and stored, the final accepted deposit automatically commits completed-day state and starts a simple ending stand-in. Check interrupted transition/save restoration now. M5 replaces the stand-in with the meal; it does not add a Finish button or table task.

## M5–M6 — earn and finish the presentation

Build one representative short arc with real intended assets before dressing every corner. Include local pile depletion, the wheelbarrow reveal, a large machine dump, a rack deposit, household displays, and the meal. A reduced test supply is allowed; record it as a test layout.

Profile that slice and check input, save/resume, readability, audio comfort, and build behavior. Then finish the remaining property using the same components and visual techniques. Source free commercially usable assets first, following the [asset policy](unity-and-assets.md). Inspect licenses and scale/colliders/materials, import only useful content, and record actual imports and required credits. Reserve custom work for distinctive equipment or interaction needs.

Use a small selection from the canonical English draft dialogue bank and preserve eventual localization/native review as later presentation work. Returned jars, lyutenitsa, grinder, cellar foods, family boxes, old fridge/tool cupboard, reused tools, vine table, and Grandpa's bottle/gift remain presentation. The four food-display states derive from stored progress, never separate task scripts.

Measure enjoyable duration and adjust the illustrative 732-unit manifest. Cut empty walking and excess repetitions. Do not preserve a quantity or map pocket merely because it appeared in the proposal.

## M7–M9 — finish the product

Provide basic pause/sensitivity/hold-toggle controls from M1; M7 completes and verifies the shipped options: bindings, sensitivity/FOV, invert Y, volume, supported display modes, and reduced motion. Make progress and the last remaining actionable pile readable without adding a scavenger hunt.

Run systematic save/recovery and full-game checks in M8, then freeze a candidate and test that artifact. Rebuild only when a fix warrants it, preserving exact version/evidence links. Set final minimum hardware and supported display claims from measured builds.

M9 includes complete credits and asset/music/font license records, final product/build identity, truthful media, a concise player support guide, and distribution preparation. Publishing is a later implementation/release task, not part of this repository organization request.

## How agents take work

Start from the [task queue](tasks/readme.md), [status](status.md), and the selected task's required context/dependency records. Implement one numbered task, integrate a playable scene/build, verify its acceptance criteria, and update the queue, task delivery record, milestone summary, and affected contracts in the same change. Hand over the exact scene/build path, controls, and a short play checklist. Record the tester's feedback separately from technical readiness. Record important bugs with their preventing tests in [testing](testing-and-performance.md#regression-records).

The numbered briefs now supply the requested feature-level plan for the full current scope. Refine them when implementation evidence requires it; add a follow-up for a new requirement rather than silently changing completed task IDs. Keep useful completion evidence with the task and summaries in status. Routine iterations do not require repeating legacy checks or recruiting a full playtest cohort.



