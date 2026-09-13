# 143 - Whole currency and $ presentation

- Why: the user supersedes fractional pricing and requests whole money with a $1 minimum and only the `$` symbol in-game.
- Result: refills cost $1 per 100 missing fuel, rounded up to whole amounts; 25/85/100 fuel costs $1 and 150 costs $2. Partial affordable service and `142` validated-target delivery remain intact.
- Wallets reject fractional transactions. Legacy fractional balances round upward once on session restoration; subsequent v5 checkpoints write zero fraction. Existing whole balances, world state and upgrades remain compatible.
- All money display uses `$`: HUD, carried/sale values, workshop prices, balances, shortfalls, purchase/sale and rescue feedback. Uses the approved font glyph and existing layout.
- Evidence: 119 relevant checks pass, including 5,000 critical fractional-fuel cases, pointer refill purchase, legacy fractional-file conversion and repeated checkpoint/reloads. CLI inspection confirms $10 → $9 with a full starter tank, $2 rock value/sale, and -$10 rescue feedback. [Validation](../../../unity/Logs/Task143/validation-summary.json).
- [Windows build](../../../builds/windows/SomethingDownThere.exe): 2026-09-13 08:00:54Z; zero errors / 1 existing disabled-Pipeline warning, clean seven-second native startup. All 41 profile files preserved, temporary captures removed, MainGame clean/stopped.
- Limitation: any small top-up costs $1 by request; natural trip payoff still needs the user's `101` retest. No commit.
