# 禾序 0.11.3：完整流程测试代码与操作手册

适用：Verdant Circuit / 禾序 **0.11.3**（沿用 0.11.2 的三小时平衡规则）。这是在游戏内运行的类 Python 程序；不要放到 Unity 的 Assets 中，也不要用系统 Python 执行。

本文件包含全部 **25 个程序的完整代码**，覆盖零资源开局、34 项科技全部满级（120 个付费等级）、7 项机制验证及 16 类本地竞速。先完成手动开局，再运行自动控制器。所有资源正常生产、研究正常付费，保持“允许未解锁语法”关闭。

## 1. 准备与导入

1. 打开 0.11.3，在农场库新建一个测试农场，保留自己的原存档。应从 1×1、零库存、零研究开始。
2. 测平衡时使用设置中的 **1 倍世界速度**；快速检查可用游戏提供的更高倍率。科技中的执行速度升级仍正常生效。本文的时间是模拟游戏时间，人工读代码、编辑和点击的时间另外计算。
3. 推荐批量导入：设置 → 外部编辑器 → **导出并监视 / Export and watch**。找到设置中显示的当前农场程序文件夹，把压缩包 `programs` 里的全部 `.py` 文件复制进去，等日志出现“已重新载入外部代码”。不要替换存档；保留游戏生成的 `__builtins__.py`。
4. 导入后若仍停留在主编辑器，点一下别处，再打开程序库确认 `boot`、`farm_lib`、`campaign`、`races` 等文件都已出现。不要同时在游戏与外部编辑器修改同一个文件。
5. 也可逐个新建程序并粘贴本文代码。程序名称必须和小标题一致，**新建名称不填 `.py`**，缩进保留四个空格。

导入未解锁的代码本身没有影响；提前运行会提示需要研究。只运行步骤指定的程序。`farm_lib`、`experiment`、`race_*` 是其他程序调用的模块，不用单独点击运行。

每一步完成后先关闭任务完成提示，再启动下一步。主运行按钮会替换当前控制程序及其工作机；测试期间不要在浮动窗口另外启动生产。切换查看其他文件不会停止当前程序。开始下一项测试前，确认上一项已自然结束或点击“全部停止”。

## 2. 零资源开局：严格按表操作

下表的目标数是**当前库存**，不是累计产量。科技名称同时给出代码枚举，方便中英文界面对照。每一行研究都需要手动在科技树点击，先后顺序照表执行；不要提前购买额外升级。

| 步骤 | 运行代码 / 手动操作 | 完成条件与下一步 |
|---|---|---|
| 01 | 打开 `boot`，反复点击运行 | 草成熟后才能收获。攒到 **5 干草库存**，研究循环 `Unlocks.Loops`。未成熟时运行没有收获，等待后再点。 |
| 02 | 运行 `start` | 干草库存达到 **60** 后手动停止。 |
| 03 | 依次研究运算符 `Operators`、变量 `Variables`、传感器 `Senses`、种植 `Plant`、扩田 `Expand` 第1级 | 总支出48干草，农田变为 **2×2**。 |
| 04 | `hay` 第一行目标改成 `target = 50`，运行到自动结束 | 研究干草增产 `Grass` 第1级，支付50干草。 |
| 05 | `hay` 改为 `target = 200`，运行到自动结束 | 保留这些干草，先不买其他科技。 |
| 06 | `wood` 改为 `target = 30`，运行到自动结束 | 此时未解锁树，程序种免费灌木。累计干草已超过林木发现门槛。 |
| 07 | 依次研究林木 `Trees` 第1级、扩田 `Expand` 第2级、干草增产 `Grass` 第2级、执行速度 `Speed` 第1级 | 农田变为 **4×4**；干草支出24+145+20=189。 |
| 08 | `wood` 改为 `target = 900`，运行到自动结束 | 树与灌木棋盘种植，避免相邻树生长减速。 |
| 09 | 依次研究函数 `Functions`、列表 `Lists`、成本查询 `Costs`、计时 `Timing`、帽子 `Hats`、胡萝卜 `Carrots` 第1级、浇水 `Watering` 第1级 | 共245木材；此时累计木材已达到胡萝卜发现要求600。 |
| 10 | `hay` 改为 `target = 500`，运行到自动结束 | 准备胡萝卜种子原料。 |
| 11 | 运行 `carrot`，目标保持160 | 库存达到 **至少160胡萝卜** 后自动结束。每株消耗2干草+2木材；开始前应有至少400干草与400木材。 |
| 12 | 依次研究字典 `Dicts`、导入 `Import`、自动解锁 `Auto_Unlock`、调试 `Debug`、高级调试 `Debug_2`、实用工具 `Utilities` | 消耗140胡萝卜、5干草、160木材。开局脚本所需语言与观察工具齐全。 |
| 13 | 打开 `campaign`，点击运行 | `farm_lib` 必须已导入。接下来无需手动购买科技。 |

`hay`、`wood` 每次运行都会清田；只在指定阶段运行。`carrot` 会检查整轮种植预算，原料不足会结束：补足干草和木材后重跑本步即可，不必重开农场。

## 3. 全科技自动推进

运行 `campaign` 后，控制器会按正常科技条件生产、支付和升级，依次推进伴生、南瓜、肥料、向日葵、仙人掌、迷宫、长尾、多机、模拟、竞速、遗物与礼帽。无需人工轮换每种作物代码。

- 扩田遵循 **2×2 → 4×4 → 8×8 → 16×16 → 32×32**，达到对应作物发现后才购买。
- 首次发现需要累计产量：林木160干草、胡萝卜600木材、南瓜1500胡萝卜、向日葵8000南瓜、仙人掌6000能量、迷宫30000仙人掌、长尾30000金币。
- 每次生产先准备种子成本，草和木材可免费恢复；使用真实库存及 `get_cost()` 付费。水、肥料随已购研究自然补充。
- 控制台出现 `discovery` 表示完成一个发现阶段。运行中可能长时间没有新日志，观察累计产量、地块、无人机位置是否变化。
- 最终输出 **`campaign_complete True`** 与 **`all_research_maxed True`**，程序自然结束。科技树应有34项满级，合计120级；农田32×32，无人机容量32。

此控制器以单控制程序的串行生产为主，便于复现和排查。多机满负载另有专门验证。不同算法、随机种子、反复清田和人工停顿都会影响总时间；三小时是平衡基准路线的目标，不保证所有写法恰好用时三小时。

### 生产算法说明

