# 28 - Visible small finds and quiet HUD

- Why: small finds should collect as soon as a part is visible; the user requested left-click collection and fewer instructions.
- Integrated: authored Small/Large size rules, real-collider visibility for small slivers, 80% exposure only for Large, and primary-click pickup before digging. E remains for stations. Holding a pickup press cannot dig through the removed object or collect another find.
- Presentation: marker-only X-ray; removed its readout/popups, first-use hints, dig-button prompts, move-closer coaching and Pause's free-jump/shared-battery paragraph. Item identity, pickup confirmation and capacity feedback remain.
- Evidence: 39/39 EditMode and 42/42 PlayMode checks; official CLI inspection of buried markers, a 35% exposed marble collected after two level-1 cuts, pickup feedback and Pause. Windows build succeeded; native 1920x1080 startup inspected with no script errors/exceptions. Evidence is in `unity/Logs/Task28/`.
- Delivery: `builds/windows/SomethingDownThere.exe`, the same development executable.
- Remaining: current content consists of the three user-requested small shapes; Large eligibility is integration-tested, with final discovery art still owned by `09`.
