> 0.11.2 已改为[约三小时的分阶段研究](BALANCE-3H.md)。下文是历史基线，保留旧规则及实测证据。

# 禾序：完整加速流程实测报告

> 历史基线报告：后续已完成一小时流程平衡修复。当前价格、产量和最新时长请看[新平衡报告](BALANCE-1H.md)，下文保留修改前的证据。


测试日期：2026-11-01。对象：最新版 Discovery Studio 本地源码，包含科技树、音量拖动条和本次南瓜竞速修复；不是历史 30 委托版。

## 结论

三个种子全部通过。每次均从一格、零库存、零研究开始，完成 **34 个科技节点的 120 次真实付费研究、13 项本地成就、9 项界面竞速加 7 项单机接口竞速**，并实际达到 **32×32 农田、32 台同时生产**。共核对 360 次研究扣费，全部与真实成本一致。

主样本种子 731：主世界最终时钟 **31:43.367**；实际测试计算与记录 **29.957 秒**。满级里程碑在 **31:15.767** 达成，后面时间用于复用迷宫、32 机压力段与竞速验证。

## 如何理解时间和加速

- 使用真实 `ClassicSession.FixedTick()`：普通执行每步推进主世界 1/60 游戏秒，隔离实验按同样步长推进各自世界。使用真实解释器、成长、资源成本和随机数；不提高成长倍率、不赠资源、不跳过主流程语法门槛。
- 加速来自连续计算，省去帧渲染和等待。实际测试秒数包含该轮脚本执行、校验和记录文件的时间，不含 Unity 加载/退出，以及本次测试工具编写和报告整理。
- 主世界时钟按 1 倍速规则理解，已经包含正常研究带来的动作加速；不是把科技速度重设为 1。
- 这是覆盖优先的自动脚本路线；不计真人编辑、思考、点击耗时，也不是最短通关或新手平均时长。
- 模拟和竞速在游戏自带的隔离预设中运行，时间单独列出，不把预设供给计为主流程资源，也不把其时钟冒充主存档时钟。竞速始终使用预设种子 41。

## 三种子复跑

| 主世界种子 | 结果 | 主世界最终时钟 | 实际计算与记录 | 步骤 | 满级节点 | 成就 | 竞速 | 同时工作机 |
|---|---|---:|---:|---:|---:|---:|---:|---:|
| 731 | 通过 | 31:43.367 | 29.957s | 249 | 34/34 | 13/13 | 16 | 32 |
| 17 | 通过 | 30:00.200 | 27.984s | 243 | 34/34 | 13/13 | 16 | 32 |
| 20261101 | 通过 | 30:59.400 | 28.319s | 244 | 34/34 | 13/13 | 16 | 32 |

随机复跑覆盖主流程的伴生偏好、南瓜死亡、迷宫和苹果目标变化；不等于竞速测试了三个不同预设地图。

## 操作顺序与阶段耗时（主样本 731）

游戏内复现时新建独立农场，关闭未解锁语法选项；按阶段研究并替换程序。不要先解锁全部科技或导入最终存档再把它当作新开局测试。

| 阶段 | 操作 | 本段游戏耗时 | 累计时钟 |
|---|---|---:|---:|
| 起步与基础语法 | 收草→循环→运算符、变量、传感器、种植 | 03:18.233 | 03:18.233 |
| 基础规模与产能 | 扩到 8×8，交替升级干草与执行速度，再升级林木 | 09:37.600 | 12:55.833 |
| 作物与编程工具 | 胡萝卜、灌溉、南瓜、太阳花，以及集合、模块、自动研究等 | 00:44.000 | 13:39.833 |
| 向日葵供能 | 成熟等待与九档优先收获，达到 1,000 能量库存目标 | 00:43.967 | 14:23.800 |
| 胡萝卜/南瓜升级 | 准备原料，升级胡萝卜和南瓜至满级 | 02:38.167 | 17:01.967 |
| 高级玩法初开 | 感染材料→迷宫→仙人掌→长尾→机群→模拟→竞速 | 02:32.983 | 19:34.950 |
| 算法验证 | 伴生、早期并行、固定种子模拟 | 00:07.983 | 19:42.933 |
| 其余升级与 32×32 | 升级剩余生产/机群，再完成扩田 | 05:15.750 | 24:58.683 |
| 后期里程碑 | 32×32 长尾生产骨头，研究自治信标、获取金币、研究大师礼帽 | 06:17.083 | 31:15.767 |
| 最终验证 | 外观、迷宫复用、32 机同时工作和全部竞速 | 00:27.600 | 31:43.367 |

### 关键执行说明

1. 起步运行 `harvest()` 并等待自然成熟，累计五份干草后在树中研究“循环”。接着用 `while True: harvest()` 攒基础成本，到目标后停止。
2. 后续生产段运行至库存目标自动结束，再研究或更换作物。`clear()` 会清理农田，测试路线因此会损耗部分在田作物；它没有替你赠送种子。
3. 自动研究节点获得前，研究通过与 UI 相同的 `Farm.Unlock` 付费方法完成；获得后以真实 `unlock()` 脚本购买。
4. 每段完整代码、执行前后库存和阶段结束存档都已保留；这份表下方有全部步骤索引。
5. 竞速前在“程序”中新建同名模块，复制 modules 目录中的代码，再运行对应的 leaderboard_run 入口。实验模块的 rounds 由 simulate 的 globals 参数传入。

## 34 节点解锁记录

