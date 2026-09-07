# Current status

## Milestone

The repository and FPS foundation are operational. Production excavation, discoveries, economy, progression, saving, and rescue are not implemented.

## Active and next work

- `M01` is `in_progress`: restore/refresh the compatible Unity package baseline and validate project integration.
- After `M01`, Feature/Task `01` is ready: define the first-playable scene and core-loop implementation slices.
- Manual review still needed: open `unity/Assets/Scenes/FpsValidation.unity` in Unity `6000.6.0f1` and assess controls, layout, and feel.

## Latest evidence

- FPS foundation compiled in Unity `6000.6.0f1`; 7 EditMode and 14 PlayMode integration checks passed.
- The validation scene covers movement, input gating, digging/interaction routing, battery/jetpack, inventory, menus, and primitive station adapters.
- Automated evidence is reproducible with `tools/test-fps.ps1`; generated logs remain local under ignored `unity/Logs/`.

## Limitations and blockers

- Headless checks do not establish visual quality, movement feel, or tutorial clarity.
- Validation adapters are not production terrain, discovery, selling, upgrades, or persistence.
- Unity MCP is not exposed in the current session; deterministic file and batch-editor work remains possible, but live-editor tasks may require setup or user input.
