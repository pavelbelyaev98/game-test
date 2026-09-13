# 142 - Reliable refills and proportional prices

- Why: critical fractional fuel could reject a rounded addition after payment. The user also requests and confirms proportional fractional prices.
- Result: refill commits its validated target before charging; affordable full refills end exactly at capacity. Failed, stale, repeated and full-tank offers cannot bill. Affordable partial service remains available.
- Price: 1 credit per 100 fuel, rounded up to 0.01 cr; 25/85/150 fuel costs 0.25/0.85/1.50. Workshop quote, payment, feedback and HUD agree. Integer hundredths preserve exact balances through purchases and capped rescue fees.
- Saves: v5 appends the fractional remainder; frozen v4 and earlier supported formats keep existing money/world state. Fractional balance and full fuel survive repeated file reloads.
- Evidence: 109 relevant checks pass. Actual Unity critical-charge probe improves from 46,213 failures to zero in 100,000 cases. Pointer purchase and CLI review reproduce 10 credits + 13.00586/100 fuel becoming 9.13 credits + 100/100; repeat purchase disables and warning clears. [Validation](../../../unity/Logs/Task142/validation-summary.json).
- [Windows build](../../../builds/windows/SomethingDownThere.exe): 2026-09-13 07:47:32Z, zero errors / 1 existing disabled-Pipeline warning; clean seven-second native startup. All 41 save/preference files preserved; temporary captures removed.
- Limitation: payment rounds upward to the nearest hundredth; natural trip payoff still needs the user's `101` retest. No commit.
