# Task 63 - Define Windows support and performance targets

Type: design/research; documentation only. Status: `planned`. Prerequisites: `35` and existing native-build evidence.

Feature: [release validation](../../features/backlog/release-validation.md). Consumers: `58`, later implementation and `54`. [Queue](../tasks.md).

## Research and proposal

- Inspect the actual Unity/Windows configuration and measured early/late edit, collision, save/load and launch behaviour. Read the [physical-comfort and compatibility findings](../../research/player-review-findings.md#physical-comfort) as test prompts, not proof of defects here.
- Propose supported Windows versions, representative hardware/resolutions/window modes and explicit frame-time, accepted-edit spike, load/save latency and memory targets. Explain the cost/quality tradeoffs and use real evidence where available; label unmeasured estimates.
- Specify the native test matrix and release acceptance for long excavations, resume/focus, persistent progress and audio/input comfort. Keep click-and-hold and keyboard/mouse support; controller/rebinding and auto-benchmarking remain uncommitted unless explicitly selected.
- Define what platform/app access is needed later for Steam testing without installing SDKs or changing remote configuration. Coordinate with `55` and leave actual platform integration to `53`.

## Questions to resolve with the user

Review a concrete support/budget proposal and the hardware they want to support, with a recommended baseline. Record access limitations separately from design choices; do not promise unsupported targets or add platform work silently.

## Done when

- Record accepted support targets, budgets and review cases in the release feature. Performance-sensitive tasks and the site plan reference those targets before expanding content/terrain cost.
- `54` can qualify the final game against agreed criteria, rather than deciding what acceptable performance means at the end. This does not claim certification, optimization or release readiness.
