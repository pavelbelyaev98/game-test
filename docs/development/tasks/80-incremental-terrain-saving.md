# Task 80 - Incremental terrain capture and non-blocking saves

Type: implementation. Status: `blocked` (performance qualification deferred by the user). Prerequisites: `35`, `63`, `79` (complete).

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

## Implementation and validation

- Selected: immutable 4,096-sample pages with copy-on-write in the grid. Capture copies only the page table; edits, cleanup, reset and restore preserve snapshots already owned by the writer. This avoids both full-grid capture pauses and an accumulating delta/compaction log. The existing single writer and coalesced requests remain the scheduling boundary.
- The codec streams pages through a 16 KB buffer; versions 1/2, checksums and atomic recovery remain compatible. Capture, encoding, write/flush, replacement and durable-completion diagnostics are separate.
- The initial native late case passed capture/durability but missed whole-frame p99. Attribution showed repeated mesh/collider rebuilding dominated; the mesher now reuses density samples and retains unchanged chunks, including their normal halo. Geometry hashes match the previous output across 150 chunks. This technical optimization preserves excavation/collision rules and adds bounded per-chunk sample caches.
- Current evidence: 124/124 EditMode and 107/107 PlayMode after cache integration. The same 80-cut CLI comparison reduced capture mean/max from 4.545/17.393 ms to 0.0015/0.035 ms; these are diagnostic timings, not whole-frame qualification. The normal Windows player loaded a copied version-2 save, durably retained seven new strokes and restored identical terrain/player state on relaunch; quit took <1 s and original user save hashes match after restoration.
- Remaining: optimized whole-frame/capture-frame performance acceptance. The user explicitly stopped the lengthy repeat benchmark and requested handoff; do not restart it without a new request. The validation players exited safely. The first full run passed capture/durability but missed late-world frame p99; it predates the mesh optimizations and cannot qualify them. Keep this task incomplete.
- `SavePerformanceBuild`/`SavePerformanceFixture` retain isolated, repeatable validation. Frame intervals now use Stopwatch after Unity's DX12 frame statistics reported shorter frames than timed edits. Evidence: `unity/Logs/Task80/`, including `final-run-stopped.json`; allocation API cleanup also passes all 14 input-specific tests. No further benchmark is running.
- Instrumentation: [Unity ProfilerRecorder](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Unity.Profiling.ProfilerRecorder.html) measures frame allocations; [Windows process counters](https://learn.microsoft.com/en-us/windows/win32/api/psapi/ns-psapi-process_memory_counters_ex) measure private commit. Unsupported Mono allocation counters report unavailable, never a zero-allocation claim.
