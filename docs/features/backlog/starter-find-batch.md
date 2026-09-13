# Starter bottle trial

**Currently disabled for new games:** the user requests a rock-only playtest in [126](../../development/tasks/126-shallow-find-density.md). All three bottles retain their assets, save keys and compatibility aliases for existing saves. Their earlier selected dimensions, **60% exposure** and detached physics remain below. Task [110](../../development/tasks/110-bottle-physics-and-recognition.md) removed cans/bricks; they are not approved to return automatically. All art remains **replaceable trial art** and better models are expected later.

[114](../../development/tasks/114-rock-variants-integration.md) integrates one common Rock with three appearances: A hollow, B/C solid, all with random 3D spawn rotation. Its [separate catalog/source guide](../../../art/photo-rock/README.md) owns the rock allocation and replacement contract. New games contain 336 rocks after `140`; this document owns the retained, inactive bottle subset.

The [editable catalog](../../../art/starter-finds/catalog.json) owns model/map paths, IDs, dimensions, prices, counts and compatibility aliases. Follow the [replacement guide](../../development/replacing-find-models.md) for model, reference and specification changes.

| Variant / stable content ID | Width x depth x height | Total / shallow | Value / slots |
| --- | --- | ---: | ---: |
| Tall - `common_bottle_tall` | 10 x 10 x 37.5 cm | 0 / 0 | 2 / 1 |
| Stubby - `common_bottle_stubby` | 12.5 x 12.5 x 30 cm | 0 / 0 | 2 / 1 |
| Square - `common_bottle_square` | 11.25 x 8.75 x 35 cm | 0 / 0 | 2 / 1 |

All display as Glass Bottle and remain common, `FindSize.Small`, one bag slot and detector-ineligible at every tier. Dimensions are **25% larger in every axis** than the original bottles, baked into Blender geometry with unit runtime scale. Existing saved populations and values remain historical; zero counts only disable new spawning. Counts/prices are trial tuning, not full-campaign solvency evidence.

## Appearance and recognition

- Muted green, amber and blue-green glass; long-neck, squat shoulder and rounded square-body silhouettes. Preserve thick lips, shaped bases, original worn material variation and torn Bulgarian labels. Glass stays opaque/frosted for soil readability.
- Larger bottles/60% reveal help recognition. The user clarified in `112` that holding Dig directly over an eligible bottle should collect it without releasing. A wide scoop leaves off-aim bottles visible/physical until hovered over; `111`'s fresh-press rule was an overcorrection. No forced pause, inspection modal, extra binding or detector cue is selected.
- Broad form/color reads first; faint Bulgarian packaging rewards close inspection and is never required reading or a clue. Avoid bland surfaces, pristine labels, noisy camouflage and uniform blurry mottling.
- Use the broad material treatment of the [preferred reference](https://juliovii.itch.io/ftpgrass-dirt); no reference artwork is imported. Final coordinated style remains `105`, final recognition/art acceptance `09`. A passing physics test is not a player readability verdict.

## Placement, motion and persistence

- Bottle orientation rules remain side-lying with varied yaw/tilt if explicitly reactivated. Current source counts are zero; restore uses saved poses and identities. Full weighted/cluster generation remains `45`.
- Collection requires at least 60% of 256 authored exterior samples, actual centre-ray visibility and 3 m reach. Held LMB clears nearby covering soil on a visible ineligible bottle and collects when directly aimed at the eligible result. Shovel radius/terrain removal alone never collect off-aim bottles. Full inventory leaves loot in place. [Collection](discovery-collection.md) owns hold/toggle, recovery and prompt behavior.
- Bottles remain anchored while soil intersects their centre/exterior/interior support samples. Pickup eligibility does not release physics. Detached bottles fall, tip and settle on actual rebuilt ground using a separate Blender-authored convex hull (200/198/190 triangles), gravity, damped rotation and zero bounce; player collision is ignored so they cannot obstruct walking/crouching.
- Released bottles remain physical when settled and wake after nearby digging. Ground reset reanchors reburied finds. Pause/loading suspend motion; no destruction, mandatory carrying, manual placement or rolling-loot puzzle is added. Invalid/deeply penetrating/out-of-site motion returns the same identity to its last clear pose and holds it until another terrain change.
- Save version 3 records current pose and released state; v1/v2 readers remain supported. Reload starts motion from rest at the saved pose and gravity continues after resume. Motion/settling participates in autosave dirtiness independently of player input.
- Existing 192-find, 96-find and 72-find saves retain population, centres, rotations, collected absence and historical inventory/value records. Original primitive GUIDs and the four retired can/brick IDs map directly to current bottle IDs in the catalog. Only uncollected names change; never delete/reroll a save or remove aliases as obsolete model references.
- Exports use base-centre pivots; sync centres runtime geometry and support/exposure samples around the persisted find centre. Preserve bottle content IDs and Unity metas/GUIDs during future swaps. Each catalog entry has both `fbx` and `collision_fbx` paths; replacement hulls stay within 112 distinct points / 220 triangles and match the visual envelope. The convex hull is for physical collision; exposure uses the visual exterior, not the hull or an ellipsoid.

## Later roster and setting

The user's other liked categories remain roster preferences for `40`: jars, large tile fragments, chunky scrap/pipe/wire, pottery, ordinary bones, construction debris and larger metal tins. Cans/bricks were explicitly removed from this trial; reintroducing assets requires a new specific request/approval. Broader physics categories remain design `96` and special interaction/chests `97`.

Riverbed versus drained reservoir remains `58`. Current bottle sources are [here](../../../art/starter-finds/README.md); ownership and removal are in the [ledger](../../asset-ledger.md). Tasks `108`/`109` record original delivery; `110` owns this revision, `09`/`105` final model/style acceptance.
