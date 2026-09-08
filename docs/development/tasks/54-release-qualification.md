# Task 54 - Full-game performance, comfort and Windows release qualification

Type: validation. Status: `planned`. Prerequisites: 22, 37, 53, 63.

Feature: [release validation](../../features/backlog/release-validation.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Profile actual early/late tools, dense discovery fields, mixed materials, long excavations, saved-hole restoration and Continue. Compare edit/collision spikes with Task `34` evidence; fix game-owned bottlenecks rather than merely compiling or quoting average frame rate.
- Test cold launch, pause/focus, resize/readability, controls, click-and-hold digging, safe exit, interrupted-save recovery and native relaunch on agreed Windows targets. Controller drift/rebinding and automatic hardware benchmarking stay uncommitted unless explicitly selected; do not infer platform support from one review.
- Review approved digging/motor/jetpack/detector/station sounds across a 2–3 hour run for harsh repetition, layering and intelligibility. Preserve quiet gaps, restrained error feedback, no music/voice acting and the user's asset approval rules.
- Validate final tool/terrain/discovery feedback, readable boundaries and sustained recognition, plus actual player traversal through extensively edited vertical and lateral routes. Existing cleanup remains enabled for every included excavation source.
- Run the full game-owned acceptance set after fixes, verify `22` admin exclusion again in the final non-development build, and deliver `builds/windows/SomethingDownThere.exe`. No extra launchers, primitive final content or fixture adapters in the release.

## Before implementation

- Research: use real profiles and official Unity guidance for any diagnosed engine interaction; compare observed sound/input/return fatigue with `15`/`37` evidence. Do not add speculative dependencies or mechanics.
- Qualify against the Windows support matrix and frame-time/load/save/memory budgets accepted in [63](63-windows-targets-and-budgets.md). Bring back only evidence-backed changes to those targets or remaining presentation scope; actual additional assets/audio still need explicit approval.
- Fixes within this qualification scope are part of the task. A new feature or widened platform promise requires its own selected scope, rather than hiding it in polish.

## Acceptance

- Representative full runs meet the agreed performance/comfort targets, launch and recover safely, and finish/continue on the same save with useful remaining gameplay.
- Record concise measured evidence, checks, final native build review and remaining limitations. Do not mark done while known game-owned release blockers or required acceptance failures remain.
