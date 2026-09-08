# Task 25 - Independent shovel speed and strength purchases

Type: implementation. Status: `planned`. Prerequisites: 35, 56.

Feature: [shovel progression](../../features/backlog/shovel-progression.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- After `35`, implement the requested **digging speed** and **digging strength** as independently purchasable upgrades with separate levels, costs and saved state; do not reopen the completed tuning tasks.
- Speed changes shovel cycle time/cadence without increasing soil removed per stroke. Strength changes soil removal without automatically accelerating cadence. The current six combined presets remain until this task; reach must stay capped at 4 m.
- Acceptance: each purchase affects only its intended attribute, shows current-to-next cost/effect/level, persists through `35`, and rejects invalid/unaffordable/duplicate purchases. Validate throughput across combinations in the current terrain and preserve admin testing. The later material task `39` verifies overpowering the same old hard formation; do not introduce a circular dependency. Prices and track lengths feed [Task `37`](../../features/backlog/selling-upgrades.md#task-37---full-run-upgrade-pacing), without depth gates or compensating terrain scaling.
- Concern: instant late-level cuts can feel excessive because volume grows cubically with radius. Completed `24`/`31` softened the radius curve. If it still feels abrupt, discuss removing soil progressively during a short shovel stroke, keeping collision synchronized and charging only once per accepted stroke. This timed-removal option remains a proposal, not implemented behaviour.
- Before implementation: read [progression findings](../../research/player-review-findings.md#progression-and-discovery), measure the existing profiles and inspect shop comparisons. Implement the accepted track, reach and non-destructive save-conversion decisions from [56](56-progression-design.md); do not ask the same product questions again. Validate milestone throughput and migration, with final prices in `37`. A timed-removal proposal still requires reproduced evidence and a separate explicit decision.