| 节点 | 最高等级 | 首级完成时钟 | 满级完成时钟 | 累计支付 | 前置 |
|---|---:|---:|---:|---:|---|
| 种植 (`plant`) | 1 | 03:18.233 | 03:18.233 | 5 干草 | 起始 |
| 农田扩张 (`expand`) | 5 | 03:18.233 | 24:58.683 | 452 干草 | 种植 |
| 执行速度 (`speed`) | 8 | 09:26.917 | 12:24.300 | 15,714 干草 | 农田扩张 |
| 干草增产 (`grass`) | 8 | 09:26.917 | 12:20.950 | 39,280 干草 | 种植 |
| 林木 (`trees`) | 8 | 04:26.483 | 12:55.833 | 23,569 木材 | 种植 |
| 胡萝卜种植 (`carrots`) | 8 | 06:53.533 | 14:34.617 | 31,425 木材 | 农田扩张 |
| 灌溉 (`watering`) | 6 | 12:55.833 | 19:43.017 | 10,865 木材 | 胡萝卜种植 |
| 巨型南瓜 (`pumpkins`) | 8 | 13:06.767 | 17:01.967 | 78,556 胡萝卜 | 胡萝卜种植 |
| 太阳花 (`sunflowers`) | 8 | 13:22.333 | 21:18.367 | 117,833 胡萝卜 | 灌溉 |
| 肥料与感染 (`fertilizer`) | 6 | 18:16.317 | 21:18.450 | 13,580 南瓜 | 巨型南瓜 |
| 伴生种植 (`polyculture`) | 6 | 21:18.467 | 21:31.417 | 67,895 干草 | 林木 |
| 仙人掌排序 (`cactus`) | 8 | 19:24.383 | 24:22.933 | 314,218 南瓜 | 巨型南瓜 |
| 树篱迷宫 (`mazes`) | 8 | 19:24.367 | 24:27.400 | 23,569 奇异物质 | 肥料与感染 |
| 长尾探索 (`dinosaurs`) | 8 | 19:28.567 | 24:27.283 | 392,771 仙人掌 | 仙人掌排序 |
| 并行无人机 (`megafarm`) | 5 | 19:28.817 | 24:58.650 | 5,617 宝藏金币 | 树篱迷宫 |
| 模拟实验室 (`simulation`) | 1 | 19:31.433 | 19:31.433 | 100 骨头 | 长尾探索 |
| 本地竞速 (`leaderboard`) | 1 | 19:34.950 | 19:34.950 | 1,000 宝藏金币 | 模拟实验室 |
| 循环 (`loops`) | 1 | 00:16.017 | 00:16.017 | 5 干草 | 起始 |
| 运算符 (`operators`) | 1 | 03:18.233 | 03:18.233 | 10 干草 | 循环 |
| 变量 (`variables`) | 1 | 03:18.233 | 03:18.233 | 15 干草 | 运算符 |
| 函数 (`functions`) | 1 | 04:49.783 | 04:49.783 | 20 木材 | 变量 |
| 列表与元组 (`lists`) | 1 | 05:20.333 | 05:20.333 | 50 木材 | 函数 |
| 字典与集合 (`dicts`) | 1 | 13:28.150 | 13:28.150 | 30 胡萝卜 | 列表与元组 |
| 模块 (`import`) | 1 | 13:33.967 | 13:33.967 | 50 胡萝卜 | 函数 |
| 传感器 (`senses`) | 1 | 03:18.233 | 03:18.233 | 10 干草 | 起始 |
| 成本查询 (`costs`) | 1 | 05:43.633 | 05:43.633 | 10 木材 | 起始 |
| 自动研究 (`auto_unlock`) | 1 | 13:39.783 | 13:39.783 | 60 胡萝卜 | 起始 |
| 计时工具 (`timing`) | 1 | 06:06.933 | 06:06.933 | 20 木材 | 起始 |
| 调试输出 (`debug`) | 1 | 13:39.800 | 13:39.800 | 5 干草 | 起始 |
| 帽子 (`hats`) | 1 | 06:30.233 | 06:30.233 | 25 木材 | 起始 |
| 世界调试 (`debug2`) | 1 | 13:39.817 | 13:39.817 | 100 木材 | 调试输出 |
| 数值工具 (`utilities`) | 1 | 13:39.833 | 13:39.833 | 60 木材 | 运算符 |
| 自治信标 (`the_farmers_remains`) | 1 | 31:12.667 | 31:12.667 | 10,000 骨头 | 本地竞速 |
| 大师礼帽 (`top_hat`) | 1 | 31:15.767 | 31:15.767 | 100,000 宝藏金币 | 自治信标 |

[120 次付费研究逐笔对账 CSV](reports/full-playthrough/run-731/research-ledger.csv)。三轮共 360 笔，期望成本与实际扣费全部相符。

## 各部分算法

| 部分 | 本次使用的方法 | 复杂度/代价 | 完整代码 |
|---|---|---|---|
| 起步和遍历 | 一格自然成长→五份干草→解锁循环；之后逐行扫描，成熟收获。 | 每轮 O(n²) | [步骤 23](reports/full-playthrough/run-731/programs/0023.py) |
| 棋盘林地 | 树与灌木交错，减少四邻树木减速；空地补种。 | 每轮 O(n²) | [步骤 11](reports/full-playthrough/run-731/programs/0011.py) |
| 胡萝卜与原料预算 | 先按当前种子成本和产量估算轮数，准备干草、木材；再种植、等待、补种。 | 每轮 O(n²) | [步骤 69](reports/full-playthrough/run-731/programs/0069.py) |
| 巨型南瓜 | 反复扫描、补种枯死格；整块成熟再收获，不能提前拆散方阵。 | O(k·n²)，k 为修复轮数 | [步骤 119](reports/full-playthrough/run-731/programs/0119.py) |
| 浇水和感染 | 水分低于 0.3 且库存有水才灌溉；成熟南瓜收获格施肥感染，产生奇异物质。 | 每轮 O(n²) | [步骤 124](reports/full-playthrough/run-731/programs/0124.py) |
| 向日葵供能 | 先播完整田；等待时只读成熟状态，不重复移动。按 15→7 花瓣九档扫描，保持最大值优先。 | 固定九档 O(n²) 扫描 | [步骤 84](reports/full-playthrough/run-731/programs/0084.py) |
| 仙人掌二维排序 | 成熟后先对每行冒泡，再对每列冒泡。列排序保留行间单调性，形成二维有序田，触发连收。 | O(n³) 比较/移动 | [步骤 127](reports/full-playthrough/run-731/programs/0127.py) |
| 迷宫搜索 | 用坐标集合去重，方向栈记录来路；遇到死路按反方向回溯。 | O(V+E)，网格图为 O(n²) | [步骤 129](reports/full-playthrough/run-731/programs/0129.py) |
| 迷宫复用 | 到宝藏先使用奇异物质重置目标并开墙，再使用含 visited 的 DFS，支持有环地图。 | 每轮 O(V+E) | [步骤 232](reports/full-playthrough/run-731/programs/0232.py) |
| 长尾覆盖 | 偶数边长上的哈密顿回路：蛇行覆盖其余列，以第一列回到起点；满尾受阻后换帽结算。 | 脚本方向决策 O(1)；引擎碰撞检查随尾长增加 | [步骤 132](reports/full-playthrough/run-731/programs/0132.py) |
| 伴生布局 | 读取偏好类型和坐标，在目标点布置伴生，再回原格成熟收获。 | 单次移动 O(n) | [步骤 152](reports/full-playthrough/run-731/programs/0152.py) |
| 32 台独立无人机 | 按列分工；生成失败时主机回退工作；等待句柄回收。压力段持续工作八游戏秒，实际峰值 32 台。 | 单轮全田 O(n²)；压力段按固定时长重复 | [步骤 233](reports/full-playthrough/run-731/programs/0233.py) |
| 固定种子模拟 | 在独立世界注入 rounds=64，随机决定动作；同种子耗时相同，换种子耗时改变。 | 64 次试验 | [步骤 154](reports/full-playthrough/run-731/programs/0154.py) |

