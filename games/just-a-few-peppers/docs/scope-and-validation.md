# Scope and validation

[Design index](readme.md) · Just a few peppers · v4 focused first-game scope · untested

**Build one enjoyable handling loop with a clear beginning, useful upgrades, and an ending.** The initial experiment remains small. The first complete game adds a compact yard and cultural presentation around the same loop.

## Scope contract

| Area | First-version cap |
| --- | --- |
| World | One yard with up to five connected pockets, one shed view, one small cellar view, and a street backdrop. |
| Actions | Scoop, carry, tip/place, and activate exposed equipment. |
| Raw handling | One active carrier: 12-unit crate, upgraded to 48-unit wheelbarrow. |
| Processing | One automatic line with three authored equipment stages, one input, and one output type. |
| Finished handling | One reusable output carrier and one always-accessible storage rack. |
| Food | One sound pepper class and roasted-pepper jars. Cosmetic variation does not create rules. |
| Progress | Finite supply, equipment discoveries, one stored-food total, and a completed-day state. |
| Household | Static cultural props, four food-display states derived from stored progress, and one automatic meal transition. |
| Characters | One seated Grandpa, limited gestures, up to about 24 short lines, and optional offscreen family audio. |
| Ending | Harvest cleared and stored, then Finish the day. No additional favors or supply loop. |
| Representation | Authored pile depletion and bounded decorative motion as the first approach. Individual pepper physics is not assumed. |

Excluded from this version: recipe selection, sorting, a grinder branch, a second product, recipient inventories, food reassignment, temporary storage relocation, jar-return tasks, table chores, task checklists, collectible tracking, NPC work schedules, currency, shops, farming, driving, crafting, repairs, conveyor layout, cooking quality, and rakia production.

These cuts are not queued as automatic follow-up milestones after the prototype. The previous 2–3 minute household-task target and five-minute ceiling no longer apply because those tasks are removed.

## Where the complexity is worth spending

Spend effort on responsive local pile depletion, a satisfying scoop-to-carrier-to-dump transition, comfortable wheelbarrow movement, and upgrades that save time across the whole job. Keep enough state to avoid lost contents, broken saves, or an unfinished last batch. Removing recipes does not remove that basic reliability requirement.

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

Use a small outdoor corner, authored mound, crate gathering/tipping, one automatic station, one reusable output carrier, one storage rack, and a partly concealed wheelbarrow. Start with the crate available. No full property, final machine, dialogue sequence, or household presentation is needed.

First prove repeated crate loads. Then expose the wheelbarrow and matching station capacity so it can be compared on the same amount of work. Record implementation effort and test results separately from the historical roasting spike.

| Question | Evidence to collect | Response if it fails |
| --- | --- | --- |
| Is the ordinary action enjoyable? | Responsive local depletion, clear carried volume, satisfying dump, and voluntary repetition without a reveal reward. | Improve control/representation before adding content; use at most two bounded revisions before reconsidering the approach. |
| Does a visible tool encourage clearing? | Players notice the wheel and deliberately approach it. | Improve the clue and placement. |
| Does the upgrade help the whole job? | Compare 48 units as four crate deliveries versus one wheelbarrow delivery, including processing and output storage. | Fix throughput, handling, or distance; capacity alone is not success. |
| Is storing food satisfying enough? | Players connect the deposit to completed work; count output trips, empty walking, and prompts. | Shorten the route and strengthen batch feedback; reconsider the handoff if it remains a chore. |
| Can the section finish reliably? | Partial loads and last batches resolve, contents survive recovery/save, and completion is reachable. | Fix the broken transfer before interpreting enjoyment. |

Where practical, borrow the [shared scorecard](../../../research/concepts/prototypes/prototype-comparison-scorecard.md)'s six-player approach. Provisional gates remain median ordinary-action enjoyment at least 4/5, at least four of six choosing a brief unrewarded continuation, median forced waiting/support friction at most 20%, and zero unresolved completion blockers. Ask whether players liked the action itself, the joke, or the discoveries. Small samples guide iteration; they do not validate demand.

## Later checks for the complete game

After the core works, compare the modified and final stations on the same 96 units, with the same wheelbarrow and path. Verify that accumulating output reduces collection trips without increasing forced waiting. Assess any shortcut separately.

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
- The yard is empty but unfinished food remains; readiness waits for its deposit.
- Repeating an empty deposit or reloading the ending cannot duplicate progress.

These are future acceptance cases. This documentation revision produces no code, build, new player observations, or changes to historical prototype scores.
