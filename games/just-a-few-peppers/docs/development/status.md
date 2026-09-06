# Implementation status

Updated September 6, 2026. This file summarizes milestone delivery and evidence. The [numbered queue](tasks/readme.md) is the source of truth for individual tasks and their feedback; [roadmap](roadmap.md) explains milestones. Use the [fresh-chat prompt](new-chat-prompt.md) to continue chronologically.

## Current milestones

- [x] M0 — Repository organization and planning: grouped files, development contracts, relocation verification, documentation checks, and legacy state-test rerun completed. Evidence below.
- [ ] M1 — Physical batch loop, tasks 1_01–1_08: In progress. The interim scoop-to-stored-food loop is technically delivered, including freely handled raw/finished carriers and automatic processing. Next is 1_05 comfort/handoff; loose props, physical pepper comparison and direct operation remain pending. See the [queue](tasks/readme.md#ordered-task-queue) for task/feedback state.
- [ ] M2 — Coins, two equipment choices and core human gate: Todo. Both working offers and purchase-use evidence belong before 2_03.
- [ ] M3 — local save, resume, and recovery: Todo.
- [ ] M4 — Accessible production yard, powered apparatus and food completion: Todo.
- [ ] M5 — Representative vertical slice: Todo.
- [ ] M6 — Content and balance: Todo.
- [ ] M7 — Shipped menus, controls, and comfort: Todo.
- [ ] M8 — Regression/performance hardening: Todo.
- [ ] M9 — Shipping preparation and final artifact checks: Todo.

The current scene supports scooping, free raw/finished placement/rotation/drop, tipping, automatic batches, receiving, permanent food handoff, nearby stored-food growth and recovery. All authored food can reach storage. Revised M1 still needs comfort review, loose props, physical pepper comparison and direct operation; Coins/purchases remain M2. The queue retains supplied feedback and separates technical readiness from human acceptance.

## Existing implementation and evidence

| Item | Status / evidence |
| --- | --- |
| Historical one-pepper scene | Exists under `unity/Assets/Stage0/`; different roast/steam/peel loop. |
| Legacy deterministic rules | 10/10 standalone state tests passed from the relocated project on September 5, 2026. |
| Legacy scene/build/feel | Discarded experiment; old reports removed. Further Stage0 acceptance is not a current gate. No Unity scene/build checks were rerun for the documentation cleanup. |
| Raw pile representation and handling | Local depletion and crate filling have technical evidence in [1_02](tasks/1_02_scooping-and-crate-carrying.md#delivery-record--september-6-2026); its [latest feedback](tasks/1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026) rejects placement restrictions, without a new scoop enjoyment rating. |
| Foundation checks and builds | [1_01 movement revision](tasks/1_01_unity-foundation-and-walkable-scene.md#movement-revision--september-6-2026), including original delivery history; [current play guide](../../unity/readme.md). |
| Full saves / full game | Not implemented. |

Historical relocation evidence, September 5: 97 Unity asset/source/settings/test files matched their pre-move hashes. All 37 original Markdown documents were relocated; the original bootstrap content was unchanged. Documentation links, encoding, formatting, and ignore rules were checked at that point. These figures describe the relocation before archive cleanup, not the current document inventory.

September 6 documentation update: removed the ten discarded roasting/prototype archive files; retained source research centrally with direct links from the game; added the start guide, supported Unity API/Input System rules, and free commercially usable asset policy. Local document links, heading targets, UTF-8 decoding, and code fences checked successfully. No gameplay source, packages, or project settings were changed, and no Unity or legacy state tests were rerun for this update.

September 6 + current consolidation pass: aligned requirements, scope contracts, and developer workflow text across design and task docs for the next implementation chat. This was documentation-only; queue ordering, task IDs, and delivery states are unchanged.

## Recording future work

The September 6 task mapping adds 31 concrete briefs and the fresh-chat protocol. At mapping time only 0_01 (planning) was Done; 1_01–9_03 were Todo. Task 1_01 was subsequently delivered below. Documentation checks passed for local links, heading targets, task IDs/order, predecessor/next links, required brief sections, and initial statuses. No gameplay, package setup, or Unity/state-test runs were performed for this mapping.

Maintain task state in the [queue](tasks/readme.md#ordered-task-queue) and execution evidence in each task's delivery record. This file tracks aggregate milestones and repository history; update it when that aggregate state or a blocker changes. Follow the queue's review gates and [handoff rules](tasks/readme.md#handoff-and-recording). The [M1 contract](first-playable-task.md) now spans 1_01–1_08.

September 6 implementation: [1_01 delivered the foundation](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026), making M1 in progress. Its delivery record owns package choices, test/build evidence, regression details, and untested human checks.

September 6 process improvement: reduced repeated workflow reading and status copies while preserving feature specifications, task briefs, and review gates. The task queue now routes context by the affected system. Ordinary playtest and development diagnostic builds have separate paths; build maintenance evidence is appended to [1_01](tasks/1_01_unity-foundation-and-walkable-scene.md#process-and-build-follow-up--september-6-2026). No gameplay task or player acceptance advanced.

September 6 scope lock: updated existing design and affected development contracts for English-only draft dialogue, the Finished Food Handoff Rack, a final processor that combines capacity/output gains with a substantially shorter loaded route, a measured pocket count capped at five, and an automatic last-deposit meal. The M1–M2 interaction prototype remains one corner without household/narrative/ending systems. [Scope-lock audit and changed files](../design-pivot.md#scope-lock-audit--september-6-2026) records the decisions and remaining risks. No code/assets/packages/builds or shared research changed; task IDs/order, review gates, delivery statuses, feedback, and historical execution evidence remain unchanged. Documentation links, scope consistency, and preservation checks passed; no Unity or legacy tests were run for this documentation-only pass.

September 6 harvest-completion amendment: the current specs now replace the compulsory meal/day transition with a final-deposit harvest-completion state that leaves normal movement/camera and pause/menu control available. A table/gift/thank-you beat remains optional, cuttable presentation. The discovery-only prototype remains current; a bounded Coins comparison is pending separate authorization after 2_03 and before whole-yard production. This amendment changes no Unity source, serialized data, assets, packages, builds, task IDs/order/status, or historical play evidence. The preceding scope-lock paragraph remains a historical record of the superseded completion clause. Checks covered 54 active Markdown files, 555 local links, 155 heading targets, UTF-8 decoding, balanced code fences, whitespace, 31 unique queue rows in order, and unchanged external URLs. The repository audit points to the bootstrap's current shared AI-setup location. No Unity, build, package, or legacy tests were run for this documentation-only amendment.

September 6 free-handling revision: the developer's placement feedback reopens 1_02 as the next implementation blocker while retaining existing scooping and 1_03 processing. M1 now includes the newly planned 1_06 loose-object sample after the complete-loop checkpoint. The [research decision and documentation checks](../design-pivot.md#free-handling-and-research-review--september-6-2026) record the six local studies, updated behavior/task ownership, and preservation evidence. This changes the plan and records supplied negative feedback; it delivers no new Unity behavior or human acceptance.

September 6 processing-and-inventions pivot: the supplied continuation makes Coins/two useful purchases part of the prototype and replaces required yard discoveries with food preparation and directly operated equipment. New 1_07/1_08 extend M1; 2_01–2_03 and later contracts now evaluate physical operations, equipment choices and winter food. The [decision record](../design-pivot.md#food-machinery-and-coins--september-6-2026) owns this scope change and its documentation verification. Existing technical/human statuses remain unchanged; NEXT is still 1_02. No Unity implementation or player acceptance was produced.

September 6 targeted play-feedback refinement: existing 2_01 gains one complete attachment snap; its comparison, saving and later presentation owners are refined in place. The [decision and check record](../design-pivot.md#snap-installation-and-restrained-comic-variety--september-6-2026) preserves the firsthand source and separates required prototype work from optional later ideas. Milestone readiness and the 1_02 blocker are unchanged; this documentation pass supplies no new Unity delivery or human acceptance.

September 6 finished-batch refinement: the [decision/source/check record](../design-pivot.md#finished-batches-and-visible-accumulation--september-6-2026) strengthens recognizable food output and nearby accumulation within existing tasks. Milestone readiness and NEXT remain unchanged; documentation and reference verification do not supply Unity delivery or player acceptance.

September 6 implementation: the [1_02 free-placement correction](tasks/1_02_scooping-and-crate-carrying.md#free-placement-delivery-record--september-6-2026) removes the earliest technical blocker. M1 remains in progress, the revised build awaits human feedback, and NEXT advances to 1_04 under ordinary technical dependency rules. The task record owns execution evidence.
