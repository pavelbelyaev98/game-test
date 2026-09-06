# Repository guidance

## Purpose and navigation

This repository groups small-game research and Unity experiments. **Just a few peppers** uses the current production scope: one small outdoor Bulgarian yard, finite peppers, crate then wheelbarrow, one automatic line with three tiers, one reusable finished carrier, one Finished Food Handoff Rack, and an automatic meal ending once all harvest is cleared and stored. Household distribution is presentation-driven by one stored-food total. The old Stage0 roasting spike is disposable reference material.

- Start implementation at the [numbered queue](games/just-a-few-peppers/docs/development/tasks/readme.md); it owns task selection and the reading route. Use the [new-chat prompt](games/just-a-few-peppers/docs/development/new-chat-prompt.md) to resume.
- Find every feature through the [design index](games/just-a-few-peppers/docs/readme.md), [scope contract](games/just-a-few-peppers/docs/scope-and-validation.md#scope-contract), and [roadmap](games/just-a-few-peppers/docs/development/roadmap.md).
- [Start here](games/just-a-few-peppers/docs/development/start-here.md) is the human developer workflow guide; [Unity project guide](games/just-a-few-peppers/unity/readme.md) explains how to play.
- Technical references: [architecture](games/just-a-few-peppers/ARCHITECTURE.md), [state/saving](games/just-a-few-peppers/docs/development/state-and-saving.md), [Unity/assets](games/just-a-few-peppers/docs/development/unity-and-assets.md), and [verification](games/just-a-few-peppers/docs/development/testing-and-performance.md). Read relevant sections as routed by the queue.
- [Research](research/readme.md) supplies evidence and ideas, not additional requirements. The older bootstrap in `instructions/` is optional process reference.

Unity root: `games/just-a-few-peppers/unity/`. Game-document paths starting with `Assets/` are relative to it. Future games belong under `games/<name>/`.

## Working rules

- Follow the current user request and current scope. Planning/process work does not select a gameplay task. For implementation, select the requested ID or NEXT under the queue rules; deliver one task unless a larger range is requested. Resume recorded partial work and supplied feedback first.
- Keep active dialogue and design drafts in English; eventual localization/native review does not add a localization task now. Five yard pockets is a ceiling, chosen after prototype measurement. The final processor improves loaded travel through authored layout as well as buffer/output capacity, using the same wheelbarrow and controls. Keep the M1–M2 interaction prototype within its explicit boundary.
- Inspect actual source/scenes/packages and preserve user changes. Existing files and old test passes do not prove current behavior.
- Before Unity API/package decisions, read pinned editor/package versions and consult matching official Unity documentation. Use supported APIs and compatible stable packages; fix new deprecation warnings. Do not automatically upgrade the editor.
- Use the Input System for gameplay. Stage0's legacy Input Manager use is an audit fact, not a pattern to extend.
- Prefer free assets licensed for commercial use; source and integrate suitable packs before creating ordinary production assets. Primitives are appropriate early. Follow the asset policy for actual imports/licenses and distinctive custom work; no unrequested purchases.
- Use simple C#, composition, explicit references, and clear ownership. Presentation physics cannot own required progress. Separate authored configuration from mutable state; ScriptableObjects are not save state. Avoid hidden globals and frameworks.
- Do not add multiplayer, ECS, economies, recipes, sorting, NPC schedules, household tasks, or other excluded features. Earlier proposals/research do not restore removed scope.
- Preserve Unity `.meta` files with their assets. Prefer editor authoring to fragile scene YAML edits. Never regenerate new authored work with Stage0's builder. Reuse or retire Stage0 only after checking retained references; preserving its old gameplay/checks is not a scope prerequisite.
- Own scene/prefab wiring, assets, input, UI, and build configuration. The human developer and playtester should not be expected to do routine Inspector assembly. Default handoffs use ordinary Windows playtest builds; keep development diagnostics in a separate output folder.
- Maintain feature discoverability: preserve specifications, task IDs, and links when simplifying documents. Trim duplicate process prose, not feature requirements. Add a focused note/ADR only when useful; no empty template trees.

## Definition of done and records

A feature needs documented behavior, relevant automated checks, an integrated scene, visible feedback, and recovery/save verification where applicable. Compilation alone is insufficient. Verify a packaged player when packaged behavior changes or the task requires it. Use relevant checks once; repeat only after a change, failure, or unresolved concern. Documentation-only edits need documentation checks, not Unity or Stage0 runs.

Keep technical readiness separate from human feedback. Never invent passing tests, player acceptance, or fun. Partial work stays unchecked. Follow the queue's explicit human review gates; an ordinary NEXT request does not provide gate evidence. Handoff includes exact scene/build, controls, a short play checklist, checks, limitations, and next task; stop there.

Record each fact once: the **queue** owns task delivery/feedback, each **task's delivery record** owns execution evidence, and [status.md](games/just-a-few-peppers/docs/development/status.md) owns milestone summaries/history. Update a milestone summary when its aggregate state or blocker changes; entry pages link to these sources instead of copying progress. Update behavior contracts when behavior changes, plus relevant asset/regression records. Publishing or contacting others requires authorization.
