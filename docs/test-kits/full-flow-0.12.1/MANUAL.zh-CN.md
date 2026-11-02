# 禾序 0.12.1 — 田野手册

[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)

## 01 / 第一份收获

从一格草地、零库存开始。草会自然成熟，运行 harvest()，攒够五份干草后研究循环，再从模板载入第一份收获。

之后研究运算符、变量、传感器和种植。库存够用不等于自动研究：在科技树点击付费，或后续调用 unlock()。草和灌木不消耗种子，原料用完仍可恢复生产。

本版目标是有可运行自动化策略时，约三小时游戏时间完成全部研究；0.12.1另有多小时的六岛物流流程，可以与研究路线交错推进；编程与思考另计。正常 1 倍速保留科技加速，设置中的倍率会加快整个世界。

```python
# Requires Loops. Stop when you have enough hay.
while True:
    harvest()
```

## 02 / 程序与状态

程序名称支持数字或汉字开头，例如123、1号农场、浇水程序。允许汉字、字母、数字和下划线，最多80字符，不填.py。重复名称（含仅大小写不同）不能创建。数字文件可用 from 123 import work 或 import 123 as farm；中文文件可直接 import 浇水程序。

程序也是可复用模块。程序库按文件显示卡片，并显示总数、活跃程序数和无人机槽位。绿色表示还有工作机在运行；琥珀色边框表示当前主编辑器打开的文件。同一个程序可以同时处于运行和编辑状态。暂停仍算活跃，空闲表示没有存活工作机。

编辑只切换查看，不会停止已有工作。主运行按钮会替换控制程序及其全部工作机。窗口按钮打开可拖动编辑器，独立运行在有空余槽位时启动文件。全部停止会结束所有任务。关闭编辑窗口不删除代码，也不停止任务；活跃程序不能删除，至少保留一个文件。

Cmd/Ctrl+Z 撤销，Cmd/Ctrl+Shift+Z 重做；F9 在光标行切换断点。用暂停、单步和变量监视定位问题。

## 03 / 语言基础

解释器采用类 Python 语法，并不是完整 Python 环境。每层缩进四个空格或一个 Tab，不要混用。名称和枚举大小写敏感；变量、条件、函数、集合需要对应语言研究。设置中允许未解锁语法只移除语法门槛，不赠送作物与研究。

支持 if/elif/else、while、for、break/continue、带参函数、默认参数、return、global、模块导入、列表、不可变元组、集合、字典、索引、解包与闭包。不提供 Python 标准库或操作系统访问。比较用 ==，赋值用 =；负索引从末尾计数。

调试可用 print()，频繁输出有执行成本。没有退出条件的 while 会一直运行到点击停止；有限生产任务可按库存设定目标。

```python
# Requires Operators, Variables and Loops.
target = 200
while num_items(Items.Hay) < target:
    if can_harvest():
        harvest()
```

## 04 / 分阶段研究

研究分为三种条件：前置发现、累计生产和实际付费。累计生产统计成功收获，即使资源已经花掉也算完成，不会再扣一次；科技树显示当前量与目标。发现新作物后才逐步开放田规模、速度和增产等级。

农田规模：种植→2×2、林木→4×4、南瓜→8×8、仙人掌→16×16、长尾→32×32；每级仍需支付扩田成本。更新保留已购研究，完整体验新进度请新建农场。

get_cost() 查询价格，get_unlock_requirements() 查询门槛，can_unlock() 同时检查门槛和库存。条件不齐时 unlock() 返回 False，不扣资源。没有固定时长认证。下面列出各升级等级所需的发现。

