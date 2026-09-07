# Blender MCP Setup (local-first)

This uses the official Blender Lab MCP source:
`projects.blender.org/lab/blender_mcp`.

Requirements:

- Blender **5.1+** installed (official MCP add-on is published for current Blender branch)
- Git installed
- UV binary available (this repo includes a local uv in `.tmp/uv/uv.exe` once set up)

1) Install the Blender add-on

1. Open Blender -> Edit -> Preferences -> Get Extensions
1. Search `MCP`. If not visible, download the latest Blender MCP add-on package from the official Blender MCP source and install it from disk from `Get Extensions`.

1. Enable **Blender MCP**, open its preferences, enable Online Access, and keep host `127.0.0.1`, port `9876`.
1. Leave Blender running while using MCP

2) Install MCP server package

From the repo root, run:

```powershell
.\.tmp\uv\uv.exe tool install "git+https://projects.blender.org/lab/blender_mcp" --with "mcp<2" --force
```

If your network is blocked in sandbox mode, run this command outside the sandbox or with the required approval.

3) Register in Codex/Astra MCP config

Use this local wrapper path:

```json
{
  "servers": {
    "blender": {
      "type": "stdio",
      "command": "tools\\blender-mcp-server.cmd",
      "args": ["--transport", "stdio"],
      "env": {
        "BLENDER_MCP_HOST": "127.0.0.1",
        "BLENDER_MCP_PORT": "9876"
      }
    }
  }
}
```

4) Quick verification

```powershell
& .\tools\blender-mcp-server.cmd --help
```

Expected: the command prints usage text and exits.

Then reopen VS Code and confirm a `blender` MCP server entry is visible.

If MCP fails, gives unexpected errors, or setup is unclear:

- pause and ask for clarification before continuing risky actions,
- use manual Blender or docs-only workflow for safe steps,
- record the blocker in `docs/development/status.md`.

Safety:

- MCP can execute Python in Blender; do not run write tools on critical scenes.
- Keep backups of `.blend` files under version control or source-safe copies.
