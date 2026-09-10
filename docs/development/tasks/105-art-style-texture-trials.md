# Task 105 - Research, test and establish the shared art style

Type: design/research with practical visual trials. Status: `ready`. Prerequisites: existing MainGame and user-owned terrain; independent of paused `56`/`68`. Specific trial asset additions require the normal concrete batch approval before entering the project.

Feature: [art style](../../features/backlog/art-style.md). Reference: [JulioVII — Stylized Grass & Dirt](https://juliovii.itch.io/ftpgrass-dirt). Consumers: `57` presentation, `08` foundation, `89`/`09` starter finds, `58`/`39` terrain, `40` later content.

## Scope and research

- Establish how textures and assets belong to one consistent game. Study the preferred reference, current terrain/material mapping, retained stations/HUD and object readability; produce a concrete comparison brief, not just a mood board or questions.
- Propose an initial **three meaningfully different coordinated texture packs**, varying a controlled combination of palette, painted detail, stone shape/scale and material response. Trial a manageable shared set such as dirt, sand, gravel and rock; add grass only if it belongs to the specifically reviewed terrain batch. Exact material coverage follows inspection, not every reference download.
- Create/obtain and test **multiple texture packs, then revise or create further packs until the user likes and explicitly selects one**. There is no fixed iteration limit and no automatic “closest candidate wins.” Preserve useful reasons for rejection while keeping the current comparison scannable.
- Research feasibility and exact commercial licenses before proposing each source. For custom visual work use Blender MCP and retain `.blend` sources/exports; downloadable alternatives must permit commercial game use. Do not substitute image generation, Unity primitives or code-authored stand-in art. If required tooling is unavailable, record the concrete blocker before creation.
- Prepare each actual candidate batch with previews, purpose, source/license, intended files/integration and precise removal/restoration steps. Obtain specific approval before creation/import under repository asset rules, then record actual ownership in the ledger. Preserve user-owned assets and make comparison/restoration independent for each pack.

## In-game comparison and revision

- Use the same MainGame excavation, camera routes, exposure conditions and fixed lighting for each candidate. Include untouched surface, sloped/vertical cut faces, a curved tunnel and repeated digging; material preview spheres alone cannot qualify the style.
- Compare ground at shovel distance, normal collection reach and farther views: tiling, projection seams/stretching, repeating patterns, normal-map noise, grazing-angle shimmer and whether stone detail looks like fake physical geometry.
- Check silhouette/color separation with retained real assets and, when approved, the three selected common finds. `89` supplies names/specifications before `09`; any approved trial models are owned here in the ledger and reused by `09` for production integration, not created twice. Do not require completed `09` to run these trials. If matching assets are absent, leave that check unvalidated instead of inventing substitute final props or claiming completion.
- Record reasonable texture scale/density, resolution/mipmap/filtering, material-response and memory/frame-cost guidance against [63's budgets](../../features/backlog/release-validation.md#release-budgets). Judge runtime output, not source resolution. Keep material types distinct while avoiding noisy surfaces, glowing finds or blanket outlines as a readability repair.
- Deliver labeled comparable screenshots and a Windows build for each integrated review round at `builds/windows/SomethingDownThere.exe`. Use official Unity CLI inspection when available. Clearly state which candidate the build contains; keep comparison evidence tied to its pack/version.

## User feel checks for each round

| Check | Intended feeling | Verdict / notes |
| --- | --- | --- |
| Overall direction | I like looking at this world and want the rest of the assets to follow it | UNTESTED |
| Digging surfaces | Fresh cuts and deep walls look intentional, as good as the surface | UNTESTED |
| Common-find recognition | I notice and recognize exposed belongings without detector guidance | UNTESTED |
| Consistent objects | Ground, stones, finds, shovel and retained stations share a visual language | UNTESTED |
| Sustained viewing | Repetition, shimmer and detail do not tire me or bury the useful shapes | UNTESTED |

## Acceptance and handoff

- The user selects a tested pack/style they like; relevant rows are OK on recorded evidence, with failed cases revised and retested. Missing asset/tool conditions remain unvalidated, never an implied pass. Do not declare completion because three options were shown.
- Update the owning feature with the accepted palette, texture/shape/material rules, representative views, asset matching examples, runtime constraints and rejected directions/reasons. Future assets follow this guide; a material change to the selected style requires review rather than silent drift.
- Update `57`, `08`, `09` and `58` to consume that recorded choice. Separate selection of a visual direction from per-asset approval and from implementation of the whole game's presentation. Restore/remove rejected trial integrations according to their recorded ownership, keeping selected evidence and user-owned work intact.