| Research | Discoveries required for successive levels |
|---|---|
| expand | 1: plant / 2: trees / 3: pumpkins / 4: cactus / 5: dinosaurs |
| speed | 1: trees / 2: carrots / 3: pumpkins / 4: sunflowers / 5: cactus / 6: mazes / 7: dinosaurs / 8: megafarm |
| grass | 1: plant / 2: trees / 3: carrots / 4: pumpkins / 5: sunflowers / 6: cactus / 7: mazes / 8: dinosaurs |
| trees | 1: base / 2: carrots / 3: pumpkins / 4: sunflowers / 5: cactus / 6: mazes / 7: dinosaurs / 8: megafarm |
| carrots | 1: base / 2: pumpkins / 3: sunflowers / 4: cactus / 5: mazes / 6: dinosaurs / 7: megafarm / 8: simulation |
| pumpkins | 1: base / 2: sunflowers / 3: cactus / 4: mazes / 5: dinosaurs / 6: megafarm / 7: simulation / 8: leaderboard |
| sunflowers | 1: base / 2: cactus / 3: mazes / 4: dinosaurs / 5: megafarm / 6: simulation / 7: leaderboard / 8: the_farmers_remains |
| cactus | 1: base / 2: mazes / 3: dinosaurs / 4: megafarm / 5: simulation / 6: leaderboard / 7: the_farmers_remains / 8: top_hat |
| mazes | 1: base / 2: dinosaurs / 3: megafarm / 4: simulation / 5: leaderboard / 6: the_farmers_remains / 7: top_hat / 8: top_hat |
| dinosaurs | 1: base / 2: megafarm / 3: simulation / 4: leaderboard / 5: the_farmers_remains / 6: top_hat / 7: top_hat / 8: top_hat |
| watering | 1: base / 2: pumpkins / 3: sunflowers / 4: cactus / 5: mazes / 6: dinosaurs |
| fertilizer | 1: base / 2: cactus / 3: mazes / 4: dinosaurs / 5: megafarm / 6: simulation |
| polyculture | 1: base / 2: pumpkins / 3: sunflowers / 4: cactus / 5: mazes / 6: dinosaurs |
| megafarm | 1: dinosaurs / 2: dinosaurs / 3: simulation / 4: leaderboard / 5: the_farmers_remains |

| First discovery | Cumulative production |
|---|---:|
| trees | hay 160 |
| carrots | wood 600 |
| pumpkins | carrot 1,500 |
| sunflowers | pumpkin 8,000 |
| cactus | power 6,000 |
| mazes | cactus 30,000 |
| dinosaurs | gold 30,000 |
```python
# Requires Costs, Automatic research and Operators.
if can_unlock(Unlocks.Speed):
    unlock(Unlocks.Speed)
# Requires Debug for output.
print(get_unlock_requirements(Unlocks.Expand))
print(num_produced(Items.Wood))
```

## 05 / 作物与供应链

草产干草，灌木与树产木材。胡萝卜需要耕地、干草和木材；南瓜与太阳花消耗胡萝卜，仙人掌消耗南瓜。增产研究也会提高种子成本；铺田前用 get_cost(Entities.Carrot) 查种子价格，并为多轮补种预留原料。

种植要求空格和合适地面，枯死南瓜可直接补种。till() 切换地面；清田和扩田会重置田地、损失在田作物。整田换作物之前先留足库存。

每株相邻树都会进一步拖慢树木生长，推荐树与灌木棋盘交替。普通农田越界环绕，最右列再向东会回到第零列；迷宫和长尾不能环绕。无水基础生长时间：草3秒、灌木4秒、树6秒、胡萝卜7秒、南瓜11秒、太阳花8.5秒、仙人掌1.5秒；浇水会加快生长。

```python
# Requires Sensors, Planting, Carrots, Loops and Operators.
while num_items(Items.Carrot) < 200:
    if get_ground_type() != Grounds.Soil:
        till()
    if can_harvest():
        harvest()
    if get_entity_type() == None:
        plant(Entities.Carrot)
    move(East)
```

## 06 / 灌溉与感染

灌溉研究每十个游戏秒产生水。一份水增加0.25水分，上限1.0，水分会逐渐衰减。仅在水分低于阈值且库存有水时灌溉；如果阈值设为0.3，地块约50%水分时不浇水是预期行为。

