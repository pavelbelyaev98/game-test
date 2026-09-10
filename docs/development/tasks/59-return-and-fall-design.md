# Task 59 - Decide return pressure and fall consequences

Type: design/research; documentation only. Status: `planned`. Prerequisites: `47`, `49`, `70`, `94`, plus `15` trip evidence.

Feature: [return and rescue](../../features/backlog/return-rescue.md). Implementation: `50`; full-run review: `37`. [Queue](../tasks.md).

## Research and proposal

- Read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect existing rescue and compare actual return/landing evidence at paid jetpack tiers. Separate collision faults, orientation trouble and dull commuting from intended return risk.
- Produce a consequence table for harmless small drops, substantial landings and extreme situations: trigger, result, duration/energy effect, recovery and how often it may apply. Compare brief stun and battery-loss proposals against the same routes.
- Define whether an extreme fall can invoke rescue and what confirmation/cost rules apply. Preserve `86` automatic fuel-depletion rescue, protected progress, no health meter and no penalties for ordinary wall/ceiling bumps.
- Audit the implemented rescue cap of 10 credits plus ordinary carried loot loss across early/late wealth and empty/full bags. Propose a transparent economy-aware deterrent if needed so rich players do not use rescue as normal return. Compare cost/time tradeoffs and protect broke/stranded recovery; no hidden fee, debt trap or changed loss policy without a user decision.
- Resolve the [finite-economy recovery gap](../../research/steam-review-audit/progression-and-recovery.md#protect-a-finite-economy-from-permanent-loss) using `56`'s necessary-cost assumptions. State how a save remains finishable after losses and distinguish that guarantee from buying every optional upgrade after arbitrary repeated loss. Prepare valuable-bag-loss, repeated early-rescue, nonessential-purchase and depleted-shallow-region cases for `50`/`37`; returning to the surface alone does not prove economic recovery.
- Include starting thresholds and targeted traversal tests; final numerical tuning belongs to `50`/`37`. Improved mobility must remain desirable.
- Consume `93`/`94`'s selected reserve/advice meaning and `15`'s comprehension evidence. Separate a warning the player misunderstood, an intentionally accepted risk and an unexpectedly costly route before tuning penalties; do not compensate for misleading advice by making rescue harsher or reopening warning semantics here.
- Consume the keep/defer/implement decisions from `70`; do not re-decide HOME/marker form while choosing consequences. A dull commute needs diagnosis before changing rescue, not an intentional incentive to teleport. No outposts, normal teleports or new hazards are authorized.
- Read the user's reopened refill/travel proposals in `98`/`99`. Use their selected cost/route policy, or explicitly the current baseline if omitted/deferred, when comparing rescue against normal service. Do not independently introduce a refill tariff or teleporter here; paid service must not make deliberate depletion the cheap refill, and a selected shortcut must not invalidate loss/recovery assumptions.

## Questions to resolve with the user

Review the consequence table, extreme-fall rescue policy and poor/rich rescue price examples. Decide how strongly repeated rescue should be discouraged, whether the proposed scaling is understandable and how empty-wallet recovery remains safe. Use `70` for unresolved navigation questions rather than assuming an aid is selected.

Review the concrete finite-loss examples and any policy needed to preserve the existing finishable-save promise. Keep finite populations and current losses unless a change is selected; replenishing loot, refunds, free equipment and guaranteed all-upgrade completion are not automatic remedies.

## Done when

- The accepted rules and intended feel are recorded in the return feature, with no ambiguous automatic fee or loss policy.
- `50` implements those rules and validates its controller/landing classification rather than deciding the punishment while coding it. Any optional scope change remains separately selected.

## Return friction budget

- From `15`/`70`, propose a measurable excavation-versus-commuting budget for review, separating active digging/discovery, physical return, station interaction and travel back to the work face. Record each return cause: full bag, energy, voluntary purchase or disorientation; diagnose badly staggered limits that force another immediate trip.
- Keep sell/recharge/upgrade points adjacent and service interactions brief. Tune starting capacity/energy and rewarding jetpack gains before adding convenience systems; no warmth, oxygen, food or second fuel meter. `37` validates the accepted budget at early/middle/late progression and across route shapes.
