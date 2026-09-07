# Task queue

Use numeric IDs only. Never add prefixes, suffixes, decimals, or parallel numbering. Continue the single `in_progress` task; otherwise execute the first `ready` task.

| ID | Task | Status | Source / record |
| --- | --- | --- | --- |
| `01` | Repository foundation | `done` | [Completion](completed/01-foundation.md) |
| `02` | Convert idea into feature backlog | `done` | [Completion](completed/02-feature-backlog.md) |
| `03` | Package and URP baseline | `done` | [Completion](completed/03-package-and-urp.md) |
| `04` | Main game scene and loop plan | `done` | [Completion](completed/04-core-loop-plan.md); [feature](../features/backlog/core-loop.md) |
| `05` | FPS movement and controls | `planned` | Reopened by `18`: mechanics verified; production HUD/feel acceptance after `08`; [record](completed/05-fps-foundation.md); [feature](../features/backlog/fps-controls.md) |
| `06` | Finite excavation in the main scene | `planned` | Reopened by `18`: terrain verified; approved presentation acceptance after `08`; [record](completed/06-terrain-shell.md); [feature](../features/backlog/excavation-terrain.md) |
| `07` | Session inventory identities and values | `planned` | Reopened by `18`: records verified; production inspection UI acceptance after `08`; [record](completed/07-session-inventory.md); [feature](../features/backlog/inventory.md) |
| `08` | Production visual and audio foundation | `planned` | Deferred by user: terrain art handled separately; no new scenery/audio; [feature/task](../features/backlog/presentation-audio.md) |
| `09` | Production discovery reveal and collection | `planned` | [Feature/task](../features/backlog/discovery-collection.md) |
| `10` | Passive detector feedback | `planned` | [Feature/task](../features/backlog/detector.md) |
| `11` | Shovel progression connected to terrain | `planned` | [Feature/task](../features/backlog/shovel-progression.md) |
| `12` | Selling and paid shovel upgrade | `planned` | [Feature/task](../features/backlog/selling-upgrades.md) |
| `13` | Surface recharge and return warnings | `planned` | [Feature/task](../features/backlog/battery-jetpack.md) |
| `14` | Confirmed rescue and consequences | `planned` | [Feature/task](../features/backlog/return-rescue.md) |
| `15` | Integrate and validate one complete trip | `planned` | [Feature/task](../features/backlog/core-loop.md) |
| `16` | Adaptive review window and jump/hold jetpack | `ready` | Window verified by `19`; movement/jetpack feel review pending; [feature](../features/backlog/fps-controls.md) |
| `17` | Official Unity CLI, Pipeline, and Codex integration | `done` | [Completion](completed/17-unity-cli.md); [setup](../../unity/readme.md); official beta/experimental releases required for this workflow |
| `18` | Direct CLI workflow and completed-task audit against official skills | `done` | [Completion](completed/18-cli-and-skill-audit.md); [workflow](../../unity/readme.md); all 54 checks passed; `05`-`07` production acceptance reopened |
| `19` | Revert asset pass, improve rendering, and repair Blender connection | `done` | [Completion](completed/19-presentation-rollback.md); [scope](../features/backlog/presentation-audio.md); [asset inventory](../asset-ledger.md) |

No task is active. Next ready task is `16`; its existing window/jump behavior is preserved. New art/audio work is deferred to a separate user request; revalidate `05`-`07` only after explicitly scoped presentation work. The next new task ID is `20`.
