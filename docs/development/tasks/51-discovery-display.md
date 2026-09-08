# Task 51 - Persistent personal discovery snapshots and display

Type: implementation. Status: `planned`. Prerequisites: 50, 35, 42-45, 60.

Feature: [discovery display](../../features/backlog/discovery-display.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

Create a compact surface display (for example a refrigerator, corkboard, wall, or workbench) that automatically captures how each distinctive item looked when first discovered.

## Before implementation

- Read [persistent-investment findings](../../research/player-review-findings.md#persistent-investment), inspect real collection/exposure events and research snapshot capture/storage cost without stalling a shovel hit. Use the same versioned save boundary (`35`) for identity/metadata and recoverable image references.
- Implement the display form, capture moment, ordering and full-capacity behaviour accepted in [60](60-discovery-display-design.md); obtain approval for the actual asset batch. Capture the player's excavation rather than a stock image and preserve the 50% held-pickup contract.

## Done when

- Discover, deduplicate, save/load, and version-migration checks pass.
- The display does not block selling or the core trip loop.
- Explicit completion goals belong in platform achievements rather than cluttering the display.
- Inspect multiple real snapshots, full-display browsing and continued collection after selling/rescue/relaunch in the Windows build. `52` verifies the same records after the ending; `53` owns achievements, without a forced fresh save.
