# Task 110 - Bottle physics and recognition revision

Type: implementation. Status: `done`. Prerequisites: 109. Explicit user-selected revision; broader category physics design remains `96`.

Feature: [collection](../../features/backlog/discovery-collection.md), [starter batch](../../features/backlog/starter-find-batch.md). Context: [physics design](96-discovery-physics-design.md), [recognition/loot risks](../design-risks.md), [replacement guide](../replacing-find-models.md).

## Selected scope and feel

- User reports small items disappear into collection before their appearance can be appreciated. Keep only the three bottle variants, enlarge them 25% in Blender, and require **60% exposure** for held collection. No forced delay or extra button. Keep 72 finds/24 shallow, distributed 29/24/19 with 10/8/6 shallow; all one slot, 2 credits and detector-ineligible.
- Remove can/brick models, textures, materials, prefabs and their source/preview/recipe content. Retain only historical save-ID mappings and useful historical records. Existing uncollected retired finds become bottles; identities, positions and historical values remain intact. No save deletion or world reroll.
- Bottles stay anchored while embedded. Once detached from soil they become physical, fall, tip and settle on actual rebuilt terrain. Pickup exposure and physical release are separate. Retained partial support must not release at the 60% pickup threshold.
- Use restrained, non-destructive motion; no player obstruction/launching, bottle breakage, rolling-loot chore or physical carrying. Recover the same identity from an invalid/out-of-bounds physics pose to its last valid local pose. Terrain reset reanchors reburying bottles; further digging wakes settled bottles.
- Persist released state and current pose with backward save readers. Resume a saved falling bottle from rest at that pose; gravity continues. Motion/settling must make autosave dirty even without player input. Pause/loading must not simulate finds against an incomplete terrain restore.
- Preserve stable bottle IDs/GUIDs, retained source and replacement workflow. All art remains replaceable trial art; broader roster/style decisions remain `40`/`09`/`105`.

## Work and acceptance

- Update approved Blender sources/exports, catalog/importer and all current owning docs/ledger. Verify only three active model variants and no retired asset references; regenerate collider/exposure data for changed dimensions. Use separate Blender convex hulls (200/198/190 triangles) because the detailed visual meshes exceed Unity convex cooking limits; no partial-hull fallback warnings may ship.
- Test 60% eligibility, partial attachment, release/fall/ground rest, subsequent support removal, reset, pause, several adjacent bottles, no player obstruction, full bag, one-time pickup/sale and safe recovery.
- Test legacy 96-find and previous 72-find saves, retired-ID conversion, current/released pose and checkpoint recovery. Keep collected/carried historical names and values consistent.
- Use official CLI for live MainGame inspection with weak/strong digging; provide a current Windows build. Record user feel/readability as awaiting review, not an automated pass.

Technical references: [Unity convex collider rules](https://docs.unity3d.com/6000.0/Manual/class-MeshCollider.html), [Rigidbody damping and motion](https://docs.unity3d.com/6000.0/ScriptReference/Rigidbody.html). Numerical tuning and collision details are implementation choices within the selected behavior.

Questions: none blocking this bounded bottle revision; `96` retains larger/special-object comparisons.

Result: [integrated behavior, compatible saves, validation and Windows build](../completed/110-bottle-physics-and-recognition.md). User readability/settling verdicts remain with `101`; final art/style with `09`/`105`.
