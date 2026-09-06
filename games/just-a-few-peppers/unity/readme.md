# Just a few peppers — Unity project

**Run `Builds/JustAFewPeppers/JustAFewPeppers.exe`** with its adjacent files to play the ordinary Windows build. Keep the whole directory together when copying it. Unity is optional for playtesting.

To use the editor, open this folder (`games/just-a-few-peppers/unity/`) in Unity Hub with the version pinned in `ProjectSettings/ProjectVersion.txt`, then open `Assets/JustAFewPeppers/Scenes/PepperYard.unity` and press Play. No Inspector assembly is needed. Paths on this page are relative to the Unity project.

## Play the crate prototype

The [1_02 quiet-placement handoff](../docs/development/tasks/1_02_scooping-and-crate-carrying.md#quiet-placement-delivery-record--september-6-2026) owns the current build, checklist and verification evidence. Automatic processing remains active; finished-output pickup is task 1_04.

Enter/click Walk starts; WASD/arrows move, hold either Shift to sprint, Space jumps, mouse looks, Esc pauses/resumes, and R returns to the gate. The pause menu supports mouse or Up/Down plus Enter/Space. Focus return stays paused until you resume. Release Space after using it in a menu before jumping; holding it does not repeat jumps. There is no stamina meter or sprint/landing camera effect.

Look at the crate near the gate and press **E** to grab it. **Hold left mouse** at the pepper pile to scoop; release stops. At the broad processor tray, **E tips** the accepted load and starts automatic processing. Jars accumulate, including partial loads.

Aim at nearby ground, the worktop or a broad stable support: **E places**, optional **Z/X rotates**, and **G drops**. There is no placement outline or continuous valid/blocked indicator; a rejected E attempt briefly explains why it cannot place there. Regrab it with E; placing, dropping and toppling keep its contents. The worktop and low support left of the opening route provide convenient examples, and other suitable geometry works too.

**R** returns you to the gate and recovers a held/lost crate with all food/progress kept; valid supported crate placements stay where you left them. **F8 / Restart processing test (clears food)** restores the pepper pile and empties crate/station. Pause/focus freezes motion and processing; release handling controls before acting again after resume.

Output collection and the rack handoff come in **1_04**. Until then, full output safely blocks more processing while the input buffer accepts remaining room; use F8 for a fresh test. The [queue](../docs/development/tasks/readme.md) owns current progress; use the [fresh-chat prompt](../docs/development/new-chat-prompt.md) to request NEXT.

## Build and verify

The editor menu **Just a few peppers > Build Windows playtest** builds the saved scene without regeneration. It disables Unity's Development flag. Development diagnostics have their own menu and `Builds/JustAFewPeppers-Development/` folder; they can still trigger a firewall prompt. Inbound network access is not needed for the offline gameplay.

Use the [verification guide](../docs/development/testing-and-performance.md#verified-foundation-commands) for the PowerShell test/build/smoke commands. Close this project's editor before batch runs. Checks write local evidence under ignored `Logs/`; `Builds/` is also local/ignored. The create command only works when PepperYard is missing and must not replace later authored work.

## Project files

`Assets/` contains source content. `.unity` files store scenes, `.mat` files store material settings, `.inputactions` stores controls, and `.asmdef` defines C# assemblies. Their `.meta` files hold stable IDs/import settings and belong in Git with the assets. `Library/` and `Temp/` are generated caches.

`Assets/Stage0/` is the discarded roasting/peeling experiment. Its generation/build is guarded while PepperYard exists; its legacy input is incompatible with the new Input System-only setup. Use the [historical audit](../docs/development/repository-audit.md) only when inspecting/reusing it. Find the full game's requirements in the [design index](../docs/readme.md).
