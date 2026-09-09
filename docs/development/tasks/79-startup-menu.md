# Task 79 - Startup menu

Type: implementation. Status: `done`. Prerequisites: `35`, `74` (completed persistence and menu systems).

Feature: [menu presentation](../../features/backlog/menu-presentation.md), [world persistence](../../features/backlog/core-loop.md). User-selected alongside refinement of [77](77-ground-textures.md).

Delivery: [completion record](../completed/79-startup-menu.md).

## Scope and selected behavior

- Opening the executable presents New Game, Load Game, Settings and Quit in the existing UI Toolkit style. Gameplay stays paused, HUD hidden and cursor free until a game starts.
- Keep the existing single save slot. Load Game restores it through the existing validation/recovery flow; disable it when no checkpoint exists. Never turn a failed load into a new game.
- New Game starts immediately when no save exists; otherwise show a replacement confirmation with Cancel focused first. Preserve old checkpoint files in a recovery archive before replacing the slot; failed creation must not destroy the old world.
- Settings exposes the existing field-of-view and steady-crosshair controls before play, persists device preferences, and returns to the startup menu. This does not implement the separate input-accessibility task `78`.
- Quit from startup does not create or overwrite a world. Existing gameplay Save and quit remains available. No new scenery, audio or art batch is required.
- User review: prevent concurrent Windows launches in development and release builds. Keep the profile lock as a safeguard; a conflict must explain that the save is already open and support retry without exposing OS errors or personal paths.
- Technical layout and single-slot defaults follow the current implementation; no unresolved product decision blocks this requested flow.

## Launch research

- Rejected [Unity forceSingleInstance](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PlayerSettings-forceSingleInstance.html): native verification produced a blocking “Fatal error” dialog on a duplicate launch. Keep it disabled. `DesktopInstance` reserves a named Windows kernel object before the splash/first scene; duplicates request the existing window and exit successfully before profile initialization. The reservation disappears when the process closes, including a crash.
- [Steamworks initialization](https://partner.steamgames.com/doc/sdk/api?language=english) documents routing executable launches through Steam with `SteamAPI_RestartAppIfNecessary`. Steam integration remains `53`; Windows launch/save protection must work before it.

## Acceptance

- Inspect the actual startup UI, settings/back and replacement confirmation through official Unity CLI at normal and minimum supported window sizes, with mouse and keyboard navigation.
- Test empty and existing slots, cancellation, confirmed fresh progression, restoration, corrupt saves, write failures and input blocking using isolated profiles. Keep user saves untouched during checks.
- Verify a repeated native launch leaves one game running. Test locked-profile New Game and Load recovery after the owner closes; preserve checkpoint bytes during conflict and cancellation.
- Inspect [77](77-ground-textures.md)'s reduced stone density, dirt coating and finer contrast in fresh excavation; retain its source and ownership records.
- Run relevant compile/UI/persistence checks, build `builds/windows/SomethingDownThere.exe`, update concise records and promote the next eligible task. Do not commit.

Validation: 106/106 EditMode; all 93 PlayMode cases covered by the full run and corrected 7/7 startup retest. CLI inspected the 960×540 conflict screen and successful retry in an isolated profile. Native visible launches verified duplicate exit code 0, first-window focus, Quit and relaunch. Hidden-start automation on this host caused a DirectX presentation warning; ordinary visible launch was clean. Evidence: `unity/Logs/Task79/`, Windows build `2026-09-09 13:16 UTC`.
