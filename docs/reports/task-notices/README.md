# Completion notice patch checks

- Legacy Unity project: 13 isolated task-notice checks, 0 runtime errors. Covers reaching a goal, showing the task/reward, preserving existing dialogs, claiming once, dismissing, reload persistence, and the next task.
- Current game: real-frame checks include achievement popups, grouping, saved unread notices, acknowledgment without repetition, delayed display behind an existing dialog, and Chinese race results.
- Farm/runtime regression: 39 checks; language regression: 9 checks. Native leaderboard completion emits one callback on success.
- Localization: 1,696 table entries, 0 gaps, 2 locales.
- macOS Universal local patch build: 0 errors / 0 warnings. Actual English/Chinese popup display and Escape acknowledgment were checked. Temporary standalone test data was restored.

The existing v0.11.1 GitHub release is unchanged. This patch is in the local source projects and the local macOS patch package.
