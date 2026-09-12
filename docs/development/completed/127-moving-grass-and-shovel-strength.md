# Task 127 - Moving surface grass and gentler shovels

The user requested short moving 3D grass above Sunny r8 turf and 40% weaker digging at every shovel level, with unchanged reach.

- Integrated the explicitly approved original Blender clump/atlas with anchored gentle wind, camera/distance culling and stable distance thinning. Excavation clears unsupported roots; reset/restore/re-enable derive the same patches without save data.
- Reduced all six default/serialized cutter radii by cube root of 0.6; measured fresh-soil removal retains 58.7–59.6%. Reach, cadence, energy, prices and saved levels remain intact.
- 21 relevant tests pass. CLI-inspected lawn, excavation/find visibility and fixed-camera wind frames: grass implementation record. [Detailed evidence](../../../unity/Logs/Task127/).
- Windows build: **2026-09-12 14:55 UTC**, zero errors, one existing Pipeline runtime-config warning; seven-second native headless startup has no errors/exceptions.
- The reviewed views submit 75–128 batches / 0.87–1.24 million grass triangles, with 0.16–0.27 ms CPU submission in this Editor session. Minimum-hardware/GPU qualification stays 54; user feel/pacing reviews remain separate.
- [Task contract](../tasks/127-moving-grass-and-shovel-strength.md), [asset ownership/removal](../../asset-ledger.md). Next: 126 shallow-find density using the gentler shovel baseline.
