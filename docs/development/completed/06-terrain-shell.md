# Task 06 - Finite excavation in the main game scene

Why: replace dig-block fixtures with player-shaped excavation before integrating discoveries and the economy.

Integrated result: `TerrainVolume` implements `IDigTarget` over a finite 0.5 m occupancy grid; 108 chunks of 8 cells per axis share exposed-face render/collision geometry. A 1.1 m spherical brush rebuilds only affected chunks and neighbors. Removal survives trips/reactivation within the scene session. `MainGame.unity` adds permanent bedrock/perimeter, surface anchors, URP primitives and inaccessible visible water; the FPS regression scene is preserved.

Evidence: deterministic C# compile; Unity 6000.6.0f1 passes 11/11 EditMode and 20/20 PlayMode checks, including real player descent/lateral traversal/ascent, seams, rejected-hit energy, boundaries and persistence. Isolated editor wiring inspection and URP renders passed after correcting overlapping rim surfaces. `tools/test-terrain.ps1` reproduces the suites; metadata, snapshot equality and documentation checks pass.

Measured: 20 accepted cuts in the final run averaged 0.123 ms (maximum 0.198 ms), rebuilding 1-2 of 108 chunks including collider cooking. A separate seam case verifies neighboring chunk updates. These are local batch measurements, not a frame-rate guarantee.

Limitation/next: stepped surfaces and unsupported static pieces need later polish; application restart resets state, recharge/rescue are pending, and live movement feel remains unreviewed without Unity MCP. Task `07` is ready.
