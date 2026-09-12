# Task 105 - Research, test and establish the shared art style

Type: design/research with practical visual trials. Status: `done`, **2026-09-12**. The user accepted the delivered Sunny r8 result (“its perfect now”) and authorized closing this task. [Completion record](../completed/105-art-style-texture-trials.md).

Feature: [art style](../../features/backlog/art-style.md). Prerequisites: existing MainGame and original soil/turf; independent of paused `56`/`68`. Original-ground refinement retained Task 77's direct-creation approval. Consumers: `57`, `08`, `09`, `58`/`39` and `40`.

## Selected scope and decisions

- Establish a bright cartoon direction inspired by **Berry Bury Berry** and **A Game About Digging a Hole**, through practical ground trials, MainGame comparisons and a concrete reusable style guide. Sunny colors were selected first; surface quality was iterated until the user accepted r8.
- Accepted: warm textured soil, sparse earthy mineral fragments, clear short turf with a thin irregular cut fringe, high-quality sun/contact shadows and daylight that fades gradually through the excavation. Preserve the selected palette and avoid washed-out surfaces or painted/translucent stone faces.
- Initial alternatives: A Sunny, B Soft and C Bold. B weakened cream-label separation; C was too busy. R1/r2 were rejected for washed-out/flat detail, r4 for excessive stones, r6 for residual blur and r7 for rapid darkness and stone ghosting. [Shared guide](../../features/backlog/art-style.md) retains the matching rules and rejected directions; do not reopen palette selection.
- The user supplied four Digging a Hole reference screenshots. Official game galleries and [JulioVII's grass/dirt example](https://juliovii.itch.io/ftpgrass-dirt) informed comparison only; none of their artwork was imported. All revised art uses the existing approved original Blender scope, with editable sources and ownership in the [ledger](../../asset-ledger.md#task-77---original-soil-and-short-turf).
- The user accepts the current ground without requiring the unimplemented upright moving-grass batch to block style selection. Its concrete brief/integration/approval remains future work in [08](08-production-presentation.md#moving-grass-handoff-from-105). Final finds, shovel and broader scene presentation retain `09`, `11` and `08`/`57`; hardware qualification retains `54`. No light-item design or addition is selected; `68` remains paused.

## Integrated result

- Six 2048px maps: r8 soil albedo, r7 soil normal/mask and r6 turf; stable world projection, clear material boundaries and preserved texture identities. The soil keeps 110 embedded stone/chip identities; r8 selects coherent mineral samples and removes heavy dirt-pattern coating. Soil pixels outside stone coverage remain identical.
- Soil/turf use 2 m / 1.25 m tiles, a narrow 0.035 m turf cap, BC7 color/masks and BC5 normals with mipmaps/16x anisotropy. Four-cascade 4096px sunlight and full-resolution contact shading preserve depth. The guide records memory/render costs and matching rules for future props.
- Connected-air daylight retains measured ambient multipliers of about 0.81 / 0.55 / 0.35 at 1.5 / 4 / 7 m in the open shaft, with sealed spaces at 0.14. Roof changes/restore update the field. The reserved `OnTerrainChanged` callback-name error was repaired without suppressing the Development Console.

## Acceptance evidence

- **User review passed:** the 2026-09-12 14:01 UTC [Windows build](../../../builds/windows/SomethingDownThere.exe) was accepted as “perfect”; overall direction, ground definition and natural-light revision are accepted. Future object/grass production is not implied by that verdict.
- **7 targeted tests pass:** 4 connected-air transport cases, MainGame integration, callback/material lifecycle and terrain seam/collision/re-enable. Build succeeds with zero errors and one existing Pipeline tooling warning; 7-second native headless startup is clean. [Results](../../../unity/Logs/Task105/R8/).
- **Visual evidence:** the original review inspected twelve matched r7/r8 views and two closer mineral views. After user-requested cleanup, the [accepted-style page](../art-review-105/index.html) keeps six representative final views; superseded and temporary images were removed. Native graphics/performance qualification is not inferred from these checks.
- Six texture GUIDs and all 33 save/preference files were preserved. The albedo keeps 4,128,790 non-stone pixels unchanged; stone coverage remains about 1.56%. MainGame was returned clean/stopped with temporary pipeline overrides removed.
- `57`, `08`, `09` and `58` consume the accepted guide. [Image/source retention](../art-review-105/ground-brief.md#image-and-source-retention) distinguishes runtime art from editable sources, old trials and review screenshots. Requested cleanup retains selected maps, editable sources, six final screenshots and validation reports; superseded image exports and packed cartoon bakes are pruned.

Questions: none remain for this style-selection task. Next eligible task: [126 — shallow find density](126-shallow-find-density.md), then `106`.
