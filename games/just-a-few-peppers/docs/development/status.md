# Implementation status

Updated September 6, 2026. This file summarizes milestone delivery and evidence. The [numbered queue](tasks/readme.md) is the source of truth for individual tasks and their feedback; [roadmap](roadmap.md) explains milestones. Use the [fresh-chat prompt](new-chat-prompt.md) to continue chronologically.

## Current milestones

- [x] M0 — Repository organization and planning: grouped files, development contracts, relocation verification, documentation checks, and legacy state-test rerun completed. Evidence below.
- [ ] M1 — First complete crate loop, tasks 1_01–1_05: In progress. The walkable foundation is delivered; the complete crate loop and player acceptance remain pending. See [task evidence](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026) and the [queue](tasks/readme.md#ordered-task-queue) for current task/feedback state.
- [ ] M2 — Wheelbarrow discovery and fun gate: Todo.
- [ ] M3 — local save, resume, and recovery: Todo.
- [ ] M4 — Complete graybox yard and final station: Todo.
- [ ] M5 — Representative vertical slice: Todo.
- [ ] M6 — Content and balance: Todo.
- [ ] M7 — Shipped menus, controls, and comfort: Todo.
- [ ] M8 — Regression/performance hardening: Todo.
- [ ] M9 — Shipping preparation and final artifact checks: Todo.

The current walkable foundation is technically delivered; bulk handling and the complete M1 loop remain unfinished. Foundation verification does not complete a milestone or establish enjoyable controls.

## Existing implementation and evidence

| Item | Status / evidence |
| --- | --- |
| Historical one-pepper scene | Exists under `unity/Assets/Stage0/`; different roast/steam/peel loop. |
| Legacy deterministic rules | 10/10 standalone state tests passed from the relocated project on September 5, 2026. |
| Legacy scene/build/feel | Discarded experiment; old reports removed. Further Stage0 acceptance is not a current gate. No Unity scene/build checks were rerun for the documentation cleanup. |
| Raw pile representation and fun | Untested. |
| Foundation checks and builds | [1_01 movement revision](tasks/1_01_unity-foundation-and-walkable-scene.md#movement-revision--september-6-2026), including original delivery history; [current play guide](../../unity/readme.md). |
| Full saves / full game | Not implemented. |

Historical relocation evidence, September 5: 97 Unity asset/source/settings/test files matched their pre-move hashes. All 37 original Markdown documents were relocated; the original bootstrap content was unchanged. Documentation links, encoding, formatting, and ignore rules were checked at that point. These figures describe the relocation before archive cleanup, not the current document inventory.

September 6 documentation update: removed the ten discarded roasting/prototype archive files; retained source research centrally with direct links from the game; added the start guide, supported Unity API/Input System rules, and free commercially usable asset policy. Local document links, heading targets, UTF-8 decoding, and code fences checked successfully. No gameplay source, packages, or project settings were changed, and no Unity or legacy state tests were rerun for this update.

September 6 + current consolidation pass: aligned requirements, scope contracts, and developer workflow text across design and task docs for the next implementation chat. This was documentation-only; queue ordering, task IDs, and delivery states are unchanged.

## Recording future work

The September 6 task mapping adds 31 concrete briefs and the fresh-chat protocol. At mapping time only 0_01 (planning) was Done; 1_01–9_03 were Todo. Task 1_01 was subsequently delivered below. Documentation checks passed for local links, heading targets, task IDs/order, predecessor/next links, required brief sections, and initial statuses. No gameplay, package setup, or Unity/state-test runs were performed for this mapping.

Maintain task state in the [queue](tasks/readme.md#ordered-task-queue) and execution evidence in each task's delivery record. This file tracks aggregate milestones and repository history; update it when that aggregate state or a blocker changes. Follow the queue's review gates and [handoff rules](tasks/readme.md#handoff-and-recording). The [M1 contract](first-playable-task.md) spans 1_01–1_05.

September 6 implementation: [1_01 delivered the foundation](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026), making M1 in progress. Its delivery record owns package choices, test/build evidence, regression details, and untested human checks.

September 6 process improvement: reduced repeated workflow reading and status copies while preserving feature specifications, task briefs, and review gates. The task queue now routes context by the affected system. Ordinary playtest and development diagnostic builds have separate paths; build maintenance evidence is appended to [1_01](tasks/1_01_unity-foundation-and-walkable-scene.md#process-and-build-follow-up--september-6-2026). No gameplay task or player acceptance advanced.

September 6 scope lock: updated existing design and affected development contracts for English-only draft dialogue, the Finished Food Handoff Rack, a final processor that combines capacity/output gains with a substantially shorter loaded route, a measured pocket count capped at five, and an automatic last-deposit meal. The M1–M2 interaction prototype remains one corner without household/narrative/ending systems. [Scope-lock audit and changed files](../design-pivot.md#scope-lock-audit--september-6-2026) records the decisions and remaining risks. No code/assets/packages/builds or shared research changed; task IDs/order, review gates, delivery statuses, feedback, and historical execution evidence remain unchanged. Documentation links, scope consistency, and preservation checks passed; no Unity or legacy tests were run for this documentation-only pass.

September 6 harvest-completion amendment: the current specs now replace the compulsory meal/day transition with a final-deposit harvest-completion state that leaves normal movement/camera and pause/menu control available. A table/gift/thank-you beat remains optional, cuttable presentation. The discovery-only prototype remains current; a bounded Coins comparison is pending separate authorization after 2_03 and before whole-yard production. This amendment changes no Unity source, serialized data, assets, packages, builds, task IDs/order/status, or historical play evidence. The preceding scope-lock paragraph remains a historical record of the superseded completion clause.

