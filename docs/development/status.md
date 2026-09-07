# Current status

## Milestone

The FPS foundation and URP baseline are operational. Task `01`'s first-playable plan is complete; its terrain, discoveries, economy, upgrade and rescue slices remain unimplemented.

## Active and next work

- No task is currently active. Task `03a` is ready: [finite excavation and first-playable scene shell](../features/backlog/03-excavation-and-terrain.md#first-playable-task-03a).
- The [core-loop plan](../features/backlog/01-core-loop-contract.md) maps scene ownership and orders nine bounded slices ending in a physical return/sell/recharge/upgrade trip plus overextension/rescue proof.

## Latest evidence

- Task `01`: documentation links/anchors, unique queue IDs, slice order/ownership/acceptance, existing runtime references and working-document lengths checked; `git diff --check` passed.
- Runtime baseline from `M01`: Unity `6000.6.0f1` / URP `17.6.0`, 21/21 scene mesh renderers using URP Lit, 7/7 EditMode and 14/14 PlayMode checks passed. Reproduce with `tools/test-fps.ps1`; logs stay in ignored `unity/Logs/`.
- No runtime or asset changes in task `01`; Unity tests were not rerun for this documentation-only task.

## Limitations and blockers

- The planned proof uses primitive presentation, two authored finds and session-only state; disk saving, randomized placement and long-run progression remain full-feature work.
- Actual editor review of FPS visuals/feel remains outstanding; the future full-loop proof also requires observed signal tension and upgrade impact.
- Unity MCP remains unavailable in this session (no Unity tools exposed; prior setup check found no configured server). This did not block planning, but live scene/editor validation needs the connection resolved under `AGENTS.md`.
