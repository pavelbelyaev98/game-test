# Task 35 - Persist excavation and progression across quit/reload

Type: implementation. Status: `ready`. Prerequisites: 34 (complete).

Feature: [core loop](../../features/backlog/core-loop.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Gap: terrain, collected identities, inventory, wallet and owned shovel levels currently live only in memory. The locked design rule that excavation never resets still needs save/load support; this is also the foundation for [permanent discoveries (`36`)](../../features/backlog/buried-upgrades.md).
- Scope after `34`: persist the authoritative excavation density and deterministic generation state, discovery identities/collected state, carried records, wallet, owned upgrades, battery and player return position as one consistent save. Rebuild derived meshes/colliders and exposure from restored state before play resumes; developer overrides are not progression.
- Use versioned saves and atomic replacement with last-valid recovery. Failed/interrupted writes or incompatible data must not silently overwrite progress or start a fresh excavation; provide a readable recovery state. Save during play and on normal exit with a measured cost suitable for continuous digging.
- Acceptance: dig downward and sideways, collect/sell/upgrade, return/rescue, quit and relaunch the Windows build. Restore the same traversable cuts, cleared remnants, absent collected finds and exact economy/upgrade state without duplication or a fresh seed. Check interrupted writes, recovery and repeated load; new reward state in `36` must use this same save boundary.
- Excavation never resets within a save through ordinary gameplay, rescue, reload or the ending. A separate new game and the existing explicitly confirmed development-only reset do not redefine that rule; preserve the release gate in `22`.
- Before implementation: read [persistent-investment findings](../../research/player-review-findings.md#persistent-investment), inventory every authoritative runtime owner and research a versioned snapshot/recovery strategy against the actual density size. Use official Unity/.NET guidance if needed; measure saving during continuous digging and safe rebuild-before-control on load.
- User question: none required for the existing scoped save/recovery contract. Choose conservative technical defaults; propose any destructive save conversion or new player-facing save-management policy before adopting it. Future content/upgrades/photos/ending/achievements extend this boundary in their own tasks, without a forced fresh save.
