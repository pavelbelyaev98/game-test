> Archived roasting-focused proposal. Superseded by the [current bulk-clearing design](../../readme.md). Historical ideas and test proposals below are not current requirements.

# Scope, study lessons, and validation

[Design index](readme.md) · design proposal, not a production start

The main risk is that roasting feels like watching a timer while peeling and transfers become unpaid chores. Story, extra recipes, and more crates cannot resolve that risk. Test the ordinary actions before treating the campaign as a commitment.

AI-generated implementation can reduce typing and scaffolding effort. A solo developer still has to judge feel, catch state errors, integrate assets, and tune the whole experience. The design therefore limits concurrent demands, reusable content, and systems that depend on one another.

## What the local studies change

These are design inferences from the repository's research, not independently verified claims about current sales or player counts.

| Research document | Lesson used here | Concrete design consequence |
| --- | --- | --- |
| [Cash Cleaner](../../../../../Ideas/Cash_Cleaner_Simulator_Case_Study.md) | Processing states, useful containers, specifications, and physical output can sustain one place. | Roast/rest/finish/pack; bowls and trays move useful batches; jars and parcels display progress. |
| [Leaf it Alone and Librarian](../../../../../Ideas/Leaf_it_Alone_and_Librarian_Case_Study.md) | Visible completion and learned recognition can reward the player. | Finished shelves and readable roast judgment; no compulsory rare-loot economy. |
| [Drywall Eating Simulator](../../../../../Ideas/Drywall_Eating_Simulator_Case_Study.md) | The funny premise and the ordinary action need separate evaluation; unreliable interactions spoil both. | Test with banter muted; broad targets, recoverable mistakes, finite jobs. |
| [Twenty-game meta-study](../../../../../Ideas/Simulator_Games_Meta_Study_20_Games_Blueprint.md) | Progression should relieve established friction and keep a clear activity. | Triple capacity and tray transfer preserve roast decisions while reducing support actions. |
| [Prison Escape](../../../../../Ideas/Prison_Escape_Simulator_Dig_Out_Case_Study.md) | A concrete destination makes progress understandable. | One family list, visible final crate, reachable ending. |
| [Chopping Trees](../../../../../Ideas/A_Game_About_Chopping_Trees_Case_Study.md) | Repeated physical actions need immediate response and readable results. | Prioritize contact, blistering, peel release, and packed food before more content. |
| [Early blueprint](../../../../../Ideas/game_type.md), [idea collection](../../../../../Ideas/ideas.md), [final shortlist](../../../../../Ideas/Final_Game_Ideas_Bulgarian_Traditions_and_Meme_Sims.md) | These provide hypotheses and alternatives, not mandatory checkboxes. | Keep processing, transformation, upgrades, and comic identity; omit mandatory loot, detector, economy, and exploration layers. |

## Proposed smallest complete game

| Area | Baseline cap |
| --- | --- |
| Places | One outdoor yard; street backdrop; supply shed frontage; small cellar ending alcove. |
| Campaign | Eight main jobs, five chapter beats, one ending; no procedural story. |
| Food | One pepper family with two readable timing variants; optional curved shape only if worthwhile. |
| Products | Opening lunch plate, roasted-pepper jars, one conditional lyutenitsa recipe. |
| Urgent stations | At most three roasting sockets in one active appliance. |
| Other work | Two bowls, one finishing tray, one packing station, family handoff racks; conditional grinder/pan. |
| Characters | One seated Grandpa; relatives through notes/offscreen lines. |
| Dialogue | About 24–30 short authored lines total. |
| Progression | The finite milestone rewards in [Yard and progression](yard-and-progression.md); no mandatory currency economy. |
| Optional content | Up to four job remixes and three pepper portraits using existing assets. |
| Essential comfort | Clear inputs/cues, pause, sensitivity/FOV, readable text, restart/recovery, and progress persistence. |

Do not add growing crops, shopping trips, free construction, traffic, co-op, dynamic weather hazards, electricity management, autonomous worker AI, freeform soft-body food, cooking every Bulgarian dish, or rakia distillation. Those features would require a different scope decision.

Potential growth after a successful small game should favor new requests, family notes, jar presentation, and layout variations. A second neighborhood setting or another recipe is a later expansion candidate. Do not count speculative expansions as necessary to make the current game enjoyable.

## Experiments before production

No coding or playtests were performed for this documentation request. These are future steps, not completed evidence. Preserve the existing [Stage 0 report](../../../../prototypes/chushkopek-stage0.md) and [original prototype scorecard](../../../../prototypes/prototype-comparison-scorecard.md) as separate records.

