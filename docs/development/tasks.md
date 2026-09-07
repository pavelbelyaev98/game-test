# Task queue

Continue the single `in_progress` task; otherwise execute the first `ready` task. The user may explicitly select another task or bounded batch. Git provides history.

Gameplay task numbers match their feature file numbers. `Sxx` identifies repository setup and `Mxx` identifies maintenance outside gameplay features.

| ID | Task | Status | Source / record |
| --- | --- | --- | --- |
| `S01` | Repository foundation | `done` | [Completion record](completed/S01-foundation.md) |
| `S02` | Convert idea into feature backlog | `done` | [Completion record](completed/S02-feature-backlog.md) |
| `M01` | Restore compatible Unity package baseline | `in_progress` | Resolve manifest/lockfile, compile, and run relevant integration checks once. |
| `01` | Core loop and first-playable scene | `ready` | [Feature/task](../features/backlog/01-core-loop-contract.md) |
| `02` | FPS movement and controls | `done` | [Feature/task](../features/backlog/02-fps-movement-and-controls.md); [completion](completed/02-fps-foundation.md) |
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
