# Scope and validation for Just a few peppers

[Design index](readme.md) · household winter-preparation proposal · documentation only

The largest uncertainty is whether gathering and tipping peppers is satisfying after the first discovery. An attractive yard, a wheelbarrow upgrade, a funny machine, and family chores cannot rescue an ordinary action that feels like filling a progress bar. The whole-game goal is household readiness; the next test still isolates bulk handling.

The old roasting tests answer a different question. Keep their evidence separate. This request makes no runtime changes and does not start the Unity documentation bootstrap.

## What the research contributes

| Source | Design inference for this version | What it does not establish |
| --- | --- | --- |
| [Leaf it Alone / Librarian study](../../../Ideas/Leaf_it_Alone_and_Librarian_Case_Study.md) | Clearing can reveal useful space and objects; visibly completed work matters. | Whether collecting peppers feels like clearing leaves. |
| [Cash Cleaner study](../../../Ideas/Cash_Cleaner_Simulator_Case_Study.md) | Batch contents, containers, output handling, and meaningful capacity upgrades can sustain one workspace. | That automatic machines or new material types guarantee depth. |
| [Meta-study](../../../Ideas/Simulator_Games_Meta_Study_20_Games_Blueprint.md) | Progression should reduce mastered friction while retaining a satisfying action. | A required economy, detector, collection, or production line for every game. |
| [Chopping Trees study](../../../Ideas/A_Game_About_Chopping_Trees_Case_Study.md) | Repeated physical actions need immediate response and visible consequences. | That increasing quantities compensates for weak feel. |
| [Drywall study](../../../Ideas/Drywall_Eating_Simulator_Case_Study.md) | Separate the premise's joke from action enjoyment; avoid lost items and progress blockers. | That a culturally funny premise produces voluntary replay. |
| [Prison Escape study](../../../Ideas/Prison_Escape_Simulator_Dig_Out_Case_Study.md) | A visible destination and successive access can organize progress. | A need for enemies, escape mechanics, or a large environment. |
| [Early blueprint](../../../Ideas/game_type.md), [idea collection](../../../Ideas/ideas.md), [shortlist](../../../Ideas/Final_Game_Ideas_Bulgarian_Traditions_and_Meme_Sims.md) | Supply alternative motivations and scope warnings. | Mandatory feature checklists. |

The [pivot note](design-pivot.md) records checked official descriptions for Leaf it Alone, Cash Cleaner, and A Game About Digging a Hole. The connections above remain design judgments.

The newly supplied [comparable-game memo](../../../Ideas/Just_A_Few_Peppers_Comparable_Game_Case_Studies.md) and [culture memo](research/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md) add specific constraints:

| Input | Spec consequence | Later evidence required |
| --- | --- | --- |
| Food Processing Simulator | Two output routes at most; a new ingredient/label alone is not a new task. | Players make and appreciate a routing decision. |
| Garden-project game | Multiple reachable priorities, short hauling, state-derived completion, recoverable objects. | Players use different valid orders without getting stuck. |
| My Summer Car | Culture in handled objects, a specific property, and atmosphere. | The scene communicates a Bulgarian household with dialogue muted. |
| Barn Finders | Sparse useful discoveries rather than a mandatory collection. | A reveal changes the player's next action. |
| Mon Bazou | Added chores visibly serve the larger household goal. | Players see a purpose beyond another checked box. |
| Bulgarian household research | Returned jars, two family parcels, reused tools, half-stocked cellar, final table. | These small additions improve context and payoff without padding. |

The memos' review figures remain dated research notes. No fresh market ranking or causal claim about another game's success is required.

## Smallest complete candidate

