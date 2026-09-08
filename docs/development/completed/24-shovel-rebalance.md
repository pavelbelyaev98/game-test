# Task 24 - shovel rebalance

- Why: level 1 needed a softer scoop/shorter reach, and cubic volume growth made late-level cuts feel excessive. User requests a maximum reach near 4 m.
- Integrated result: `MainGame` and defaults use radii 0.44 / 0.56 / 0.68 / 0.80 / 0.92 / 1.04 m and reach 2.4 / 2.7 / 3.0 / 3.3 / 3.6 / 4.0 m. A shared 4 m cap covers actual digging and HUD/admin labels, including custom tuning. Existing cadence, organic variation, energy and admin workflow are retained.
- Evidence: 33/33 EditMode and 35/35 PlayMode checks pass; volume progression is bounded, every owned reach level works, out-of-range/custom-cap/occluded hits fail and collection reach remains separate.
- CLI inspection: six fresh scoops remove 0.20 / 0.38 / 0.66 / 1.29 / 1.76 / 2.88 m3 at equal 2 energy. All six out-of-range attempts rejected; sampled mesh/collider updates 3.95-6.11 ms on this Editor/machine. Evidence: `unity/Logs/Task24/`.
- Windows build succeeded with zero errors/code warnings; expected Pipeline notice concerns disabled editor services in players. Normal 1920x1080 startup/HUD inspected, no gameplay exceptions in log.
- Future TODO: Task `25` specifies independent purchasable digging speed and strength. Cuts remain instant; progressive removal during a shovel stroke is documented for discussion if the smaller cuts still feel abrupt. User feel review remains Task `16`.
