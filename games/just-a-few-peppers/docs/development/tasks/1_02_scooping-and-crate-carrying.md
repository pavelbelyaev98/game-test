# 1_02 — Scooping and crate carrying

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Make taking an ordinary load visibly satisfying before adding processing.

**Depends on:** [1_01 — Unity foundation and walkable scene](1_01_unity-foundation-and-walkable-scene.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [Core mechanics](../../core-loop-and-mechanics.md) · [State and saving](../state-and-saving.md) · [Look, sound, and comfort](../../look-sound-and-comfort.md) · [Architecture](../../../ARCHITECTURE.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Add the authoritative finite pile/raw-carrier state and validated gathering commands, using a 12-unit crate available from the start.
- Connect broad hold/toggle scooping to local authored depletion, increasing carried volume, short action audio, and clear full/invalid-target feedback.
- Tune load rhythm so visible scooping work is the dominant loop action. Avoid states where one trip fills instantly and then long walking/servicing consumes most of the load cycle.
- Provide stable carrying/parking and recovery to a safe resting point with the same contents. Existing sprint/jump input remains available while the crate is held; the carrier stays under one authoritative owner through the motion. Yard boundaries, carrier collisions, and recovery prevent jumping or dropping from bypassing access gates or losing units. Decorative pepper motion is bounded and never owns harvest units.

## Acceptance

- Scooping changes the touched pile region immediately; accepted units leave that pocket and enter the crate once, up to capacity.
- Meaningful checks cover full/partial scoops, invalid targets, interruption, reset, and recovery without loss or duplication. Verify full/invalid feedback is rate-limited (one soft cue, no spam while the trigger is held). The scene exposes these actions to the human playtester.

## Human playtest check

Fill and carry the crate, scoop at different parts of the mound, try an overfill, and recover the loaded carrier.

**Outside this task:** An output economy, free pepper physics, wheelbarrow, manual cooking, or arbitrary consumable stamina.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [1_03 — Tipping and automatic processing](1_03_tipping-and-automatic-processing.md). Stop after this task's handoff unless the developer explicitly requested a larger range.


