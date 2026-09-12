# Task 131 - Responsive collection and pickup motion

Type: implementation. Status: `done`. [Completion/evidence](../completed/131-responsive-find-pickup.md). Prerequisites: 115/116 delivered. Explicitly selected by the user; 130's reservoir batch remains pending approval.

Feature: [discovery/collection](../../features/backlog/discovery-collection.md). Context: [115](115-find-handling-and-recognition.md), [116](116-find-hold-and-rest-stability.md), [collection playtest](101-collection-return-playtest.md).

## Selected scope

- Fix the direct-aim revealing stroke: when Dig starts on a visible but ineligible bottle/rock and that successful stroke makes the same item collectible, collect it immediately after rechecking direct visibility, 60% exposure, 3 m reach, inventory and gameplay state. This explicitly supersedes 115's extra post-stroke recognition wait for that already-aimed item. Soil-only strokes do not scoop up newly revealed/off-aim neighbors; ordinary held aim retains its existing recognition behavior.
- Animate successful collection with the existing approved mesh/material: briefly shrink and draw a nonphysical visual copy toward the player. Commit inventory and collected absence immediately; animation owns no identity, collider, physics or save state. Freeze on pause and clear on restore/return/player shutdown.
- Walking over a fully uncovered, nearby floor item collects it without looking or holding Dig. Require actual foot-level overlap, unobstructed contact path, no remaining soil attachment (allow ordinary contact tolerance), normal active grounded movement and inventory space. No pickup through floors/walls, while held, while flying, from partly buried finds, or during pause/loading/focus loss.
- Explicitly dropped/thrown items stay available for handling until the player leaves their immediate vicinity and returns. Full bags retain finds and avoid repeated feedback spam. Preserve one-identity transfers, per-item values, saves, shovel costs/reach/cadence and physical handling.

## Acceptance

- Reproduce a partly uncovered directly aimed item crossing the threshold on one stroke, including fresh press and continued hold/toggle; collect the same item once with one stroke's fuel cost. Failed digs, out-of-reach/full-bag cases and off-aim finds remain correct.
- Walk pickup covers all current bottle/rock variants; reject partial exposure, obstruction, elevated/below-floor targets, pause/focus and held/recent-release cases. Space recovery and later re-entry work without duplicate pickup.
- Inspect the shrinking/travelling model in MainGame through official Unity CLI; validate collision-free visual copies, cleanup and committed identity across capture/restore during animation.
- Run relevant deterministic/PlayMode/input/save checks, deliver Windows build, update current contracts/status and concise completion evidence. User feel remains a playtest verdict in 101.

## Current result

- Implemented same-stroke direct-aim collection, an eight-copy bounded pool of 0.42 s pickup visuals, and grounded walk-over collection with soil/contact/obstruction checks. Explicit release requires leaving the item vicinity before automatic walking pickup can resume.
- Direct aim, all six walk-over appearances, full bags, obstruction/focus, physical handling, weak/strong held/toggle input and save/reload during an active pickup effect pass. MainGame camera review shows a 48% uncovered bottle collecting on one revealing stroke, then shrinking and sweeping toward the player. [Pickup validation record](../completed/131-responsive-find-pickup.md). Windows build **2026-09-12 16:51 UTC** succeeds with zero errors and one existing Pipeline warning; seven-second native startup is clean. Shallow-find distribution remains with 126.
