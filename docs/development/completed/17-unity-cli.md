# Task 17 - Official Unity CLI setup

Why: enable Unity's official CLI/Pipeline workflow and official agent skills.

Integrated result: CLI `1.0.0-beta.8`, `com.unity.pipeline` `0.6.0-exp.1` and Unity Technologies' `unity-agent-plugin` (`unity` `0.1.0-beta`) are installed and verified. The previous Assistant bridge/package and temporary setup were removed. Official beta/experimental releases are required for this workflow.

Task 18 audit: direct `unity status` and `unity command` calls connect to the running `6000.6.0f1` Editor and support hierarchy, packages, C# eval, captures and all 54 repository checks. The optional Codex server entry was removed at the user's request; direct CLI access needs no such entry. [Current guide](../../../unity/readme.md).

Limitation: keep the Editor open for live work. Installing skills does not retroactively approve older gameplay or presentation; see the [completed-task audit](../../../unity/Logs/Task18Audit/audit.md). No gameplay build or asset changes belong to this setup task.
