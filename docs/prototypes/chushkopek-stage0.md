# Chushkopek Stage 0 — implementation and hands-on check

This is the narrower **one-pepper interaction spike**, not the 18-pepper prototype. There are no upgrades or additional gameplay systems. Its technical checks do not establish that the interaction is satisfying. Pavel's hands-on acceptance remains pending.

## Run

Use Unity **6000.6.0f1**, the version installed and recorded in this project. Open `games/chushkopek/` as the Unity project, open [ChushkopekStage0.unity](../../games/chushkopek/Assets/Stage0/Scenes/ChushkopekStage0.unity), and press Play. The existing Windows player is at `games/chushkopek/Builds/Stage0/ChushkopekStage0.exe`; keep its adjacent data and DLL files together.

Unless stated otherwise, asset, build, and log paths below are relative to `games/chushkopek/`. Shared research and prototype briefs remain outside the Unity project.

Controls: **WASD** move, **mouse** look, **left click** pick/place, **hold left mouse and drag** to peel, **right click** return a carried pepper to its previous position, **R** reset the cycle, **Esc** pause/resume. Sensitivity is adjustable in the pause panel. Losing focus pauses timers and sound; resume deliberately. There is no free drop.

## Files and state ownership

| Location | Responsibility |
| --- | --- |
| [PepperState.cs](../../games/chushkopek/Assets/Stage0/Scripts/PepperState.cs) | Plain C# state, location, roast/steam progress, two strip-progress values, retained burnt quality, single completion, reset. |
| [Stage0Session.cs](../../games/chushkopek/Assets/Stage0/Scripts/Stage0Session.cs) | Owns the one model, ticks it, handles requests, state-change cues, pause and reset. |
| [Stage0Interaction.cs](../../games/chushkopek/Assets/Stage0/Scripts/Stage0Interaction.cs), [Stage0Target.cs](../../games/chushkopek/Assets/Stage0/Scripts/Stage0Target.cs) | Stable first-person view, targeting, generous click volumes, mouse-driven peel, rejection explanations and small HUD. |
| [PepperPresentation.cs](../../games/chushkopek/Assets/Stage0/Scripts/PepperPresentation.cs), [PepperGeometry.cs](../../games/chushkopek/Assets/Stage0/Scripts/PepperGeometry.cs), [Pepper.shader](../../games/chushkopek/Assets/Stage0/Art/Pepper.shader) | Pepper shape, skin curl/release, loose edges, roast blistering/char and exposed flesh. |
| [RoasterStation.cs](../../games/chushkopek/Assets/Stage0/Scripts/RoasterStation.cs), [SteamingStation.cs](../../games/chushkopek/Assets/Stage0/Scripts/SteamingStation.cs), [FinishedSocket.cs](../../games/chushkopek/Assets/Stage0/Scripts/FinishedSocket.cs) | The single socket at each station and its local feedback. |
| [Stage0Audio.cs](../../games/chushkopek/Assets/Stage0/Scripts/Stage0Audio.cs) | Locally synthesized sizzle, pops, hiss, skin friction, release, placement and readiness/completion cues. |
| [Scene builder](../../games/chushkopek/Assets/Stage0/Editor/Stage0SceneBuilder.cs) | Recreates this specific scene and its generated meshes/materials; builds the Windows player. |
| [Scene probe](../../games/chushkopek/Assets/Stage0/Editor/Stage0SceneProbe.cs), [state tests](../../games/chushkopek/tests/Stage0.StateTests/Program.cs) | Editor-only scene smoke check and dependency-free automated state tests. |

The `Assets/Stage0/Scenes` scene, `Assets/Stage0/Art/Generated` assets, Unity `.meta` files, `Packages`, and `ProjectSettings` make this an openable project. Generated caches, logs, test outputs, and player builds are ignored by Git. No documentation bootstrap was run.

The model permits **Raw → Roasting → Perfect → Burnt** while heating, followed by **Steaming → Peelable → Peeled → Finished**. Perfect or Burnt can enter steaming. Location is explicit and separate from processing state; only Roaster ownership permits heating. Lifting immediately changes ownership to Held. Early removal preserves roast progress and permits returning to heat. Invalid placement leaves ownership intact. Burnt quality survives subsequent states. Completion requires Peeled plus a successful tray placement and can occur once per reset.

Provisional timings are **10 seconds to perfect, 7 seconds of grace, 3 seconds of steaming**. They can be changed on the scene's session component before Play. None is a runtime target.

## Peel representation and implementation findings

Two broad half-surface meshes cover a separate flesh mesh. A valid drag advances only the chosen strip, with reduced movement at the start to suggest resistance. Rows of the strip curl outward and flatten into a ribbon; completion gives a short release motion and sound. Releasing the button preserves progress. Time and a stationary held button never peel the pepper.

The spike uses the **visible half of the pepper as the generous selection region**, with a loose-edge marker, rather than requiring a hit on a tiny edge. The projected stem-to-tip direction determines the drag direction, so the HUD can say UP/DOWN/LEFT/RIGHT from the current view. Camera movement stays suspended through strip release until the mouse button is released.