肥料同样每十秒自动产生；使用后增加成长并感染本株。合并南瓜或仙人掌收获时，奇异物质按感染株数比例计算，一株感染不会把整田的一半变成奇异物质。肥料催熟也会执行正常南瓜死亡判定。

奇异物质翻转本格及去重后的相邻格感染状态；对灌木使用足量物质会生成迷宫，消耗为边长×2^(迷宫等级−1)；对宝藏使用相同数量会给金币并复用迷宫，最多复用300次。

```python
# Add inside a farming loop after Irrigation and Sensors.
if get_water() < 0.3 and num_items(Items.Water) > 0:
    use_item(Items.Water)
```

## 07 / 巨型南瓜

南瓜在成熟时有20%概率枯死。反复扫描完整方阵，补种 Dead_Pumpkin；修复期间不要单独收割健康南瓜。目标方阵的每一格都成熟后才收获。游戏会合并包含收获点的最大完整方阵。

边长n的产量为 n²×min(n,6)×本级单株产量。要检查整块田，不能只检查脚下一格；预留补种种子。生产奇异物质时，成熟后对需要的株数施肥，再合并收获。累计进度看 num_produced()，可支付库存看 num_items()。

```python
# Full runnable repair algorithm: Templates → Growing giant pumpkins.
# Preconditions: empty tilled field, sufficient carrot seeds.
if get_entity_type() == Entities.Dead_Pumpkin:
    plant(Entities.Pumpkin)
```

## 08 / 太阳花优先级

太阳花的 measure() 返回7～15花瓣。田里至少剩十株太阳花时，收获当前最大花瓣可获8倍奖励；先收小花会破坏奖励序列，直到太阳花田清空。只扩种、不测量会损失大部分潜在能量。

先铺田并等成熟，等待时避免来回移动；然后按15到7花瓣分档扫描收获。科技和能量会加速指令，移动与解释执行也消耗能量。不要对每一株都做全田最大值搜索，也不要在等待时把能量全部耗掉。

能量是耗材，并不是永久速度解锁。留下储备和下一轮太阳花所需的胡萝卜。

```python
# On an existing mature sunflower field, for each petal tier:
for petals in range(15, 6, -1):
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_entity_type() == Entities.Sunflower and measure() == petals:
                harvest()
            move(East)
        move(North)
```

## 09 / 伴生种植

研究伴生种植后，get_companion() 返回偏好作物和目标坐标；不适用地块返回 None。草、灌木、树和胡萝卜可提出偏好。原作物收获时，目标坐标需要放着指定类型的作物，伴生本身不用成熟。

先读取并保存偏好，布置伴生，再回到原株成熟后收获。收割补种后偏好可能变化，不要永久缓存。1级为2倍，每级再加0.5，6级达4.5倍；未研究没有伴生加成。

可用字典管理坐标到偏好的映射，处理不同株的需求冲突；盲目覆盖整田会破坏已种作物。

```python
request = get_companion()
if request != None:
    entity, position = request
    target_x, target_y = position
    print(entity, target_x, target_y)
```

## 10 / 仙人掌排序

成熟仙人掌的测量尺寸为0～9。swap(East) 或 swap(North) 交换相邻植株。相连成熟区域必须满足向东、向北尺寸均不递减，才能整块连锁；一对逆序就会破坏区域连收。

先排序每一行，再排序每一列。模板的冒泡方法简单可靠；理解不变量后可以优化比较和移动。行排序后再做列排序会保留行间有序性。m株连收产量为本级单株产量×m×min(m,16)，较大有序田仍有收益，倍率不会无限暴涨。

先准备南瓜种子，再清理前一种作物；仅排序尚未成熟的仙人掌仍不能完成连收。

```python
# Full algorithm: Templates → Sorting cacti.
if measure() > measure(East):
    swap(East)
```

## 11 / 迷宫搜索

种一株灌木，使用足量奇异物质生成连通迷宫。can_move() 测墙，move() 返回移动是否成功，measure() 返回宝藏坐标。到宝藏收获金币；误收树篱会清除迷宫，得不到宝藏奖励。

