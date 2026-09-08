# Task 22 - Verify release builds exclude developer admin access

Type: validation. Status: `planned`. Prerequisites: 53.

Feature: [excavation terrain](../../features/backlog/excavation-terrain.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- The user's Task `23` preference supersedes deleting all developer tooling. Keep durable admin code; Unity's `Debug.isDebugBuild` flag gates access, actions and UI. No Windows elevation or separate game executable is needed.
- Before production, build without `Development`, verify admin shortcuts/menu/overrides are unavailable, and verify paid upgrades, recharge and rescue work through real game systems. No launch argument or scene setting may enable admin in a release player.
- Practice launchers/flag are removed under `23`. Confirm no old launcher is distributed; historical completion records remain as evidence. This remains a mandatory production release gate.
- Before implementation, after `53`: read [persistent-investment findings](../../research/player-review-findings.md#persistent-investment), audit the actual build flags, admin entry points and development-content restrictions, and research the current official Unity build API only if needed. No user decision is required for the existing non-development gate; prove ordinary paid play and saving work in that build. Task `54` performs final full-game qualification afterward.
