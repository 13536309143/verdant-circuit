# 禾序 0.13.0 — 地球与群岛完整手册

[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)

本手册从游戏实际接口与配方导出：24个章节、127个独立函数／方法条目、28份配方、11个分层控制程序。示例在游戏中运行，不是在电脑系统Python中运行。

## 01 / 第一份收获

在世界首页选择“进入地球”。从一格花园、零库存开始，收获免费草，学习循环、种植和传感器，再沿原有作物路线推进。研究并行无人机后继续解锁“地球与群岛网络”，同时需要模块、字典和灌溉。

这是一条完整流程。地球是0号岛，原有库存就是母港仓库。花卉、矿物、鱼类群岛属于后期科技分支，物资通过实际运输连接；特产不会瞬间出现在家里。算法与策略影响耗时，没有固定等待认证。

```python
# Requires Loops. Stop when you have enough hay.
while True:
    harvest()
```

## 02 / 程序与状态

程序中心将文件和执行目标分开。先选择地球农务机、联合总控、岛屿管理、指定单机或机群，再选择文件。编辑只切换查看；“仅绑定”保存目标与文件的关系并停止目标旧代码；“绑定并运行”从头启动文件。同一台机、同一岛管理或联合总控最多运行一个解释器，不同目标可以并行。

绿色卡片表示活跃，琥珀边框表示正在编辑，绑定标签显示目标编号。正在运行的文件和它导入的库不能删除，至少保留一个文件。支持中文和数字开头命名，导入不填.py。

主工作台目标条显示运行按钮将影响谁，浮动编辑器也使用此目标。“停止此目标”保留其他运行；“全部停止”结束总控、管理和单机代码，并取消落地实体任务。保留撤销、重做、多窗口与外部编辑器。

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

地球农务机执行原有作物函数，操作同一片地球农田和原有库存，每台有独立坐标与变量。持久工作机通过fleet_build建造，再用program_bind、program_run绑定运行；原有spawn_drone(函数,参数...)在地球农务程序中创建临时子机，结束释放槽位，wait_for取得返回值。

机群用group_create(名称,[编号...])创建，再group_run(名称,文件)。各成员独立执行同一文件，通过fleet_self识别自身，不共享可变变量，协作需消息通信。管理程序不占实体无人机名额。农务机为1×1；专用机使用已研究的矩形实体通行。地球作物网格保留原有飞行器可跨越同格的规则。

## 15 / 排查与恢复

一直长不出作物：检查对应研究、种子库存、地面和空格。plant() 失败返回 False，在空地上等成熟不会产生作物。

物资够却研究不了：查看下一级所需作物发现和累计产量。执行速度与增产现在不能一次全部开放。

南瓜不能合并：检查每格是否枯死或未成熟。仙人掌不连收：找向东/北逆序或未成熟格。能量耗光：减少重复移动与扫描。迷宫卡住：使用 visited 和回溯，迷宫没有普通环绕。无人机 global 不返回：用 wait_for() 接收返回值。

用F9断点、暂停和单步逐行检查；语法错误带行号。备份文件以 .bak 结尾。农场库创建独立存档及程序目录。外部编辑器会导出 .py 和 __builtins__.py 提示并监视保存，避免两处同时改同一文件。音量滑条松开时保存。

更新后测试完整进度建议新建农场。旧规则获得的资源不会被追回。

## 16 / 接口速查

下方列出全部游戏接口、前置与返回值。命名空间包括 Entities、Items、Grounds、Unlocks、Hats、Leaderboards，大小写敏感。研究成功才从共享库存付费。查询到价格不代表已满足发现与产量条件；满级节点的价格和条件均返回 None。

其他章节的代码可能是已有田地或循环中的片段，请先满足文中的前提。主要作物的完整算法可从模板创建。

### `harvest()`

收获脚下成熟作物。南瓜、仙人掌可能触发区域收获；在迷宫宝藏上获得金币。

**参数**: 无参数。

**返回值**: bool：成功为True，未成熟或空地为False。

**解锁与使用条件**: 绑定地球农务机；第一格草无需科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
harvest()
```

### `can_harvest()`

只检查脚下成熟情况，不消耗收获动作。

**参数**: 无参数。

**返回值**: bool：可以收获返回True。

**解锁与使用条件**: 地球农务机。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
if can_harvest():
    harvest()
```

### `plant(entity)`

在空地或死亡南瓜上种植；草、灌木、树在草地，胡萝卜等需要耕地。

**参数**: entity：Entities中的作物枚举；大小写敏感。

**返回值**: bool：种植成功True；占用、缺种子、地面不符返回False，不额外扣费。

**解锁与使用条件**: 地球农务机；对应种植科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
if get_entity_type() == None:
    plant(Entities.Grass)
```

### `move(direction)`

按方向移动一格。普通田地边界环绕；迷宫墙与长尾会阻挡。

**参数**: direction：North、East、South或West，不要加引号。

**返回值**: bool：移动成功True。

**解锁与使用条件**: 地球农务机。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
move(East)
move(North)
```

### `can_move(direction)`

按相同墙与尾巴规则预判，不改变位置。

**参数**: direction：四个方向枚举之一。

**返回值**: bool。

**解锁与使用条件**: 地球农务机；传感器科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
if can_move(North):
    move(North)
```

### `till()`

切换脚下草地／耕地；不会种植。

**参数**: 无参数。

**返回值**: None。

**解锁与使用条件**: 地球农务机；种植科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
till()
```

### `use_item(item, amount=1)`

消耗地球库存的水、肥料或奇异物质。水每份增加0.25，最多1；肥料推进成熟并感染；奇异物质可切换感染或创建迷宫。

**参数**: item：Items.Water、Items.Fertilizer或Items.Weird_Substance；amount正整数，默认1。

**返回值**: bool：消耗成功True；库存或条件不足False。

**解锁与使用条件**: 地球农务机；物资及相关科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
if get_water() < 0.3:
    use_item(Items.Water, 1)
```

### `get_pos_x()`

返回本机在地球农田的横坐标。

**参数**: 无参数。

**返回值**: number：从0到边长减1。

**解锁与使用条件**: 地球农务机；传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
x = get_pos_x()
```

### `get_pos_y()`

返回本机在地球农田的纵坐标。

**参数**: 无参数。

**返回值**: number：从0到边长减1。

