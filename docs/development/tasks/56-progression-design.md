# Task 56 - Design the equipment progression structure

Type: design/research; documentation only. Status: `planned`. Prerequisites: `35`, `63`, `68`.

Features: [shovel](../../features/backlog/shovel-progression.md), [equipment economy](../../features/backlog/selling-upgrades.md). Implementation: `25`, `11`, `39`, `46`–`49`. [Queue](../tasks.md).

## Research and proposal

- Read [progression findings](../../research/player-review-findings.md#progression-and-discovery); inspect the current six shovel profiles, before/after shop statistics, saved purchases and trip evidence. Separate meaningful player choices from mere implementation variables.
- Produce a table for shovel strength, shovel speed, battery, jetpack, inventory and detector: attribute ownership, proposed level counts, major milestones, starting effect ranges and qualitative early/middle/late benefits.
- Resolve how reach belongs to the independent shovel tracks within the existing 4 m cap, and how already-owned combined levels convert without losing paid progress. Define charge handling when buying battery capacity and whether jetpack thrust/efficiency share one track.
- Describe detector interpretation/range milestones over physically noteworthy authored targets, independent from cash/rarity. Foreground one nearby signal without identity, rarity, exact-distance or GPS disclosure; upgrades improve interpretation, not monetary filtering. Tie visible shovel milestones to the equipment structure; do not commission models here.
- Preserve money-based sequential purchases, powerful old-obstacle revisits, click-and-hold input and the current chosen scoop feel. Timed removal remains a separate proposal only if evidence shows a problem.
- Propose changing purchase pressures across different routes and priorities: capacity, energy, resistant formations, control/ascent and searching should each matter. Avoid battery always dominating, fixed stage gates or resistance automatically canceling upgrades. For each jetpack/shovel level, specify a noticeable same-route benefit; ask to shorten a track if extra tiers cannot justify themselves.
- Consume `68`'s fixed-light versus selected improvement decision; do not add a light category by assumption. Report examples such as startup energy or shovel efficiency are not changes to existing energy/input rules until explicitly selected.
- Define initial price/value bands and measurable benefit hypotheses, not final balanced prices. Starting battery/slots must already allow a satisfying expedition, common finds must remain useful income later, and deeper pools must not make a straight-shaft rush overwhelmingly optimal. Resistance values, movement tuning and full-run economy remain implementation/playtest work in `39`, `46`–`49` and `37`.

## Questions to resolve with the user

Review the concrete track/milestone table, independent attribute ownership, reach and old-save conversion, battery charge policy, jetpack track structure and the proposed shifting purchase priorities. Review any suggested track shortening or selected light improvements explicitly; the sample inventory → battery → strength order is not mandatory. Explain tradeoffs and recommend a coherent default for each unresolved product choice.

## Done when

- Record the accepted structure in the owning feature docs and reference it from the dependent implementation tasks, without duplicating tables.
- The tasks can implement named attributes/levels and compare actual results with the intended benefits. Unproven numerical estimates are labelled for testing; no extra categories, mechanics or assets are silently approved.

## Meltopia progression constraints

- Use the [comparator synthesis](../../research/meltopia-lessons.md) to map milestones across the whole run: basic, reinforced, powered and absurd versions of the same shovel. New material cannot require a weak replacement tool or cancel all prior purchases.
- Propose at least one exciting shovel/jetpack/detector milestone near the final phase. `37` tests final meaningful power purchase around 75-85% of first-completion time, leaving actual excavation to enjoy it; this is a pacing hypothesis, never a time/depth purchase gate or max-all requirement.
- Give `57` named physical attachment milestones and `61` a capable final kit. Keep money-based choices viable in different orders and old terrain substantially easier after upgrades.