[算法解释及完整可复制代码](reports/full-playthrough/algorithms.md)；[最快重置完整程序](reports/full-playthrough/run-731/fastest-reset.py)。复杂度中的 n 是农田边长，V/E 是迷宫格点/通道数。Python 风格解释器步骤和移动计费也会影响实际耗时。

### 向日葵失败案例与改进

初版按每株反复寻找全田最大值，扫描开销约 O(n⁴)；随后虽然改成九档扫描，成长期仍反复走完整田。两次试跑均实际生产了 1,796 能量，但库存目标未达、种子耗尽，最后 61 株加 3 个空格，脚本继续等空格成熟。

正式版同时改变排序与等待方式：整田播种后，仅用传感器轮询等待成熟，再按 15→7 花瓣分档收获。三轮复跑达到库存目标及累计能量成就。没有修改能量产量、消耗或成长常数。[初版代码](reports/full-playthrough/algorithm-comparison/naive-maximum-scan.py) / [中间版代码](reports/full-playthrough/algorithm-comparison/bucket-scan-with-busy-wait.py)。

## 独立模拟与全部竞速

模拟注入 rounds=64：种子 7 两次均为 **15.967s**，种子 8 为 **16.967s**，验证了同种子复现和不同种子的差别。主样本覆盖的隔离模拟/竞速世界时间共 **760.517s**，另计。

| 类别 | 成功耗时（隔离世界游戏秒） | 程序 |
|---|---:|---|
| `Hay` | 3.083 | [race_hay](reports/full-playthrough/run-731/modules/race_hay.py) |
| `Hay_Single` | 3.083 | [race_hay](reports/full-playthrough/run-731/modules/race_hay.py) |
| `Wood` | 4.333 | [race_wood](reports/full-playthrough/run-731/modules/race_wood.py) |
| `Wood_Single` | 4.333 | [race_wood](reports/full-playthrough/run-731/modules/race_wood.py) |
| `Carrots` | 5.417 | [race_carrots](reports/full-playthrough/run-731/modules/race_carrots.py) |
| `Carrots_Single` | 5.417 | [race_carrots](reports/full-playthrough/run-731/modules/race_carrots.py) |
| `Pumpkins` | 24.350 | [race_pumpkins](reports/full-playthrough/run-731/modules/race_pumpkins.py) |
| `Pumpkins_Single` | 24.350 | [race_pumpkins](reports/full-playthrough/run-731/modules/race_pumpkins.py) |
| `Sunflowers` | 7.817 | [race_sunflowers](reports/full-playthrough/run-731/modules/race_sunflowers.py) |
| `Sunflowers_Single` | 7.817 | [race_sunflowers](reports/full-playthrough/run-731/modules/race_sunflowers.py) |
| `Cactus` | 2.600 | [race_cactus](reports/full-playthrough/run-731/modules/race_cactus.py) |
| `Cactus_Single` | 2.600 | [race_cactus](reports/full-playthrough/run-731/modules/race_cactus.py) |
| `Maze` | 0.017 | [race_maze](reports/full-playthrough/run-731/modules/race_maze.py) |
| `Maze_Single` | 0.017 | [race_maze](reports/full-playthrough/run-731/modules/race_maze.py) |
| `Dinosaur` | 1.667 | [race_dinosaur](reports/full-playthrough/run-731/modules/race_dinosaur.py) |
| `Fastest_Reset` | 614.717 | [race_reset](reports/full-playthrough/run-731/modules/race_reset.py) |

这些是本次有效成绩，不是优化后的最快纪录。8 种普通资源竞速加最快重置是界面中的 9 项；另外 7 项是 SDK 声明的单机类别。所有记录使用固定预设种子 41，三轮结果一致。

迷宫预设审计：宝藏索引 18，起点索引 0；起点不是宝藏，DFS 的 visited 记录 4 个非宝藏格，因此该预设路线很短。0.017s 成绩不应被解释为复杂迷宫搜索的典型耗时。[审计原始值](reports/full-playthrough/run-731/maze-preset-audit.json)。

## 资源与规则核验

| 资源 | 主世界累计产出 | 最终库存 |
|---|---:|---:|
| 干草 | 1,752,176 | 799,723 |
| 木材 | 952,752 | 57,546 |
| 胡萝卜 | 316,227 | 26,194 |
| 南瓜 | 393,216 | 57,162 |
| 能量 | 1,347 | 0 |
| 仙人掌 | 528,384 | 134,525 |
| 骨头 | 133,959,681 | 133,949,581 |
| 宝藏金币 | 402,560 | 295,943 |
| 水 | 不使用 harvested 计数，见逐步库存 | 1,280 |
| 肥料 | 不使用 harvested 计数，见逐步库存 | 2,000 |
| 奇异物质 | 不使用 harvested 计数，见逐步库存 | 12,127 |

水和肥料来自真实时间供应，奇异物质来自感染收获；这三种资源没有与普通作物一致的 harvested 累计栏，不能把最终库存当作累计产量。

本版基础规则：草 3 秒、灌木 4 秒、树 6 秒、胡萝卜 5 秒、南瓜 8 秒、向日葵 7 秒、仙人掌 1 秒；水分提升成长速度，正交邻树降低树木速度。研究和动作加速会显著缩短实际生产周期。

### 长尾后期产出

32×32 满尾长度 1023，8 级长尾结算为 `1023² × 128 = 133,955,712` 骨头。该量远大于自治信标 10,000 骨头成本；这证明可通关，也暴露了后期资源消耗偏低。并行压力段则真实达到 32 台，而不是只解锁容量后宣称并行已验证。

## 发现、修复与后续建议

