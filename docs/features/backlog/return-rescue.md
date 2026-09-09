# Return and rescue

Status: Task `14` rescue is complete; [completion](../../development/completed/14-confirmed-rescue.md). Task `50` adds fall consequences after paid jetpack/detector progression (`47`/`49`); saving is `35`.

Idea coverage: sections 32-37 and relevant tuning in section 53.

Research: [70 - navigation and revisit aids](../../development/tasks/70-navigation-and-revisit-research.md) decides whether HOME/markers are justified. Design: [59 - return pressure and fall consequences](../../development/tasks/59-return-and-fall-design.md) resolves player-facing loss/recovery rules before `50`; controller research and numerical landing tests remain with implementation.

## Purpose

Make the route back through the player's excavation meaningful without allowing a mistake to destroy the whole run.

## Task 14 - confirmed rescue

- Scope after Task `13`: add `RescueController` and a polished pause-menu rescue action with confirmation. Introduce the session wallet ahead of Task `12`; it starts at zero until selling supplies income. Lose carried ordinary finds and charge up to 10 credits, clamped to that wallet; show actual lost items/value, fee and remaining credits before confirmation. Return to the clear surface anchor and refill battery.
- Acceptance: Cancel is selected first and Escape returns to Pause without consequences. Confirm applies once, works at zero energy/money, closes menus and restores control without leaking held input. Revalidate landing clearance and the displayed inventory/wallet snapshot before any loss; changed costs require another review. Preserve excavation, owned shovel level and collected registry records so lost loot never respawns; normal returns remain physical.
- Evidence: depletion, confirmation/cancel/repeated-command and recovery integration checks. Protected-item rules, fall penalties and disk persistence remain later work.
- Next: retain completed `12` earning/spending, then `15` validates [complete-loop integration](core-loop.md). Protected passive state belongs to `36`; any selected ending components are owned by `52`.

## Later expansion

Implement return warnings, fall consequences, and a confirmed rescue fallback connected to battery, inventory, and excavation persistence.

## Required behavior

- Normal return is physical; jetpack helps but does not replace route planning.
- Once at the surface, selling/upgrades are nearby rather than across an empty travel area.
- Minor falls have no important damage; large falls may stun or remove battery, and only extreme situations lead to rescue.
- Rescue explains its cost before activation.
- Excavation progress never resets; Task `35` extends current session preservation across save/load. Ordinary carried loot may be lost; unique/special progress, including permanent passive rewards (`36`), is protected.
- Rescue remains an emergency fallback at late wealth and with empty bags as well as early play. The implemented up-to-10-credit fee is a baseline, not proven full-run balance; `59` proposes any scaling and `50` implements the chosen policy. Show exact consequences and preserve broke/stranded recovery; do not make rescue optimal normal transport or add a hidden debt trap.

## Done when

- Depletion, falls, rescue confirmation/cancel, and recovery cannot deadlock the player.
- Item/money consequences are deterministic and covered by integration checks.

## Task 50 - fall consequences with desirable mobility

See [numbered Task `50`](../../development/tasks/50-fall-consequences.md) for scope, research, questions and acceptance.

## Conditional HOME-direction experiment

- Run `70` immediately after `15` for an early concrete HOME/marker comparison; do not wait for final paid jetpack/detector tiers. Inclusion and free-versus-purchased access remain uncommitted until review; neither aid is mandatory for normal return or the ending.
- If justified, test a coarse bearing toward the surface/base only. No route solving, waypoints, exact distance, GPS path or guaranteed safe exit; an obstructed bearing still leaves the player to plan the return.
- Compare return clarity and player agency with/without it in sideways tunnels and multi-level pits. Defer or drop it if the benefit is weak. Create an implementation task only after this gate; any new visual/audio asset still needs explicit approval.
- `70` owns the evidence-based HOME decision; no implementation is implied by completing that research. A defer/omit decision must retain its reason and the evidence that would justify revisiting it.
- Research in `15`/`37` also measures whether return time is meaningful risk or repetitive commuting. First inspect route readability, jetpack progression, content spacing and station distance. Ask the user about a specific change only when that evidence exists; underground outposts, teleports or shortcuts are not approved by the supplied [review report](../../research/player-review-findings.md#scope-boundaries-and-conditional-investigation).

## Conditional revisit markers

`70` also tests whether players forget a particular hard wall or branch, separately from surface orientation. If justified, discuss a simple placed marker/flag/light, quantity/reuse, removal, visibility, persistence and disappearing terrain support. No minimap, automatic waypoint chain, through-wall treasure marker, building menu or lamp platform. Keep this optional; remembering the player's own excavation may already work well.
