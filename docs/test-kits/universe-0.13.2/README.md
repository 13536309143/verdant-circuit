# Four-planet universe — 0.13.2

[English](README.md) · [简体中文](README.zh-CN.md)

1. Choose Earth, initially the only unlocked planet. Harvest the first plot and develop the original crops.
2. Purchase Spaceflight after Parallel Drones, Modules, Dictionaries and Irrigation.
3. Research Flower, Ocean and Mineral planets. Enter one to receive its native starter worker.
4. Use Colony for local surface, count and hull research. Top navigation always switches between tools.
5. Use Programs to select Universe/planet/swarm/worker on the left and files on the right.
6. Create the 11 files below. Run universe_controller on Universe.
7. Build a UFO, send actual Earth water/compost/fish feed/solar energy, and check arrival through ufo_fleet.
8. Establish flowers, fish catches and mineral refining. Bring products back and fulfill 24 projects.
9. Full extents are360×128; read width and height independently, and never assume latitude wraps.

[Complete function manual](MANUAL.md)

This version uses a separate Automation/Universe0131 save namespace. Older farms remain intact. In orbit view, scroll to zoom and drag with the right mouse button to rotate. Focus worker gives a close view of active craft animation.

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

Night-only presentation is included in v0.13.2. See [Night validation](validation-night/README.md).
