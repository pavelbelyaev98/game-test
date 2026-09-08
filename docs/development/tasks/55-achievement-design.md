# Task 55 - Design achievements and completion goals

Type: design/research; documentation only. Status: `planned`. Prerequisites: `40`, `56`, `63`.

Feature: [achievements](../../features/backlog/achievements.md). Implementation: [53](53-steam-achievements.md). [Queue](../tasks.md).

## Research and proposal

- Read the [persistent-investment findings](../../research/player-review-findings.md#persistent-investment), the named discovery roster and agreed progression. Inspect existing saves and official Steamworks achievement/stat requirements; investigate what can be earned after selling, rescue and Continue.
- Propose the actual achievement list: stable ID, player-facing name/description, exact unlock condition, progress/threshold, spoiler visibility and whether it is a natural milestone or an optional completion goal.
- For every condition, identify its authoritative saved fact, trigger owner and offline/reload reconciliation rule. Prefer goals recoverable from durable state. Do not leave needed tracking until the late Steam integration; assign it to a concrete upcoming implementation task before the relevant action can occur, or add a focused task only if an actual gap remains.
- Prefer naturally desirable discoveries, purchases and exploration. Avoid chores requiring enormous specially excavated shafts or fragile uninterrupted-fall/geometry tricks. Any proposed silly challenge needs explicit user selection, clear tolerant rules and a real feel test; examples involving optional dynamite do not select that mechanic.
- Check each goal against the finite roster and same-save postgame: no forced reset, missable one-shot reveal, mutually exclusive purchases or impossible randomly absent item. Keep counters out of the personal discovery display. Completion means recovering interesting finds, not deleting every voxel or awkward empty border wedge; no 100% terrain-removal goal. Freely clearing the site remains a player choice.
- Record icon briefs and platform/app-access needs, without adding assets, SDKs or changing external Steam configuration. Research inaccessible behind an account must be distinguished from design work that can proceed.

## Questions to resolve with the user

Present a concrete list for review: which goals belong, how demanding optional completion should be, whether any proposed unusual challenges add enjoyment without terrain-cleanup chores, and the naming/tone and spoiler policy. Do not ask the user to invent the achievement set from a blank page. Icon approval and platform authorization remain separate later steps.

## Done when

- The user has reviewed and accepted the concrete list and meaningful product choices; record it in the owning feature and reference it from `53`.
- Every goal has testable criteria, a same-save attainability check and an assigned persistence/trigger owner. Future producer tasks include any required tracking; no unresolved tracking dependency is hidden in `53`.
- The integration task consumes this decision instead of deciding achievements while wiring the SDK. This planning result does not claim achievements are implemented.