**解锁与使用条件**: 地球农务机；传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
y = get_pos_y()
```

### `get_world_size()`

返回原有地球农田边长；不是群岛20×14物流图尺寸。

**参数**: 无参数。

**返回值**: number。

**解锁与使用条件**: 地球农务机；传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
for i in range(get_world_size()):
    move(East)
```

### `get_entity_type()`

读取脚下作物或特殊实体。

**参数**: 无参数。

**返回值**: 实体标识字符串，空地为None。

**解锁与使用条件**: 地球农务机；传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
empty = get_entity_type() == None
```

### `get_ground_type()`

读取草地或耕地。

**参数**: 无参数。

**返回值**: Grounds.Grassland或Grounds.Soil对应值。

**解锁与使用条件**: 地球农务机；传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
if get_ground_type() != Grounds.Soil:
    till()
```

### `get_water()`

查询脚下水分，不会自动浇水。

**参数**: 无参数。

**返回值**: number：0～1。

**解锁与使用条件**: 地球农务机；传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
wet = get_water() >= 0.3
```

### `num_items(item)`

读取地球可用库存；与fleet_stock(0,item)为同一份物资。已预留运输货物不在可用库存中。

**参数**: item：Items枚举或物资标识字符串。

**返回值**: number：可用数量。

**解锁与使用条件**: 传感器；任意管理程序也可查询。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
available = num_items(Items.Wood)
```

### `num_produced(item)`

读取原有地球累计收获量。出售、研究、运输不会抹去累计收获。群岛产量见岛屿库存与订单。

**参数**: item：物资枚举或标识。

**返回值**: number。

**解锁与使用条件**: 传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
total = num_produced(Items.Carrot)
```

### `num_unlocked(research)`

查询已购科技等级。

**参数**: research：Unlocks枚举，如Unlocks.Archipelago。

**返回值**: number：未解锁为0。

**解锁与使用条件**: 无额外科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
opened = num_unlocked(Unlocks.Archipelago) > 0
```

### `get_cost(entity_or_research, level=-1)`

区分种子费用和科技费用；草种免费不等于干草增产免费。

**参数**: Entities返回种子费用；Unlocks返回研究费用；level可选，按已购等级从0起。

**返回值**: dict资源→数量；满级或不存在返回None。

**解锁与使用条件**: 成本查询科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
cost = get_cost(Unlocks.Flower_Island)
```

### `get_unlock_requirements(research)`

查看前置科技和累计产量要求，不包含需支付的价格。

**参数**: research：Unlocks枚举。

**返回值**: dict：unlock:<id>与produced:<item>；无下一等级为None。

**解锁与使用条件**: 成本查询科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
requirements = get_unlock_requirements(Unlocks.Archipelago)
```

### `can_unlock(research)`

同时检查前置、产量和库存，不扣费。

**参数**: research：Unlocks枚举。

**返回值**: bool。

**解锁与使用条件**: 成本查询科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
ready = can_unlock(Unlocks.Fish_Island)
```

### `unlock(research)`

支付资源并购买下一级。条件失败不扣物资；扩田会重置田地。

**参数**: research：Unlocks枚举。

**返回值**: bool：成功True。

**解锁与使用条件**: 自动科研科技；相关前置与物资。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
if can_unlock(Unlocks.Flower_Island):
    unlock(Unlocks.Flower_Island)
```

### `get_companion()`

查询脚下适用作物的伴生需求。

**参数**: 无参数。

**返回值**: (作物,(x,y))；不适用时None。

**解锁与使用条件**: 地球农务机；传感器与伴生种植。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
companion = get_companion()
```

### `measure(direction=None)`

太阳花花瓣数、仙人掌大小、迷宫宝藏坐标或下个苹果坐标。

**参数**: direction可省略；给方向时测量邻格。

**返回值**: number、坐标元组或None。

**解锁与使用条件**: 地球农务机；传感器。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
value = measure()
neighbor = measure(East)
```

### `swap(direction)`

交换脚下与相邻仙人掌地块，用于二维排序。

**参数**: direction：方向枚举。

**返回值**: bool：不满足条件False。

**解锁与使用条件**: 地球农务机；仙人掌科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
if measure() > measure(East):
    swap(East)
```

### `clear()`

清空并重新建立当前地球田地，未收获作物会损失。

**参数**: 无参数。

**返回值**: None。

**解锁与使用条件**: 地球农务机。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
# Only use when you intentionally reset the standing field.
clear()
```

### `set_world_size(side)`

临时调整地球农田边长，不能超过已购扩田。程序停止、结束或报错会恢复原边长。

**参数**: side：1～已购上限；0表示上限。

**返回值**: None；非法边长抛错。

**解锁与使用条件**: 地球农务机；世界调试工具。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
set_world_size(2)
```

### `set_execution_speed(speed)`

给本机执行速度设置上限，不会超出科技速度。

**参数**: speed：正数上限。

**返回值**: None。

**解锁与使用条件**: 地球农务机；世界调试工具。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
set_execution_speed(1)
```

### `change_hat(hat)`

更换帽子；进入恐龙帽模式开始长尾探索，离开时结算骨头。

**参数**: hat：Hats.Straw_Hat、Hats.Dinosaur_Hat、Hats.Top_Hat等。

**返回值**: bool。

**解锁与使用条件**: 地球农务机；恐龙帽／礼帽各有科技要求。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
change_hat(Hats.Straw_Hat)
```

### `get_time()`

查询当前地球世界秒数，不是现实电脑时间。

**参数**: 无参数。

**返回值**: number。

**解锁与使用条件**: 计时工具科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
start = get_time()
```

### `get_tick_count()`

读取当前程序的执行步数／指令计费计数，不能与游戏秒直接互换。

**参数**: 无参数。

**返回值**: number。

**解锁与使用条件**: 计时工具；绑定程序返回自身步数。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
steps = get_tick_count()
```

### `random()`

读取世界种子生成的随机数。

**参数**: 无参数。

**返回值**: number：[0,1)。

**解锁与使用条件**: 数值工具。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
choice = int(random() * 4)
```

### `print(values...)`

把一个或多个值写入日志；频繁输出会消耗执行预算。

**参数**: 任意数量可显示值，用逗号分隔。

**返回值**: None。

**解锁与使用条件**: 调试科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
print('stock', fleet_stock(0, 'wood'))
```

### `quick_print(values...)`

向同一日志写入多个值，适合调试；仍占程序执行预算。

**参数**: 任意数量可显示值。

**返回值**: None。

**解锁与使用条件**: 调试科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
quick_print('ready')
```

### `do_a_flip()`

