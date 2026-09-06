# 1_06 — Loose yard objects and playful handling

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Let the player arrange and play with a small set of loose yard objects using the established handling controls, then return naturally to pepper work.

**Depends on:** [1_05 — First playable comfort and handoff](1_05_first-playable-comfort-and-handoff.md), including the completed [1_02 free-placement revision](1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026) and [1_04 finished carrier](1_04_finished-carrier-and-storage-rack.md). Follow [AGENTS.md](../../../../../AGENTS.md) and the queue's earlier-gate rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), each dependency's delivery/feedback record, the [free handling contract](../../core-loop-and-mechanics.md#pick-up-place-and-play), [research decisions](../../design-pivot.md#free-handling-and-research-review--september-6-2026), [look/comfort](../../look-sound-and-comfort.md), [state/saving](../state-and-saving.md), [architecture](../../../ARCHITECTURE.md), and relevant [asset](../unity-and-assets.md) / [verification](../testing-and-performance.md#free-handling-coverage) rules. Inspect the current Unity scene, shared handling/input, and pinned versions before implementation.

## Work

- Extend existing grab/rotate/place/drop behavior to a representative loose basin, stool, empty crate, and ball in the same prototype corner. These are a compact test sample, not a permanent one-toy cap or a full household art pass. Use primitives or suitable free assets with recorded licenses.
- Give them appropriate collision/gravity, stable placement and stacking where shape permits, and deliberate small-prop tossing. Keep careful placement easy; throwing must not be the only release method. Share controls/target feedback with carriers and resolve overlapping targets clearly.
- Let objects remain where placed and be moved again, including temporary route clutter. Add recoverable identity/pose state and lost-object recovery without resetting food or the player's valid arrangements. Freeze physical motion/input during pause/focus loss. Disk persistence remains M3.
- Provide a little useful free space and a work surface for arranging things. Let a loose object be uncovered through ordinary pile clearing where convenient. No objectives, score, mandatory trick, prop collection, or waiting timer is needed; play remains optional while useful work can continue.
- Own scene/prefab wiring, input, interaction feedback, build configuration, and a focused ordinary Windows player handoff. Record initial physical-body count and observed frame behavior; reuse/sleep bodies instead of tying simulation cost to harvest size.

## Acceptance

- Pick up, rotate, carefully place, stack, drop and toss the sample objects through ordinary input. Comparable loose props give consistent grab feedback; installed fixtures do not promise a grab action.
- Objects collide and settle without routine tunnelling, explosive stacks, held-object player launch, or duplicated identities. A moving object freezes across pause/focus and resumes safely. Recover an out-of-bounds/stuck object and move a blocking prop out of a route.
- A loaded raw or finished carrier can be set down among these objects, recovered/regrabbed, and used to finish its food transfer. Prop play neither changes harvest accounting nor becomes a completion requirement.
- Relevant scene/physics integration checks and the packaged player cover these cases. Record unsupported cases and human feel feedback honestly; screenshots or compilation alone cannot establish comfortable handling.

## Human playtest check

1. Make a small arrangement on the worktop; rotate/place objects neatly and try a stable stack.
2. Drop and toss a small prop, play with the ball, then clear any clutter from your route.
3. Leave a loaded carrier nearby, play briefly while processing runs, and return to collect/deposit food.
4. Pause during a fall, resume, and recover a deliberately inaccessible object without resetting the work.

Ask what felt naturally movable, what refused unexpectedly, and whether arranging objects was enjoyable. No extra reward is needed for the check.

**Outside this task:** Individual pepper inventories, simulated glass breakage, moving installed machinery, furniture construction, household quests, achievements, a prop under every pile, full-yard art, or disk saves.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Record the exact scene/build, bindings, focused checklist, checks, limitations, and real feedback; update the queue and M1 summary without inventing acceptance. Stop after the handoff.

Next in order: [1_07 — Physical pepper batch comparison](1_07_physical-pepper-batch-comparison.md).

## Planning record — September 6, 2026

Added by the developer's request for fewer handling restrictions and ideas from `research/case-studies`. This is the one new follow-up for loose-object play; the existing 1_02 brief owns the crate correction. Technical delivery is **Todo**, feedback **Not tested**. No Unity edits, imported assets, build, or gameplay verification were produced by this planning pass.
