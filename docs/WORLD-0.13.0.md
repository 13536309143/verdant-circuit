# Unified Earth & Islands — 0.13.0

[Full function manual](MANUAL.md) · [中文](WORLD-0.13.0.zh-CN.md)

1. Enter Earth and develop the original crop route.
2. Research Parallel Drones, Modules, Dictionaries and Irrigation, then Archipelago (500 treasure gold).
3. Open Flowers (2000 pumpkin), Minerals (5000 cactus) and/or Fish (20000 carrot) from Research.
4. In Programs choose a target on the left, a file on the right, then Bind + run.
5. Create the unified example; bind unified_controller to Network. It calls island managers, which bind independent drone programs.
6. Ship actual Earth inputs. Example: fleet_ship(hauler, 0, 6, "water", 24). Wait for real delivery before processing.
7. Build cross-island recipes, return products and fulfill the 24 contracts.

Programs are stored in programs/. File names omit .py in game calls. The 11 examples require programming research and appropriate island/role unlocks. They teach control hierarchy; no money, resources or technology are granted.

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

