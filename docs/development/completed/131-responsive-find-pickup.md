# 131 - Responsive collection and pickup motion

- Why: a directly aimed half-covered find became eligible on a dig but waited for another recognition cycle. The user also requested shrink/travel feedback and walking pickup.
- Integrated: a successful revealing stroke collects that same already-aimed eligible find immediately, with normal fuel/range/cooldown and one identity transfer. Ordinary held/toggle recognition and off-aim scoop protection remain.
- Grounded walking collects fully uncovered nearby floor finds through foot contact/visibility checks. Partial burial, obstacles, full bags, menus/focus, airborne/idle movement and held objects block it. Explicit releases require leaving and returning.
- Presentation: a bounded pool reuses approved meshes/materials for pickup effects; [133](133-grass-performance-and-pickup-feel.md) replaces the original 0.42-second shrink/sweep with a 0.18-second direct pull and at most 15% shrink. Original inventory/absence commits immediately; animation freezes on pause and clears on completion, restore, return or shutdown.
- Evidence: **49 distinct checks pass**, including all six appearances, weak/strong held/toggle input, full bags, handling and save/reload while the effect is active. pickup validation record; [test/build evidence](../../../unity/Logs/Task131/).
- Windows build: **2026-09-12 16:51 UTC**, zero errors / one existing Pipeline runtime warning; clean seven-second native startup. Owner saves remain untouched.
- Remaining: user pickup feel verdict belongs to 101; native minimum-hardware qualification remains 54. Reservoir artwork remains staged under 130's existing approval request.
