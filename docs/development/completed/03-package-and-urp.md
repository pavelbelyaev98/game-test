# Task 03 - Package and URP baseline

Why: configure the installed Unity project to render through URP.

Integrated result: Unity `6000.6.0f1` uses `SomethingDownThereURP` / URP `17.6.0`; quality levels inherit the default asset. Pipeline is an editor-tooling dependency; Input System remains the input provider.

Task 18 audit: direct CLI package listing succeeded; live C# inspection confirmed the assigned/current render pipeline, six inheriting quality levels and all eight main-scene material shaders using URP Lit. Fresh repository checks passed: 28 EditMode and 26 PlayMode. [Audit evidence](../../../unity/Logs/Task18Audit/audit.md).

Limitation: correct pipeline/material wiring does not approve the existing generated art. Production presentation belongs to Task `08`. Use the live package commands/UPM Client API for future dependency changes.