消耗一个动作但不移动，可在原有农田脚本等待生长。

**参数**: 无参数。

**返回值**: None。

**解锁与使用条件**: 地球农务机。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
while not can_harvest():
    do_a_flip()
```

### `num_drones()`

在绑定程序中读取当前活动无人机代码数量，管理程序不占实体机名额。

**参数**: 无参数。

**返回值**: number。

**解锁与使用条件**: 绑定程序。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
active = num_drones()
```

### `max_drones()`

返回统一机队名额；已购并行科技和数量研究共同影响容量。

**参数**: 无参数。

**返回值**: number。

**解锁与使用条件**: 无额外要求。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
limit = max_drones()
```

### `spawn_drone(function, args...)`

地球农务机创建临时子机执行函数；内存独立复制，结束释放实体槽位。长期任务推荐program_bind。

**参数**: function：函数对象，不加括号；后面可传位置参数。

**返回值**: 绑定程序中返回数字机号；无名额None。

**解锁与使用条件**: 地球农务机；并行科技。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
def worker(n):
    return n * 2
child = spawn_drone(worker, 7)
if child != None:
    answer = wait_for(child)
```

### `has_finished(handle)`

不阻塞地检查临时子机是否结束。

**参数**: handle：spawn_drone返回值。

**返回值**: bool。

**解锁与使用条件**: 有效的子机句柄。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
done = has_finished(child)
```

### `wait_for(handle)`

等待子机结束并读取返回值，其他无人机和实体任务继续。

**参数**: handle：不能等待自己。

**返回值**: 任意返回值。

**解锁与使用条件**: 有效的子机句柄。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
answer = wait_for(child)
```

### `leaderboard_run(category, file, speedup)`

在独立竞速预设中运行文件并记录成绩，保留竞速功能；已移除模拟实验。

**参数**: category：Leaderboards枚举；file文件名不含.py；speedup保留参数。

**返回值**: None，达标且退出后记录成绩。

**解锁与使用条件**: 本地竞速科技；从竞速界面或地球农务机运行。

**失败与边界情况**: 参数个数、类型错误会停止调用程序；返回False表示条件未满足。

```python
leaderboard_run(Leaderboards.Hay, 'hay_race', 64)
```

### `len(value)`

字符串、列表、元组、集合或字典长度。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
size = len([1, 2])
```

### `str(value)`

转换为可显示字符串。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
text = str(123)
```

### `int(value)`

转整数，向零截断。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
n = int(3.7)
```

### `float(value)`

转数值。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
n = float('3.5')
```

### `bool(value)`

空值、0、空集合为False，其余为True。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
valid = bool([1])
```

### `abs(number)`

绝对值。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
distance = abs(-4)
```

### `min(values) / min(a, b, ...)`

非空数值集合或多个数值的最小值。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
n = min([2, 1, 3])
```

### `max(values) / max(a, b, ...)`

非空数值集合或多个数值的最大值。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
n = max(2, 7)
```

### `range(stop) / range(start, stop) / range(start, stop, step)`

不含终点的整数序列；step可负但不能0，最多100000项。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
for i in range(5, 0, -1):
    do_a_flip()
```

### `list() / list(iterable)`

创建可变列表或复制可迭代值。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
a = list((1, 2))
```

### `tuple() / tuple(iterable)`

创建不可变元组。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
position = tuple([2, 3])
```

### `set() / set(iterable)`

创建无重复集合。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
visited = set([1, 1, 2])
```

### `dict()`

创建空字典；非空字典用字面量。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
stock = dict()
stock['rose'] = 3
```

### `append(list, value)`

列表尾部追加；也可用list.append(value)。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
a = [1]
append(a, 2)
```

### `add(set, value)`

集合加入元素；也可用set.add(value)。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
seen = set()
add(seen, 2)
```

### `insert(list, index, value)`

在列表位置插入，超界位置被限制到合法范围。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
a = [1]
insert(a, 0, 9)
```

### `pop(collection, key_or_index) / pop(list)`

从列表弹出元素或从字典移除指定键并返回值。负列表索引从末尾起。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
a = [1, 2]
last = pop(a)
```

### `remove(collection, value)`

按值删除列表或集合成员；不存在会抛错。

**参数**: 参数及各种调用形式见签名与示例。

**返回值**: 返回转换值／集合／查询值；修改操作返回None，pop返回被移除值。

**解锁与使用条件**: 数值基本函数可直接用；集合语法需列表／字典科技。

**失败与边界情况**: 不适用类型、缺参数、越界或空min/max会抛错；集合最多100000项。

```python
a = [1, 2]
remove(a, 1)
```

### `list.index(value)`

查找首个相等元素位置，不存在抛错。

**参数**: 见签名；必须由集合对象调用。

**返回值**: 查询值、列表或None。

**解锁与使用条件**: 列表／字典科技。

**失败与边界情况**: 集合类型或参数不符会抛错。

```python
a = [3, 7]
i = a.index(7)
```

### `dict.keys()`

返回键列表。

**参数**: 见签名；必须由集合对象调用。

**返回值**: 查询值、列表或None。

**解锁与使用条件**: 列表／字典科技。

**失败与边界情况**: 集合类型或参数不符会抛错。

```python
a = {'x': 1}
keys = a.keys()
```

### `dict.values()`

返回值列表。

**参数**: 见签名；必须由集合对象调用。

**返回值**: 查询值、列表或None。

**解锁与使用条件**: 列表／字典科技。

**失败与边界情况**: 集合类型或参数不符会抛错。

```python
a = {'x': 1}
values = a.values()
```

### `dict.items()`

返回(键,值)元组列表，可在for里解包。

**参数**: 见签名；必须由集合对象调用。

**返回值**: 查询值、列表或None。

**解锁与使用条件**: 列表／字典科技。

**失败与边界情况**: 集合类型或参数不符会抛错。

```python
a = {'x': 1}
for key, value in a.items():
    n = value
