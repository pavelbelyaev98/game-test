# Additional reviews: player feel and avoidable directions

The second batch adds **259 previously unseen reviews: 199 negative and 60 positive**. All were read from full text, about 18,700 additional words. Combined: **634 reviews, 499 negative / 135 positive**. This expands the qualitative comparison; it does not measure how common any opinion is. See [methodology](methodology.md) for the older Super Motherload coverage and Meltopia collection limit.

## Discovery must register without becoming a chore

Meltopia accounts describe an attractive but noninteractive hub, unclear collection purpose and difficulty judging tool reach. A positive reviewer also wanted a visible consequence from collecting. Our inference is to clarify what visual cues promise and make the reward readable, not to make every prop usable or give every artifact a bonus. [^225497526][^225119169][^226822729]

Our held collection, exposure rules and quiet HUD are already selected. The remaining design question is whether the reveal survives fast digging and overlapping feedback. [FpsPlayer.ShowFeedback](../../../unity/Assets/Runtime/Player/FpsPlayer.cs) currently replaces one message and gives it 2.5 seconds; this is an inspected implementation constraint, not proof players miss finds. `57` now owns a concrete priority/recognition matrix; `09` tests it with rapid, full-bag and powerful-tool cases. Existing pickup timing is preserved unless an evidence-backed change is selected.

The target feeling is noticing a shape, understanding what was uncovered and carrying away a memory. Do not substitute mandatory inspection, washing, extra clicks, a pop-up after every item or a larger catalogue of indistinguishable rewards. `08`/`58` also need to distinguish decoration, usable stations, buried finds, hard diggable material and permanent boundaries. A Hole reviewer spent substantial time seeking a presumed solution that the game did not provide: ambiguity should invite discovery without promising nonexistent interactions. [^222004745]

## Return advice and the surface payoff

A Super Motherload reviewer missed both clear fuel feedback and the satisfying pause associated with returning and improving equipment. This does not justify extra service steps; it suggests that instant service still needs understandable acknowledgment and a wanted next purchase. `57` briefs that short beat, `08` applies it to existing stations and `15` observes comprehension and station time. [^17227856]

The concrete project gap is narrower: completed `13` intentionally implements [reserve bands](../../../unity/Assets/Runtime/Player/ReturnWarning.cs) at 35%/15%, while [GameHudView.UpdateBattery](../../../unity/Assets/Runtime/UI/Toolkit/GameHudView.cs) prints SAFE/RISKY/CRITICAL. The full concept's section 33 instead asks for approximate return difficulty. New [93](../../development/tasks/93-return-warning-design.md) compares honest meanings and route examples; [94](../../development/tasks/94-return-warning-update.md) implements the selected result. We have not observed a player misreading the label, and an approximate depth estimate must never imply knowledge of obstructed or lateral routes.

## Preserve voluntary excavation and earned power

Positive Hole reviews describe tidying a personal hole and choosing a shaft versus broad clearing as worthwhile play. Meltopia reviews also show that a thorough explorer can exhaust upgrades earlier than a fast player; a positive account reports purchases ending before the artifact search. `37` now explicitly compares these styles without forcing equal duration, adding filler purchases or making all terrain clearance mandatory. [^215710545][^219801263][^225124953][^225276363]

Visible hardware changes can help a purchase feel real, but they cannot compensate for replacing a strong tool with a weak one or increasing return friction. Another positive Meltopia review liked visible upgrades while criticizing those costs. Preserve the one-shovel design and test what a purchase changes across the whole trip. [^233599475]

## Keep completion, sound and the ending consistent with the promise

A positive Meltopia reviewer valued the absence of a last-percent cleanup demand. Hole accounts disagree about speedrunning: one found randomness frustrating, another enjoyed the speedrun but disliked a long-fall chore after clearing. `55` should evaluate a concrete goal's seed dependence and impact on an existing excavation, without assuming every challenge is unwanted. Relaxed audio use also supports quiet optional feedback rather than constant attention demands. [^225783618][^222320788][^226709062][^207929926]

The additional Super Motherload ending account reinforces the existing exclusion of surprise reflex tests. Keep mystery escalation and an understandable absurd reveal; do not copy a boss or disable earned tools to manufacture a finale. Save freezes, inconsistent notices and partial restoration are useful regression risks, but existing `35`/`79` and deferred `80` already own that work. The expanded review set does not authorize restarting the postponed benchmark. [^18339936][^224850513][^224666363][^24042801]

## What to build from this evidence

The durable [risk register](../../development/design-risks.md) records intended feeling, warning signs, what to avoid and the task that must produce evidence. New work is limited to `93`/`94`; existing presentation, starter collection, trip, progression, achievement, site and return specs now have sharper proposal/acceptance cases. Existing successful systems are recorded only as guardrails against regression.

