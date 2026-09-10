# Steam review gap audit for Something Down There

Something Down There's strongest opportunity is to make excavation produce memorable discoveries, worthwhile equipment decisions and a satisfying conclusion within its intended 2–3 hours. The main risk is that a larger object catalogue could still feel like repetitive resource collection. Increasing item counts or running time alone would not address the dissatisfaction in these reviews.

This audit collected and screened **634 unique Steam reviews: 499 negative and 135 positive**, across two disjoint batches, retrieved on 2026-09-09. It compares those accounts with the full concept, existing research, unfinished contracts and relevant runtime code. Recommendations below concern remaining gaps or stronger acceptance criteria, not a catalogue of comparator problems already addressed in the project. They are design assessments; this audit did not play the three commercial games or validate the current Windows build.

## Read the findings

1. [Discovery identity, opening payoff and the personal display](discovery-and-payoff.md): make the existing content budget deliver recognizable, appropriately rewarded discoveries.
2. [Equipment value, finite money and recoverable finds](progression-and-recovery.md): test the complete expedition and the consequences of permanent losses.
3. [Ending clarity, save portability and market fit](delivery-and-positioning.md): communicate what the adventure delivers and protect the player's investment across devices.
4. [Additional reviews and player feel](player-feel.md): reveal/feedback clarity, honest return advice, surface payoff and voluntary excavation; durable [risk register](../../development/design-risks.md).
5. [Methodology and evidence limits](methodology.md): sampling, exclusions, version corrections and the distinction between reports and verified facts.

## Recommended changes

| Priority | Remaining gap | Concrete improvement | Future owner |
| --- | --- | --- | --- |
| Highest | A named roster does not establish why discoveries are memorable or rewarding | Give finds a recognition/reward rationale; observe whether the starter batch communicates the game's appeal before more content is commissioned | `89`, `09`, `15`, `40` |
| Highest | Finite loot plus permanent losses lacks an explicit full-campaign solvency test | Define a minimum viable finishing kit; test remaining reachable income after plausible mistakes and competing purchases | `56`, `59`, `50`, `37` |
| High | An upgrade can improve a stat while its practical benefit disappears at another bottleneck | Compare adjacent purchases over whole expeditions, including spend gaps, full bags and meaningful excavation after buying | `56`, `37` |
| High | Valid object placement does not establish that the object remains recoverable | Exercise ceiling/wall/boundary finds after widening and return, with normal purchased reach and flight | `45`, `47` |
| High | A saved personal photograph can be technically valid but visually worthless | Review snapshot legibility under fast collection, partial burial, oblique views and enclosed lighting | `60`, `51` |
| High | An absurd final object may be recognizable locally but its significance unclear to other players | Test whether players understand why it is impossible and how their finds prepared the payoff | `61`, `52` |
| Medium | Local recovery does not define cross-device or Steam-account save ownership | Make an explicit portability/Steam Cloud decision, including whole-save conflicts and device-local preferences | New design task `91` |

These changes strengthen existing work rather than select new gameplay systems. The initial audit added `91`; the expanded review adds `93`/`94` to design and implement honest return-warning semantics. Existing `13` remains complete; its reserve bands do not estimate route difficulty. Other improvements stay with existing task owners. Optional mechanics, the exact ending, specific assets and any cloud implementation remain subject to their owning design decisions. The paused equipment and lighting tasks remain paused; `89` remains next after this research batch.

## What this means for the game

The proposed game fits players who enjoy the short excavation-and-upgrade loop but want more discovery identity and a more coherent payoff. It will not satisfy every reviewer asking for combat, extensive puzzles or an endless progression treadmill. The positive sample supports keeping a simple, focused activity: some players value deliberately clearing their hole, while others enjoy an inexpensive, finite experience. [^228740862][^210151153]

The practical recommendation is to prove that one production expedition is memorable, then use that evidence to author the larger roster. The risk is execution and content density more than a missing headline mechanic. The existing 2–3-hour target remains appropriate as a design hypothesis; this sample cannot establish demand, price elasticity, sales or the finished game's quality.

## Evidence package

- [Review dataset](reviews.csv): all 634 reviews in five columns: ID, game, verdict, bounded excerpt and source. Dates, text hashes and screening live in the separate [audit index](audit/review-index.csv).
- [Selected evidence](evidence.csv): 57 selected paraphrases with cautions and direct sources; start here when a task needs individual evidence.
- [Request manifest](manifest.json) and [collector](collect_reviews.py): exact API requests and a repeatable collection procedure. See methodology before interpreting the sample.
- [Verification](verification.json): dataset integrity, documentation references and primary-source permalink spot checks.
- Research tasks [90](../../development/tasks/90-steam-review-gap-audit.md) and [92](../../development/tasks/92-player-feel-risk-research.md): scope and completion acceptance. The existing [research synthesis](../player-review-findings.md) remains the general entry point for earlier findings.

## Sources

[^228740862]: Steam community, A Game About Digging A Hole [positive review 228740862](https://steamcommunity.com/profiles/76561198046886614/recommended/3244220/), posted 2026-06-24; retrieved 2026-09-09. Public author and updated date are retained in the dataset.
[^210151153]: Steam community, A Game About Digging A Hole [positive review 210151153](https://steamcommunity.com/profiles/76561198798596795/recommended/3244220/), posted 2025-11-24; retrieved 2026-09-09. Public author and updated date are retained in the dataset.
