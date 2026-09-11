# Common PC settings

[124](../tasks/124-monochrome-control-states.md) applies the latest corrections through [shared components](design-system.md). [122](../tasks/122-common-pc-settings.md) introduced the settings; [PC settings](../../features/backlog/pc-settings.md) owns behavior. 12 settings plus 12 bindings use the same components; package versions are unchanged.

| Category | Available options | Defaults / rules |
|---|---|---|
| Display | Window mode; resolution; VSync; FPS limit; Show FPS | Native-resolution Borderless by default; saved choices retained; Borderless/Fullscreen/Windowed. Resolution choices from monitor modes plus common window sizes. VSync Off; cap 144, inactive when VSync is enabled; FPS readout Off. |
| Graphics | TBD | No controls until individual review. Internal maximum defaults remain; saved graphics values are preserved. |
| Audio | Master volume only | 100%. Applies to the audio listener. Separate music/SFX groups await actual approved audio and routing. |
| Controls | Digging mode; mouse sensitivity; horizontal/vertical inversion; all 12 bindings | Hold; 1.00× (0.10–3.00×); both Off. All controls, including digging mode, scroll in one aligned list. |
| Accessibility | Vertical FOV; steady crosshair | 75° (55–90°); On. Other reticle/brightness choices remain with `81`/`82`. |

FPS caps: **30, 45, 60, 90, 120, 144, 165, 240, Unlimited**. The disabled cap reads right-aligned **Automatic**, with no dropdown arrow, while synchronized; disabling VSync restores the chosen cap. The FPS readout updates twice per second and remains visible over settings.

Built-in Unity dropdowns, switches and sliders share one field column. Every setting takes effect on change. There are no Apply buttons. Mode/resolution changes open a standalone **Keep display changes?** dialog with no tabs, Revert focused and a 15-second countdown. Escape, timeout and focus loss revert; only Keep saves the actual result. Display reset uses the same confirmation for native-borderless defaults. Back is a bottom-left text button; Reset category uses the same compact style and sits beside Back.

Normal screens use titles, labels, values and short action names. Startup/Pause/settings/shop slogans, routine explanatory paragraphs and the redundant digging-mode description are removed. New-game/reset/recovery screens retain concise loss information; rebinding names the affected action and gives a short prompt. Error text is **Not saved. Active for this session.** with **Retry**.


Rendering verification: changing 3D resolution from 100% to 50% changed the actual camera color target from 1280×720 to 640×360; MSAA changed from 4 samples to 1. [50% / AA off](../../../unity/Logs/Task123/render-output-50.json), [100% / 4×](../../../unity/Logs/Task123/render-output-100.json). These are live render targets, not preference values. Those controls are now hidden behind Graphics TBD pending user review.

## Current review views

| Page | Small window | Wide window |
|---|---|---|
| Display | [960×540](../../../unity/Logs/Task124/display-960x540.png) | [1920×1080](../../../unity/Logs/Task124/display-1920x1080.png) |
| Graphics | [960×540](../../../unity/Logs/Task124/graphics-960x540.png) | [1920×1080](../../../unity/Logs/Task124/graphics-1920x1080.png) |
| Audio | [960×540](../../../unity/Logs/Task124/audio-960x540.png) | [1920×1080](../../../unity/Logs/Task124/audio-1920x1080.png) |
| Controls | [960×540](../../../unity/Logs/Task124/controls-960x540.png) | [1920×1080](../../../unity/Logs/Task124/controls-1920x1080.png) |
| Accessibility | [960×540](../../../unity/Logs/Task124/accessibility-960x540.png) | [1920×1080](../../../unity/Logs/Task124/accessibility-1920x1080.png) |

[Dropdown hover](../../../unity/Logs/Task124/native-dropdown-hover.png), [Automatic](../../../unity/Logs/Task124/automatic.png), [pressed slider](../../../unity/Logs/Task124/native-slider-pressed.png), [new-game dialog](../../../unity/Logs/Task124/new-confirm.png), [development save error](../../../unity/Logs/Task124/save-write-error.png), [last bindings](../../../unity/Logs/Task124/controls-long.png), [startup hover](../../../unity/Logs/Task124/native-continue-hover.png). CLI fixtures use an isolated profile. Developer admin is an ordinary Pause button, hidden in release; production write failures quit silently. No Open save folder action.

Validation: **158 EditMode + 49 relevant PlayMode passed** (25 UI, 12 save, 8 startup, 4 station). [Contrast measurements](../../../unity/Logs/Task124/contrast-report.json): 125 enabled text samples at least 7.69:1; measured slider boundaries at least 3.69:1. Normal/hover/pressed/focused combinations and slider endpoints have regression checks. [Native pointer review](../../../unity/Logs/Task124/native-final-results.json) and [build report](../../../unity/Logs/Task124/build-report.json). Save/preferences preserved. Broader `106` verdicts and hardware qualification remain separate.
