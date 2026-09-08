# Task 52 - Mystery progression, final discovery and same-save Continue

Type: implementation. Status: `planned`. Prerequisites: 51, 61.

Feature: [ending](../../features/backlog/ending.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

Implement a persisted late-game mystery escalation, end-state trigger, reveal sequence boundary, and continue mode without coupling normal progression to one fragile scene event.

## Before implementation

- Read [persistent-investment findings](../../research/player-review-findings.md#persistent-investment) and idea sections 46–49; inspect real late equipment/content rather than borrowing the reference games' finales. Research a recoverable trigger/cutscene state flow using the existing save owner and official Unity guidance where needed.
- Implement the final object, lead-up, trigger, cutscene and any selected protected-component rules accepted in [61](61-ending-design.md). Verify automatic/obvious component use and rescue-safe persistence without introducing an inventory puzzle; do not invent the payoff during implementation.
- Obtain explicit approval for each actual final-object/cutscene asset or sound batch before adding it. The brief in `61` selects the design; it does not approve imports or make the research's example mandatory.

## Done when

- Trigger, interruption, reload, and continue-mode checks pass. Include only the short story reactions explicitly selected in `61`, if any: they cannot block controls/transactions, spam after reload or replace the quiet HUD with dialogue.
- Neither the ending nor Continue goals require full terrain clearance; preserve self-shaped routes and free excavation as worthwhile play.
- Complete the playable ending with ordinary purchased equipment and no rare passive rewards; verify each included tool remains usable before the cutscene and after Continue Playing. Validate late purchases through [Task `37`](../../features/backlog/selling-upgrades.md#task-37---full-run-upgrade-pacing).
- After the ending, players can keep upgrading, finding missed discoveries, completing achievements, and filling the discovery display.
- The result matches the accepted `61` narrative/interaction brief. Return unresolved changes to the user as concrete proposals instead of silently altering the ending.
- Same-save postgame retains collected/sold identities, photos, money and useful purchases. Re-entering the reveal or recovering after interruption cannot wipe the hole, respawn rewards or force a restart for later achievement completion (`53`). Inspect both the playable reveal and Continue in the Windows build.
