# Changelog / 更新日志

## [0.11.3] — 2026-11-01 · Program Name Fix

- Accept Chinese- and digit-leading program names across creation, saves, file export/import, module lookup and simulations.
- Preserve invalid input and show creation errors inline; reject duplicate case/Unicode variants and reserved file names; suggest an unused default name.
- Support Unicode script identifiers and module aliases such as `import 123 as farm`; numeric files also support `from 123 import work`.
- Keep the 0.11.2 progression and economy. Existing farms and programs are retained.

## [0.11.2] — 2026-11-01 · Staged Discovery

- Two-column program cards with file/active counts, selected/running/paused/idle indicators, live worker counts and active-file deletion protection.
- Searchable bilingual manual: 16 chapters, 33 API groups, crop algorithms, prerequisites, examples and troubleshooting; English/Chinese Markdown manuals generated from the same content.
- Add `get_unlock_requirements()`, `can_unlock()` and `num_produced()`; enforce all discovery/production gates in the paid research path, including script calls.

- Rebalance fresh-farm progression toward three hours of normal game time with crop-gated field, speed and yield upgrades; cumulative-production milestones and [three-seed paid-research timing evidence](docs/BALANCE-3H.md).
- Bound late cactus/tail scaling, gate companion bonuses behind research, fix proportional merged infection and fertilizer maturity checks; tune research, crop growth and late milestones without timer gates.
- Unify mid-tier competition presets and per-resource targets; separate historical scores and require meaningful maze routes.

- Fix declared `Leaderboards.Pumpkins` / `Pumpkins_Single` aliases; both now complete and have regression checks.
- Add a reproducible zero-inventory full playthrough runner and report: three seeds, all 34 research nodes / 120 paid levels, 13 achievements, 16 competition categories, 32 concurrent workers and phase-by-phase timings/algorithms.

- Continuous 0–100% audio slider with live percentage, immediate music/effect volume and saved settings.

- Connected, searchable, pannable and zoomable research tree covering all 34 existing cultivation, syntax and automation nodes; live prerequisites/costs and node details.
- Botanical laboratory interface: navigation rail, resource cards, next-discovery progress, code line numbers, field journal, isometric farm view and hover feedback.
- Completion notices for local achievements/challenges, persisted unread notifications, grouping and acknowledgment.
- Mac Universal, Windows x64 Mono and complete Unity source packages; earlier releases are preserved.

## [0.11.1] — 2026-11-01

A fresh automation-farm rebuild. Earlier releases are preserved; saves are not migrated.

- Independent coroutine interpreter: functions, closures, returns, modules and collections; isolated worker drones with handles and results.
- Resource-funded progression with 33 research nodes, crop algorithms, giant/dead pumpkins, sunflower priorities, cactus sorting, companions and infection.
- Seeded reusable mazes, treasure, apple/tail movement and bone production.
- Configurable simulations, local fixed-start races, production records and 13 persistent local achievements. Steam online services deferred.
- Code studio with multiple programs/windows, 100-state undo/redo, breakpoints, stepping, variable hover/watch and exported external-editor files.
- Separate farm profiles, automatic backups and default English / optional Simplified Chinese.
- Replaces the earlier contract, money/research-point and large-body traffic model. No floating islands or mining expansion.
- Automated model/interpreter and real-frame bilingual/profile/file tests; Mac Universal and Windows x64 Mono packages. See [scope](docs/PARITY-0.11.1.md) and [acceptance guide](docs/TESTING.md).

本版完整重写为资源驱动的自动化农场，旧存档不迁移。采用独立函数无人机及作物算法，先提供本地竞速、纪录和成就。Windows 真机验收、长期真人平衡和 Mac 签名公证仍待完成。

## [0.10.2] — 2026-10-30

- Playable multi-drone fleet: capacity 1/2/4/8, independently researched airframe tiers, 1×1, 2×2, 3×2, 2×3 and 3×3 bodies.
- General, irrigator and harvester roles; per-drone queues, full-footprint pathfinding, bounded collision fees, active upkeep and zero-money recovery.
- Eight new research projects (26 total), two fleet templates (11 total), fleet management and resizing panels, stable saved IDs and paused queue restoration.
- Crop coin/research returns and supplementary order payouts rebalanced; all 30 contracts remain reachable. See [Fleet & economy](docs/FLEET.md) for exact prices and APIs.
- English default, persistent Simplified Chinese selection, F8 shortcut, Unity Localization String/Asset Tables and bundled CJK fonts. English-first README with Chinese edition.
- macOS Universal and Windows x64 Mono release packages; Windows is cross-built on Mac, native Windows verification pending.
- Actual-engine fleet/core/campaign/localization checks and real-frame bilingual/fleet UI smoke test. Floating islands remain roadmap work.

本版落实多无人机、独立数量／规格解锁与经济调整，并提供中英文和双平台构建。保留 0.10.1 发行与旧存档；新机群科技需正常解锁。

## [0.10.1] — 2026-10-29

首次公开测试发行，macOS 通用应用，包含 Intel x86_64 和 Apple Silicon arm64。

### 游戏与进度

- 用八种不同的新增收获数量目标替代固定 600 秒认证；达量立即领奖。
- 新任务记录收获基线，提前囤货不直接抵扣后续作物任务。
- 明确显示目标、数量、进度与操作提示。
- 科技路线改为节点树，增加前置链，金币与研究成本分别调整为原来的 3 倍和 2 倍。
- 星辉温室范例改为向日葵/星辉花 1:1，改善未研究聚光采集时的能源持续性。

### 代码工作台

- 修复运行时初始化顺序导致光标和选区不显示的问题。
- 增加每份程序最多 100 步会话撤销/重做与操作按钮。
- 新农场默认一个程序，可自由增删，至少保留一个；旧存档的程序保留。
- 查看其他程序不会停止当前执行；运行新程序才替换现有 VM。
- 删除运行中的程序前必须先停止，删除其他程序会修正编号与历史对应关系。

### 保留的内容与修复

- 8 种作物、30 份主线委托、18 项研究、12 项成就。
- 6 项独立试炼、5 种加工配方与 3 个持续更新的订单槽。
- 保留 v0.9.1 的灌溉阈值修复和第 9 个范例“灌溉巡田”。
- 自动保存、备份与恢复机制。

### 验证

50 项规则检查、30 份主线回放和 6 项试炼范例通过。新增收获数量的无加速基准约 11.5～12.6 分钟；实际时间可被算法与科技缩短。

多无人机、大尺寸机体与浮空岛仅完成设计和独立规则原型验证，尚未接入本版可玩场景。发行包未完成 Apple 签名公证、跨机兼容矩阵和长期真人平衡测试。
