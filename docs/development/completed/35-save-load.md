# Task 35 — Persistent excavation and progression

- Why: player-shaped excavation and earned progress must survive quitting, rescue and interruption together.
- Integrated result: versioned whole-world snapshots restore exact terrain/collision, accepted finds and collected identities, inventory, credits, owned shovel, battery and player pose. Developer overrides stay temporary.
- Dirty autosaves use a 10-second interval; trades/rescue request immediate checkpoints. A single worker flushes and replaces files with a previous-checkpoint backup. Pause includes Save and quit; load/write failures offer readable recovery without silently starting fresh.
- Evidence: 77 EditMode + 65 distinct PlayMode checks pass, including interrupted commit stages, repeated restore, rescue, overlapping writes and the focused UI rerun. Official CLI reviewed actual Pause/error/recovery presentation.
- Native Windows evidence: two collected finds sold for 19 credits; shovel 2 purchased for 10; forced stops after sale/purchase and during held digging restored complete matching snapshots. Rescue and normal Save and quit retained the exact excavation hash.
- Measured current-grid capture: normally 4–5 ms and 14.45 MB copied when terrain changes, under 0.3 ms when reused; observed periodic loss window about 10.14 seconds including write completion. Full details and research are in [the retained task](../tasks/35-save-load.md); local evidence is `unity/Logs/Task35/`.
- Windows build succeeded `2026-09-08 18:47 UTC`: `builds/windows/SomethingDownThere.exe`. Validation profiles are archived outside the live save location.
- Limitation/next action: `63` agrees hardware and save/load/frame/memory budgets; `54` qualifies long excavations. Future content preserves save identities and explicitly extends/migrates this format.
