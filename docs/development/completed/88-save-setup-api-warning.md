# Task 88 - Save setup API warning

- Why: Unity 6000.6 reports CS0618 for the save setup command's ordered player lookup.
- Result: `SaveGameSetup.Configure` uses supported `FindAnyObjectByType<FpsPlayer>()`; no remaining `FindFirstObjectByType` calls exist in project C#.
- Evidence: official CLI recompilation completed without errors or C# warnings; read-only inspection found exactly one MainGame player and the matching lookup, with the scene clean. Reports: `unity/Logs/Task88/`.
- Scope: editor-only maintenance; the existing sprint Windows build remains current. Nothing committed.
