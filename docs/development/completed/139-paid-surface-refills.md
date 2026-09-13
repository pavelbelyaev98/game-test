# 139 - Paid surface fuel service

- Why: the user explicitly requests paying for fuel; [98](../tasks/98-refill-economy-design.md) supplies the concrete policy/ledgers.
- Result: explicit instant workshop purchase, 1 credit per up to 50 fuel added rounded up; cap delivery to missing capacity and available money. Empty starter tank costs 2. Full/no-money actions disable; partial amount and price appear before purchase. Surface arrival/opening/hover never refills or bills.
- Evidence: exact full/partial/zero/fractional charge, stale/foreign/replayed offers, capacity purchase without free refill, paused/focus/re-entry behavior, real digging/flight and checkpoint/reload. Empty wallet/bag/fuel still recover through existing rescue.
- [Shared Windows build and 71-check validation](../../../unity/Logs/Task138/validation-summary.json); [task](../tasks/139-paid-surface-refills.md). Concept section 34 and shop/HUD copy now match paid service.
- Limitation: rate is initial tuning; existing free broke rescue remains exploitable. `59`/`37` own changed consequences and full finite-run economy; no portable fuel is selected.