| 系统 | 控制器采用的算法与观察点 |
|---|---|
| 干草 | 全田循环扫描，只收成熟草；草自然再生。 |
| 木材 | 树/灌木棋盘格，检查空格后再播种，防止树四邻减速。 |
| 胡萝卜 | 先按整轮播种计算干草和木材需求，再逐格收获补种。 |
| 南瓜 | 全田耕地，修补枯死南瓜，逐格催熟；全部成熟后合并收获。感染比例参与奇异物质产出。 |
| 向日葵 | 先测量全田花瓣，再优先收高花瓣，减少能量惩罚；能量会随指令执行消耗。 |
| 仙人掌 | 逐行、逐列比较和交换，建立东/北非递减排列，再触发连锁收获。 |
| 奇异物质 | 在有产量的作物上催熟感染，再收获；并非仅添加肥料就直接入库。 |
| 迷宫金币 | 批量备齐奇异物质，生成迷宫，用已访问集合和路径栈做深度优先搜索，遇死路回溯，找到宝藏再收获。 |
| 长尾骨头 | 偶数边长上的 Hamilton 回路：保留边缘通道，蛇形遍历，避免自身封路；消耗仙人掌生成苹果，最后换回草帽结算。 |
| 自动研究 | 递归补齐前置发现、累计产量、材料；扩田/执行速度/增产逐级检查作物发现。 |

## 4. 通关后的七项机制测试

等 `campaign` 完全结束后，按下列顺序运行。可单独重跑；这些测试会清田并消耗真实库存，建议先备份测试农场。

| 顺序 | 运行程序 | 预期结果 |
|---|---|---|
| 14 | `verify_companion` | `companion_ok True`；实际随机伴生搭配带来额外干草产量。 |
| 15 | `verify_maze` | `maze_reuse_ok True`；生成并复用迷宫，两次宝藏的金币增量符合当前等级。 |
| 16 | `verify_drones` | 瞬时达到32台活跃工作机；最后 `parallel_complete True capacity 32`，子机均已等待回收。显示本程序还剩主机1台是正常的。 |
| 17 | `verify_simulation` | 同种子7重复结果相同，种子8结果不同；`simulation_ok True`。需保留模块 `experiment`。 |
| 18 | `verify_tail` | `tail_ok True` 并打印 `bone_gain` 正值；实际走满长尾回路并结算骨头。 |
| 19 | `verify_hat` | `hat_ok True`；礼帽切换成功，然后换回草帽。 |
| 20 | `verify_debug` | `temporary_size_ok True` 与指令数；运行时临时2×2，**自然结束后恢复32×32**及正常执行限制。 |

## 5. 全部16类本地竞速

运行 `races`。保留所有9个 `race_*` 模块和 `experiment`，不单独启动它们。程序逐项输出 `challenge_start` / `challenge_passed`，最终输出 **`all_16_challenges_finished True`**。

| 资源 / 类型 | 竞速类别 | 被调用模块 |
|---|---|---|
| 干草 | Hay / Hay_Single | race_hay |
| 木材 | Wood / Wood_Single | race_wood |
| 胡萝卜 | Carrots / Carrots_Single | race_carrots |
| 南瓜 | Pumpkins / Pumpkins_Single | race_pumpkins |
| 向日葵 | Sunflowers / Sunflowers_Single | race_sunflowers |
| 仙人掌 | Cactus / Cactus_Single | race_cactus |
| 迷宫 | Maze / Maze_Single | race_maze |
| 长尾 | Dinosaur | race_dinosaur |
| 重置通关 | Fastest_Reset | race_reset |

竞速与模拟使用独立农场及独立计时，不会扣掉主农场库存。资源竞速按游戏预设的8×8中阶研究运行；单机类别限制1台。Fastest_Reset 从自己的零库存预设出发，允许预设语法，目标为获得遗物。`race_reset` 是竞速程序，**普通零解锁开局请使用前面的手动步骤**。

模拟/竞速调用最后的64是独立模拟加速参数，和主农场设置倍率是两件事。查看本地竞速列表：新测试农场应产生16类纪录。完成整套流程后，成就应为 **13/13**。

## 6. 异常排查与内部验收

- `Research needed`：运行阶段提前了，或步骤12尚未完成；查看报错对应的研究，不要开未解锁语法来掩盖漏项。
- `Module not found`：检查同一测试农场中是否有 `farm_lib`、`experiment` 和9个 `race_*` 程序，文件名不要加空格或改大小写。
- 点击运行无反应：先关闭完成提示、退出暂停/单步状态；确认点击的是目标程序的主运行按钮。
- 水满却不浇水：脚本仅在水分低于0.3且有水道具时浇水。水道具库存的自动补充与地块含水量不是同一数值。
- 数量下降：研究、播种、迷宫、苹果与能量执行都有真实消耗；查看累计生产而不只看当前库存。控制器可能先生产原料再生产目标资源。
- 正在运行却想看其他代码：可以切换查看；不要点另一个文件的运行按钮，否则会替换现有任务。
- 本套验证只覆盖当前实现：无人机共享农场与资源、变量独立；不包含尚未实现的不同尺寸实体机体、实体碰撞/扣款及浮空岛。请勿将这些旧设计需求当作当前版本已有功能。

内部测试记录建议：种子（若可见）、系统、游戏版本、世界倍率、每步开始/结束时钟、研究等级、预期与实际输出、异常截图或报错行号。流程为：零库存 → 正常开局 → 全科技 → 七机制 → 十六竞速 → 检查13成就。界面、音量、撤销、中文/数字程序名称等操作回归需要另外人工检查，自动脚本不能替代界面验收。

## 7. 本次实际验证记录

验证状态：**通过**。种子 `731`，Unity `2022.3.62f3c1`，使用游戏真实解释器，固定每帧1/60秒；从零库存和零研究开始，未赠物资，未开放锁定语法。

- 全科技满级：主农场游戏时钟 **02:55:46**（10546.483 秒）。
- 加上机制与竞速驱动后的主农场时钟：**03:04:13**。独立竞速耗时另列，不与主农场相加。
- 34/34科技满级，13/13成就，16/16竞速纪录；32台工作机峰值通过验证。
- 这里是实际逻辑运行时间；加速验证不渲染交互界面，不包含人工点击、编辑或阅读时间。

