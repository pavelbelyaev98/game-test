# Theme and navigation

Current components: [design system](design-system.md), refined in `124`. Selected basis: the user's white typography and Continue-first examples (Schedule I and A Game About Digging a Hole, supplied 2026-09-10). Adapt their hierarchy to this game's implemented systems; reproduce no artwork, logo, font or screen texture.

| Direction | Result / reason |
|---|---|
| Previous charcoal / cream / brass | Replaced: warm yellow emphasis conflicts with the requested white menu language. |
| White text, dark translucent backing, flat navigation | Selected: legible over the existing world, strong title, restrained focus marker, generous row spacing. |
| White panels with dark text | Not selected: references show white typography over the scene; bright panels would dominate the excavation backdrop. |
| Teal workshop UI | Selected for sale/upgrade screens: same type, spacing and navigation, mint emphasis for balances and improvements; shared neutral actions. |

## Shared tokens

| Role | Value | Use |
|---|---|---|
| Text / menu accent | `#FFFFFF` | Titles, buttons, active tab, aiming cue, normal battery |
| Secondary text | `#BFBFBF` | Explanations, units and metadata |
| Ink / panel | `#181818` / `rgba(24,24,24,.97)` | Ink / panel backing |
| Raised / border | `#2B2B2B` / `#BFBFBF` | Rows and focus outlines |
| Shop emphasis | `#96DDCD` | Credit header and next values |
| Warning / destructive | `#FFCE80` / `#FFB3A7` | Existing HUD resource warnings; dialogs use neutral styles |
| Disabled text | `#707070` with reduced opacity | Unavailable actions, with visible reason nearby |

Typography uses the existing font: shared title 30, startup 54, controls 18, secondary text 15–16; 1280×720 design scale. Rows and tabs use shaded surfaces without dividers. Focus uses a subdued keyboard-only outline; initial selection and pointer hover never outline a whole row. No decorative animation competes with selection.

## Selected order and behavior

- Startup: **Continue → New Game → Settings → Quit**. Continue is styled identically to its siblings, with no initial painted highlight. It is disabled if no checkpoint exists; keyboard starts there when available, otherwise at New Game. New Game retains Cancel-first replacement confirmation.
- Pause: **Resume → Settings → Save and quit**; development tools are visually separated below. No save timestamp or automatic action highlight. No redundant second entry for Controls.
- Settings: **Display → Graphics → Audio → Controls → Accessibility** (`123`), with shared rows and a fixed frame. Back returns directly to the originating startup/Pause screen and focuses Settings. Category changes flush preferences. [Current options and display confirmation](common-settings.md). Matching Back/Reset buttons sit together at bottom-left. Settings apply immediately; display changes open a standalone dialog.
- Shops: balance first, content/next upgrade second, compact right-aligned close and transaction actions in the footer. Close receives safe initial focus. Current and next values occupy separate aligned columns; costs and shortfall remain explicit. Selecting a sale row sells immediately; its button names the sale and value.
- HUD: reuse white/neutral tokens, retain meaningful warning bands and hide under every menu except the optional FPS readout. Do not change return-warning semantics in this styling pass.

## Research and future suggestions

[Xbox XAG 114](https://learn.microsoft.com/en-us/xbox/accessibility/xbox-accessibility-guidelines/114) supports consistent tab/navigation conventions and understandable labels. [XAG 102](https://learn.microsoft.com/en-us/xbox/accessibility/xbox-accessibility-guidelines/102) specifies 4.5:1 for ordinary meaningful text, 3:1 for large/inactive content and clear contrast for controls. These inform the treatment; this pass does not claim full accessibility certification.

[The Last of Us Part II's official accessibility overview](https://www.playstation.com/en-us/games/the-last-of-us-part-ii/accessibility/) groups related options and offers hold/toggle alternatives. Preserve this game's existing Hold/Toggle and bindings, accessible before starting a world.

**Selected in the follow-up request (`122`):** Display owns resolution/window mode/VSync/FPS; Graphics now contains only TBD (`124`); Audio contains Master volume only; mouse sensitivity belongs with Controls, existing visual comfort with Accessibility. [XAG 112](https://learn.microsoft.com/en-us/xbox/accessibility/xbox-accessibility-guidelines/112) informs grouping and pre-game access. Graphics controls are deferred for review; nonexistent effects and submixes remain excluded.

Credits should sit between Settings and Quit when a player-facing credits/attribution screen exists. Audio mixers, a menu backdrop camera and additional accessibility options remain future work. `81`/`82` retain brightness/reticle design; `57` retains broader feedback policy; `118` retains the action bar. The user's follow-up also removes routine descriptions and slogans; loss/conflict/error prompts remain concise.
