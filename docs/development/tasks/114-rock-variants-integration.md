# Task 114 - Refine and integrate three rock appearances

Type: implementation. Status: `done`. User-selected follow-up to [113](113-photo-rock-model.md). [Completion and evidence](../completed/114-rock-variants-integration.md).

Feature: [discovery content](../../features/backlog/discovery-content.md); shared [collection rules](../../features/backlog/discovery-collection.md), [replacement guide](../replacing-find-models.md), [recognition risks](../design-risks.md). Prerequisites: source `113`, physical collection `110`/`112`.

## Selected outcome

- Replace the rejected paired top cuts with one irregular shallow central hollow. Narrow and break up the underside; avoid a level perimeter, box/chest rear, matching parallel walls or artificial lid seam. User-provided closeups clarify the original rock reference; preserve broad sloping fracture faces and stone grain.
- Author three distinct Blender MCP silhouettes: A retains the single central hollow; B and C have solid uneven tops with no hollow (explicit user refinement). All are the same ordinary rock, each within the existing 0.5 m placement radius. Starting envelopes are approximately 70 x 55 x 45 cm, with variant proportions adjusted below that clearance. Keep original editable sources, individual baked maps, convex hulls and previews.
- Integrate as **one Rock catalog item with three randomly chosen appearances**, not three named/value/rarity tiers. Use seeded random orientations at spawn and preserve the appearance/pose through saves. Stable per-appearance save-content keys may distinguish exported geometry without adding item categories or changing the save format.
- Implementation tuning: 24 rocks in addition to the retained 72 bottles; keep the existing 24 shallow bottles and place rocks in the remaining depth pool. All rocks share 1 credit, 1 inventory slot, common/detector-silent status and 60% exposure. These are editable starting numbers, not final economy acceptance.
- Rock uses the large-find surrounding-soil excavation rule and current held/toggle pickup once directly aimed and eligible. Existing support/release physics lets it fall/tip/settle; no auto-collection from digging radius, damage, breakage or carrying mechanic.
- Explicit approval: the user requested this corrected three-appearance batch **and game integration**. Reuse original material source/license; import into isolated `Assets/Content/PhotoRocks/` with reversible ownership. Preserve bottle/terrain assets and existing saves.

## Acceptance

- Inspect Blender front/rear/top views: only A has a central depression; B/C have solid crowns. All have varied shoulders and narrower irregular foot; no twin slots or flat box pedestal. Validate closed finite meshes, UVs/normals, textures, dimensions and convex hulls after export.
- MainGame uses one catalog entry and three appearances with identical gameplay specs. Seeds reproduce layout/rotation/appearance; multiple seeds exercise all shapes and orientations. Meshes remain below ground and within spacing/site bounds.
- Existing bottle/legacy saves load unchanged. Saved rocks retain exact appearance, pose, value and collected state; replacing a model does not change its stable key.
- Relevant EditMode and PlayMode checks cover catalog integration, rock reveal/held pickup, full bag/off-aim behavior, physical release and save restore. Run a fast deterministic compile/check after edits.
- Inspect actual MainGame rendering/physics via official Unity CLI and deliver `builds/windows/SomethingDownThere.exe`. Record evidence, replacement/spec references and current asset ownership; final subjective style/feel remains user review.

Questions: none blocking; use the existing ordinary-item interaction and editable tuning above.
