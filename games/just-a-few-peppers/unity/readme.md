# Just a few peppers — Unity project

**Run `Builds/JustAFewPeppers/JustAFewPeppers.exe`** with its adjacent files to play the ordinary Windows build. Keep the whole directory together when copying it. Unity is optional for playtesting.

To use the editor, open this folder (`games/just-a-few-peppers/unity/`) in Unity Hub with the version pinned in `ProjectSettings/ProjectVersion.txt`, then open `Assets/JustAFewPeppers/Scenes/PepperYard.unity` and press Play. No Inspector assembly is needed. Paths on this page are relative to the Unity project.

## Play the crate prototype

**Current build limitation:** the executable still uses two crate mats and automatic processing, with no finished-output pickup. The [queue](../docs/development/tasks/readme.md) first resumes the 1_02 placement correction, then 1_04 handoff; new 1_07/1_08 test physical material and direct machine operation, and 2_01 implements Coins/two purchases. The [revised design](../docs/design-pivot.md#food-machinery-and-coins--september-6-2026) is planned behavior; this documentation pass did not rebuild the game. “Mound” in the current HUD means pepper pile.

Enter/click Walk starts; WASD/arrows move, hold either Shift to sprint, Space jumps, mouse looks, Esc pauses/resumes, and R returns to the gate. The pause menu supports mouse or Up/Down plus Enter/Space. Focus return stays paused until you resume. Release Space after using it in a menu before jumping; holding it does not repeat jumps. There is no stamina meter or sprint/landing camera effect.

Look at the crate near the gate and press E to pick it up. Hold left mouse at the mound and sweep between clumps; release to stop scooping. Bring the loaded crate to the broad tray on the processor and press **E to tip**. The line runs automatically and jars accumulate; a partial load works too. E beside a marked mat parks the crate. R returns you and the crate to the gate **with the load, depletion, and all station food/progress kept**. F8 or **Restart processing test (clears food)** in the menu restores the mound and empties crate and station. Pause/focus loss freezes processing and stops handling; release the controls before starting another action after resuming.

Output collection and the rack handoff come in 1_04. At present the line safely stops when output is full, accepts any remaining input room, and keeps unaccepted food in your crate; use F8 to start another test. See the [1_03 handoff](../docs/development/tasks/1_03_tipping-and-automatic-processing.md#delivery-record--september-6-2026) for the play checklist and limitations, the [queue](../docs/development/tasks/readme.md) for current progress, and the [fresh-chat prompt](../docs/development/new-chat-prompt.md) to request NEXT.

## Build and verify

The editor menu **Just a few peppers > Build Windows playtest** builds the saved scene without regeneration. It disables Unity's Development flag. Development diagnostics have their own menu and `Builds/JustAFewPeppers-Development/` folder; they can still trigger a firewall prompt. Inbound network access is not needed for the offline gameplay.

Use the [verification guide](../docs/development/testing-and-performance.md#verified-foundation-commands) for the PowerShell test/build/smoke commands. Close this project's editor before batch runs. Checks write local evidence under ignored `Logs/`; `Builds/` is also local/ignored. The create command only works when PepperYard is missing and must not replace later authored work.

## Project files

`Assets/` contains source content. `.unity` files store scenes, `.mat` files store material settings, `.inputactions` stores controls, and `.asmdef` defines C# assemblies. Their `.meta` files hold stable IDs/import settings and belong in Git with the assets. `Library/` and `Temp/` are generated caches.

`Assets/Stage0/` is the discarded roasting/peeling experiment. Its generation/build is guarded while PepperYard exists; its legacy input is incompatible with the new Input System-only setup. Use the [historical audit](../docs/development/repository-audit.md) only when inspecting/reusing it. Find the full game's requirements in the [design index](../docs/readme.md).
