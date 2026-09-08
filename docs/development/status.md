# Current status

Task `12` is done: the surface checkpoint now sells carried finds and purchases sequential shovel upgrades.

- E opens the approved SELL machine or UPGRADES workbench. Item rows/Sell All transfer exact finds for credits; five purchases cost 10 / 25 / 55 / 100 / 180. Menus show results, affordability and owned-tool effects, with stale-command and focus/range/visibility protection.
- All 122 checks pass: 62 EditMode + 60 PlayMode. Official CLI live review uncovered/collected three actual finds, sold them for 24 credits, then bought level 2 for 10; balance 14, excavation and collected identities preserved. Both model animations moved/restored correctly while paused. Evidence: `unity/Logs/Task12/`.
- Windows development build rebuilt at `2026-09-08 15:25 UTC`: `builds/windows/SomethingDownThere.exe`, zero errors, expected Pipeline-disabled-in-player warning only. Native review verified both stations, E/mouse/Escape controls, empty/unaffordable states and readable 960x540/1920x1080 menus; no game exceptions.
- The user-approved Blender pair replaces only the station pedestals. Retained source: `art/stations/Stations.blend`; exports/textures/prefabs and removal steps are in the asset ledger. No new audio or packages.

No active task or blocker. State remains scene-session only; disk saving, visible shovel progression (`11`) and independent speed/strength purchases (`25`) remain separate work. No commit.
