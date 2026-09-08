# Task 53 - Same-save Steam achievements

Type: implementation. Status: `planned`. Prerequisites: 37, 51, 52, 55.

Feature: [achievements](../../features/backlog/achievements.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Implement the achievement list and exact conditions accepted in [55](55-achievement-design.md). Preserve its same-save/no-missability rules; do not decide goals or introduce new completion requirements during SDK integration.
- Derive progress from durable game-owned events/records through `35`; deduplicate grants and retain locally earned milestones while offline. Reconcile with the platform safely when it becomes available; platform failure cannot block normal play or saves.
- Keep names/depth-only discovery photos and the quiet gameplay HUD. Platform goals must not turn the display into a percentage checklist or add a second progression currency.

## Before implementation

- Research: inspect existing discovery/save/ending IDs and official Steam achievement lifecycle, offline behaviour and deployment requirements. Verify platform access before promising an integrated result.
- Verify the accepted `55` list and persistence/trigger mapping against the finished game. Request missing Steam app/account configuration, and present any necessary SDK/package or actual icon batch concretely; icons still require separate asset approval and permitted sources.
- If Steam access is blocked, record that blocker. Local flags alone do not complete a Steam integration task; publishing external configuration requires the appropriate explicit authorization.

## Acceptance

- Earn/reconcile achievements through actual game actions, offline/relaunch, Continue and repeated loading without duplicate or lost progress. Already-sold discoveries can still count from durable records.
- Verify same-save attainability against the generated finite roster, including no impossible objective requiring a missing random item or an exact terrain-removal percentage. Check the accepted goals require neither artificial terrain-construction chores nor fragile wall-touch/fall conditions; any deliberately selected challenge must pass its documented tolerance cases.
- Validate the integrated Windows build with the configured platform; record remaining platform/release limitations.
