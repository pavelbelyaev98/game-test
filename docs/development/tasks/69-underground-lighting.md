# Task 69 - Integrate passive underground lighting

Type: implementation. Status: `planned`. Prerequisites: `08`, `68`.

Feature: [underground lighting](../../features/backlog/underground-lighting.md). [Research](../../research/player-review-findings.md#physical-comfort). [Queue](../tasks.md).

## Task contract

- Integrate the accepted `68` light in MainGame using the approved production presentation. It works automatically underground without tool switching, lamp-placement chores or battery consumption, including after depletion/rescue/reload.
- Implement the selected surface transition, beam/shadow and glare rules while preserving `65` FOV/low-motion settings and `67` stance if present. Do not add aiming bob or mandatory camera effects.
- Use actual approved terrain/find materials; light must preserve shape, exposure readability and material/boundary distinction without revealing hidden finds through soil.
- This task delivers the complete usable baseline. If `68` selected later light improvements, its decision must name the paid-track owner (for example `46`) or a separate numbered delivery task; keep those dependencies explicit before `37`/`54`. Do not add a new shop category or claim those later purchases implemented here.
- Any housing/model/material/audio addition needs a specific preview/source/license/files/removal approval and ledger entry. Runtime lighting is not permission to create substitute visible art; respect the user's terrain ownership.

## Before implementation

Read the chosen `68` contract, `57` presentation brief and `63` budgets. `68` currently provides a concrete proposal awaiting review; do not treat its recommendation as selected. Its CLI evidence identifies unoccluded sun/ambient, disabled main/additional shadows and an existing shadow-capable ground shader. Address this full lighting interaction while preserving `19`'s clean rim and the user's terrain appearance. Inspect live Unity using the official CLI and relevant rendering skills; ask only about a concrete contract change or actual asset batch after selection.

## Acceptance

- Inspect fully enclosed sideways tunnels, deep shafts, broad chambers and near-wall views at supported FOVs/aspects against the feature's selected visibility distances. Use final approved finds/materials and verify relief, recognizable exposed finds, boundaries and footing; require light through zero-fuel automatic rescue without changing rescue timing.
- Compare surface/underground transitions and camera/stance movement for flicker, glare, shadow artifacts and through-wall reveal; test normal play/save reload rather than only a staged preview.
- Measure matched light-on/off render cost, including required sun/ambient/occlusion changes, against the feature's lighting allocation and `63`'s whole-frame budgets; record hardware, resolution and mean/p95/p99/max. Verify the baseline remains sufficient without future purchases. Later selected paid improvements must independently validate their benefit/saved ownership in their assigned task.
- Deliver the Windows build with presentation evidence. `39` rechecks mixed materials and `54` covers long-session visibility/performance.