The fit remains a compact excavation adventure with authored discoveries and growing physical power. Some positive Super Motherload players explicitly enjoy puzzles, bosses and repeated character progression; they are a different audience, not evidence that our scope must absorb those systems. Short-game praise and requests for more content cannot establish our price or demand. The key proof remains one memorable production trip followed by a dense complete run. [^202100545][^229760419]

## Reading without loading the corpus

Start here or in the owning task. [evidence.csv](evidence.csv) contains 57 selected paraphrases with cautions and direct sources. [reviews.csv](reviews.csv) contains all 634 bounded excerpts in five columns; [audit/review-index.csv](audit/review-index.csv) keeps dates, hashes and screening separately. No language, hardware or purchase flags clutter the reading view. Full texts remain in the temporary collection caches; excerpts alone are not substitutes for rereading a source when investigating a new claim.

## Sources

[^225497526]: Steam community, Meltopia [negative review 225497526](https://steamcommunity.com/profiles/76561198122279814/recommended/3601800/), posted 2026-05-14; retrieved 2026-09-09.
[^225119169]: Steam community, Meltopia [negative review 225119169](https://steamcommunity.com/profiles/76561198020117519/recommended/3601800/), posted 2026-05-09; retrieved 2026-09-09.
[^226822729]: Steam community, Meltopia [positive review 226822729](https://steamcommunity.com/profiles/76561198011293667/recommended/3601800/), posted 2026-05-31; retrieved 2026-09-09.
[^222004745]: Steam community, A Game About Digging A Hole [negative review 222004745](https://steamcommunity.com/profiles/76561198124814374/recommended/3244220/), posted 2026-03-29; retrieved 2026-09-09.
[^17227856]: Steam community, Super Motherload [negative review 17227856](https://steamcommunity.com/profiles/76561198004890916/recommended/269110/), posted 2015-07-25; retrieved 2026-09-09.
[^215710545]: Steam community, A Game About Digging A Hole [positive review 215710545](https://steamcommunity.com/profiles/76561197968658621/recommended/3244220/), posted 2026-01-11; retrieved 2026-09-09.
[^219801263]: Steam community, A Game About Digging A Hole [positive review 219801263](https://steamcommunity.com/profiles/76561198045701821/recommended/3244220/), posted 2026-03-04; retrieved 2026-09-09.
[^225124953]: Steam community, Meltopia [negative review 225124953](https://steamcommunity.com/profiles/76561199220440280/recommended/3601800/), posted 2026-05-09; retrieved 2026-09-09.
[^225276363]: Steam community, Meltopia [positive review 225276363](https://steamcommunity.com/profiles/76561197987565903/recommended/3601800/), posted 2026-05-11; retrieved 2026-09-09.
[^233599475]: Steam community, Meltopia [positive review 233599475](https://steamcommunity.com/profiles/76561198218657020/recommended/3601800/), posted 2026-08-24; retrieved 2026-09-09.
[^225783618]: Steam community, Meltopia [positive review 225783618](https://steamcommunity.com/profiles/76561198031409603/recommended/3601800/), posted 2026-05-17; retrieved 2026-09-09.
[^222320788]: Steam community, A Game About Digging A Hole [negative review 222320788](https://steamcommunity.com/profiles/76561199203821591/recommended/3244220/), posted 2026-04-02; retrieved 2026-09-09.
[^226709062]: Steam community, A Game About Digging A Hole [positive review 226709062](https://steamcommunity.com/profiles/76561198015261565/recommended/3244220/), posted 2026-05-29; retrieved 2026-09-09.
[^207929926]: Steam community, A Game About Digging A Hole [positive review 207929926](https://steamcommunity.com/profiles/76561199387434371/recommended/3244220/), posted 2025-10-30; retrieved 2026-09-09.
[^18339936]: Steam community, Super Motherload [negative review 18339936](https://steamcommunity.com/profiles/76561197974622842/recommended/269110/), posted 2015-10-05; retrieved 2026-09-09.
[^224850513]: Steam community, Meltopia [negative review 224850513](https://steamcommunity.com/profiles/76561198023704930/recommended/3601800/), posted 2026-05-05; retrieved 2026-09-09.
[^224666363]: Steam community, Meltopia [negative review 224666363](https://steamcommunity.com/profiles/76561197989531710/recommended/3601800/), posted 2026-05-03; retrieved 2026-09-09.
[^24042801]: Steam community, Super Motherload [negative review 24042801](https://steamcommunity.com/profiles/76561197985969658/recommended/269110/), posted 2016-07-03; retrieved 2026-09-09.
[^202100545]: Steam community, Super Motherload [positive review 202100545](https://steamcommunity.com/profiles/76561199837036729/recommended/269110/), posted 2025-08-14; retrieved 2026-09-09.
[^229760419]: Steam community, A Game About Digging A Hole [positive review 229760419](https://steamcommunity.com/profiles/76561198045236620/recommended/3244220/), posted 2026-07-06; retrieved 2026-09-09.
