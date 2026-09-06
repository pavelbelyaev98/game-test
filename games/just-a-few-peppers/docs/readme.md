# Just a few peppers

Current production target · scope boundary September 6, 2026 · walkable foundation delivered · bulk gameplay feel untested

[Game entry point](../readme.md) · [Numbered tasks](development/tasks/readme.md) · [New-chat prompt](development/new-chat-prompt.md) · [Development roadmap](development/roadmap.md) · [Implementation status](development/status.md)

**Grandpa asked for help with “just a few peppers.” Clear an absurd pepper-covered Bulgarian yard, uncover useful equipment and shortcuts, process the harvest in increasingly unreasonable machinery, and see winter food accumulate for the family.**

The current prototype loop is **SCOOP → CARRY → DUMP → AUTOMATIC PROCESSING → HAND OFF FINISHED FOOD → SEE PROGRESS → UNCOVER USEFUL EQUIPMENT**. One automatic processing line produces roasted-pepper jars. The **Finished Food Handoff Rack** accepts every finished carrier and ends the player's food-handling responsibility. One stored-food total drives household distribution visually.

Winter preparation remains the story. The cellar and labelled family parcels fill visually from stored progress; returned jars, the old refrigerator/tool cupboard, decorative grinder, reused tools, and the vine table give the place its identity. A simple table/gift visual or short thank-you may be added later as a cuttable flourish after completion, but it creates no interaction, cinematic requirement, or completion flag. Active dialogue and design drafts use English; eventual localization/native review remains later presentation work.

Depth comes from **visible transformation + increased power + spatial discovery + cultural personality**. Retain implementation requirements that improve scoop, dump, reveal, upgrade, or the visible payoff; other household details stay scenery. This is the authority, and removed systems are not restored.
## Read the specifications

For numbered feature briefs, use the [task queue](development/tasks/readme.md); [Start developing](development/start-here.md) explains the workflow and [the fresh-chat prompt](development/new-chat-prompt.md) recovers the necessary context. The AI builds and wires each task; the human playtester plays it and gives feedback. [Unity and asset rules](development/unity-and-assets.md) require supported APIs and prioritize free commercially usable assets.

| File | What it settles |
| --- | --- |
| [Core loop and mechanics](core-loop-and-mechanics.md) | Gathering, tipping, one automatic line, one deposit target, and reliable progress. |
| [Yard and progression](yard-and-progression.md) | One compact property, useful paths, carrier upgrades, and Grandpa's three equipment stages. |
| [Household presentation and parcels](household-readiness-and-parcels.md) | Food displays driven by stored progress, background culture, and exact finish conditions. |
| [Story and characters](story-and-characters.md) | Grandpa, the family purpose, original humor, harvest completion, and optional closing presentation. |
| [Objectives, discoveries, and comedy](jobs-events-and-comedy.md) | A short playable arc and dialogue candidates. |
| [Look, sound, and comfort](look-sound-and-comfort.md) | Responsive piles, satisfying loads, readable equipment, and a Bulgarian yard. |
| [Research and authenticity](research-and-authenticity.md) | Real processes and cultural anchors, with fictional machinery clearly identified. |
| [Scope and validation](scope-and-validation.md) | First-version limits, meaningful tests, and what AI implementation still needs to prove. |
| [Design decisions and revision history](design-pivot.md) | Why current removes household tasks and the second product route. |

## Current design state snapshot

- **Current requirements (authoritative):** one finite outdoor property, one outdoor loading chain (crate → wheelbarrow → final station), one automatic processing line, one reusable output carrier, one handoff rack, and automatic harvest completion that preserves normal control. The working prototype has no currency/economy system.
- **Reported developer observations (firsthand, not universal):** relaxing visuals, sparse outdoor ambience, earning/upgrading, faster collection, a collection-rate statistic, area rewards, autosave, and an incidental kickable ball were appreciated in Leaf it Alone. A small bag reportedly filled in about four seconds, causing frustrating interruptions and repeated full/error sounds; a medium bag existed elsewhere, but in a different upgrade location from the usual Tab interface.
- **Working design hypotheses:** repeated visible progress from discoveries and a clear final upgrade that visibly shortens hauling/output handling will sustain player continuation longer than capacity-only changes.
- **Pending decision experiment:** after the core handling gate and before whole-yard production, compare the discovery baseline with a separately authorized, bounded **Coins** variant. Until authorized, the prototype remains discovery-only. Optional readable cards remain a deferred flavor candidate, not a progression layer.
- **Implementation evidence status:** this pass is documentation-only; no Unity scene/assets/build changes were made.

## First-version limits

| Area | Decision |
| --- | --- |
| World | One compact outdoor yard, at most five connected pockets, a shed view and small cellar view. Choose the pocket count after measuring the core prototype; three or four is enough if the arc works. |
| Handling | Crate immediately, then wheelbarrow; broad scoop, carry, and tip/place. |
| Machinery | Familiar appliance → Grandpa's improved loader → absurd final processor; one active station. The final tier combines larger buffers/fewer output trips with a substantially shorter final-supply hauling route. |
| Food | One sound pepper class, one finished product, one permanent storage handoff. |
| Choice | Which reachable pile or access pocket to pursue and when to collect finished food. |
| Culture | Props, a few lines, simple food-display changes, and an optional table/gift flourish. |
| Completion | The final valid deposit stores the full finite harvest, commits completion, shows a quiet nonmodal acknowledgement, and leaves normal movement/camera and pause/menu controls available. |

Sorting, recipe management, grinder gameplay, manual parcel allocation, jar-return tasks, table chores, NPC work schedules, money, crafting, and post-game favors are excluded from this first version. The former under-five-minute household-task allowance is retired, not a feature budget to fill later.

## Three moments to earn

1. A wheel emerges from the pile; the next wheelbarrow load visibly changes the scale of the work.
2. Grandpa's tarp comes off, revealing a ridiculous machine whose fixed nearby intake substantially shortens hauling and whose larger output reduces collection trips, with enough supply left for repeated use.
3. The open yard, fuller cellar, and waiting family parcels remain visible after a quiet harvest-complete acknowledgement; an optional vine-table/gift flourish may reinforce the family payoff.

These are design targets, not evidence of fun. Short paths, responsive handling, and worthwhile upgrades must make the ordinary loads enjoyable between reveals.

## Sources and implementation status

The [comparable-game memo](../../../research/case-studies/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md), [Bulgarian culture memo](../../../research/culture/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md), and latest supplied conversation inform this revision. Source notes remain unchanged. Their suggestions are not automatic feature requirements or measured engineering estimates.

The display title remains **Just a few peppers**. The game is grouped under `games/just-a-few-peppers/`, with this design in `docs/` and the Unity project in `unity/`. Historical Chushkopek asset names and namespaces remain unchanged.

Discarded roasting designs and prototype reports have been removed. The [audit](development/repository-audit.md) records the old code that remains; it is disposable reference material. The [comparison scorecard](../../../research/concepts/prototypes/prototype-comparison-scorecard.md) remains historical and untested for this loop.

The immediate interaction prototype stays one outdoor corner, one authored mound, crate, broad scoop, automatic processor, reusable output carrier, one handoff rack, and a partly exposed wheelbarrow with comparable supply remaining after unlock. The roadmap builds the crate loop in M1 and adds discovery/comparison in M2. Grandpa dialogue, household display states, final machine, cellar, parcels, grinder, and ending scene are outside this prototype. See the interaction question set in [Scope and validation](scope-and-validation.md#next-experiment-one-pile-one-carrier-one-discovery) and implementation status for actual evidence. This documentation update changes no Unity code or playable artifacts.

