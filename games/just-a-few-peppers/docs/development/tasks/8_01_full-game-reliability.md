# 8_01 — Full game reliability

Milestone: M8 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Resolve progress and save blockers across the complete content and UI.

**Depends on:** [7_03 — Audio display and guidance](7_03_audio-display-and-guidance.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Testing and performance](../testing-and-performance.md) · [State and saving](../state-and-saving.md) · [Scope and validation](../../scope-and-validation.md) · [Household and ending](../../household-readiness-and-parcels.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Review existing evidence and known bugs, then exercise missing full-game cases: partial/cancelled transfers, carrier recovery, output away/full, pending upgrades, alternate routes, completed saves, and settings transitions.
- Include restoration before/after the final intake switches, the same handoff rack throughout, and harvest completion on the last partial deposit. Check pause/focus and reload with normal completed-yard control, no required transition or Finish command, no repeated gift/feedback reward, and no extra household inventory.
- Check primary/backup write failure and invalid content against isolated fixtures. Add regression coverage for significant observed failures, not tests mirroring private methods.
- Repair the integrated behavior and update evidence/limitations. Do not count old Stage0 tests as current coverage or rerun unrelated suites merely to increase counts.

## Acceptance

- No known completion blocker, duplicate food, corrupting save path, or unexplained runtime error remains in the covered full-game flows.
- The regression record identifies actual failures, fixes, and useful preventing checks; any untested environment/case is explicit.

## Human playtest check

Use a stable full-game build for a normal run; the AI supplies only focused additional steps for player-visible fixes.

**Outside this task:** New features, a testing-framework rewrite, invented results, and destructive checks on personal progress.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [8_02 — Performance and build cleanup](8_02_performance-and-build-cleanup.md). Stop after this task's handoff unless the developer explicitly requested a larger range.