```

### `dict.get(key, default=None)`

查询键，不存在时返回default，省略default为None。

**参数**: 见签名；必须由集合对象调用。

**返回值**: 查询值、列表或None。

**解锁与使用条件**: 列表／字典科技。

**失败与边界情况**: 集合类型或参数不符会抛错。

```python
a = {'x': 1}
value = a.get('missing', 0)
```

### `collection.clear()`

清空列表、集合或字典；不要与重置田地的clear()混淆。

**参数**: 见签名；必须由集合对象调用。

**返回值**: 查询值、列表或None。

**解锁与使用条件**: 列表／字典科技。

**失败与边界情况**: 集合类型或参数不符会抛错。

```python
a = [1, 2]
a.clear()
```

## 17 / 统一机队与通行

所有持久机体统一出现在机队列表中，包括地球农务机。容量由已购并行无人机科技和机队数量研究共同决定，数量、机体面积、角色分别研究。大机体的原料和产量按面积放大，耗时增幅较慢，但需要更宽的通道。

每机实体队列最多64项。宽桥采用单向环路，小机体可抄窄道，码头与降落检查整个占地。危险手动步进可能扣维修费；自动路径会等待、避让，不允许重叠。工程机由第6单或矿物岛科技开放，采收／渔业机由第7单或鱼类岛科技开放。配方需要正确机型。

## 18 / 地球与群岛物资网

地球0号岛使用原有库存，没有第二份仓库。fleet_stock(0,'wood')与num_items(Items.Wood)查询一致。订单、出口、作物种子与科研都使用同一份可用地球资源。运输接单即预留货物，分舱实际航行，到达后只卸货一次；特色产物运回地球就进入原有母港库存。

保留原有五座物流据点作为过渡。新增科技分支分别开放芳华6号、鎏金7号和沧澜8号，三者可独立解锁。用world_islands检查unlocked，不要认为“小于开放数量”的编号都能进入。原物流订单后增加六产物订单与跨岛循环订单，最终向地球联合交付。

## 19 / 总控、管理与子程序

在程序中心创建统一控制范例，把unified_controller绑定到联合总控。它启动earth_manager以及已开放的花卉、矿物、鱼类管理程序后结束，管理和单机继续独立运行。每岛最多一个管理程序，每台机持久保存自己的文件绑定。重复运行管理程序会复用匹配机体，也可能重启该机，因此派工前应检查已有队列。

专用子程序导入production_helpers，空闲时只排一批。先安排真实补给：玫瑰需要水与堆肥，鲤鱼需要干草与水。名额用尽时管理程序无法建造，要先释放名额、研究数量或重新分配机体。范例用来学习分层控制，不会自动完成全部订单。

实体队列、在途货物、文件绑定和机群成员会保存；解释器调用栈、收件箱、结果不保存。载入后重新启动需要的总控，停止父程序不会递归停止子程序。单机报错只停自身，可在卡片和program_state查看；单次执行预算防止死循环拖住其他解释器。

## 20 / 地球到三个特色岛

1. 进入地球，推进原有作物和语法科技。
2. 研究并行无人机、模块、字典和灌溉，以500宝藏金币购买地球与群岛网络，不再另收开拓费。
3. 可分别购买花卉岛（2000南瓜）、矿物岛（5000仙人掌）、鱼类岛（20000胡萝卜），三分支互相独立。
4. 在名额允许时建对应专用机和运输机。
5. 绑定程序，先把真实原料运输到岛，再等待产出。
6. 把特产运回地球，交付订单，扩大协作网络。

## 21 / 全部产物专属函数

每个专属函数只在指定岛排队生产，不会立即赠送产物。每批投入和产出按机体面积放大，下方配方耗时为基础1×1值。黄金矿料标识gold_ore，与原有迷宫宝藏金币gold分开；青铜是冶炼合金，并非直接挖到的天然矿；渔业批次代表养殖与捕捞作业，不是瞬间钓鱼。

### `grow_rose(id, batches=1)`

在6号岛排队生产玫瑰；等价于fleet_work(id, 6, 'rose', batches)。

**参数**: id为grower机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与grower机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 6 / grower / 3 rose / 24s; 2 water + 1 compost

```python
grow_rose(fleet_self(), 4)
```

### `grow_tulip(id, batches=1)`

在6号岛排队生产郁金香；等价于fleet_work(id, 6, 'tulip', batches)。

**参数**: id为grower机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与grower机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 6 / grower / 4 tulip / 30s; 2 water + 1 wood

```python
grow_tulip(fleet_self(), 4)
```

### `grow_lavender(id, batches=1)`

在6号岛排队生产薰衣草；等价于fleet_work(id, 6, 'lavender', batches)。

**参数**: id为grower机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与grower机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 6 / grower / 3 lavender / 28s; 1 water + 2 hay

```python
grow_lavender(fleet_self(), 4)
```

### `grow_lily(id, batches=1)`

在6号岛排队生产百合；等价于fleet_work(id, 6, 'lily', batches)。

**参数**: id为grower机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与grower机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 6 / grower / 2 lily / 40s; 3 water + 1 fertilizer

```python
grow_lily(fleet_self(), 4)
```

### `grow_orchid(id, batches=1)`

在6号岛排队生产兰花；等价于fleet_work(id, 6, 'orchid', batches)。

**参数**: id为grower机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与grower机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 6 / grower / 2 orchid / 65s; 2 water + 1 silver + 1 lavender

```python
grow_orchid(fleet_self(), 4)
```

### `grow_lotus(id, batches=1)`

在6号岛排队生产荷花；等价于fleet_work(id, 6, 'lotus', batches)。

**参数**: id为grower机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与grower机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 6 / grower / 2 lotus / 70s; 4 water + 1 fishmeal

```python
grow_lotus(fleet_self(), 4)
```

### `mine_stone(id, batches=1)`

在7号岛排队生产石头；等价于fleet_work(id, 7, 'stone', batches)。

**参数**: id为engineer机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与engineer机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 7 / engineer / 4 stone / 18s; 无原料

```python
mine_stone(fleet_self(), 4)
```

### `smelt_bronze(id, batches=1)`

在7号岛排队生产青铜；等价于fleet_work(id, 7, 'bronze', batches)。

**参数**: id为engineer机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与engineer机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 7 / engineer / 2 bronze / 32s; 3 stone + 2 wood

```python
smelt_bronze(fleet_self(), 4)
```

### `mine_silver(id, batches=1)`

在7号岛排队生产白银；等价于fleet_work(id, 7, 'silver', batches)。

**参数**: id为engineer机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与engineer机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 7 / engineer / 2 silver / 45s; 4 stone + 1 power

```python
mine_silver(fleet_self(), 4)
```

### `mine_gold(id, batches=1)`

在7号岛排队生产黄金矿料；等价于fleet_work(id, 7, 'gold_ore', batches)。

**参数**: id为engineer机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与engineer机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 7 / engineer / 1 gold_ore / 60s; 2 silver + 1 bronze + 2 power

```python
mine_gold(fleet_self(), 4)
```

### `refine_platinum(id, batches=1)`

在7号岛排队生产铂金；等价于fleet_work(id, 7, 'platinum', batches)。

**参数**: id为engineer机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与engineer机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 7 / engineer / 1 platinum / 85s; 1 gold_ore + 1 rose + 3 power

```python
refine_platinum(fleet_self(), 4)
```

### `cut_diamond(id, batches=1)`

在7号岛排队生产钻石；等价于fleet_work(id, 7, 'diamond', batches)。

**参数**: id为engineer机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与engineer机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 7 / engineer / 1 diamond / 110s; 1 platinum + 1 orchid + 4 power

```python
cut_diamond(fleet_self(), 4)
```

### `catch_carp(id, batches=1)`

在8号岛排队生产鲤鱼；等价于fleet_work(id, 8, 'carp', batches)。

**参数**: id为harvester机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与harvester机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 8 / harvester / 3 carp / 24s; 2 hay + 1 water

```python
catch_carp(fleet_self(), 4)
```

### `catch_trout(id, batches=1)`

在8号岛排队生产鳟鱼；等价于fleet_work(id, 8, 'trout', batches)。

**参数**: id为harvester机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与harvester机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 8 / harvester / 3 trout / 32s; 2 grain + 1 water

```python
catch_trout(fleet_self(), 4)
```

### `catch_salmon(id, batches=1)`

在8号岛排队生产鲑鱼；等价于fleet_work(id, 8, 'salmon', batches)。

**参数**: id为harvester机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与harvester机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 8 / harvester / 2 salmon / 45s; 1 lavender + 2 grain

```python
catch_salmon(fleet_self(), 4)
```

### `catch_cod(id, batches=1)`

在8号岛排队生产鳕鱼；等价于fleet_work(id, 8, 'cod', batches)。

**参数**: id为harvester机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与harvester机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 8 / harvester / 3 cod / 48s; 2 grain + 1 bronze

```python
catch_cod(fleet_self(), 4)
```

### `catch_tuna(id, batches=1)`

在8号岛排队生产金枪鱼；等价于fleet_work(id, 8, 'tuna', batches)。

**参数**: id为harvester机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与harvester机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 8 / harvester / 2 tuna / 70s; 1 salmon + 2 power

```python
catch_tuna(fleet_self(), 4)
```

### `catch_koi(id, batches=1)`

在8号岛排队生产锦鲤；等价于fleet_work(id, 8, 'koi', batches)。

**参数**: id为harvester机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与harvester机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 8 / harvester / 1 koi / 90s; 1 lotus + 2 carp

```python
catch_koi(fleet_self(), 4)
```

### `make_fishmeal(id, batches=1)`

在8号岛排队生产鱼粉；等价于fleet_work(id, 8, 'fishmeal', batches)。

**参数**: id为harvester机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与harvester机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 8 / harvester / 3 fishmeal / 18s; 2 carp

```python
make_fishmeal(fleet_self(), 4)
```

### `make_compost(id, batches=1)`

在0号岛排队生产堆肥；等价于fleet_work(id, 0, 'compost', batches)。

**参数**: id为grower机号；batches为正整数1～10000，省略为1。

**返回值**: bool：只是接单成功；产物在加工完成后入仓。

**解锁与使用条件**: 解锁目标岛与grower机型；查询product_info获取精确配方。

**失败与边界情况**: 错误机型、锁定岛、队列满或无效批数返回False；缺原料会等待，不赠送物资。

**配方**: island 0 / grower / 4 compost / 18s; 4 hay + 2 carrot

```python
make_compost(fleet_self(), 4)
```

## 22 / 全部网络与绑定函数

下方每个函数列出参数、返回值、解锁条件、失败行为和示例。机号应使用fleet_build返回的稳定编号，不是列表位置。岛管理用island_self，单机用fleet_self。队列接单成功不代表加工或运输已经完成。

只支持位置参数，不支持关键字参数；示例中的drone_id、hauler需要先取得现有机号。指定文件必须已在程序库中创建。

库存查询返回可用量；机体快照可查队列长度、货物、飞行、程序状态和报错。

### `fleet_start()`

确认群岛网络启用；需先在原有科技树购买地球与群岛网络。不会再扣第二次开拓费用。

**参数**: 无参数。

**返回值**: bool。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_start()
```

