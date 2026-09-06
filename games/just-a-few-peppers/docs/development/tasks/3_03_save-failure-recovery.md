# 3_03 — Save failure recovery

Milestone: M3 · Type: Milestone handoff · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make damaged or unwritable saves understandable and recoverable.

**Depends on:** [3_02 — Local save and continue](3_02_local-save-and-continue.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [State and saving](../state-and-saving.md) · [Testing and performance](../testing-and-performance.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Add clear handling for missing saves, unreadable/corrupt files, unsupported versions, missing/duplicate content IDs, and invalid quantities.
- Offer a valid backup or an explicit new-game choice without silently erasing the unreadable file. Add retryable save-error feedback while leaving gameplay usable.
- Use isolated disposable save fixtures to exercise interrupted writes and denied writes; retain useful regression tests and record exact save locations and recovery behavior.

## Acceptance

- Corrupt budget/offer/mechanism fixtures reject invalid or duplicate ownership without awarding Coins or charging again; backup recovery preserves valid food, purchases and physical arrangements.
- Add duplicate-kit, contradictory paid/mounted/installed and lost/blocked kit-pose fixtures. Restore a valid backup's entitlement, recover one usable attachment when unmounted, retain accepted pending fitting, and never create a loose copy of an installed upgrade. Verify active food and both purchase orders survive the recovery boundary.

- The previous valid save survives each simulated write failure, and invalid load data never becomes a silently successful fresh game.
- Primary and backup recovery work in the actual scene/build, with no duplicate deposit or lost pending upgrade. M3 round-trip/recovery evidence is recorded.
- Isolated bad-pose fixtures cover non-finite/out-of-bounds positions, blocked recovery positions and conflicting stacked placements. Restore valid saved arrangements and recover only affected objects with the same identity/contents; never reset all props or create extra harvest to hide invalid data.

## Human playtest check

Try a supplied isolated corrupt-save scenario, recover its backup, and verify the normal personal save is unaffected.

**Outside this task:** Destructive tests on the tester's real progress, a generalized migration framework, and testing unrelated legacy code.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [4_01 — Connected graybox yard](4_01_connected-graybox-yard.md). Stop after this task's handoff unless the developer explicitly requested a larger range.
