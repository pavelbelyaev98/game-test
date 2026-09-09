# 65 — Camera comfort settings

- Added Pause → Camera comfort with a 55–90° vertical FOV slider, steady crosshair On by default, reset and Back; existing view remains 75°.
- Device preferences load before play, persist separately from excavation and survive resize/rescue/Continue. Persistent controls retain focus; write failures keep session values and offer local Retry.
- Evidence: **13 preference tests + 68 PlayMode tests passed**; official CLI passage/pit comparisons, native keyboard/mouse, held digging/flight, 960×540–1080p and 16:10 review passed (`unity/Logs/Task65/`).
- Native saved/corrupt/non-finite/out-of-range files, locked-file retry, focus flush and relaunch passed. Reset preserved world-save bytes and a 476.25 m³ live excavation/economy snapshot. Stable-setting microcheck: 0 allocations/writes/events in 100,000 unchanged operations.
- [Windows executable](../../../builds/windows/SomethingDownThere.exe) rebuilt `2026-09-09 05:49 UTC`; MainGame and original profiles restored after isolated validation.
- Remaining: production tool clipping/HUD integration and long-session comfort in `11`/`05`/`54`; [selected contract](../../features/backlog/camera-comfort.md) remains authoritative.