### `fleet_state()`

查看网络资金、名额、尺寸、订单及已解锁岛屿编号列表。islands是数量，unlocked_islands是可用编号。

**参数**: 无参数。

**返回值**: dict。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
state = fleet_state()
for island in state['unlocked_islands']:
    print(island)
```

### `fleet_order()`

读取当前多目标订单、库存、奖励、机体面积与并行要求。

**参数**: 无参数。

**返回值**: dict或None（全部完成）。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
order = fleet_order()
if order != None and order['ready']:
    fleet_submit()
```

### `fleet_units()`

返回全部统一无人机快照，含地球农务机、岛屿、尺寸、角色、绑定文件、代码状态、队列、货物和报错。

**参数**: 无参数。

**返回值**: list[dict]；id稳定，不等于列表索引。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
for drone in fleet_units():
    print(drone['id'], drone['program'])
```

### `fleet_stock(island, item)`

读取指定岛可用仓储；0号地球直接读取原有库存。运输预留后会立刻减少可用量。

**参数**: island：world_islands给出的编号；item物资标识或Items枚举。

**返回值**: number。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
wood = fleet_stock(0, 'wood')
```

### `fleet_build(role, width, height, island)`

使用网络资金建造持久无人机；类型、尺寸、数量各自检查。farmer仅能在地球建造1×1。

**参数**: role：farmer/irrigator/grower/hauler/engineer/scout/harvester；width,height为已解锁尺寸；island需解锁。

**返回值**: number机号；失败-1。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
drone = fleet_build('grower', 1, 1, 6)
```

### `fleet_remove(id)`

拆解已落地机体并按造价退款；归还队列原料、预留货物及持有货物，移除绑定与机群成员。

**参数**: id：机号。

**返回值**: bool；飞行中False。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
removed = fleet_remove(drone_id)
```

### `fleet_research(kind)`

购买数量、尺寸或类型研究。数量最多+5级；尺寸依次2×1、2×2、3×2、3×3；工程机可由矿物科技提前开放，渔业机可由鱼类科技提前开放。

**参数**: kind：slots、size或角色标识。

