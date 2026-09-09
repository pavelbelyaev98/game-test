# Task 54 - Full-game performance, comfort and Windows release qualification

Type: validation. Status: `planned`. Prerequisites: 22, 37, 53, 63, 65, 67, 69, 78, 80, 82; selected 83/85 delivery must also be complete.

Feature: [release validation](../../features/backlog/release-validation.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Profile actual early/late tools, dense discovery fields, mixed materials, long excavations, saved-hole restoration and Continue. Compare edit/collision spikes with Task `34` evidence; fix game-owned bottlenecks rather than merely compiling or quoting average frame rate.
- Test cold launch, pause/focus, resize/readability, controls, click-and-hold digging, FOV/steady-reticle settings and all implemented low-motion controls through late equipment/Continue, safe exit, periodic/transaction interrupted-save recovery and native relaunch on agreed Windows targets. Full keyboard/mouse rebinding and optional toggle digging from `78`, brightness/reticle options from `82` and detector-only volume/mute are required cases. Controller support and automatic hardware benchmarking stay uncommitted; do not infer platform support from one review.
- Review approved digging/motor/jetpack/detector/station sounds across a 2–3 hour run for harsh repetition, layering and intelligibility. Preserve quiet gaps, restrained error feedback, no music/voice acting and the user's asset approval rules.
- Recheck [precision crouch](../../features/backlog/precision-movement.md), including ground/air steering, low-ceiling release, stance transitions and saved clearance through Continue; also passive visibility at empty battery, persistent bag/battery readings and unambiguous saving/transactions. Validate any separately selected aid/explosive/story additions under their recorded contracts; a research task's completion is not evidence they exist.
- Validate final tool/terrain/discovery feedback, readable boundaries and sustained recognition, plus actual player traversal through extensively edited vertical and lateral routes. Existing cleanup remains enabled for every included excavation source.
- Run the full game-owned acceptance set after fixes, verify `22` admin exclusion again in the final non-development build, and deliver `builds/windows/SomethingDownThere.exe`. No extra launchers, primitive final content or fixture adapters in the release.

## Before implementation

- Research: use real profiles and official Unity guidance for any diagnosed engine interaction; compare observed sound/input/return fatigue with `15`/`37` evidence. Do not add speculative dependencies or mechanics.
- Qualify against the Windows support matrix and frame-time/load/save/memory budgets accepted in [63](63-windows-targets-and-budgets.md). Bring back only evidence-backed changes to those targets or remaining presentation scope; actual additional assets/audio still need explicit approval.
- Fixes within this qualification scope are part of the task. A new feature or widened platform promise requires its own selected scope, rather than hiding it in polish.

## Native matrix from Task 63

Task `63` selected the [release feature's targets](../../features/backlog/release-validation.md) under user-delegated technical judgment. Perform this full matrix on the finished game; publish hardware requirements only after measurement, adjusting test candidates if evidence warrants. Record executable hash/build mode, OS/driver, CPU/GPU/RAM/storage, client resolution/render settings, seed/save/tool and sample counts. Isolate validation saves from the user's profile. Profile without Deep Profiling and qualify final release builds; development captures explain bottlenecks but cannot certify release performance.

| Coverage | Required native cases |
|---|---|
| Minimum hardware | Actual Intel/NVIDIA and AMD/AMD candidate systems at 1080p; SATA SSD on at least one. Each passes an ordinary 3-hour run, late-save load and 1-hour Continue/excavation stress. Check all available tool extremes and dense finds; no developer overrides in the qualification run. |
| OS / graphics API | Spread oldest/newest selected serviced Windows 11 versions across the baseline systems; launch, dig, save/reload and focus smoke tests on both DX12/DX11 per GPU family. Windows 10 22H2 remains deferred; any later support decision needs its own measured compatibility row. Record each tested combination and uncovered combinations. |
| Recommended / reference | At least one recommended candidate completes the same frame/load/memory cases at 1440p. Current development PC supplies reproducible regression traces, not a replacement minimum row. |
| Terrain and repetition | Three saved route shapes: deep/vertical, mixed lateral chambers with supported steps, broad open pit. Measure ≥10 minutes per early/late tool case, including accepted cuts, near-boundary cleanup, high exposed surface area/dense finds and overlapping autosaves. A heavily cleared validation save tests voluntary excavation without making it a player completion chore. |
| Launch / memory | Three cold launches after separate Windows restarts plus five warm launches per hardware tier, on fresh and most expensive late saves; clock process start to usable controls or enabled Resume. Report every latency, not just average. Run ten same-save load/return cycles; compare idle memory after cycles 5/10, investigate retained growth >100 MiB and require no monotonically leaking meshes/colliders/buffers. |
| Window / comfort | Every supported client size/aspect plus desktop scaling extremes; low/default/high `64`/`65` FOV in tight passages and shafts, reticle/center-ray agreement, menu readability and preferences across relaunch/rescue/Continue. One full run uses steady reticle and every implemented camera effect at zero/off; preserve normal tool power and precision movement. |
| Input / focus | Pause, Alt-Tab, minimize, 60-second background wait, Windows sleep/resume, resize and device disconnect/reconnect; test LMB/Space held while leaving/returning. No hidden digging/thrust/menu leakage, no progress simulation while paused, no stuck cursor; release and fresh input resume correctly. Steam overlay repeats focus cases once `53` exists. |
| Persistence | Interrupt dirty digging at 1/9/10 seconds and during capture/write/replacement; force stop after sale/purchase/rescue before/after durable completion. Require one complete consistent snapshot, no duplicates/mixed terrain/economy, measured ≤11-second healthy loss and ≤1-second transaction durability. A crash before durability may recover the prior whole checkpoint; normal quit must retain latest state. |
| Failure / sound | Isolated profiles test full/unwritable storage, damaged primary with valid previous save, unsupported data and retry. Timing guarantees apply to healthy storage; errors never silently reset. Review approved audio over the full run for harsh repetition, clipping/layer masking and interrupted/reconnected output; passive light and HUD remain usable at zero battery. |

Report mean/p95/p99/max frame/edit/capture timings and peak memory per case against the feature, with actual worst dig/save frames retained. Pending content/settings/platform cases stay visibly untested until their owners deliver. Any game-owned budget or correctness failure prevents qualification; resolve it and rerun the affected case before delivery.

## Acceptance

- Representative full runs meet the agreed performance/comfort targets, launch and recover safely, and finish/continue on the same save with useful remaining gameplay.
- Record concise measured evidence, checks, final native build review and remaining limitations. Do not mark done while known game-owned release blockers or required acceptance failures remain.

## Added research coverage

Repeat `80`'s incremental capture/write correctness and whole-frame budgets during late sustained digging and trades; retain worst save-overlap frames. Exercise remapped hold/toggle through every focus/lifecycle case, visual settings in dark/bright regions and muted detector play. Qualify selected inspection/completion additions without turning optional research into a release requirement.
