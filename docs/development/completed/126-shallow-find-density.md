# 126 - Shallow rock-only playtest

- Why: the user wants to judge the digging/collection loop with only rocks and more shallow encounters.
- MainGame new games contain 312 rocks in three existing appearances: 240 at 0.65–1.1 m centre depth and 72 deeper. Bottle spawn quotas are zero; retained bottle assets/aliases keep old saves readable.
- New rocks sell for 2 credits: five buy the first shovel, a ten-slot bag sells for 20, and shallow finds can fund the 370-credit shovel track. Historical saved values remain unchanged.
- Slow supported contact damping is stronger for newly sampled rocking poses; strict settling, dynamic support removal, throwing and one-identity collection remain intact.
- Evidence: **93 relevant checks pass**, covering catalog/100 seeds, codec, fixed-patch pacing, collection/physics, save/trade/upgrade and startup. Source catalogs, prefabs and MainGame agree. [Validation](../../../unity/Logs/Task126RockOnly/validation-summary.json).
- Official CLI inspection: four fixed ~2 m patches using the default 0.345807 m shovel reveal 3–4 rocks in 36–37 strokes, first seen after 10–13; 26–28 battery remains. One or two are pickup-ready; larger partly buried rocks need aimed finishing. This is scripted digging, not a human timing verdict.
- Live MainGame collection transfers one directly aimed rock with no extra stroke/fuel or duplicate. Windows build **2026-09-12 21:47 UTC**, zero errors; clean seven-second native startup. [Executable](../../../builds/windows/SomethingDownThere.exe).
- Saves/preferences preserved; temporary captures removed, approved art and pending 137 preview retained. No commit. Next eligible task: `106`.
- Limitation: choose **New Game** for rock-only density; Continue retains its saved population. Human feel/novelty and full-run balance remain `101`/`37`/`40`.
