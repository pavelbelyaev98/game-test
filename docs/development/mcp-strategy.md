# MCP strategy

## Rule

1. Use direct repo file edits for deterministic code/doc changes.
2. Use MCP when state is editor-dependent or scene-sensitive.

## Suggested usage

1. Use Unity MCP for:
1. scene graph checks,
1. console and player error diagnostics,
1. automated batch-safe editor actions.

2. Use Blender MCP for:
1. asset creation passes,
1. pose/mesh/lighting setup,
1. render validation and iteration.

3. Never allow blind scripted changes on critical gameplay scenes; route them through review.

## If MCP is blocked

If MCP is unavailable, errors, or setup is unclear:

- stop and ask for clarification before continuing,
- continue with direct file work where it is safe,
- and add a note in `docs/development/status.md` with the blocker details.
