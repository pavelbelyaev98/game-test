# Task 39 - Terrain materials and upgrade-sensitive resistance

Type: implementation. Status: `planned`. Prerequisites: 15, 58, 63.

Feature: [terrain materials](../../features/backlog/terrain-materials.md). [Queue](../tasks.md). [Research basis](../../research/player-review-findings.md).

## Task contract

- Add an agreed finite set of materials with local formations and overlapping depth tendencies. Their approved appearance distinguishes diggable resistance from permanent boundaries, never reveals treasure or forces a particular route.
- Hard formations may be inefficient with weak equipment; later purchased strength must make the same formation clearly easy. No chapter/depth permission flag, automatic resistance scaling with owned equipment or bomb-only ordinary progression.
- Preserve downward/sideways/diagonal cuts, supported structures and `26`/`34` cleanup. One accepted stroke owns energy, changed density, matching collision, discovery exposure and removal accounting. Define how partial work becomes an accepted paid action before implementing it.
- Material identity and any partial excavation state survive `35`; updating an existing save cannot regenerate its hole. Keep material lookup independent from discovery metadata.

## Before implementation

- Research: measure current stroke/chunk costs; compare candidate resistance models on the same geometry and several strength levels. Research current official Unity mesh/collider guidance where needed; integrate the selected model into MainGame.
- Implement the material vocabulary, site scale and early-resistance policy accepted in [58](58-site-and-terrain-design.md), within the [63 budgets](63-windows-targets-and-budgets.md). Obtain specific material/asset approval before import. Return to the user only for an evidence-backed change to that design, not to re-decide it during coding.
- Define a representative obstacle and the expected qualitative before/after improvement. Exact resistance numbers follow actual cuts and traversal, not paper-only balancing.

## Acceptance

- Buy a major strength upgrade, revisit the same hard formation, and demonstrate substantially easier access both down and sideways. Speed alone changes cadence, not material removal per stroke.
- Test material interfaces, thin remnants, permanent boundaries, stale hits, save/reload and real movement through heavily excavated mixed terrain; render and collision agree.
- Measure accepted-edit cost at normal and late tool tiers; inspect material recognition/tool feedback and deliver the Windows build. Final whole-run balance belongs to `37`.