| 编号 | 类别 | 发现与处理 |
|---|---|---|
| F01 | 已修复的游戏缺陷 | Leaderboards.Pumpkins / Pumpkins_Single 缺少复数别名映射。 ClassicSession.Leaderboard 将 pumpkins 映射到 pumpkin；两种类别新增回归；3 次完整重跑均完成 16 竞速。 |
| F02 | 已修正的测试算法 | 向日葵逐株全田扫描及移动等待开销高，按固定库存目标估算种子不足后会等待空格成熟。 正式路线使用花瓣九档扫描和传感器等待；种子预算充足，实际达到库存目标并获得累计 1,000 能量成就。 |
| F03 | 待平衡 | 32×32、8 级长尾一次输出 133,955,712 骨头，远超自治信标 10,000 骨头成本。 本次不改数值。建议后续按机群/田规模调整后期消耗或增加持续资源用途。 |
| F04 | 待平衡 | 所有资源竞速统一新增 1,000，且预设全部科技，种类之间难度差异很大。 不改本次预设。建议按作物产量设不同目标，并用多个固定地图或种子综合成绩。 |
| F05 | 规则说明 | 伴生基础倍率在未研究伴生增产时就存在；相关科技提升倍率，而不是首次开放基础奖励。 建议在教学/科技说明中讲清基础机制与倍率升级的区别，避免误以为未解锁功能。 |
| F06 | 性能建议（待专项测量） | 长尾方向决策本身是常数复杂度，但引擎用列表检查尾巴占用；生成苹果逐格调用 tail.Contains，最坏约 O(n²·k)，k 为尾长。 建议单独性能测试后考虑占用 HashSet 与空格索引池；本次不改实现，避免把静态推断当成已量化的性能结论。 |

[全部失败尝试分类](reports/full-playthrough/earlier-attempts.json)。其中本地化初始化失败是测试工具问题；能量目标不全和等待逻辑是测试方案问题；Pumpkins 名称映射是实际游戏接口问题，已经修复。除该别名修复外，本次没有改经济或成长数值。

## 专项回归、构建与边界

- 44 项规则/运行系统检查、9 项语言检查通过；其中包含新补的 Pumpkins 和 Pumpkins_Single 两项回归。
- 界面专项推进 1136 真实引擎帧、0 运行错误，覆盖首收、多机、双语、档案/文件、独立代码窗口、完成弹窗、科技树购买/平移/缩放和音量条即时生效/保存。
- Mac Universal、Windows x64 Mono 构建均为 0 错误 / 0 警告。Windows 未做真机运行，Mac 未做发行签名公证。
- 规则和界面专项使用独立沙盒夹具；不将其预置资源算入零存档通关。
- 本报告没有穷尽所有用户程序、每个随机种子或全部异常硬件组合；32 机验证是经营逻辑并发，不是跨硬件 32 机渲染性能基准。
- 本次测试未覆盖历史 30 委托版、Steam 在线排行/成就或尚未加入的岛屿/采矿模式。GitHub 旧发行未被替换，本地修复包包含南瓜竞速修复。

## 全部逐步操作和代码索引

主样本共有 249 步；[步骤与耗时 CSV](reports/full-playthrough/run-731/steps.csv)、[执行前后库存/步骤/tick 原始 JSON](reports/full-playthrough/run-731/report.json)、[逐阶段存档](reports/full-playthrough/run-731/saves/)。手动研究步骤没有脚本，按同一行科技树节点正常付款。

