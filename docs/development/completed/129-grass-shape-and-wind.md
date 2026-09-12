# Task 129 - Taller grass and natural wind

The user wanted taller, prettier grass and more natural movement, referencing A Game About Digging a Hole.

- Refined the existing approved Blender clump/atlas in place: 26–42 cm authored blades, wider curved silhouettes, softer folds and restrained green variation. Heights average 2.15 times the previous source; twelve blades / 168 triangles are retained. Root centres remain within floating-point tolerance; all 20,451 seeded clump positions are unchanged.
- Travelling gusts and per-blade tip response replace the shared small sway. Roots stay fixed, normals follow the bend, and culling bounds include the taller moving tips. Full visible density and excavation support remain.
- MainGame scene and grass integration tests pass, including imported motion data, tall-tip culling, cuts/reset/restore/re-enable. CLI review includes matched views, a six-second video at game speed and excavation clearing 33 clumps. grass validation record; [detailed evidence](../../../unity/Logs/Task129/).
- A 180-frame local Editor sample after three cuts submits 132 batches / 3.15 million grass triangles: 7.95 ms median / 8.74 ms p95 frame time, 0.17 ms median CPU submission on RX 9060 XT / Ryzen 9700X. These are observations, not a before/after speed claim or minimum-hardware qualification (`54`).
- Windows build **2026-09-12 15:37 UTC** succeeds with zero errors and one existing Pipeline warning; seven-second native headless startup is clean. User review determines the preferred visual feel.
- [Task contract](../tasks/129-grass-shape-and-wind.md), [source and rollback](../../../art/ground-grass/README.md). Next: 126 shallow-find density; sun, ground, saves and gentler shovels are preserved.
