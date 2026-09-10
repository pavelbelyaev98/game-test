# Task 109 - Integrate replaceable starter-find trial models

Type: implementation. Status: `done`. Prerequisites: 89, 108. Explicit user-selected early integration; `09` retains final production presentation and `105` final style acceptance.

Feature: [starter batch](../../features/backlog/starter-find-batch.md), [collection](../../features/backlog/discovery-collection.md). Context: [review findings](../../research/player-review-findings.md#progression-and-discovery), [player feel](../../research/steam-review-audit/player-feel.md).

## Selected scope

- The user explicitly approved integrating the revised seven models and says better models will arrive later: **trial visuals, not final art acceptance**. Integrate the specific original Blender batch into MainGame and the Windows build using existing interaction/feedback rules; preserve user terrain and other content.
- Use the batch's 72 exact new-game allocations, initial values, metre dimensions and 24 shallow finds. Keep all common/minor finds silent, 40% exposure and held collection. No new physics, detector, UI, sound, timing or world-design scope.
- Separate stable item IDs/gameplay from replaceable FBX/material references. Supply repeatable import/setup that regenerates centred geometry, colliders and real surface samples while keeping prefab GUIDs and save IDs. Keep original source, manifest and active Unity assets clearly distinguished.
- Implement the recorded legacy content-ID migration in memory before normal checkpointing. Preserve old 96 identities, placement, collected absence, carried records and historical values; use correct replacement scale and update uncollected names only. Unknown content blocks load instead of resetting the world.
- Record exact asset ownership/approval/removal and a concise replacement guide covering model/material changes, every reference, gameplay/spec changes, compatibility and relevant docs. Update the existing owning docs; avoid a second competing roster.

## Acceptance

- CLI imports and inspects all seven textured models in MainGame. New games contain the exact counts with correct dimensions and authored real-surface sampling; no proxy sphere/ellipsoid is used for their visual or hit collider.
- Deterministic checks cover allocations, old/current save restore, repeat migration/checkpoint recovery, rejection of missing content and stable replacement identities. Playable checks cover burial/visibility/reach, weak/strong held digging, one-time pickup, full bag, sale and saved absence.
- Verify repeat setup preserves asset references/GUIDs and save identity. Retain the original development prefabs as reversible legacy assets, with no active MainGame references.
- Provide one current Windows build and visual evidence; mark remaining subjective trial-art/readability limitations and final `09`/`105` work explicitly. User replacement models are anticipated, not approved unseen assets.

Questions: none blocking this scoped approved integration. The user reviews the trial build and will provide replacement art later.

Result: [integration, compatibility, build and validation evidence](../completed/109-starter-find-trial-integration.md). Final model/style acceptance remains open in `09`/`105`; [replacement guide](../replacing-find-models.md) is the entry point for future art swaps.