| Experiment | Smallest useful setup | Evidence sought | Decision if it fails |
| --- | --- | --- | --- |
| A. Does roasting involve judgment? | One pepper type, clear inspection, six complete cycles, subdued dialogue. | Player can explain cues and enjoys selecting removal time, not merely waiting for a prompt. | Improve cues once, then reconsider roasting as the hero action. Do not add more stations as compensation. |
| B. Is the batch rhythm comfortable? | Same loop with one versus three sockets and two bowls; six to twelve peppers per run. | Increased capacity feels better, with useful interleaving and manageable errors. | Widen grace, simplify transfers, or reduce active sockets. |
| C. How much peeling belongs? | Comparable batches with brief manual peeling versus the representative-pepper handoff. | Whether the reveal stays enjoyable after repetition, and whether assistance preserves satisfaction. | Keep the preferred amount; do not defend manual repetition for realism alone. |
| D. Is packing a second reward? | Six prepared peppers into two jars, handoff transition, labeling, shelf placement. | Voluntary inspection/praise of output; low confusion about finished versus merely packed jars. | Simplify inputs or use tray-level packing. |
| E. Does lyutenitsa earn its cost? | One prepared batch, one base, short grinder/pan/fill sequence. | A distinct enjoyable transformation without a large rise in support work. | Use the documented J7/J8 fallback; remove grinder/pan. |
| F. Does a finite arc feel complete? | Three representative jobs with one upgrade and a temporary ending. | Players notice reduced chores, appreciate the finished display, and believe the stated final job. | Fix pacing and payoff before authoring all eight jobs. |

Reuse the shared scorecard's six-player approach where practical: log actual build/version, action ratings, input failures, assistance, boredom, and optional continuation. The new experiments change tasks, so do not insert their numbers into the original three-concept comparison as if they were the same protocol.

For each batch test, use the existing provisional gates as a reference: median ordinary-action appeal at least 4/5, at least four of six accepting a short unrewarded continuation, support friction no more than 20%, and no unresolved completion blockers. Also ask what the player actually enjoyed. These small samples are directional evidence, not commercial validation.

Compare like quantities before/after an upgrade. Count support actions as well as seconds; completing more food with the same amount of busywork is not automatically an improvement. Rotate test order when comparing two modes so familiarity does not masquerade as preference.

Declare an effort cap before each future experiment. Allow at most two bounded revisions for a clearly identified issue. If the everyday action still fails, stop expanding the concept instead of rescuing it with story volume.

## Risks and design responses

| Risk | Early symptom | Response |
| --- | --- | --- |
| Covered appliance hides all useful information | Players repeatedly lift at random or rely entirely on a countdown. | Improve inspection and audio; observe a real session before finalizing cues. |
| Three sockets create stress | Players abandon all other work or dread readiness sounds. | Longer windows and optional fewer sockets; resting/packing never become urgent clocks. |
| Peel drags feel like a progress bar | Players ask to skip by the third pepper. | Shorter physical reveal or batch assistance; retain only the part they enjoy. |
| New product adds chores | Players like the jars but dislike every intermediate step. | Compress or cut lyutenitsa; keep one excellent process. |
| Upgrades just bring more peppers | Time and support actions per comparable job do not improve. | Hold quantities steady during comparison; preserve real efficiency gains. |
| Comedy undermines trust | Players doubt the final job or resent surprise refills. | Fixed accepted counts, visible campaign length, no post-ending obligation. |
| End reward disappoints | Cellar speculation outweighs interest in finished work. | Set modest expectations and emphasize food, family recognition, and keepsake. |
| Solo scope grows through “small” additions | Each job needs a bespoke subsystem or character behavior. | Reuse existing verbs, cut optional events, keep one property. |

## What is settled and what is still unknown

**Settled for this proposal:** outdoor work; one property; finite family list; jars as the main output; humorous understatement; processing as the structural model; a real completion scene; no code in this task.

**Default choices awaiting play:** roasting as the main skill, short manual peeling with optional assistance, three urgent sockets, milestone rewards, and the eight-job campaign.

**Conditional additions:** lyutenitsa, curved-pepper repositioning, portraits, optional remixes, and cosmetic events. Each must earn its place through the intended pleasure, not because it sounds amusing in a document.

**Unknown:** whether people want another batch, which action they enjoy most, final duration, integration effort, and market demand. The plan defines a coherent game to evaluate; a production commitment needs that evidence.
