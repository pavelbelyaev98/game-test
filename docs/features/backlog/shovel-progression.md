# Shovel progression

Status: Task `10` is planned; one temporary cadence upgrade exists for validation.

Idea coverage: sections 9-11, the tool arc in section 46, and relevant tuning in section 53.

## Purpose

Make each upgrade visibly and physically improve excavation rather than merely increasing a hidden number.

## Task 10 - two shovel levels

- Scope after Task `09`: add session-owned `ShovelState` with two data-driven levels; level 2 starts at 1.5 times the basic removal radius, retaining energy cost/cadence. Connect settings to terrain; the paid station follows in Task `11`.
- Acceptance: equal accepted hits into equal fresh soil remove observably more at level 2 for the same energy; boundaries stay intact. Invalid/skipped/repeated level changes fail, and the chosen level survives surface trips.
- Next: Task `11`. Full level progression, tool art, resistance variety and disk persistence remain later work.

## Later expansion

Create a data-driven sequence of roughly 6-8 shovel levels affecting cadence, resistance, reach, removal size, efficiency, or feedback as appropriate after terrain playtesting.

## Required behavior

- Levels are purchased in order and communicate their concrete effect.
- Better tools make previous resistance noticeably easier.
- Keep one recognizable evolving shovel: basic, reinforced, powered, motorized, then an unreasonable homemade machine with visible attachments.
- Mix incremental upgrades with expensive milestone levels that create a large, exciting jump.
- Use money rather than arbitrary depth gates. The player chooses among tool, battery, jetpack, inventory, and detector priorities.
- Upgrade data can be balanced without rewriting gameplay logic.

## Done when

- Each implemented level changes observable excavation behavior.
- Purchase, save/load, and invalid-level checks pass.
