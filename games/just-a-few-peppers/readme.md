# Just a few peppers

**Help Grandpa prepare an unreasonable amount of winter food using increasingly absurd homemade machinery.** Physically handle batches, directly operate equipment, earn Coins at food handoff and choose useful improvements in one mostly accessible Bulgarian yard.

| Start here | Purpose |
| --- | --- |
| [Start developing](docs/development/start-here.md) | How AI builds while you playtest, one numbered task at a time. |
| [Numbered task queue](docs/development/tasks/readme.md) | Every task from 0_01 to 9_03, dependencies, progress, and feedback. |
| [Prompt for a fresh chat](docs/development/new-chat-prompt.md) | Recover context and implement NEXT or a specific task ID. |
| [Current design](docs/readme.md) | Authoritative current gameplay and scope. |
| [Development roadmap](docs/development/roadmap.md) | Ordered work from first playable loop to release. |
| [Implementation status](docs/development/status.md) | What exists and what evidence is still required. |
| [First task: 1_01](docs/development/tasks/1_01_unity-foundation-and-walkable-scene.md) | New Unity foundation and walkable scene. |
| [Combined M1 contract](docs/development/first-playable-task.md) | The physical food loop across 1_01–1_08, followed by earned equipment choices in M2. |
| [Unity practices and assets](docs/development/unity-and-assets.md) | Supported APIs, Input System, and free commercially usable assets first. |
| [Architecture](ARCHITECTURE.md) | Proposed ownership and Unity boundaries. |
| [Repository audit](docs/development/repository-audit.md) | Actual editor, packages, code, tests, and reuse limits. |
| [State and saving](docs/development/state-and-saving.md) | Conservation, snapshots, and recovery. |
| [Testing and performance](docs/development/testing-and-performance.md) | Existing commands and future gates. |
| [Unity project](unity/readme.md) | Project location and current prototype baseline. |
| [Bulgarian culture research](../../research/culture/Just_A_Few_Peppers_Bulgarian_Culture_and_Game_Direction.md) | The game's cultural source memo, kept with the other research. |
| [Shared research](../../research/readme.md) | Sources and earlier concepts. |

Open **`games/just-a-few-peppers/unity/`** in Unity Hub using **6000.6.0f1**. This parent directory groups documents and the project; it is not itself a Unity project.

Current delivery and player feedback live in the [task queue](docs/development/tasks/readme.md); [milestone status](docs/development/status.md) summarizes progress. Use the [Unity play guide](unity/readme.md) for the current artifact and controls.

The [current design](docs/readme.md) centers food, machinery and physical play. Stored winter food remains permanent progress; Coins are a separate equipment budget spent at one visible bench with two meaningful prototype choices. The handoff is generous, the yard mostly accessible, and the final food deposit leaves normal control available. Customers, sales management, parts hunts and chore systems are outside this baseline. Lyutenitsa/rakia remain explicit future activity decisions. See the [latest decision](docs/design-pivot.md#food-machinery-and-coins--september-6-2026).
