# Task 47 - Paid jetpack power and efficiency progression

Type: implementation. Status: `planned`. Prerequisites: 46, 56.

Feature: [battery jetpack](../../features/backlog/battery-jetpack.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- After `46`, implement the agreed sequential jetpack track in the existing shop/save flow. Improve thrust, efficiency and sustained controllable ascent; preserve jump/hold timing, airborne restart and shared battery accounting.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), compare narrow-shaft, lateral-tunnel and open-pit routes, and measure acceleration/braking/energy. Implement the accepted track structure and movement milestones from [56](56-progression-design.md); test control and efficiency rather than inventing extra purchase categories while coding.
- Show what changes before purchase and make old shallow returns clearly easier. No speed-scaled ordinary wall/ceiling bump penalty; fall consequences belong to `50`. Any new jetpack model/audio requires its own specific approval.
- Acceptance: real bought levels remain controllable at different frame rates, preserve all existing input barriers and affordable final thrust, and survive saving/rescue. Compare normal/late traversal without admin overrides; `50` rechecks landings and `37` measures whether returns become dull commuting.
