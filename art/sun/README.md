# Sunny sun

The user explicitly approved the sun preview on 2026-09-12 for [128](../../docs/development/tasks/128-visible-sun-and-grass-density.md).

- `Sun.blend` retains the original disc/halo material, receiver, camera and blue preview world in an isolated authoring scene.
- `create_sun.py` renders the 512×512 transparent RGBA `Sun_Disc.png` and an optional temporary blue-background preview through Blender MCP. Run in a fresh file; set `__file__` to the recipe's absolute path when executing its text.
- The runtime sky shader projects this authored texture at infinity, in the existing directional light's direction. The disc is approximately six degrees across, with a restrained halo. No lens flare, clouds or day/night system is included.
- The camera's local Skybox component preserves the blue background and does not replace the environment's ambient or reflection lighting. Run **Configure Approved Sun** after intentionally changing the sun direction or camera background.
- [License](LICENSE.txt) permits free commercial use without attribution. [Ownership and removal](../../docs/asset-ledger.md) lists all game/source files and integration hooks.
