# Task 138 - Compact upgrade workshop

Type: implementation. Status: `done`. Explicitly selected with the user's inventory/fuel/refill request after `126`.

Prerequisites: existing Toolkit station UI and transactions (`12`/`74`/`107`); coordinate requested `46`/`48` and the selected policy from `98`. Features: [menus](../../features/backlog/menu-presentation.md), [selling/upgrades](../../features/backlog/selling-upgrades.md). Research: [current trip feedback](101-collection-return-playtest.md), [UI review](../ui-review/index.md).

## Selected work

- Each implemented upgrade gets one compact selectable row: name, owned level and next price/state. Hover, click selection and keyboard focus show that upgrade's brief description and current-to-next effects in a stable right-hand pane.
- Keep one explicit purchase action, accurate affordability/maxed states, concise result/error feedback, safe initial focus and retained selection after buying. Hovering or selecting never spends money.
- Reuse the existing charcoal/white/mint theme, font, station models and shared controls. No new visual assets, icons or audio are required.
- Integrate the requested backpack and shared-fuel capacity purchases from `48`/`46` and the paid refill flow from `139` once `98` resolves its policy. Do not expose inert future jetpack/detector tracks.
- Update exact shop copy/state entries in the existing UI catalog. Broader `106` review remains open; this is its selected workshop change only.

## Acceptance

- All available upgrade rows select independently using pointer and keyboard; the right pane updates without an accidental purchase or loss of selection/focus. Stale, repeated, unaffordable and maxed purchases are harmless.
- Compact rows and right pane remain readable at 960×540, 1920×1080 and 16:10; important actions remain accessible and keyboard selection scrolls into view.
- MainGame supports a natural sale/upgrade/refill/return cycle with owned equipment and carried finds preserved by save/reload. Preserve existing excavation and input comfort.
- Proportionate unit/integration checks, official CLI visual inspection, Windows build, updated task/status and concise completion evidence. No permanent review gallery or commit.

Questions: the list/right-pane direction and shared capacity/no-free-fill policy are selected. `56` records that answer; `98` records paid service and its initial amount-based pricing assumption. Existing rescue remains; further price/feel tuning follows playtest.

## Delivery evidence

[Completion](../completed/138-compact-upgrade-workshop.md). Windows build: `builds/windows/SomethingDownThere.exe`, 2026-09-13 06:00:28Z. [Validation](../../../unity/Logs/Task138/validation-summary.json): 71 checks pass, official CLI visual inspection, clean native startup and preserved user profiles. Initial price/usefulness tuning remains `37`/`103`; this delivery does not claim a balanced full game.