用 visited 集合和方向栈进行深度优先搜索。探索没去过的邻格，走不动时按栈反方向回溯。迷宫复用会拆墙产生环，所以 visited 必不可少。宝藏按照真实通道距离选在足够远的位置，不会重复出现在脚下。

到宝藏后用奇异物质替代 harvest()，会发金币并保留布局、迁移宝藏；清空 visited 再找下一轮。复用消耗材料，最多300次，然后收获并重新生成迷宫。

```python
# Requires Mazes, Planting and Sensors. Use on an empty plot.
plant(Entities.Bush)
amount = get_world_size() * 2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance, amount)
# Then run Templates → Searching a maze.
```

## 12 / 长尾路线

恐龙帽开启苹果与长尾模式。苹果消耗仙人掌库存，尾巴占据地块、禁止穿越，田边不再环绕。can_move() 与 move() 遵守占用规则；在苹果处 measure() 可得到下一目标。一次只有一台无人机能控制长尾。

偶数边长空田可使用哈密顿回路：无交叉地走遍每格，再沿第一列返回。持续跟随回路，比贪心冲向每个苹果更安全。至少准备边长²+1份仙人掌，停止或换回草帽即可结算当前尾巴。

骨头=floor(尾长×min(尾长,64)×(1+0.25×(研究等级−1)))。长度超过64仍增加产量，但长度倍率封顶。模板会在满路线受阻后结束。

```python
# Full algorithm: Templates → Tail coverage.
change_hat(Hats.Dinosaur_Hat)
# Follow a safe route, then settle:
change_hat(Hats.Straw_Hat)
```

## 13 / 独立无人机

并行研究将容量从1提升到2、4、8、16、32；主控制程序也占槽位。spawn_drone(function,参数...) 返回句柄，槽满返回 None。工作机拿到独立变量和集合副本，但共享田地和库存，可以同格移动，不发生实体碰撞。

按列或分区工作，避免两台机器同时在一格种植、争抢种子。保存句柄，再逐个 wait_for() 取返回值；has_finished() 不等待只查询。工作机修改 global 不会改变控制程序副本，请用 return 传递结果。完成后槽位会释放。

生成失败时由主控制程序接手任务。达标后仍有工作机不停执行的程序不能完成资源竞速。

```python
# Requires Functions, Lists and Parallel drones.
def worker(n):
    for i in range(n):
        if can_harvest():
            harvest()
        move(North)
    return n
handle = spawn_drone(worker, get_world_size())
if handle != None:
    print(wait_for(handle))
else:
    worker(get_world_size())
```

## 14 / 模拟与竞速

创建名为 experiment 的程序。simulate(名称,科技,库存,全局变量,种子,加速) 在独立农场运行，返回游戏秒。Unlocks 代表全部科技，也可用字典指定等级；传入库存和全局变量是独立副本。重复非负种子可复现初始随机性，真实库存和作物不受影响。模拟最多一小时游戏时间，不支持嵌套。

资源竞速使用种子41、中阶8×8农场，提供原料。目标分别为：干草4096、木材6000、胡萝卜2048、南瓜12000、能量8000、仙人掌20000、金币4096、骨头30000；需要达标并结束全部工作机。_Single 类别只允许一台。资源竞速上限一小时；最快重置从零开局，上限四小时，通过程序买下本地竞速研究后结束。

成绩仅保存在本机。旧规则成绩标为历史，不与当前成绩比较。成就和竞速完成后弹窗提示，未读提示重启保留，不向 Steam 在线榜提交。

```python
# experiment.py, then call from a different program:
# def work lives in the experiment file; modules use the file name.
seconds = simulate('experiment', Unlocks, {Items.Carrot: 10000}, {}, 7, 64)
print(seconds)
```

## 15 / 排查与恢复

