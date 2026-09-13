# Task 145 - Deeper excavation and mineral progression

Type: implementation. Status: `done`. Explicit user-selected playtest work, ahead of `106`. [Completion and evidence](../completed/145-depth-mineral-progression.md).

Prerequisites: saved excavation `35`, catalog/rock handling `109`/`114`, selling `13`, whole money `143`.
Features: [excavation](../../features/backlog/excavation-terrain.md), [discovery content](../../features/backlog/discovery-content.md), [generation](../../features/backlog/discovery-generation.md), [selling](../../features/backlog/selling-upgrades.md).

## Selected scope

- User requests substantially deeper digging and Coal → Copper → Iron → Silver → Gold → Emerald → Ruby → Diamond by depth, with adjusted selling prices. This selects collectible minerals previously deferred to `40`; no refining, crafting, resistance tiers, detector changes or new scenery.
- Target 32 m depth at the existing 24 × 24 m footprint and 0.125 m resolution. Keep the surface, approved terrain art, digging, collection, handling and purchases.
- Keep deep mineral colors readable using a 0.45 minimum for existing ambient daylight (previously 0.14); open-sky lighting remains 1, with sun occlusion and connected-air falloff intact. This numerical adjustment adds no lamp/equipment and leaves `68` paused.
- New games receive overlapping mineral bands with increasing whole-dollar values and broad lateral coverage. Tune counts/ranges/prices here; the existing 1024-identity ceiling remains.
- Preserve old saves, excavation, poses, inventory and historical prices with a tested downward terrain extension. Existing populations are retained; New Game supplies the full mineral distribution.
- Original Blender MCP mineral art requires a concrete batch preview and explicit approval before import. Prepare outside Unity Assets; retain source/exports/license and record approved ownership/removal in the ledger.

## Acceptance and validation

- Dig below the former 12 m floor down to the new boundary in MainGame; no pre-dug route, broken walls or changed surface alignment.
- All eight recognizable minerals occur in the intended order/bands, sell individually and together for their listed values, and retain identity/value/pose through reload.
- Seeded generation replays with valid separation, bounded work, shallow encounters and lateral/depth coverage across representative seeds.
- Old checkpoint migration preserves every density sample relative to the surface, discovers no duplicate loot and round-trips without repeated expansion. Reject unrelated incompatible layouts through recovery.
- Fast deterministic compile, focused generation/terrain/save/trade integration checks, official CLI visual review, Windows build and native startup. Measure larger-world initialization/capture costs; do not claim minimum-hardware qualification.
- Keep profiles unchanged during review; delete temporary captures after approval/completion. Update owning contracts, queue and status; no commit.

Art approved: user answered **"Approve this eight-mineral batch"** to the Blender preview, including textures and collision models. [Ownership and reversal](../../asset-ledger.md#task-145---depth-progression-minerals). Source: [catalog](../../../art/minerals/catalog.json).

| Mineral | Centre depth (m) | Sale | Count |
| --- | --- | --- | --- |
| Coal | 0.65–5; 216 at 0.65–1.1 | $2 | 240 |
| Copper | 3–9 | $4 | 160 |
| Iron | 7–13 | $6 | 128 |
| Silver | 11–17 | $9 | 112 |
| Gold | 15–22 | $13 | 96 |
| Emerald | 20–26 | $20 | 80 |
| Ruby | 24–29 | $30 | 64 |
| Diamond | 28–31.2 | $45 | 48 |

Retain 96 ordinary shallow rocks at $2: total 1024 finds, 312 shallow. Minerals are repeatable ordinary sellable discoveries with one slot, 60% exposure, existing physics/handling and detector silence. This batch does not select a unique-find policy. Numerical tuning belongs here; full-run balance remains `103`/`37`.
