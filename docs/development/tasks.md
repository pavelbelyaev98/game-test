# Task queue

Continue the single `in_progress` task; otherwise execute the first `ready` task. The user may explicitly select another task or bounded batch. Git provides history.

Gameplay task numbers match their feature files; letter suffixes identify bounded implementation slices. `Sxx` identifies repository setup and `Mxx` identifies maintenance. Execute slices in row order; each owning spec names the next slice to make ready.

| ID | Task | Status | Source / record |
| --- | --- | --- | --- |
| `S01` | Repository foundation | `done` | [Completion record](completed/S01-foundation.md) |
| `S02` | Convert idea into feature backlog | `done` | [Completion record](completed/S02-feature-backlog.md) |
| `M01` | Restore compatible Unity package baseline | `done` | [Completion record](completed/M01-package-and-urp-baseline.md) |
| `01` | Core loop and first-playable scene | `done` | [Feature/task](../features/backlog/01-core-loop-contract.md); [completion](completed/01-core-loop-contract.md) |
| `02` | FPS movement and controls | `done` | [Feature/task](../features/backlog/02-fps-movement-and-controls.md); [completion](completed/02-fps-foundation.md) |
| `03a` | Finite excavation and first-playable scene shell | `ready` | [Feature/task](../features/backlog/03-excavation-and-terrain.md#first-playable-task-03a) |
| `07a` | Session inventory identities and values | `planned` | [Feature/task](../features/backlog/07-inventory-abstraction.md#first-playable-task-07a) |
| `06a` | Two terrain-exposed authored finds | `planned` | [Feature/task](../features/backlog/06-item-reveal-collection.md#first-playable-task-06a) |
| `04a` | Passive proximity pulse fixture | `planned` | [Feature/task](../features/backlog/04-detector-feedback.md#first-playable-task-04a) |
| `09a` | Two shovel levels connected to terrain | `planned` | [Feature/task](../features/backlog/09-shovel-progression.md#first-playable-task-09a) |
| `08a` | Session selling and paid shovel upgrade | `planned` | [Feature/task](../features/backlog/08-selling-upgrade-checkpoint.md#first-playable-task-08a) |
| `10a` | Surface recharge and coarse return warnings | `planned` | [Feature/task](../features/backlog/10-battery-jetpack-economy.md#first-playable-task-10a) |
| `11a` | Confirmed rescue with session consequences | `planned` | [Feature/task](../features/backlog/11-return-and-risk.md#first-playable-task-11a) |
| `01a` | Integrate and validate one complete excavation trip | `planned` | [Feature/task](../features/backlog/01-core-loop-contract.md#ordered-implementation-and-playable-proof) |
| `03` | Excavation and terrain | `planned` | [Feature/task](../features/backlog/03-excavation-and-terrain.md) |
| `04` | Detector feedback | `planned` | [Feature/task](../features/backlog/04-detector-feedback.md) |
| `05` | Discovery generation | `planned` | [Feature/task](../features/backlog/05-discovery-generation.md) |
| `06` | Item reveal and collection | `planned` | [Feature/task](../features/backlog/06-item-reveal-collection.md) |
| `07` | Inventory | `planned` | [Feature/task](../features/backlog/07-inventory-abstraction.md) |
| `08` | Selling and upgrade checkpoint | `planned` | [Feature/task](../features/backlog/08-selling-upgrade-checkpoint.md) |
| `09` | Shovel progression | `planned` | [Feature/task](../features/backlog/09-shovel-progression.md) |
| `10` | Battery and jetpack | `planned` | [Feature/task](../features/backlog/10-battery-jetpack-economy.md) |
| `11` | Return and risk | `planned` | [Feature/task](../features/backlog/11-return-and-risk.md) |
| `12` | Presentation and audio | `planned` | [Feature/task](../features/backlog/12-camera-audio-tone.md) |
| `13` | Discovery display | `planned` | [Feature/task](../features/backlog/13-progression-discovery-wall.md) |
| `14` | Final arc and ending | `planned` | [Feature/task](../features/backlog/14-final-arc-and-ending.md) |
| `15` | Optional systems | `planned` | [Feature/task](../features/backlog/15-optional-systems.md) |

Statuses: `planned`, `ready`, `in_progress`, `blocked`, `done`.
