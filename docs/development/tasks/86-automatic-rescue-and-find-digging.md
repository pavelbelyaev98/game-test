# Task 86 — Automatic rescue and digging around small finds

Type: implementation. Status: `done`. Prerequisites: `14`, `30`, `35`, `72`, `79` (complete). User-selected fixes. [Completion](../completed/86-automatic-rescue-and-find-digging.md).

Features: [return and rescue](../../features/backlog/return-rescue.md), [discovery collection](../../features/backlog/discovery-collection.md).

## Scope and decisions

- User request: rescue automatically when shared digging/jetpack fuel reaches zero; remove the Pause action and confirmation. Retain the existing ordinary-find loss, balance-clamped fee, safe surface return, refill and checkpoint.
- User request: holding dig on a visible, insufficiently uncovered small find clears nearby covering ground until ordinary collection becomes eligible. Keep the 40% threshold, actual visibility, shovel reach/cadence/energy, pickup recovery and full-bag rules. Large finds retain deliberate surrounding excavation.
- Research/context: inspect existing rescue transaction and save integration, held collection from `30`/`72`, terrain hit validation and release-after-menu/focus behavior. No unresolved product questions for these bounded fixes.

## Acceptance

- Final digging/jetpack fuel automatically rescues once, preserving excavation/upgrades/collected identities and checkpointing consequences; broke/empty-bag recovery works.
- Loading an empty-battery checkpoint recovers after play resumes. Menus/focus/persistence barriers remain safe; return resets motion and suppresses held digging/thrust. Blocked landing never charges or discards items and can retry.
- Pause has no rescue action or confirmation. Existing settings/admin/save navigation works.
- A continuous aim/hold on a small find progresses from covering soil through exposure to exactly one pickup and resumes digging. Assisted strokes change actual nearby terrain with normal energy/cadence; no large-find, range, occlusion or capacity bypass.
- Fast compile check, relevant EditMode/PlayMode regressions, official CLI MainGame inspection and Windows build at `builds/windows/SomethingDownThere.exe`.

Evidence: 106/106 EditMode; 98 PlayMode cases covered by 96 passes in the full run plus corrected 6/6 save and 3/3 recharge retests. CLI review showed 13.5% → 40.6% exposure in three aimed strokes, free single pickup and automatic rescue with preserved terrain. Build succeeded at `2026-09-09 14:34 UTC`, zero errors. Evidence: `unity/Logs/Task86/`.
