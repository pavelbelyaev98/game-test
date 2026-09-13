# Task 56 - Design the equipment progression structure

Type: design/research; documentation only. Status: `in_progress`; the user resumed only the inventory/shared-fuel capacity portion for `46`/`48`. Other track decisions remain paused. Prerequisites: `35`, `63` (complete). Resume only after the user returns to equipment work; [status](../status.md) owns the current next task.

Features: [shovel](../../features/backlog/shovel-progression.md), [equipment economy](../../features/backlog/selling-upgrades.md). Implementation: `25`, `11`, `39`, `46`–`49`. [Queue](../tasks.md).

## Selected capacity increment

The user likes the current digging, calls collection decent and reports 6–8 credits per trip (`100`/`101`). They request independent backpack and fuel upgrades, paid refills and a compact workshop. They explicitly select **larger shared capacity for digging and jetpack**, preserving current charge on purchase. Keep the combined shovel and current consumption/flight unchanged.

- Backpack: five owned levels, 10 → 15 → 20 → 30 → 40 slots; no weight penalty, retain every carried identity. Fuel: five levels, 100 → 150 → 200 → 300 → 400 shared capacity; no free refill with purchase. Numeric milestones/prices are initial implementation tuning.
- Each new track costs 6 / 14 / 28 / 48 credits independently; a 6–8-credit trip less up to 2 for a starter refill leaves 4–6 toward a first purchase. More slots benefit trips that fill the bag; more fuel benefits excavation/ascent. Neither is a prerequisite for the other or for the shovel.
- Save explicit owned levels and actual capacities; older saves begin these tracks at level 1 with their exact existing capacities and charge. Apply positive increments to preserve unusual legacy capacities. `98` owns paid service; `138` presents choices.
- This scoped decision enables `46`/`48`; it does not settle separate shovel strength/speed, jetpack power/efficiency, detector, lighting, model milestones or full-run pacing. Those remain below and prevent marking all of `56` done.

## Research and proposal

- Read [progression findings](../../research/player-review-findings.md#progression-and-discovery); inspect the current six shovel profiles, before/after shop statistics, saved purchases and trip evidence. Separate meaningful player choices from mere implementation variables.
- Produce a table for shovel strength, shovel speed, battery, jetpack, inventory and detector: attribute ownership, proposed level counts, major milestones, starting effect ranges and qualitative early/middle/late benefits.
- Resolve how reach belongs to the independent shovel tracks within the existing 4 m cap, and how already-owned combined levels convert without losing paid progress. Define charge handling when buying battery capacity and whether jetpack thrust/efficiency share one track.
- When resumed, consume selected refill/travel policies from `98`/`99`, or retain the implemented baseline if they are omitted/deferred. Recheck practical value and necessary costs; these new design questions do not authorize resuming this paused task or adding another equipment track.
- Describe detector interpretation/range milestones over physically eligible non-minor targets, independent from cash value. Every minor/common find stays silent at every tier, including large metal objects and clusters. Foreground one signal without identity, monetary rarity, exact-distance or GPS disclosure. Tie visible shovel milestones to equipment structure; do not commission models here.
- Preserve money-based sequential purchases, powerful old-obstacle revisits, click-and-hold input and the current chosen scoop feel. [120](120-powered-excavation-feel-design.md) later compares continuous powered removal against this baseline; it does not block these equipment decisions or authorize changing removal now.
- Propose changing purchase pressures across different routes and priorities: capacity, energy, resistant formations, control/ascent and searching should each matter. Avoid battery always dominating, fixed stage gates or resistance automatically canceling upgrades. For each jetpack/shovel level, specify a noticeable same-route benefit; ask to shorten a track if extra tiers cannot justify themselves.
- Lighting `68` is paused; it does not block the six core equipment tracks when this task resumes. Leave light purchases undecided and outside this proposal. If `68` later selects an improvement, amend the owning track and name its delivery task before implementation, without making baseline visibility depend on a purchase. Report examples such as startup energy or shovel efficiency do not change existing energy/input rules without selection.
- Define initial price/value bands and measurable benefit hypotheses, not final balanced prices. Starting battery/slots must already allow a satisfying expedition, common finds must remain useful income later, and deeper pools must not make a straight-shaft rush overwhelmingly optimal. Resistance values, movement tuning and full-run economy remain implementation/playtest work in `39`, `46`–`49` and `37`.
- Apply the [whole-expedition value audit](../../research/steam-review-audit/progression-and-recovery.md): pair major price/benefit hypotheses with expected expeditions to afford and first useful application; include mixed bag/battery/shovel levels. An upgrade can reward excavation shape or control without raising income, but fixture speed alone cannot prove its promised whole-trip benefit.
- Identify any minimum paid capabilities necessary for a normal no-passive ending, separately from optional convenience or maxed tracks. Give `59`/`37` the remaining-cost assumptions needed to check completion after finite loot losses; do not introduce a required upgrade merely to fill this table.

## Questions to resolve with the user

Review the concrete track/milestone table, independent attribute ownership, reach and old-save conversion, battery charge policy, jetpack track structure and the proposed shifting purchase priorities. Review any suggested track shortening or selected light improvements explicitly; the sample inventory → battery → strength order is not mandatory. Explain tradeoffs and recommend a coherent default for each unresolved product choice.

## Done when

- Record the accepted structure in the owning feature docs and reference it from the dependent implementation tasks, without duplicating tables.
- The tasks can implement named attributes/levels and compare actual results with the intended benefits. Unproven numerical estimates are labelled for testing; no extra categories, mechanics or assets are silently approved.

## Meltopia progression constraints

- Use the [comparator synthesis](../../research/meltopia-lessons.md) to map milestones across the whole run: basic, reinforced, powered and absurd versions of the same shovel. New material cannot require a weak replacement tool or cancel all prior purchases.
- Propose at least one exciting shovel/jetpack/detector milestone near the final phase. `37` tests final meaningful power purchase around 75-85% of first-completion time, leaving actual excavation to enjoy it; this is a pacing hypothesis, never a time/depth purchase gate or max-all requirement.
- Give `57` named physical attachment milestones and `61` a capable final kit. Keep money-based choices viable in different orders and old terrain substantially easier after upgrades.