| 步骤 | 操作 | 本段游戏秒 | 实际计算秒 | 代码/操作 |
|---:|---|---:|---:|---|
| 1 | 起步：手动收获五份干草 | 16.017 | 0.0166 | [完整代码](reports/full-playthrough/run-731/programs/0001.py) |
| 2 | 研究「循环」→ 1 | 0.000 | 0.0002 | 科技树点击研究并支付成本 |
| 3 | 起步循环攒干草至 60 | 182.217 | 0.0086 | [完整代码](reports/full-playthrough/run-731/programs/0003.py) |
| 4 | 研究「运算符」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 5 | 研究「变量」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 6 | 研究「传感器」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 7 | 研究「种植」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 8 | 研究「农田扩张」→ 1 | 0.000 | 0.0007 | 科技树点击研究并支付成本 |
| 9 | 生产 木材 至 30 | 68.250 | 0.0078 | [完整代码](reports/full-playthrough/run-731/programs/0009.py) |
| 10 | 研究「林木」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 11 | 生产 木材 至 20 | 23.300 | 0.0029 | [完整代码](reports/full-playthrough/run-731/programs/0011.py) |
| 12 | 研究「函数」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 13 | 生产 木材 至 50 | 30.550 | 0.0031 | [完整代码](reports/full-playthrough/run-731/programs/0013.py) |
| 14 | 研究「列表与元组」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 15 | 生产 木材 至 10 | 23.300 | 0.0025 | [完整代码](reports/full-playthrough/run-731/programs/0015.py) |
| 16 | 研究「成本查询」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 17 | 生产 木材 至 20 | 23.300 | 0.0023 | [完整代码](reports/full-playthrough/run-731/programs/0017.py) |
| 18 | 研究「计时工具」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 19 | 生产 木材 至 25 | 23.300 | 0.0022 | [完整代码](reports/full-playthrough/run-731/programs/0019.py) |
| 20 | 研究「帽子」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 21 | 生产 木材 至 40 | 23.300 | 0.0023 | [完整代码](reports/full-playthrough/run-731/programs/0021.py) |
| 22 | 研究「胡萝卜种植」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 23 | 生产 干草 至 20 | 13.883 | 0.0008 | [完整代码](reports/full-playthrough/run-731/programs/0023.py) |
| 24 | 研究「农田扩张」→ 2 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 25 | 生产 干草 至 47 | 72.083 | 0.0073 | [完整代码](reports/full-playthrough/run-731/programs/0025.py) |
| 26 | 研究「农田扩张」→ 3 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 27 | 生产 干草 至 50 | 67.417 | 0.0141 | [完整代码](reports/full-playthrough/run-731/programs/0027.py) |
| 28 | 研究「干草增产」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 29 | 研究「执行速度」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 30 | 生产 干草 至 120 | 67.150 | 0.0156 | [完整代码](reports/full-playthrough/run-731/programs/0030.py) |
| 31 | 研究「干草增产」→ 2 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 32 | 研究「执行速度」→ 2 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 33 | 生产 干草 至 288 | 32.200 | 0.0085 | [完整代码](reports/full-playthrough/run-731/programs/0033.py) |
| 34 | 研究「干草增产」→ 3 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 35 | 研究「执行速度」→ 3 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 36 | 生产 干草 至 692 | 14.833 | 0.0046 | [完整代码](reports/full-playthrough/run-731/programs/0036.py) |
| 37 | 研究「干草增产」→ 4 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 38 | 生产 干草 至 277 | 6.167 | 0.0024 | [完整代码](reports/full-playthrough/run-731/programs/0038.py) |
| 39 | 研究「执行速度」→ 4 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 40 | 生产 干草 至 1659 | 10.433 | 0.0050 | [完整代码](reports/full-playthrough/run-731/programs/0040.py) |
| 41 | 研究「干草增产」→ 5 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 42 | 生产 干草 至 664 | 6.100 | 0.0032 | [完整代码](reports/full-playthrough/run-731/programs/0042.py) |
| 43 | 研究「执行速度」→ 5 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 44 | 生产 干草 至 3982 | 9.000 | 0.0072 | [完整代码](reports/full-playthrough/run-731/programs/0044.py) |
| 45 | 研究「干草增产」→ 6 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 46 | 生产 干草 至 1593 | 3.900 | 0.0031 | [完整代码](reports/full-playthrough/run-731/programs/0046.py) |
| 47 | 研究「执行速度」→ 6 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 48 | 生产 干草 至 9556 | 10.700 | 0.0151 | [完整代码](reports/full-playthrough/run-731/programs/0048.py) |
| 49 | 研究「干草增产」→ 7 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 50 | 生产 干草 至 3823 | 3.900 | 0.0073 | [完整代码](reports/full-playthrough/run-731/programs/0050.py) |
| 51 | 研究「执行速度」→ 7 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 52 | 生产 干草 至 22933 | 9.650 | 0.0272 | [完整代码](reports/full-playthrough/run-731/programs/0052.py) |
| 53 | 研究「干草增产」→ 8 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 54 | 生产 干草 至 9173 | 3.350 | 0.0088 | [完整代码](reports/full-playthrough/run-731/programs/0054.py) |
| 55 | 研究「执行速度」→ 8 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 56 | 生产 木材 至 72 | 6.717 | 0.0558 | [完整代码](reports/full-playthrough/run-731/programs/0056.py) |
| 57 | 研究「林木」→ 2 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 58 | 研究「林木」→ 3 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 59 | 生产 木材 至 415 | 6.717 | 0.0510 | [完整代码](reports/full-playthrough/run-731/programs/0059.py) |
| 60 | 研究「林木」→ 4 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 61 | 研究「林木」→ 5 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 62 | 生产 木材 至 2389 | 6.717 | 0.0511 | [完整代码](reports/full-playthrough/run-731/programs/0062.py) |
| 63 | 研究「林木」→ 6 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 64 | 生产 木材 至 5734 | 6.717 | 0.0504 | [完整代码](reports/full-playthrough/run-731/programs/0064.py) |
| 65 | 研究「林木」→ 7 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 66 | 生产 木材 至 13760 | 4.667 | 0.0352 | [完整代码](reports/full-playthrough/run-731/programs/0066.py) |
| 67 | 研究「林木」→ 8 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 68 | 研究「灌溉」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 69 | 生产 胡萝卜 至 100 | 10.933 | 0.1318 | [完整代码](reports/full-playthrough/run-731/programs/0069.py) |
| 70 | 研究「巨型南瓜」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 71 | 生产 木材 至 384 | 4.633 | 0.0588 | [完整代码](reports/full-playthrough/run-731/programs/0071.py) |
| 72 | 生产 胡萝卜 至 150 | 10.933 | 0.1338 | [完整代码](reports/full-playthrough/run-731/programs/0072.py) |
| 73 | 研究「太阳花」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 74 | 生产 胡萝卜 至 30 | 5.817 | 0.0685 | [完整代码](reports/full-playthrough/run-731/programs/0074.py) |
| 75 | 研究「字典与集合」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 76 | 生产 胡萝卜 至 50 | 5.817 | 0.0711 | [完整代码](reports/full-playthrough/run-731/programs/0076.py) |
| 77 | 研究「模块」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 78 | 生产 胡萝卜 至 60 | 5.817 | 0.0704 | [完整代码](reports/full-playthrough/run-731/programs/0078.py) |
| 79 | 研究「自动研究」→ 1 | 0.000 | 0.0000 | 科技树点击研究并支付成本 |
| 80 | 研究「调试输出」→ 1 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0080.py) |
| 81 | 研究「世界调试」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0081.py) |
| 82 | 研究「数值工具」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0082.py) |
| 83 | 生产 胡萝卜 至 256 | 21.167 | 0.2614 | [完整代码](reports/full-playthrough/run-731/programs/0083.py) |
| 84 | 生产 能量 至 1000 | 22.800 | 0.4542 | [完整代码](reports/full-playthrough/run-731/programs/0084.py) |
| 85 | 研究「胡萝卜种植」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0085.py) |
| 86 | 研究「胡萝卜种植」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0086.py) |
| 87 | 研究「胡萝卜种植」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0087.py) |
| 88 | 研究「胡萝卜种植」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0088.py) |
| 89 | 研究「胡萝卜种植」→ 6 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0089.py) |
| 90 | 生产 木材 至 7645 | 4.350 | 0.1204 | [完整代码](reports/full-playthrough/run-731/programs/0090.py) |
| 91 | 研究「胡萝卜种植」→ 7 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0091.py) |
| 92 | 生产 木材 至 18346 | 6.350 | 0.1782 | [完整代码](reports/full-playthrough/run-731/programs/0092.py) |
| 93 | 研究「胡萝卜种植」→ 8 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0093.py) |
| 94 | 生产 干草 至 32768 | 6.250 | 0.1602 | [完整代码](reports/full-playthrough/run-731/programs/0094.py) |
| 95 | 生产 木材 至 32768 | 4.333 | 0.1273 | [完整代码](reports/full-playthrough/run-731/programs/0095.py) |
| 96 | 生产 胡萝卜 至 240 | 3.050 | 0.0892 | [完整代码](reports/full-playthrough/run-731/programs/0096.py) |
| 97 | 研究「巨型南瓜」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0097.py) |
| 98 | 生产 干草 至 32768 | 3.200 | 0.0827 | [完整代码](reports/full-playthrough/run-731/programs/0098.py) |
| 99 | 生产 木材 至 32768 | 6.250 | 0.1853 | [完整代码](reports/full-playthrough/run-731/programs/0099.py) |
| 100 | 生产 胡萝卜 至 576 | 5.617 | 0.1149 | [完整代码](reports/full-playthrough/run-731/programs/0100.py) |
| 101 | 研究「巨型南瓜」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0101.py) |
| 102 | 研究「巨型南瓜」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0102.py) |
| 103 | 研究「巨型南瓜」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0103.py) |
| 104 | 生产 干草 至 32768 | 6.533 | 0.0741 | [完整代码](reports/full-playthrough/run-731/programs/0104.py) |
| 105 | 生产 木材 至 32768 | 6.583 | 0.0880 | [完整代码](reports/full-playthrough/run-731/programs/0105.py) |
| 106 | 生产 胡萝卜 至 7963 | 5.817 | 0.0741 | [完整代码](reports/full-playthrough/run-731/programs/0106.py) |
| 107 | 研究「巨型南瓜」→ 6 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0107.py) |
| 108 | 生产 干草 至 49152 | 9.650 | 0.1084 | [完整代码](reports/full-playthrough/run-731/programs/0108.py) |
| 109 | 生产 木材 至 49152 | 8.700 | 0.1202 | [完整代码](reports/full-playthrough/run-731/programs/0109.py) |
| 110 | 生产 胡萝卜 至 19111 | 10.933 | 0.1482 | [完整代码](reports/full-playthrough/run-731/programs/0110.py) |
| 111 | 研究「巨型南瓜」→ 7 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0111.py) |
| 112 | 生产 干草 至 114688 | 21.933 | 0.2488 | [完整代码](reports/full-playthrough/run-731/programs/0112.py) |
| 113 | 生产 木材 至 114688 | 16.983 | 0.2313 | [完整代码](reports/full-playthrough/run-731/programs/0113.py) |
| 114 | 生产 胡萝卜 至 45865 | 31.400 | 0.4034 | [完整代码](reports/full-playthrough/run-731/programs/0114.py) |
| 115 | 研究「巨型南瓜」→ 8 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0115.py) |
| 116 | 生产 干草 至 81920 | 15.883 | 0.1802 | [完整代码](reports/full-playthrough/run-731/programs/0116.py) |
| 117 | 生产 木材 至 81920 | 12.617 | 0.1703 | [完整代码](reports/full-playthrough/run-731/programs/0117.py) |
| 118 | 生产 胡萝卜 至 36045 | 21.167 | 0.3259 | [完整代码](reports/full-playthrough/run-731/programs/0118.py) |
| 119 | 生产 南瓜 至 100 | 24.667 | 0.5502 | [完整代码](reports/full-playthrough/run-731/programs/0119.py) |
| 120 | 研究「肥料与感染」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0120.py) |
| 121 | 生产 干草 至 49152 | 9.650 | 0.1051 | [完整代码](reports/full-playthrough/run-731/programs/0121.py) |
| 122 | 生产 木材 至 49152 | 6.583 | 0.0852 | [完整代码](reports/full-playthrough/run-731/programs/0122.py) |
| 123 | 生产 胡萝卜 至 36045 | 10.933 | 0.1363 | [完整代码](reports/full-playthrough/run-731/programs/0123.py) |
| 124 | 生产 奇异物质 至 30 | 40.867 | 0.9062 | [完整代码](reports/full-playthrough/run-731/programs/0124.py) |
| 125 | 研究「树篱迷宫」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0125.py) |
| 126 | 研究「仙人掌排序」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0126.py) |
| 127 | 生产 仙人掌 至 500 | 4.167 | 0.0292 | [完整代码](reports/full-playthrough/run-731/programs/0127.py) |
| 128 | 研究「长尾探索」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0128.py) |
| 129 | 迷宫生产 宝藏金币 至 100 | 0.050 | 0.0033 | [完整代码](reports/full-playthrough/run-731/programs/0129.py) |
| 130 | 迷宫生产 宝藏金币 至 100 | 0.183 | 0.0043 | [完整代码](reports/full-playthrough/run-731/programs/0130.py) |
| 131 | 研究「并行无人机」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0131.py) |
| 132 | 长尾生产 骨头 至 100 | 2.600 | 0.0313 | [完整代码](reports/full-playthrough/run-731/programs/0132.py) |
| 133 | 研究「模拟实验室」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0133.py) |
| 134 | 迷宫生产 宝藏金币 至 1000 | 0.167 | 0.0044 | [完整代码](reports/full-playthrough/run-731/programs/0134.py) |
| 135 | 迷宫生产 宝藏金币 至 1000 | 0.567 | 0.0148 | [完整代码](reports/full-playthrough/run-731/programs/0135.py) |
| 136 | 迷宫生产 宝藏金币 至 1000 | 0.133 | 0.0032 | [完整代码](reports/full-playthrough/run-731/programs/0136.py) |
| 137 | 迷宫生产 宝藏金币 至 1000 | 0.033 | 0.0013 | [完整代码](reports/full-playthrough/run-731/programs/0137.py) |
| 138 | 迷宫生产 宝藏金币 至 1000 | 0.400 | 0.0098 | [完整代码](reports/full-playthrough/run-731/programs/0138.py) |
| 139 | 迷宫生产 宝藏金币 至 1000 | 0.167 | 0.0052 | [完整代码](reports/full-playthrough/run-731/programs/0139.py) |
| 140 | 迷宫生产 宝藏金币 至 1000 | 0.817 | 0.0188 | [完整代码](reports/full-playthrough/run-731/programs/0140.py) |
| 141 | 迷宫生产 宝藏金币 至 1000 | 0.017 | 0.0005 | [完整代码](reports/full-playthrough/run-731/programs/0141.py) |
| 142 | 迷宫生产 宝藏金币 至 1000 | 0.217 | 0.0068 | [完整代码](reports/full-playthrough/run-731/programs/0142.py) |
| 143 | 迷宫生产 宝藏金币 至 1000 | 0.033 | 0.0011 | [完整代码](reports/full-playthrough/run-731/programs/0143.py) |
| 144 | 迷宫生产 宝藏金币 至 1000 | 0.283 | 0.0080 | [完整代码](reports/full-playthrough/run-731/programs/0144.py) |
| 145 | 迷宫生产 宝藏金币 至 1000 | 0.133 | 0.0035 | [完整代码](reports/full-playthrough/run-731/programs/0145.py) |
| 146 | 迷宫生产 宝藏金币 至 1000 | 0.067 | 0.0013 | [完整代码](reports/full-playthrough/run-731/programs/0146.py) |
| 147 | 迷宫生产 宝藏金币 至 1000 | 0.283 | 0.0068 | [完整代码](reports/full-playthrough/run-731/programs/0147.py) |
| 148 | 迷宫生产 宝藏金币 至 1000 | 0.100 | 0.0028 | [完整代码](reports/full-playthrough/run-731/programs/0148.py) |
| 149 | 迷宫生产 宝藏金币 至 1000 | 0.083 | 0.0022 | [完整代码](reports/full-playthrough/run-731/programs/0149.py) |
| 150 | 研究「本地竞速」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0150.py) |
| 151 | 生产 干草 至 200 | 1.800 | 0.0215 | [完整代码](reports/full-playthrough/run-731/programs/0151.py) |
| 152 | 验证随机伴生目标 | 3.033 | 0.0130 | [完整代码](reports/full-playthrough/run-731/programs/0152.py) |
| 153 | 验证独立工作机与等待 | 3.083 | 0.0553 | [完整代码](reports/full-playthrough/run-731/programs/0153.py) |
| 154 | 验证固定种子模拟 | 0.067 | 0.0043 | [完整代码](reports/full-playthrough/run-731/programs/0154.py) |
| 155 | 研究「灌溉」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0155.py) |
| 156 | 研究「灌溉」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0156.py) |
| 157 | 研究「灌溉」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0157.py) |
| 158 | 研究「灌溉」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0158.py) |
| 159 | 研究「灌溉」→ 6 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0159.py) |
| 160 | 研究「太阳花」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0160.py) |
| 161 | 研究「太阳花」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0161.py) |
| 162 | 研究「太阳花」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0162.py) |
| 163 | 研究「太阳花」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0163.py) |
| 164 | 研究「太阳花」→ 6 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0164.py) |
| 165 | 生产 干草 至 49152 | 6.333 | 0.0843 | [完整代码](reports/full-playthrough/run-731/programs/0165.py) |
| 166 | 生产 木材 至 49152 | 6.650 | 0.1086 | [完整代码](reports/full-playthrough/run-731/programs/0166.py) |
| 167 | 生产 胡萝卜 至 28666 | 10.833 | 0.1452 | [完整代码](reports/full-playthrough/run-731/programs/0167.py) |
| 168 | 研究「太阳花」→ 7 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0168.py) |
| 169 | 生产 干草 至 163840 | 23.583 | 0.2673 | [完整代码](reports/full-playthrough/run-731/programs/0169.py) |
| 170 | 生产 木材 至 163840 | 17.250 | 0.2435 | [完整代码](reports/full-playthrough/run-731/programs/0170.py) |
| 171 | 生产 胡萝卜 至 68798 | 30.583 | 0.3744 | [完整代码](reports/full-playthrough/run-731/programs/0171.py) |
| 172 | 研究「太阳花」→ 8 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0172.py) |
| 173 | 研究「肥料与感染」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0173.py) |
| 174 | 研究「肥料与感染」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0174.py) |
| 175 | 研究「肥料与感染」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0175.py) |
| 176 | 研究「肥料与感染」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0176.py) |
| 177 | 研究「肥料与感染」→ 6 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0177.py) |
| 178 | 研究「伴生种植」→ 1 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0178.py) |
| 179 | 研究「伴生种植」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0179.py) |
| 180 | 生产 干草 至 2880 | 3.250 | 0.0349 | [完整代码](reports/full-playthrough/run-731/programs/0180.py) |
| 181 | 研究「伴生种植」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0181.py) |
| 182 | 研究「伴生种植」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0182.py) |
| 183 | 生产 干草 至 16589 | 3.250 | 0.0342 | [完整代码](reports/full-playthrough/run-731/programs/0183.py) |
| 184 | 研究「伴生种植」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0184.py) |
| 185 | 生产 干草 至 39814 | 6.367 | 0.0696 | [完整代码](reports/full-playthrough/run-731/programs/0185.py) |
| 186 | 研究「伴生种植」→ 6 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0186.py) |
| 187 | 研究「仙人掌排序」→ 2 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0187.py) |
| 188 | 研究「仙人掌排序」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0188.py) |
| 189 | 研究「仙人掌排序」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0189.py) |
| 190 | 研究「仙人掌排序」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0190.py) |
| 191 | 研究「仙人掌排序」→ 6 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0191.py) |
| 192 | 生产 干草 至 81920 | 12.333 | 0.1331 | [完整代码](reports/full-playthrough/run-731/programs/0192.py) |
| 193 | 生产 木材 至 81920 | 3.417 | 0.0441 | [完整代码](reports/full-playthrough/run-731/programs/0193.py) |
| 194 | 生产 胡萝卜 至 36045 | 17.867 | 0.2286 | [完整代码](reports/full-playthrough/run-731/programs/0194.py) |
| 195 | 生产 南瓜 至 76442 | 36.800 | 0.7614 | [完整代码](reports/full-playthrough/run-731/programs/0195.py) |
| 196 | 研究「仙人掌排序」→ 7 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0196.py) |
| 197 | 生产 干草 至 131072 | 18.183 | 0.1896 | [完整代码](reports/full-playthrough/run-731/programs/0197.py) |
| 198 | 生产 木材 至 131072 | 4.633 | 0.0580 | [完整代码](reports/full-playthrough/run-731/programs/0198.py) |
| 199 | 生产 胡萝卜 至 72090 | 22.817 | 0.2697 | [完整代码](reports/full-playthrough/run-731/programs/0199.py) |
| 200 | 生产 南瓜 至 183459 | 55.350 | 1.1374 | [完整代码](reports/full-playthrough/run-731/programs/0200.py) |
| 201 | 研究「仙人掌排序」→ 8 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0201.py) |
| 202 | 研究「长尾探索」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0202.py) |
| 203 | 生产 仙人掌 至 2880 | 4.233 | 0.0277 | [完整代码](reports/full-playthrough/run-731/programs/0203.py) |
| 204 | 研究「长尾探索」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0204.py) |
| 205 | 研究「长尾探索」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0205.py) |
| 206 | 研究「长尾探索」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0206.py) |
| 207 | 研究「长尾探索」→ 6 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0207.py) |
| 208 | 研究「长尾探索」→ 7 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0208.py) |
| 209 | 研究「长尾探索」→ 8 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0209.py) |
| 210 | 研究「树篱迷宫」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0210.py) |
| 211 | 研究「树篱迷宫」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0211.py) |
| 212 | 研究「树篱迷宫」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0212.py) |
| 213 | 研究「树篱迷宫」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0213.py) |
| 214 | 研究「树篱迷宫」→ 6 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0214.py) |
| 215 | 研究「树篱迷宫」→ 7 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0215.py) |
| 216 | 研究「树篱迷宫」→ 8 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0216.py) |
| 217 | 生产 干草 至 32768 | 4.950 | 0.0526 | [完整代码](reports/full-playthrough/run-731/programs/0217.py) |
| 218 | 生产 胡萝卜 至 36045 | 4.700 | 0.0547 | [完整代码](reports/full-playthrough/run-731/programs/0218.py) |
| 219 | 生产 奇异物质 至 1024 | 21.283 | 0.4586 | [完整代码](reports/full-playthrough/run-731/programs/0219.py) |
| 220 | 迷宫生产 宝藏金币 至 240 | 0.250 | 0.0061 | [完整代码](reports/full-playthrough/run-731/programs/0220.py) |
| 221 | 研究「并行无人机」→ 2 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0221.py) |
| 222 | 研究「并行无人机」→ 3 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0222.py) |
| 223 | 研究「并行无人机」→ 4 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0223.py) |
| 224 | 研究「并行无人机」→ 5 | 0.017 | 0.0000 | [完整代码](reports/full-playthrough/run-731/programs/0224.py) |
| 225 | 研究「农田扩张」→ 4 | 0.017 | 0.0002 | [完整代码](reports/full-playthrough/run-731/programs/0225.py) |
| 226 | 研究「农田扩张」→ 5 | 0.017 | 0.0009 | [完整代码](reports/full-playthrough/run-731/programs/0226.py) |
| 227 | 长尾生产 骨头 至 10000 | 373.967 | 8.6652 | [完整代码](reports/full-playthrough/run-731/programs/0227.py) |
| 228 | 研究「自治信标」→ 1 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0228.py) |
| 229 | 迷宫生产 宝藏金币 至 100000 | 3.083 | 0.2788 | [完整代码](reports/full-playthrough/run-731/programs/0229.py) |
| 230 | 研究「大师礼帽」→ 1 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0230.py) |
| 231 | 验证大师礼帽外观接口 | 0.017 | 0.0001 | [完整代码](reports/full-playthrough/run-731/programs/0231.py) |
| 232 | 验证迷宫复用与开墙 | 15.567 | 1.7294 | [完整代码](reports/full-playthrough/run-731/programs/0232.py) |
| 233 | 验证 32 台工作机同时生产 | 11.750 | 2.2087 | [完整代码](reports/full-playthrough/run-731/programs/0233.py) |
| 234 | 本地竞速 Hay | 0.017 | 0.0349 | [完整代码](reports/full-playthrough/run-731/programs/0234.py) |
| 235 | 单机竞速 Hay | 0.017 | 0.0340 | [完整代码](reports/full-playthrough/run-731/programs/0235.py) |
| 236 | 本地竞速 Wood | 0.017 | 0.0655 | [完整代码](reports/full-playthrough/run-731/programs/0236.py) |
| 237 | 单机竞速 Wood | 0.017 | 0.0626 | [完整代码](reports/full-playthrough/run-731/programs/0237.py) |
| 238 | 本地竞速 Carrots | 0.017 | 0.0776 | [完整代码](reports/full-playthrough/run-731/programs/0238.py) |
| 239 | 单机竞速 Carrots | 0.017 | 0.0754 | [完整代码](reports/full-playthrough/run-731/programs/0239.py) |
| 240 | 本地竞速 Pumpkins | 0.017 | 0.6041 | [完整代码](reports/full-playthrough/run-731/programs/0240.py) |
| 241 | 单机竞速 Pumpkins | 0.017 | 0.5933 | [完整代码](reports/full-playthrough/run-731/programs/0241.py) |
| 242 | 本地竞速 Sunflowers | 0.017 | 0.1335 | [完整代码](reports/full-playthrough/run-731/programs/0242.py) |
| 243 | 单机竞速 Sunflowers | 0.017 | 0.1313 | [完整代码](reports/full-playthrough/run-731/programs/0243.py) |
| 244 | 本地竞速 Cactus | 0.017 | 0.0329 | [完整代码](reports/full-playthrough/run-731/programs/0244.py) |
| 245 | 单机竞速 Cactus | 0.017 | 0.0345 | [完整代码](reports/full-playthrough/run-731/programs/0245.py) |
| 246 | 本地竞速 Maze | 0.017 | 0.0027 | [完整代码](reports/full-playthrough/run-731/programs/0246.py) |
| 247 | 单机竞速 Maze | 0.017 | 0.0027 | [完整代码](reports/full-playthrough/run-731/programs/0247.py) |
| 248 | 本地竞速 Dinosaur | 0.017 | 0.0327 | [完整代码](reports/full-playthrough/run-731/programs/0248.py) |
| 249 | 最快重置完整竞速 | 0.017 | 2.6879 | [完整代码](reports/full-playthrough/run-731/programs/0249.py) |

## 自动复跑

关闭此新版工程的 Editor，使用项目匹配的 Unity 2022.3.62f3c1。命令不加 `-quit`：启动器进入 Play 模式、等待本地化就绪，再自行退出。输出是独立目录，不读取或覆盖玩家农场。

```bash
VERDANT_PLAYTHROUGH_SEED=731 \
VERDANT_PLAYTHROUGH_OUTPUT="/absolute/path/to/test-output" \
"$VERDANT_UNITY_EDITOR" -batchmode \
  -projectPath "/absolute/path/to/verdant-circuit" \
  -executeMethod VerdantFullPlaythrough.Run \
  -logFile "/absolute/path/to/test-output.log"
```

被测 Automation 源码 SHA-256：`991ac1b7f0bb2175fd560eeed2a19e3b33aa33d07a298e2cb8e0e384c6d57a6b`。[被测源码快照](reports/full-playthrough/runtime-source/)与[测试调度器快照](reports/full-playthrough/test-runner/)已包含在资料包中，避免后续代码改变后误用这次时间。
