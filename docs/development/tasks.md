# Task queue

Use numeric IDs only. Never add prefixes, suffixes, decimals, or parallel numbering. The next new task after `14` is `15`.

Continue the single `in_progress` task; otherwise execute the first `ready` task. A task may link any relevant feature document; feature filenames do not define task numbers.

| ID | Task | Status | Source / record |
| --- | --- | --- | --- |
| `01` | Repository foundation | `done` | [Completion](completed/01-foundation.md) |
| `02` | Convert idea into feature backlog | `done` | [Completion](completed/02-feature-backlog.md) |
| `03` | Package and URP baseline | `done` | [Completion](completed/03-package-and-urp.md) |
| `04` | Main game scene and loop plan | `done` | [Completion](completed/04-core-loop-plan.md); [feature](../features/backlog/core-loop.md) |
| `05` | FPS movement and controls | `done` | [Completion](completed/05-fps-foundation.md); [feature](../features/backlog/fps-controls.md) |
| `06` | Finite excavation in the main scene | `done` | [Completion](completed/06-terrain-shell.md); [feature](../features/backlog/excavation-terrain.md) |
| `07` | Session inventory identities and values | `ready` | [Feature/task](../features/backlog/inventory.md) |
| `08` | Terrain-exposed authored finds | `planned` | [Feature/task](../features/backlog/discovery-collection.md) |
| `09` | Passive detector feedback | `planned` | [Feature/task](../features/backlog/detector.md) |
| `10` | Two shovel levels connected to terrain | `planned` | [Feature/task](../features/backlog/shovel-progression.md) |
| `11` | Selling and paid shovel upgrade | `planned` | [Feature/task](../features/backlog/selling-upgrades.md) |
| `12` | Surface recharge and return warnings | `planned` | [Feature/task](../features/backlog/battery-jetpack.md) |
| `13` | Confirmed rescue and consequences | `planned` | [Feature/task](../features/backlog/return-rescue.md) |
| `14` | Integrate and validate one complete trip | `planned` | [Feature/task](../features/backlog/core-loop.md) |

After Task `14`, add only the next concrete implementation task as `15`; do not pre-create subtask IDs.
