# Scope and validation (Something Down There)

## Scope

1. Start with one compact, playable loop.
1. Primary identity: first-person excavation and absurd digging progression.
1. Favor discoverable physical progression before gated systems.
1. Keep tooling honest: only documented dependencies, no hidden systems.
1. Asset policy: use only commercially free/attributed existing assets, AI-generated assets with documented provenance, or assets created through Blender MCP/Blender workflow.
1. Sound policy: all SFX/music/audio assets must have explicit source and license/rights records before integration.
1. No coding of gameplay systems in this repo initialization pass; only project scaffolding and planning docs.

## Validation contract

1. Define each feature in docs first.
1. Implement one feature at a time.
1. Verify with direct checks and runtime checks per task.
1. For this repo setup pass, validation means required folders/files exist and docs are consistent.
1. Update docs with limitations and next tasks.
1. Every change to mechanics, controls, scene flow, or dependencies must be reflected in relevant docs before the task is considered done.
1. If docs and structure are consistent, task is eligible for handoff.

## Navigation contract (how to keep aligned)

1. Start task work from:  
   - `docs/development/tasks.md` (what to do)  
   - `docs/development/status.md` (current milestone + blockers)  
   - `docs/scope-and-validation.md` (constraints)  
   - `docs/architecture.md` (system ownership)

2. For implementation:
   - `unity/` is source/runtime.
   - `docs/` is behavior/spec state.

## Asset and audio provenance (required)

1. Any non-primitive asset or sound file imported into `unity/` must be recorded before it is used in gameplay.
2. Create one ledger row per addition/update in:
   - `docs/asset-ledger.md` (new)
3. Required record fields:
   - Asset path
   - Asset type (Model/Texture/Material/UI/SFX/Music/etc.)
   - Source (`Purchased`, `Free asset pack`, `AI generated`, `Blender workflow`, `Other`)
   - Source link or file provenance
   - License/commercial terms
   - Attribution text required (if any)
   - Approval date
   - Feature/task link
   - Last review date

4. For AI-generated assets/sounds, include:
   - Generator/model used
   - Prompt intent and settings
   - Resulting file set
   - Reviewer notes before approval

5. A task is not considered done while required provenance fields are missing.

3. After any non-trivial change, update at least:
   - the relevant design/spec note,
   - one status bullet,
   - and the task log row.

## Version policy (Unity + libraries)

1. Default to compatibility-first updates, not the absolute newest.
1. Update packages only after checking:
   - package changelog/API compatibility,
   - Unity version compatibility,
   - platform/platform-requirement impact.
1. Record version decisions and rationale in task notes or status.

## Project mode (current pass)

1. This pass is **documentation + project foundation** only.
2. No gameplay implementation tasks are considered started until a task in `docs/development/tasks.md` is marked `ready -> in_progress`.

## Test cadence policy

1. Run lightweight checks frequently (compile/build status, lint-like quick checks, direct script or scene sanity checks).
2. Run heavier integration/play validation once per completed task, unless:
   - behavior changed,
   - dependencies changed,
   - a previous test failed.
3. If a task is stable and unchanged, avoid repeated full runs to save time and resources.
