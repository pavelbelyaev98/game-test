# Presentation and art style

Owns the selected visual direction, accepted environment/lighting states, runtime matching rules and asset approval context. Audio direction: ambient nature plus digging/action feedback (see Audio).

## Direction
- Accepted: bright cartoon style — fresh leaf greens over warm ochre/terracotta earth, cream labels with teal/amber accents, simple distinctive silhouettes, readable relief. References: Berry Bury Berry (color/shape) and A Game About Digging a Hole (tactile earth/cut faces); no content or mechanics taken, no artwork imported.
- JulioVII Stylized Grass & Dirt is a secondary painted-surface reference only; not downloaded or used. Graphics ambition stays modest and practical.
- The world stays bright, playful and tangible; soil stays quiet enough that a partly exposed find is recognizable. Avoid endless brown mud, excessive darkness, generic procedural scenery, flat placeholders, outlines and large painted stones that promise loot.

## Accepted environment (Sunny r8)
- Ground `105` accepted: six 2048² Blender-baked maps; 2 m soil / 1.25 m turf world tiles; BC7 color / BC5 normals; broad color variation, sparse mineral faces (25 stones + 85 chips per 2 m tile), mostly matte.
- Lighting: gradual daylight falloff with depth, full sun shadows, ambient floor 0.45 for mineral readability at 32 m, open sky 1. Warm cream sun disc in the camera-local skybox that terrain occludes; bright cyan gradient retained.
- Grass `127`–`129`/`135` accepted: short upright blades, subtle rooted wind, excavation-aware support, full visible-site density, seeded ~25–63 cm height/spacing variation and a 2–4 cm ragged cut-lip fringe. Twelve-blade near/far meshes, batched.
- `135` restores continuous turf over the land patch and removes clouds, trees, river/water and decorative ground rocks; `133`/`134` bare islands, clearings, surface stones and cloud experiments stay retired. Do not restore or add replacement scenery without a new request.
- Shadows: 4096 px atlas, four cascades (4.5/11.7/24.75 m) in a 45 m range, high soft filtering. SSAO 8 samples, 0.18 m radius.
- Reservoir: `130`'s full draft is rejected; `137` reviews one real-photo-informed section at a time for credible construction and fitted joints before more building. The cartoon direction does not lower this bar.
- Trial verdict: r1/r2 washed out; r3/r4 wrong turf/stones/caves; r5–r7 blur/darkness issues; r8 accepted after fixes. A/B/C palettes are closed; do not re-ask.

## Matching and budgets
- Soil/turf/stone blend: exclude axes >0.16 below the strongest normal; narrow derivative-filtered boundary (0.035 m cap, 1.5 mm min); shared coordinates/weights for color, normal and roughness. Relief 0.55 soil / 0.90 stone / 0.40 turf.
- Authoring: small props 256–512 px, station/equipment sheets 512–1024 px, mipmaps/filtering kept. Check readability at shovel distance, 3 m reach, in shadow and against grass/soil. Windows budgets apply; `54` owns minimum-hardware qualification.

## Asset rules
- Every new model, pack, font, sprite or sound needs specific user approval before addition; use free commercially licensed assets or Blender MCP with retained `.blend`/exports. Record source, license and rollback in the [asset ledger](../asset-ledger.md).
- No decorative scenery, props or tools. Audio follows the selected direction with per-sound approval. Preserve the user's terrain, existing props and save identities.
- Design briefs (not creation approval): bottle = clean glass + cream label remnants; rock = broad planes with restrained mineral variation; shovel = chunky blade/handle with recognizable hardware; station = broad amber SELL / teal UPGRADES panels.
- UI keeps the cream/teal/amber palette and existing font; `106` owns layout/text; no new font or HUD redesign implied.

## Audio
- Selected: ambient nature (wind, birds/insects, weather, site water) plus action feedback (digging/soil, footsteps, jump/jetpack, collection/handling, stations, menus). No music or voice acting.
- Sound supports, never replaces, readability: common/minor finds stay detector-silent; no audio-only clue or placeholder noise. Every specific sound needs user approval, a free commercial license and a ledger entry (`08` owns the production pass).
- The user wants future excavation guns with implementation deferred; `154` restored ordinary Scoop, removed the trial sounds and isolated the experiment. `56`/`120` define progression before `57` briefs a production silhouette.
- Open: no selected event-priority/station-payoff/affordance package (`57`); final bottle/rock/shovel presentation with `09` and `58`/`39`.
