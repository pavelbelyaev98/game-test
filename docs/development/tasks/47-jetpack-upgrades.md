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

## Movement as a reward

- Compare enjoyment and controllable freedom, not only ascent speed: revisit old shafts, arrest falls and steer lateral returns at matched battery capacity. Later equipment should shorten known commutes and feel capable without a grapple, mandatory platforming or compulsory camera motion; feed measured travel-time gains into `37`.
- Use `99`/`100`/`101` to test the user’s Meltopia comparison: enjoyable vertical flight versus dull horizontal travel. This is not a reported defect in our current build. Consume selected mobility changes when available; stronger vertical thrust alone does not establish better lateral traversal. New horizontal capabilities require the `99`/`56` decision rather than an unreviewed extra track.
- Exercise `45`'s ceiling/high-wall and widened-chamber recovery cases using actual collection reach, normal paid tiers and realistic remaining battery. Confirm the player can approach/aim/collect and return without developer overrides or optional explosives; mere visibility or reachable camera height is insufficient. Return impossible content/placement cases to their owner rather than silently extending interaction range.
- Recheck `93`/`94`'s warning scenarios against the actual bought efficiency/thrust and battery combinations. Any selected effort estimate must consume effective settings and retain its uncertainty limits; a changed flight tier cannot silently invalidate the advice. Do not reinterpret a reserve-only label as measured route safety.
