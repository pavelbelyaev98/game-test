# Discovery reveal and collection

Current content: the [three-bottle trial](starter-find-batch.md) and [three-appearance Rock](../../../art/photo-rock/README.md), integrated in `110`/`114`, with **60% required exposure** and physical release. [115](../../development/tasks/115-find-handling-and-recognition.md) owns the selected aimed-uncovering, recognition and lift/throw refinement below. `09` retains final discovery-art/recognition acceptance.

Idea coverage: sections 19-22 and relevant tuning in section 53.

## Purpose and selected behavior

The player should see the exposed object before held Dig collects it. The user confirmed **0.6 seconds of eligible direct observation** in `115`, because `112` could collect on the frame immediately after a powerful revealing stroke. Mandatory release/repress remains rejected; a separate deliberate press can collect an already eligible find immediately. Cleaning minigames and modal inspection remain excluded.

- Covered finds cannot be named or collected through terrain. Require the authored threshold plus actual collider visibility and 3 m reach; developer X-ray never bypasses these rules.
- Active bottles and rocks require at least 60% of 256 real exterior samples. Aim and hold Dig on either size of visible ineligible find to excavate its covering soil using ordinary shovel reach/radius/cadence/fuel. Only the aimed find is ignored by those queries; other obstacles remain blockers.
- A revealing stroke ends its action and leaves the find visible. Automatic collection requires 0.6 seconds of continuous eligible direct observation, then transfers exactly once while Dig remains active. Observation before reaching the threshold never counts; looking away, occlusion, reburial, menu/focus loss or handling resets it. A fresh deliberate press can collect an already eligible find. Pickup uses the centre ray independently of shovel radius, costs no energy and cannot also dig that frame. Terrain removal/release never mutate inventory.
- Toggle mode provides the same aimed pickup while active and continues after collection. Its stop press performs no dig or pickup. The target prompt shows the current bound action and Hold/Toggle mode.
- Full inventory leaves the find available and suppresses repeated error spam. Making room permits collection only with eligible, recognized direct aim and active Dig, or a deliberate press. Names identify immediately without appraisal or other chores. Inventory and collected absence persist across surface trips, sale, rescue and reload.
- Menus/focus loss clear held action intent. E remains for stations; deliberate special-find interaction/chests are still proposed in `97`.
- Common bottles and rocks are detector-silent without exceptions for size, material, clusters or upgrades. Common repetition should feel useful and recognizable; higher tiers carry stronger surprise.

## Selected bottle and rock physics

**Optional handling (`115`):** RMB / rebindable **Lift / drop find** lifts an eligible directly aimed object into view; another press drops it. A fresh Dig press (default LMB) throws it. The actual world rigid body remains visible and collides with terrain/props; lifting never puts it in inventory and also works with a full bag. Dig/automatic collection are blocked while holding. Throw consumes its press and clears held/toggle digging until a new press. No breakage, fuel cost or mandatory carrying home. Per-item source-catalog `throw_speed` starts at 8 m/s for bottles and 4 m/s for Rock.

| State / trigger | Behavior and player expectation |
| --- | --- |
| Embedded or partly exposed | Kinematic and anchored while soil intersects the centre or exterior/interior support samples. 60% pickup eligibility does not release it. |
| Detached after terrain rebuild | Convex physical collider and gravity let it fall, tip and settle on actual terrain. Restrained friction/damping and zero bounce reduce loot chasing; collision with the player is ignored. |
| Held while looking/moving/flying | Track the player's actual movement independently of throw strength; normal jetpack ascent must not outrun the hand. Keep the rotated hand target clear of soil/props while the real body obeys collisions. |
| Dropped and nearly at rest | Use discrete contacts at low speed, restoring continuous detection for fast travel/holding; stop persistent small rocking after a quiet interval on real upward support. Keep the body dynamic and wakeable. [116](../../development/tasks/116-find-hold-and-rest-stability.md) owns the stability repair. |
| Resting, then soil below is dug | Keep the released body physical; nearby terrain changes wake it. No floating anchored remnant after its support disappears. |
| Pause/loading | Freeze with the existing pause flow and suspend physics while terrain colliders restore. Resume after the world is ready. |
| Save/reload | Save current pose and released state; resume from rest at that pose. Falling continues under gravity. Motion/settling alone makes autosave dirty. |
| Holding across pause/focus or checkpoint | Menus freeze the held object and suppress old clicks. Save its world pose/released state; reload leaves one released world object, with empty hands and unchanged inventory. |
| Rescue/return/reset or player shutdown | Release the held object at its current position before moving the player. Preserve its world identity; no teleport into inventory or duplicate. |
| Admin ground reset | Reburied uncollected bottles/rocks anchor at their current saved position. Collected identities stay absent. |
| Invalid/out-of-site/deep penetration | Restore the same find to its last clear pose and hold until the next nearby terrain change. Never reroll, destroy, auto-sell or duplicate it. |

Physics and optional lifting/throwing are selected for these bottles and the ordinary Rock. No breakage, mandatory carrying, push puzzle or unrestricted handling on every future discovery is implied. [96](../../development/tasks/96-discovery-physics-design.md) retains remaining bulky/special/ceiling category decisions; [97](../../development/tasks/97-special-find-interaction-design.md) retains special interactions and chests.

## Sources and acceptance owners

- [Starter batch](starter-find-batch.md) owns bottle dimensions/counts/values and trial state; [replacement guide](../../development/replacing-find-models.md) owns source/mesh/material/reference/spec changes and compatible removal. `110` owns bottle physics/size, `114` rock integration; `115` refines `112`'s held/aimed rule with recognition and optional handling.
- [09](../../development/tasks/09-starter-discoveries.md) and [57](../../development/tasks/57-presentation-design.md) retain final presentation/recognition; [101](../../development/tasks/101-collection-return-playtest.md) records current user feel, including support/drop and larger-bottle readability. Automated collection/physics checks do not establish subjective recognition.
- Full roster/batches/generation remain `40`-`45`, saving `35`, personal display `60`/`51`; permanent passive discoveries `36` use shared reveal/aim rules outside ordinary sale flow.
- Historical tasks `27`/`28` established buried finds/X-ray, `30` held collection, `72` the previous 40% threshold and `86` aimed digging around small finds. Their original primitive content/threshold are superseded by `109`/`110`; `111`'s fresh-press experiment is superseded by `112`. Preserve direct aim, visibility, one-identity pickup and save guarantees.
