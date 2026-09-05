# Just a few peppers — historical one-pepper interaction spike

Open this folder (`games/chushkopek`) with **Unity 6000.6.0f1**, then open `Assets/Stage0/Scenes/ChushkopekStage0.unity` and press Play. Paths on this page are relative to this Unity project folder.

One pepper, one roaster socket, one steam/peel position, one tray position. This is Stage 0; the core interaction still needs hands-on evaluation.

[Game design, authentic process, story, and progression](../../docs/games/chushkopek/readme.md) now proposes getting Grandpa's household ready for winter through bulk clearing, equipment discoveries, automatic processing, returned jars, family parcels, and a final meal under the vine. That direction is not implemented in this one-pepper spike; its engineering checks do not validate the new gameplay loop. The current game title is **Just a few peppers**; `chushkopek` remains the existing project folder and historical asset identifier.

- **WASD / mouse:** move / look.
- **Left click:** pick up or place. For peeling, hold a broad skin region and drag in the direction shown.
- **Right click:** return a held pepper. **R:** reset. **Esc:** pause and mouse sensitivity.

[Implementation, test results and exact manual acceptance steps](../../docs/prototypes/chushkopek-stage0.md).

State tests: `dotnet run --project tests/Stage0.StateTests/Stage0.StateTests.csproj`.

The Unity menu **Stage 0 → Build Windows player** writes `Builds/Stage0/ChushkopekStage0.exe`.