一直长不出作物：检查对应研究、种子库存、地面和空格。plant() 失败返回 False，在空地上等成熟不会产生作物。

物资够却研究不了：查看下一级所需作物发现和累计产量。执行速度与增产现在不能一次全部开放。

南瓜不能合并：检查每格是否枯死或未成熟。仙人掌不连收：找向东/北逆序或未成熟格。能量耗光：减少重复移动与扫描。迷宫卡住：使用 visited 和回溯，迷宫没有普通环绕。无人机 global 不返回：用 wait_for() 接收返回值。

用F9断点、暂停和单步逐行检查；语法错误带行号。备份文件以 .bak 结尾。农场库创建独立存档及程序目录。外部编辑器会导出 .py 和 __builtins__.py 提示并监视保存，避免两处同时改同一文件。音量滑条松开时保存。

更新后测试完整进度建议新建农场。旧规则获得的资源不会被追回。

## 16 / 接口速查

下方列出全部游戏接口、前置与返回值。命名空间包括 Entities、Items、Grounds、Unlocks、Hats、Leaderboards，大小写敏感。研究成功才从共享库存付费。查询到价格不代表已满足发现与产量条件；满级节点的价格和条件均返回 None。

其他章节的代码可能是已有田地或循环中的片段，请先满足文中的前提。主要作物的完整算法可从模板创建。

### `harvest() → bool`

收获成熟作物；在宝藏处发金币。未成熟或空地返回 False。

### `can_harvest() → bool`

查询脚下是否成熟，不执行收获。

### `plant(entity) → bool`

需要种植/作物研究及种子，格子被占或地面不符时返回 False。

### `till()`

切换草地和耕地，耕地作物种植前使用。

### `move(direction) → bool`

向四方向移动，可能被墙或尾巴阻挡；普通田环绕。

### `can_move(direction) → bool`

传感器。按墙和尾巴规则预判，不移动。

### `use_item(item, amount=1) → bool`

消耗水、肥料或奇异物质，不适用或不足时返回 False。

### `get_pos_x() / get_pos_y() / get_world_size() → number`

传感器。零起始坐标及当前农田边长。

### `get_entity_type() / get_ground_type()`

传感器。返回实体、None 或地面枚举。

### `get_water() → number`

传感器。当前0～1水分。

### `num_items(item) → number`

传感器。当前可支付库存，无人机共享。

### `num_produced(item) → number`

传感器。累计收获产量，包含收获的奇异物质；花掉库存不会减少。自动生成的水和肥料请用 num_items() 查询。

### `num_unlocked(unlock) → number`

已购买的科技等级。

### `get_cost(entity_or_unlock, level=-1) → dict | None`

成本查询。实体返回种子费用，科技返回下一等级价格；可选 level 是零起始已购等级索引。

### `get_unlock_requirements(unlock) → dict | None`

成本查询。unlock:<id> 为前置等级，produced:<资源> 为累计产量要求；不含需支付价格。

### `can_unlock(unlock) → bool`

成本查询。发现、累计产量和可支付物资都足够时返回 True。

### `unlock(unlock) → bool`

自动科研。付费购买下一级，条件不满足返回 False，不扣资源。

### `get_companion() → tuple | None`

传感器与伴生研究。适用地块返回(作物,(x,y))。

### `measure(direction=None)`

传感器。花瓣/仙人掌尺寸、迷宫宝藏或下个苹果坐标，不适用返回 None。

### `swap(direction) → bool`

交换相邻仙人掌，不购买种子。

### `spawn_drone(function, args...) → handle | None`

并行研究。复制独立内存，槽满返回 None。

### `wait_for(handle) → value`

等待并取得工作机返回值，田地与其他工作机继续推进。

### `has_finished(handle) → bool`

不阻塞地查询任务是否完成。

### `num_drones() / max_drones() → number`

活跃机器数（含主程序）与容量上限。

### `simulate(file, unlocks, items, globals, seed, speedup) → seconds`

模拟研究。隔离运行程序，固定种子复现初始随机性。

