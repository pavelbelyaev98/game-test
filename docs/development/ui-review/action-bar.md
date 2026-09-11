# Bottom action-bar proposal

Owner: [118](../tasks/118-bottom-action-bar.md). State: **proposed, not implemented or asset-approved**. The user selected white menu/settings typography; the icon itself still needs a concrete preview and approval under `AGENTS.md`.

Place one compact backpack control cue at the **bottom center**, 24 reference pixels above the edge. Reserve a 40×40 icon well followed by a high-contrast current-key label. Total height: 48 reference pixels. Use the shared charcoal backing and white foreground; 3-pixel corners match the HUD.

| Case | Proposed appearance / behavior |
|---|---|
| Empty / partial / full bag | Same backpack and binding; existing top-left capacity remains the sole count. |
| Default binding | Backpack icon + **Tab** |
| Remapped keyboard | Backpack icon + actual display text, e.g. **Numpad Enter** |
| Remapped mouse | Backpack icon + **Middle mouse**; allow width growth up to 240 reference pixels. |
| Open menu / startup | Entire cue hidden with the existing HUD. |
| Target / feedback | Target stays below the reticle; feedback bottom 78 clears the 48-high cue at bottom 24. Check multi-line feedback before final placement. |

The cue opens the inventory through the existing binding; it is non-picking and introduces no new input handler. No decorative empty slots and no C4 entry before the mechanic exists.

Icon brief for the later approval: recognizable closed backpack silhouette, white monochrome, no interior loot/detail, produced through Blender MCP or a specifically approved commercial-use free source. Keep its isolated source/export/import ownership in the asset ledger only after the concrete proposal is approved. A text box labeled “backpack” is not the final icon.

Compare bottom-center against bottom-left during `118`'s actual icon preview. Center is recommended because resources already occupy the upper left and the held find appears lower right; confirm with held-rock, long-key and multi-line feedback captures at 960×540 and 16:10. No new action deserves space solely to fill the row.
