# Task 15 - Integrate and validate one complete production trip

Type: validation. Status: `planned`. Prerequisites: 35, 25, 94, user playtests 100/101 and production acceptance in 05–09 and 11–14. This validates the three-common-type starter trip; positive detector feedback follows non-minor content in `42` → `10` → `102`.

Feature: [core loop](../../features/backlog/core-loop.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

After its prerequisites, integrate presentation, common discoveries, shovel progression, selling, recharge, warnings and rescue. Common finds are detector-silent; do not add a fourth item or manufacture a signal to validate this trip:

1. Collect the first find, physically return, sell it, recharge, and buy the upgrade.
2. Re-enter the unchanged excavation; the upgrade must visibly improve digging toward the second find.
3. Verify the alternative overextension/rescue path without resetting excavation, collected state, or upgrades.

- Before implementation: read the [review findings](../../research/player-review-findings.md), inspect the already-complete transactions/recharge/rescue and record the missing integration. Research by watching a first-time trip: can the player explain their purchase, notice the find and navigate back? No new system is required by a comparator review alone.
- Include an unguided roughly 30-minute observation when the playable content supports it: inspect the actual hole and ask why the player chose shafts, widening or branches. Record raw behaviour before suggesting a route; a vertical shaft is not inherently wrong. Evaluate quiet excavation and recognizable common finds here; `102`/`37` later evaluate whether non-minor detector signals feel optional.
- Ask the user only if observed first-use confusion warrants an onboarding choice; the removed routine HUD hints/subtitles remain removed. Distinguish orientation trouble from repetitive commuting before considering [HOME](../../features/backlog/return-rescue.md#conditional-home-direction-experiment) or return convenience; retain evidence for `70` navigation/revisit research. Do not add outposts/shortcuts without a separate decision.
- Done when focused integration checks pass, the Windows build is updated, and manual review records starter-battery/slot budgets, uninterrupted digging time, finds before first return, and dig/discovery/return/station time. The unupgraded trip must already feel satisfying; the next purchase extends meaningful work rather than fixing compulsory tiny trips. Record useful downward and lateral progress and the main feel/visual limitation. Restore the trip through `35`; expanded content/generation (`40`–`45`) and full-run pacing (`37`) remain separate.

## Additional observation measures

- Split outbound retracing from return and station time; record the actual full-bag/remaining-energy combination at each retreat and any near-immediate second return. Capture reasons as well as ratios so `59` can propose a return friction budget.
- Observe sustained digging with no immediate treasure reward, then object reveal and pickup. Record what makes removal satisfying and whether feedback obscures the object. Pass concrete early branch/return examples to `70` immediately after this task, without waiting for late paid equipment.
- Apply the [opening payoff audit](../../research/steam-review-audit/discovery-and-payoff.md#demonstrate-the-appeal-in-the-opening) at this batch's actual tier: record first common-object recognition, whether repeated collection feels useful/satisfying and a wanted purchase across named seeds/routes. Ask what the player expects to find next. Do not require a surprising unique from three common types; `40`/`42`/`102` and `37` must later establish the step up to distinctive finds and full-opening discovery appeal. No extra starter asset or fixed reveal minute is mandated.
- Apply `57`'s feedback/affordance examples and `93`/`94`'s selected warning meaning: record what the player thinks the warning knows, attempted interactions with decoration, missed identities and whether returning feels like banking a successful trip. Ask before explaining the intended meaning. A fast station transaction or mathematically correct battery percentage alone does not establish understanding; preserve findings for `59`/`37`.
- Consume the actual editable user verdicts in [100](100-movement-excavation-playtest.md) and [101](101-collection-return-playtest.md). Resolve relevant NOT OK cases and retest affected behavior on the production build. [102](102-production-discovery-playtest.md) follows actual distinctive content/detector delivery and expands these early observations; it does not gate `15`. Preserve the difference between informed user observations and separate uncoached first-player evidence.
