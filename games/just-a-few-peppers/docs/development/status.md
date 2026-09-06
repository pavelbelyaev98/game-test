# Implementation status

Updated September 6, 2026. This file summarizes milestone delivery and evidence. The [numbered queue](tasks/readme.md) is the source of truth for individual tasks and their feedback; [roadmap](roadmap.md) explains milestones. Use the [fresh-chat prompt](new-chat-prompt.md) to continue chronologically.

## Current milestones

- [x] M0 — Repository organization and planning: grouped files, development contracts, relocation verification, documentation checks, and legacy state-test rerun completed. Evidence below.
- [ ] M1 — First complete crate loop, tasks 1_01–1_05: In progress. [1_01 foundation](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026) is Ready for Pavel; feedback Not tested. [Next task: 1_02](tasks/1_02_scooping-and-crate-carrying.md).
- [ ] M2 — Wheelbarrow discovery and fun gate: Todo.
- [ ] M3 — V4 saving, resume, and recovery: Todo.
- [ ] M4 — Complete graybox yard and final station: Todo.
- [ ] M5 — Representative vertical slice: Todo.
- [ ] M6 — Content and balance: Todo.
- [ ] M7 — Shipped menus, controls, and comfort: Todo.
- [ ] M8 — Regression/performance hardening: Todo.
- [ ] M9 — Shipping preparation and final artifact checks: Todo.

The v4 walkable foundation is technically delivered; bulk handling and the complete M1 loop remain unfinished. Foundation verification does not complete a milestone or establish enjoyable controls.

## Existing implementation and evidence

| Item | Status / evidence |
| --- | --- |
| Historical one-pepper scene | Exists under `unity/Assets/Stage0/`; different roast/steam/peel loop. |
| Legacy deterministic rules | 10/10 standalone state tests passed from the relocated project on September 5, 2026. |
| Legacy scene/build/feel | Discarded experiment; old reports removed. Further Stage0 acceptance is not a v4 gate. No Unity scene/build checks were rerun for the documentation cleanup. |
| V4 pile representation and fun | Untested. |
| V4 Unity EditMode/PlayMode tests | Installed in 1_01: final EditMode 2/2 and PlayMode 5/5 pass; scene/input/targeting/pause/recovery coverage. |
| V4 new scene / foundation build | `Assets/JustAFewPeppers/Scenes/PepperYard.unity`; Windows development player `Builds/JustAFewPeppers/JustAFewPeppers.exe`. Packaged smoke passes; rendered captures inspected. |
| V4 saves / full game | Not implemented. |

Historical relocation evidence, September 5: 97 Unity asset/source/settings/test files matched their pre-move hashes. All 37 original Markdown documents were relocated; the original bootstrap content was unchanged. Documentation links, encoding, formatting, and ignore rules were checked at that point. These figures describe the relocation before archive cleanup, not the current document inventory.

September 6 documentation update: removed the ten discarded roasting/prototype archive files; retained source research centrally with direct links from the game; added the start guide, supported Unity API/Input System rules, and free commercially usable asset policy. Local document links, heading targets, UTF-8 decoding, and code fences checked successfully. No gameplay source, packages, or project settings were changed, and no Unity or legacy state tests were rerun for this update.

## Recording future work

The September 6 task mapping adds 31 concrete briefs and the fresh-chat protocol. At mapping time only 0_01 (planning) was Done; 1_01–9_03 were Todo. Task 1_01 was subsequently delivered below. Documentation checks passed for local links, heading targets, task IDs/order, predecessor/next links, required brief sections, and initial statuses. No gameplay, package setup, or Unity/state-test runs were performed for this mapping.

Maintain per-task delivery and feedback in the [queue](tasks/readme.md#ordered-task-queue), with dated evidence in the task's delivery record. Use this file for milestone summaries. Ready for Pavel means an integrated result with relevant technical checks recorded; it does not imply player acceptance. Follow the queue's rules for continuing after ordinary handoffs and resolving explicit review gates. Mark a milestone complete only when its exit gate is met, including required play evidence. Leave Partial milestones unchecked.

M1 now: **foundation delivered; complete crate loop unfinished; Pavel's playtest Not tested**. A handoff must identify its scene/build, controls, relevant test results, a short play checklist, and known limitations. Do not send Pavel a list of routine Inspector wiring tasks.

Record actual elapsed work and playtest observations where useful. Do not invent test counts, estimated campaign duration, performance, or results for the older comparison scorecard.

Next implementation instruction: **Implement 1_02 using the numbered queue and fresh-chat prompt.** Later chats request NEXT or a specific eligible ID; finish one task at a time. The [M1 contract](first-playable-task.md) applies across 1_01–1_05, not to the foundation task alone.

September 6 implementation delivery: 1_01 supplies the saved PepperYard scene, 10 integrated placeholder materials, input asset, camera/controller/targeting, pause UI, focus handling, and safe-spawn recovery. Editor remains 6000.6.0f1; installed packages are Input System 1.20.0, Test Framework 1.8.0, and uGUI 2.6.0. Final checks: EditMode **2/2**, PlayMode **5/5**, successful Windows development build and packaged smoke; final scene/menu captures inspected. The controller's minimum-movement cutoff was fixed after a high-frame-rate test failure. No new C# warnings or game exceptions remain. Startup license/debug-interface notices and verification limits are explained in the [delivery record](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026). Physical window focus, camera comfort, and Pavel's play feedback remain untested; no fun or complete-loop acceptance is claimed.
