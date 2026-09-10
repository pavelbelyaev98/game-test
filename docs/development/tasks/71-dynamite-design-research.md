# Task 71 - Evaluate optional C4 and design forgiving placement

Type: design/research; documentation only. Status: `planned`. Prerequisites: `15`, `39`, `47`, `49`.

Feature: [optional systems](../../features/backlog/optional-systems.md). [Research](../../research/player-review-findings.md#scope-boundaries-and-conditional-investigation). [Queue](../tasks.md).

## Why and scope

The user selected **C4** as the name for the existing placed, remotely detonated explosive proposal previously called dynamite. Keep this task's stable ID/path; the naming decision adds no different mechanic, asset or bomb-only material. Inclusion remains optional. The reports sharpen its interaction: inaccurate bouncing or stance-dependent placement undermines an otherwise satisfying blast. Research whether it adds value alongside strong late shovels before selecting the mechanic.

## Research and proposal

- Inspect real terrain, boundaries, dig costs, shared cleanup, saving and remembered-obstacle routes. Compare the benefit of an occasional paid large blast with existing tool milestones; no bomb-only normal material, mandatory ending blast or consumable stockpile.
- Resolve the user's direct question: **why use this when the shovel is already strong?** Produce an early/late usefulness matrix against the strongest normal shovel: setup/action time, charge price, shared-energy implications, useful removed shape/volume, control and remaining work. Compare a broad planned chamber/route widening and coordinated placed charges; these are candidates, not selected abilities. A larger visual explosion alone does not prove utility.
- Recommend omit/defer if no actual use case survives that comparison. Do not weaken the shovel, invent bomb-only barriers, require bombs for chest rewards or add a combat/puzzle loop to justify an otherwise redundant item. If selected, make the delivery task include a player feel check that the player voluntarily chooses a charge for its advantage.
- If worthwhile, propose forgiving surface-snapping from a valid nearby aim, including walls/ceilings and placement while moving or airborne. No throw/bounce, stationary requirement, perfect-angle requirement or tiny interaction hotspot.
- Specify readable valid/invalid preview, range/occlusion, charge spacing, anchoring after nearby digging, removal/cancellation, capacity/cost, remote detonation and persisted state. A preview must use an approved asset/representation, not a substitute primitive.
- If included, hand the selected selection/use bindings and availability/charge states to the [118 action bar](118-bottom-action-bar.md). Its eventual implementation adds the real C4 entry and clearly distinguishes selecting, placing, cancelling and detonating; `118`'s backpack delivery does not implement C4 or reserve a misleading empty slot.
- Research current official Unity collision/query guidance and bounded blast/remeshing cost under `63`. Define substantial predictable blast scenarios with synchronized `26`/`34` cleanup and protected discoveries.
- Present actual content/audio briefs and integration/rollback boundaries without producing assets. No player-health or new hazard system follows from explosive presentation.

## Questions to resolve with the user

- Does the proposed blast add enough beyond the upgraded shovel to include C4 at all? Recommend include, defer or omit using concrete obstacle examples.
- If selected, choose placement/detonation inputs, carry/purchase rules, cancellation/reclaim behaviour and the intended blast scale.
- Confirm whether upgrades are worthwhile and how charges behave if their support is excavated. Review the precise preview/feedback brief separately from later asset approval.

## Done when

Record the decision, rationale, rejected alternatives and any selected interaction/economy/save contract in the optional feature. Only selection creates a numbered implementation task and specific asset requests; schedule delivery before any dependent passive reward, achievement or full-run validation. Update affected contracts; local flags or a speculative brief do not count as implemented C4.

## Blast readability proposal

- Compare a placement ghost plus approximate affected-volume preview against placement-only feedback. Explain why actual removal may differ at boundaries or resistant material; define deterministic charge/material examples instead of a misleading guaranteed sphere.
- Demonstrate a visibly substantial benefit for the proposed price on ordinary and hard terrain, protection of discoveries and no charge spent on invalid placement. Include saved anchoring, remote detonation and large-edit capture costs from `80`; exact preview assets and inclusion remain review-gated.
