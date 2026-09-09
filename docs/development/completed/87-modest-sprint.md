# Task 87 - Modest held sprint

- Why: the user requested Shift sprint without excessive speed.
- Result: held Left Shift gives 1.35× horizontal speed (4 to 5.4 m/s), including airborne steering; crouch/blocked standing take priority. Release restores walking immediately, with normalized diagonals and unchanged battery, vertical movement and FOV.
- Controls and Pause expose the current Sprint binding. Older preference maps keep existing actions/mode and receive an unused Sprint key; reset restores Left Shift.
- Evidence: compile and 128/128 EditMode pass. Full PlayMode: 110/111 initially; the existing held-dig test passed alone, then its frame-count timeout was corrected to elapsed game time and all 13 affected discovery tests passed. Sprint, collision, menu/focus and preference checks passed.
- Official CLI MainGame measurements: 4 m walk and 5.400001 m sprint in one second; crouch with Sprint held moves 0.700003 m in half a second. Pause/Controls inspected at 960×540, including updated remap labels, with isolated preferences and no save session.
- Windows build succeeded **2026-09-09 17:45 UTC**, zero errors; only the expected disabled player Pipeline warning. Evidence/screenshots/build report: `unity/Logs/Task87/`.
- Next action: user reviews sprint feel in the Windows build. Task 80 performance qualification remains deferred; no benchmark was restarted.
