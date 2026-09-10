# Sampling and evidence limits

## Collection and coverage

Public reviews were retrieved on **2026-09-09** through Valve's documented endpoint. Two batches retain **634 unique IDs: 499 negative / 135 positive**. Batch `90` collected 100 negatives and 25 positives per game. Batch `92` excluded all 375 earlier IDs and targeted another 75 negatives / 20 positives per game. [Manifest index](manifest.json); exact [first requests](audit/batch-90/manifest.json) and [additional requests](audit/batch-92/manifest.json). [^review-api]

| Game / AppID | First batch − / + | Additional − / + | Combined | Additional batch creation dates |
| --- | ---: | ---: | ---: | --- |
| Meltopia / `3601800` | 100 / 25 | 49 / 20 | 194 | 2026-05-01–2026-08-24 |
| Super Motherload / `269110` | 100 / 25 | 75 / 20 | 220 | 2014-06-30–2026-09-02 |
| A Game About Digging A Hole / `3244220` | 100 / 25 | 75 / 20 | 220 | 2025-10-05–2026-07-29 |
| **Total** | **300 / 75** | **199 / 60** | **634** | **Two disjoint samples** |

Each negative stream starts with up to 50 helpful-window entries, then uses a recent-stream cursor to reach its target. First-batch retained helpful/recent negatives were Meltopia 50/50, Super Motherload 11/89 and Hole 50/50. The additional Meltopia negative cursor returned 50, 50, 49, then zero rows, exhausting this English/default-filter request with only 49 unseen negatives; we did not manufacture the remaining 26. This is exhaustion of that endpoint view, not all languages/all Steam reviews. Additional Super Motherload negatives extend back to 2014; they are especially historical.

Positives begin with a helpful-window request; batch `92` uses recent fallback when exclusions leave too few, as happened for Super Motherload. Parameters include `language=english`, `purchase_type=all`, `day_range=365`, and default off-topic exclusion. The `all` filter uses helpfulness in sliding windows, so these are **not all-time top-upvoted reviews**. Recent ordering follows creation time, not necessarily the latest edit. Duplicate IDs count once; stream memberships remain in the audit index. [^review-api]

## Reading, screening and interpretation

Every retained full text was read in temporary local caches: roughly 35,000 words in batch `90` and 18,700 more in `92`. The five-column [reviews.csv](reviews.csv) is a bounded excerpt index, not the full text used for analysis. [evidence.csv](evidence.csv) retains **57 selected accounts** as concise paraphrases, cautions and direct source links. A selection is an evidence-retrieval decision, not exhaustive topic coding.

The combined audit index records 57 selected accounts, 556 screened but unselected, six mislabeled non-English entries, two empty entries, eight definite bundled-Goldium-only entries and five ambiguous bundled-original nostalgia entries. Exclusions remain in the collected denominator. Brief votes, jokes and low-information text do not strengthen claims merely by repeating sentiment. Only explicit Super Motherload observations within mixed bundle reviews are used for sequel findings; unresolvable original-game praise is not transferred to it.

Steam's recommendation label is not the sentiment of every sentence: positives can report serious problems and negatives can value the core activity. No population percentages, thematic prevalence, independent double-coding or causal effect is claimed. Helpful votes are not credibility scores; longer reviews are not weighted more heavily. All purchase routes are included. This English-labeled targeted corpus does not represent all buyers, non-reviewers or all languages. Self-reported durations and platform playtime do not establish measured campaign length. Speculation about developer intent, undisclosed AI assets, malware or refund manipulation is not adopted as fact.

## Historical claims and source checks

Meltopia announced Cloud support on August 27, generated expeditions on June 25, and artifact/cold/tutorial fixes on June 7. Earlier complaints illustrate needs rather than prove those defects persist. These are developer-reported changes, not independently retested fixes. Individual reviews can also be edited after the experience they describe; the API does not identify the played build. Store pages confirmed game identity; no current prices, overall ratings or market forecast are inferred. [^platform-update][^expedition-update][^ice-fixes]

Full-text API records ground individual evidence, with direct public review permalinks retained. [verification.json](verification.json) reports count/hash/link checks and a bounded set of public-page spot checks. A page returning HTTP 200 alone is not proof of a gameplay claim. The commercial games and current Windows build were not played as part of this documentation audit.

## Repository comparison

The audit read the full concept, summary, status, relevant existing research, feature/task contracts and completed evidence. Code inspection covered exposure/reach, inventory and rescue loss, whole-save ownership, then the existing `ReturnWarning`, Toolkit `GameHudView` and `FpsPlayer` feedback path. Prior implementation evidence was not rerun or recertified. We distinguished an intentional charge-band baseline from the concept's still-unimplemented approximate-return-effort requirement.

Recommendations require a concrete source account, a remaining project gap or under-specified acceptance case, and a bounded owner. Already addressed concerns are retained only as useful regression guardrails. Timing, recognition, solvency, warning comprehension and payoff are testable project-specific inferences; research completion does not make the feature implemented or select optional mechanics/assets. The [risk register](../../development/design-risks.md) is the future-work entry point.

## Compact reuse and reproducibility

Read the relevant report section or selected evidence first. The all-review view contains only ID, game, verdict, an excerpt of at most 18 words and source URL. Dates, author attribution, text SHA-256, screening and selected topic keys live in [audit/review-index.csv](audit/review-index.csv); language, device and purchase flags are omitted from routine files. Full source texts remain in the temporary caches and can be refetched by URL; they are not republished here. A new interpretation requires full source context, not only the short excerpt.

The [collector](collect_reviews.py) uses bounded requests/retries, deduplication and an overwrite guard. For another batch, run `python docs/research/steam-review-audit/collect_reviews.py --output <new-directory> --exclude docs/research/steam-review-audit/audit/review-index.csv --negative-target 75 --positive-target 20`. Newly collected rows are marked **not-screened** until read; the tool does not invent takeaways. It now emits compact reading and separate metadata views. The method is reproducible; live contents and helpful ranking are not immutable.

The original batch's [verification](audit/batch-90/verification.json) records checksums for the former 23-column layout at that time; it is historical evidence, not a validator for today's merged files. Current verification supersedes those file-layout hashes. Both original request manifests and every retained review ID/text hash are preserved. Compaction removed duplicated wide tables, not collected reviews.

## Sources

[^review-api]: Valve, [User Reviews — Get List](https://partner.steamgames.com/doc/store/getreviews?l=english), accessed 2026-09-09.
[^platform-update]: Garden of Dreams, [Platform Update!](https://steamcommunity.com/games/3601800/announcements/detail/675130357246329176), 2026-08-27.
[^expedition-update]: Garden of Dreams, [Expedition Generator Out Now!](https://steamcommunity.com/games/3601800/announcements/detail/699894448411116030), 2026-06-25.
[^ice-fixes]: Garden of Dreams, [Ice Cubes Fixes](https://steamcommunity.com/games/3601800/announcements/detail/702144344758420434), 2026-06-07.
