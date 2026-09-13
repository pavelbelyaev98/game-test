# Task 94 - Update return warning logic and presentation

Type: implementation. Status: `planned`. Prerequisites: selected [93](93-return-warning-design.md), production HUD acceptance `05`.

Feature: [return and rescue](../../features/backlog/return-rescue.md). Validation: `15`, paid-tier follow-through `47`, full-run `37`. [Queue](../tasks.md).

## Scope

The user separately selected charge-warning prominence in `140`: bottom-center yellow/red at the existing 35%/15% bands. Preserve that presentation when applying the later `93` decision; `140` does not finish this task’s return-effort contract.

- Update the existing `ReturnWarning` and Toolkit battery/warning presentation to implement `93`'s selected meaning. Preserve `139` paid workshop refills and the current shared battery; do not rebuild working rescue or settings.
- If the selected design estimates effort, use actual effective flight/capacity settings and the agreed uncertainty fallback. If it retains charge bands, use wording that describes reserve accurately. Implement only the selected branch.
- Keep warning state stable around thresholds and consistent through digging, thrust, recharge, menus/focus, save/load and automatic rescue. Keep zero-charge handling and blocked-anchor feedback coherent; no implied loss-policy changes.
- Preserve compact readable HUD hierarchy, non-color meaning, controller/keyboard input and existing comfort preferences. Any selected sound must pass the normal specific asset approval; no new asset is implied by this task.

## Research and acceptance

- Before coding, inspect the selected scenario matrix and current battery/flight ownership. Technical tuning stays here; return unresolved product choices to `93` with concrete evidence.
- Run deterministic checks for the repository's chosen classification, uncertainty and transitions; include equal-percent/different-route cases and different effective flight/capacity combinations. Verify integration with recharge and rescue, not Unity internals.
- Inspect the actual MainGame HUD through the official Unity CLI, including small supported window sizes, muted audio and interruption/reload states. Provide a Windows build for user review; automated classification checks alone do not establish understandable advice.
- Record what the warning can and cannot infer and evidence for its selected scenarios. Preserve `13`/`75`/`86` completed records; update the return feature/status and pass remaining human-comprehension observation to `15`. No guaranteed safe return or new pathfinding system.
