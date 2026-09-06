# 8_03 — Release candidate rehearsal

Milestone: M8 · Type: Play gate · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Freeze a candidate whose complete player flow can be reproduced and assessed.

**Depends on:** [8_02 — Performance and build cleanup](8_02_performance-and-build-cleanup.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Testing and performance](../testing-and-performance.md) · [Roadmap](../roadmap.md) · [State and saving](../state-and-saving.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Rehearse earning, choosing either useful offer, paying once, repeatedly using its effect and resuming paid installation. Verify stored food remains separate from spendable Coins in the exact candidate.

- Create a versioned Windows candidate from the intended scenes/settings and record its artifact path and configuration.
- Use an isolated clean user-data location to rehearse install/start, new game, save/exit/continue, options, full completion, continued yard control, and relaunch into the completed property.
- Collect the tester's final full-run feedback and resolve blockers before calling the candidate ready. Rebuild/recheck affected behavior only when fixes change the artifact.

## Acceptance

- The exact recorded candidate completes its player flow with existing reliability/performance evidence and no unresolved release blocker.
- the tester's candidate feedback is recorded; a build alone does not imply he accepted it. Unknown compatibility stays explicit.

This is a review gate. Prepare the playable/reviewable artifact first; keep missing human evidence pending and do not silently advance beyond the gate.

## Human playtest check

Play the candidate as a new player, close and continue midway, finish, then relaunch once.

**Outside this task:** Public release, store uploads, new content, or retesting every past artifact after an unrelated document edit.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [9_01 — Product identity and credits](9_01_product-identity-and-credits.md). Stop after this task's handoff unless the developer explicitly requested a larger range.
