# Completed work

Point-in-time delivery index; later records supersede earlier numbers. Current behavior and open work live in the [feature contracts](../features/readme.md) and [status](status.md). Full per-task evidence is preserved in git history (`docs/development/completed/` before the 2026-09 docs cleanup).

## Foundation and tooling
- `01` Repository foundation — Unity project, docs ownership and asset-provenance policy established.
- `02` Feature backlog — 15 linked feature docs cover all 54 concept sections.
- `03` Package and URP — Unity 6000.6.0f1 on URP 17.6.0 with Input System throughout.
- `04` Core loop plan — scene ownership and return/overextension acceptance defined.
- `17` Unity CLI — official CLI 1.0.0-beta.8, Pipeline 0.6.0-exp.1 and agent plugin installed.
- `18` CLI and skill audit — earlier completion records re-audited; `05`-`07` reopened for presentation.
- `73` Concise task navigation — status.md entry point; 45 short queue links, stable IDs.
- `88` Save setup API warning — obsolete lookup replaced with FindAnyObjectByType; warning-free compile.
- `146` Distinctiveness feedback triage — idea and 25 specs updated; tickets `147`-`153` added.

## Core gameplay and controls
- `05` FPS foundation — CharacterController movement, camera look and Input System contracts.
- `16` Window and jetpack — resizable window; final-fraction thrust; jump/hold flight.
- `66` Precision movement design — held Left Ctrl true crouch selected; acceptance defined.
- `67` Precision movement — smooth crouch at 35% horizontal speed; safe standing; saved stance.
- `86` Automatic rescue and find digging — zero-fuel auto-rescue; digging clears covering soil.
- `87` Modest sprint — Left Shift 1.35x speed (4 to 5.4 m/s), crouch priority, remappable.
- `121` Input preference recovery — damaged binding maps restore Hold and all default keys.

## Terrain and excavation
- `06` Terrain shell — finite occupancy grid, chunked mesh and collision.
- `20` Smooth digging — signed density surface nets at 0.125 m; six scoop tiers (widths later rebalanced by `24`/`127`).
- `21` Organic digging — seeded 12% contour variation; review strength/hold controls.
- `23` Developer admin and shovel reach — Ctrl+Shift+F10 admin tooling; reach later capped at 4 m (`24`/`127`).
- `24` Shovel rebalance — radii 0.44-1.04 m, reach 2.4-4.0 m, bounded volume growth.
- `26` Detached soil cleanup — unsupported soil removed within the same accepted stroke.
- `29` Irregular shovel bites — surface-oriented asymmetric cuts; volume popups removed.
- `31` Gentler shovels and pickup tuning — radii 0.41-0.96 m; 50% exposure; one-line warning.
- `34` Tiny terrain remnants — thin spikes/slivers cleared locally; supported structures kept.
- `120` Powered excavation feel design — earlier review complete; reopened for progression details.
- `154` Experimental excavation isolation — normal digging restored; silent admin-only trial modes.

## Discoveries and content
- `07` Session inventory — ten slots, immutable IDs/names/values and read-only inspection.
- `27` Buried finds and admin X-ray — 96 seeded finds (24 shallow); X-ray markers and readout.
- `28` Small find pickup and quiet HUD — visible slivers collect; LMB pickup; quieter guidance.
- `30` Larger finds and held collection — doubled prefab sizes; held pickup at 60% exposure.
- `72` Pickup exposure and doc cleanup — all finds need 40% exposure (later 60%, `110`); absence reports removed.
- `89` Starter minor find design — bottle/can/brick batch: 7 variants, 72 instances, 2/1/3 credits (bottles retired for new games by `145`).
- `108` Starter find models — Blender bottle/can/brick batch, nine 2048px maps, 9,508 triangles.
- `109` Starter find trial integration — 72 finds, 2/1/3 credits, legacy saves migrated.
- `110` Bottle physics and recognition — three bottles 25% larger, 60% exposure, physical release.
- `111` Deliberate discovery pickup — fresh-press pickup; superseded by `112` after user review.
- `112` Held aimed pickup — continuous held pickup on the centre ray; off-aim finds protected.
- `113` Photo rock model — angular grey-green rock source; 12,500-tri visual, convex collider.
- `114` Rock variants integration — three rock appearances (A hollow); 72 bottles + 24 rocks.
- `115` Find handling and recognition — RMB lift/drop, LMB throw; 0.6 s observation later removed by `136`.
- `116` Find hold and rest stability — carried finds track player motion; dropped bodies settle.
- `126` Shallow find density — 312 rocks, 240 at 0.65-1.1 m, 2 credits each.
- `131` Responsive find pickup — aimed reveal collects at once; walking pickup; 0.18 s pull.
- `136` Immediate held collection — held/toggle input collects eligible finds before cooldown.
- `140` Shallow rocks and fuel warning — shallow/deep rock split; staged HUD fuel alerts.
- `145` Depth mineral progression — 24 × 24 × 32 m; 928 minerals, eight ores, $2-$45.

