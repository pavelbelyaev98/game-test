# Task 26 - detached soil cleanup

- Why: digging around soil left unsupported pieces floating; the user wants them to disappear immediately.
- Integrated result: after each scoop, incremental connectivity checks remove components detached from the floor/perimeter. Attached bridges and overhangs survive; the top surface alone is not support. Reused buffers and early anchor exits avoid scanning the full site for ordinary cuts.
- The same accepted stroke includes detached volume, expands mesh/collider updates across chunks, and charges energy once. No falling debris, new assets or disposable tools. Actual-mesh validation rejects cached hits on vanished crowns.
- Evidence: 36/36 EditMode checks pass, including an independent full-field support oracle after 80 irregular cuts and isolated versus boundary-attached crowns. All 36 PlayMode cases pass across the final suite and a focused retry of an existing focus test; cross-chunk removal, stale hits, reset, one revision/charge and matching collision are covered.
- CLI visual inspection: the tall pillar survives surrounding excavation, then disappears when its base is cut. Final stroke removes 2.03 m3 of detached soil, rebuilds 12/864 chunks in 19.46 ms and charges 2 energy. The 51 setup cuts average 12.84 ms (24.10 ms maximum) on this Editor/machine. Evidence: `unity/Logs/Task26/`.
- Windows build succeeded with zero errors and only the expected disabled Pipeline editor-services notice; native 1920x1080 startup/HUD inspected, no gameplay exceptions logged. Delivered through the existing executable.
- Limitation/next action: cleanup is synchronous; larger detachments can cost more than a normal scoop. User feel review remains `16`; profile large-island cases if a hitch becomes noticeable.