| 阶段 | 主农场开始 | 主农场结束 | 主农场耗时 |
|---|---:|---:|---:|
| 01 · 重复运行 boot，收获5份干草 | 00:00:00 | 00:00:16 | 00:00:16 |
| 02 · 手动研究 loops → 1 | 00:00:16 | 00:00:16 | 00:00:00 |
| 03 · 运行 start，干草库存60时停止 | 00:00:16 | 00:03:18 | 00:03:02 |
| 04 · 手动研究 operators → 1 | 00:03:18 | 00:03:18 | 00:00:00 |
| 05 · 手动研究 variables → 1 | 00:03:18 | 00:03:18 | 00:00:00 |
| 06 · 手动研究 senses → 1 | 00:03:18 | 00:03:18 | 00:00:00 |
| 07 · 手动研究 plant → 1 | 00:03:18 | 00:03:18 | 00:00:00 |
| 08 · 手动研究 expand → 1 | 00:03:18 | 00:03:18 | 00:00:00 |
| 09 · 运行玩家程序 hay，target=50 | 00:03:18 | 00:04:14 | 00:00:56 |
| 10 · 手动研究 grass → 1 | 00:04:14 | 00:04:14 | 00:00:00 |
| 11 · 运行玩家程序 hay，target=200 | 00:04:14 | 00:06:27 | 00:02:14 |
| 12 · 运行玩家程序 wood，target=30 | 00:06:27 | 00:07:37 | 00:01:09 |
| 13 · 手动研究 trees → 1 | 00:07:37 | 00:07:37 | 00:00:00 |
| 14 · 手动研究 expand → 2 | 00:07:37 | 00:07:37 | 00:00:00 |
| 15 · 手动研究 grass → 2 | 00:07:37 | 00:07:37 | 00:00:00 |
| 16 · 手动研究 speed → 1 | 00:07:37 | 00:07:37 | 00:00:00 |
| 17 · 运行玩家程序 wood，target=900 | 00:07:37 | 00:12:46 | 00:05:09 |
| 18 · 手动研究 functions → 1 | 00:12:46 | 00:12:46 | 00:00:00 |
| 19 · 手动研究 lists → 1 | 00:12:46 | 00:12:46 | 00:00:00 |
| 20 · 手动研究 costs → 1 | 00:12:46 | 00:12:46 | 00:00:00 |
| 21 · 手动研究 timing → 1 | 00:12:46 | 00:12:46 | 00:00:00 |
| 22 · 手动研究 hats → 1 | 00:12:46 | 00:12:46 | 00:00:00 |
| 23 · 手动研究 carrots → 1 | 00:12:46 | 00:12:46 | 00:00:00 |
| 24 · 手动研究 watering → 1 | 00:12:46 | 00:12:46 | 00:00:00 |
| 25 · 运行玩家程序 hay，target=500 | 00:12:46 | 00:14:20 | 00:01:34 |
| 26 · 运行玩家程序 carrot | 00:14:20 | 00:17:15 | 00:02:55 |
| 27 · 手动研究 dicts → 1 | 00:17:15 | 00:17:15 | 00:00:00 |
| 28 · 手动研究 import → 1 | 00:17:15 | 00:17:15 | 00:00:00 |
| 29 · 手动研究 auto_unlock → 1 | 00:17:15 | 00:17:15 | 00:00:00 |
| 30 · 手动研究 debug → 1 | 00:17:15 | 00:17:15 | 00:00:00 |
| 31 · 手动研究 debug2 → 1 | 00:17:15 | 00:17:15 | 00:00:00 |
| 32 · 手动研究 utilities → 1 | 00:17:15 | 00:17:15 | 00:00:00 |
| 33 · 运行玩家程序 campaign | 00:17:15 | 02:55:46 | 02:38:31 |
| 34 · 运行玩家程序 verify_companion | 02:55:46 | 02:55:50 | 00:00:03 |
| 35 · 运行玩家程序 verify_maze | 02:55:50 | 02:57:48 | 00:01:58 |
| 36 · 运行玩家程序 verify_drones | 02:57:48 | 02:58:00 | 00:00:12 |
| 37 · 运行玩家程序 verify_simulation | 02:58:00 | 02:58:00 | 00:00:00 |
| 38 · 运行玩家程序 verify_tail | 02:58:00 | 03:04:11 | 00:06:11 |
| 39 · 运行玩家程序 verify_hat | 03:04:11 | 03:04:11 | 00:00:00 |
| 40 · 运行玩家程序 verify_debug | 03:04:11 | 03:04:12 | 00:00:00 |
| 41 · 运行玩家程序 races | 03:04:12 | 03:04:13 | 00:00:02 |

独立竞速记录（与主农场时间分开）：

| 类别 | 独立耗时 |
|---|---:|
| Hay | 64.417 秒 |
| Hay_Single | 64.417 秒 |
| Wood | 81.983 秒 |
| Wood_Single | 81.983 秒 |
| Carrots | 102.733 秒 |
| Carrots_Single | 102.733 秒 |
| Pumpkins | 241.383 秒 |
| Pumpkins_Single | 241.383 秒 |
| Sunflowers | 288.317 秒 |
| Sunflowers_Single | 288.317 秒 |
| Cactus | 431.750 秒 |
| Cactus_Single | 431.750 秒 |
| Maze | 112.617 秒 |
| Maze_Single | 112.617 秒 |
| Dinosaur | 457.383 秒 |
| Fastest_Reset | 7304.650 秒 |

原始记录：压缩包内 `validation/report.json`。逐阶段文件可用于对照预期，而非直接覆盖自己的存档。

## 8. 全部25个程序：可直接复制

下列每节对应一个独立游戏程序。不要把25段代码拼成一个文件。代码保留英文标识，适用于中英文界面。

### boot

```python
harvest()
```

### start

```python
while True:
    harvest()
```

### hay

```python
# Change target to 50, 200 or 500 as directed in the guide.
target = 500
clear()
while num_items(Items.Hay) < target:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)
```

### wood

```python
# Use target 30 before Trees; use 900 after the 4x4 expansion.
target = 900
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood) < target:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type() == None:
                if num_unlocked(Unlocks.Trees) > 0 and (get_pos_x()+get_pos_y())%2 == 0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)
```

### carrot

```python
# Start with at least 400 Hay and 400 Wood; target is inventory.
target = 160
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
n = get_world_size()
while num_items(Items.Carrot) < target and num_items(Items.Hay) >= 2*n*n and num_items(Items.Wood) >= 2*n*n:
    for row in range(n):
        for col in range(n):
            if can_harvest():
                harvest()
            if get_entity_type() == None:
                plant(Entities.Carrot)
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)
```

### farm_lib

