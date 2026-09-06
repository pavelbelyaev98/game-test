# Just a few peppers — Unity project

**Run `Builds/JustAFewPeppers/JustAFewPeppers.exe`** with its adjacent files to play the ordinary Windows build. Keep the whole directory together when copying it. Unity is optional for playtesting.

To use the editor, open this folder (`games/just-a-few-peppers/unity/`) in Unity Hub with the version pinned in `ProjectSettings/ProjectVersion.txt`, then open `Assets/JustAFewPeppers/Scenes/PepperYard.unity` and press Play. No Inspector assembly is needed. Paths on this page are relative to the Unity project.

## Play the food loop

The [1_06 physical-handling revision](../docs/development/tasks/1_06_loose-yard-objects-and-playful-handling.md#physical-handling-revision-delivery--september-6-2026) owns the current build, checklist and verification. The scene supports the complete food loop and four movable props beside the worktop. Normal play has no persistent controls list or instruction footer. **F1** opens all keys, debug tools and optional work help; F1/Esc closes it to the previous mode. Esc alone opens a compact pause menu with session-only sensitivity (0.25x to 2.50x, default 1.00x). Broader settings/options are already planned in 7_02 and 7_03.

Enter/click Walk starts; WASD/arrows move, hold either Shift to sprint, Space jumps, mouse looks, Esc pauses/resumes, and R returns to the gate. The pause menu supports mouse or Up/Down plus Enter/Space. Focus return stays paused until you resume. Release Space after using it in a menu before jumping; holding it does not repeat jumps. There is no stamina meter or sprint/landing camera effect.

Look at the crate near the gate and **right click** to grab it. **Hold left mouse** at the pepper pile to scoop; release stops. At the broad processor tray, **E tips** the accepted load and starts automatic processing. Jars accumulate, including partial loads.

**Right click again releases the object from your hand with physics**; **G** does the same. Moving releases retain a little carry motion. Aim near the ground to lower an object before releasing. **E** offers secondary careful placement on ground, worktops or broad stable supports; optional **Z/X** rotates. Placement indicators stay hidden, and only a rejected careful-placement attempt explains the obstruction. Regrab with right click. Dropping/toppling keeps carrier contents.

At the **receiving tray on the machine's right**, E collects ready jars into one reusable finished carrier, including partial loads. Release/set down your current object first; collecting does not automatically park it. Right click always releases what you are holding before a later click can grab another object. More output can accumulate while the finished carrier is away.

Carry finished food to the **Finished Food Handoff Rack** behind the machine and press E at the rack. Stored food increases, nearby jar groups fill, and the empty carrier returns to the receiving tray automatically. Placing beside the rack does not deposit. Use the loaded carrier before collecting again; no empty return trip is needed. All 107 peppers, including the last eleven-unit load, can reach storage.

With a **loose prop held**, **hold left mouse to charge a throw, then release**. A short hold gives a gentle toss; strength caps after 0.8 seconds. Food carriers use ordinary physical release. The basin/empty crate are open, the stool supports a small stack, and the ball can bounce or roll, including down the sloped wooden board beside the props. Careful placement remains available on E. Prop play grants no food or task credit.

**R** returns you to the gate and recovers held/lost props and carriers with all food/progress kept; valid supported placements stay where you left them. **F8 / Restart food test (clears stored food too)** restores the pepper pile and empties both carriers, station and stored food; valid prop arrangements remain. Pause/focus freezes motion and processing; release handling controls before acting again after resume. Disk saving, Coins/purchases and direct machine operation remain later tasks. The [queue](../docs/development/tasks/readme.md) owns current progress; use the [fresh-chat prompt](../docs/development/new-chat-prompt.md) to request NEXT.

## Build and verify

The editor menu **Just a few peppers > Build Windows playtest** builds the saved scene without regeneration. It disables Unity's Development flag. Development diagnostics have their own menu and `Builds/JustAFewPeppers-Development/` folder; they can still trigger a firewall prompt. Inbound network access is not needed for the offline gameplay.

Use the [verification guide](../docs/development/testing-and-performance.md#verified-foundation-commands) for the PowerShell test/build/smoke commands. Close this project's editor before batch runs. Checks write local evidence under ignored `Logs/`; `Builds/` is also local/ignored. The create command only works when PepperYard is missing and must not replace later authored work.

## Project files

`Assets/` contains source content. `.unity` files store scenes, `.mat` files store material settings, `.inputactions` stores controls, and `.asmdef` defines C# assemblies. Their `.meta` files hold stable IDs/import settings and belong in Git with the assets. `Library/` and `Temp/` are generated caches.

`Assets/Stage0/` is the discarded roasting/peeling experiment. Its generation/build is guarded while PepperYard exists; its legacy input is incompatible with the new Input System-only setup. Use the [historical audit](../docs/development/repository-audit.md) only when inspecting/reusing it. Find the full game's requirements in the [design index](../docs/readme.md).
