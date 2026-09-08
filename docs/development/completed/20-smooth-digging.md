# Task 20 - smooth digging overhaul

- Why: whole 0.5 m cubes made excavation visibly blocky; the requested reference uses overlapping rounded scoops and stronger shovel levels.
- Integrated result: signed density and interpolated surface nets at 0.125 m in `MainGame`; continuous normals across shared chunk samples, matching local collision, unchanged permanent site boundaries and session lifetime.
- Six serialized shovel profiles increase scoop width from 0.96 to 3.80 m, improve cadence/reach, and keep energy at 2 per accepted stroke. Normal upgrades must be sequential; practice selection leaves the owned level intact.
- Testing access: `builds/windows/Digging Practice.cmd` or `tools/play-digging-practice.cmd`; also Esc > Digging practice. Keys 1-6 select strength, R refills, Home returns; menu reset requires confirmation.
- Evidence: 30/30 EditMode and 30/30 PlayMode checks; all four practice integration checks passed again after hardening background launch. Coverage includes seams/winding, increasing volume, energy rejection, traversal/return, boundaries, input gates and reset UI.
- CLI inspection: first/fourth level-1 cuts, all six fresh scoops, a lateral cavity with intact roof, and practice UI. Estimated fresh scoop volumes: 0.25 / 0.64 / 1.45 / 3.17 / 7.33 / 16.58 m3 at equal energy.
- Performance: 24 live cuts across six levels, 10.53 ms mean / 30.77 ms maximum including collision; 4-36 of 864 chunks rebuilt. Largest cuts can exceed a 60 Hz frame budget; figures are from this Editor/machine.
- Windows build: successful, zero errors; actual practice launcher and 1920x1080 player menu inspected, startup confirmed in an exception-free player log. Expected Pipeline warning: editor-control services are disabled in player builds. Evidence: `unity/Logs/Task20/`.
- Limits: existing art/audio retained, no new imports. Approved terrain textures/evolving shovel models, paid purchases and disk persistence remain separate tasks. This completes the requested geometry/strength/testing scope, not the deferred production art gate.
