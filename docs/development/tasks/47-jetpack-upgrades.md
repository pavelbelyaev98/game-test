# Task 47 - Paid jetpack power and efficiency progression

Type: implementation. Status: `planned`. Prerequisites: 46, 56, 67.

Feature: [battery jetpack](../../features/backlog/battery-jetpack.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- After `46`, implement the agreed sequential jetpack track in the existing shop/save flow. Improve thrust, efficiency and sustained controllable ascent; preserve jump/hold timing, airborne restart and shared battery accounting.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), compare narrow-shaft, lateral-tunnel and open-pit routes, and measure acceleration/braking/energy. Implement the accepted track structure and movement milestones from [56](56-progression-design.md); test control and efficiency rather than inventing extra purchase categories while coding.
- Show what changes before purchase and make old shallow returns clearly easier. No speed-scaled ordinary wall/ceiling bump penalty; fall consequences belong to `50`. Honor the `65` comfort preferences at every tier, including any later jetpack camera effects; do not introduce roll, shake or FOV kicks simply to advertise thrust. Any new jetpack model/audio requires its own specific approval.
- Each paid level must demonstrate a practical benefit on matched routes with battery capacity held constant, then in realistic mixed purchases. Distinguish better control/ascent/efficiency from simply having a larger battery; major tiers need qualitative milestones. If a tier cannot justify its price, return a concrete track change to `56` rather than selling negligible percentages.
- Acceptance: real bought levels remain controllable at different frame rates, preserve all existing input barriers and affordable final thrust, and survive saving/rescue. Preserve [67's crouch](../../features/backlog/precision-movement.md), including slower horizontal flight corrections, safe low ceilings and stance changes without added thrust/flight resets. Compare normal/late traversal without admin overrides; `50` rechecks landings and `37` measures whether returns become dull commuting.
- Use the [accepted `63` targets and review cases](../../features/backlog/release-validation.md) for traversal/focus/FOV and frame-time checks; late flight through heavily edited routes must retain the same budgets and comfort controls.
