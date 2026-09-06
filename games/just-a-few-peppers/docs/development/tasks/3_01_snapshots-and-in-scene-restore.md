# 3_01 — Snapshots and in-scene restore

Milestone: M3 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Reconstruct the current playable loop from one coherent logical snapshot.

**Depends on:** [2_03 — Core feel playtest gate](2_03_core-feel-playtest-gate.md). All earlier play gates must also be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), then: [State and saving](../state-and-saving.md) · [Architecture](../../../ARCHITECTURE.md) · [Core mechanics](../../core-loop-and-mechanics.md). Inspect the actual code, scenes, packages, and predecessor's delivery record; the brief does not prove that implementation exists.

## Work

- Represent stable content/source/prop/offer IDs, exact food and live pepper IDs/owner membership (including held-single and spills), free carrier/prop poses, mechanism and output-grouping phase/selection, Coin balance, paid/installed improvements and pending installation in versioned data. Most yard access is initial configuration, not a discovery-unlock state.
- Capture after committed actions and restore the model before rebuilding views. Provide a small development-only capture/restore action in the playable scene.
- Include the [attachment lifecycle](../state-and-saving.md#attachment-ownership-and-installation): paid kit identity/pose/holder/recovery, accepted mounting while pending, and installed effect. Rebuild one kit or installed view from ownership; never issue another purchasable or functional copy.
- Validate IDs, finite values, bounds, capacities, conservation, and content/schema versions; recover invalid transforms to a clear last safe pose or authored fallback without adding food. Follow the [pose reconstruction contract](../state-and-saving.md#disk-contract-for-m3): preserve stable arrangements/rotated stacks, restore held objects under one holder, and safely settle objects saved in motion before enabling simulation.

## Acceptance

- Round-trip proportional handoff earnings, both purchase orders and a paid pending installation. Restore mechanism controls and combined capabilities without replaying income, payment or food transactions; malformed/duplicate offer state is rejected.
- Round-trip unowned, paid loose, held/dropped/recovered, fitted-awaiting-boundary and installed kit states with active/queued food and accumulated output in both purchase orders. Recovery, repeated mounting and restore preserve the single entitlement/effect and next safe boundary; reject contradictory ownership/mount state and duplicate kit IDs.

- Round trips preserve partial piles, held-single/scattered/container-owned peppers, partial bulk pickup/pours, raw loads, interrupted grouping before/after its commit, active processing, accumulating output, carried finished loads, pending upgrades and stored progress. An uncommitted grouping selection remains within station output or resets safely; restore never recollects it. Reject duplicate food owners and stale absorbed IDs.
- Meaningful tests reject invalid snapshots; restoration does not replay deposits or make particles authoritative. This task claims in-memory restore only.
- Round-trip ground/worktop carrier placements, arranged/stacked props, and a falling loaded carrier. Valid arrangements survive; invalid poses recover without duplicate bodies, stack explosions, or lost food. Do not replace pose saving with a mat index or save every decorative pepper.

## Human playtest check

Capture during several load stages, change the scene state, restore, and check that quantities and usable equipment return correctly.

**Outside this task:** Disk writes, cloud, slot browser, or prototype-save compatibility.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Update the queue and milestone summary; append a dated delivery record here when work is performed. Keep scope decisions and unresolved blockers in the repository so the next chat can recover them.

Next in order: [3_02 — Local save and continue](3_02_local-save-and-continue.md). Stop after this task's handoff unless the developer explicitly requested a larger range.
