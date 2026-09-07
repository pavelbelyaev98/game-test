# Task 07 - Session inventory identities and values

Why: establish reliable carried-find identities and ordinary values before terrain discovery and selling integration.

Integrated result: immutable `InventoryItem` records contain an owner-supplied instance ID, display name and nonnegative integer sale value. `SessionInventory` retains 10 slots, rejects duplicate IDs/full additions without mutation, exposes read-only inspection and removes by ID while returning the exact record. HUD inspection shows names and values; validation finds retain a single session record across retries, and fixture stations use identity-based removal.

Evidence: deterministic runtime/test compilation; `./tools/test-fps.ps1` passes 21/21 EditMode and 22/22 PlayMode checks on Unity 6000.6.0f1. Coverage includes same-name IDs, rejection without mutation, removal after slot shifts, fixture retry/reactivation, real Tab inspection, station input and existing FPS/terrain integration. Results: `unity/Logs/FpsValidation-20260907-214931-124/`. Main-scene Windows build succeeded at `builds/windows/SomethingDownThere.exe`; metadata, documentation links and diff checks pass.

Limitation/next: the main scene's inventory remains empty until Task `09` adds production discoveries. Task `08` first replaces the visible placeholder foundation. Transactions, capacity upgrades, protected items and disk persistence remain later work.
