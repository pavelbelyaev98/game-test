# 29 - Irregular shovel bites

- Why: noisy spheres still looked like ice-cream scoops; the user also wanted excavation-volume popups removed.
- Integrated: surface-oriented cuts with broad tilted floors, asymmetric taper, softened chipped shoulders and bounded seeded variation. Existing density meshes, synchronous collision, detached-soil cleanup, six shovel presets and collection remain integrated. Removed the volume popup while retaining pickup feedback and the reticle response.
- Evidence: 42/42 EditMode and 42/42 PlayMode checks pass. Official CLI inspected single/repeated small and large cuts. Six fresh cuts took 4.2-10.5 ms; twelve repeated level-6 cuts took 12.6-24.4 ms including terrain/collision updates. Unaffected-sample rejection reduced cutter work without changing measured volumes. Evidence: `unity/Logs/Task29/`.
- Delivery: `builds/windows/SomethingDownThere.exe` rebuilt with zero errors; native 1920x1080 startup inspected and no script errors/exceptions in its log.
- Remaining: deep high-level edits can still exceed a 60 Hz frame budget because meshing/collision remain synchronous; final terrain art stays user-owned. No new assets, dependencies or disposable practice tooling.
