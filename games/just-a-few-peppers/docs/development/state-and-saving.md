# State ownership and saving

Status: tasks 1_02–1_03 implement finite piles, raw-carrier state, gathering/tipping transfers, queued and active processing, and accumulated output. Finished-carrier/deposit and saving/persistence remain unimplemented. [Core mechanics](../core-loop-and-mechanics.md) define player behavior; [architecture](../../ARCHITECTURE.md) defines components. Extend transfer rules through M1, expand for upgrades in M2, and add disk persistence in M3.

## One authoritative model

Use integer pepper-equivalent units for gameplay. At every committed state:

**initial harvest = remaining raw sources + raw carrier + registered loose/transit raw units (if used) + queued input + active batch + finished station output + carried finished food + stored food.**

Every term is nonnegative. Carriers and buffers respect their current capacities. One transfer subtracts and adds the same accepted amount once. Cosmetic cascades, jar meshes, progress counters, and household displays do not introduce another copy.

| State group | Required data |
| --- | --- |
| Authored world | Stable content/version and raw-source IDs, initial quantities, equipment offer IDs/effects/prices, earn rate, mechanism/capacity configuration. Configuration is not mutable food or budget. |
| Remaining supply | Units and local depletion per source, including heaps or staged containers. Optional authoritative loose/transit material needs stable IDs, exact units and recoverable ownership; otherwise physical pepper views remain within their existing batch owner. |
| Raw carrier | Stable identity, current tool, units, held/released state, world pose, and last safe recovery pose. Older replaced tools cannot hold a second active inventory. |
| Station | Installed improvements and derived capabilities, queued/active/finished units and duration, mechanism phase/stroke state, and paid pending installation at a safe boundary. |
| Finished carrier | Stable identity, empty/docked, held or released state, world pose, last safe recovery pose, and exact units. One reusable carrier. |
| Loose props | Stable authored IDs and chosen poses; held/released state where needed for reconstruction. Placement state has no food, collection, or task credit. |
| Purchased attachment | Stable offer/kit identity, paid-awaiting-installation or installed status, loose/held pose and recovery pose, and whether fitting to the known mount was accepted while waiting for a safe boundary. One kit/effect per offer. |
| Progress | Non-decreasing stored-food total, Coin balance, paid/installed improvement IDs and pending installation. Yard access does not derive from required clearing/discovery flags. |
| Session | Whether harvest completion has been committed; valid player position/look and pause/resume behavior. |
| Settings | Input bindings, sensitivity/FOV, invert Y, audio/display/comfort options as they are implemented; stored separately from New Game progress. |

Derive output reservation from active work: finished output plus the reserved active amount must fit output capacity. A new batch starts only if its result has a destination. Picking up available output does not cancel the active reservation.

## Transactions and reconstruction

Gather, release/recover registered material, tip, complete a machine operation, output pickup, deposit and purchase/install commands validate and commit once. Physical contact can request a transfer only through this ownership boundary; repeated callbacks cannot duplicate credit. A cosmetic body owns no extra food. If 1_07 chooses authoritative loose material, transfers move units out of the prior owner into that registered owner and back exactly once. A cancelled visual action reconstructs committed state, not a second in-flight inventory.

Partial amounts are valid, including the final batch. A deposit empties the finished carrier, increments stored food and awards proportional Coins atomically once, then returns the empty representation to its dock. Stored food cannot be withdrawn. A freely placed output carrier can coexist with output accumulating at the station. Before 2_01, the delivered handoff may have no budget yet; after 2_01 both values belong to the same transaction.

The **Finished Food Handoff Rack** is the single permanent deposit command target and the end of player food handling. Cellar and family boxes read `stored food`; they own no units, transfers, secondary inventory, allocation, or distribution jobs. Their apparent filling never subtracts from stored progress or leaves retrievable jars at the rack.

Upgrades preserve food and prior capabilities. The two prototype offers work independently and in either purchase order; derive capacities/operation behavior from installed improvement IDs rather than a single discovery ordinal. The capacity offer may bundle wheelbarrow and station support as specified by its configuration. A later powered conversion preserves both earlier gains. A paid pending installation survives pause/recovery/save without a second charge.

Equipment geometry and active controls derive from installed configuration. Keep one logical station/input/output and handoff; a local feeder extension is optional authored geometry, not a new queue or mandatory distant-route unlock. A pending upgrade keeps the old apparatus usable until its safe installation boundary, then reconstructs exactly one valid set of targets.

