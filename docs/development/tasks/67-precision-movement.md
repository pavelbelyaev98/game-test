# Task 67 - Implement held precision crouch

Type: implementation. Status: `done`. Prerequisites: `65`, `66` (complete). [Completion](../completed/67-precision-movement.md).

Feature: [precision movement](../../features/backlog/precision-movement.md). [Design/evidence](66-precision-movement-design.md). [Research](../../research/player-review-findings.md#physical-comfort). [Queue](../tasks.md).

## Task contract

- Implement `66`'s selected true crouch on held **Left Ctrl**, reducing horizontal steering on ground/in air. Integrate with the existing Input System/controller, physical capsule, centered targeting and compact Toolkit Pause reference.
- Follow the feature's smooth foot-preserving body/camera transition, safe blocked stand-up, continuous Ctrl recovery and unchanged vertical Space/jetpack contract. Preserve `65` FOV/reticle settings and `26`/`34` terrain cleanup.
- Extend the whole-world save boundary for actual stance/partial transitions, including dirty detection, validation, restore ordering and version-1 migration. Preserve previous checkpoints and recovery behavior. Do not serialize held input or reset excavation to resolve clearance.
- Inspect current controller/save ownership before editing. Use official Unity CLI and applicable UI skills; the design links checked Unity collision/input references. Ask only if evidence requires changing a selected product behavior.

## Initial tuning and route examples

| Parameter | Starting point; tune through implementation review |
| --- | --- |
| Horizontal speed | 35% of normal (1.4 vs 4 m/s today); immediate precision response, no new acceleration/inertia. Normal speed returns only when fully standing and Ctrl released. |
| Capsule / eye height | Standing 1.8 / 1.6 m; crouched 1.1 / 0.95 m. Radius remains 0.3 m; keep feet fixed by adjusting capsule center. |
| Height transition | Approximately 0.2 s each direction, smoothly coupled body/camera with safe interrupted reversal. Preserve look control, FOV and reticle behavior. |
| Ordinary routes | Roughly 1.2 m-wide supported routes, ramps up to 30°, steps up to 0.2 m, tunnel turns with about 2.1 m headroom. Keep normal walking useful; these are evaluation examples, not forced player routes. |
| Precision / crouch routes | Compare 0.8–1.0 m supported ledges, small landing pads and roughly 1.3 m-high tunnels; compare a lower-than-crouch passage that remains blocked. Width still respects the unchanged capsule radius. |

Tune numerical margins/heights/times, not new stance/input semantics. `66` measured only the existing standing controller and static capsule queries; it did not prove dynamic resizing, crouch feel or Windows input.

## Acceptance

- Simulated Input System devices verify Left Ctrl alone, press/hold/release/reversal, normalized diagonal speed, held LMB excavation/collection, and coexistence with Ctrl+Shift admin chords. Inactive crouch preserves normal speed; no toggle, sprint or permanent HUD coaching is added.
- Compare normal/crouched traversal at 30/60/144 Hz on supported ledges, slopes, steps, tunnel corners and edited chunk seams, including diagonal motion. No clipping, jitter, stuck floor-contact clearance, radius squeezing or artificial cliff stop. Releasing into a low roof retains crouch/slow movement; leaving/digging out stands safely without repeated notices or oscillation.
- Approach low roofs while lowering, move/jump/fly during either transition, reverse Ctrl midway, and obstruct a stand-up as it proceeds. Camera/near-plane view must not enter solid terrain. Resize cannot move feet, create a jump/grounding exploit, change flight readiness or consume charge itself; real ceiling collision still constrains ascent.
- Verify Space tap/first hold/airborne restart/landing/depletion at both stances and while changing them. Precision affects horizontal steering only. Look/targeting/held digging work from the lowered origin with unchanged range and battery rules.
- Exercise Pause, inventory, station, comfort/admin/persistence menus, focus/device loss, disable/re-enable, rescue and developer return while holding/releasing Ctrl, LMB, Space and E. Menus freeze stance/body/camera and world actions; explicit resume uses current Ctrl before movement and preserves LMB/Space/E release barriers.
- Save/reload fully crouched and mid-transition under a roof and in the air, Ctrl held/released, normal quit and interrupted writes. Capsule/viewpoint restore before play; an old version-1 world migrates without terrain/economy loss. Malformed stance or impossible occupancy uses recovery rather than clipping or silent reset. Rescue returns to safe clearance without leaving a stuck stance or input latch.
- Inspect MainGame at FOV 55/75/90 and supported window sizes: smooth lowered viewpoint, usable targeting, unobscured future-tool integration points, readable Pause reference and one-shot Low ceiling feedback. Stable stance adds no recurring allocation, unnecessary geometry mutation, UI rebuild or per-frame save writes.
- Run proportionate deterministic checks after code changes and the full relevant regression set once near completion. Inspect actual excavated routes, perform the brief announced Windows feel/input review, and deliver `builds/windows/SomethingDownThere.exe`. `05` retains production presentation acceptance; `47`/`50` and `54` retain the movement contract.

## Delivered tuning

The starting speed, dimensions and 0.2 s transition passed implementation review. The near plane now stays inside the effective capsule across stance, FOV and viewport changes without changing FOV. Version 2 snapshots preserve stance; version 1 loads as standing. Regression, visual/native evidence and the review build are linked in the completion record.