```python
# Shared library. Import it; do not run it as the campaign.
tiers = {Unlocks.Expand: [Unlocks.Plant, Unlocks.Trees, Unlocks.Pumpkins, Unlocks.Cactus, Unlocks.Dinosaurs], Unlocks.Speed: [Unlocks.Trees, Unlocks.Carrots, Unlocks.Pumpkins, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm], Unlocks.Grass: [Unlocks.Plant, Unlocks.Trees, Unlocks.Carrots, Unlocks.Pumpkins, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs], Unlocks.Trees: [None, Unlocks.Carrots, Unlocks.Pumpkins, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm], Unlocks.Carrots: [None, Unlocks.Pumpkins, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation], Unlocks.Pumpkins: [None, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation, Unlocks.Leaderboard], Unlocks.Sunflowers: [None, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation, Unlocks.Leaderboard, Unlocks.The_Farmers_Remains], Unlocks.Cactus: [None, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation, Unlocks.Leaderboard, Unlocks.The_Farmers_Remains, Unlocks.Top_Hat], Unlocks.Mazes: [None, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation, Unlocks.Leaderboard, Unlocks.The_Farmers_Remains, Unlocks.Top_Hat, Unlocks.Top_Hat], Unlocks.Dinosaurs: [None, Unlocks.Megafarm, Unlocks.Simulation, Unlocks.Leaderboard, Unlocks.The_Farmers_Remains, Unlocks.Top_Hat, Unlocks.Top_Hat, Unlocks.Top_Hat], Unlocks.Watering: [None, Unlocks.Pumpkins, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs], Unlocks.Fertilizer: [None, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation], Unlocks.Polyculture: [None, Unlocks.Pumpkins, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs], Unlocks.Megafarm: [Unlocks.Dinosaurs, Unlocks.Dinosaurs, Unlocks.Simulation, Unlocks.Leaderboard, Unlocks.The_Farmers_Remains]}
dependencies = {Unlocks.Plant: [], Unlocks.Expand: [Unlocks.Plant], Unlocks.Speed: [Unlocks.Expand, Unlocks.Trees], Unlocks.Grass: [Unlocks.Plant], Unlocks.Trees: [Unlocks.Plant], Unlocks.Carrots: [Unlocks.Expand], Unlocks.Watering: [Unlocks.Carrots], Unlocks.Pumpkins: [Unlocks.Carrots], Unlocks.Sunflowers: [Unlocks.Watering, Unlocks.Pumpkins], Unlocks.Fertilizer: [Unlocks.Pumpkins], Unlocks.Polyculture: [Unlocks.Trees], Unlocks.Cactus: [Unlocks.Pumpkins, Unlocks.Sunflowers], Unlocks.Mazes: [Unlocks.Fertilizer, Unlocks.Cactus], Unlocks.Dinosaurs: [Unlocks.Cactus, Unlocks.Mazes], Unlocks.Megafarm: [Unlocks.Mazes, Unlocks.Dinosaurs], Unlocks.Simulation: [Unlocks.Dinosaurs], Unlocks.Leaderboard: [Unlocks.Simulation], Unlocks.Loops: [], Unlocks.Operators: [Unlocks.Loops], Unlocks.Variables: [Unlocks.Operators], Unlocks.Functions: [Unlocks.Variables], Unlocks.Lists: [Unlocks.Functions], Unlocks.Dicts: [Unlocks.Lists], Unlocks.Import: [Unlocks.Functions], Unlocks.Senses: [], Unlocks.Costs: [], Unlocks.Auto_Unlock: [], Unlocks.Timing: [], Unlocks.Debug: [], Unlocks.Hats: [], Unlocks.Debug_2: [Unlocks.Debug], Unlocks.Utilities: [Unlocks.Operators], Unlocks.The_Farmers_Remains: [Unlocks.Leaderboard], Unlocks.Top_Hat: [Unlocks.The_Farmers_Remains]}
goals = {Unlocks.Trees: (Items.Hay, 160), Unlocks.Carrots: (Items.Wood, 600), Unlocks.Pumpkins: (Items.Carrot, 1500), Unlocks.Sunflowers: (Items.Pumpkin, 8000), Unlocks.Cactus: (Items.Power, 6000), Unlocks.Mazes: (Items.Cactus, 30000), Unlocks.Dinosaurs: (Items.Gold, 30000)}

def buy(tech):
    if num_unlocked(tech) == 0:
        for parent in dependencies[tech]:
            if num_unlocked(parent) == 0:
                buy(parent)
        if tech in goals:
            item, target = goals[tech]
            missing = target - num_produced(item)
            if missing > 0:
                ensure(item, num_items(item) + missing)
    cost = get_cost(tech)
    if cost == None:
        return
    for item in cost:
        ensure(item, cost[item])
    unlock(tech)

def fund(entity, amount):
    cost = get_cost(entity)
    for item in cost:
        ensure(item, cost[item] * amount)

def ensure(item, target):
    if num_items(item) >= target:
        return
    n = get_world_size()
    if item == Items.Hay:
        clear()
        while num_items(Items.Hay) < target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    move(East)
                move(North)
    elif item == Items.Wood:
        clear()
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                till()
                move(East)
            move(North)
        while num_items(Items.Wood) < target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    if get_entity_type() == None:
                        if num_unlocked(Unlocks.Trees) > 0 and (get_pos_x() + get_pos_y()) % 2 == 0:
                            plant(Entities.Tree)
                        else:
                            plant(Entities.Bush)
                    move(East)
                move(North)
    elif item == Items.Carrot:
        if num_unlocked(Unlocks.Carrots) == 0:
            buy(Unlocks.Carrots)
        amount = (int((target - num_items(item)) / (n * n * 2 ** (num_unlocked(Unlocks.Carrots) - 1))) + 3) * n * n
        fund(Entities.Carrot, amount)
        clear()
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                till()
                move(East)
            move(North)
        while num_items(Items.Carrot) < target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    if get_entity_type() == None:
                        plant(Entities.Carrot)
                    move(East)
                move(North)
    elif item == Items.Pumpkin or item == Items.Weird_Substance:
        if num_unlocked(Unlocks.Pumpkins) == 0:
            buy(Unlocks.Pumpkins)
        infected = item == Items.Weird_Substance
        if infected and num_unlocked(Unlocks.Fertilizer) == 0:
            buy(Unlocks.Fertilizer)
        yield_amount = n * n * min(n, 6) * 2 ** (num_unlocked(Unlocks.Pumpkins) - 1)
        if infected:
            yield_amount = yield_amount / 2
        amount = (int((target - num_items(item)) / yield_amount) + 3) * n * n * 3
        fund(Entities.Pumpkin, amount)
        if infected:
            clear()
            while num_items(Items.Weird_Substance) < target:
                complete = True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_ground_type() != Grounds.Soil:
                            till()
                        if get_entity_type() == None or get_entity_type() == Entities.Dead_Pumpkin:
                            plant(Entities.Pumpkin)
                        if get_entity_type() != Entities.Pumpkin or not can_harvest():
                            complete = False
                        if get_water() < 0.3 and num_items(Items.Water) > 0:
                            use_item(Items.Water)
                        move(East)
                    move(North)
                if complete:
                    per_plant = min(get_world_size(), 6) * 2 ** (num_unlocked(Unlocks.Pumpkins) - 1) / 2
                    infected = min(get_world_size() * get_world_size(), int((target - num_items(Items.Weird_Substance) - 1) / per_plant) + 1)
                    for row in range(get_world_size()):
                        for col in range(get_world_size()):
                            if infected > 0:
                                while num_items(Items.Fertilizer) == 0:
                                    pass
                                use_item(Items.Fertilizer)
                                infected -= 1
                            move(East)
                        move(North)
                    harvest()
        else:
            clear()
            while num_items(Items.Pumpkin) < target:
                complete = True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_ground_type() != Grounds.Soil:
                            till()
                        if get_entity_type() == None or get_entity_type() == Entities.Dead_Pumpkin:
                            plant(Entities.Pumpkin)
                        if get_entity_type() != Entities.Pumpkin or not can_harvest():
                            complete = False
                        if get_water() < 0.3 and num_items(Items.Water) > 0:
                            use_item(Items.Water)
                        move(East)
                    move(North)
                if complete:
                    harvest()
    elif item == Items.Power:
        if num_unlocked(Unlocks.Sunflowers) == 0:
            buy(Unlocks.Sunflowers)
        amount = (int((target - num_items(item)) / (n * n * 2 ** (num_unlocked(Unlocks.Sunflowers) - 1))) + 3) * n * n
        fund(Entities.Sunflower, amount)
        clear()
        while num_items(Items.Power) < target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if get_ground_type() != Grounds.Soil:
                        till()
                    if get_entity_type() == None:
                        plant(Entities.Sunflower)
                    move(East)
                move(North)
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    while not can_harvest():
                        pass
                    move(East)
                move(North)
            for petals in range(15, 6, -1):
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_entity_type() == Entities.Sunflower and measure() == petals:
                            harvest()
                        move(East)
                    move(North)
    elif item == Items.Cactus:
        if num_unlocked(Unlocks.Cactus) == 0:
            buy(Unlocks.Cactus)
        amount = (int((target - num_items(item)) / (n * n * min(n * n, 16) * 2 ** (num_unlocked(Unlocks.Cactus) - 1))) + 3) * n * n
        fund(Entities.Cactus, amount)
        clear()
        while num_items(Items.Cactus) < target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if get_ground_type() != Grounds.Soil:
                        till()
                    if get_entity_type() == None:
                        plant(Entities.Cactus)
                    move(East)
                move(North)
            complete = False
            while not complete:
                complete = True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if not can_harvest():
                            complete = False
                        move(East)
                    move(North)
            for axis in [East, North]:
                for sweep in range(get_world_size()):
                    for row in range(get_world_size()):
                        for col in range(get_world_size() - 1):
                            if measure() > measure(axis):
                                swap(axis)
                            move(axis)
                        move(axis)
                        if axis == East:
                            move(North)
                        else:
                            move(East)
            harvest()
    elif item == Items.Bone:
        while num_items(Items.Bone) < target:
            ensure(Items.Cactus, n * n + 1)
            clear()
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    till()
                    move(East)
                move(North)
            change_hat(Hats.Dinosaur_Hat)
            while True:
                direction = East
                if get_pos_x() == 0:
                    if get_pos_y() > 0:
                        direction = South
                elif get_pos_y() % 2 == 0:
                    if get_pos_x() == get_world_size() - 1:
                        direction = North
                else:
                    direction = West
                    if get_pos_x() == 1 and get_pos_y() != get_world_size() - 1:
                        direction = North
                if not move(direction):
                    break
            change_hat(Hats.Straw_Hat)
    elif item == Items.Gold:
        scale = 2 ** (num_unlocked(Unlocks.Mazes) - 1)
        count = int((target - num_items(Items.Gold) - 1) / (n * n * scale)) + 1
        ensure(Items.Weird_Substance, count * n * scale)
        while num_items(Items.Gold) < target:
            clear()
            till()
            plant(Entities.Bush)
            amount = get_world_size() * 2 ** (num_unlocked(Unlocks.Mazes) - 1)
            use_item(Items.Weird_Substance, amount)
            directions = [North, East, South, West]

            def reverse(direction):
                return directions[(directions.index(direction) + 2) % 4]
            visited = set()
            path = []
            while get_entity_type() != Entities.Treasure:
                visited.add((get_pos_x(), get_pos_y()))
                advanced = False
                for direction in directions:
                    if can_move(direction):
                        move(direction)
                        position = (get_pos_x(), get_pos_y())
                        if position not in visited:
                            path.append(direction)
                            advanced = True
                            break
                        move(reverse(direction))
                if not advanced:
                    if len(path) == 0:
                        break
                    move(reverse(path.pop()))
            if get_entity_type() == Entities.Treasure:
                harvest()

def upgrade():
    for i in range(8):
        for tech in [Unlocks.Expand, Unlocks.Grass, Unlocks.Speed, Unlocks.Trees, Unlocks.Carrots, Unlocks.Pumpkins, Unlocks.Sunflowers, Unlocks.Watering, Unlocks.Fertilizer, Unlocks.Polyculture, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm]:
            level = num_unlocked(tech)
            if (level > 0 or tech == Unlocks.Grass or tech == Unlocks.Speed) and level < len(tiers[tech]):
                parent = tiers[tech][level]
                if parent == None or num_unlocked(parent) > 0:
                    buy(tech)

def run_campaign():
    # All language and observation tools must already be bought in the manual opening.
    upgrade()
    for tech in [Unlocks.Polyculture, Unlocks.Pumpkins, Unlocks.Fertilizer, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation, Unlocks.Leaderboard]:
        if num_unlocked(tech) == 0:
            buy(tech)
        upgrade()
        print("discovery", tech, "field", get_world_size())
    ensure(Items.Power, 1000)
    buy(Unlocks.The_Farmers_Remains)
    upgrade()
    buy(Unlocks.Top_Hat)
    upgrade()
    for tech in dependencies:
        while get_cost(tech) != None:
            buy(tech)
    complete = True
    for tech in dependencies:
        if get_cost(tech) != None:
            complete = False
    print("campaign_complete", complete)
    return complete
```

