# Task 59 - Decide return pressure and fall consequences

Type: design/research; documentation only. Status: `planned`. Prerequisites: `47`, `49`, plus `15` trip evidence.

Feature: [return and rescue](../../features/backlog/return-rescue.md). Implementation: `50`; full-run review: `37`. [Queue](../tasks.md).

## Research and proposal

- Read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect existing rescue and compare actual return/landing evidence at paid jetpack tiers. Separate collision faults, orientation trouble and dull commuting from intended return risk.
- Produce a consequence table for harmless small drops, substantial landings and extreme situations: trigger, result, duration/energy effect, recovery and how often it may apply. Compare brief stun and battery-loss proposals against the same routes.
- Define whether an extreme fall can invoke rescue and what confirmation/cost rules apply. Preserve the accepted manual rescue, protected progress, no health meter and no penalties for ordinary wall/ceiling bumps.
- Include starting thresholds and targeted traversal tests; final numerical tuning belongs to `50`/`37`. Improved mobility must remain desirable.
- If return evidence suggests navigation or commute changes, record the actual problem and a separate decision proposal. This task does not authorize HOME, outposts, teleporting or new hazards.

## Questions to resolve with the user

Review the consequence table and extreme-fall rescue policy using concrete examples. Decide whether the proposed outcome feels appropriate; discuss optional navigation only if the existing evidence gate has been met.

## Done when

- The accepted rules and intended feel are recorded in the return feature, with no ambiguous automatic fee or loss policy.
- `50` implements those rules and validates its controller/landing classification rather than deciding the punishment while coding it. Any optional scope change remains separately selected.
