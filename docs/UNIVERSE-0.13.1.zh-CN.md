# 四星宇宙 — 0.13.1 操作与代码

[English](UNIVERSE-0.13.1.md) · [简体中文](UNIVERSE-0.13.1.zh-CN.md)

1. 在宇宙首页选择地球，初始只有地球开放，从一格收获开始发展原有作物。
2. 在并行无人机、模块、字典和灌溉后研究星际航行。
3. 分别研究花卉、海洋和矿物星球，进入时获得各自本地初始载具。
4. 在殖民地面板研究表层范围、集群数量和机体尺寸。顶栏切换各工具。
5. 程序中心左选宇宙／星球／集群／单体，右选文件。
6. 创建下方11文件，把universe_controller绑定到宇宙总控运行。
7. 建UFO，实际运送地球水、堆肥、鱼饵和太阳能，通过ufo_fleet检查到货。
8. 建立花田、鱼群捕捞、矿物冶炼，回运产物并完成八份订单。
9. 满级360×128，宽高分别查询，纬度永远不环绕。

[完整函数手册](MANUAL.zh-CN.md)

本版使用独立Automation/Universe0131存档空间，旧农场保留。全景模式滚轮缩放、右键拖拽旋转；聚焦载具可近看扇翅、旋翼等动画。

### universe_controller.py

```python
# Bind to Universe. Managers stay alive independently.
planet_run(0, 'earth_manager')
for planet in universe_planets():
    if planet['unlocked']:
        if planet['id'] == 1:
            planet_run(1, 'flower_manager')
        elif planet['id'] == 2:
            planet_run(2, 'ocean_manager')
        elif planet['id'] == 3:
            planet_run(3, 'mineral_manager')

```

### earth_manager.py

```python
# One free initial native worker per unlocked planet.
worker = worker_starter(0)
if worker > 0:
    program_bind(worker, 'earth_worker')
    program_run(worker)

```

### flower_manager.py

```python
# One free initial native worker per unlocked planet.
worker = worker_starter(1)
if worker > 0:
    program_bind(worker, 'bee_worker')
    program_run(worker)

```

### ocean_manager.py

```python
# One free initial native worker per unlocked planet.
worker = worker_starter(2)
if worker > 0:
    program_bind(worker, 'boat_worker')
    program_run(worker)

```

### mineral_manager.py

```python
# One free initial native worker per unlocked planet.
worker = worker_starter(3)
if worker > 0:
    program_bind(worker, 'miner_worker')
    program_run(worker)

```

### earth_worker.py

```python
# Bounded latitude rows: sweep east, then return west at each row.
while True:
    for y in range(get_world_height()):
        for x in range(get_world_width()):
            if can_harvest():
                harvest()
            if x + 1 < get_world_width():
                move(East)
        for x in range(get_world_width() - 1):
            move(West)
        if y + 1 < get_world_height():
            move(North)
    for y in range(get_world_height() - 1):
        move(South)

```

### bee_worker.py

```python
from worker_helpers import idle
while True:
    if idle(worker_self()):
        cultivate_rose(worker_self(), 1)
    worker_wait(1)

```

### boat_worker.py

```python
from worker_helpers import idle
while True:
    if idle(worker_self()):
        catch_carp(worker_self(), 1)
    worker_wait(1)

```

### miner_worker.py

```python
from worker_helpers import idle
while True:
    if idle(worker_self()):
        mine_stone(worker_self(), 1)
    worker_wait(1)

```

### ufo_dispatch.py

```python
# Run from Universe, after Spaceflight and Flower Planet are researched.
ships = ufo_fleet()
if len(ships) == 0:
    ship = ufo_build()
else:
    ship = ships[0]['id']
if ship > 0:
    if planet_inventory(0, 'water') >= 12:
        ufo_transfer(ship, 0, 1, 'water', 12)

```

### worker_helpers.py

```python
def idle(id):
    state = worker_state(id)
    return state != None and state['jobs'] == 0

```
