# Task 10 - Passive detector feedback

Type: implementation. Status: `planned`. Prerequisites: 09, 42, 57. `40` must allocate actual eligible non-minor targets to `42`; common-only starter trip `15` does not require detector signals.

Feature: [detector](../../features/backlog/detector.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Scope after `42`: implement `PassiveDetector` over physically eligible uncollected **non-minor** registry positions, with tunable range, distance-driven pulse interval, quiet gaps, and production-quality feedback consistent with `08`. Filter minors before focus selection, aggregation and audio/non-audio presentation.
- Acceptance: every common type is silent at any size/value/material, alone or clustered, including next to an eligible distinctive target. Approaching/receding from eligible targets changes cadence predictably; out-of-range/collected finds are silent, overlap foregrounds one stable target and menus pause feedback. Never reveal identity, value, rarity, exact distance or a target marker. Test low-priced eligible non-minor and physically ineligible non-minor cases; changing price cannot change eligibility. Collection/out-of-range transitions release focus without rapid switching.
- In real digging, the stream must remain suggestive enough to ignore while widening, carving routes or making chambers. Do not optimize feedback into continuous waypoint chasing; retain quiet intervals and user-defined excavation shape. Observe this in `102`/`37` alongside meaningful common-only quiet digging.
- Next: production discovery playtest `102`. Positive detector acceptance needs the approved MainGame non-minor content from `42`; fixtures may check selection logic but cannot complete the feature. Do not reclassify a common starter to manufacture a signal. Later upgrades may add stereo/directional guidance without revealing identity or value.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect common/non-minor classification and authored non-minor physical eligibility independently from price, and test target selection, stable switching and cadence/quiet gaps. Implement [57's feedback brief](57-presentation-design.md), obtain approval for the actual sound/visual batch and audition sustained use. Paid interpretation milestones belong to `56`; no rarity signal or target marker.

## Independent audio comfort

- Add a detector-only volume control including mute in existing Settings and persist it as a user preference. It must not mute digging, stations or all world audio; supply readable non-audio feedback through `57`'s reviewed brief so muting is usable.
- Acceptance includes minimum/default/mute, relaunch, pause/focus and sustained nearby/overlapping targets on headphones and speakers. Avoid restart bursts, harsh repetition and continuous beeping; `49` preserves this at maximum range and `54` checks a full session.
