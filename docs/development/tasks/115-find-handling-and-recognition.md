# Task 115 - Aimed uncovering, recognition and physical handling

Type: implementation. Status: `done`. User-selected follow-up to `112`/`114`. [Completion and evidence](../completed/115-find-handling-and-recognition.md).

Feature: [collection](../../features/backlog/discovery-collection.md); [physics context](96-discovery-physics-design.md), [recognition risks](../design-risks.md), [model replacement](../replacing-find-models.md). Prerequisites: `110`, `112`, `114`.

## Selected outcome

- Aiming Dig at a visible but ineligible rock excavates its remaining covering soil with the same assist as bottles. Retain ordinary reach, shovel radius/cadence, fuel and other-world-blocker checks.
- A stroke that crosses the exposure threshold leaves the item visible. Continued held/toggle Dig collects only after a short period of eligible direct observation; initial tuning is 0.6 seconds. Looking away/occlusion, falling below threshold, menus or focus loss reset observation. A fresh deliberate collection press can collect an already eligible item immediately. Never accumulate recognition while only a buried sliver is visible or collect across the scoop area.
- Right-click physically lifts an eligible directly aimed find into view; left-click throws the held object. Another right-click drops it. Optional handling does not add it to inventory, sell it, consume fuel, or require carrying it home. Existing held Dig remains the inventory collection path.
- Hold/throw/drop use the existing approved bottle/rock meshes and physical colliders. Per-item throw-speed tuning makes bottles travel farther than rocks. Physical collisions, pause/focus, menus, rescue/reset and save/reload preserve the same identity and a recoverable world pose. A held object restores as a released world object, with no duplicate inventory entry.
- Add a rebindable Grab/drop action without overwriting existing saved bindings. Throw follows the Dig binding, uses a fresh press even in toggle mode, and consumes that press; held/toggle input must not immediately dig or collect after throwing.
- Update target hints and the existing Pause/Controls reference. Keep docs/catalogs current so model replacement preserves handling settings. No new art/audio is requested.

## Acceptance

- MainGame rocks and bottles support aimed uncovering under weak/strong shovels without collecting during the revealing stroke or the immediately following held frames.
- Device-input checks demonstrate the recognition interval, continued hold/toggle collection, direct aim/visibility/60% threshold and fresh intentional pickup. Full inventory and pause/focus remain barriers.
- Physical grabbing/throwing checks cover all appearances, partial and detached pickup, world collisions, per-object throw speeds, one held object, input suppression, drop, menu/focus/rescue and capture/restore without loss/duplication.
- Existing input preferences migrate additively, bindings remain conflict-free, and hints follow remaps. Relevant deterministic, PlayMode/save/input checks pass.
- Inspect MainGame interaction/HUD through official Unity CLI and deliver an updated Windows build. User recognition/handling feel remains a playtest verdict in `101`.

Decision: the user explicitly confirmed the 0.6-second observation interval, RMB lift/drop and LMB throw. No blocking questions; remaining numerical tuning belongs to implementation and playtest `101`.