Rendered inspection led to closing the mesh tips, moving the broad shoulder toward the stem, flattening the detached skin, correcting the cover's closing position, and keeping a held pepper out of the selection ray. These are implementation corrections, not evidence that the design hypothesis passes.

## Automated checks and limits

- **10/10 state tests passed.** They cover invalid Raw-to-Finished placement; stopped/resumed roasting; the Perfect/Burnt boundary; burnt processing; steam timing; input-only partial peeling; single completion; carry cancellation; reset; and invalid numeric input.
- **Scene smoke check passed after the final mesh fixes:** two cycles through actual scene components, selection rays, invalid placement recovery, stopped heat, idle partial-peel preservation, completion and reset. Eight state captures are in `Logs/Stage0Captures`; raw, partial-peel and finished captures were visually inspected. The probe also checks that the generated flesh mesh has a closed end.
- **Windows build succeeded** on Unity 6000.6.0f1. A ten-second hidden startup check using Direct3D 11 stayed running and logged no runtime errors or exceptions; the test process was then closed. This is startup coverage, not a manual playthrough. Build and startup logs are `Logs/stage0-build.log` and `Logs/stage0-player-smoke.log`.
- The smoke check calls action methods; it does **not** simulate a person's mouse gesture or judge sound quality, resistance, comfort, or desire to repeat.
- Unity 6000.6's editor search-index startup produced an `ArgumentOutOfRangeException` in the batch check. The probe records that exact editor-only stack separately. Other errors/exceptions still fail the check. This exception is not counted as a successful gameplay check or silently discarded.

Run the state tests from the repository root:

```powershell
dotnet run --project games/chushkopek/tests/Stage0.StateTests/Stage0.StateTests.csproj
```

Unity menu: **Stage 0 → Create or rebuild the one-pepper scene** regenerates the specific scene and generated art; **Stage 0 → Build Windows player** creates the player. Rebuilding intentionally replaces edits to that generated scene/art.

For the editor probe, close this project's editor first and run from the repository root:

```powershell
$stage0Project = (Resolve-Path 'games/chushkopek').Path
& 'C:\Program Files\Unity\Hub\Editor\6000.6.0f1\Editor\Unity.com' -batchmode -force-d3d11 -projectPath $stage0Project -executeMethod Chushkopek.Stage0.Editor.Stage0SceneProbe.Run -logFile (Join-Path $stage0Project 'Logs/stage0-playmode-probe.log')
```

The probe recreates the generated scene, enters Play mode, records checks/captures, and exits. Results are in `Logs/stage0-scene-probe-result.txt`. Do not use `-nographics` for rendered captures.

## Exact manual acceptance steps

1. Launch the Windows player, or open the scene and press Play. Turn toward the raw pepper on the left board and click it. It should move into the held position.
2. Aim at the chushkopek and click. Listen for insertion and sustained sizzle; watch the skin change. There is one active socket.
3. Near 10 seconds, identify readiness from increasing pops, blistered skin, the green indicator and ready cue. Lift during the forgiving window. Heating and further charring must stop immediately.
4. Click the steaming position. Watch the cover move and steam appear; listen for the hiss. After roughly three seconds the cover opens and loose-edge markers appear.
5. Aim at either broad half of the pepper. Hold left mouse and drag in the direction shown. Move slowly, stop while still holding, then release halfway. The skin must follow movement, stop when you stop, and retain partial progress after release.
6. Regrab and finish that strip. Continue moving while holding after the release: the camera must remain stable until you let go. Repeat on the other half. Judge whether stretch, curl, friction and release feel like peeling.
7. Click the fully peeled pepper, then click the finished tray. Confirm the transformed appearance and one completion cue. Repeated clicks must not generate another completion.
8. Press R. The pepper, both strips, timers, cover, particles, completion and starting view should reset. Repeat once with an early removal, an invalid tray placement, a right-click return, and an intentional burn past 17 seconds. Every case should remain recoverable; the burnt pepper must still steam, peel and finish. Also pause/Alt-Tab during heating and confirm it did not advance while paused.
9. Decide whether you **personally want to do the cycle again**, and which specific action creates that desire. Record problems with targeting, waits, resistance, release or sound. Completing the sequence mechanically is not a positive fun verdict.

## Known interaction weaknesses and brief implications

The broad strips are authored shapes, not physical skin; side views and large deformations may expose that approximation. The pepper has a fixed peeling pose, movement is constrained to its projected length, and the hand is represented by the held pepper/contact target. Returning a pepper uses a short guided movement. Sound is synthesized placeholder material feedback and still needs listening and tuning. With only one pepper, even the short roast includes observation time that cannot establish the larger batch rhythm.

The [larger Chushkopek brief](chushkopek-prototype.md) should retain its hypotheses and unresolved result. Carry forward broad selection, explicit Finished state and recovery rules if hands-on testing supports them. Do not adopt the spike's timings as final balance or add its upgrade/batch content until this one-pepper interaction earns that next experiment.
