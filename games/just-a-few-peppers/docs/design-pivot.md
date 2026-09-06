# Design decisions: Just a few peppers

[Design index](readme.md) · current decision September 6, 2026 · earlier records preserved below

## Snap installation and restrained comic variety — September 6, 2026

This is a targeted documentation refinement of the current design, following the developer's supplied play-feedback amendment. The preceding food/machinery/Coins revision was already present in the repository's contracts and queue. Food preparation and permanent winter stock remain the objective; physical batch handling and directly operated machinery are the recurring play. One Coins balance earned at handoff funds two meaningful initial offers at one bench. Stored food is cumulative and spending never reduces it. Equipment is not primarily discovered, essential yard access is open, physics is chosen per object type with recoverable contents, and the final food handoff leaves normal control. No business, property-restoration, cellar-sorting, clock/bedtime or compulsory meal progression is restored. These are documented requirements, not implemented or accepted prototype behavior.

### Firsthand source and its limits

The developer reports **approximately 20 minutes** in the garden-construction reference game. These are personal early-session observations, not universal findings, a complete review or player acceptance of Just a few peppers.

| Reported response | Firsthand observation |
| --- | --- |
| Enjoyed | An immediately funny optional computer-chat gag, assembling objects, and predetermined construction positions where parts were easy to place. |
| Disliked | Remembering requirements/working out what to buy, repeated long-distance hauling of individual objects, confusion between personal and fund balances, and humor becoming repetitive or excessive. |
| Interested, not committed | Bringing a machine drawing into existence, occasional personality/story messages or events, and day/night presentation. |

The developer does not want construction to become the primary activity and has not requested stress management. The earlier reference ranking remains; this feedback refines a small installation payoff and comic restraint without importing the reference game's wider systems.

### Decisions and existing owners

Sequence remains **physical batch-processing/handoff loop → approved one-wallet/two-purchase flow → one short snap installation → comparable repeated work and assessment**. The assisted-loading purchase supplies the prototype's one complete physical kit beside a large forgiving mount; the capacity package adds no assembly step. Full price includes the attachment, and the same bench shows workflow effect, availability/owned-awaiting-installation/installed state and location. No shopping list or separate wallet/catalogue is introduced.

Paid ownership survives placement interruption and lost-kit recovery. One stable kit/effect installs once; the old apparatus works while pending, active food survives and the safe boundary is visible. Model/in-memory checks belong to 2_01; the existing M3 tasks add persistence. Measure installation finding/understanding, placement attempts/rejections and boundary wait separately from recurring batch handling/travel and useful work remaining. Keep all supplies/kit/handoff nearby and do not improve ratios through slower gathering, timers or extra mandatory supply.

At most one inexpensive optional original static gag may accompany the prototype upgrade test without a computer/mail prerequisite. Ordinary batch enjoyment is judged separately. Later selected messages, labels and brief authored reactions share the existing writing budget, remain readable later without stopping work, and leave instructions visible and long quiet gaps. No replies, expiry, quotas, surprise payments, stacked joke replay, real chatbot, copied branded interface or general random-event scheduler is needed.

Drawing an authored outline into its already-defined attachment remains an optional later presentation alternative; do not stack it with mandatory assembly. A later hero machine's few chunky modules require worthwhile prototype evidence. Atmospheric lighting remains optional and changes no progression. These candidates create no automatic tasks, stress/rest/bedtime rules or new completion actions.

