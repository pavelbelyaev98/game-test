# Release and optional systems

Owns Windows targets, release budgets, the qualification gate and optional/deferred systems. Task IDs are plain numbers.

## Windows targets (`63` selected)
- Windows 11 x64 only on serviced consumer releases; test 24H2/25H2 and recheck at release. Windows 10 22H2 compatibility is deferred with no support promise.
- DX12 default with DX11 fallback on AMD and NVIDIA; API changes need evidence. Keyboard/mouse, click-and-hold digging and release-before-resume preserved.
- Window: bordered resizable + maximize, startup fits the desktop, 16:9 and 16:10. Check 960×540 (readability floor), 1280×720/800, 1920×1080/1200, 2560×1440 and desktop scaling 100/150/200%.
- Uncommitted: borderless/exclusive fullscreen, ultrawide/4:3 guarantees, controller, integrated-GPU/8 GB tiers, ARM/emulation, Linux/Deck, automatic benchmarking.
- Hardware candidates (final minimum/recommended awaits `54`): baseline Ryzen 5 3600 / i5-10400 + GTX 1650 4 GB / RX 5500 XT 4 GB, 16 GB RAM, 1080p 60 fps; higher-resolution Ryzen 5 5600 / i5-12400 + RTX 3060 / RX 6600, 1440p 60 fps; Ryzen 7 9700X + RX 9060 XT 16 GB is a development reference only.

## Release budgets (`63` selected goals, unmeasured until `54`)
- Active frame: mean ≤16.7 ms, p95 ≤18 ms, p99 ≤25 ms, no game-owned frame >50 ms, >33.3 ms ≤0.1%.
- Accepted edit including cleanup/collision/discoveries: p95 ≤8 ms, p99 ≤12 ms, max ≤16.7 ms; terrain and collision must agree before targeting or movement uses the cut.
- Save capture on the main thread: p95 ≤5 ms, max ≤8 ms. Checkpoint request → durable ≤1 s; healthy autosave loses ≤11 s. Save-and-quit/close durable and exits ≤3 s; failed storage keeps readable retry/recovery.
- Launch/restore: cold ≤10 s fresh / ≤15 s Resume, warm ≤5 s / ≤10 s; generation bounded ≤5 s inside the fresh-launch allowance with readable failure on exhaustion.
- Memory over 3 h + Continue: player private ≤2.5 GiB play, peak ≤3.5 GiB; GPU ≤3 GiB on the 4 GB tier; no growing retention.
- Measurement: ≥10 min per case after a 60 s warm-up, retain all spikes, report mean/p95/p99/max and slow frames; final non-development native builds qualify; Editor timings are diagnostic. Budgets apply to later material, generation and lighting work.
- `65`/`78`/toggle digging are implemented; `54` owns the full qualification matrix after `37`/`53`/`22`.

## Release gate
- `22` verifies developer admin exclusion in the release build. `54` passes only with measured budgets, comfort/persistence cases and admin exclusion; missing hardware/platform evidence is an explicit gap.
- Steam testing needs the real AppID, a licensed test account, depot access and a private branch; access is unverified, not refused. No credentials or SDK install are needed for `63`. Publishing needs specific authorization.
- Save portability/Steam Cloud (`91`) is proposed/open: decide save ownership, device-local preferences and conflict handling before release; local persistence remains required. An unselected decision must state the transfer procedure and an honest launch promise.

## Optional systems (implementation deferred until the loop feels good)
- Promote at most one selected optional system at a time into its own task and contract; research may conclude include, defer or omit with the reason. Acceptance: loop already playable, meaningful benefit, no second tool loop or menu bloat.
- C4 (`71`; name selected over "dynamite", same placed remote-detonated concept): charges cost money and remove substantial terrain; forgiving visible-near-surface snapping, valid/invalid preview, no throw, invalid placement free; test anchoring, clipping, blasts, save state and shared `26`/`34` cleanup. Inclusion remains open; the ending must not require it.
- Item condition is low priority and removed if noisy.
- Remembered hard formations give meaningful access, not guaranteed treasure. Large finds require exposure and never enter ordinary slots; extraction may use an in-world cable or stay underground. A tiny number of protected keys/components may live outside normal capacity without inventory puzzles.
- Story reactions belong to `61`/`52`. `70` investigates simple revisit markers. Buried upgrades (`36`) are optional and never make C4 mandatory.
- Keep smelting/recipes, cargo-weight simulation, extra hazards, infinite-world expansion and a compulsory boss out.
- Deferred comparisons: `151` excavation recap/timelapse (bounded images first; no runtime capture or permanent gallery), `152` automatic camp evolution (no placement/upkeep/base-building), suction (deferred; `120` covers Shave in earned progression). Existing cleanup and RMB handling remain.
- First scale comparison belongs to `40`/`42`/`148`; it does not choose cable extraction.
