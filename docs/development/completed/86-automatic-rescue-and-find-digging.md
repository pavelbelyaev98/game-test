# Task 86 — Automatic rescue and digging around small finds

- Why: zero fuel should recover automatically; aiming at a visible small find should keep excavation moving toward pickup. [Task](../tasks/86-automatic-rescue-and-find-digging.md).
- Integrated: final digging/jetpack fuel triggers the existing loss/fee transaction, safe standing surface return, refill, input suppression and checkpoint. Pause no longer exposes rescue or confirmation. Blocked landing retains belongings and retries.
- Integrated: held digging on ineligible small finds clears actual nearby covering soil with normal shovel cadence, radius, reach and fuel. At 40% exposure, ordinary collection wins; other obstacles, large finds, capacity and pickup recovery retain their rules.
- Checks: 106/106 EditMode; all 98 PlayMode cases covered by the full run and corrected save/recharge retests. Existing fixtures now release their test-owned pause across reloads and distinguish partial surface recharge from zero-fuel rescue.
- CLI MainGame inspection: unchanged aim cleared 13.5% → 40.6% exposure in three strokes, then collected once without a terrain edit or energy cost; final thrust rescued, charged 10 credits and preserved terrain. [Pause preview](../../../unity/Logs/Task86/pause.png). Screenshots/results: `unity/Logs/Task86/`; isolated review save, user saves preserved.
- Windows build: [SomethingDownThere.exe](../../../builds/windows/SomethingDownThere.exe), `2026-09-09 14:34 UTC`, zero errors. Existing Pipeline runtime-configuration warning does not affect gameplay.
- Remaining: user feel review of this build; final discovery art remains `09`, and lighting design `68` is next.
