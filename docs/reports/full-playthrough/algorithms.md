# 本次实跑的算法与完整代码

这些代码来自种子 731 成功运行中的真实脚本，不是另写的伪代码。按相应阶段准备科技与库存后使用；`clear()` 会清理当前农田。每个阶段的准确库存见 `run-731/report.json` 的 before/after。

## 起步和遍历

一格自然成长→五份干草→解锁循环；之后逐行扫描，成熟收获。

复杂度：每轮 O(n²)。实跑阶段 23，主世界耗时 00:13.883；[原文件](run-731/programs/0023.py)。

```python
clear()
while num_items(Items.Hay)<20:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

## 棋盘林地

树与灌木交错，减少四邻树木减速；空地补种。

复杂度：每轮 O(n²)。实跑阶段 11，主世界耗时 00:23.300；[原文件](run-731/programs/0011.py)。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood)<20:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                if (get_pos_x()+get_pos_y())%2==0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            move(East)
        move(North)
```

## 胡萝卜与原料预算

先按当前种子成本和产量估算轮数，准备干草、木材；再种植、等待、补种。

复杂度：每轮 O(n²)。实跑阶段 69，主世界耗时 00:10.933；[原文件](run-731/programs/0069.py)。

```python
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Carrot)<100:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type()==None:
                plant(Entities.Carrot)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
            move(East)
        move(North)
```

## 巨型南瓜

反复扫描、补种枯死格；整块成熟再收获，不能提前拆散方阵。

复杂度：O(k·n²)，k 为修复轮数。实跑阶段 119，主世界耗时 00:24.667；[原文件](run-731/programs/0119.py)。

```python
clear()
while num_items(Items.Pumpkin)<100:
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

## 浇水和感染

水分低于 0.3 且库存有水才灌溉；成熟南瓜收获格施肥感染，产生奇异物质。

复杂度：每轮 O(n²)。实跑阶段 124，主世界耗时 00:40.867；[原文件](run-731/programs/0124.py)。

```python
clear()
while num_items(Items.Weird_Substance)<30:
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
    if complete and num_items(Items.Fertilizer)>0:
        use_item(Items.Fertilizer)
        harvest()
```

## 向日葵供能

先播完整田；等待时只读成熟状态，不重复移动。按 15→7 花瓣九档扫描，保持最大值优先。

复杂度：固定九档 O(n²) 扫描。实跑阶段 84，主世界耗时 00:22.800；[原文件](run-731/programs/0084.py)。

```python
clear()
while num_items(Items.Power)<1000:
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

## 仙人掌二维排序

成熟后先对每行冒泡，再对每列冒泡。列排序保留行间单调性，形成二维有序田，触发连收。

复杂度：O(n³) 比较/移动。实跑阶段 127，主世界耗时 00:04.167；[原文件](run-731/programs/0127.py)。

```python
clear()
while num_items(Items.Cactus)<500:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if get_ground_type()!=Grounds.Soil:
                till()
            if get_entity_type()==None:
                plant(Entities.Cactus)
            if get_water()<0.3 and num_items(Items.Water)>0:
                use_item(Items.Water)
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

## 迷宫搜索

用坐标集合去重，方向栈记录来路；遇到死路按反方向回溯。

复杂度：O(V+E)，网格图为 O(n²)。实跑阶段 129，主世界耗时 00:00.050；[原文件](run-731/programs/0129.py)。

```python
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

## 迷宫复用

到宝藏先使用奇异物质重置目标并开墙，再使用含 visited 的 DFS，支持有环地图。

复杂度：每轮 O(V+E)。实跑阶段 232，主世界耗时 00:15.567；[原文件](run-731/programs/0232.py)。

```python
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
```

## 长尾覆盖

偶数边长上的哈密顿回路：蛇行覆盖其余列，以第一列回到起点；满尾受阻后换帽结算。

复杂度：脚本方向决策 O(1)；引擎碰撞检查随尾长增加。实跑阶段 132，主世界耗时 00:02.600；[原文件](run-731/programs/0132.py)。

```python
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

## 伴生布局

读取偏好类型和坐标，在目标点布置伴生，再回原格成熟收获。

复杂度：单次移动 O(n)。实跑阶段 152，主世界耗时 00:03.033；[原文件](run-731/programs/0152.py)。

```python
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
```

## 32 台独立无人机

按列分工；生成失败时主机回退工作；等待句柄回收。压力段持续工作八游戏秒，实际峰值 32 台。

复杂度：单轮全田 O(n²)；压力段按固定时长重复。实跑阶段 233，主世界耗时 00:11.750；[原文件](run-731/programs/0233.py)。

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
```

## 固定种子模拟

在独立世界注入 rounds=64，随机决定动作；同种子耗时相同，换种子耗时改变。

复杂度：64 次试验。实跑阶段 154，主世界耗时 00:00.067；[原文件](run-731/programs/0154.py)。

```python
a=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
b=simulate('experiment', {}, {}, {'rounds':64}, 7, 64)
c=simulate('experiment', {}, {}, {'rounds':64}, 8, 64)
print(a==b)
```

## 竞速模块与最快重置

普通竞速代码保存在 [modules](run-731/modules/)；请在游戏“程序”中创建同名文件。`experiment` 供模拟调用，并由 globals 参数传入 rounds。

[最快重置完整程序](run-731/fastest-reset.py) 只用于本地 Fastest_Reset 预设。该预设按游戏规则开放语法，研究仍需正常生产和付款；不等于普通零存档可以直接越过语法研究。

## 资源规划和解释器实现

[测试调度器](test-runner/VerdantFullPlaythrough.cs) 负责按目标估算种子预算、切换程序和执行正常付费研究；[竞速生成器](test-runner/VerdantFullPlaythroughRaces.cs) 给出隔离竞速脚本。资源来自实际动作、成长和收获，不修改库存。
