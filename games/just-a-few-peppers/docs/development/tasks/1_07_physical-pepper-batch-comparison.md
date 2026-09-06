# 1_07 — Physical pepper batch comparison

Milestone: M1 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Deliver deliberate single-pepper and predictable bulk handling of a representative scattered physical batch, then choose measured simulation limits without losing food.

**Depends on:** [1_06 — Loose yard objects](1_06_loose-yard-objects-and-playful-handling.md). All earlier play gates must be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), dependency delivery/feedback records, and [core mechanics](../../core-loop-and-mechanics.md), [scope/validation](../../scope-and-validation.md), [state/saving](../state-and-saving.md), [architecture](../../../ARCHITECTURE.md), and relevant [verification](../testing-and-performance.md) / [asset rules](../unity-and-assets.md). Inspect actual source, scene/input wiring and pinned versions before implementation; the brief is not delivery evidence.

## Work

- Reuse the completed crate/handoff loop and shared handling. Add a representative scattered group of physical gameplay peppers in the same work area: select one intended pepper, pick it up, move/place it, put it into a container, deliberately gather several and pour them. Individual handling is optional; the harvest must remain convenient in bulk. Use the existing raw carrier as the minimal bulk destination; a separate held-group system is not required.
- Compare bounded physical batches with grouped resting/distant representation under equal quantities and timing during pickup, filling and pouring. Both candidates retain the nearby gameplay above; disappearing scenery plus cosmetic particles/counts cannot be the chosen replacement.
- Implement the [single/bulk/placement/pouring control contract](../../core-loop-and-mechanics.md#single-bulk-placement-and-pouring-controls): a visible single target, previewed bulk affected set and destination before commit, capacity-capped selection, occlusion and stable contents/container priority. Teach each action briefly at its target; no hidden modes/unexplained modifiers. Preserve hold-left-mouse bulk gathering/release-to-stop, quiet denial cues and hidden placement indicators; a bulk selection preview does not restore placement outlines. Document exact delivered bindings.
- Let physical peppers contact the carrier/intake and settle convincingly. Choose active-body limits, sleeping/reuse and grouped resting contents from measured behavior rather than a blanket no-physics rule. Use placeholders or suitable licensed assets; no full-harvest simulation requirement.
- Register live interactable pepper IDs, exact units, owner/membership and recovery poses under the state contract. Single/bulk pickup, taking targeted contents out of a container, partial acceptance and representation changes move the same units once. A gameplay spill/off-target pour creates recoverable loose ownership, never silent loss or a second copy. Provide forgiving recovery for inaccessible strays without requiring a tiny-item scavenger hunt.
- Verify exactly-one selection, capacity-limited previewed bulk pickup, occluded/overlapping contents, partial container transfers, pause/focus, interrupted pours, off-target spills, body recovery, repeated loads and partial final amounts in the actual scene/player. Record active/sleeping body counts, frame behavior, visual contact and limitations.
- Choose and integrate a technically viable default, retain useful comparison evidence and report pending human preference honestly. No fun rating is inferred from physics or screenshots.

## Acceptance

- Pick exactly one intended physical pepper, deliberately gather several without surprising selections, place them into a container and pour with convincing contact. The affected set is visible before bulk commitment and respects capacity/occlusion. Single pickup never grabs the container when its contents are targeted.
- The chosen representation preserves exact ownership through single/bulk pickup, filling, full/partial pours, spills, interrupted motion and recovery. Repeated contact/recovery cannot duplicate material; decorative proxies never own another copy.
- Packaged input checks and observed contacts cover repeated use, physical interruption and the final partial batch. No routine tunnelling, growing body leak or unrecoverable food remains.
- The decision states what is simulated, what owns food, measured limits, tradeoffs and untested feel. It retains required nearby physical gameplay without claiming the entire harvest needs active rigidbodies. Record individual handling, bulk handling and control clarity separately; screenshots cannot prove enjoyment.

## Human playtest check

Pick exactly one pepper from a scattered group and from exposed container contents. Preview and deliberately gather several, including near capacity and an obstacle; check that only intended peppers move. Fill/pour full and partial loads, miss the intake or drop a carrier, pause during motion, then recover and finish the same food. Compare individual handling, bulk handling and control clarity separately using the recorded representation alternative.

**Outside this task:** new machine operations (1_08), Coins, all-yard pepper rigidbodies, cooking judgment, disk saves or second products.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Own scene/prefab, input, UI, assets and build wiring; record exact scene/build, controls, short checklist, checks, limitations and actual feedback. Update queue/milestone records and stop after this handoff.

Next in order: [1_08 — Direct machine operation](1_08_direct-machine-operation.md).

## Planning record — September 6, 2026

Added for the supplied processing-and-inventions direction. Todo / Not tested; no new gameplay or measured comparison is delivered by this documentation pass.
