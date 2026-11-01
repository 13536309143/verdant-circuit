# Discovery Studio / 发现工作台验证

- 42 farm/runtime/layout checks and 9 language checks.
- All 34 research nodes appear once; no overlapping cards; dependency routes advance without cycles.
- Real-frame UI checks: live affordability, resource deductions, locked prerequisites, syntax-node purchase, preserved viewport, search, wheel zoom, drag panning, fit overview and bilingual view preservation.
- Previous completion-popup, multi-drone, profile, external-file and code-window checks remain included; 0 runtime errors.
- 1,838 localized table entries, 2 locales, 0 gaps.
- macOS Universal and Windows x64 Mono builds: 0 errors / 0 warnings.
- Mac standalone: actual dashboard, code line numbers, isometric field, F2 research entry, language branch navigation and EN/ZH rendering inspected. Temporary visual-test farm data was restored afterward.

Windows is cross-built on Mac and remains untested on native Windows hardware. Desktop automation mouse clicks do not reliably activate Unity buttons; pointer routing, pan/zoom and purchases are separately tested through the engine event system. Mac packages are not distribution-signed or notarized.

The existing v0.11.1 GitHub release is unchanged. The local source and local Discovery Studio packages contain this update. Open the `verdant-circuit` Unity project for this edition; the earlier `/Volumes/OUT/unity/test` project is the legacy contract edition.