### `leaderboard_run(category, file, speedup)`

本地竞速研究。用固定预设运行指定程序，达标并终止后记成绩。

### `get_time() / get_tick_count() → number`

计时工具。当前世界秒数/本机指令计费计数。

### `set_world_size(side)`

世界调试工具。临时设置不超过已购上限的边长，执行结束恢复；0表示最大。

### `set_execution_speed(speed)`

世界调试工具。限制当前执行速度，不能突破已购速度。

### `change_hat(hat) → bool`

切换帽子；恐龙帽与礼帽要求对应研究。退出长尾会结算骨头。

### `random() → number`

数值工具。固定种子生成[0,1)随机数。

### `print(values...) / quick_print(values...)`

调试。写入日志，频繁 print 增加执行成本。

### `clear() / do_a_flip()`

clear 重置当前田并损失在田作物；do_a_flip 消耗动作但不移动。

### `fleet_start() → bool`

签约一次：胡萝卜研究＋100木材＋100胡萝卜；一次性获得100机队资金。

### `fleet_state() / fleet_order() / fleet_units() → dict / list`

读取独立数量／尺寸／类型进度、多目标订单与实体机快照，含在途货运、在制品、代码错误和收件箱数量。

### `fleet_stock(island, item) → number`

本地可用库存，不含已接受货运的预留。货物用字符串：water、grain、ore、alloy、crystal、medicine、core。

### `fleet_map(island) → dict`

从下到上的地图行、配方、类型要求、坐标和岛屿连接；岛屿编号0—5。

### `fleet_build(role, width, height, island) → id | -1`

支付资金，分别检查类型、机体、槽位与停机空间；支持旋转尺寸。

### `fleet_research("slots" | "size" | role) → bool`

达到订单发现后支付机队资金；增加数量不会扩大机体。

### `fleet_work(id, island, item, batches) → bool`

排入类型匹配的本地配方；每批原料与产出按机体面积计算，加工开始时预留原料。

### `fleet_ship(id, source, target, item, amount) → bool`

仅运输机。原子预留源仓可用库存，再真实装货、航行与卸货，不会瞬移物资。

### `fleet_rebase(id, island) / fleet_route(id, x, y) → bool`

排入跨岛移动或本岛路径，检查地形、单向通道和全部机体占地。

### `fleet_plan(id, x, y) → list`

查询考虑地形的本岛路径；实际通行还可能因实时占地而等待或绕行。

### `fleet_step(id, direction) / fleet_rotate(id) → bool`

仅空闲机。危险移动失败触发急停，维修费不超过2资金；旋转后须能完整放下。

### `fleet_cancel(id) / fleet_remove(id) / fleet_halt_all() → bool`

仅已降落机。退回预留原料与未提取货物，拆解另返还机体价格。

### `fleet_submit() → bool`

满足机体／程序／航行清单后，一次性扣除本单全部目标物资，奖励资金并开放下一单。

### `fleet_run(id, file) / fleet_stop(id) → bool`

为实体机启动独立文件解释器，或停止代码；停止代码不会取消已排入的物理工作。

### `fleet_self() → id`

单机子程序内返回绑定机体编号；主控制器中返回-1。

### `fleet_send(id, payload) / fleet_receive() → bool / value`

向容量16的收件箱发送值副本；非阻塞接收，空时返回None；单机程序只接收本机消息。

### `fleet_result(id) → value | None`

读取已结束子程序的result变量副本；结果为临时数据，不写存档。

### `fleet_wait(seconds)`

让出当前调用者，让物理队列和其他程序推进；范围0—3600，不是解锁计时条件。

### `fleet_market() / fleet_sell(island, item, amount) → dict / credits`

查询出口价格，或出售本地可用库存恢复资金；已预留及在途货物不能出售。

## 17 / 专用机队