### campaign

```python
import farm_lib
complete = farm_lib.run_campaign()
print("all_research_maxed", complete)
```

### verify_companion

```python
import farm_lib
farm_lib.ensure(Items.Hay,1000)
farm_lib.ensure(Items.Wood,1000)
before = num_items(Items.Hay)
clear()
entity,position=get_companion()
x,y=position
while get_pos_x()!=x:
    move(East)
while get_pos_y()!=y:
    move(North)
till()
plant(entity)
while get_pos_x()!=0:
    move(East)
while get_pos_y()!=0:
    move(North)
while not can_harvest():
    do_a_flip()
harvest()

companion_ok = num_items(Items.Hay)-before > 2**num_unlocked(Unlocks.Grass)
print("companion_ok", companion_ok)
```

### verify_maze

```python
import farm_lib
amount = get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
farm_lib.ensure(Items.Weird_Substance, amount*2)
before = num_items(Items.Gold)
clear()
till()
plant(Entities.Bush)
amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))

use_item(Items.Weird_Substance,amount)
# Generate a maze from a bush first. Explore using a stack.
directions = [North, East, South, West]
def reverse(direction):
    return directions[(directions.index(direction)+2)%4]
visited = set()
path = []
while get_entity_type() != Entities.Treasure:
    visited.add((get_pos_x(),get_pos_y()))
    advanced = False
    for direction in directions:
        if can_move(direction):
            move(direction)
            position = (get_pos_x(),get_pos_y())
            if position not in visited:
                path.append(direction)
                advanced = True
                break
            move(reverse(direction))
    if not advanced:
        if len(path) == 0:
            break
        move(reverse(path.pop()))
if get_entity_type() == Entities.Treasure:
    harvest()

maze_reuse_ok = num_items(Items.Gold)-before == get_world_size()*get_world_size()*2**num_unlocked(Unlocks.Mazes)
print("maze_reuse_ok", maze_reuse_ok)
```

