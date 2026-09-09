# Task 82 - Implement brightness and reticle accessibility

Type: implementation. Status: `planned`; gated on `81`'s reviewed contract. Prerequisites: `81`, `69`, `79`.

Feature: [camera comfort](../../features/backlog/camera-comfort.md). [Queue](../tasks.md).

## Scope and research

- Implement `81`'s selected brightness and reticle choices in existing startup/Pause Settings, with immediate preview, reset, proper focus and keyboard/mouse operation.
- Extend user preferences without changing world saves or losing FOV/steady-crosshair/input settings. Apply before play, preserve across New Game/load/rescue/Continue and handle malformed values/write retry.
- Use the actual lighting/rendering pipeline and approved UI resources. Research official rendering guidance for the chosen adjustment; do not introduce unrelated art, postprocessing effects or camera motion.

## Acceptance and questions

- Inspect supported window sizes and low/default/high brightness underground and at the surface. Check empty-battery visibility, material/boundary distinction and HUD/reticle readability without altering reach or target rays.
- Exercise reset, startup-to-play, pause/focus/relaunch and both dig modes. Reticle choices honor the selected contract during digging and flight; preferences cannot cause input leakage.
- Deliver relevant regression evidence and the Windows build. Return unresolved behavior to `81`; numerical rendering implementation stays here. `54` covers sustained final-tool comfort.