**返回值**: bool；门槛或资金不足False。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_research('engineer')
```

### `fleet_work(id, island, item, batches)`

通用生产入口，自动前往该岛设施，等待原料并逐批加工。专属产物函数封装了同一逻辑。

**参数**: id机号；island设施所在岛；item配方产物；batches整数1～10000。

**返回值**: bool：排队成功，不代表已经生产完成。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
accepted = fleet_work(drone_id, 7, 'stone', 4)
```

### `fleet_ship(id, source, destination, item, amount)`

预留真实源库存，运输机按载重逐舱装载、跨岛航行和卸货。数量大于一舱会返航重复运输。

**参数**: id运输机；source源岛；destination目标岛，不能相同；item物资；amount整数1～1000000。

**返回值**: bool：缺货、非运输机、队列满或岛未开False，不预留半批。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
ok = fleet_ship(hauler, 0, 6, 'water', 24)
```

### `fleet_rebase(id, destination)`

排队空机跨岛调动；农务机只在地球工作，不能迁出。

**参数**: id：机号；destination：已解锁岛。

**返回值**: bool。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_rebase(drone_id, 7)
```

### `fleet_route(id, x, y)`

在当前岛物流图排队前往坐标，规划完整机体占地。

**参数**: x 0～19；y 0～13；坐标不能落水或被固定地形阻挡。

**返回值**: bool；暂时拥堵等待通道。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_route(drone_id, 15, 2)
```

### `fleet_step(id, direction)`

低层手动一步；危险移动会急停并收维修费。优先使用路径队列。

**参数**: id：机号；direction方向枚举或同名字符串。

**返回值**: bool；繁忙、冷却或非法方向False。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_step(drone_id, East)
```

### `fleet_rotate(id)`

交换宽高，空间不足就恢复原朝向；只在落地空闲时可用。

**参数**: id：机号。

**返回值**: bool。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
rotated = fleet_rotate(drone_id)
```

### `fleet_cancel(id)`

取消落地实体队列、退还已预留原料，停止当前实体工作。不会停止其代码；活跃代码可能重新派工。

**参数**: id：机号。

**返回值**: bool；飞行中False。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
program_stop(drone_id)
fleet_cancel(drone_id)
```

### `fleet_submit()`

所有订单物资和并行指标达标时一次交付，扣除目标库存并发资金。无固定时长认证。

**参数**: 无参数。

**返回值**: bool。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
if fleet_order() != None and fleet_order()['ready']:
    fleet_submit()
```

### `fleet_wait(seconds)`

只暂停当前调用程序，其他代码、运输与生产继续推进。

**参数**: seconds：0～3600；最小调度粒度0.25游戏秒。

**返回值**: None。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_wait(1)
```

### `fleet_plan(id, x, y)`

查询忽略临时占用的地形路径，不移动机体；空列表可能表示不可达或已在终点。

**参数**: id机号；x,y物流坐标。

**返回值**: list[(x,y)]。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
path = fleet_plan(drone_id, 15, 2)
```

### `fleet_run(id, file)`

将文件绑定到单机并立即启动独立解释器；已有任务被同机替换，其他机保持。等价于绑定再运行。

**参数**: id：机号；file：现有文件名，不填.py。

**返回值**: bool。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_run(drone_id, 'rose_worker')
```

### `fleet_stop(id)`

停止该机代码并清理收件箱，保留文件绑定与实体队列。

**参数**: id：机号。

**返回值**: bool。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_stop(drone_id)
```

### `fleet_send(id, payload)`

向单机发送数据副本；收件箱最多16条，满时不丢旧消息而返回False。

**参数**: payload可为数字、字符串、列表、字典或组合；id为接收机号。

**返回值**: bool。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_send(hauler, {'from': 0, 'to': 6, 'item': 'water', 'amount': 12})
```

### `fleet_receive()`

弹出本机收件箱最早一条消息，非阻塞。总控与岛屿管理没有单机收件箱。

**参数**: 无参数。

**返回值**: 数据副本或None。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
message = fleet_receive()
```

### `fleet_self()`

查询绑定实体无人机编号。总控和岛屿管理返回-1。

**参数**: 无参数。

**返回值**: number。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
me = fleet_self()
```

### `fleet_result(id)`

读取已完成程序的result变量副本，未定义时读顶层返回值；运行中或无结果None。

**参数**: id：机号。

**返回值**: 任意结果副本；临时数据不保存。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
answer = fleet_result(drone_id)
```

### `fleet_map(island)`

读取20×14物流图、配方、设施坐标和已开放航线，区分宽桥与窄路。

**参数**: island：已解锁编号。

**返回值**: dict；rows_bottom_to_top中的#为不可通行。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
layout = fleet_map(7)
```

### `fleet_halt_all()`

停止全部总控、岛屿管理和单机程序，取消落地任务。飞行机先安全降落再归还货物。

**参数**: 无参数。

**返回值**: True。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
fleet_halt_all()
```

### `fleet_market()`

查询所有可出口物资的固定单价，用于比较余料收益；不等于高级产物越高就一定更赚。

**参数**: 无参数。

**返回值**: dict：物资→资金单价。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
prices = fleet_market()
```

### `fleet_sell(island, item, amount)`

从指定岛出售可用库存，资金保留两位小数；已预留和在途货物不可出售。

**参数**: island已开放；item物资；amount正整数。

**返回值**: number实际收入；失败0。

**解锁与使用条件**: 网络操作需地球与群岛科技；查询及地球农务基本操作可提前使用。

**失败与边界情况**: 参数错误抛错；bool失败不代表程序报错。每机实体队列最多64项。

```python
income = fleet_sell(7, 'stone', 20)
```

### `world_islands()`

列出全部稳定岛号，包含key、name、unlocked、research和管理文件。岛号不连续解锁，应检查unlocked。

**参数**: 无参数。

**返回值**: list[dict]。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
for island in world_islands():
    if island['unlocked']:
        print(island['id'], island['name'])
```

### `island_products(island)`

列出该岛可生产的产物标识，不能把其他岛特产当成本岛配方。

**参数**: island：岛号。

**返回值**: list[str]；未开放为空列表。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
products = island_products(6)
```

### `product_info(item)`

查看产物所在岛、专属函数、每批产量、耗时、可用机型、输入配方与出口价。大机体按面积放大投入和产出。

**参数**: item：字符串或Items枚举。

**返回值**: dict；无生产配方为None。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
recipe = product_info('orchid')
print(recipe['inputs'])
```

### `island_self()`

单机返回当前所在岛；岛屿管理返回负责岛；联合总控为-1。用于通用子程序。

**参数**: 无参数。

**返回值**: number。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
island = island_self()
```

