# Scope and validation

[Design index](readme.md) · Just a few peppers · v4 focused first-game scope · untested

**Build one enjoyable handling loop with a clear beginning, useful upgrades, and an ending.** The initial experiment remains small. The first complete game adds a compact yard and cultural presentation around the same loop.

## Scope contract

| Area | First-version cap |
| --- | --- |
| World | One finite outdoor yard with at most five connected pockets, one shed view, one small cellar view, and a street backdrop. Select the count after the core prototype is measured; three or four areas need no fifth. |
| Actions | Scoop, carry, tip/place, and activate exposed equipment. |
| Raw handling | One active carrier: 12-unit crate, upgraded to 48-unit wheelbarrow. |
| Processing | One automatic line with three authored equipment stages, one input, and one output type. |
| Finished handling | One reusable output carrier and one always-accessible Finished Food Handoff Rack; every finished load ends the player's handling responsibility there. |
| Food | One sound pepper class and roasted-pepper jars. Cosmetic variation does not create rules. |
| Progress | Finite supply, equipment discoveries, one stored-food total, and a completed-day state. |
| Household | Static cultural props, four food-display states derived from stored progress, and one automatic meal transition. |
| Characters | One seated Grandpa, limited gestures, up to about 24 short English draft lines, and optional offscreen family audio. Eventual localization/native review remains later presentation work. |
| Ending | All harvest cleared and stored automatically starts the family meal. No separate finish action, additional favors, or supply loop. |
| Representation | Authored pile depletion and bounded decorative motion as the first approach. Individual pepper physics is not assumed. |

Excluded from this version: individual roasting judgment, peeling gameplay, recipe selection, pepper sorting, functional pepper classes, lyutenitsa production gameplay, grinder gameplay, a second product route, family recipient inventories, parcel allocation, food reassignment, temporary storage relocation, jar-return tasks, table-preparation chores, task checklists, currency, shops, farming, driving, crafting, repairs, conveyors/factory construction, NPC worker AI, post-game favors, collectible progression, and rakia production. There is no carrier beyond the wheelbarrow or powered clearing tool.

Lyutenitsa, returned jars, family boxes, the decorative grinder, cellar foods, old refrigerator/tool cupboard, reused tools, vine table, Grandpa's bottle/gift, and understated humor remain presentation. A few authored household display states read the same stored-food total; no secondary storage or distribution model is permitted.

These cuts are not queued as automatic follow-up milestones after the prototype. The previous 2–3 minute household-task target and five-minute ceiling no longer apply because those tasks are removed.

## Where the complexity is worth spending

Spend effort on responsive local pile depletion, a satisfying scoop-to-carrier-to-dump transition, comfortable wheelbarrow movement, and upgrades that save time across the whole job. Keep enough state to avoid lost contents, broken saves, or an unfinished last batch. Removing recipes does not remove that basic reliability requirement.

The design's depth is **visible transformation + increased power + spatial discovery + cultural personality**, not more processing rules. A requirement should improve scoop, dump, reveal, upgrade, or visible payoff. Keep other cultural details as scenery, and do not restore V3 systems through later development tasks.

AI is expected to implement and iterate on the game. Its ability to generate code does not demonstrate enjoyable controls, convincing motion, performance, or correct integration. Judge the delivered build through measured play and bounded checks. No engineering-time percentages, guaranteed completion date, or claim that cultural presentation is free is adopted from the supplied conversation.

Art still needs production and review. Reuse a small prop set, equipment materials, jar groups, and authored display states. Keep Grandpa seated and make household displays derive from one progress value.

## What the research contributes

| Input | Decision retained in this smaller version |
| --- | --- |
| [Leaf it Alone / Librarian study](../../../research/case-studies/Leaf_it_Alone_and_Librarian_Case_Study.md) | Clearing reveals useful space and equipment; the transformed workspace is a reward. |
| [Cash Cleaner study](../../../research/case-studies/Cash_Cleaner_Simulator_Case_Study.md) | Physical loads, satisfying transfers, visible finished output, and upgrades measured across the complete workflow. |
| [Meta-study](../../../research/blueprints/Simulator_Games_Meta_Study_20_Games_Blueprint.md) and [Drywall study](../../../research/case-studies/Drywall_Eating_Simulator_Case_Study.md) | Separate the premise's joke from repeatable enjoyment; remove mastered friction and progress blockers. |
| [Comparable-game memo](../../../research/case-studies/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md) | Avoid repetitive processing branches, long hauling, and rigid task order; express culture through a specific place. |
| [Culture memo](../../../research/culture/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md) | Retain winter food, jars, parcels, reused tools, and hospitality as presentation around the work. |

These are design inferences, not evidence that the prototype will be fun or a claim that this game's development effort matches another title.

## Next experiment: one pile, one carrier, one discovery

Use one small outdoor corner, one authored mound, the immediately available crate, broad scoop/dump, one automatic station, one reusable finished-food carrier, one Finished Food Handoff Rack, and a partly concealed wheelbarrow. Leave a comparable amount of material after its unlock. M1 builds the crate loop; M2 adds the wheelbarrow discovery/comparison in this same corner. Local depletion/discovery regions may subdivide the mound without adding separate yard areas.

