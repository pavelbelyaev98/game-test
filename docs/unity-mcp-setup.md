# Unity MCP Setup (Unity 6+ baseline)

Unity MCP was published as a Unity AI workflow integration in the Unity AI tools beta. Requirements move quickly, so check current package docs at start of each session.

Recommended minimum:

1. Unity 6 / 6000.0+ project.
1. Unity AI Assistant package installed in the project.
1. Unity project linked to Unity Cloud (if your org workflow requires it).
1. Client that supports MCP config (Claude Code, Claude Desktop, Cursor, etc.).

## Baseline configuration

1. In Unity Editor: `Edit` → `Project Settings` → `AI` → `MCP Server`.
1. Enable MCP bridge and confirm server status is running.
1. Configure integration for your client if available.
1. If your client is not listed, add a manual `mcpServers` entry using an absolute command path.

## Config shape (server-side project config pattern)

```json
{
  "enabled": true,
  "path": "C:\\path\\to\\tool\\bin",
  "mcpServers": {
    "unity-bridge": {
      "command": "C:\\path\\to\\bridge.exe",
      "type": "stdio",
      "args": ["--mcp"]
    }
  }
}
```

## What MCP is good for in this project

1. Scene checks where script context is not enough (component values, prefabs, runtime logs).
1. Repeatable editor tasks (scene open, import cleanup, console capture).
1. Structured task handoff checks: "show changed components", "read warnings", "open scene".

## What to keep manual

1. Critical gameplay logic decisions.
1. Large batch scene edits that need human review before commit.
1. Versioned design docs and spec updates.

## Practical rule

If the change can be proven and merged safely from file diff alone, do direct file edits.
If the change depends on editor state or live runtime context, use MCP.
If MCP is not working, keep going with deterministic manual edits and ask for help before guessing next MCP steps.
