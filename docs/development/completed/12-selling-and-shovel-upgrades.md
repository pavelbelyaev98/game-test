# 12 - Selling and paid shovel upgrades

- Why: discoveries and shovel progression needed an ordinary earn/spend loop at the surface.
- Integrated result: the approved Blender SELL machine and UPGRADES workbench replace their two pedestals. E opens explicit item sale rows/Sell All or an owned-tool comparison and priced purchase. Five sequential prices: 10 / 25 / 55 / 100 / 180 credits.
- Safety: offers bind exact inventory/wallet revisions and owned level; old UI callbacks, changed contents, insufficient funds, overflow, lost focus, disabled/remote/occluded stations and inventory inspection cannot transact incorrectly. Menus open with Close selected; keyboard focus scrolls item rows into view.
- Assets: explicit approval for the pair recorded in `docs/asset-ledger.md`; `art/stations/Stations.blend`, FBX exports, baked albedo/normal textures and prefabs retained. No new audio, fonts or packages. Only authored flap/drawer transforms animate.
- Checks: 62/62 EditMode and 60/60 PlayMode passed. Live CLI trip: three real finds sold for 24, level 2 purchased for 10, balance 14; excavation/collected identities preserved. Both animations moved/restored and stopped updating while paused. Evidence: `unity/Logs/Task12/`.
- Windows: rebuilt `builds/windows/SomethingDownThere.exe` at `2026-09-08 15:25 UTC`, zero errors; expected Pipeline-disabled-in-player warning only. Native E/mouse/Escape, station visuals, empty/unaffordable states and 960x540/1920x1080 menu review passed without game exceptions.
- Limitation: state lasts for the scene session. Visible shovel art and independent speed/strength purchasing remain `11`/`25`; disk saving remains separate. No commit.