Free carrier placement and released-body physics follow the [handling contract](../core-loop-and-mechanics.md#pick-up-place-and-play). Physics supplies motion, while validated commands own pickup/release and food transfers. A position need not match a named mat. Record the chosen pose and a last safe pose; a carrier can be falling without its food becoming invalid. Toppling leaves the existing bulk contents intact. Recover a stuck/out-of-bounds carrier at a clear last safe pose, or a safe authored fallback, with the same identity and contents. Validate player coordinates on load and fall back to a safe spawn if needed. Recovery neither spawns extra harvest nor resets other valid object arrangements.

In the **currently delivered crate prototype**, held/parked ownership and the authored mat index belong to `HarvestState`; transforms are reconstructed views. R, Return to gate, and automatic fall recovery preserve local depletion, the raw load, queued/active/output food, and batch progress. F8 or the explicitly labelled Restart processing test button restores copied authored quantities, empties crate and station, clears batch time, and returns the crate to its initial mat. Both operations keep the current pause state. The mat index and lack of physical release are superseded design choices awaiting the 1_02 revision; this paragraph describes actual code, not the placement contract above.

**Current 1_03 backend:** `Tip` caps acceptance and immediately attempts to start a batch; `ActiveUnits` reserves output space and unpaused ticking completes it. Output collection is 1_04. **Planned 1_08 interaction:** loading queues food, and completing the directly controlled handle/rack operation validates/reserves space and starts the batch once. Save an interrupted stroke or restore a documented safe mechanical pose without changing committed food. Automatic internal stages and finished output wait safely; paused/focus-lost input cannot replay an operation. Historical auto-start tests do not establish this revised phase boundary.

The four household food displays derive from stored progress. Optional table/gift presentation also derives from the completed property and requires no independent meal, parcel, visitor, or reward state. A skipped visual milestone restores directly to the current display.

The [nearby amount-driven food group](../household-readiness-and-parcels.md#progress-drives-presentation) also reads stored units, including deposits between household milestones. Its meshes/partial fill introduce no new food owner, per-jar save data or pickup command. Restore current appearance without replaying deposits; empty/repeated requests, recovery and Coin spending cannot change the represented total. Display density may be bounded/approximate while partial-unit accounting remains exact.

## Equipment budget and purchases

Coins represent an equipment budget, not sales revenue. Initial earn rate is one Coin per accepted pepper-equivalent unit; store the authored rate/version explicitly. Handoff awards only committed units. Splitting a load gives exactly the same total; rounding or a per-deposit bonus cannot create extra income. Raw collection, tipping, operation, output pickup, empty/repeated deposits and reconstruction award nothing.

Track a nonnegative integer balance and stable paid/installed offer IDs. Purchase atomically checks the known offer, affordability and unowned status, subtracts its configured price once and records ownership or a paid pending installation. Failure changes nothing. The same food total remains stored. No extra food threshold, hidden discovery or yard condition precedes payment for the two prototype offers.

Capture food/budget/ownership coherently. Loading/recovery rebuilds effects without rerunning earning or charging; a paid upgrade waiting for a batch boundary remains paid. Validate nonnegative/finite ranges, known IDs, allowed combinations, duplicate ownership and consistency between paid/pending/installed state. A balance alone is not proof that configuration was affordable: 2_01/2_02 also verify the finite total, prices and both purchase orders with useful work left. Baseline equipment can always finish; no consumable expense or purchase can strand food.

Use one Coin unit per food unit initially to avoid fractional credit. A later rate/schema change needs an explicit compatibility decision and exact split-load equivalence. No multi-currency wallet, transaction service or economy framework is required.

## Attachment ownership and installation

For the [assisted-loading snap installation](../core-loop-and-mechanics.md#attach-a-purchased-improvement), use **available → purchased/awaiting installation → installed**. Available means unowned; one validated purchase spends the full price and records the paid entitlement before issuing its complete kit beside the apparatus. A failed or repeated purchase changes nothing. Installed implies paid ownership, one applied capability and no loose functional kit.

While awaiting installation, an unmounted kit uses shared physical handling/pose recovery and the old machine remains usable. Store whether its placement onto the mount has been accepted; this is a flag within the pending state, not another purchase stage. Accepted fitting secures the kit at the mount. The safe boundary is a resting mechanism with no active batch, before the next operation starts. Let an existing stroke and any batch it starts finish under the old configuration; the ordinary safe reset of an uncommitted stroke may return it to rest without changing food. Show which operation/batch is pending. Then install once, preserving queued food, output/reservations, carrier contents and the other improvement. Idle fitting applies immediately. Repeated contact, resume or reconstruction cannot charge or apply the effect again.

Recover a lost/inaccessible unmounted kit using the same stable identity at a clear nearby kit position. Remove or invalidate a stray representation before restoring the one usable body. An installed or already secured kit cannot be recovered as a second loose attachment. Interrupted handling retains the paid entitlement; physical loss never makes the offer purchasable again. Prototype restart resets food, Coins, ownership and kit views together, while ordinary recovery preserves them.

2_01 owns deterministic purchase, fitting, interruption and recovery checks in memory. M3 adds coherent capture/restore of the lifecycle, kit identity/pose/holder, accepted mount flag, machine state and derived effects; disk saving is not pulled into M2. Validate contradictory unowned/paid/installed states and duplicate kit IDs. Test unowned, paid loose, held/dropped, recovered, fitted-awaiting-boundary and installed snapshots in both purchase orders, including active/queued food and accumulated output. Reconstruction yields one usable kit or one installed effect, with no repeated payment or food loss.

## Harvest completion contract for M4 onward

Winter preparation completes when every initial harvest unit is stored and all other food owners are empty, including registered loose/transit units if used. Yard tidiness and object poses are irrelevant. The final deposit commits stored food, earned Coins and completion atomically, followed by quiet nonmodal feedback. Unspent Coins, unpurchased equipment, cellar views and household props add no requirements; there is no Finish Day command or mandatory ending transition.

Persist completion before relying on a visual callback. Loading a completed snapshot restores the finished yard directly with normal camera/movement and pause/menu controls, without replaying a reward or running a required sequence. A pre-completion snapshot remains unfinished until its remaining food is deposited. Reject inconsistent completion snapshots under normal snapshot validation. A repeated/empty deposit or reconstruction cannot re-trigger completion feedback as a reward. Machines reconstruct idle and completed food displays remain visible. Optional table/gift props are derived presentation, not new state inventories.

M1–M2 only verify that their small section can be fully stored, with plain completion feedback. They do not implement household display states or disk saves. M3 supplies the snapshot machinery; 4_03 applies this harvest-completion contract and 5_04 may supply cuttable closing presentation.

## Disk contract for M3

Use one local save slot under the application's persistent-data directory, a schema version, and a content version. Runtime state is ordinary serializable data. Do not save live Unity object references or use mutable ScriptableObjects as progress.

Capture a coherent model including food, Coins, ownership/pending installation and mechanism state after a committed action, with a coalesced autosave after meaningful progress and a flush for explicit save/exit. A quit with a partial carried load must preserve that committed load; autosave cannot depend only on completed deposits. Use brief, noninterrupting save feedback and avoid disk writes per decorative pepper. Pause can request a save, but gameplay remains usable if writing fails. Start a resumed game paused when necessary to prevent unintended progress during reconstruction.

Save player-arranged carriers and loose props by stable ID and pose, including rotations and supported stacks. Capture settled placements without writing every physics frame. Held objects resume under one holder at a valid carry pose; released objects saved in motion may restore safely at rest, with zero launch velocity, using their recorded pose if valid or their recovery pose otherwise. Validate support/clearance against the reconstructed scene, including other saved objects, before simulation resumes. Restore the arrangement coherently rather than dropping each object through a half-loaded stack. Do not reset valid arrangements to authored pads, and do not serialize every decorative pepper or duplicate food inside a physics snapshot.

Apply the same pose rules to an unmounted purchased kit; reconstruct a fitted kit at its mount and an installed attachment from its capability state. Later selected messages remain available to reread without replaying notification bursts: derive availability from saved progress/ownership and store only minimal shown/read IDs where necessary in 5_04. Do not build a mail service or add message persistence work before that presentation exists.

Write to a temporary sibling file, validate the serialized snapshot, then replace the current save while retaining one previous valid backup where supported. If a write fails, preserve the last valid save and show a retryable notice; do not claim success. Never intentionally destroy the only valid save to recover a bad write.

On load, validate schema/content versions, stable IDs, finite numeric values, bounds, capacities, and conservation. Corrupt or unsupported data must not silently start a fresh game. Offer the valid backup or an explicit new-game choice with a clear explanation; preserve the unreadable file for diagnosis. Do not invent migrations for older prototype schemas. Any later save format change needs an explicit compatibility decision and fixtures.

## Acceptance checks

Round-trip raw carrying, optional registered loose material, partial depletion, interrupted operation, active processing, output, carried finished food, Coin balance, both purchase orders, paid pending installation, stored food and completion. Preserve all quantities/entitlements and reconstruct usable controls without replaying deposits, charges or mechanism commands.

Round-trip free ground/worktop placements, rotated props and supported stacks, both loaded carriers, and a snapshot taken during a drop. Reject non-finite poses/duplicate IDs; recover obstructed or out-of-bounds objects without food loss or duplicate bodies. Check pause/resume while bodies are moving. These are future 1_02/1_06 handling and M3 persistence checks; historical mat-based passes do not establish them.

Verify interrupted/corrupt writes, unsupported versions, unknown/duplicate IDs, out-of-bounds quantities, and a missing content definition. Invalid data is rejected without overwriting a valid save. Reloading a deposit or completed property cannot repeat feedback as a reward.

The current runtime does not yet implement this snapshot. If a future implementation already has a serialized field named for the former day/ended concept, treat it as the harvest-completion semantic during a reviewed compatibility change; do not blindly rename serialized data in a documentation task.

These checks are mandatory when persistence is built. This document does not claim an existing save implementation.
