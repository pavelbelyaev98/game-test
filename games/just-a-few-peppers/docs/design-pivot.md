# Design decisions: Just a few peppers

[Design index](readme.md) · revision history through v4, September 5, 2026

## Current revision: one loop, a household story

The user wants a simple, fun first game and expects AI to implement it. The latest supplied conversation supports removing interactive household obligations while retaining winter preparation, cultural objects, useful discoveries, and Grandpa's machinery.

**V4 is the current design authority.** Earlier proposals and research recommendations do not add features to its [scope contract](scope-and-validation.md#scope-contract).

| Earlier v3 system | Current v4 decision |
| --- | --- |
| Normal and irregular pepper classes | One sound pepper class; appearance can vary cosmetically. |
| Whole-pepper and lyutenitsa routes | One automatic line producing roasted-pepper jars. Grinder and lyutenitsa remain scenery. |
| Grandpa/Aunt/city inventories and reassignment | One permanent rack deposit increases one stored-food total; family destinations are visual. |
| Returned-jar objective and supply handling | Static jar props, with no return state or economy. |
| Manual table preparation | Automatic table/meal transition after Finish the day. |
| Temporary outdoor stock moved into the cellar | One storage handoff available from the start, with no later relocation. |
| Separate household readiness conditions | All finite harvest cleared and stored, then the player's ending action. |
| Up to five minutes of household tasks | Zero added household tasks; the former pacing allowance is retired. |
| Optional favors and collectible tracking | Excluded from the first complete game. |

The [household presentation spec](household-readiness-and-parcels.md) retains the cellar, two labelled family boxes, returned jars, old tools, and final meal. Their appearance derives from the same stored progress or completed-day state. The attachment's example milestones based on clearing are adapted to **stored food**, so a cleared but unprocessed yard cannot falsely produce filled shelves.

Crate → wheelbarrow and familiar appliance → modified loader → final processor remain. The [equipment stages](yard-and-progression.md#grandpas-three-equipment-stages) share input, output, and controls. Capacity and spectacle must improve the complete job.

The next test stays one mound, crate, automatic processor, output carrier, storage rack, and partly exposed wheelbarrow. This is a scope decision, not production approval, implementation evidence, or proof of fun.

## How the design arrived here

| Revision | Main idea | Lesson carried forward |
| --- | --- | --- |
| Roasting v1, discarded | Individual roasting, peeling, and a cooking workday. | Recognizable food transformation and appliance character matter. |
| Bulk v2 | Finite piles, larger loads, equipment reveals, cellar progress. | Gathering and dumping must be enjoyable between discoveries. |
| Household v3 | Two products, family allocation, returned jars, table task, meal. | The family gives the work purpose; short tasks still add interacting states. |
| Focused v4, current | One food loop, useful upgrades, household presentation, automatic meal. | Preserve the purpose while concentrating implementation and testing on the main action. |

The earlier two-product and household-task rules are superseded, including their alternate-order inventories and timing gates. They are not deferred commitments.

## Research retained

The supplied [comparable-game memo](../../../research/case-studies/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md) warns about repeated processing chains, hauling fatigue, rigid tasks, and progress failures. The [culture memo](../../../research/culture/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md) supplies winter preserving, food circulation, reused objects, and hospitality. Original source notes remain unchanged; reception figures and engineering-cost guesses are not new measurements.

Leaf it Alone's official description links clearing with useful tools and things uncovered. That supports discovery through removal. [Official page](https://store.steampowered.com/app/3981100/)

Cash Cleaner's official description includes physical deliveries, processing equipment, efficiency upgrades, and workspace secrets. This informs batch handling and visible output without requiring every processing rule in this game. [Official page](https://store.steampowered.com/app/2488370/)

A Game About Digging a Hole links collecting, equipment upgrades, further access, and a mystery. Its clear progression is the reference for the desired focus; this does not establish equivalent development cost or likely reception. [Official page](https://store.steampowered.com/app/3244220/)

These are previously checked descriptions and design inferences. The revision does not refresh market statistics or establish what caused another game's success.

## The remaining risk

The small version can still become repetitive container walking. Responsive piles, satisfying transfers, useful route choices, and real gains from upgrades must carry it. More jokes, food types, or mandatory supply cannot rescue weak handling.

AI implementation still needs build verification and play observations. Presentation has an asset and integration cost even when it has no task system. The [validation plan](scope-and-validation.md) concentrates that work on the remaining uncertainty.
