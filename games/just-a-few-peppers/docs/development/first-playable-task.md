# M1 contract: first complete crate loop

Status: 1_01 foundation delivered; the complete crate loop remains specified and unimplemented. This is the aggregate M1 behavior contract, now split into numbered tasks **1_01–1_05**. For one fresh-chat task, start at [1_01](tasks/1_01_unity-foundation-and-walkable-scene.md) using the [reusable prompt](new-chat-prompt.md). Read the root agent instructions, [architecture](../../ARCHITECTURE.md), [core mechanics](../core-loop-and-mechanics.md), [state rules](state-and-saving.md), [Unity and asset policy](unity-and-assets.md), and [current status](status.md).

## Deliverable

A separate outdoor graybox scene in the existing Unity project: one finite authored mound, a 12-unit crate, one automatic station, one reusable finished-food carrier, and one **Finished Food Handoff Rack**. The starting crate is available immediately. The scene can run repeated whole loads and a partial final load to complete storage, with plain completion feedback.

M1 is the crate-only phase of the [one-corner interaction prototype](../scope-and-validation.md#next-experiment-one-pile-one-carrier-one-discovery). M2 adds the partly exposed wheelbarrow within the same mound and comparable material after unlock. Neither phase includes Grandpa dialogue, household display states, final machine, cellar, parcels, grinder, or an ending scene. Full-game automatic completion/meal presentation comes in M4–M5; prototype completion must not pull those systems forward.

The current Stage0 scene is disposable reference material, not this feature. Task 1_01 supplies `Assets/JustAFewPeppers/Scenes/PepperYard.unity`, its input/movement/pause foundation, and a checked Windows development build; pepper handling begins in 1_02. The agent owns scene/component wiring, controls, assets, materials, and build configuration; Pavel receives a scene or build ready to play.

## Numbered implementation tasks

1. [1_01 — Unity foundation and walkable scene](tasks/1_01_unity-foundation-and-walkable-scene.md): supported input/test setup, new scene, movement, basic pause/focus.
2. [1_02 — Scooping and crate carrying](tasks/1_02_scooping-and-crate-carrying.md): finite pile state, local depletion, 12-unit crate, stable handling and recovery.
3. [1_03 — Tipping and automatic processing](tasks/1_03_tipping-and-automatic-processing.md): partial accepted transfers, one batch, reserved/accumulating output, and feedback.
4. [1_04 — Finished carrier and handoff rack](tasks/1_04_finished-carrier-and-storage-rack.md): collect, park raw carrier, deposit exactly once, automatic empty return.
5. [1_05 — First playable comfort and handoff](tasks/1_05_first-playable-comfort-and-handoff.md): finish comfort/reset/feedback, verify the full M1 contract, and deliver a Windows smoke build.

Each task leaves an inspectable scene and records its own acceptance evidence in the [queue](tasks/readme.md). The earliest tasks deliberately expose only the behavior implemented so far; the whole loop is not required in 1_01.

Use authored pile pockets/depletion and bounded moving visuals first. Use primitives/placeholders or suitable free commercially usable assets; do not delay this task for polished art. A small non-multiple-of-12 harvest is useful for testing the last batch. The exact number is test content, not the full-game manifest.

## Acceptance criteria

- A scoop changes the touched part of the pile immediately and visibly fills the crate.
- One broad tip moves the accepted amount; full, partial, and cancelled actions never lose or duplicate food.
- The line runs automatically, accepts a partial final batch, and safely pauses at full output.
- The sole finished carrier can be collected, deposited once, and reused without an empty-container trip.
- Every starting unit can reach stored progress; an empty yard with unfinished food is not complete.
- Pause/focus loss stops relevant timers; reset returns the entire small scene to its authored initial state.
- Actual targeting, model/view synchronization, and a complete scene run pass integration checks.
- The agent has connected scene references and input actions; opening the supplied scene and pressing Play requires no manual component assembly.
- A packaged player starts and completes the small loop without unexplained runtime errors.
- Gathering and dumping have inspectable visual/audio feedback suitable for the next feel test.

Do not add the wheelbarrow, full yard, final machine, disk saving, household displays, narrative scenes, new recipes, or a factory framework to this task. M2 tests the discovery/upgrade; M3 adds disk persistence.

## Handoff

Report the exact new scene/build path, controls, changed assets, verified test/build commands, observed results, and remaining issues. Give Pavel a short checklist: complete one load, try a partial final load, judge scoop/tip feedback, and try pause/reset. Keep failed or untested criteria visible. Update [status](status.md), [testing](testing-and-performance.md), and any contract changed by the implementation.

For a numbered-task request, stop after that task's handoff. Task 1_05 closes this aggregate M1 contract. Mark technical readiness separately from Pavel's pending feedback. Do not call the game fun or mark later milestones complete because M1 works. Run the checks relevant to the new loop; repeated Stage0 state/probe/build runs are not part of this milestone.