### verify_drones

```python
clear()
while not can_harvest():
    pass
def column(seconds):
    start=get_time()
    while get_time()-start<seconds:
        if can_harvest():
            harvest()
        move(North)
    return True
workers=[]
for col in range(get_world_size()):
    handle=spawn_drone(column,8)
    if handle==None:
        column(8)
    else:
        workers.append(handle)
    move(East)
for worker in workers:
    wait_for(worker)

print("parallel_complete", num_drones()==1, "capacity", max_drones())
```

### experiment

```python
for i in range(rounds):
    if random()>0.5:
        do_a_flip()
```

### verify_simulation

```python
a=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
b=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
c=simulate('experiment', {}, {}, {'rounds':64}, 8, 64)
print(a==b)

simulation_ok = a==b and a!=c
print("simulation_ok", simulation_ok)
```

### verify_tail

```python
import farm_lib
farm_lib.ensure(Items.Cactus,get_world_size()*get_world_size()+1)
before = num_items(Items.Bone)
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
# Use an even-sized empty field and keep cactus in inventory.
change_hat(Hats.Dinosaur_Hat)
while True:
    direction=East
    if get_pos_x()==0:
        if get_pos_y()>0:
            direction=South
    elif get_pos_y()%2==0:
        if get_pos_x()==get_world_size()-1:
            direction=North
    else:
        direction=West
        if get_pos_x()==1 and get_pos_y()!=get_world_size()-1:
            direction=North
    if not move(direction):
        break
change_hat(Hats.Straw_Hat)

tail_ok = num_items(Items.Bone)>before
print("tail_ok", tail_ok, "bone_gain", num_items(Items.Bone)-before)
```

### verify_hat

```python
hat_ok = change_hat(Hats.Top_Hat)
change_hat(Hats.Straw_Hat)
print("hat_ok", hat_ok)
```

### verify_debug

```python
set_world_size(2)
set_execution_speed(1)
print("temporary_size_ok",get_world_size()==2)
quick_print("debug_ticks",get_tick_count())
# The original field size and execution limit restore when this program ends.
```

### race_hay

```python
clear()
while num_items(Items.Hay)<4096:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

### race_wood

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<6000:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if num_unlocked(Unlocks.Trees)>0 and (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            move(East)
        move(North)
```

### race_carrots

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<2048:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            move(East)
        move(North)
```

### race_pumpkins

```python
clear()
while num_items(Items.Pumpkin)<12000:
    complete=True
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                plant(Entities.Pumpkin)
            if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                complete=False
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
    if complete:
        harvest()
```

### race_sunflowers

```python
clear()
while num_items(Items.Power)<8000:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Sunflower)
            move(East)
        move(North)
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            while not can_harvest():
                pass
            move(East)
        move(North)
    for petals in range(15,6,-1):
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if get_entity_type()==Entities.Sunflower and measure()==petals:
                    harvest()
                move(East)
            move(North)
```

### race_cactus

```python
clear()
while num_items(Items.Cactus)<20000:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Cactus)
            move(East)
        move(North)
    complete=False
    while not complete:
        complete=True
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                if not can_harvest():
                    complete=False
                move(East)
            move(North)
    # Run on a field of mature cacti. Bubble each row, then each column.
    for axis in [East, North]:
        for sweep in range(get_world_size()):
            for row in range(get_world_size()):
                for col in range(get_world_size()-1):
                    if measure() > measure(axis):
                        swap(axis)
                    move(axis)
                move(axis)
                if axis == East:
                    move(North)
                else:
                    move(East)
    harvest()
```

### race_maze

```python
while num_items(Items.Gold)<4096:
    clear()
    till()
    plant(Entities.Bush)
    amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
    use_item(Items.Weird_Substance,amount)
    # Generate a maze from a bush first. Explore using a stack.
    directions = [North, East, South, West]
    def reverse(direction):
        return directions[(directions.index(direction)+2)%4]
    visited = set()
    path = []
    while get_entity_type() != Entities.Treasure:
        visited.add((get_pos_x(),get_pos_y()))
        advanced = False
        for direction in directions:
            if can_move(direction):
                move(direction)
                position = (get_pos_x(),get_pos_y())
                if position not in visited:
                    path.append(direction)
                    advanced = True
                    break
                move(reverse(direction))
        if not advanced:
            if len(path) == 0:
                break
            move(reverse(path.pop()))
    if get_entity_type() == Entities.Treasure:
        harvest()
```

### race_dinosaur

```python
while num_items(Items.Bone)<30000:
    clear()
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            till()
            move(East)
        move(North)
    # Use an even-sized empty field and keep cactus in inventory.
    change_hat(Hats.Dinosaur_Hat)
    while True:
        direction=East
        if get_pos_x()==0:
            if get_pos_y()>0:
                direction=South
        elif get_pos_y()%2==0:
            if get_pos_x()==get_world_size()-1:
                direction=North
        else:
            direction=West
            if get_pos_x()==1 and get_pos_y()!=get_world_size()-1:
                direction=North
        if not move(direction):
            break
    change_hat(Hats.Straw_Hat)
