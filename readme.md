# Game interaction experiments

Each playable experiment has its own Unity project under `games/`. Add that individual project folder to Unity Hub.

| Project | Unity project folder | Instructions |
| --- | --- | --- |
| Just a few peppers — historical one-pepper Stage 0 | `games/chushkopek/` | [Run and controls](games/chushkopek/readme.md) · [Implementation and manual checks](docs/prototypes/chushkopek-stage0.md) |

Shared research stays in `Ideas/`, decision briefs and the comparison scorecard in `docs/prototypes/`, and reusable prompts in `instructions/`. Future experiments can live alongside Just a few peppers under `games/`.

[Just a few peppers — game design and cultural research](docs/games/chushkopek/readme.md) develops a proposed outdoor household project: clear the yard, process peppers, return jars, fill winter shelves, prepare family parcels, and reclaim the table under the vine. These documents describe a future direction; the playable project remains the one-pepper Stage 0.

Run the historical spike's state tests from this repository root:

```powershell
dotnet run --project games/chushkopek/tests/Stage0.StateTests/Stage0.StateTests.csproj
```