| Area | Proposed cap |
| --- | --- |
| World | One yard split into five connected areas, one shed bay, small cellar storage, street backdrop. |
| Main actions | Gather, carry, tip/place, reveal, and route. Household tasks reuse those controls. |
| Carrying | Hands, 12-unit crate, 48-unit wheelbarrow. |
| Processing | One upgraded roasting/preparation front end; two fixed product routes; one final front-end replacement. |
| Stock | Two functional classes: normal and sound irregular. |
| Products | Roasted-pepper jars and conditional lyutenitsa jars. Other pantry foods and the meal are presentation, not more production chains. |
| Household tasks | One returned-jar carrier, two family parcel boxes, and one table setpiece with three props and a cloth. |
| Local choice | Reachable wheelbarrow, shed, and cellar approaches after the gate; parcels and table work can be completed early. |
| Discovery | Guaranteed equipment reveals; at most three optional keepsakes/portraits. |
| Story | One seated Grandpa, relatives through props/notes/offscreen lines, about 24 short lines. |
| Progression | Clearing exposes equipment; no mandatory currency, shopping, crafting, or repair economy. |
| Completion | [Household finish conditions](household-readiness-and-parcels.md#finish-conditions), followed by the player's Finish the day action. One meal ending; optional replay afterward. |
| Representation | Grouped pile depletion and bounded decorative motion as the first candidate. Individual pepper physics is not assumed. |
| Reliability | Exact contents through transfers and shelf/parcel reallocation; persistent returned-jar/table state; partial loads, pause, save/resume, and valid recovery. |

AI-assisted implementation still needs human judgment about visual continuity, interaction feel, performance, and edge cases. Bulk representation may be harder than the old single-pepper spike even though the player has fewer cooking rules. Treat it as a feasibility question, not an automatic scope saving.

Defer baskets that duplicate the crate, hot-pepper recipes, giant-pepper machines, conveyors, free construction, autonomous workers, co-op, vehicle driving, weather hazards, cooking quality systems, electricity management, farming, shopping trips, and rakia production.

The lyutenitsa route belongs in the current complete-game candidate, but only after its routing experiment. If it adds no worthwhile decision, remove its functional stock distinction and process all sound stock through the default roasted-pepper route; keep the cupboard discovery as a useful tool reveal. Both parcels still accept that output, so no new production chain or mandatory recipe quota replaces the cut feature. Document this as a scope revision.

Do not add compote, turshiya, fermented cabbage, or tomato-juice production just because they belong in the real cellar. Represent them as existing supplies. Avoid realistic canning, jar breakage, individual lid matching, firewood/heating chores, and a recurring jar economy.

## Next experiment: one pile, one carrier, one discovery

A future test needs a small outdoor corner, an authored mound, crate gathering/tipping, one automatic intake, one finished-carrier output, a storage target, and a wheelbarrow partly concealed by the mound. No full yard, story scenes, new recipe, final machine, returned-jar task, family parcel, or table setpiece is added to this prototype.

First prove repeated gathering and tipping with the crate. Then expose the wheelbarrow and give the player a comparable amount to clear. Make the processing/output capacity change with the carrier so the comparison includes the complete workflow.

Record actual work and implementation time against a declared cap. No code, test build, or player observations were produced by this documentation revision.

## Separate the uncertainties

| Test | Question | Useful evidence | Response to failure |
| --- | --- | --- | --- |
| A. Gathering and tipping | Does each scoop/dump feel substantial? | Immediate local depletion, clear carried amount, voluntary repetition without a new reward. | Improve representation and control first; at most two bounded revisions. |
| B. Discovery | Does a partly exposed tool encourage further clearing? | Player notices the clue and deliberately works toward it. | Improve placement/silhouette; do not add random sparkle everywhere. |
| C. Upgrade | Does the wheelbarrow improve the complete same-size task? | Fewer support actions/trips and better enjoyment, including queue/output time. | Fix coupled capacity, walking, or carrier controls. |
| D. Processing rhythm | Is there useful activity while a batch runs? | Low forced idle time and few repetitive output confirmations. | Raise throughput, compress handoffs, shorten routes. |
| E. Routing | Do two stock classes and two outputs produce meaningful choices? | Players understand the destinations and enjoy allocating batches. | Keep homogeneous sources, simplify selection, or cut the extra route. |
| F. Full clear | Can a tiny finite section finish reliably? | No hidden leftovers, lost loads, unresolvable partial jars, or trapped equipment. | Fix accounting/recovery before interpreting enjoyment. |
| G. Household layer, after A–F | Do the jar return, parcel allocation, and table setpiece improve purpose and local choice? | Players understand recipients, can use different orders, and do not resent new errands. | Compress or cut tasks that add detours without value. |
| H. Ending, after the core succeeds | Does the winter-ready household and meal feel complete? | Player notices clear access, allocated food, waiting parcels, and the reclaimed table; Finish the day stays voluntary. | Fix the payoff; never insert another obligation after the finish action. |

For the first valid evaluation, borrow the [shared scorecard](../../prototypes/prototype-comparison-scorecard.md)'s six-player approach where practical. Keep this build/task recorded separately from the original three-concept comparison. Do not replace its untested values with predictions.

Provisional reference gates: median ordinary-action enjoyment at least 4/5, at least four of six choosing a brief unrewarded continuation, median forced waiting/support friction at most 20%, and no unresolved completion blockers. Ask what players liked: jokes, discoveries, or the action itself. Small samples guide iteration; they do not validate a market.

Compare equal quantities and rotate test order when possible. Record actual seconds gathering, loaded travel, empty travel, tipping, queue waiting, and output handling. Count control errors and assistance separately from intentional routing decisions.

## Failure cases the design must survive

- A carrier is full, half full, cancelled mid-tip, or aimed at a partly full intake.
- A processed carrier blocks output while the player explores.
- The last supply is smaller than a machine batch or a full jar.
- An irregular-stock source is approached before the grinder can accept it.
- Food is already on the outdoor rack when the cellar opens.
- A player routes all compatible stock to one product.
- A tool becomes exposed while nearby pile visuals still settle.
- The game is paused, quit, or resumed while contents are carried, queued, processed, or stored.
- Decorative physics clips an object under a table.
- The yard is visually empty while food remains in processing.
- All current food is shelved before the player packs either family box; it remains retrievable.
- A ready parcel is moved or emptied; the current readiness indicator updates without losing food.
- Returned jars or the table task are completed before their dialogue triggers.
- The cellar is opened before the wheelbarrow or grinder; no mandatory stock requires an inaccessible route.
- The table is set while machines are active; it does not displace a required work surface.
- Every pepper is resolved but a parcel is not at the gate; Finish the day stays unavailable with a clear reason.
- Both parcel targets exceed a revised harvest budget; the authored content check catches the mismatch before play.

These are future acceptance cases, not tests written in this task. The intended responses are specified in [Mechanics](core-loop-and-mechanics.md), [Progression](yard-and-progression.md), and [Household readiness](household-readiness-and-parcels.md).

## Household scope gate

The household layer is authorized as a design proposal, not as extra work in the next prototype. After the core test succeeds, evaluate one returned carrier, the two parcel destinations, and the small table setpiece together. Compare whether they create useful return trips and a better ending rather than merely extending duration.

Target well under five minutes of additional one-off handling across the full campaign; parcel allocation should fit normal output trips. Count all detours, prompts, and repeated transfers. Completion in a different valid order must require no developer rescue or repeated tutorial action.

## Where the design can scale

Prefer more uses of established actions: alternate pile arrangements, a different path revealed, clearly tagged mixed loads, new family notes, and new views of finished storage. Introduce them only if they change a decision or a tactile moment.

The finite supply should take fewer burdensome actions as tools improve. Do not exactly cancel every capacity increase by multiplying the next mandatory pile, or shift saved time into processing delays.

Do not promise campaign duration yet. The illustrative 732-unit budget is for discussing scale and accounting. Choose final quantities from a measured enjoyable section, then stop when the arc feels complete.

## Current decision

Use **Just a few peppers** as the title and **get Grandpa's household ready for winter** as the whole-game goal. Keep bulk clearing, handling upgrades, and useful discoveries as the mechanical core. Add only the bounded household layer described here, after its prerequisite tests. These are design choices to evaluate, not proof of fun.

Unproven: pile representation, enjoyable repeated handling, performance, routing value, total duration, integration cost, and demand. The [earlier cooking package](archive/roasting-v1/readme.md), [one-pepper Stage 0](../../prototypes/chushkopek-stage0.md), and [18-pepper brief](../../prototypes/chushkopek-prototype.md) remain historical references; their results cannot answer these new questions.
