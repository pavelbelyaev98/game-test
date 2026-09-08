# Current status

The [queue](tasks.md) has 45 unfinished tasks: 15 design/research and 30 implementation/validation, plus 26 completed and cancelled `38`. IDs are `01`?`72`; the next new ID is `73`.

- Current milestone: [Task `72` complete](completed/72-pickup-exposure-and-doc-cleanup.md). Finds require 40% exposure; held pickup and saved progression remain intact. Redundant asset-absence reports are removed, with actual content ownership/removal records retained.
- No active task. Next ready: [Task `63` ? Windows support and performance targets](tasks/63-windows-targets-and-budgets.md), a concrete hardware/budget proposal for user review.
- Latest checks: 4 discovery placement + 7 collection integration tests pass. Official CLI review blocked pickup at 35.4% and collected once at 41.7%, with matching prompt/HUD and no extra cut or battery cost. Evidence: `unity/Logs/Task72/`.
- Latest Windows development build: `2026-09-08 19:23 UTC`, `builds/windows/SomethingDownThere.exe`; succeeded with zero errors.
- Persistence remains implemented in [Task `35`](completed/35-save-load.md), including native interruption/recovery validation. Its current-grid save measurements and existing large synchronous digging spikes feed `63`; long-excavation qualification remains `54`.

No current blocker.
