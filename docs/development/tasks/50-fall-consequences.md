# Task 50 - Fall consequences and balanced rescue

Type: implementation. Status: `planned`. Prerequisites: 49, 47, 59.

Feature: [return rescue](../../features/backlog/return-rescue.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Retain harmless minor falls and the working manual-rescue confirmation/clearance flow. Extend the existing rescue fee only to the policy explicitly selected in `59`; `14` remains complete. Implement agreed meaningful large-fall consequences without a health-management system, escalating ordinary wall/ceiling bump damage or making purchased jetpack speed a liability.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect actual landing/jetpack behaviour and research reliable landing classification in the current controller. Compare identical narrow/multi-level return routes at every paid jetpack tier.
- Implement the consequence/confirmation policy accepted in [59](59-return-and-fall-design.md), and validate starting thresholds against real landings. Implement the accepted rescue scaling/limits, show exact cost/loss before confirmation and preserve snapshot revalidation; numerical tuning must not introduce a new fee or debt policy. Any proposed automatic rescue fee or changed loss rule requires a specific new decision; technical landing classification stays within this task.
- Acceptance: a landing applies its consequence once, ordinary bumps/small drops remain harmless, and stronger mobility remains controllable/desirable. Zero battery, pause/focus, rescue, ceilings/slopes and save/relaunch cannot create penalty loops or a softlock; preserve excavation and all protected progression. Test low/high wallet balances, empty/full bags and repeated rescue: recovery remains available without debt/softlocks and the accepted late-game deterrent is applied consistently. Inspect native traversal and deliver the build.
