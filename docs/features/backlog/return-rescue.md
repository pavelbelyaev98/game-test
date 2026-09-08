# Return and rescue

Status: Task `14` is complete using the existing pause UI and surface anchor; [completion](../../development/completed/14-confirmed-rescue.md).

Idea coverage: sections 32-37 and relevant tuning in section 53.

## Purpose

Make the route back through the player's excavation meaningful without allowing a mistake to destroy the whole run.

## Task 14 - confirmed rescue

- Scope after Task `13`: add `RescueController` and a polished pause-menu rescue action with confirmation. Introduce the session wallet ahead of Task `12`; it starts at zero until selling supplies income. Lose carried ordinary finds and charge up to 10 credits, clamped to that wallet; show actual lost items/value, fee and remaining credits before confirmation. Return to the clear surface anchor and refill battery. No new art/audio.
- Acceptance: Cancel is selected first and Escape returns to Pause without consequences. Confirm applies once, works at zero energy/money, closes menus and restores control without leaking held input. Revalidate landing clearance and the displayed inventory/wallet snapshot before any loss; changed costs require another review. Preserve excavation, owned shovel level and collected registry records so lost loot never respawns; normal returns remain physical.
- Evidence: depletion, confirmation/cancel/repeated-command and recovery integration checks. Protected-item rules, fall penalties and disk persistence remain later work.
- Next: Task `12` supplies earning/spending credits, then `15` validates [complete-loop integration](core-loop.md).

## Later expansion

Implement return warnings, fall consequences, and a confirmed rescue fallback connected to battery, inventory, and excavation persistence.

## Required behavior

- Normal return is physical; jetpack helps but does not replace route planning.
- Once at the surface, selling/upgrades are nearby rather than across an empty travel area.
- Minor falls have no important damage; large falls may stun or remove battery, and only extreme situations lead to rescue.
- Rescue explains its cost before activation.
- Excavation progress never resets. Ordinary carried loot may be lost; unique/special progress is protected.
- Rescue also charges enough money to discourage teleport abuse but can never permanently ruin the save.

## Done when

- Depletion, falls, rescue confirmation/cancel, and recovery cannot deadlock the player.
- Item/money consequences are deterministic and covered by integration checks.
