# Task 71 - Evaluate optional dynamite and design forgiving placement

Type: design/research; documentation only. Status: `planned`. Prerequisites: `15`, `39`, `47`, `49`.

Feature: [optional systems](../../features/backlog/optional-systems.md). [Research](../../research/player-review-findings.md#scope-boundaries-and-conditional-investigation). [Queue](../tasks.md).

## Why and scope

Placed, remotely detonated dynamite is an existing optional concept. The reports sharpen its interaction: inaccurate bouncing or stance-dependent placement undermines an otherwise satisfying blast. Research whether it adds value alongside strong late shovels before selecting the mechanic.

## Research and proposal

- Inspect real terrain, boundaries, dig costs, shared cleanup, saving and remembered-obstacle routes. Compare the benefit of an occasional paid large blast with existing tool milestones; no bomb-only normal material, mandatory ending blast or consumable stockpile.
- If worthwhile, propose forgiving surface-snapping from a valid nearby aim, including walls/ceilings and placement while moving or airborne. No throw/bounce, stationary requirement, perfect-angle requirement or tiny interaction hotspot.
- Specify readable valid/invalid preview, range/occlusion, charge spacing, anchoring after nearby digging, removal/cancellation, capacity/cost, remote detonation and persisted state. A preview must use an approved asset/representation, not a substitute primitive.
- Research current official Unity collision/query guidance and bounded blast/remeshing cost under `63`. Define substantial predictable blast scenarios with synchronized `26`/`34` cleanup and protected discoveries.
- Present actual content/audio briefs and integration/rollback boundaries without producing assets. No player-health or new hazard system follows from explosive presentation.

## Questions to resolve with the user

- Does the proposed blast add enough beyond the upgraded shovel to include dynamite at all? Recommend include, defer or omit using concrete obstacle examples.
- If selected, choose placement/detonation inputs, carry/purchase rules, cancellation/reclaim behaviour and the intended blast scale.
- Confirm whether upgrades are worthwhile and how charges behave if their support is excavated. Review the precise preview/feedback brief separately from later asset approval.

## Done when

Record the decision, rationale, rejected alternatives and any selected interaction/economy/save contract in the optional feature. Only selection creates a numbered implementation task and specific asset requests; schedule delivery before any dependent passive reward, achievement or full-run validation. Update affected contracts; local flags or a speculative brief do not count as implemented dynamite.
