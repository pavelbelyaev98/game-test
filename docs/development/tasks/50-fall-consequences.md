# Task 50 - Fall consequences without punishing mobility upgrades

Type: implementation. Status: `planned`. Prerequisites: 49, 47, 59.

Feature: [return rescue](../../features/backlog/return-rescue.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Retain harmless minor falls and existing manual rescue. Implement agreed meaningful large-fall consequences without a health-management system, escalating ordinary wall/ceiling bump damage or making purchased jetpack speed a liability.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect actual landing/jetpack behaviour and research reliable landing classification in the current controller. Compare identical narrow/multi-level return routes at every paid jetpack tier.
- Implement the consequence/confirmation policy accepted in [59](59-return-and-fall-design.md), and validate starting thresholds against real landings. Any proposed automatic rescue fee or changed loss rule requires a specific new decision; technical landing classification stays within this task.
- Acceptance: a landing applies its consequence once, ordinary bumps/small drops remain harmless, and stronger mobility remains controllable/desirable. Zero battery, pause/focus, rescue, ceilings/slopes and save/relaunch cannot create penalty loops or a softlock; preserve excavation and all protected progression. Inspect native traversal and deliver the build.
