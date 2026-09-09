# Task 80 - Incremental terrain capture and non-blocking saves

Type: implementation. Status: `planned`. Prerequisites: `35`, `63`, `79`.

Feature: [core loop persistence](../../features/backlog/core-loop.md). [Queue](../tasks.md). [Research](../../research/meltopia-lessons.md).

## Existing baseline and gap

`35` already has a coalescing background writer, atomic recovery and 10-second dirty checkpoints. Its changed-grid capture copies 14.45 MB and measured 4-5 ms on the development PC; this is not late-game qualification. Preserve `35` as done. Target capture allocations and scaling, not a redundant new save system.

## Work

- Profile capture, serialization/compression, write and replacement separately during sustained digging and overlapping trades. Compare current and representative late/high-surface-area terrain against `63` budgets.
- Track dirty regions/revisions and use bounded immutable capture or copy-on-write where useful; no full-grid copy for each small edit or per-frame serialization. Keep Unity objects/API access on the main thread and move safe data processing/I/O off it.
- Select chunk snapshots, deltas with bounded compaction, or a measured hybrid. Preserve one coherent terrain/discovery/inventory/economy revision; edits made during a write remain dirty for the next commit. Bound queue memory and prevent autosave starvation under continuous edits.
- Retain stable discovery IDs, seeds, serializable world state and explicit edit boundaries independent of the player view. This keeps later co-op possible without networking packages, authority code or a multiplayer promise.
- Preserve old saves through explicit version handling/migration, checksums, previous-valid recovery and startup New Game replacement isolation. Normal quit waits for current state; failed writes retain the live world and retry path.

## Acceptance and questions

- Record p95/p99/max capture and whole-frame costs, allocations, peak memory and durability on declared hardware for fresh/late saves, small cuts and large edits. Meet `63`'s existing frame/capture limits, healthy loss window <=11 seconds and transaction durability <=1 second; do not invent a more lenient budget.
- Interrupt capture/write/replacement and overlap dig/collect/sell/buy/rescue. Recover a complete revision without lost committed transactions, duplicated finds, mixed state or autosave-induced multi-second freezes.
- Verify existing-save loading, failed storage/retry, bounded coalescing, New Game during pending work and Windows quit/relaunch. `54` repeats the finished-game hardware matrix.
- Technical approach is delegated; ask only if a destructive compatibility change or a changed player-visible recovery policy is unavoidable. Update architecture only if actual ownership changes during implementation.
