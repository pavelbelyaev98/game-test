# Precision movement

Status: **implemented in [67](../../development/tasks/67-precision-movement.md)** following [66's selected design](../../development/tasks/66-precision-movement-design.md). Sustained production movement acceptance remains `05`/`54`. Idea coverage: section 50 and traversal throughout excavation.

## Selected interaction and rationale

The user selected **true crouch on held Left Ctrl, with slower horizontal steering on the ground and in the air**. Provide it from the start, with no equipment purchase. Preserve responsive normal walking and let players choose careful ledge approaches, landings and lower tunnels.

| Compared option | Decision and reason |
| --- | --- |
| Slow walking with unchanged stance | Rejected for this delivery: it reduces correction distance but cannot enter lower tunnels; the user chose that additional crouch capability. |
| True crouch with a lowered viewpoint | Selected: changes physical clearance as well as speed. Smooth, collision-safe height changes and safe saved stance are required. |
| Left Ctrl / Left Shift | Left Ctrl selected by the user; Shift has no crouch binding. Preserve existing Ctrl+Shift developer chords and their fresh action-key requirement. |
| Ground-only precision | Rejected: the user wants careful airborne corrections too; crossing a ledge must not suddenly restore full horizontal speed while crouched. |

The [comfort findings](../../research/player-review-findings.md#physical-comfort) motivate player choice, not a universal slower speed. Normal route widths and crouch dimensions remain implementation tuning; [66's baseline](../../development/tasks/66-precision-movement-design.md#research-and-evidence) distinguishes measured controller behavior from comfort acceptance.

## Movement and physical stance

- Hold Left Ctrl to crouch; release requests standing. Reduce normalized camera-yaw-relative horizontal speed immediately on the held request, throughout the height transition and while standing is blocked. Restore normal speed only once fully standing with Ctrl released. Keep immediate horizontal stopping/direction response; add no inertia or acceleration ramp.
- Crouch works while grounded, jumping, falling and flying. Keep radius, grounded step/slope capabilities, gravity, jump height, thrust delay/restart, ascent cap and energy rules. Space remains the same free jump/held jetpack action; do not require standing first. A low roof can physically limit a jump or ascent. Crouching cannot lift the feet, grant another jump, reset flight readiness or add upward impulse.
- Lower/raise the capsule and viewpoint together through a brief smooth, interruptible transition with the feet fixed relative to the player root. The camera must remain inside the safe body envelope throughout movement: shrinking the capsule instantly while leaving a high camera behind is unacceptable. Reversing Ctrl midway continues from the current height without a snap.
- Release under a low/sloping roof retains a safe crouched stance and precision speed. Stand automatically after entering sufficient headroom, including when digging removes the obstruction. Test the full intended standing clearance before rising and revalidate during expansion/movement; if clearance becomes blocked, stop expansion and safely return toward crouch without pushing through terrain or oscillating at the threshold.
- Check actual world blockers, including terrain chunk meshes, boundaries, props and exposed finds; exclude the player's own colliders and nonblocking triggers. Use occupancy checks as well as any sweep, account for skin/contact margins, and do not mistake ordinary floor contact for a ceiling. Use current collision after excavation; retain `26`/`34` cleanup and supported ledges.
- Height change alters the aiming origin naturally. Preserve centered targeting, dig/collection reach, cadence, hold-to-dig and camera sensitivity. Retain saved FOV and steady crosshair; do not add FOV kicks, bob, roll, shake or stance-linked crosshair animation. Physical crouch height is the selected exception to an otherwise fixed camera position, not a new camera-effects settings group.

## Recovery and feedback

- Menus and focus loss pause movement, stance animation, digging and thrust. Explicit resume samples the currently held Ctrl before movement; Ctrl is a continuous modifier, not a latched toggle or release-gated action. Held LMB/Space/E retain their existing release-before-resume barriers. Releasing Ctrl while paused requests safe standing only after resume; holding it through focus/device recovery cannot produce a full-speed frame.
- Save the actual physical stance, including a partial transition, with the same world snapshot as feet position/terrain. Never save a held-key latch. Restore valid capsule and matching camera height before the first playable frame, then use current input and clearance after explicit resume. Old version-1 saves migrate as standing; retain excavation, progression and interrupted-write recovery. Invalid/physically impossible restores use existing recovery UI rather than expanding into terrain, carving clearance or silently resetting the world.
- Rescue/developer return uses the existing clear standing surface anchor and clears transient transition state. Apply a currently held Ctrl safely before resumed movement. Disabling/re-enabling a player under a roof must not force an unsafe standing reset.
- Add **Left Ctrl — Hold to crouch / move carefully** to the compact Pause reference. The lowered view and slower movement communicate stance; no permanent HUD mode label or routine coaching. A failed release-to-stand attempt may show one brief **Low ceiling** notice; do not repeat it every frame or every automatic clearance check.

## Scope and future acceptance

No toggle crouch, prone/crawl, stamina, stealth benefit, automatic cliff guard, mandatory crouch puzzle or new equipment requirement. Toggle digging (`78`) and modest held sprint (`87`) belong to [FPS controls](fps-controls.md); crouch/blocked standing always override sprint. Crouch permits optional lower routes, not narrow-width squeezing: capsule radius stays unchanged. Ordinary sufficiently wide routes remain usable at normal speed; exact examples and numerical tuning belong to `67`.

`67` owns clearance/input/save regression checks and the MainGame/Windows feel review. `05` retains production presentation acceptance; `47`/`50` preserve the stance and airborne contract; `54` checks sustained comfort and Continue. Revisit transition tuning if actual play finds clipping, oscillation or discomfort; changing stance capability or input semantics requires a new explicit product decision.
