# Task 13 - surface recharge and return warnings

- Why: the shared dig/flight battery needed a real surface refill and readable reserve pressure; this slice can precede paid transactions.
- Integrated: `SurfaceRecharge` on the existing main-scene anchor refills instantly for free when feet are inside the 4 x 3 m footprint and 0-0.35 m above the surface. Underground overlap, disabled players/zones and menu/focus suspension cannot refill; resume/re-entry works while stationary.
- HUD: tunable safe/risky (<=35%)/critical (<=15%) bands, a reserve bar, empty-charge handling, nearby-zone guidance and a brief refill confirmation. Bands describe charge, never a guaranteed return cost. Paused inspection freezes warning transitions.
- Evidence: 45/45 EditMode and 50/50 PlayMode checks; final HUD percentage/contrast correction rechecked with 7/7 UI tests. Real terrain/flight share power; recharge preserves carried identity, shovel level and excavation.
- Review: official CLI main-scene captures and Windows keyboard flight through risky/critical/empty, followed by walking back to 100% recharge. HUD/menu inspected at 1920x1080, 960x540 and 1280x800. Evidence: `unity/Logs/Task13/`.
- Build: `builds/windows/SomethingDownThere.exe`; succeeded with zero errors and the expected disabled Pipeline player-services notice. No script exceptions/errors in the native player log.
- Scope: no new art/audio, fonts, dependencies or launchers; reused existing HUD/font and scene anchor. No commit. Rescue, transactions, disk persistence and the deferred presentation pass remain separate tasks.