Grandpa dialogue, household display states, final machine, cellar, parcels, grinder, and ending scene are explicitly outside this interaction prototype. A plain completed-section indication can verify complete storage without importing the full game's presentation.

First prove repeated crate loads. Then expose the wheelbarrow and matching station capacity so it can be compared on the same amount of work. Record implementation effort and test results separately from the historical roasting spike.

| Question | Evidence to collect | Response if it fails |
| --- | --- | --- |
| 1. Does scooping feel satisfying? | Ordinary-action rating and voluntary repetition without a reveal reward. | Improve handling before adding content; retain the existing limit of two bounded feel revisions. |
| 2. Does the visible pile locally change convincingly? | Observe the touched face, exposed ground, and remaining clumps throughout a load. | Improve the authored depletion representation; a shrinking number alone is insufficient. |
| 3. Is filling a carrier satisfying? | Visible volume responds to scoops without obscuring the route or requiring individual pepper placement. | Improve transfer/readability before adding more piles. |
| 4. Is dumping satisfying? | One broad action produces a substantial, readable cascade with immediate accepted-load feedback. | Improve target, timing, and presentation; add no processing rule. |
| 5. Does the emerging wheelbarrow motivate clearing? | Players notice the wheel and deliberately clear toward it. | Improve clue and placement. |
| 6. Does the wheelbarrow make the same work dramatically better? | Compare 48 units as four crate deliveries versus one wheelbarrow delivery, through final storage; record comparable supply after unlock. | Fix throughput, handling, or distance; capacity alone is not success. |
| 7. Is storing one finished batch satisfying rather than an extra chore? | Count output trips, empty walking, and prompts; check that the one handoff reads as completion. | Shorten the rack route and strengthen batch feedback within the same one-destination rule. |
| 8. Can the whole section finish reliably? | Repeated and partial final loads complete, and recovery preserves contents. | Fix transfers before interpreting enjoyment. Disk saves remain M3, not a prototype prerequisite. |

Where practical, borrow the [shared scorecard](../../../research/concepts/prototypes/prototype-comparison-scorecard.md)'s six-player approach. Provisional gates remain median ordinary-action enjoyment at least 4/5, at least four of six choosing a brief unrewarded continuation, median forced waiting/support friction at most 20%, and zero unresolved completion blockers. Ask whether players liked the action itself, the joke, or the discoveries. Small samples guide iteration; they do not validate demand.

## Later checks for the complete game

After the core gate, measure the **combined final upgrade**, including its authored layout benefit:

1. Use the same 96 units from the same final-supply location, the same 48-unit wheelbarrow/scoop settings, and the same permanent handoff rack. The baseline uses the modified station's original intake; the upgraded run uses the revealed nearby fixed intake and 96-unit buffer/output. Start from comparable empty carriers/buffers and end with all 96 units stored.
2. Record loaded travel distance/time, scooping, tipping, forced waiting, finished-output pickup/deposit trips, empty walking, and complete elapsed workflow time. Report absolute and percentage changes over matched repeated trials, with build/layout, quantities, route, and timing method recorded in 4_02's delivery evidence.
3. Require a substantially shorter loaded route, fewer output collections when combining two loads, and a measurable reduction in the complete scoop-to-storage job. A tiny change within trial variability or a larger capacity number alone does not pass. A fixed-path timing test may diagnose machinery separately but cannot replace the combined comparison.
4. Record supply remaining at reveal and repeated complete cycles available on the intended approach. If the upgrade appears too late or simply moves the burden into output hauling/waiting, revise its placement, route, or pacing before adding content. Do not add supply, another tool, queue, resource, construction step, or UI subsystem to manufacture the improvement.

Then check the small yard arc and household display together: players may discover paths in different orders; stored food must explain the cellar and parcels without suggesting new tasks; the final machine must arrive while useful work remains; and the meal should make the day feel complete.

Measure the enjoyable length of a representative section before choosing final quantities. The illustrative 732 units do not establish campaign duration. Do not add supply, machine delays, recipes, or errands to extend playtime.

## Essential reliability cases

- A partial load is tipped into limited input space or the action is cancelled.
- Output accumulates while the finished carrier is away, pauses when full, and resumes safely.
- The last batch is smaller than the machine or a visible jar group.
- A loaded carrier is recovered, or an equipment upgrade occurs with contents in transit.
- The final station is discovered before the wheelbarrow; a later rack cannot downgrade capacity.
- The game resumes with raw, processing, carried finished food, or stored progress; displays rebuild without replaying rewards.
- A large deposit skips display milestones or the cellar view opens after earlier deposits.
- The yard is empty but unfinished food remains; ending waits for its deposit. The last accepted deposit automatically completes the day, including a partial final batch.
- Pause or reload during the automatic transition restores a coherent completed-day presentation without another Finish command, reward, or household obligation.
- Repeating an empty deposit or reloading the ending cannot duplicate progress.

These are future acceptance cases. This documentation revision produces no code, build, new player observations, or changes to historical prototype scores.
