# Task 10 - Passive detector feedback

Type: implementation. Status: `planned`. Prerequisites: 09, 57.

Feature: [detector](../../features/backlog/detector.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Scope after Task `09`: implement `PassiveDetector` over uncollected registry positions, with tunable range, distance-driven pulse interval, quiet gaps, and production-quality feedback consistent with Task `08`.
- Acceptance: approaching/receding changes cadence predictably, out-of-range/collected finds are silent, overlap produces one readable pulse stream, and menus pause feedback. Never reveal identity, value, rarity, exact distance or a target marker.
- Next: Task `11`. Later upgrades may add stereo/directional guidance without revealing item identity or value.
- Before implementation: read [physical-comfort findings](../../research/player-review-findings.md#physical-comfort), inspect eligible registry data and test proximity cadence/quiet gaps in the current audio space. Implement the initial feedback approach from [57](57-presentation-design.md), obtain explicit approval for its actual sound/visual batch, and audition sustained use. Paid interpretation milestones are designed in `56`; no rarity signal or target marker.
