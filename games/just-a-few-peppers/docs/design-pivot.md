# Design decisions: Just a few peppers

[Design index](readme.md) · revision history through current implementation · scope boundary September 6, 2026

## Current revision: one loop, a household story

The user wants a simple, fun first game and expects AI to implement it. The supplied clean-up prompt defines the production target around bulk handling, useful discoveries, winter preparation, cultural presentation, and Grandpa's machinery. This is an in-place documentation update, not a new implementation direction.

**The current design is authoritative.** Earlier proposals and research recommendations do not add features to its [scope contract](scope-and-validation.md#scope-contract).

| Earlier prototype system | Current design decision |
| --- | --- |
| Normal and irregular pepper classes | One sound pepper class; appearance can vary cosmetically. |
| Whole-pepper and lyutenitsa routes | One automatic line producing roasted-pepper jars. Grinder and lyutenitsa remain scenery. |
| Grandpa/Aunt/city inventories and reassignment | One permanent rack deposit increases one stored-food total; family destinations are visual. |
| Returned-jar objective and supply handling | Static jar props, with no return state or economy. |
| Manual table preparation | Automatic table/meal transition after all harvest is cleared and stored, with no Finish button. |
| Temporary outdoor stock moved into the cellar | One storage handoff available from the start, with no later relocation. |
| Separate household readiness conditions | All finite harvest cleared and stored automatically completes the day. |
| Up to five minutes of household tasks | Zero added household tasks; the former pacing allowance is retired. |
| Optional favors and collectible tracking | Excluded from the first complete game. |

The [household presentation spec](household-readiness-and-parcels.md) retains the cellar, two labelled family boxes, returned jars, old tools, and final meal. Their appearance derives from the same stored progress or completed-day state. The attachment's example milestones based on clearing are adapted to **stored food**, so a cleared but unprocessed yard cannot falsely produce filled shelves.

Crate → wheelbarrow and familiar appliance → modified loader → final processor remain. The [equipment stages](yard-and-progression.md#grandpas-three-equipment-stages) share one logical input, output, and controls. The final processor's larger buffers/fewer output trips and strong spectacle combine with a substantially shorter final-supply haul through a fixed nearby intake. It must measurably improve the complete scoop-to-storage job with the same wheelbarrow.

The immediate M1–M2 interaction test stays one outdoor corner, one authored mound, crate, broad scoop, automatic processor, reusable finished-food carrier, Finished Food Handoff Rack, and partly exposed wheelbarrow with comparable work remaining after unlock. Production scope does not establish implementation evidence or proof of fun.

## Scope-lock audit — September 6, 2026

Contradictions removed: voluntary Finish/table actions versus an automatic meal; generic rack storage versus the player's final household handoff; fixed-path capacity-only final-upgrade checks versus a substantial hauling-route gain; a five-area sketch versus a measured maximum; Bulgarian/English duplicate dialogue drafts versus one English bank. The jokes, triggers, and eventual localization/native review remain. The eight prototype questions are explicit, and their reliability check does not pull M3 saves or M4–M5 presentation into the first interaction test.

Active first-version systems remain movement, finite local pile clearing, crate-to-wheelbarrow handling, broad scoop/dump, one automatic line with three tiers, one reusable finished carrier, one permanent handoff, one stored-food total, useful equipment/path discoveries, recovery/saving, and automatic completed-day/meal presentation. The meal follows the final valid deposit without a new verb. Existing sprint/jump controls are retained.

Presentation retains the winter-preparation story, cellar filling, labelled family boxes, returned jars, lyutenitsa/older cellar foods, old refrigerator/tool cupboard, decorative grinder, reused tools, vine table, family meal, Grandpa's bottle/gift and understated humor. Household distribution reads stored food; no player or helper transports food between these displays. The [excluded-system list](scope-and-validation.md#scope-contract) stays explicit, including all removed earlier rules, chores, economies, factory construction, NPC workers, favors, and collectible progression.

Unresolved prototype risks: satisfying scoop/fill/dump feedback and convincing local depletion; a motivating wheelbarrow reveal with a dramatic equal-work gain; finished-food handling that feels like a payoff rather than another hauling chore; and reliable full/partial transfers and completion. Later measured risks remain the combined final-intake/output benefit, reveal timing with repeated work left, a small enough pocket count, and readable cultural presentation/automatic ending. None is a reason to restore processing rules or household tasks.

Development contracts now identify the single handoff owner, tier-derived intake restoration, last-deposit completion transaction, interrupted-ending recovery, matched workflow measurements, and content boundaries. Task IDs, ordering, delivery/feedback statuses, review gates, and past execution evidence are preserved. Existing `Storage rack` names in the delivered foundation and its historical records describe the current artifact; 1_04 will update its actual label when implementing the handoff. No Unity source, assets, build, package, or shared research files change in this pass.

Verification: checked 54 active Markdown files and 549 local links/heading targets, English draft text, UTF-8, code fences, and whitespace. Compared all 31 queue entries against their previous IDs/order/kinds/statuses/feedback/evidence; retained all 17 English joke/alternative lines, the delivered movement contract, historical delivery records, and source URLs. The file list below matches the complete diff. No Unity or legacy tests were run for this documentation-only change.

Files changed in this documentation pass (34 existing Markdown files; none added or removed):

| Group | Files |
| --- | --- |
| Entry and ownership | [AGENTS.md](../../../AGENTS.md), [game readme](../readme.md), [ARCHITECTURE.md](../ARCHITECTURE.md). |
| Core design | [docs readme](readme.md), [core loop](core-loop-and-mechanics.md), [scope/validation](scope-and-validation.md), [yard/progression](yard-and-progression.md), [this revision/audit](design-pivot.md). |
| Household, writing, presentation | [household/ending](household-readiness-and-parcels.md), [story](story-and-characters.md), [English dialogue bank](jobs-events-and-comedy.md), [look/sound/comfort](look-sound-and-comfort.md), [active authenticity notes](research-and-authenticity.md). |
| Development contracts | [M1 contract](development/first-playable-task.md), [roadmap](development/roadmap.md), [state/saving](development/state-and-saving.md), [testing](development/testing-and-performance.md), [status history](development/status.md), [task queue](development/tasks/readme.md). |
| Prototype briefs and label references | [1_03](development/tasks/1_03_tipping-and-automatic-processing.md), [1_04](development/tasks/1_04_finished-carrier-and-storage-rack.md), [1_05](development/tasks/1_05_first-playable-comfort-and-handoff.md), [2_01](development/tasks/2_01_wheelbarrow-discovery-and-loader.md), [2_03](development/tasks/2_03_core-feel-playtest-gate.md). |
| Full graybox briefs | [4_01](development/tasks/4_01_connected-graybox-yard.md), [4_02](development/tasks/4_02_final-processor-and-upgrade-order.md), [4_03](development/tasks/4_03_harvest-completion-and-ending-state.md). |
| Presentation and pacing briefs | [5_02](development/tasks/5_02_handling-and-machine-presentation.md), [5_03](development/tasks/5_03_winter-food-and-family-displays.md), [5_04](development/tasks/5_04_grandpa-and-meal-transition.md), [6_01](development/tasks/6_01_finish-the-compact-property.md), [6_02](development/tasks/6_02_campaign-pacing-and-dialogue-pass.md). |
| Guidance and reliability briefs | [7_03](development/tasks/7_03_audio-display-and-guidance.md), [8_01](development/tasks/8_01_full-game-reliability.md). |

## How the design arrived here

| Revision | Main idea | Lesson carried forward |
| --- | --- | --- |
| Roasting prototype (discarded) | Individual roasting, peeling, and a cooking workday. | Recognizable food transformation and appliance character matter. |
| Bulk prototype | Finite piles, larger loads, equipment reveals, cellar progress. | Gathering and dumping must be enjoyable between discoveries. |
| Household prototype | Two products, family allocation, returned jars, table task, meal. | The family gives the work purpose; short tasks still add interacting states. |
| Focused current implementation | One food loop, useful upgrades, household presentation, automatic meal. | Preserve the purpose while concentrating implementation and testing on the main action. |

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

