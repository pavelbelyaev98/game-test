# Implemented navigation

User request: consistent ordering, categorized settings and concise copy. Theme: [107](../tasks/107-ui-ux-cleanup.md); current [settings](common-settings.md) in `123`. Exact strings and conditions: [catalog](catalog.csv).

```mermaid
flowchart TD
    Startup["Startup: Continue / New Game / Settings / Quit"]
    Startup -->|Continue: checkpoint exists|Load["Loading"]
    Startup -->|New Game: no checkpoint|Create["Starting"]
    Startup -->|New Game: checkpoint exists|Replace["Cancel / Start New Game"]
    Replace -->|Cancel or Escape|Startup
    Replace -->|Confirm|Create
    Startup -->|Settings|Settings["Display / Graphics / Audio / Controls / Accessibility"]
    Play["Excavation"] -->|Pause or focus loss|Pause["Resume / Settings / Save and quit"]
    Pause -->|Resume|Play
    Pause -->|Settings|Settings
    Settings -->|Bottom Back or Escape; close dropdown first|Origin["Originating Startup or Pause; focus Settings"]
    Settings -->|Binding selected|Capture["Release / Listen / Conflict"]
    Capture -->|Apply, Cancel or Escape|Settings
    Settings -->|Change mode or resolution|Preview["Keep display changes? Standalone dialog; Revert focused"]
    Preview -->|Keep: persist actual mode|Settings
    Preview -->|Escape, 15s timeout or focus loss: revert|Settings
    Load -->|Success|Play
    Create -->|First checkpoint success|Play
    Load -->|Previous checkpoint|Recovery["Recovered: Continue / folder / Quit"]
    Recovery -->|Continue|Play
    Load -->|Failure|Error["Retry / folder or menu / Quit"]
    Create -->|Failure|Error
    Pause -->|Save and quit|Write["Checkpoint then exit"]
    Write -->|Failure|Error
    Error -->|Quit after write failure|Discard["Back / Quit without saving"]
    Play -->|Inventory key|Bag["Inspect only; Close / Escape / Inventory key"]
    Bag -->Play
    Play -->|Interact at station|Shop["Sell or Upgrade; Close initially focused"]
    Shop -->|Close or Escape|Play
```

No dead-end destructive state: safe Cancel/Back remains visible on confirmations; load failure never starts a replacement world. Waiting screens intentionally block actions until an asynchronous result. Development-only admin/reset branches are cataloged separately and remain unavailable in release.
