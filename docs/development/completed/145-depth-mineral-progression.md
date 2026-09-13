# 145 - Deeper excavation and mineral progression

- Why: the user needs deeper digging and varied, increasingly valuable minerals to test the game feel.
- Result: MainGame expands from 12 to 32 m without changing its 24 × 24 m footprint, surface or soil art. New Game has 928 minerals and 96 rocks, including 312 shallow finds. [Selected bands/counts](../tasks/145-depth-mineral-progression.md).
- Coal → Copper → Iron → Silver → Gold → Emerald → Ruby → Diamond sell for $2/$4/$6/$9/$13/$20/$30/$45. All use the explicitly approved Blender models, textures and collision hulls with existing exposure, pickup, handling and inventory. [Sources](../../../art/minerals/README.md), [ownership](../../asset-ledger.md#task-145---depth-progression-minerals).
- Continue extends old 12 m terrain downward while preserving all original density samples, excavation, identities, poses and historical prices. Existing populations remain; New Game supplies the full mineral roster.
- Existing ambient minimum rises to 0.45 for deep readability; open sky remains 1, with sun occlusion and connected-air falloff intact. Task `68` equipment choices remain paused.
- Evidence: 130 relevant checks pass, including 100-seed shallow coverage/separation, 20-seed mineral bands, four blind excavation patches, real excavation to the 32 m boundary, collection/sale/reload and old-save migration. CLI inspection covers all eight models, prices, deep readability and the grassy surface. [Validation](../../../unity/Logs/Task145/validation-summary.json).
- This machine: complete 1,024-find world initialization 1.61 s; seeded layout 490 ms; shared-density capture 0.22 ms. These are observations, not minimum-hardware qualification.
- [Windows build](../../../builds/windows/SomethingDownThere.exe): 2026-09-13 09:58:37Z, zero errors / 1 existing disabled-Pipeline warning; clean seven-second native startup. Editor domain reload cleared an initial Unity URP build-preprocessor reference error before the successful build.
- All 41 profile files preserved, temporary review images removed, source art retained, MainGame clean/stopped. No commit.
- Limitation: natural trip payoff and full-run price/depth tuning still need the user's `101`/`103`/`37` playtests. Next eligible task is `106`.