### `program_bind(id, file)`

只绑定文件，不启动；若该机旧代码正在运行则停止旧代码。保留实体队列。

**参数**: id机号；file现有文件名。

**返回值**: bool；文件或机号不存在False。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
program_bind(drone_id, 'rose_worker')
```

### `program_run(id)`

从头启动该机已绑定文件；同机原任务被替换，不会同时运行两个解释器。

**参数**: id机号。

**返回值**: bool；未绑定或文件不存在False。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
program_run(drone_id)
```

### `program_stop(id)`

停止单机代码；绑定保留，实体工作继续。需要取消实体队列时另用fleet_cancel。

**参数**: id机号。

**返回值**: bool。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
program_stop(drone_id)
```

### `program_state(id)`

查询单机程序名、是否运行、报错与结果。

**参数**: id机号。

**返回值**: dict或None（机号不存在）。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
state = program_state(drone_id)
```

### `network_run(file)`

绑定并启动唯一联合总控；替换原联合总控，已启动的岛屿管理和单机不受停止影响。

**参数**: file现有文件名。

**返回值**: bool。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
network_run('unified_controller')
```

### `island_run(island, file)`

为一个岛绑定并启动唯一管理程序；不同岛独立运行。管理程序可安排多个无人机和导入库。

**参数**: island已解锁岛号；file现有文件名。

**返回值**: bool。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
island_run(6, 'flower_manager')
```

### `island_stop(island)`

仅停止该岛管理程序。它已派出的无人机保持运行。

**参数**: island岛号。

**返回值**: bool：有运行管理程序被停止为True。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
island_stop(6)
```

### `control_state()`

列出联合总控及各岛管理的scope、program、running与error。

**参数**: 无参数。

**返回值**: list[dict]。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
for control in control_state():
    print(control['scope'], control['running'])
```

### `group_create(name, ids)`

创建或更新持久机群，成员必须存在且不重复。机群是一组实体机，共用文件但各自有独立变量和解释器。

**参数**: name非空最多80字符；ids为机号列表。

**返回值**: bool；最多32个机群。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
group_create('flower_team', [rose_drone, tulip_drone])
```

### `group_units(name)`

查询机群成员；拆解无人机会移除对应成员。

**参数**: name机群名。

**返回值**: list[number]；不存在为空列表。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
members = group_units('flower_team')
```

### `group_run(name, file)`

向机群所有成员绑定并启动同一文件，可用fleet_self、island_self和消息分配不同工作。

**参数**: name现有机群；file现有文件。

**返回值**: bool；部分成员失败会返回False，已成功成员不会回滚。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
group_run('flower_team', 'rose_worker')
```

### `group_stop(name)`

停止机群成员的代码，保留绑定及实体队列。

**参数**: name机群名。

**返回值**: bool。

**解锁与使用条件**: 联合总控、岛管理与机群创建需要地球与群岛网络科技；目标岛必须解锁。

**失败与边界情况**: 非法参数抛错；不存在目标通常False或None，见返回值。

```python
group_stop('flower_team')
```

## 23 / 调用方式与恢复

直接调用：管理程序grow_rose(机号,4)。单机绑定：program_bind(机号,'rose_worker')，再program_run(机号)。机群调用：group_create('team',[a,b])，再group_run('team','worker')。模块导入：import production_helpers，每个解释器有独立模块变量。消息调用：fleet_send(机号,数据)，子程序fleet_receive接收。结果调用：有限子程序设置result，外部fleet_result(机号)读取。

不同产物可绑定不同文件，或同一文件按fleet_self/island_self分支，或通过消息接收目标配方。循环链从基础鲤鱼、薰衣草开始，之后做荷花、锦鲤；黄金矿料需要青铜和白银，铂金需要玫瑰，钻石需要兰花。

缺料时检查product_info、岛仓库存、已预留量、机型和岛屿解锁。堵塞时检查完整机体占地和码头，改派前取消已落地队列。若管理程序不停重启子机，先停管理程序或全部停止，再手动重绑定。

## 24 / 本地竞速与纪录

在纪录界面进入本地竞速，使用固定种子的预设独立农场；完成目标并结束程序后记录成绩。历史规则分开标记。已移除模拟实验室、其科技和simulate()，保留本地竞速。竞速不会重置正在经营的地球。

## 25 / 命名空间与物资编号

Entities选择地球原有作物，Items选择库存物资，Unlocks选择科技，不能把科技名当种子。North/East/South/West表示方向，Grounds表示地面，Hats表示外观或探索行为，Leaderboards表示本地竞速类别。

在fleet_stock、fleet_ship、product_info中可用物资标识字符串。Items.Rose等价于'rose'，Items.Gold_Ore等价于'gold_ore'；Items.Gold仍是宝藏金币。岛号固定：地球0，物流据点1～5，花卉6、矿物7、鱼类8；机号单独分配，不是岛号。

存档保存实体库存、任务、预留货物、绑定、机群和未读提示；调用栈、消息、结果为临时数据。升级时旧地球和旧0号岛库存只合并一次。与旧版本比较前请备份农场。

## 全部岛屿编号

| ID | 岛屿 | 科技 | 原物流订单门槛 |

|---|---|---|---|

| 0 | 地球·本地花园 | Earth | 0 |

| 1 | 苇泽水库 | archipelago | 2 |

| 2 | 余烬工坊 | archipelago | 5 |

| 3 | 琉光礁 | archipelago | 8 |

| 4 | 花汐温室 | archipelago | 11 |

| 5 | 天穹档案馆 | archipelago | 14 |

| 6 | 芳华·花卉群岛 | flower_island | 0 |

| 7 | 鎏金·矿物群岛 | mineral_island | 0 |

| 8 | 沧澜·鱼类群岛 | fish_island | 0 |

## 完整生产配方

以下为1×1基础值。投入与产出乘机体面积，实际加工秒数乘0.8＋0.2×sqrt(面积)，采收／渔业机再乘0.8。

| Item | Island | Role | Output | Seconds | Inputs | Function |

|---|---|---|---|---|---|---|

| water | 0 | irrigator | 4 | 8.0 | — | fleet_work |

| water | 1 | irrigator | 6 | 8.0 | — | fleet_work |

| grain | 0 | grower / harvester | 2 | 14.0 | 1 water | fleet_work |

| ore | 2 | engineer | 2 | 12.0 | — | fleet_work |

| alloy | 2 | engineer | 2 | 24.0 | 2 ore + 1 grain + 1 water | fleet_work |

| crystal | 3 | scout | 2 | 34.0 | 1 alloy + 2 water | fleet_work |

| medicine | 4 | grower | 2 | 42.0 | 2 grain + 1 crystal + 1 water | fleet_work |

| core | 5 | engineer | 1 | 60.0 | 2 alloy + 2 crystal + 1 medicine | fleet_work |

| compost | 0 | grower | 4 | 18.0 | 4 hay + 2 carrot | make_compost |

| rose | 6 | grower | 3 | 24.0 | 2 water + 1 compost | grow_rose |

| tulip | 6 | grower | 4 | 30.0 | 2 water + 1 wood | grow_tulip |

| lavender | 6 | grower | 3 | 28.0 | 1 water + 2 hay | grow_lavender |

| lily | 6 | grower | 2 | 40.0 | 3 water + 1 fertilizer | grow_lily |

| orchid | 6 | grower | 2 | 65.0 | 2 water + 1 silver + 1 lavender | grow_orchid |

| lotus | 6 | grower | 2 | 70.0 | 4 water + 1 fishmeal | grow_lotus |

| stone | 7 | engineer | 4 | 18.0 | — | mine_stone |

| bronze | 7 | engineer | 2 | 32.0 | 3 stone + 2 wood | smelt_bronze |

| silver | 7 | engineer | 2 | 45.0 | 4 stone + 1 power | mine_silver |

| gold_ore | 7 | engineer | 1 | 60.0 | 2 silver + 1 bronze + 2 power | mine_gold |

| platinum | 7 | engineer | 1 | 85.0 | 1 gold_ore + 1 rose + 3 power | refine_platinum |

| diamond | 7 | engineer | 1 | 110.0 | 1 platinum + 1 orchid + 4 power | cut_diamond |

| carp | 8 | harvester | 3 | 24.0 | 2 hay + 1 water | catch_carp |

| trout | 8 | harvester | 3 | 32.0 | 2 grain + 1 water | catch_trout |

| salmon | 8 | harvester | 2 | 45.0 | 1 lavender + 2 grain | catch_salmon |

| cod | 8 | harvester | 3 | 48.0 | 2 grain + 1 bronze | catch_cod |

| tuna | 8 | harvester | 2 | 70.0 | 1 salmon + 2 power | catch_tuna |

| koi | 8 | harvester | 1 | 90.0 | 1 lotus + 2 carp | catch_koi |

| fishmeal | 8 | harvester | 3 | 18.0 | 2 carp | make_fishmeal |

## 分层控制完整程序文件

在程序中心点击“创建统一控制范例”即可生成这些文件，把unified_controller绑定到联合总控。管理程序需要解锁目标岛、有空余机位与资金；补给需另行安排，范例展示调用层级，不会自动解决全部订单。

### unified_controller.py

```python
# Bind to Network. Start once; managers and workers remain independent.
island_run(0, 'earth_manager')
for island in world_islands():
    if island['unlocked']:
        if island['id'] == 6:
            island_run(6, 'flower_manager')
        elif island['id'] == 7:
            island_run(7, 'mineral_manager')
        elif island['id'] == 8:
            island_run(8, 'fish_manager')

