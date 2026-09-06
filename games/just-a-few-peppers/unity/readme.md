# Just a few peppers — Unity project

Open this folder (`games/just-a-few-peppers/unity`) in Unity Hub with the editor version recorded in `ProjectSettings/ProjectVersion.txt`, currently **6000.6.0f1**. Paths below are relative to this Unity project folder.

**Play the task 1_01 foundation:** open `Assets/JustAFewPeppers/Scenes/PepperYard.unity` and press Play, or launch `Builds/JustAFewPeppers/JustAFewPeppers.exe` with its adjacent build files. Enter/click Walk starts; WASD/arrows walk, mouse looks, Esc pauses/resumes, and R returns to the gate. The pause menu supports mouse or Up/Down plus Enter. Focus return stays paused until you resume.

The scene includes mound, crate, processor, and rack placeholders with targeting feedback. Pepper handling is not present yet. [Task 1_01's delivery record](../docs/development/tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026) contains the play checklist and verification evidence. Use the [fresh-chat prompt](../docs/development/new-chat-prompt.md) with NEXT to continue at **1_02**; 1_02–1_05 add the complete crate loop. See the [numbered queue](../docs/development/tasks/readme.md).

The [current design](../docs/readme.md) is bulk pepper clearing, useful equipment discoveries, one automatic processing line, one finished product, and one storage rack. Winter food and parcels appear through progress-driven presentation, followed by the meal ending. That gameplay remains specified; only the walkable foundation is implemented.

The existing `Assets/Stage0/Scenes/ChushkopekStage0.unity` is an older one-pepper roasting/peeling experiment. Its code and generated assets are disposable reference material; no further hands-on acceptance or routine test reruns are required for it. Historical Chushkopek identifiers remain until implementation replaces or reuses the relevant pieces.

Use the [roadmap](../docs/development/roadmap.md), [status](../docs/development/status.md), historical [audit](../docs/development/repository-audit.md), [Unity and asset policy](../docs/development/unity-and-assets.md), and [verification rules](../docs/development/testing-and-performance.md) for new work. Stage0 generation/build now refuses to modify this project while PepperYard exists. Its legacy gameplay is not compatible with the new Input System-only setting.

The `Just a few peppers` editor menu builds the saved scene without regenerating it. The create command only works when the scene is missing. Reproducible checks from this project directory:

```powershell
./tools/Verify-Foundation.ps1 -Mode EditMode
./tools/Verify-Foundation.ps1 -Mode PlayMode
./tools/Verify-Foundation.ps1 -Mode Build
./tools/Verify-Foundation.ps1 -Mode Smoke
```

The wrapper uses the pinned Hub editor, writes evidence under ignored `Logs/`, and verifies fresh test/smoke results. `Smoke` launches the development player hidden with synthetic input; normal launches have no automation. `Builds/` is also local/ignored. Keep the entire `Builds/JustAFewPeppers/` directory together when playing or copying it.
