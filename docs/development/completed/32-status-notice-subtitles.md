# Task 32 - status notice subtitles

- Why: the user's correction applies to all status subtitles, beyond Task `31`'s low-reserve warning.
- Integrated: reserve, critical, empty-battery, recharge and refill notices show titles alone. Removed the developer "Normal gameplay rules" subtitle; meaningful override state stays on one line. Confirmation menus retain their actual consequence details.
- Evidence: existing UI/recharge checks assert title-only messages within the 110 passing Task `14` checks. Official CLI inspected critical/empty/recharge/refill notices; native Windows verified empty/recharge and developer status at 1280x800 and 960x540. Evidence: `unity/Logs/Task14/`.
- Delivery: included in the successful 2026-09-08 14:08 UTC Windows build at `builds/windows/SomethingDownThere.exe`.
- No new art/audio, dependencies or commit. No remaining limitation for this correction.
