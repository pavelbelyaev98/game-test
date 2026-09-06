# State ownership and saving

Status: v4 contract, not implemented. [Core mechanics](../core-loop-and-mechanics.md) define player behavior; [architecture](../../ARCHITECTURE.md) defines components. Implement transfer rules in M1, expand for upgrades in M2, and add disk persistence in M3.

## One authoritative model

Use integer pepper-equivalent units for gameplay. At every committed state:

**initial harvest = remaining piles + raw carrier + queued input + active batch + finished station output + carried finished food + stored food.**

Every term is nonnegative. Carriers and buffers respect their current capacities. One transfer subtracts and adds the same accepted amount once. Cosmetic cascades, jar meshes, progress counters, and household displays do not introduce another copy.

| State group | Required data |
| --- | --- |
| Authored world | Stable content/version identifier, unique pile-pocket IDs, initial quantities, discovery references, capacity/rate configuration. Static configuration is not duplicated mutable inventory. |
| Remaining supply | Units and local depletion state per authored pile pocket; enough detail to reconstruct where the player scooped. |
| Raw carrier | Current tool, units, and held/valid-resting location. Older replaced tools cannot hold a second active inventory. |
| Station | Installed tier, queued amount, active amount and remaining duration, finished amount, and a pending upgrade if activation waits for a cycle boundary. |
| Finished carrier | Empty/docked, held, or safely parked, with its exact units. One reusable carrier. |
| Progress | Stored-food total and activated equipment IDs. Cleared paths derive from saved pocket state. |
| Session | Whether the day has ended; valid player position/look and pause/resume behavior. |
| Settings | Input bindings, sensitivity/FOV, invert Y, audio/display/comfort options as they are implemented; stored separately from New Game progress. |

Derive output reservation from active work: finished output plus the reserved active amount must fit output capacity. A new batch starts only if its result has a destination. Picking up available output does not cancel the active reservation.

## Transactions and reconstruction

Gather, tip, output pickup, deposit, and equipment activation validate the current model, commit once, then drive presentation. A cancelled visual action restores a view of the committed contents. Do not save a second in-flight pepper inventory inside particles or transforms.

Partial amounts are valid, including the final batch. Deposit empties the finished carrier, increments stored food once, and returns its empty representation to the dock. Stored food cannot be withdrawn. A held/parked output carrier can coexist with output accumulating at the station.

The **Finished Food Handoff Rack** is the single permanent deposit command target and the end of player food handling. Cellar and family boxes read `stored food`; they own no units, transfers, secondary inventory, allocation, or distribution jobs. Their apparent filling never subtracts from stored progress or leaves retrievable jars at the rack.

Upgrades preserve contents and only increase capability. A station upgrade does not grant a wheelbarrow. A smaller later discovery cannot reduce an installed tier. Cycle-boundary activation must survive pause/save without losing its pending request.

For the final tier in M4, the nearby fixed intake is authored station configuration. Installed tier selects its active dump target and visible feed layout; the existing input buffer remains the only queued-food owner. Switch the active intake with the committed installation, keep the output dock/handoff rack fixed, and reconstruct the correct target from the saved tier. Saving before installation must not activate the chute early, and loading afterward must not recreate a second queue or downgrade the route.

Recover invalid carrier placement at a safe authored resting point with the same contents. Validate player coordinates on load and fall back to a safe spawn if needed. No required pepper is recovered by spawning extra harvest.

The four household food displays derive from stored progress; the meal derives from the ended flag. They require no independent parcel contents, task flags, visitor state, or reward counters. A skipped visual milestone restores directly to the current display.

## Automatic completion contract for M4 onward

The full game completes when all authored pile units are cleared and all initial harvest units are stored, with raw, queued, active, finished-output, and carried-finished amounts zero. After the final valid deposit, set the existing ended flag in the same committed model update. Then show deposit feedback and start the meal transition. There is no Ready-to-finish state, Finish command, table target, equipment prerequisite, or household checklist.

Persist completion before relying on a visual callback. Pause freezes the transition; loading a completed snapshot restores the finished scene directly instead of replaying a gift or waiting for a button. A pre-completion snapshot remains unfinished until its remaining food is deposited. Reject inconsistent ended snapshots under normal snapshot validation. A repeated/empty deposit or reconstruction cannot re-trigger completion rewards. Gift and meal props are presentation, not new state inventories.

M1–M2 only verify that their small section can be fully stored, with plain completion feedback. They do not implement the meal, household display states, or disk saves. M3 supplies the snapshot machinery; 4_03 applies this automatic ending contract and 5_04 supplies its final presentation.

## Disk contract for M3

Use one local save slot under the application's persistent-data directory, a schema version, and a content version. Runtime state is ordinary serializable data. Do not save live Unity object references or use mutable ScriptableObjects as progress.

Capture a coherent model after a committed action, with a coalesced autosave after meaningful progress and a flush for explicit save/exit. Avoid disk writes per decorative pepper. Pause can request a save, but gameplay remains usable if writing fails. Start a resumed game paused when necessary to prevent unintended progress during reconstruction.

Write to a temporary sibling file, validate the serialized snapshot, then replace the current save while retaining one previous valid backup where supported. If a write fails, preserve the last valid save and show a retryable notice; do not claim success. Never intentionally destroy the only valid save to recover a bad write.

On load, validate schema/content versions, stable IDs, finite numeric values, bounds, capacities, and conservation. Corrupt or unsupported data must not silently start a fresh game. Offer the valid backup or an explicit new-game choice with a clear explanation; preserve the unreadable file for diagnosis. Do not invent migrations for older prototype schemas. Any later save format change needs an explicit compatibility decision and fixtures.

## Acceptance checks

Round-trip raw carrying, partial depletion, an active batch, accumulated output, a carried finished load, a pending upgrade, stored food, and the ending. All conserved quantities and capabilities remain equal; views reconstruct correctly.

Verify interrupted/corrupt writes, unsupported versions, unknown/duplicate IDs, out-of-bounds quantities, and a missing content definition. Invalid data is rejected without overwriting a valid save. Reloading a deposit or ending cannot repeat its reward.

These checks are mandatory when persistence is built. This document does not claim an existing save implementation.
