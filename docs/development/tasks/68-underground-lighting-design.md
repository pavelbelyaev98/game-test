# Task 68 - Design passive underground lighting

Type: design/research; documentation only. Status: `planned`. Prerequisites: `63`, `65`.

Feature: [underground lighting](../../features/backlog/underground-lighting.md). Implementation: [69](69-underground-lighting.md); consumers: `56`, `57`, `39`. [Research](../../research/player-review-findings.md#physical-comfort). [Queue](../tasks.md).

## Why and current gap

The current scene has a directional sun and no dedicated player light. Deep/lateral excavation needs dependable visibility without placing lamps, switching tools or purchasing basic usability. This is a visibility requirement, not a darkness hazard or another battery chore.

## Research and proposal

- Inspect the current URP setup, occlusion, user-owned terrain and approved content; assess enclosed passages, large chambers, close walls and surface transitions at `65` FOVs.
- Propose a passive headlamp or tool-mounted light, beam width/range/falloff, placement and glare/shadow policy. Baseline visibility must work at zero battery; only accepted digs and active thrust consume the shared battery.
- Research official Unity/URP lighting and performance guidance under `63`. Identify through-wall leakage, flat-looking materials and washed-out nearby finds; record a measurable visibility/performance target before integration.
- Compare a sufficient fixed baseline against optional cheap light improvements or integration with an existing equipment track. The report's 2–3 levels are a proposal, not an approved new shop category; pass any selected structure to `56`.
- Define the visible housing, if needed, and its first-person visibility/terrain handoff with `57`. No lights-as-platforms, placeable-light chores or generated substitute props; any actual model/material/audio batch still needs explicit approval.

## Questions to resolve with the user

- Head-mounted or tool-mounted presentation, and how broad/bright should close tunnels versus large chambers feel? Present readable options with tradeoffs.
- Keep a sufficient fixed light, or select later improvements within an existing track? Decide whether a new track is worthwhile before adding one.
- Which visible equipment and darkness/contrast direction fit the terrain art? Preserve navigation/readability even when battery is empty.

## Done when

Record the chosen mounting, baseline visibility, optional upgrade decision, rationale/avoidances, performance targets and asset brief in the feature. Update `56`/`57`/`69` with the selected scope, explicitly assigning any later paid improvement to its implementation owner without making baseline lighting depend on late tracks; unanswered style questions or a brief alone do not complete gameplay.