| Changed contracts/files | Refinement |
| --- | --- |
| [AGENTS.md](../../../AGENTS.md), [design index](readme.md), [queue](development/tasks/readme.md), [roadmap](development/roadmap.md), [status history](development/status.md) and this decision record | Discoverable bounded scope and existing ownership; no new task or advancement. |
| [Core mechanics](core-loop-and-mechanics.md#attach-a-purchased-improvement), [yard/progression](yard-and-progression.md), [scope/validation](scope-and-validation.md) | Complete purchase cards, one kit/mount exception, nearby batch work, required prototype versus optional presentation. |
| [Architecture](../ARCHITECTURE.md), [state/saving](development/state-and-saving.md#attachment-ownership-and-installation) | One paid entitlement/kit, pending fitting, exactly-once install and later coherent reconstruction. |
| [Look/sound](look-sound-and-comfort.md), [comedy](jobs-events-and-comedy.md#optional-messages-and-comic-variety), [story](story-and-characters.md) | Snap feedback, restrained original variety, shared writing budget and bounded drawing/lighting candidates. |
| [Testing](development/testing-and-performance.md#purchased-attachment-checks) | Deterministic lifecycle/recovery, player fitting, separated one-time/recurring measurements and later message checks. |
| [2_01](development/tasks/2_01_wheelbarrow-discovery-and-loader.md), [2_02](development/tasks/2_02_upgrade-throughput-and-handling.md), [2_03](development/tasks/2_03_core-feel-playtest-gate.md) | Implement the existing purchase's snap, measure it and assess it alongside meaningful repeated improvement use. |
| [3_01](development/tasks/3_01_snapshots-and-in-scene-restore.md), [3_02](development/tasks/3_02_local-save-and-continue.md), [3_03](development/tasks/3_03_save-failure-recovery.md) | Loose/held/recovered/fitted-pending/installed save and corrupt/duplicate-kit recovery cases. |
| [4_02](development/tasks/4_02_final-processor-and-upgrade-order.md), [5_02](development/tasks/5_02_handling-and-machine-presentation.md), [5_04](development/tasks/5_04_grandpa-and-meal-transition.md), [5_05](development/tasks/5_05_representative-slice-playtest-gate.md), [6_02](development/tasks/6_02_campaign-pacing-and-dialogue-pass.md) | Preserve bounded installation in later equipment/art, implement selected comic variety and evaluate clarity, quiet pacing and rereading. |

No new task is needed: 2_01 already owns the attachment purchase. Stable IDs, order, delivery/feedback values and historical evidence remain unchanged. The current executable is still the mat-based automatic-processing prototype, and NEXT remains the 1_02 placement correction. Exact prices/effects, mount tolerance and useful remaining supply are implementation/test settings in 2_01–2_02, not an unresolved currency decision. Optional drawing, lighting or larger-module presentation can remain unchosen without blocking the prototype.

Documentation verification passed across 59 Markdown files, 740 local links and 266 heading/compatibility-anchor targets, with valid UTF-8 and balanced fences. All 34 queue rows are unchanged, including order, delivery and feedback; dependencies, next links, required brief sections and the nine prototype questions remain valid. The 1_01–1_03 briefs, delivered controls/backend descriptions, earlier decision/task history and 17 original dialogue quotes were preserved. A fresh start-of-refinement hash/file-set comparison found all 224 protected Unity source/assets/configuration/tools, research and key build files unchanged; whitespace and asset/meta checks passed. This pass updates 26 existing documents and adds no task or specification file. Local checks/baselines remain ignored under `unity/Logs/snap-install-docs-check.txt`, `check_snap_install_docs.py` and `snap-install-*.json`. No Unity tests, rebuild or human playtest ran; technical readiness and fun are not newly established.

## Food, machinery and Coins — September 6, 2026

The developer supplied a new design conversation, then explicitly continued with the decision to include a minimal earn-and-spend equipment system in the prototype. The current direction is **physical batch handling + enjoyable directly operated machinery + chosen equipment progression + a growing winter-food stockpile**. Help Grandpa prepare an unreasonable amount of winter food with increasingly absurd homemade equipment.

This decision supersedes the earlier clearing/discovery emphasis and the pending discovery-versus-Coins experiment. The question about currency is resolved: Coins, two useful purchases and repeated improvement use belong before the revised prototype's human gate. This is planning authorization, not evidence that an implementation exists or was accepted.

| Earlier rule | Current decision |
| --- | --- |
| Clearing pockets exposes the next tool and unlocks useful yard routes. | Most of the compact yard and essential work locations are accessible from the start. Finite food use makes space as a consequence; equipment is purchased at the work area. |
| Visible clearing is the primary repeated action; machinery is an automatic line/timer. | Physically handle a batch, directly operate a substantial mechanism, then collect visible winter food. Compress internal preparation and avoid ten-step cooking or precise doneness. |
| Discovery-only baseline; Coins comparison awaits separate authorization after the gate. | Coins are required in the revised prototype: handoff earns budget, one nearby bench shows two meaningful offers, the player buys either and repeatedly uses it before 2_03. |
| Equipment can require physical discovery or a food-progress unlock. | Both initial offers/effects/prices are visible from the start; no hidden shop, yard access or prepared-food threshold followed by payment. Food milestones drive presentation only. |
| Physics is mostly cosmetic peppers plus freely handled carriers/props. | Choose simulation per object type. Important objects may be physical; test manageable physical pepper batches and grouped representation. Register/recover the same units without loss or duplicate progress. |
| Final machine requires a distant-supply intake and substantially shorter haul. | A powered conversion must change useful operation/material or output handling and improve the complete food job. Layout assistance is optional; no mandatory route reveal. |
| Lyutenitsa/rakia gameplay is permanently excluded. | Prove pepper machinery first, then consider at most one compact additional activity through an explicit scoped decision/task. No extra chain is currently authorized; rakia is not made from peppers. |

**Food and budget:** permanent stored winter food is separate from spendable Coins. Initial configuration earns one Coin per accepted pepper-equivalent unit at the committed handoff. Partial batches earn proportionally; split, repeated or empty deposits cannot earn extra. Purchases spend once and retain paid entitlement through safe installation/pause/save. Either order must be viable within the finite food/budget, with repeated use remaining. Starting equipment can finish without upgrades. There are no customers, food sales, recurring expenses or large upgrade tree.

**Two prototype improvements:** coordinated larger batch/carrier capacity (the wheelbarrow can be part of this equipment package) and an assisted loading/receiving mechanism that reduces useful operator effort/actions. 2_01 records exact real effects/costs, and 2_02 proves baseline/each-first/combined results. Do not sell speed when no wait bottleneck exists or make baseline controls irritating to sell relief. The supplied modest/converted/excessive examples guide the apparatus's escalation, not additional committed cooking steps or component hunts.

**References:** Cash Cleaner is the main physical handling/processing reference; Food Processing is the equipment-structure reference; the garden-invention game supplies absurd domestic escalation. Leaf it Alone is secondary for earning/upgrades, pacing and comfort, not the map template. The [scope research table](scope-and-validation.md#what-the-research-contributes) and existing [comparables memo](../../../research/case-studies/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md) record what is borrowed and omitted. This pass uses supplied conversations/local studies, without refreshing official-page claims, market statistics or other games' implementation details. Raw shared research is unchanged.

**Task mapping:** retain movement/input, free carrier correction, conservation, recovery and the delivered 1_03 automatic backend. 1_04 completes usable output/handoff; 1_05 remains an interim loop checkpoint and 1_06 shared prop play. Add [1_07 physical pepper comparison](development/tasks/1_07_physical-pepper-batch-comparison.md) and [1_08 direct machine operation](development/tasks/1_08_direct-machine-operation.md). Reframe stable [2_01](development/tasks/2_01_wheelbarrow-discovery-and-loader.md) around Coins/two working purchases and [2_02](development/tasks/2_02_upgrade-throughput-and-handling.md) around their comparison before 2_03. M3 covers mechanism/budget/paid ownership and physical saves; M4 is the accessible production work area and powered conversion. Later art, dialogue, input, reliability and media briefs follow this food/machinery direction.

The [queue](development/tasks/readme.md#ordered-task-queue) now has 34 tasks. Existing delivery and human-feedback values are preserved; new 1_07/1_08 are Todo / Not tested. NEXT still resumes the 1_02 placement correction. The existing executable remains the mat-based automatic-processing prototype; no new gameplay, build, asset import or human acceptance is supplied by this documentation pass.

Documentation verification passed across 59 Markdown files, 682 local links and 230 heading/compatibility-anchor targets, with valid UTF-8, balanced fences, 34 unique ordered task IDs, earlier dependency/next links and required brief sections. All previous task delivery/feedback values, the 1_01–1_03 execution histories and 17 original dialogue quotes were preserved. A start-of-pass hash comparison found all 224 protected Unity source/assets/configuration/tools, shared research and key build files unchanged. Whitespace checks passed. The pass revised 47 existing Markdown documents and added the two briefs; local check details remain ignored in `unity/Logs/food-machinery-docs-check.txt`. No Unity tests, rebuild or human playtest ran for this documentation-only revision.

## Earlier decisions retained as history

All sections below describe earlier proposals, scope locks and documentation passes. Their use of “current” refers to those earlier records; the food/machinery/Coins decision above governs future implementation. In particular, old discovery-only, deferred-Coins, locked-pocket and required final-haul clauses are superseded. Their sources and execution/check figures remain historical evidence, not results for this revision.

## Current revision: one loop, a household story

The developer wants a simple, fun first game and expects AI implementation. The supplied documentation amendments keep the production target around bulk handling, useful discoveries, winter preparation, cultural presentation, and Grandpa's machinery. This is an in-place clarification of the current design, not a new game.

**The current design is authoritative.** Earlier proposals and research recommendations do not add features to its [scope contract](scope-and-validation.md#scope-contract).

| Earlier prototype system | Current design decision |
| --- | --- |
| Normal and irregular pepper classes | One sound pepper class; appearance can vary cosmetically. |
| Whole-pepper and lyutenitsa routes | One automatic line producing roasted-pepper jars. Grinder and lyutenitsa remain scenery. |
| Grandpa/Aunt/city inventories and reassignment | One permanent rack deposit increases one stored-food total; family destinations are visual. |
| Returned-jar objective and supply handling | Jar props, including movable loose empties, with no return state or economy. |
| Manual table preparation | No table requirement. An optional table/gift tableau is cuttable presentation after harvest completion. |
| Temporary outdoor stock moved into the cellar | One storage handoff available from the start, with no later relocation. |
| Separate household readiness conditions | The final valid deposit commits harvest completion once every authored unit is permanently stored. |
| Up to five minutes of household tasks | Zero added household tasks; the former pacing allowance is retired. |
| Optional favors and collectible tracking | Excluded from the first complete game. |

The [household presentation spec](household-readiness-and-parcels.md) retains the cellar, two labelled family boxes, returned jars, old tools, vine table, and optional gift/table flourish. Their appearance derives from the same stored progress or completed property. Example milestones based on clearing are adapted to **stored food**, so a cleared but unprocessed yard cannot falsely produce filled shelves.

Crate → wheelbarrow and familiar appliance → modified loader → final processor remain. The [equipment stages](yard-and-progression.md#grandpas-three-equipment-stages) share one logical input, output, and controls. The final processor's larger buffers/fewer output trips and strong spectacle combine with a substantially shorter final-supply haul through a fixed nearby intake. It must measurably improve the complete scoop-to-storage job with the same wheelbarrow.

The immediate M1–M2 interaction test stays one outdoor corner, one authored pepper pile, crate, broad scoop, automatic processor, reusable finished-food carrier, Finished Food Handoff Rack, a small physical-prop sample, and partly exposed wheelbarrow with comparable work remaining after unlock. Production scope does not establish implementation evidence or proof of fun.

## Free handling and research review — September 6, 2026

The developer's play feedback rejected fixed parking mats and the inability to grab/place things naturally. They also asked what “mound” meant and why finished jars could not be used. The mat restriction made a physical yard feel like a sequence of permitted spots. Conserving food never required that restriction: logical quantities can coexist with freely moving carriers and props.

The [current handling contract](core-loop-and-mechanics.md#pick-up-place-and-play) now requires chosen placement based on geometry, rotation, careful stacking where stable, physical dropping/settling, deliberate small-prop tossing, and shared grab controls. Players can arrange temporary loads, move portable yard objects, take reachable shortcuts, and play without a new objective. Attached machinery remains fixed; stored-food displays remain derived views. Recovery and later pose saving preserve both food and the player's arrangements. Player guidance calls the source a pepper pile.

Reviewed all six local files in `research/case-studies`. The decisions below are design inferences from those existing notes, not refreshed market statistics, independently verified current versions, or evidence that this prototype is fun. Raw research is retained unchanged; its older broader-game proposals are not a hidden backlog.

| Local study and relevant passage | Decision for this game | Task ownership |
| --- | --- | --- |
| [Cash Cleaner: workplace/playground](../../../research/case-studies/Cash_Cleaner_Simulator_Case_Study.md#13-the-lab-is-both-workplace-and-playground), [physics clutter](../../../research/case-studies/Cash_Cleaner_Simulator_Case_Study.md#21-negative-reviews-physics-clutter), and [layout frustration](../../../research/case-studies/Cash_Cleaner_Simulator_Case_Study.md#23-negative-reviews-machine-layout-frustration) | Adopt a workspace the player can rearrange, clear pickup priority, forgiving optional alignment, and physical delight with separate food accounting. Recovery solves lost-object failures; it must not force all placement onto pads. Retain visible output as a payoff. | 1_02 revision, 1_04, 1_06; polish in 5_02. |
| [Leaf it Alone: emergent play](../../../research/case-studies/Leaf_it_Alone_and_Librarian_Case_Study.md#6-leaf-piles-create-emergent-play) and [Librarian: placing books neatly](../../../research/case-studies/Leaf_it_Alone_and_Librarian_Case_Study.md#28-negative-reviews--inability-to-place-books-neatly) | Adopt optional ball/prop play and useful temporary piles or stacks. Provide careful placement from the first handling revision; a throw-only system would frustrate players who want to arrange things. Keep clearing's visible-space payoff. | 1_02 revision, 1_06, 2_03; preserve with real props in 5_01/6_01. |
| [Chopping Trees: hauling](../../../research/case-studies/A_Game_About_Chopping_Trees_Case_Study.md#13-better-hauling-architecture) and [physics logs](../../../research/case-studies/A_Game_About_Chopping_Trees_Case_Study.md#23-physics-logs-are-a-major-bug-source) | Retain capacity/route upgrades measured through the whole job. Use physical carriers with recoverable contents instead of making every scattered pepper a completion dependency. Do not add stamina or forced return trips merely to ration the enjoyable action. | 2_01–2_03, 3_01/3_03, 4_02. |
| [Prison Escape: waiting](../../../research/case-studies/Prison_Escape_Simulator_Dig_Out_Case_Study.md#6-waiting-is-dangerous-design) and [unreachable objects](../../../research/case-studies/Prison_Escape_Simulator_Dig_Out_Case_Study.md#11-unreachable-objects-and-bad-geometry) | Retain useful work during processing and generous access/recovery. A toy can be fun during a natural pause, but cannot justify lengthening machine waits. Handle lost props and loaded carriers without resetting the harvest. | 1_06, 2_02/2_03, 3_03, 8_01. |
| [Drywall: action as traversal](../../../research/case-studies/Drywall_Eating_Simulator_Case_Study.md#6-wall-destruction-doubles-as-traversal) | Clearing should expose routes, viewpoints, and usable space through the same action. Support alternate physical approaches and consistent affordances; an amusing premise cannot substitute for pleasant handling. Yard destruction and scripted errands are not needed. | 2_01, 4_01, 5_01/6_01. |
| [Comparable-game memo: low agency](../../../research/case-studies/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md#biggest-gameplay-failure-low-agency), plus its My Summer Car / Barn Finders / Mon Bazou comparisons | Replace exact-object-to-exact-highlighted-spot choreography with choices of pile, route, staging position, output timing, and workspace arrangement. Express local character through things the player touches; use sparse amusing discoveries and visible progress on one personal project. | 1_02 revision, 1_04, 1_06, 4_01, 5_01/6_01. |

Implementation order: reopen 1_02 under the supplied negative feedback, retain its useful delivered work and 1_03 processing, then implement 1_04's usable finished carrier. Keep 1_05 as the complete-loop checkpoint. New task 1_06 adds the small loose-object sample before 2_01; M1 now spans 1_01–1_06. Extend wheelbarrow placement, pose saving, real-asset affordances, input options, and reliability/profiling in their existing briefs. Task IDs and historical delivery evidence stay intact; review gates still require actual human evidence.

The fixed-mat rule, blanket noninteractive-scenery rule, and one-deferred-physics-toy cap are superseded. The one-yard/one-food-loop direction remains: this request does not add recipes, household obligations, a construction/economy system, or a broad sandbox game. The previously pending Coins comparison and readable cards remain separate decisions. More freedom should come from handling and space before adding systems.

This was a documentation pass. The current executable still has the mat restriction and output collection remains pending; revised docs do not count as a physical-handling delivery. [Queue status](development/tasks/readme.md#ordered-task-queue) and [1_02 feedback](development/tasks/1_02_scooping-and-crate-carrying.md#free-placement-feedback-and-revision--september-6-2026) own readiness and the reported observations.

Documentation verification checked 57 Markdown files for local links/heading targets, UTF-8 and balanced fences; 32 unique ordered task IDs, brief sections/next links, the intended 1_02 status change and new 1_06 row; and preservation of all other queue rows/review gates and earlier 1_02/1_03 delivery history. A start-of-pass hash comparison preserved all 206 Unity source/asset/configuration/tool and case-study files, including existing uncommitted implementation work. Whitespace checks passed. Local check details are ignored under `unity/Logs/free-handling-docs-check.txt`. No Unity tests, player builds, or human playtests ran for this documentation revision; the new handling still needs its task-specific technical and human evidence.

## Scope-lock audit — September 6, 2026

Contradictions removed: mandatory day/meal machinery versus quiet harvest completion with continued control; generic rack storage versus the player's final household handoff; discovery rewards versus wording that implied hidden upgrade progress; fixed-path capacity-only final-upgrade checks versus a substantial hauling-route gain; a five-area sketch versus a measured maximum; optional props versus the scope table; and Bulgarian/English duplicate dialogue drafts versus one English bank. The jokes, triggers, and eventual localization/native review remain. The prototype question table contains nine explicit questions, and its reliability check does not pull M3 saves or M4–M5 presentation into the first interaction test.

Active first-version systems remain movement, finite local pile clearing, crate-to-wheelbarrow handling, broad scoop/dump, one automatic line with three tiers, one reusable finished carrier, one permanent handoff, one stored-food total, useful equipment/path discoveries, recovery/saving, and harvest completion. The final valid deposit commits completion and leaves the player in the finished yard with normal control. Existing sprint/jump controls are retained.

Presentation retains the winter-preparation story, cellar filling, labelled family boxes, returned jars, lyutenitsa/older cellar foods, old refrigerator/tool cupboard, decorative grinder, reused tools, vine table, Grandpa's bottle/gift and understated humor. A table/gift/thank-you flourish may be cut without changing completion. Household distribution reads stored food; no player or helper transports food between these displays. The [excluded-system list](scope-and-validation.md#scope-contract) stays explicit, including all removed earlier rules, chores, economies, factory construction, NPC workers, favors, and collectible progression.

Unresolved prototype risks: satisfying scoop/fill/dump feedback and convincing local depletion; a motivating wheelbarrow reveal with a dramatic equal-work gain; finished-food handling that feels like a payoff rather than another hauling chore; and reliable full/partial transfers and completion. Later measured risks remain the bounded progression decision, combined final-intake/output benefit across remaining routes, reveal timing with repeated work left, a small enough pocket count, and readable cultural presentation. None is a reason to restore processing rules or household tasks.

Development contracts now identify the single handoff owner, tier-derived intake restoration, last-deposit completion transaction, completed-property recovery, matched workflow measurements, and content boundaries. Task IDs, ordering, delivery/feedback statuses, review gates, and past execution evidence are preserved. Existing `Storage rack` names in the delivered foundation and its historical records describe the current artifact; 1_04 will update its actual label when implementing the handoff. No serialized field is blindly renamed, and this documentation pass changes no Unity source, assets, build, package, or shared research.

Prior-pass verification checked 54 active Markdown files and 549 local links/heading targets, English draft text, UTF-8, code fences, and whitespace. It compared all 31 queue entries against their previous IDs/order/kinds/statuses/feedback/evidence and retained all 17 English joke/alternative lines, the delivered movement contract, historical delivery records, and source URLs. The list below records that prior pass. Current amendment checks are reported in the final diff and do not claim Unity or legacy test execution.

Files changed in that prior documentation pass (34 existing Markdown files; none added or removed):

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
| Focused current implementation | One food loop, useful upgrades, household presentation, and harvest completion with continued control. | Preserve the purpose while concentrating implementation and testing on the main action. |

The earlier two-product and household-task rules are superseded, including their alternate-order inventories and timing gates. They are not deferred commitments.

## Research retained

The supplied [comparable-game memo](../../../research/case-studies/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md) warns about repeated processing chains, hauling fatigue, rigid tasks, and progress failures. The [culture memo](../../../research/culture/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md) supplies winter preserving, food circulation, reused objects, and hospitality. Original source notes remain unchanged; reception figures and engineering-cost guesses are not new measurements.

Leaf it Alone's official description links clearing with useful tools and things uncovered. That supports discovery through removal. [Official page](https://store.steampowered.com/app/3981100/)

Cash Cleaner's official description includes physical deliveries, processing equipment, efficiency upgrades, and workspace secrets. This informs batch handling and visible output without requiring every processing rule in this game. [Official page](https://store.steampowered.com/app/2488370/)

A Game About Digging a Hole links collecting, equipment upgrades, further access, and a mystery. Its clear progression is the reference for the desired focus; this does not establish equivalent development cost or likely reception. [Official page](https://store.steampowered.com/app/3244220/)

These are previously checked descriptions and design inferences. The revision does not refresh market statistics or establish what caused another game's success.

## Firsthand progression and comfort observations

The developer reported enjoying Leaf it Alone's relaxing visuals, sparse outdoor ambience, earning/upgrading, faster collection, collection-rate statistic, area rewards, autosave, and incidental kickable ball. The developer also reported that a small bag filled in about four seconds, creating frustrating interruptions and repeated full/error sounds. A medium bag later proved available elsewhere; the issue also involved a separate upgrade location from the usual Tab interface. These observations describe that play experience, not every player or every version.

The current game therefore measures the complete carrier rhythm, rate-limits full/invalid audio by meaningful state transition, keeps visual status readable when muted, makes equipment availability and effects explicit, and keeps any future Tab/workbench access pointed at the same interface. It never slows the satisfying scoop merely to improve a timing ratio.

Discovery remains the working baseline. The developer's enjoyment of earning and choosing upgrades is not treated as proof that discovery alone is equivalent. The [bounded Coins experiment](scope-and-validation.md#pending-progression-decision-experiment) is **pending separate authorization** after the core handling gate and before whole-yard production; it is not current shipping scope and has not been tested.

## The remaining risk

The small version can still become repetitive container walking. Responsive piles, satisfying transfers, useful route choices, and real gains from upgrades must carry it. More jokes, food types, or mandatory supply cannot rescue weak handling.

AI implementation still needs build verification and play observations. Presentation has an asset and integration cost even when it has no task system. The [validation plan](scope-and-validation.md) concentrates that work on the remaining uncertainty.

