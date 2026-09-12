# Task 128 - Visible sun and consistent grass density

The user requested a sun and reported the grass visibly thickening on approach.

- Integrated the explicitly approved Blender cream/yellow disc and restrained halo in MainGame's camera skybox, aligned with the existing sunlight. Sources/license/removal are in the [ledger](../../asset-ledger.md#task-128---approved-visible-sun).
- Removed the abrupt half-clump/six-blade switch and distance cutoff. Full twelve-blade density now covers the visible site; spatial culling, rooted wind and excavation support remain. All 20,451 roots retain the previous placement hash.
- Grass integration and MainGame scene tests pass. CLI review confirms constant density, soil occlusion and a stationary sun when translating the camera. Sky pixels away from the disc exactly match the old solid background. sun and grass validation record; [detailed results](../../../unity/Logs/Task128/).
- The widest reviewed lawn view submits 128 batches / 3.06 million grass triangles, up from 1.24 million with thinning. A 180-frame Editor sample on RX 9060 XT / Ryzen 9700X recorded 8.56 ms median frame time, 12.31 ms p95 and 0.16 ms median CPU grass submission. This is local observation, not native/minimum-hardware qualification (`54`).
- Windows build **2026-09-12 15:17 UTC** succeeds with zero errors and one existing Pipeline warning; seven-second native headless startup is clean. Saves, ground/daylight and 127's gentler shovels remain intact.
- [Task contract](../tasks/128-visible-sun-and-grass-density.md). Next: 126 shallow-find density; user review uses the [Windows build](../../../builds/windows/SomethingDownThere.exe).
