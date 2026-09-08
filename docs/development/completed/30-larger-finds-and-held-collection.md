# Task 30 - larger finds and held collection

- Why: the user wanted larger valuables, meaningful uncovering and collection without releasing the digging button.
- Integrated: doubled the three existing prefab dimensions; all 96 remain initially buried and separated. Preserved meshes, materials, GUIDs, count and values. Asset ledger records the requested resize and reversal.
- Collection: all current finds require tunable 60% exposure plus visible geometry within 3 m. A held primary action checks pickup before digging, pauses briefly after pickup, then continues; one action per frame, no pickup energy cost and no repeated full-bag feedback.
- Evidence: 46/46 EditMode and 51/51 PlayMode checks, including real-device continuous dig/uncover/pickup/resumed digging, menu/focus release safety, authored thresholds, unsampled-sliver rejection, capacity, identity, reset, range and occlusion.
- Presentation: official CLI inspected the enlarged marble at 17% and 61% exposure and verified a free, single pickup. Native Windows review repeated the sequence with one mouse hold: 82% charge at pickup, 80% after the subsequent dig, one carried item. Captures/results: `unity/Logs/Task30/`.
- Build: `builds/windows/SomethingDownThere.exe`, zero build errors and the expected disabled Pipeline player-services notice; no player script exceptions/errors. Task `13` changes are preserved. No commit.
- Limitation: the existing approved development shapes remain pending final art (`09`); exposure/size tuning remains adjustable and disk persistence remains future work.
