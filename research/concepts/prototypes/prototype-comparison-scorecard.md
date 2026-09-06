# Prototype comparison scorecard

Status: historical decision framework for three proposed prototypes; comparison playtests have not started. The roasting proposal was discarded in favor of the [current Just a few peppers design](../../../games/just-a-few-peppers/docs/readme.md). Its old spike remains in the [repository audit](../../../games/just-a-few-peppers/docs/development/repository-audit.md); technical checks do not populate this comparison. Created 2026-09-05. This retained research framework does not schedule work on the discarded proposal.

Purpose: determine whether each repeated interaction deserves further development. This is not a production design, market ranking, or selection of a winner.

## Three experiments

| Prototype | Hypothesis to test | Exactly one power increase |
| --- | --- | --- |
| Chushkopek Season (discarded roasting proposal) | Roasting, steaming, and peeling remain satisfying across repeated peppers. | One active roasting socket becomes three. |
| [Basement Cleanout](basement-cleanout-prototype.md) | Removing ordinary clutter is enjoyable without a valuable discovery. | One-object pulling becomes up-to-three-object pulling. |
| [Cable Closet Untangler](cable-closet-prototype.md) | Tracing, freeing, routing, and reconnecting feels controlled and satisfying. | A toner reveals the selected cable's full path. |

The main session targets 5–10 minutes, followed by an optional two-minute continuation probe. Times are playtime targets, not development estimates. Never lengthen waits or resistance merely to meet the runtime. Record an unexpectedly short completion as a pacing observation.

## Research basis and scope precedence

The existing corpus was reviewed before these briefs, with the final shortlist read last. These are the sources and the specific lessons being applied:

| Research document | Application here |
| --- | --- |
| [Reusable Unity blueprint](../../blueprints/game_type.md) | Immediate feedback, authored transformations, explicit state ownership, recoverable interactions. |
| [Earlier concept exploration](../ideas.md) | Test ordinary clearing before relying on hidden finds. |
| [Chopping Trees](../../case-studies/A_Game_About_Chopping_Trees_Case_Study.md) | Make the single upgrade perceptible and retain the original action afterward. |
| [Prison Escape](../../case-studies/Prison_Escape_Simulator_Dig_Out_Case_Study.md) | Avoid forced inactivity and progression blocked by object placement. |
| [20-game meta-study](../../blueprints/Simulator_Games_Meta_Study_20_Games_Blueprint.md) | Keep support friction small; identify why the player wants one more action. |
| [Leaf / Librarian](../../case-studies/Leaf_it_Alone_and_Librarian_Case_Study.md) | Visible order and player competence can motivate play without an economy or random loot. |
| [Cash Cleaner](../../case-studies/Cash_Cleaner_Simulator_Case_Study.md) | A few meaningful object states; satisfying processing feedback; controlled placement. |
| [Drywall Eating](../../case-studies/Drywall_Eating_Simulator_Case_Study.md) | Distinguish liking the premise from enjoying the action personally. |
| [Final ideas](../Final_Game_Ideas_Bulgarian_Traditions_and_Meme_Sims.md) | Preserve the three chosen fantasies while removing speculative systems. |

The current request overrides the older 20–30-minute prototype templates. Economy, four upgrade branches, complex saving, and a mystery are not requirements for these experiments. Source opinions about eventual commercial potential are not test results. No additional market research or competitor score is needed here.

## Comparable build conditions

- Use one scene per concept, an existing first-person controller if available, mouse/keyboard input, and a local full-scene reset. No shared simulator framework is a prerequisite.
- Include working anticipation, resistance, release/completion audio, and readable states before testing. Placeholder art is acceptable; missing core feedback is not an honest test of the hypothesis.
- Keep visual effort comparable: neutral lighting, legible targets, basic sound mixing, no cinematic presentation or voice performance giving one prototype an advantage.
- Provide sensitivity adjustment, stable camera, readable text, and icons/numbers alongside color. Optional viewmodel motion must not move the camera.
- Pause the interaction and timers when the game pauses or loses focus. Reset returns all content and the sole upgrade to their initial state.
- Use the same machine and audio setup for comparison sessions. Record build ID, implementation variant, and settings. Results from different cable representations must remain separate.
- Allow at most two bounded revisions of core input, feedback, or timing after the initial build. Record effort in hours and fixes attempted. Set the effort cap before coding each prototype; do not keep extending it to protect a favorite.

## Test protocol

1. Run a developer readiness check in a packaged build: complete the loop, use the upgrade, finish, reset, and repeat. Exercise cancellation and invalid targets. An obvious broken build is not ready for player evaluation.
2. Recruit six external players with interest in tactile, organizing, or small simulation games. Avoid a sample made entirely of collaborators already invested in one idea. This is directional qualitative screening, not statistical proof.
3. Where practical, have the same players test all three, with a short break between them. Give each of the six possible orders to one player: PCB, PBC, CPB, CBP, BPC, BCP; P = peppers, C = cables, B = basement. Otherwise record the different cohorts and avoid treating results as directly equivalent.
4. Give only the short task instruction in each brief and one controls card, taking no more than about 30 seconds. Do not call a game satisfying or explain which concept the developer prefers.
5. Let the player attempt the main task for up to ten minutes. They may stop at any time. Record assistance; do not silently teach them through every mistake. Do not require continuous think-aloud commentary, which changes pacing.
6. At completion, offer the same neutral choice: "Stop here, or do up to two more minutes of the same activity. There is no new reward or unlock." Let them stop without social pressure. Measure both acceptance and actual continued play.
7. Use the continuation content specified in the brief, with the acquired upgrade retained and no special reward. Do not tell basement players that there might be treasure; the offer explicitly promises no new reward.
8. Ask the same questions, in this order: "Which part would you repeat?"; "What felt annoying or unclear?"; then the ratings below. Ask about the premise and appearance only after the action questions.

