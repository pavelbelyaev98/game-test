# Task 113 - Larger rock from the supplied photos

Type: implementation; asset authoring only. Status: `done`. Explicit user-selected model request; no gameplay prerequisite. [Completion and evidence](../completed/113-photo-rock-model.md).

Follow-up: [114](114-rock-variants-integration.md) supersedes the original twin-groove/wide-base model with three integrated appearances; only A retains one central hollow, B/C have solid tops. Current files/specifications live in the linked source guide.

Feature: [discovery content](../../features/backlog/discovery-content.md). Context: [art style](../../features/backlog/art-style.md), [model replacement guide](../replacing-find-models.md), [recognition risks](../design-risks.md).

## Scope and source brief

- The user supplied four views of one rock and requested a larger Blender 3D find. Author this specific original model through Blender MCP. Estimate **70 x 55 x 45 cm** (width/depth/height); this is editable game scale, not a measurement recovered from the photos.
- Match an asymmetric, roughly block-shaped rock with a broad sloping front fracture face, clipped corners, uneven shoulders, low stable underside and a ridged top with shallow erosion grooves. Preserve broad planes before fine detail; avoid a uniformly rounded boulder or noisy spiky silhouette.
- Surface: rough desaturated grey-green stone, warmer olive/tan weathering on one face, small pits/grain, intermittent hairline cracks, sparse off-white weathering/lichen-like flecks and restrained warm chips. No labels, writing, attached leaves, shoes, pavement or environment from the photos.
- Keep the existing game's readable material direction while using this specific rock as the shape reference. Original geometry/materials only; the photos are visual reference, not projected texture pixels or a photogrammetry scan. Final whole-game style acceptance remains `105`.
- Deliver editable `.blend`, reproducible Blender recipe, UV-mapped FBX, separate convex collision FBX, 2048px base-color/tangent-normal/material maps, multi-view previews and exact specifications/ownership under `art/photo-rock/`. Preserve all existing Blender scenes and assets.
- This is a source-asset delivery like `108`. Unity population, price/slots and larger-object physics tuning are not selected by the model request; do not silently replace bottles or mark game integration complete. As an ordinary rock, size alone never makes it detector-eligible or rare.

## Acceptance

- Inspect rendered views and refine shape/material defects against the supplied views. Show a clear finished preview; source and export must use the same final model.
- Verify dimensions/unit scale, closed finite mesh, normals, UV range, material/triangle budgets, dedicated convex collider and texture paths. Reopen the saved source and reimport exports through Blender MCP to check the actual artifacts.
- Record original source/license, authoring approval, recursive ownership and precise removal/replacement steps in the asset ledger and local README. Preserve the four-view reference description above so future work does not depend on chat alone.
- Keep final source/edit controls available. Completion proves the requested asset delivery, not burial/player-feel acceptance or a changed Windows build.

Questions: none blocking asset authoring; gameplay allocation/value and integration approval remain separate from this source delivery.
