# Task 66 - Design precision movement for player-made ledges

Type: design/research; documentation only. Status: `ready`. Prerequisites: `65` (complete).

Feature: [precision movement](../../features/backlog/precision-movement.md). Implementation: [67](67-precision-movement.md). [Research](../../research/player-review-findings.md#physical-comfort). [Queue](../tasks.md).

## Why and current gap

Walking is currently a single speed with no precision/crouch input. The reports describe differing speed preferences: support careful movement on excavated ledges without making all walking slow. This is separate from already-working hold-to-dig and camera comfort.

## Research and proposal

- Inspect the actual Input System, controller, slopes/steps, jump/jetpack and `65` settings. Observe ledge approaches, ramps, tunnel turns and ceiling clearance at normal speed.
- Compare a held slow-walk modifier with a true lowered crouch. Recommend the smallest coherent interaction that solves observed control problems; a stance change is a product choice, not an assumed prerequisite.
- Define the key, speed/acceleration, camera-height transition if any, standing clearance, airborne/jetpack behaviour and focus/menu/save recovery. Preserve responsive normal movement and the single Space contract.
- If lowering the capsule, research safe resize/stand-up under low ceilings against game-owned terrain. No automatic cliff protection, stealth bonus, extra stamina or new camera bob follows from this task.

## Questions to resolve with the user

- Prefer precision walking only, or a real crouched stance that also fits under lower ceilings, based on the compared examples?
- Which held key and camera-height response feel comfortable, and should the modifier affect airborne steering or only grounded movement?
- What ledges/ramps should remain comfortably usable without precision mode? Recommend concrete examples, then tune numerical speed/clearance through play.

## Done when

Record the selected behaviour, rationale, rejected alternatives, input/clearance cases and remaining tuning in the feature. `67` has a testable contract; this task does not implement movement or authorize assets.