```

### race_reset

```python
tiers={Unlocks.Expand:[Unlocks.Plant,Unlocks.Trees,Unlocks.Pumpkins,Unlocks.Cactus,Unlocks.Dinosaurs],Unlocks.Speed:[Unlocks.Trees,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm],Unlocks.Grass:[Unlocks.Plant,Unlocks.Trees,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Trees:[None,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm],Unlocks.Carrots:[None,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation],Unlocks.Pumpkins:[None,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard],Unlocks.Sunflowers:[None,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains],Unlocks.Cactus:[None,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains,Unlocks.Top_Hat],Unlocks.Mazes:[None,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains,Unlocks.Top_Hat,Unlocks.Top_Hat],Unlocks.Dinosaurs:[None,Unlocks.Megafarm,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains,Unlocks.Top_Hat,Unlocks.Top_Hat,Unlocks.Top_Hat],Unlocks.Watering:[None,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Fertilizer:[None,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs,Unlocks.Megafarm,Unlocks.Simulation],Unlocks.Polyculture:[None,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Megafarm:[Unlocks.Dinosaurs,Unlocks.Dinosaurs,Unlocks.Simulation,Unlocks.Leaderboard,Unlocks.The_Farmers_Remains]}
dependencies={Unlocks.Plant:[],Unlocks.Expand:[Unlocks.Plant],Unlocks.Speed:[Unlocks.Expand,Unlocks.Trees],Unlocks.Grass:[Unlocks.Plant],Unlocks.Trees:[Unlocks.Plant],Unlocks.Carrots:[Unlocks.Expand],Unlocks.Watering:[Unlocks.Carrots],Unlocks.Pumpkins:[Unlocks.Carrots],Unlocks.Sunflowers:[Unlocks.Watering,Unlocks.Pumpkins],Unlocks.Fertilizer:[Unlocks.Pumpkins],Unlocks.Polyculture:[Unlocks.Trees],Unlocks.Cactus:[Unlocks.Pumpkins,Unlocks.Sunflowers],Unlocks.Mazes:[Unlocks.Fertilizer,Unlocks.Cactus],Unlocks.Dinosaurs:[Unlocks.Cactus,Unlocks.Mazes],Unlocks.Megafarm:[Unlocks.Mazes,Unlocks.Dinosaurs],Unlocks.Simulation:[Unlocks.Dinosaurs],Unlocks.Leaderboard:[Unlocks.Simulation],Unlocks.Loops:[],Unlocks.Operators:[Unlocks.Loops],Unlocks.Variables:[Unlocks.Operators],Unlocks.Functions:[Unlocks.Variables],Unlocks.Lists:[Unlocks.Functions],Unlocks.Dicts:[Unlocks.Lists],Unlocks.Import:[Unlocks.Functions],Unlocks.Senses:[],Unlocks.Costs:[],Unlocks.Auto_Unlock:[],Unlocks.Timing:[],Unlocks.Debug:[],Unlocks.Hats:[],Unlocks.Debug_2:[Unlocks.Debug],Unlocks.Utilities:[Unlocks.Operators],Unlocks.The_Farmers_Remains:[Unlocks.Leaderboard],Unlocks.Top_Hat:[Unlocks.The_Farmers_Remains],}
goals={Unlocks.Trees:(Items.Hay,160),Unlocks.Carrots:(Items.Wood,600),Unlocks.Pumpkins:(Items.Carrot,1500),Unlocks.Sunflowers:(Items.Pumpkin,8000),Unlocks.Cactus:(Items.Power,6000),Unlocks.Mazes:(Items.Cactus,30000),Unlocks.Dinosaurs:(Items.Gold,30000),}
def buy(tech):
    if num_unlocked(tech)==0:
        for parent in dependencies[tech]:
            if num_unlocked(parent)==0:
                buy(parent)
        if tech in goals:
            item,target=goals[tech]
            missing=target-num_produced(item)
            if missing>0:
                ensure(item,num_items(item)+missing)
    cost=get_cost(tech)
    if cost==None:
        return
    for item in cost:
        ensure(item,cost[item])
    unlock(tech)

def fund(entity,amount):
    cost=get_cost(entity)
    for item in cost:
        ensure(item,cost[item]*amount)

def ensure(item,target):
    if num_items(item)>=target:
        return
    n=get_world_size()
    if item==Items.Hay:
        clear()
        while num_items(Items.Hay)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    move(East)
                move(North)
    elif item==Items.Wood:
        clear()
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                till()
                move(East)
            move(North)
        while num_items(Items.Wood)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    if get_entity_type()==None:
                        if num_unlocked(Unlocks.Trees)>0 and (get_pos_x()+get_pos_y())%2==0:
                            plant(Entities.Tree)
                        else:
                            plant(Entities.Bush)
                    move(East)
                move(North)
    elif item==Items.Carrot:
        if num_unlocked(Unlocks.Carrots)==0:
            buy(Unlocks.Carrots)
        amount=(int((target-num_items(item))/(n*n*2**(num_unlocked(Unlocks.Carrots)-1)))+3)*n*n
        fund(Entities.Carrot,amount)
        clear()
        for row in range(get_world_size()):
            for col in range(get_world_size()):
                till()
                move(East)
            move(North)
        while num_items(Items.Carrot)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if can_harvest():
                        harvest()
                    if get_entity_type()==None:
                        plant(Entities.Carrot)
                    move(East)
                move(North)
    elif item==Items.Pumpkin or item==Items.Weird_Substance:
        if num_unlocked(Unlocks.Pumpkins)==0:
            buy(Unlocks.Pumpkins)
        infected=item==Items.Weird_Substance
        if infected and num_unlocked(Unlocks.Fertilizer)==0:
            buy(Unlocks.Fertilizer)
        yield_amount=n*n*min(n,6)*2**(num_unlocked(Unlocks.Pumpkins)-1)
        if infected:
            yield_amount=yield_amount/2
        amount=(int((target-num_items(item))/yield_amount)+3)*n*n*3
        fund(Entities.Pumpkin,amount)
        if infected:
            clear()
            while num_items(Items.Weird_Substance)<target:
                complete=True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_ground_type()!=Grounds.Soil:
                            till()
                        if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                            plant(Entities.Pumpkin)
                        if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                            complete=False
                        if get_water()<0.3 and num_items(Items.Water)>0:
                            use_item(Items.Water)
                        move(East)
                    move(North)
                if complete:
                    per_plant=min(get_world_size(),6)*2**(num_unlocked(Unlocks.Pumpkins)-1)/2
                    infected=min(get_world_size()*get_world_size(),int((target-num_items(Items.Weird_Substance)-1)/per_plant)+1)
                    for row in range(get_world_size()):
                        for col in range(get_world_size()):
                            if infected>0:
                                while num_items(Items.Fertilizer)==0:
                                    pass
                                use_item(Items.Fertilizer)
                                infected-=1
                            move(East)
                        move(North)
                    harvest()
        else:
            clear()
            while num_items(Items.Pumpkin)<target:
                complete=True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_ground_type()!=Grounds.Soil:
                            till()
                        if get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:
                            plant(Entities.Pumpkin)
                        if get_entity_type()!=Entities.Pumpkin or not can_harvest():
                            complete=False
                        if get_water()<0.3 and num_items(Items.Water)>0:
                            use_item(Items.Water)
                        move(East)
                    move(North)
                if complete:
                    harvest()
    elif item==Items.Power:
        if num_unlocked(Unlocks.Sunflowers)==0:
            buy(Unlocks.Sunflowers)
        amount=(int((target-num_items(item))/(n*n*2**(num_unlocked(Unlocks.Sunflowers)-1)))+3)*n*n
        fund(Entities.Sunflower,amount)
        clear()
        while num_items(Items.Power)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if get_ground_type()!=Grounds.Soil:
                        till()
                    if get_entity_type()==None:
                        plant(Entities.Sunflower)
                    move(East)
                move(North)
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    while not can_harvest():
                        pass
                    move(East)
                move(North)
            for petals in range(15,6,-1):
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if get_entity_type()==Entities.Sunflower and measure()==petals:
                            harvest()
                        move(East)
                    move(North)
    elif item==Items.Cactus:
        if num_unlocked(Unlocks.Cactus)==0:
            buy(Unlocks.Cactus)
        amount=(int((target-num_items(item))/(n*n*min(n*n,16)*2**(num_unlocked(Unlocks.Cactus)-1)))+3)*n*n
        fund(Entities.Cactus,amount)
        clear()
        while num_items(Items.Cactus)<target:
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    if get_ground_type()!=Grounds.Soil:
                        till()
                    if get_entity_type()==None:
                        plant(Entities.Cactus)
                    move(East)
                move(North)
            complete=False
            while not complete:
                complete=True
                for row in range(get_world_size()):
                    for col in range(get_world_size()):
                        if not can_harvest():
                            complete=False
                        move(East)
                    move(North)
            # Run on a field of mature cacti. Bubble each row, then each column.
            for axis in [East, North]:
                for sweep in range(get_world_size()):
                    for row in range(get_world_size()):
                        for col in range(get_world_size()-1):
                            if measure() > measure(axis):
                                swap(axis)
                            move(axis)
                        move(axis)
                        if axis == East:
                            move(North)
                        else:
                            move(East)
            harvest()
    elif item==Items.Bone:
        while num_items(Items.Bone)<target:
            ensure(Items.Cactus,n*n+1)
            clear()
            for row in range(get_world_size()):
                for col in range(get_world_size()):
                    till()
                    move(East)
                move(North)
            # Use an even-sized empty field and keep cactus in inventory.
            change_hat(Hats.Dinosaur_Hat)
            while True:
                direction=East
                if get_pos_x()==0:
                    if get_pos_y()>0:
                        direction=South
                elif get_pos_y()%2==0:
                    if get_pos_x()==get_world_size()-1:
                        direction=North
                else:
                    direction=West
                    if get_pos_x()==1 and get_pos_y()!=get_world_size()-1:
                        direction=North
                if not move(direction):
                    break
            change_hat(Hats.Straw_Hat)
    elif item==Items.Gold:
        scale=2**(num_unlocked(Unlocks.Mazes)-1)
        count=int((target-num_items(Items.Gold)-1)/(n*n*scale))+1
        ensure(Items.Weird_Substance,count*n*scale)
        while num_items(Items.Gold)<target:
            clear()
            till()
            plant(Entities.Bush)
            amount=get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)
            use_item(Items.Weird_Substance,amount)
            # Generate a maze from a bush first. Explore using a stack.
            directions = [North, East, South, West]
            def reverse(direction):
                return directions[(directions.index(direction)+2)%4]
            visited = set()
            path = []
            while get_entity_type() != Entities.Treasure:
                visited.add((get_pos_x(),get_pos_y()))
                advanced = False
                for direction in directions:
                    if can_move(direction):
                        move(direction)
                        position = (get_pos_x(),get_pos_y())
                        if position not in visited:
                            path.append(direction)
                            advanced = True
                            break
                        move(reverse(direction))
                if not advanced:
                    if len(path) == 0:
                        break
                    move(reverse(path.pop()))
            if get_entity_type() == Entities.Treasure:
                harvest()

def upgrade():
    for i in range(8):
        for tech in [Unlocks.Expand,Unlocks.Grass,Unlocks.Speed,Unlocks.Trees,Unlocks.Carrots,Unlocks.Pumpkins,Unlocks.Sunflowers,Unlocks.Fertilizer,Unlocks.Watering,Unlocks.Cactus,Unlocks.Mazes,Unlocks.Dinosaurs]:
            level=num_unlocked(tech)
            if (level>0 or tech==Unlocks.Grass or tech==Unlocks.Speed) and level<len(tiers[tech]):
                parent=tiers[tech][level]
                if parent==None or num_unlocked(parent)>0:
                    buy(tech)

buy(Unlocks.Plant)
buy(Unlocks.Expand)
upgrade()
buy(Unlocks.Trees)
upgrade()
buy(Unlocks.Carrots)
upgrade()
buy(Unlocks.Watering)
buy(Unlocks.Pumpkins)
upgrade()
buy(Unlocks.Fertilizer)
buy(Unlocks.Sunflowers)
upgrade()
buy(Unlocks.Cactus)
upgrade()
buy(Unlocks.Mazes)
upgrade()
buy(Unlocks.Dinosaurs)
buy(Unlocks.Simulation)
buy(Unlocks.Leaderboard)
```

### races

```python
challenges = [(Leaderboards.Hay, 'race_hay'), (Leaderboards.Hay_Single, 'race_hay'), (Leaderboards.Wood, 'race_wood'), (Leaderboards.Wood_Single, 'race_wood'), (Leaderboards.Carrots, 'race_carrots'), (Leaderboards.Carrots_Single, 'race_carrots'), (Leaderboards.Pumpkins, 'race_pumpkins'), (Leaderboards.Pumpkins_Single, 'race_pumpkins'), (Leaderboards.Sunflowers, 'race_sunflowers'), (Leaderboards.Sunflowers_Single, 'race_sunflowers'), (Leaderboards.Cactus, 'race_cactus'), (Leaderboards.Cactus_Single, 'race_cactus'), (Leaderboards.Maze, 'race_maze'), (Leaderboards.Maze_Single, 'race_maze'), (Leaderboards.Dinosaur, 'race_dinosaur'), (Leaderboards.Fastest_Reset, 'race_reset')]
for category,program in challenges:
    print("challenge_start",category)
    leaderboard_run(category,program,64)
    print("challenge_passed",category)
print("all_16_challenges_finished",True)
```

## 9. 382步参考回放

附带 `REFERENCE-382-STEPS.md`：已通过的种子731基准路线，每一步实际代码、购买研究与库存都有记录。该路线的完整通关主时钟约3小时。它和本手册的简化自动控制器采用不同的生产调度，不能把两条路线的中间库存混用。

基准回放中的某些生产步骤之前，由外部测试调度器先完成前置采购。参考文档按真实时间顺序逐项展开这些采购；正常手动测试优先照本手册的推荐步骤执行。
