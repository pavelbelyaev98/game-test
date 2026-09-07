# Recommended libraries (modern baseline)

Use this list as defaults, then confirm compatibility with your target Unity version.

1. `com.unity.inputsystem` — required for modern input paths and rebinding.
1. `com.unity.test-framework` — baseline test tooling.
1. `com.unity.timeline` — for simple cut-in/sequence polish.
1. `com.unity.addressables` — if you will load art chunks or presets dynamically.
1. `com.unity.cinemachine` — stable camera rig workflows in 3D.
1. `com.unity.textmeshpro` — text rendering baseline.
1. `com.unity.render-pipelines.universal` — consistent lighting + easy mobile/desktop preview.

Keep optional:

1. `com.unity.ide.visualstudio` only if needed by your workflow.
1. `com.unity.collections` for large ECS-like temporary data structures.
1. Third-party packages only when they replace a known gap, not before core flow is stable.

Update cycle:

1. Open Package Manager and inspect update availability.
1. Update only packages with changelog impact you can accept for your target platform.
1. Run your compile and play checks after each dependency bump.
