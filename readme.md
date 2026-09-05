# Game interaction experiments

Each playable experiment has its own Unity project under `games/`. Add that individual project folder to Unity Hub.

| Project | Unity project folder | Instructions |
| --- | --- | --- |
| Chushkopek Season — one-pepper Stage 0 | `games/chushkopek/` | [Run and controls](games/chushkopek/readme.md) · [Implementation and manual checks](docs/prototypes/chushkopek-stage0.md) |

Shared research stays in `Ideas/`, decision briefs and the comparison scorecard in `docs/prototypes/`, and reusable prompts in `instructions/`. Future experiments can live alongside Chushkopek under `games/`.

Run the Chushkopek state tests from this repository root:

```powershell
dotnet run --project games/chushkopek/tests/Stage0.StateTests/Stage0.StateTests.csproj
```
