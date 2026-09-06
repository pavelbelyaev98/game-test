# 2_01 — Coins and two equipment improvements

Milestone: M2 · Type: Feature · Status: [central task queue](readme.md#ordered-task-queue)

**Outcome:** Earn equipment budget from finished food, choose between two useful improvements, and physically snap on the assisted-loading purchase before repeating the work.

**Depends on:** [1_08 — Direct machine operation](1_08_direct-machine-operation.md). All earlier play gates must be resolved under the queue rules.

## Context to read

Read the [common context and task protocol](readme.md#context-for-every-new-chat), dependency delivery/feedback records, and [core mechanics](../../core-loop-and-mechanics.md), [scope/validation](../../scope-and-validation.md), [state/saving](../state-and-saving.md), [architecture](../../../ARCHITECTURE.md), and relevant [verification](../testing-and-performance.md) / [asset rules](../unity-and-assets.md) and [equipment stages](../../yard-and-progression.md#grandpas-three-equipment-stages). Inspect actual source, scene/input wiring and pinned versions before implementation; the brief is not delivery evidence.

## Work

- Implement one Coin balance separate from stored food. Handoff awards one Coin per accepted pepper-equivalent unit as initial configuration, atomically with the deposit. Partial batches earn proportionally; splitting gives no bonus, and empty/repeated deposits or recovery earn nothing.
- Wire one clear bench/interface beside the apparatus showing both offers from the beginning. Each card gives the actual workflow effect, complete package price, affordability, available / owned-awaiting-installation / installed state and installation location or automatic safe-boundary behavior. One price includes everything; no materials list, personal/fund wallets, second currency, separate catalogue, hidden second shop, buried equipment, area access or prepared-food threshold followed by payment.
- Implement both real improvements: coordinated larger batch/carrier capacity (initially 48-unit wheelbarrow plus matching hopper/output), and an assisted loading rack whose short direct lever stroke moves the whole rack with less operator travel/held effort. The latter changes direct handling, not merely an internal processing timer. Use 1_08 measurements to tune or revise the concrete attachment if that effect is not useful; record the final effect shown by the bench.
- Connect each offer to a visible changed part and a plain workflow effect the player can explain after using it. A wide hopper/receiving-tray example may illustrate an existing capacity or handling benefit; it is not permission to add extra offers. Preserve useful pouring, receiving and orderly automatic packing while removing redundant handling/confirmations.
- Choose prices from the finite test total/earn rate so either is an early first purchase after a few batches, both orders are viable, both can be afforded with useful work left, and the baseline can finish without either. Do not degrade baseline controls or add supply to hide a bad purchase economy.
- Purchase once through a validated atomic command; retain paid ownership/pending installation at safe cycle boundaries. Combining upgrades preserves both gains. Held loads, reservations, output and all existing food remain intact; never spend stored winter food.
- After the approved earning/two-purchase flow works, give only the assisted-loading purchase the [short snap installation](../../core-loop-and-mechanics.md#attach-a-purchased-improvement). Supply one complete kit beside one large machine mount, reuse grab/place controls with forgiving alignment, and confirm with snap/sound/small mechanism motion. Show all information at this workstation. The capacity/wheelbarrow package adds no assembly action; no third purchase, precision rotation, tools, component hunt or delivery wait/walk.
- Implement the [attachment lifecycle](../state-and-saving.md#attachment-ownership-and-installation) using the existing ownership/handling boundary: preserve paid entitlement through interrupted placement, recover the same kit without duplicate bodies/effects, and accept fitting during work with a clear pending-boundary status. The old apparatus remains usable until the safe boundary; apply once after the current operation/batch and before the next. Preserve food, prior capabilities and output access.
- Reuse comfortable free wheelbarrow release/regrab, assisted steering/reversing and shared carrier controls. A retired empty crate may stay a prop without a second food inventory. Record exact costs, effects and basic both-order player evidence.
- Keep kit, supply, machine and handoff close. Record one-time installation finding/understanding time, placement attempts/rejections and any boundary wait separately from recurring batch work; 2_02 owns the matched comparison. At most one inexpensive optional original static gag may accompany this test under the [comedy contract](../../jobs-events-and-comedy.md#optional-messages-and-comic-variety); no new computer/mail system or mandatory writing work.

## Acceptance

- Handoff earns exactly proportional Coins while stock only increases; split/repeated/empty requests cannot mint budget. Unaffordable/repeated purchases leave state unchanged.
- Both offers are independently purchasable and useful on the next repeated batch; there is no one-compulsory-option-at-a-time pseudo-choice. Either order and the combined apparatus work with partial loads and loaded/pending installation.
- A finite-budget viability check covers the configured total/costs and both orders, without requiring all equipment to complete food. Scene/UI/input and the ordinary player demonstrate earning, choosing, paying and using each effect.
- Coin/ownership recovery and paused installation are verified in memory; M3 owns disk persistence. Human preference remains pending until supplied.
- Deterministic checks cover purchase once, interrupted pickup/placement, lost-kit recovery, repeated fit, fitted-awaiting-boundary and installed states in either purchase order. No second charge, functional copy or doubled effect; the old machine, active/queued food and output remain usable. Actual input in the scene/player demonstrates forgiving fitting and visible pending/installed feedback, including muted play.

## Human playtest check

Hand off a few batches, inspect both complete offers and choose one. If choosing assisted loading, find the nearby kit/mount, freely set the kit down and fit it, then repeat ordinary batches. Try recovery/interruption and fitting during work without repurchase. In a fresh test choose the other first, then combine them. Check that spending reduces only Coins and that the installed effect changes the next appropriate batch; assess any optional joke separately.

**Outside this task:** customers, food sales, recurring expenses, a large tree, discovery-gated equipment, final powered conversion, disk saving or second products.

## Finish this task

Follow the [handoff and recording rules](readme.md#handoff-and-recording). Own scene/prefab, input, UI, assets and build wiring; record exact scene/build, controls, short checklist, checks, limitations and actual feedback. Update queue/milestone records and stop after this handoff.

Next in order: [2_02 — Upgrade throughput and handling](2_02_upgrade-throughput-and-handling.md).

## Planning record — September 6, 2026

Reframed from wheelbarrow discovery. The stable filename preserves links; the developer explicitly authorized minimal earn-and-spend progression in the revised prototype. Todo / Not tested; no purchase or feel evidence is claimed.

The subsequent [targeted feedback refinement](../../design-pivot.md#snap-installation-and-restrained-comic-variety--september-6-2026) assigns one complete assisted-loading kit and forgiving snap installation to this existing task. Cards, in-memory ownership/recovery and one-time effort reporting are refined; disk saves remain M3 and neither a third purchase nor a new task is added. This is planning, not installation delivery or human acceptance.