If the player stops before completion, record why and continuation as not reached. It does not count as voluntary continuation. Show both the result out of all six and the result among finishers. A technical interruption is marked invalid for the enjoyment comparison and retested after repair, while the failed run stays in the reliability record.

## Shared measures and decision gates

For participant ratings, use 1 = strongly disagree, 2 = disagree, 3 = mixed, 4 = agree, 5 = strongly agree. Record individual answers and the median; do not invent decimal precision. The same gates apply to all three.

| Criterion | Evidence / exact measurement | Initial gate |
| --- | --- | --- |
| Repeated action appeal | "The ordinary repeated action itself felt good." Separate action praise from praise of discoveries, setting, or ending. | Median at least 4; at least 4/6 identify an action they would repeat. |
| Voluntary continuation | Accepted the optional two minutes AND actively continued for at least 60 seconds, or finished the extension sooner by choice. | At least 4/6; report raw counts and duration. |
| Agency | "My movements and choices affected the result in a way I understood." | Median at least 4. |
| Clarity | Time to first complete interaction cycle; facilitator help; main task completion. | At least 5/6 complete a first cycle within 90 seconds and finish without facilitator help by ten minutes. Faster completion is acceptable. |
| Control reliability | False rejection of an intended valid action, wrong-target selection, lost grab, or unintended drop, divided by meaningful interaction attempts. | Median per-player error rate at most 10%; no recurring three-failure sequence at the same affordance. |
| Boredom / support friction | Observer-coded seconds spent forced waiting, repeating an understood but rejected action, or doing purposeless support work, divided by active session length. | Median at most 20%. Deliberate inspection, tracing, and anticipation are not automatically dead time. |
| Progression moment | "The new tool made the work more satisfying while leaving something enjoyable for me to do." Compare matched ordinary actions before/after. | Median at least 4; at least 4/6 can explain the change. Speed alone is insufficient. |
| Completion / transformation | "I could see what my work changed, and the ending felt earned." | Median at least 4. |
| Technical feasibility | Blockers, unintended state changes, resets, and developer rescue in packaged runs. | Zero unresolved completion blockers or state-loss defects before interpreting a successful batch of six valid sessions. |
| Implementation cost | Actual build/revision hours, setup time for the optional extension, and new code needed for it. | Fits the predeclared effort cap; extension uses existing interaction logic. No speculative scalability score. |

A complete interaction cycle means one peeled pepper on the tray, one ordinary clutter removal deposited, or one cable freed, guided, and correctly connected. Meaningful attempts are grab/place/drag/plug actions with clear intent, not every frame or mouse delta. Distinguish intentional cancellation and understandable player mistakes from control failures. Use event timestamps plus observer notes; event logs alone cannot identify boredom or intent.

## Minimum instrumentation

Use a small in-memory event list and optional local CSV/JSON export, or an observer worksheet if that is cheaper. No analytics service, account integration, or telemetry framework.

Common fields: anonymous participant ID, build ID, concept, variant, elapsed time, target ID, stage, event, success/reason, upgrade active. Common events: session start/end, action start/complete/cancel/reject, upgrade unlocked/used, prototype complete, reset, assistance, extension offered/accepted/stopped. Pause time is excluded.

Observer notes record confusion, three repeated failures, intentional inspection, idle frustration, spontaneous praise, and why the player stops. Preserve raw quotes and exceptions alongside scores. Keep initial failed runs in the record instead of reporting only the best build.

## Results to fill after playtests

N/T = not tested. Nothing below is a prediction or preassigned rank.

| Measure | Chushkopek | Basement | Cable |
| --- | --- | --- | --- |
| Build / representation | N/T | N/T | N/T |
| Valid participants / attempted sessions | N/T | N/T | N/T |
| Action appeal median / action-praise count | N/T | N/T | N/T |
| Continued / all participants; duration | N/T | N/T | N/T |
| Agency median | N/T | N/T | N/T |
| First-cycle time / unaided finish count | N/T | N/T | N/T |
| Intended-action error rate | N/T | N/T | N/T |
| Boredom / support-friction share | N/T | N/T | N/T |
| Upgrade rating / explanation count | N/T | N/T | N/T |
| Completion rating | N/T | N/T | N/T |
| Blockers / resets / rescue | N/T | N/T | N/T |
| Build hours / revision hours / extension setup | N/T | N/T | N/T |
| Most common complaint / strongest action quote | N/T | N/T | N/T |
| Decision and supporting evidence | Pending | Pending | Pending |

## Decision rules

- **Continue evaluation:** The prototype meets the shared gates and its concept-specific hypothesis. This permits a next feasibility step; it does not automatically authorize a full production design or select the final winner.
- **Revise once, then at most once more:** A specific correctable control, feedback, or timing issue explains the result. Change that issue and retest. Prefer fresh players; identify returning players because familiarity changes results.
- **Stop this concept for this project:** After the bounded revisions, ordinary-action appeal or voluntary continuation still fails, the representation fails its fantasy, or acceptable interaction cannot fit the declared implementation cap. Do not rescue it with story, NPCs, crafting, more content, or additional upgrades.
- **Inconclusive:** Essential feedback was absent, technical defects prevented a fair run, or the sample is incomplete. Repair within the same cap or stop on feasibility grounds; do not mislabel a broken implementation as proof that nobody enjoys the activity.

Do not sum unlike measurements into a winner score. After playable evidence exists, compare the concepts that pass, including their actual implementation cost. None, one, or several may pass. The final production decision remains open.
