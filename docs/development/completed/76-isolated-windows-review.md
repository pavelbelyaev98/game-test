# Task 76 — Windows Sandbox review cancelled

- Why: the user explicitly dropped isolation setup after Windows servicing problems and excessive delay. Task `76` is retired, not delivered as a working feature.
- Result: removed the installer, host/guest scripts, generated `.wsb` launch configuration and setup guide; cleared their queue/dependency references. Windows repair is not a game-development prerequisite.
- Evidence: the initial feature query reported component-store corruption; no native isolated run passed. Retained failure/preflight JSON is under `unity/Logs/WindowsSandbox/`. Cleanup received local-link, queue/state and stale-reference checks.
- Continuing workflow: simulated-input regression tests plus brief announced Windows reviews; retry observed input/focus interruptions. `75`'s Toolkit HUD and build remain delivered. Next ready task: `66`, precision movement design.
