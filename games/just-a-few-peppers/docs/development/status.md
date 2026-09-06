# Implementation status

Updated September 6, 2026. This file summarizes milestone delivery and evidence. The [numbered queue](tasks/readme.md) is the source of truth for individual tasks and their feedback; [roadmap](roadmap.md) explains milestones. Use the [fresh-chat prompt](new-chat-prompt.md) to continue chronologically.

## Current milestones

- [x] M0 — Repository organization and planning: grouped files, development contracts, relocation verification, documentation checks, and legacy state-test rerun completed. Evidence below.
- [ ] M1 — First complete crate loop, tasks 1_01–1_05: Todo. [Next task: 1_01](tasks/1_01_unity-foundation-and-walkable-scene.md).
- [ ] M2 — Wheelbarrow discovery and fun gate: Todo.
- [ ] M3 — V4 saving, resume, and recovery: Todo.
- [ ] M4 — Complete graybox yard and final station: Todo.
- [ ] M5 — Representative vertical slice: Todo.
- [ ] M6 — Content and balance: Todo.
- [ ] M7 — Shipped menus, controls, and comfort: Todo.
- [ ] M8 — Regression/performance hardening: Todo.
- [ ] M9 — Shipping preparation and final artifact checks: Todo.

No v4 gameplay feature is marked implemented. A source file, a planned contract, or a historical test pass does not complete a milestone.

## Existing implementation and evidence

| Item | Status / evidence |
| --- | --- |
| Historical one-pepper scene | Exists under `unity/Assets/Stage0/`; different roast/steam/peel loop. |
| Legacy deterministic rules | 10/10 standalone state tests passed from the relocated project on September 5, 2026. |
| Legacy scene/build/feel | Discarded experiment; old reports removed. Further Stage0 acceptance is not a v4 gate. No Unity scene/build checks were rerun for the documentation cleanup. |
| V4 pile representation and fun | Untested. |
| V4 Unity EditMode/PlayMode tests | Not installed or implemented. |
| V4 saves, new scene, full game build | Not implemented. |

Historical relocation evidence, September 5: 97 Unity asset/source/settings/test files matched their pre-move hashes. All 37 original Markdown documents were relocated; the original bootstrap content was unchanged. Documentation links, encoding, formatting, and ignore rules were checked at that point. These figures describe the relocation before archive cleanup, not the current document inventory.

September 6 documentation update: removed the ten discarded roasting/prototype archive files; retained source research centrally with direct links from the game; added the start guide, supported Unity API/Input System rules, and free commercially usable asset policy. Local document links, heading targets, UTF-8 decoding, and code fences checked successfully. No gameplay source, packages, or project settings were changed, and no Unity or legacy state tests were rerun for this update.

## Recording future work

The September 6 task mapping adds 31 concrete briefs and the fresh-chat protocol. Only 0_01 (planning) is Done; 1_01–9_03 remain Todo. Documentation checks passed for local links, heading targets, task IDs/order, predecessor/next links, required brief sections, and initial statuses. No gameplay, package setup, or Unity/state-test runs were performed for this mapping.

Maintain per-task delivery and feedback in the [queue](tasks/readme.md#ordered-task-queue), with dated evidence in the task's delivery record. Use this file for milestone summaries. Ready for Pavel means an integrated result with relevant technical checks recorded; it does not imply player acceptance. Follow the queue's rules for continuing after ordinary handoffs and resolving explicit review gates. Mark a milestone complete only when its exit gate is met, including required play evidence. Leave Partial milestones unchecked.

M1 now: **implementation Todo; Pavel's playtest Not tested**. A handoff must identify its scene/build, controls, relevant test results, a short play checklist, and known limitations. Do not send Pavel a list of routine Inspector wiring tasks.

Record actual elapsed work and playtest observations where useful. Do not invent test counts, estimated campaign duration, performance, or results for the older comparison scorecard.

First implementation instruction: **Implement 1_01 using the numbered queue and fresh-chat prompt.** Later chats request NEXT or a specific eligible ID; finish one task at a time. The [M1 contract](first-playable-task.md) applies across 1_01–1_05, not to the foundation task alone.
