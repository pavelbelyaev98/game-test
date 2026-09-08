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
| `16` | Adaptive review window and jump/hold jetpack | `done` | [Completion](completed/16-window-and-jetpack.md); [feature](../features/backlog/fps-controls.md); Windows controls/window verified, final fuel consumed without frame-rate flicker |
| `17` | Official Unity CLI, Pipeline, and Codex integration | `done` | [Completion](completed/17-unity-cli.md); [setup](../../unity/readme.md); official beta/experimental releases required for this workflow |
| `18` | Direct CLI workflow and completed-task audit against official skills | `done` | [Completion](completed/18-cli-and-skill-audit.md); [workflow](../../unity/readme.md); all 54 checks passed; `05`-`07` production acceptance reopened |
| `19` | Revert asset pass, improve rendering, and repair Blender connection | `done` | [Completion](completed/19-presentation-rollback.md); [scope](../features/backlog/presentation-audio.md); [asset inventory](../asset-ledger.md) |
| `20` | Smooth excavation overhaul, six shovel strengths and practice controls | `done` | [Completion](completed/20-smooth-digging.md); [terrain/research](../features/backlog/excavation-terrain.md); [shovels](../features/backlog/shovel-progression.md); existing art retained |
| `21` | Repair review shortcuts and airborne jetpack restart; vary scoop shapes | `done` | [Completion](completed/21-review-controls-and-organic-digging.md); [digging contract](../features/backlog/excavation-terrain.md); [controls](../features/backlog/fps-controls.md) |
| `22` | Verify release builds exclude developer admin access | `planned` | User now prefers durable admin tools in one development executable; mandatory release gate; [contract](../features/backlog/excavation-terrain.md#task-22---production-release-gate) |
| `23` | Consolidate developer admin access, extend shovel reach and refine scoops | `done` | [Completion](completed/23-developer-admin-and-shovel-reach.md); [contract](../features/backlog/shovel-progression.md); one executable, rare shortcuts, release gating and depth variation |
| `24` | Rebalance shovel strength and cap digging reach at four metres | `done` | [Completion](completed/24-shovel-rebalance.md); [contract](../features/backlog/shovel-progression.md); smaller scoops and gentler strength progression |
| `25` | Separate shovel digging speed and strength into independent upgrades | `planned` | User-requested future TODO; [contract/open decision](../features/backlog/shovel-progression.md#task-25---independent-speed-and-strength-upgrades-future); consider removal timed to a shovel stroke |
| `26` | Remove detached dirt immediately after excavation | `done` | [Completion](completed/26-detached-soil-cleanup.md); [terrain contract](../features/backlog/excavation-terrain.md#task-26---detached-soil-cleanup); supported overhangs retained, collision and removal accounting synchronized |
| `27` | Populate buried finds, collection feedback, admin X-ray and friendlier early reach | `done` | [Completion](completed/27-buried-finds-and-xray.md); [discovery contract](../features/backlog/discovery-collection.md#task-27---buried-finds-and-admin-x-ray); user-requested simple shapes, final art remains `09` |
| `28` | Click visible small finds to collect and remove instructional HUD text | `done` | [Completion](completed/28-small-find-pickup-and-quiet-hud.md); [collection/UI contract](../features/backlog/discovery-collection.md#task-28---visible-small-finds-and-quiet-hud); retain exposure requirements only for large finds |
| `29` | Replace bowl-shaped scoops with irregular shovel bites and remove volume popups | `done` | [Completion](completed/29-irregular-shovel-bites.md); [terrain contract](../features/backlog/excavation-terrain.md#task-29---irregular-shovel-bites); preserve upgrade balance, collision and discovery collection |

Task `16` is complete. No task is currently `in_progress` or `ready`; remaining items retain their planned/deferred scope. `09` retains final art, `10` adds detector feedback, `25` remains a future TODO, and `22` must pass before production. The next new task ID is `30`.
