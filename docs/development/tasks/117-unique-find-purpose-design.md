# Task 117 - Define the purpose, frequency and fate of unique finds

Type: design/research; documentation only. Status: `ready`. Prerequisites: existing concept and completed review research `90`/`92`. Independent of paused equipment/lighting and the common starter trial.

Feature: [discovery content](../../features/backlog/discovery-content.md). Handoff: `97` interaction/chests, `40` named roster, `45` generation, `56`/`37` economy, `60`/`51` display, `55` achievements; `62`/`36` passives and `61`/`52` ending retain their own decisions.

## User question and scope

The user has not selected what uniques are for, whether there is one unique in the whole map, one instance of each of several types, or multiple copies, or whether they can be sold or must be kept. Produce documented findings and concrete recommended approaches before asking the user to settle those choices. Collectibles becoming battle equipment is rejected. No new combat, crafting, museum-management or mandatory puzzle loop.

Separate quantity from reward policy: one-per-save does not imply unsellable, and a repeatable distinctive item can still be memorable. Separate authored type, visual appearance and individual instance. Treat the existing 30–50 distinctive-type target and sale-plus-photo direction as comparison baselines, not settled answers to this reopened question; retain current common content and implemented behavior.

## Starting findings from existing research

- [Discovery identity and payoff](../../research/steam-review-audit/discovery-and-payoff.md) records both disappointment with display-only artifacts and enjoyment of simple collectible discovery. The useful hypothesis is that the effort and presentation must match the payoff; it does not prove every unique needs a power or monetary reward.
- [Equipment and recovery](../../research/steam-review-audit/progression-and-recovery.md) identifies whole-trip purchase value and finite-income loss risks. Making distinctive finds unsellable requires checking replacement income and upgrade pacing, not silently increasing common-loot grind.
- [Player-idea assessment](../../research/player-idea-assessment.md) identifies sale/memory/use as alternative roles and warns against a purpose or quest for every item. These are retained design judgments; no new comparator playthrough or player validation is claimed.
- Recognition, absurdity, contextual mystery and a personal record can each be rewards. Review which examples actually deliver those feelings; current bottle/rock trials cannot establish the appeal of the full distinctive roster. Selected review anecdotes cannot determine the optimal count.

## Approaches to compare, none selected

| Approach | Concrete reward and destination | Main question to investigate |
| --- | --- | --- |
| Sellable discoveries + permanent memory | Recover a distinctive object, sell it for useful income, retain its personal discovery record | Does the remembered encounter justify its special status after the object is gone? |
| Protected physical collection | Recover and retain an object in a simple automatic display; compare collection alone with a one-time discovery payment | Does keeping the object add appeal, and can normal purchases remain satisfying without repeated display chores? |
| Mixed categories | Sellable distinctive finds alongside a small, clearly explained set of protected keepsakes or already-planned passive/ending objects | Can players understand the outcome immediately without hoarding everything against an unknown future use? |

My starting hypothesis is the mixed approach with sale-plus-memory for most distinctive finds, but the comparison must be capable of rejecting it. Independently compare one exceptional find in the whole map, several types each occurring once, and repeatable distinctive types. Explain what a single-find policy leaves to carry curiosity across the 2–3 hour run; do not confuse the number of types with the number of copies.

## Required decision artifact

- Develop the findings and comparison in this task, linking specific existing evidence and its limits. Inspect current content, inventory, sale, identity and save contracts. Seek additional primary-source evidence only for a concrete unanswered comparator question; distinguish documented mechanics, player opinions and our inference.
- Show representative early/middle/late encounters for each viable approach: first sight, recognition, collection, return, sale/retention, later revisit and Continue. Use illustrative objects rather than commissioning or selecting assets. State the promised feeling and observable payoff.
- Compare both axes explicitly: total/types/copies per map and payoff/destination/sale policy. Cover repeat discoveries, scarcity, useful income, sell regret, storage/capacity, full bags, rescue, first-discovery records, achievements and finite completion. Protected passive rewards remain optional; an ordinary distinctive item does not automatically inherit their protection.
- Explain the implications for the provisional roster count, discovery gaps, lateral exploration and late novelty. Include a small illustrative income/purchase scenario with assumptions labelled, not a claimed balance result or a new grind target.
- Recommend a coherent approach with concrete reasons, tradeoffs, rejected alternatives and a brief player-validation plan. Define what observation would change the recommendation. Resolve wording such as unique/distinctive/rare without selecting visible rarity labels by default.
- Keep detailed interaction/input and chest opening with `97`; named objects/count allocation with `40`; placement with `45`; display form/capture with `60`. Move the selected shared purpose/frequency/reward rules into the feature at handoff rather than duplicating contracts across those tasks.

## Questions and acceptance

Review the completed comparison with the user: desired discovery reward, total/type/copy policy, sale versus retention, treatment of repeats and clarity of protected exceptions. Questions alone do not complete this task.

Done when the user has reviewed the concrete artifact and the owning feature records the selected rules, rationale and remaining deferred options; all affected tasks consume that decision without relying on chat. Reconcile the overall concept and provisional roster targets if changed. Create implementation work only for selected missing behavior. This task does not implement or approve any unique, display, asset or new item function.
