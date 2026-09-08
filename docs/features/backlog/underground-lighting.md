# Passive underground lighting

Status: not implemented. [68 - design](../../development/tasks/68-underground-lighting-design.md) defines lighting before equipment/presentation planning; [69 - integration](../../development/tasks/69-underground-lighting.md) follows approved production presentation.

Idea coverage: sections 31 and 44; equipment structure in section 11.

## Decision and rationale

Provide dependable passive headlamp/tool lighting in underground passages. Excavation visibility must not require switching tools, placing lamps or purchasing basic usability. Darker geological atmosphere is compatible with readable terrain and finds; darkness is not an added survival pressure.

The current scene has no dedicated player light. Mounting, beam/shadow policy, exact brightness/range and any later light upgrades are open decisions in `68`, guided by [comfort findings](../../research/player-review-findings.md#physical-comfort).

## Required behaviour and exclusions

- The baseline works automatically underground, including with an empty battery. Preserve the rule that only accepted digging and active thrust consume energy.
- Keep player-made routes, material/boundary distinctions and exposed finds readable; avoid glare, washed-out materials, through-soil treasure reveal and excessive render cost.
- No placeable-light chores, lamp platforms, fuel consumables, darkness hazard or new purchase track is implied. Markers remain a separate conditional navigation question.
- Respect FOV/low-motion and any selected precision stance. Support enclosed lateral tunnels as well as shafts.
- Visible equipment and material/audio additions require explicit approval through the existing art rules; this feature does not authorize them.

## Decisions to retain

`68` records the selected mounting/presentation, minimum visibility, surface transition, performance limits and fixed-versus-upgradable choice with rationale. `56` consumes any selected purchase structure and `57` the asset brief; implementation evidence belongs to `69` and mixed-material/release reviews.
