# Presentation and audio

Status: accepted Sunny r8 ground/daylight ([105](../../development/completed/105-art-style-texture-trials.md)), tall natural moving grass ([129](../../development/completed/129-grass-shape-and-wind.md)), the approved sun ([128](../../development/completed/128-visible-sun-and-grass-density.md)) and quick pickup ([133](../../development/completed/133-grass-performance-and-pickup-feel.md)) remain. [135](../../development/tasks/135-restore-grassy-site.md) restores continuous grass while retaining optimized rendering, and removes clouds, trees, river/water and decorative ground rocks. The prior cloud/clearing compositions are retired. The user rejects 130's complete reservoir batch; [137](../../development/tasks/137-reservoir-first-section-review.md) now prepares one detailed reference-led section for review before integration or further construction. Broader production work remains with `08`.

Idea coverage: sections 42-45.

Design: [art style / 105](art-style.md) owns the accepted shared palette, textures and asset language. [57](../../development/tasks/57-presentation-design.md) consumes it for the remaining production briefs before `08`–`11`; specific future asset approvals remain required.

## Purpose

Establish a cohesive, readable production look that starts grounded and supports increasingly strange discoveries. **Current user instruction: do not add sounds.** Audio scope and sound-specific acceptance are deferred until the user explicitly changes that direction; silent visual feedback must be sufficient.

Player-feel acceptance: contact/removal remains satisfying between finds, recognition survives fast collection, and returning clearly acknowledges banked progress without service chores. Visual cues distinguish real interactions and diggable resistance from decoration and permanent boundaries. `57` owns the concrete event-priority, station-payoff and affordance proposals; `08`/`09` integrate the selected result. Consult the [risk register](../../development/design-risks.md); no new delay, interaction or asset is selected by these expectations.

The [distinctiveness feedback](../../research/excavation-distinctiveness.md) and [Keep Digging synthesis](../../research/keep-digging-lessons.md) inform readable tool/terrain/discovery presentation. The user withdraws the four-mode gun package as game content: `154` restores ordinary Scoop, retains the [silent trial](excavation-modes.md) behind admin opt-in and removes all four sounds. `56`/`120` must select meaningful progression before `57`/`11` select its production silhouette; `153` must not market the experimental gun as accepted gameplay.

## Task 19 - completed cleanup

- Remove all assistant-added models, textures, fonts, UI sprites, audio, notices and their runtime/scene integrations; also remove the user's TextMeshPro reimports following their explicit follow-up.
- Record the complete owned paths and shared-file rollback in the asset ledger. Add no scenery, props or sounds merely to decorate the terrain.
- Disable poor-quality cast shadows, improve antialiasing/color rendering and existing HUD readability, and eliminate overlapping rim/wall faces.
- Verify the official Blender Lab MCP bridge against the running Blender instance without creating or changing art.
- Acceptance: restored scene has no added presentation/audio references; rim boundaries meet without overlap; relevant checks pass; updated Windows build and concise rollback evidence are available.

## Task 08 - future production foundation (requires a new scoped request)

See [numbered Task `08`](../../development/tasks/08-production-presentation.md) for scope, research, questions and acceptance.

## Acceptance

See [numbered Task `08`](../../development/tasks/08-production-presentation.md) for scope, research, questions and acceptance.

## Ongoing direction

The user separately requested UI Toolkit research and attractive menus in `74`, followed by the HUD migration in `75`; both are implemented. [Menu presentation](menu-presentation.md) owns the shared authoring/theme scope, which `08` will refine. This does not reopen terrain, world art or audio production in `08`.

User direction from the Task `63` review: aim for modest graphics complexity roughly like the digging-a-hole game reference, with practical technical choices delegated to the assistant. Use the [selected performance targets](release-validation.md#release-budgets) when `57` develops the actual brief. This establishes visual ambition, not exact art direction, comparator hardware requirements or approval of an asset batch; the existing terrain ownership and separate art request remain.

Avoid endless brown mud, excessive darkness, and generic procedural scenery. General free-to-use commercially licensed assets are acceptable; distinctive discoveries and the evolving shovel deserve custom Blender work. Brief gradual underground geological variation without level gates through `57`/`58`; material/tool sounds remain deferred under the no-sounds instruction. `68` defines the [passive player light](underground-lighting.md) for `69`; `57` reviews whether any non-shovel equipment warrants visible milestone hardware, with no asset additions implied. Camera/tool feedback inherits [FPS comfort](fps-controls.md) from `64`/`65`; it must work with optional camera motion disabled. Later feature tasks must maintain this quality bar.

`57` additionally reviews physical installation of major shovel attachments, with `11` owning selected staging and interrupted-purchase recovery. `10` owns reviewed silent detector feedback; sound and its volume/mute control are deferred. The [Meltopia lessons](../../research/meltopia-lessons.md) inform satisfying contact/removal/reveal feedback; their sound recommendations are deferred under the current user instruction.
