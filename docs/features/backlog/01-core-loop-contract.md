# 01 - Core loop and first-playable scene

Status: task `01` is done (documentation-only). Task `01a` owns playable integration; slice state lives in the [queue](../../development/tasks.md).

Idea coverage: sections 1-3, 6, 51-52, and 54.

## Purpose and planning task

The game is a short, funny first-person excavation game that starts believable and becomes absurd. Its repeatable trip is dig, detect, uncover, collect, consider pushing farther, return, sell, recharge, upgrade, and repeat. The surface is a short checkpoint. Task `01` defines the smallest scene for one complete trip, its objects/owners, temporary fixtures and first implementation tasks; production versions of later systems stay with their features.

## Required behavior

- Start from untouched ground with no forced route.
- Let a player find something, return physically, sell it, purchase one improvement, and dig again.
- Use a finite authored surface with attractive scenery, some visible water, no required swimming, visually distinct permanent boundaries, and a persistent excavation.
- Target roughly 2-3 hours for first completion, with replayability from changed underground placements and optional completion.
- Do not turn the project into survival, crafting, puzzle, geology, museum, inventory-management, infinite-world, or automation gameplay. The player keeps doing the digging.

## Done when

- The scene plan maps each object to an existing or planned runtime owner.
- Follow-up tasks are small, ordered, and link their owning feature document.
- The plan names one playable proof of the full loop and its main limitation.
- Its proof recreates the intended tension: a signal tempts the player to make one more discovery before a risky return and meaningful upgrade.

## First-playable scene plan

Planned `unity/Assets/Scenes/FirstPlayable.unity`: a 32 x 32 m authored surface around a 24 x 24 x 12 m untouched dig volume. Start on the south rim at ground level; digging can begin anywhere inside the volume. These are prototype dimensions. Keep `FpsValidation.unity` as the controls regression scene.

Paths below are relative to `unity/Assets/Runtime/`; named new owners are proposals, not existing code. A planned editor-only `FirstPlayableSceneBuilder` authors references; it owns no gameplay state.

| Scene object | Runtime owner and responsibility |
| --- | --- |
| `FirstPlayableRoot` | Planned `Loop/FirstPlayableSession` (`01a`) wires owners and initializes a fresh scene session; checkpoints and rescue never recreate it. |
| `Player / Camera / HUD` | Existing `Player/FpsPlayer`, `FpsInput`, `Battery`, `UI/FpsHud`, CharacterController and child Camera/AudioListener; preserve Input System input and menu gating. |
| `Excavation / Chunks` | Planned `Terrain/TerrainVolume` (`03a`) implements `IDigTarget`, owns removed-volume state and rebuilds affected render/collision geometry. |
| `Surface / Bedrock / Perimeter / Scenery / Water / Sun` | Authored meshes, colliders and Light (`03a`); contrasting permanent bedrock/walls exclude excavation. Primitive rocks/trees frame visible water beyond a solid shoreline boundary; no swimming. Surface colliders stop at the dig-volume rim. |
| `Discoveries / FindA / FindB` | Planned `Discovery/BuriedFind` plus finite records (`06a`) own identity, terrain-derived exposure and collected state through `IInteractionTarget`; authored positions replace generation for this proof. |
| `Player / Detector` | Planned `Discovery/PassiveDetector` (`04a`) reads uncollected discovery positions and feeds anonymous proximity pulses to `FpsHud`. |
| `Player / Inventory / Shovel` | Existing `Interaction/SessionInventory` gains item records (`07a`); planned `Player/ShovelState` (`09a`) supplies current dig settings. |
| `Surface / SellStation / UpgradeStation` | Planned `Economy/SellStation`, `UpgradeStation` extend `StationTarget`; `SessionWallet` (`08a`) owns money. Separate stations stand within 5 m of the south rim. |
| `Surface / RechargeZone / ReturnAnchor` | Planned `Player/SurfaceRecharge`, `ReturnWarning` (`10a`) use the existing battery; planned `Loop/RescueController` (`11a`) applies confirmed costs and returns to a clear anchor. |

Temporary fixtures: Unity primitives, colored URP materials, two ordinary authored finds (A: 10 credits; B: 20), a 10-credit shovel upgrade and visual detector pulses. No validation adapter supplies gameplay truth in this scene. Generated/imported non-primitive art or audio requires a ledger entry before use.

## Ordered implementation and playable proof

Implement `03a` -> `07a` -> `06a` -> `04a` -> `09a` -> `08a` -> `10a` -> `11a` -> `01a`. The [queue](../../development/tasks.md) links each slice's scope and acceptance to its owning feature. Full production feature rows remain planned.

Task `01a` integrates and tunes this proof after those slices pass:

1. Start a fresh scene with full battery, empty inventory, zero money, basic shovel and completely covered finds. Dig downward and laterally toward passive feedback, then expose and collect A; no route is pre-dug.
2. From A, a signal from deeper, laterally offset B tempts another dig. Tune an authored test route so returning now succeeds, while chasing B crosses the risky warning and can leave too little thrust for the same direct ascent. Record observed charge/costs; UI shows no exact return guarantee.
3. Choose the physical return along the player's excavation. At the surface, explicitly sell A for 10, recharge free, and purchase the 10-credit improvement at the separate station; merely opening either menu changes nothing.
4. Return through the unchanged excavation and dig toward B. The improved shovel removes more of the same soil per accepted hit at the same energy cost; compare equal fresh patches before/after. A stays collected and cannot be sold again.
5. Exercise the alternative: overextend, deplete, cancel rescue without mutation, then confirm its displayed loot/money cost and recover at the surface. Excavation, collected-find state and purchased upgrades survive rescue.

`01a` passes when focused integration checks cover both branches, transactions and trip persistence, existing FPS regressions pass, and an actual editor play review records legible boundaries/water, usable stations, the tempting signal and observable upgrade. Use Unity MCP for live scene/editor state; its current absence must be resolved before that review, without guessing the editor state.

Main limitation: this is a short session-only proof with authored finds and primitive presentation, not the 2-3 hour game. Scene reload resets terrain, loot, wallet and upgrades; saving, randomized placements, full tool progression, fall consequences, final art/audio and long-run balance remain with their owning features. Terrain representation and warning tuning are decided and measured in their implementation slices.