发现胡萝卜后打开群岛。开拓许可消耗100木材与100胡萝卜，一次性提供100机队资金。群岛实体机与原有1—32台脚本工作机独立。数量（3—8）、机体占地（1×1、2×1、2×2、3×2、3×3，可旋转）和专用类型分别研究。大机体能加工更大批次、运输更多物资，但移动较慢，每个占地格都必须可通行。灌溉机抽水；种植机生产谷物或药剂；工程机采矿、冶炼矿石／合金或组装核心；侦察机提炼晶体；运输机搬运货物；采收机加快谷物生产。

订单奖励机队资金，类型和尺寸有明确的订单门槛。维修费导致缺钱时，可以fleet_sell出口富余库存恢复资金，不能出售已预留货单。拆解已降落机体返还全部机体价格；取消队列退回已预留配方原料和未提取货物。航行中的机体须先降落才能取消或拆解。自动路径等待不收费；危险的手动 fleet_step 会触发急停和有限维修费。机体不会重叠。手动停机时请勿堵住码头。

```python
# After signing the charter. Each call pays its real fleet credit cost.
worker = fleet_build("irrigator",1,1,0)
if worker > 0:
    fleet_work(worker,0,"water",8)
```

## 18 / 群岛、配方与订单

六座岛的独立仓库构成生产图：炉心种谷物；苇泽供水；余烬用矿石＋谷物＋水冶炼合金；琉光用合金＋水提炼晶体；花汐用谷物＋晶体＋水生产药剂；天穹把合金＋晶体＋药剂组装为核心。岛屿同时要求订单进度与主农场发现：胡萝卜／南瓜／仙人掌／迷宫／长尾／自治信标。库存按岛独立，num_items(Items.Water) 不等于 fleet_stock(岛屿,"water")。

用 fleet_map 查询各岛地图与连接图。宽通道组成向东／向西的单向环路；小机体可穿窄道。货运指令接受时预留真实源仓库存，运输机再前往码头装货，沿连接航道逐段航行，并等待完整占地可降落。已预留和在途货物不能重复投入配方或订单。

18单要求多个品类、多个目的地同时备齐，全部交付只扣一次列明的本地库存。后续任务还认证实际机体作业、不同子程序并发和同时航行；清单会显示本单峰值与目标。没有按秒数放行的认证。群岛流程衔接主农场；编辑、排错与交错生产会改变总通关时间。

```python
# Requires a hauler and discovered connected islands. Amount must be in source stock.
accepted = fleet_ship(hauler,1,0,"water",24)
print(accepted)
```

## 19 / 多群岛分层编程

在群岛界面创建10文件模块化范例。island_controller 导入 island_planner，总控分配实体机、展开配方依赖，再把工作发送到单机收件箱。fleet_worker 是共享函数库；island_water／island_grain／island_foundry／island_crystal／island_medicine／island_archive／island_cargo 分别导入它，并限制各自可加工的配方。

fleet_run(id,文件名) 为实体机绑定独立解释器。自动注入 unit_id 与启动时的 island_id；fleet_self() 返回本机编号。子程序可以继续导入其他文件、定义函数并产生返回结果。收件箱复制消息内容，每台机器都有自己的变量与模块状态。fleet_send 发送任务；fleet_receive 接收本机下一条消息；fleet_result 读取结束后的 result 变量。某台机器报错只停止该子程序，卡片显示错误，其他机器和物理队列继续。无限循环受每个模拟步的指令预算限制，不会卡住整个世界。

物理任务、预留原料与在途货物会保存；Python调用栈、消息收件箱和返回对象不保存。载入后重新运行总控，或为单机重新绑定程序，先检查已有任务再发送，避免重复。停止代码结束解释器；活跃总控可能重启它，手动接管前请先全部停止。取消队列取消已降落的物理任务。主农场运行按钮启动总控；机队卡片的绑定并运行启动单机子程序。

```python
# Main controller; create a program named water_worker first.
if fleet_run(worker,"water_worker"):
    fleet_send(worker,["water",8])
# In water_worker: use fleet_self(), fleet_receive(), fleet_work() and fleet_wait().
```
