# Changelog / 更新日志

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