## Progression and economy
- `12` Selling and shovel upgrades — SELL machine and UPGRADES bench; 10/25/55/100/180.
- `13` Surface recharge — free instant refill at the time; later explicit paid service (`139`/`98`).
- `14` Confirmed rescue — Pause rescue previews losses; later replaced by automatic rescue (`86`).
- `46` Battery upgrades — 100/150/200/300/400 capacity, prices 6/14/28/48; charge kept.
- `48` Inventory upgrades — 10/15/20/30/40 slots, prices 6/14/28/48; identities kept.
- `98` Refill economy design — explicit paid service, partial refills, no automatic billing.
- `138` Compact upgrade workshop — one row per upgrade with hover/keyboard details pane.
- `139` Paid surface refills — explicit purchase; rate later changed by `141`/`143`.
- `141` Early fuel economy — digging costs 1 fuel; 100 fuel per credit; starter refill $1.
- `142` Refill correctness and pricing — commit before charging; fractional rounding; save compatibility.
- `143` Whole currency and symbol — whole-dollar amounts, $1 minimum, `$` used everywhere.

## UI, settings and accessibility
- `32` Status notice subtitles — reserve/battery/recharge notices title-only; menus keep detail.
- `33` Sharp text scaling — HUD labels refreshed after canvas scale; later replaced by Toolkit.
- `64` Camera comfort design — 75° default, 55-90° slider and steady crosshair selected.
- `65` Camera comfort settings — FOV/crosshair preferences persist per device; reset and Retry.
- `74` UI Toolkit menus — Pause/inventory/shop/rescue/developer menus on shared UXML/USS.
- `75` UI Toolkit HUD — one UI Document owns HUD and menus; Canvas HUD removed.
- `78` Input accessibility — twelve actions rebindable; optional Toggle digging; conflict swaps.
- `79` Startup menu — New Game/Load/Settings/Quit; single-instance launch reservation.
- `107` UI/UX cleanup — white/charcoal theme, Continue first; 140 EditMode + 47 PlayMode pass.
- `122` Common PC settings — 19 settings + 12 bindings across five categories; frame pacing.
- `123` Consistent menu components — shared buttons/tabs/rows; 16 settings + 12 bindings.
- `124` Monochrome control states — grayscale widget states; quiet release save-failure exit.
- `125` Subtle menu hover — restrained grayscale hover fills; tightened dropdown spacing.

## Site and presentation
- `19` Presentation rollback — added art/audio removed; 4x MSAA, linear color; rim fixed.
- `77` Ground textures — six seamless 2048px soil/turf maps on one triplanar material.
- `105` Art style texture trials — Sunny r8 palette approved; ground, shader, daylight integrated.
- `127` Moving grass and shovel strength — wind-blown Blender grass; digging 40% weaker.
- `128` Visible sun and grass density — Blender sun disc; constant twelve-blade grass density.
- `129` Grass shape and wind — taller curved blades; travelling gusts; per-blade tips.
- `132` Brighter sparse clouds — pale cumulus forms replaced random gray domes; clouds later removed by `135`.
- `133` Grass performance and pickup feel — 88% less grass GPU cost at the time; 0.18 s pickup pull (grass later revised by `134`/`135`).
- `134` Authored surface and volume clouds — grass coverage, 49 stones, four 3D cloud volumes (clouds/stones later removed by `135`).
- `135` Restore grassy site — whole-surface turf; clouds/stones/scenery removed; 7,996 clumps.

## Persistence, release and qualification
- `35` Save and load — versioned whole-world snapshots; 10 s autosave; Save and quit.
- `63` Windows targets and budgets — Windows 11/1080p60 targets; edit/save/memory budgets.
- `76` Isolated Windows review — cancelled; sandbox tooling and references removed.
- `90` Steam review gap audit — 375 reviews sampled; 15 task specs strengthened.
- `92` Player feel risk research — 259 more reviews (634 total); risks mapped to owners.
- `95` Player ideas and playtest plan — nine ideas assessed; five playtest sheets `100`-`104`.
- `144` Git LFS storage — eight Blender sources tracked; push repaired; 179 MB uploaded.
