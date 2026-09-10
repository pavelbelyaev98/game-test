# Presentation and audio

Status: the user has requested [105 — art-style research and repeated texture trials](../../development/tasks/105-art-style-texture-trials.md). The broader production pass `08` remains deferred outside that bounded request; preserve user-owned ground. Task `19` is [complete](../../development/completed/19-presentation-rollback.md): the interrupted asset/audio pass is removed and existing rendering improved without new art.

Idea coverage: sections 42-45.

Design: [art style / 105](art-style.md) owns the tested shared palette, textures and asset language, iterating through multiple candidate packs until the user likes and selects a style. [57](../../development/tasks/57-presentation-design.md) consumes it for terrain handoff and starter/equipment/feedback briefs before `08`–`11`. Specific asset approval gates remain.

## Purpose

Establish a cohesive, readable production look and sound direction that starts grounded and can support increasingly strange discoveries. The main game must not resemble a primitive mechanics test.

Player-feel acceptance: contact/removal remains satisfying between finds, recognition survives fast collection, and returning clearly acknowledges banked progress without service chores. Visual cues distinguish real interactions and diggable resistance from decoration and permanent boundaries. `57` owns the concrete event-priority, station-payoff and affordance proposals; `08`/`09` integrate the selected result. Consult the [risk register](../../development/design-risks.md); no new delay, interaction or asset is selected by these expectations.

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

Avoid endless brown mud, excessive darkness, and generic procedural scenery. General free-to-use commercially licensed assets are acceptable; distinctive discoveries and the evolving shovel deserve custom Blender work. Brief gradual underground geological variation without level gates and material/tool-specific digging sounds early (`57`/`58`), for integration in `11`/`39`. `68` defines the [passive player light](underground-lighting.md) for `69`; `57` reviews whether any non-shovel equipment warrants visible milestone hardware, with no asset additions implied. Camera/tool feedback inherits [FPS comfort](fps-controls.md) from `64`/`65`; it must work with optional camera motion disabled. Later feature tasks must maintain this quality bar.

`57` additionally reviews physical installation of major shovel attachments, with `11` owning selected staging and interrupted-purchase recovery. `10` owns detector-only volume/mute. The [Meltopia lessons](../../research/meltopia-lessons.md) prioritize satisfying contact/removal/reveal feedback and pleasant repeated sound before adding mechanics; references do not approve any assets.
