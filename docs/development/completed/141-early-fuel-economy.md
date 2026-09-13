# 141 - Early fuel economy

- Why: the user reports repeatedly collecting just to pay for fuel, without meaningful upgrade progress.
- Result: accepted digging costs 1 fuel instead of 2. Service adds up to 100 fuel per credit instead of 50, rounded up; a full starter refill costs 1 credit. Workshop copy reads the shared rate. Continue gets both changes while preserving charge, capacity, items and upgrades.
- Real MainGame budget: 80 starter digs with 20 fuel retained, versus 40 before; first capacity upgrade permits 130 with that same reserve. Jetpack burn, dig geometry/cadence, rock value and purchase prices remain unchanged.
- Controlled trip ledger: two pairs of fixed shallow patches plus aimed finishing produced 4 then 6 collected rocks. Sales were 8/12 credits; each refill cost 1, banking 7/11. Bought shovel level 2 with 8 credits and a full tank remaining. Each trip retained 20 fuel; travel was not simulated.
- Evidence: 72 relevant checks pass across transactions, four shallow patches, fuel budgets, controller/thrust, warnings/rescue and repeated save loads. CLI inspection confirms the 100-fuel rate, full-tank quote and actual 1-credit purchase. [Validation and ledger](../../../unity/Logs/Task141/validation-summary.json).
- [Windows build](../../../builds/windows/SomethingDownThere.exe): 2026-09-13 07:10:54Z, zero errors / 1 existing disabled-Pipeline warning; clean seven-second native startup. Preserved all 41 save/preference files and removed temporary captures.
- Limitation: controlled patches do not establish natural travel times or full-run balance. User payoff retest remains `101`; full economy and rescue consequences remain `37`/`59`.
