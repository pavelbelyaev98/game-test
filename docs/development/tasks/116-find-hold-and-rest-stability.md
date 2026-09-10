# Task 116 - Stable held and dropped finds

Type: implementation. Status: `done`. User-selected bug fix following `115`. [Completion and evidence](../completed/116-find-hold-and-rest-stability.md).

Feature: [collection and physics](../../features/backlog/discovery-collection.md). Prerequisite: `115`. Context: [handling contract](115-find-handling-and-recognition.md), [player feel](101-collection-return-playtest.md), [physics risks](../design-risks.md).

## Outcome

- Fix persistent rock jitter after RMB drop and unstable handling while looking up/down.
- Keep the held object in hand during normal movement and sustained maximum-speed jetpack flight; carry tracking must not depend on item throw strength.
- Preserve real terrain/prop collision, natural drop/throw and waking after terrain support is removed. Never solve resting jitter by anchoring unsupported objects or disabling world collisions.
- Preserve the confirmed RMB lift/drop, fresh LMB throw, 60% exposure and 0.6-second recognition behavior, per-item throw strength, inventory identity, pause and save semantics.

## Acceptance

- Regressions exercise all three rock appearances, repeated pitch changes and dropping onto terrain; settled position and rotation stay stable over an observation interval.
- Sustained actual jetpack movement and look changes keep the same object held without unexpected release; contact with ground/walls remains recoverable.
- Ground excavation wakes a resting body; thrown/airborne objects remain physical. Relevant existing physics and save checks pass.
- Inspect the cases in MainGame through the official Unity CLI, record evidence and provide an updated Windows build. User feel remains subject to retest in `101`.

No unresolved product decision: this repairs the previously selected handling behavior. Existing approved models are retained.

Technical context: the previous rock follow cap was 6 m/s versus 8 m/s jetpack ascent; the failing flight regression records the growing hand gap. One seeded Rock B retained about 5 mm / 1.5 degrees of rocking after six seconds. Unity documents [speculative contacts](https://docs.unity3d.com/6000.0/Documentation/Manual/speculative-ccd.html) as potentially inaccurate. Low-speed discrete contact solving removes this persistent case; speed hysteresis restores CCD for fast translation/rotation and holding. Use [physical sleep](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.Sleep.html) only after sustained quiet motion on actual support, with terrain changes waking the body.