```

### earth_manager.py

```python
# Bind to Earth manager. Reuse the existing farmer instead of building duplicates.
for drone in fleet_units():
    if drone['role'] == 'farmer':
        if not drone['program_running']:
            program_bind(drone['id'], 'earth_worker')
            program_run(drone['id'])
        break

```

### flower_manager.py

```python
# Reuse an existing machine; build only if a slot and credits are available.
worker = -1
for drone in fleet_units():
    if drone['role'] == 'grower' and drone['island'] == 6:
        worker = drone['id']
        break
if worker < 0:
    worker = fleet_build('grower', 1, 1, 6)
if worker >= 0:
    program_bind(worker, 'rose_worker')
    program_run(worker)
else:
    print('Need a free drone slot, credits or specialist research')

```

### mineral_manager.py

```python
fleet_research('engineer')
# Reuse an existing machine; build only if a slot and credits are available.
worker = -1
for drone in fleet_units():
    if drone['role'] == 'engineer' and drone['island'] == 7:
        worker = drone['id']
        break
if worker < 0:
    worker = fleet_build('engineer', 1, 1, 7)
if worker >= 0:
    program_bind(worker, 'stone_worker')
    program_run(worker)
else:
    print('Need a free drone slot, credits or specialist research')

```

### fish_manager.py

```python
fleet_research('harvester')
# Reuse an existing machine; build only if a slot and credits are available.
worker = -1
for drone in fleet_units():
    if drone['role'] == 'harvester' and drone['island'] == 8:
        worker = drone['id']
        break
if worker < 0:
    worker = fleet_build('harvester', 1, 1, 8)
if worker >= 0:
    program_bind(worker, 'carp_worker')
    program_run(worker)
else:
    print('Need a free drone slot, credits or specialist research')

```

### earth_worker.py

```python
# Earth farmer uses the original field and inventory.
while True:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type() == None:
                plant(Entities.Grass)
            move(East)
        move(North)

```

### rose_worker.py

```python
from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        grow_rose(fleet_self(), 1)
    fleet_wait(1)

```

### stone_worker.py

```python
from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        mine_stone(fleet_self(), 1)
    fleet_wait(1)

```

### carp_worker.py

```python
from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        catch_carp(fleet_self(), 1)
    fleet_wait(1)

```

### delivery_worker.py

```python
# Bind to a hauler. Commands come from an island or network manager.
while True:
    message = fleet_receive()
    if message != None:
        accepted = fleet_ship(fleet_self(), message['from'], message['to'], message['item'], message['amount'])
        print(accepted)
    fleet_wait(1)

```

### production_helpers.py

```python
# Import this library from managers or workers.
def idle(drone_id):
    for drone in fleet_units():
        if drone['id'] == drone_id:
            return drone['jobs'] == 0 and not drone['flying']
    return False

def shipment(hauler, source, target, item, amount):
    return fleet_send(hauler, {'from': source, 'to': target, 'item': item, 'amount': amount})

```

## 验证范围与存档行为

详见[验证报告](validation/README.md)。实体任务、货物、程序绑定和机群成员会保存，调用栈、收件箱和结果不保存；载入后重启相应总控。旧地球库存与旧0号岛仓库只合并一次。本版是本地测试版本，尚未发布GitHub。
