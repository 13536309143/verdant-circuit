# 地球与群岛统一流程 — 0.13.0

[完整函数手册](MANUAL.zh-CN.md) · [English](WORLD-0.13.0.md)

1. 进入地球，沿原有作物科技路线推进。
2. 解锁并行无人机、模块、字典和灌溉，再以500宝藏金币研究地球与群岛网络。
3. 在科技树分别开放花卉岛（2000南瓜）、矿物岛（5000仙人掌）或鱼类岛（20000胡萝卜）。
4. 程序中心左侧选目标，右侧选文件，再绑定并运行。
5. 创建统一控制范例，把unified_controller绑定到联合总控，它调用各岛管理，再绑定单机程序。
6. 实际运输地球原料，例如fleet_ship(运输机号, 0, 6, "water", 24)，等实际送达后再加工。
7. 建立跨岛高级配方，运输特产回地球，完成24份订单。

完整程序在programs/。游戏调用文件名不写.py。11个范例需要相应语法、岛屿和机型科技，只演示控制层级，不赠送资金、物资或研究。

### unified_controller

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

### earth_manager

```python
# Bind to Earth manager. Reuse the existing farmer instead of building duplicates.
for drone in fleet_units():
    if drone['role'] == 'farmer':
        if not drone['program_running']:
            program_bind(drone['id'], 'earth_worker')
            program_run(drone['id'])
        break

```

### flower_manager

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

### mineral_manager

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

### fish_manager

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

### earth_worker

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

### rose_worker

```python
from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        grow_rose(fleet_self(), 1)
    fleet_wait(1)

```

### stone_worker

```python
from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        mine_stone(fleet_self(), 1)
    fleet_wait(1)

```

### carp_worker

```python
from production_helpers import idle
# A batch consumes real inputs; arrange deliveries before production.
while True:
    if idle(fleet_self()):
        catch_carp(fleet_self(), 1)
    fleet_wait(1)

```

### delivery_worker

```python
# Bind to a hauler. Commands come from an island or network manager.
while True:
    message = fleet_receive()
    if message != None:
        accepted = fleet_ship(fleet_self(), message['from'], message['to'], message['item'], message['amount'])
        print(accepted)
    fleet_wait(1)

```

### production_helpers

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

