# Starter bottle models

**Replaceable trial art, not final style approval.** The current batch contains three bottles only, enlarged 25% in [Task 110](../../docs/development/tasks/110-bottle-physics-and-recognition.md). The user explicitly removed cans/bricks. [Current sizes/counts/gameplay](../../docs/features/backlog/starter-find-batch.md); [replacement and reference guide](../../docs/development/replacing-find-models.md).

- `StarterFinds.blend`: editable bottle models, original material graphs, preview staging and native Blender text/curve label artwork. The original live Blender scene/filepath is preserved.
- `create_starter_finds.py`: bottle-only Blender authoring/bake/export recipe, executed through Blender MCP. It owns the `SDT_Starter` namespace and reads sizes/selection from the catalog.
- `refine_starter_finds.py`: detailed glass wear and torn Bulgarian labels. `label-art/` contains Lemonade, Beer and Syrup masks rendered from Blender's built-in outlines; no external fonts or real brands.
- `exports/`: three FBX, explicitly triangulated, metre dimensions, base-centre origin and +Y export up. Geometry has unit scale; the modest enlargement is baked into vertices.
- `collisions/`: three separate convex FBX (200/198/190 triangles), authored from the bottle silhouettes through `create_bottle_colliders.py`. `collision-manifest.json` and `collision-validation.json` record dimensions, manifold geometry and FBX round trips; hulls are retained in the Blender source and hidden from renders.
- `textures/`: three 2048px shared Bottles maps. BaseColor is sRGB; Normal/Masks are linear. Masks use R metallic, G AO, B roughness; Unity smoothness is 1-B.
- `previews/overview.png` and `previews/bottles.png`: current bottle-only views. Preview floor/lights/cameras are excluded from game exports.
- `catalog.json`: editable source of truth for IDs, current model/map references, dimensions, prices/counts, `throw_speed` (8 m/s starting bottle tuning) and old save-ID aliases. `manifest.json` and `validation*.json` are generated reports. `115` adds optional RMB lift/drop and fresh Dig-to-throw, while auto-collection waits for 0.6 seconds of eligible direct observation.
- `validate_starter_finds.py`: reopens source and round-trips all FBX in temporary Blender scenes without changing user work.

Three exported models total **3,956 triangles** with one shared atlas material. Source reopen and all three FBX reimports pass dimensions/unit-scale/UV/triangle checks; no missing images or non-manifold source edges. Runtime convex colliders, support release and collection are checked separately in `110`.

Writing remains ЛИМОНАД / ОСВЕЖАВАЩА НАПИТКА / 0,5 Л; ПИВО / СВЕТЛО / 0,33 Л; СИРОП / МАЛИНИ / 0,5 Л. It is weathered fictional packaging, not required reading or historical certification. Can/brick model, texture, label and preview files and owned Blender datablocks are removed; only save aliases and historical records retain their old names.

## Reproduce or replace

For the original bottle recipe, run `create_starter_finds.create()` only in a fresh owned namespace, then `refine_starter_finds.prepare()`. For an existing source use `prepare()` only to revise surfaces. Bake BaseColor/Normal/Masks through that module's `m.bake('Bottles', channel)`, then `m.export_all()`, bottle/overview `m.preview()` and `m.save_source()`. Run `validate_starter_finds.run()` and `create_bottle_colliders.validate()` afterward. Export also regenerates retained collision hulls. Do not run the original modelling recipe over user-supplied replacement models.

`unity/Assets/Content/StarterFinds/` contains three visual model copies plus three collision FBX, three maps, one visible material, six centred meshes and three prefabs, a contact physics material and runtime catalog. **Sync Starter Find Models** reads the catalog, preserves retained GUIDs, regenerates convex collision/exterior samples, configures bottle physics and 60% pickup, and removes retired generated outputs from its five owned subfolders. Source artwork and saved game files are never pruned by sync.

Model swaps retain content IDs and saved identities; old v1/v2 saves remain readable and v3 records release state/current pose. Update sizes, owning specs and source/ownership evidence when replacing art. Exact steps and rollback boundaries live in the [replacement guide](../../docs/development/replacing-find-models.md) and [asset ledger](../../docs/asset-ledger.md).

Original commercial-use license: `LICENSE.txt`, no attribution required. [JulioVII's reference](https://juliovii.itch.io/ftpgrass-dirt) informs broad style only; its artwork is not included. Final coordinated style and player recognition remain subject to `105`/`09` review.
