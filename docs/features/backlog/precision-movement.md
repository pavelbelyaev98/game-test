# Precision movement

Status: not implemented. [66 - design](../../development/tasks/66-precision-movement-design.md) selects the interaction; [67 - implementation](../../development/tasks/67-precision-movement.md) integrates it before production movement acceptance.

Idea coverage: section 50 and traversal throughout excavation.

## Decision and rationale

Provide a held precision movement option for careful player-made ledges while retaining responsive normal walking. The exact slow-walk versus lowered-crouch interaction is unresolved in `66`; a report example does not select capsule height, key or air control.

The [comfort findings](../../research/player-review-findings.md#physical-comfort) describe differing movement preferences. The response should give the player control, not globally slow traversal or repair faulty terrain collision through input changes.

## Required behaviour and exclusions

- Preserve click-and-hold digging, Space jump/jetpack, targeting and [camera comfort](fps-controls.md); precision is available without buying an ergonomic upgrade.
- Hold the chosen modifier for precision. A true crouch requires safe low-ceiling/stand-up and camera transitions only if selected.
- No stealth/stamina system, automatic cliff guard or mandatory stance puzzle. Do not alter `26`/`34` cleanup or erase player-built supported ledges.
- Restore movement safely through pause/focus and save/reload. Input and collision acceptance belongs to `67`.

## Decisions to retain

When `66` resolves, record the selected input, grounded/airborne behaviour, stance/clearance rules and reasons for rejecting alternatives here. Leave numerical tuning and measured evidence in the owning task/completion record.
