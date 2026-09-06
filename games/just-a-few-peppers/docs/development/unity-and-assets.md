# Unity practices and asset sourcing

Applies to implementation of **Just a few peppers**. Task 1_01 installed Input System 1.20.0, Test Framework 1.8.0, and uGUI 2.6.0 on the unchanged 6000.6.0f1 editor. The graybox uses Unity primitives, placeholder materials, and the built-in font; task 1_02 adds three CC0 audio clips recorded in the [asset register](asset-register.md). See [foundation evidence](tasks/1_01_unity-foundation-and-walkable-scene.md#delivery-record--september-6-2026), the historical [audit](repository-audit.md), and [start guide](start-here.md).

## Use current practices for the project's Unity version

Before choosing an API, installing a package, or changing project configuration:

1. Read `unity/ProjectSettings/ProjectVersion.txt`, `unity/Packages/manifest.json`, and `packages-lock.json` from the game folder. Check the installed editor and existing implementation.
2. Consult official Unity documentation for that editor version and the selected package version. Check relevant release notes or upgrade guidance when replacing an API or package. Do not copy old tutorial patterns without checking support.
3. Use supported, non-obsolete APIs and compatible stable package releases. Avoid preview packages unless a concrete requirement justifies them. Record meaningful package or architectural decisions in the existing documents.
4. Resolve new deprecation warnings in code we own. Review API Updater changes and handle warnings it cannot fix; do not silence warnings to make an obsolete approach appear current. Unity's [API Updater documentation](https://docs.unity3d.com/6000.6/Documentation/Manual/APIUpdater.html) describes its limits.
5. Keep editor and package choices reproducible. “Current practices” does not mean upgrading the engine or every dependency on every task. Make an upgrade a deliberate change with compatibility checks when there is a reason.

**New gameplay uses the Input System package.** Unity's manual for the current `6000.6` editor recommends it and identifies the built-in Input Manager as deprecated. Task 1_01 selected the compatible package, configured Input System-only player settings and authored action maps, and wired gameplay and pause/UI input. Do not extend Stage0's direct `UnityEngine.Input` approach into this game. [Unity 6.6 input guidance](https://docs.unity3d.com/6000.6/Documentation/Manual/Input.html)

Built-in rendering remains the initial baseline recorded in the audit. Choose a different pipeline only when a concrete visual or asset requirement justifies its migration cost. An input deprecation does not establish that every older project setting must change.

Keep the code small: ordinary components, explicit references, one authoritative gameplay state, and bounded visual physics. Modern Unity practice does not require ECS, a dependency-injection framework, or a generic simulator architecture.

## Free assets are the default

**Use existing assets that cost nothing and permit use in a commercial game before creating standard assets from scratch.** This includes models, textures, sounds, music, fonts, and UI elements. AI performs sourcing and integration as part of the feature; the human playtester should not have to assemble asset packs manually.

Use primitives or simple temporary meshes immediately for M1–M4. Maintain enough visual and audio feedback to judge the interaction. Introduce a coherent set of intended assets in M5 before dressing the full yard. Do not block a graybox feature on finding the perfect chair or crate.

| Starting source | Suitable material | License check |
| --- | --- | --- |
| [Kenney](https://kenney.nl/assets) | Stylized props, UI, and suitable sound packs. | Kenney's game assets on its asset pages are CC0 and can be used commercially; check the downloaded pack's included record. [Official guidance](https://kenney.nl/support) |
| [Poly Haven](https://polyhaven.com/) | Environment models, textures, and HDRIs that fit the chosen look and performance budget. | Its asset files are CC0; website content has separate terms. [Official asset license](https://polyhaven.com/license) |
| [Unity Asset Store free assets](https://assetstore.unity.com/top-assets/top-free) | Suitable props, effects, or focused components compatible with the project. | Check the actual asset's license and restrictions. Free price alone does not establish permitted uses. [Unity's commercial-use guidance](https://support.unity.com/hc/en-us/articles/205623589-Can-I-use-assets-from-the-Asset-Store-in-my-commercial-game) |

These are places to search, not an approved or imported asset inventory. Check the exact asset page, download license, render-pipeline compatibility, scale, polygon/texture cost, and dependencies before integrating it. Prefer a few consistent packs over unrelated free models. Import only useful content; avoid a large framework bundled with a prop.

Prefer CC0 where a suitable option exists. Other licenses are acceptable when commercial use and the intended distribution are permitted and attribution or other requirements can be met. Verify whether source files may be committed to the repository, especially if it is public; permission to ship an asset embedded in a game is not necessarily permission to redistribute the raw asset. Use a suitable free alternative when rights are unclear.

When the first third-party asset is actually imported, create a compact `docs/development/asset-register.md`. Record the asset/author, source URL, version or download date, license and retained license-file path, required credit, Unity import location, and any redistribution restrictions. Keep the register and eventual credits synchronized with real imports, including audio and fonts. Do not prefill it with assets merely considered.

Custom or kitbashed assets are appropriate for distinctive equipment, a pepper/pile shape needed by the interaction, or a small missing prop. Grandpa's chushkopek and final machine may need this treatment. Reuse available meshes/materials where practical; custom creation is an exception serving the game, not the default production pipeline. Do not buy assets as an unrequested shortcut.

## Deliver integrated work and verify what changed

The AI owns component wiring, input setup, materials, colliders, prefabs, scene/build configuration, and license records for its additions. Use available Unity/editor tooling and preserve `.meta` references. A feature is handed over as a usable scene or build with controls and a short play checklist.

The Stage0 scene and tools are disposable prototype material. Future implementation may reuse suitable pieces or remove obsolete ones after checking retained references; it does not need to preserve the old roast/steam/peel gameplay. Do not rerun or regenerate Stage0 as a routine prerequisite for current work.

For documentation changes, check relevant links and consistency. For new behavior, run focused rules/integration checks and build checks appropriate to the milestone, then stop repeating them unless changes, failures, or unresolved concerns justify another run. Record actual evidence and anything untested. Human play decides whether the action is enjoyable; AI-generated code and passing tests do not settle that question.



