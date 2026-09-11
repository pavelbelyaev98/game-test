# 126 - More finds in the shallow starting layer

- Why: the user requested more frequent buried discoveries near the beginning.
- New games contain 192 finds: 120 shallow bottles/rocks instead of 24, with 48 near the starting rim and coverage across the upper site; 72 remain deeper. Source catalogs own quotas, values and appearances.
- Seeded candidate selection spreads encounters while retaining burial and 1.15 m separation. Existing 72/96-find saves keep their population, identities, poses and historical values.
- Small supported contact oscillations are damped before the existing quiet-window sleep check; newly sampled rock orientations now settle and wake when support is dug away.
- Evidence: **159 EditMode + 14 physics + 17 discovery + 12 saving + 8 startup = 210 passed**. Coverage sampled across 100 seeds; approved mesh clearance, full-bag/held/toggle collection and legacy restore/resave checked. [Results](../../../unity/Logs/Task126/).
- CLI MainGame inspection: all 192 initially buried; scripted clearing with the default 0.41 m shovel exposed six finds, then collection transferred one bottle and left five in place. [Excavation](../../../unity/Logs/Task126/shallow-excavation.png), [pickup](../../../unity/Logs/Task126/collected-bottle.png). Refills and a controlled camera were used; this was not an expedition timing test.
- Windows build: **2026-09-11 05:43 UTC**, succeeded with zero errors and the existing Pipeline runtime-configuration warning. Editor script reload cleared a stale URP build-cache reference after live review. [Executable](../../../builds/windows/SomethingDownThere.exe).
- All 25 save/preference files unchanged; MainGame clean/stopped. No commit. Resume `106`.
- Limitation: density applies to **New Game**; user pacing review remains `101`/`37`, richer shallow categories `40`, and final model/style acceptance `09`/`105`.
