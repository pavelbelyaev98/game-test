# Just a few peppers

Working proposal v4 · September 5, 2026 · focused first-game scope · walkable foundation delivered · gameplay feel untested

[Game entry point](../readme.md) · [Numbered tasks](development/tasks/readme.md) · [New-chat prompt](development/new-chat-prompt.md) · [Development roadmap](development/roadmap.md) · [Implementation status](development/status.md)

**Grandpa asked you to help with “just a few peppers.” Clear the absurd piles consuming his outdoor yard, uncover better ways to move and process them, and use his increasingly ridiculous homemade equipment until the yard is open and the family's winter supply is finished.**

The whole game follows **scoop → carry → dump → uncover better equipment → clear bigger loads → store winter food → finish the day**. One automatic processing line produces one kind of jarred peppers. One storage rack accepts every finished carrier.

Winter preparation remains the story. The cellar and labelled family parcels fill visually from stored progress; returned jars, the old grinder, reused tools, and the vine table give the place its identity. The meal appears through an automatic ending transition. These details create no additional chores.

## Read the specifications

For numbered feature briefs, use the [task queue](development/tasks/readme.md); [Start developing](development/start-here.md) explains the workflow and [the fresh-chat prompt](development/new-chat-prompt.md) recovers the necessary context. The AI builds and wires each task; Pavel plays it and gives feedback. [Unity and asset rules](development/unity-and-assets.md) require supported APIs and prioritize free commercially usable assets.

| File | What it settles |
| --- | --- |
| [Core loop and mechanics](core-loop-and-mechanics.md) | Gathering, tipping, one automatic line, one deposit target, and reliable progress. |
| [Yard and progression](yard-and-progression.md) | One compact property, useful paths, carrier upgrades, and Grandpa's three equipment stages. |
| [Household presentation and parcels](household-readiness-and-parcels.md) | Food displays driven by stored progress, background culture, and exact finish conditions. |
| [Story and characters](story-and-characters.md) | Grandpa, the family purpose, original humor, and the meal ending. |
| [Objectives, discoveries, and comedy](jobs-events-and-comedy.md) | A short playable arc and dialogue candidates. |
| [Look, sound, and comfort](look-sound-and-comfort.md) | Responsive piles, satisfying loads, readable equipment, and a Bulgarian yard. |
| [Research and authenticity](research-and-authenticity.md) | Real processes and cultural anchors, with fictional machinery clearly identified. |
| [Scope and validation](scope-and-validation.md) | First-version limits, meaningful tests, and what AI implementation still needs to prove. |
| [Design decisions and revision history](design-pivot.md) | Why v4 removes household tasks and the second product route. |

## First-version limits

| Area | Decision |
| --- | --- |
| World | One compact outdoor yard, up to five connected pockets, a shed view and small cellar view. |
| Handling | Crate immediately, then wheelbarrow; broad scoop, carry, and tip/place. |
| Machinery | Familiar appliance → Grandpa's improved loader → absurd final processor; one active station. |
| Food | One sound pepper class, one finished product, one permanent storage handoff. |
| Choice | Which reachable pile or access pocket to pursue and when to collect finished food. |
| Culture | Props, a few lines, simple food-display changes, and one meal ending. |
| Completion | All finite harvest cleared and stored, then the player's Finish the day action. |

Sorting, recipe management, grinder gameplay, manual parcel allocation, jar-return tasks, table chores, NPC work schedules, money, crafting, and post-game favors are excluded from this first version. The former under-five-minute household-task allowance is retired, not a feature budget to fill later.

## Three moments to earn

1. A wheel emerges from the pile; the next wheelbarrow load visibly changes the scale of the work.
2. Grandpa's tarp comes off, revealing a ridiculous machine that genuinely reduces feeding and output effort.
3. The open yard, fuller cellar, and waiting family parcels lead into a meal at the same vine table seen at the start.

These are design targets, not evidence of fun. Short paths, responsive handling, and worthwhile upgrades must make the ordinary loads enjoyable between reveals.

## Sources and implementation status

The [comparable-game memo](../../../research/case-studies/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md), [Bulgarian culture memo](../../../research/culture/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md), and latest supplied conversation inform this revision. Source notes remain unchanged. Their suggestions are not automatic feature requirements or measured engineering estimates.

The display title remains **Just a few peppers**. The game is grouped under `games/just-a-few-peppers/`, with this design in `docs/` and the Unity project in `unity/`. Historical Chushkopek asset names and namespaces remain unchanged.

Discarded roasting designs and prototype reports have been removed. The [audit](development/repository-audit.md) records the old code that remains; it is disposable reference material. The [comparison scorecard](../../../research/concepts/prototypes/prototype-comparison-scorecard.md) remains historical and untested for this loop.

The next proposed prototype stays one mound, crate, automatic processor, output carrier, storage rack, and partly exposed wheelbarrow. The roadmap builds the crate loop in M1 and adds the wheelbarrow in M2. Full-yard presentation and the final machine wait until that test succeeds. Task 1_01 now supplies the walkable foundation; the bulk gameplay loop remains unimplemented. See the implementation status for verified evidence.
