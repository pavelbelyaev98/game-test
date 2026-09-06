# Just a few peppers

**One outdoor Bulgarian yard, an unreasonable harvest, and Grandpa's increasingly ridiculous equipment.** Gather, dump, uncover upgrades, and store winter food until the day ends with a family meal.

| Start here | Purpose |
| --- | --- |
| [Start developing](docs/development/start-here.md) | How AI builds while you playtest, one numbered task at a time. |
| [Numbered task queue](docs/development/tasks/readme.md) | Every task from 0_01 to 9_03, dependencies, progress, and feedback. |
| [Prompt for a fresh chat](docs/development/new-chat-prompt.md) | Recover context and implement NEXT or a specific task ID. |
| [Current design](docs/readme.md) | Authoritative v4 gameplay and scope. |
| [Development roadmap](docs/development/roadmap.md) | Ordered work from first playable loop to release. |
| [Implementation status](docs/development/status.md) | What exists and what evidence is still required. |
| [First task: 1_01](docs/development/tasks/1_01_unity-foundation-and-walkable-scene.md) | New Unity foundation and walkable scene. |
| [Combined M1 contract](docs/development/first-playable-task.md) | The complete crate loop delivered across tasks 1_01–1_05. |
| [Unity practices and assets](docs/development/unity-and-assets.md) | Supported APIs, Input System, and free commercially usable assets first. |
| [Architecture](ARCHITECTURE.md) | Proposed ownership and Unity boundaries. |
| [Repository audit](docs/development/repository-audit.md) | Actual editor, packages, code, tests, and reuse limits. |
| [State and saving](docs/development/state-and-saving.md) | Conservation, snapshots, and recovery. |
| [Testing and performance](docs/development/testing-and-performance.md) | Existing commands and future gates. |
| [Unity project](unity/readme.md) | Project location and current prototype baseline. |
| [Bulgarian culture research](../../research/culture/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md) | The game's cultural source memo, kept with the other research. |
| [Shared research](../../research/readme.md) | Sources and earlier concepts. |

Open **`games/just-a-few-peppers/unity/`** in Unity Hub using **6000.6.0f1**. This parent directory groups documents and the project; it is not itself a Unity project.

Current stage: task 1_01 is Ready for Pavel with a walkable outdoor foundation, Input System controls, pause/reset, and a Windows development build. See the [playable handoff](docs/development/tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026). Unity also retains the earlier roasting/peeling spike. V4's pile gathering, automatic bulk processing, upgrades, saving, and full yard remain unimplemented.

The first version uses one pepper class, one product, one station, and one storage rack. Culture stays in scenery, progress-driven displays, and the meal. Extra household tasks and recipe branches are excluded.
