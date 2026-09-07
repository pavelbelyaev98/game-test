# Architecture note (current foundation)

## Working model

1. `unity/Assets/Runtime`:
   - gameplay scripts, systems, and scene-owned runtime logic.
2. `unity/Assets/Editor`:
   - editor helpers/tools only.
3. `unity/Assets/Tests`:
   - unit/edit/play tests.
4. `unity/Packages/manifest.json`:
   - authoritative package + dependency baseline.
5. `docs/`:
   - rules, specs, queue, task state, and feature backlog.

## Initial constraints

1. First-person control and camera assumptions are declared in `docs/features` and task docs before any C# implementation.
2. Scene wiring is documented before major scene refactors.
3. Asset provenance is always recorded in `docs/`.
4. No gameplay system is considered implemented until architecture + docs contract + checks are updated together.
