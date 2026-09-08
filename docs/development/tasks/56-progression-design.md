# Task 56 - Design the equipment progression structure

Type: design/research; documentation only. Status: `planned`. Prerequisites: `35`, `63`.

Features: [shovel](../../features/backlog/shovel-progression.md), [equipment economy](../../features/backlog/selling-upgrades.md). Implementation: `25`, `11`, `39`, `46`–`49`. [Queue](../tasks.md).

## Research and proposal

- Read [progression findings](../../research/player-review-findings.md#progression-and-discovery); inspect the current six shovel profiles, before/after shop statistics, saved purchases and trip evidence. Separate meaningful player choices from mere implementation variables.
- Produce a table for shovel strength, shovel speed, battery, jetpack, inventory and detector: attribute ownership, proposed level counts, major milestones, starting effect ranges and qualitative early/middle/late benefits.
- Resolve how reach belongs to the independent shovel tracks within the existing 4 m cap, and how already-owned combined levels convert without losing paid progress. Define charge handling when buying battery capacity and whether jetpack thrust/efficiency share one track.
- Describe detector interpretation/range milestones without identity, rarity, exact-distance or GPS disclosure. Tie visible shovel milestones to the equipment structure; do not commission models here.
- Preserve money-based sequential purchases, powerful old-obstacle revisits, click-and-hold input and the current chosen scoop feel. Timed removal remains a separate proposal only if evidence shows a problem.
- Define initial price/value bands and measurable benefit hypotheses, not final balanced prices. Resistance values, movement tuning and full-run economy remain implementation/playtest work in `39`, `46`–`49` and `37`.

## Questions to resolve with the user

Review the concrete track/milestone table, independent attribute ownership, reach and old-save conversion, battery charge policy and jetpack track structure. Explain tradeoffs and recommend a coherent default for each unresolved product choice.

## Done when

- Record the accepted structure in the owning feature docs and reference it from the dependent implementation tasks, without duplicating tables.
- The tasks can implement named attributes/levels and compare actual results with the intended benefits. Unproven numerical estimates are labelled for testing; no extra categories, mechanics or assets are silently approved.
