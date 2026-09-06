# Imported asset register

Only assets actually imported into the Unity project are listed. [Asset policy](unity-and-assets.md#free-assets-are-the-default) governs sourcing; future release credits should read this register.

| Asset / author | Source and version | License / retained record | Imported files and use | Credit / redistribution |
| --- | --- | --- | --- | --- |
| Impact Sounds / Kenney | [Official pack](https://kenney.nl/assets/impact-sounds), version 1.0 (2019); downloaded September 6, 2026 | CC0; [included license](../../unity/Assets/JustAFewPeppers/Content/Audio/Kenney-Impact-License.txt) | `Content/Audio/impactSoft_medium_000.ogg` (scoop), `impactPlank_medium_000.ogg` (crate), `impactSoft_heavy_000.ogg` (restrained full/invalid cue), all under `Assets/JustAFewPeppers/` | Attribution optional; use “Impact Sounds — Kenney (CC0)” in eventual credits. CC0 permits commercial use and source redistribution. |

The three files are unmodified pack originals, imported mono with preloaded, decompressed playback. Task 1_03 reuses the soft heavy impact for tipping and the plank impact for the empty-crate finish and processing completion; no additional audio was imported. Its intake, stage markers, nine cascade proxies, four partial-fill jar groups, and `Content/Jar body.mat` are deliberate graybox primitives/materials. No other pack content is included. Yard primitives/materials, the authored pepper proxies, and Unity's built-in font remain the graybox representation; there are no purchased assets or new production-art claims. Sound balance and suitability await human play feedback.

The 1_02 free-placement revision reuses the plank impact for rate-limited physical crate contacts. Its worktop/support, wire preview, collision body and `Crate contact.physicMaterial` are graybox authoring using existing wood/materials; no new external asset or license is introduced. The later quiet-placement revision keeps the preview renderer/material as dormant authored assets; runtime no longer draws the outline.

Task 1_04 reuses that CC0 plank impact for receiving, finished-carrier contacts and rack handoff. `Content/Prepared peppers.mat` is one authored graybox food tint; simple prepared-pepper strips, existing jar shapes, a wooden carrier/metal handles, receiving guides and the fixed stored-food group remain primitives. Existing jar/rack materials and physical contact material are reused. Four carrier jar groups and 36 stored jar groups have no individual colliders, food ownership or pickup targets. No new pack, license, purchase or production art was introduced.
