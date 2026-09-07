# M01 — Package and URP baseline

Why: the refreshed Unity 6 package set was installed, but the project still rendered through the Built-in pipeline.

Integrated result: Unity `6000.6.0f1` keeps its bundled URP `17.6.0`; a project-wide URP asset and Universal Renderer are assigned in Graphics settings. Quality levels inherit that asset, and the validation scene's Built-in default material references now use URP Lit.

Evidence:
- Unity batch validation loaded `FpsValidation.unity` under URP and verified 21/21 mesh renderers.
- EditMode: 7/7 passed; PlayMode: 14/14 passed.

Remaining limitation: visual quality and movement feel still require an in-editor play review; Unity MCP is not configured for this Codex host.
