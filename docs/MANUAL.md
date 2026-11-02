# Verdant Circuit 0.13.2 — Universe Manual

[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)

19 chapters · 218 individual function/method entries · 31 recipes · 11 complete program files

## 01 / Enter the universe

Only Earth is initially unlocked. Enter Earth, run harvest() after the first grass matures, and pay for Loops, Planting and Sensors. The original Earth crop progression is retained; Spaceflight is a later discovery. Other planets begin at one native plot when researched. This version uses a separate Universe0131 save directory so the previous live game is preserved.

```python
harvest()
```

## 02 / Spherical coordinates

Earth is the original farm on a sphere, not a second garden. Expansion stages are 1×1,2×2,4×4,8×8,16×16,32×32,64×64,128×128,256×128,360×128. Each cell spans one degree. Full latitude coverage is the belt -64° to+64°, excluding both poles. Only a fully unlocked 360-column surface wraps longitude; latitude never wraps. Expansion centers the old patch, keeps plants and shifts local coordinates. Read current coordinates after expanding. Use get_world_width and get_world_height separately.

```python
width = get_world_width()
height = get_world_height()
```

## 03 / Research & expansion

Original crop discoveries gate Earth expansion, speed and yield. The last four Earth expansion tiers need Spaceflight, Flower Planet, Mineral Planet and Ocean Planet. Buy Spaceflight after Parallel Drones, Modules, Dictionaries and Irrigation. Flower Planet costs2000 pumpkin; Ocean Planet20000 carrot; Mineral Planet5000 cactus. Exterior surfaces expand using local produce: price20×next stage squared, paid in rose,carp or stone. No elapsed-time certificate forces a wait.

```python
requirements = get_unlock_requirements(Unlocks.Spaceflight)
```

## 04 / Original Earth crops

Keep original crop mechanics: tree spacing, companions, water and infection; giant pumpkin squares, dead replacements; sunflower petal priority; sorted cactus regions; connected mazes; dinosaur tails and bone settlement. Treasure produces coins, while gold is a refined mineral. Crop movement now respects spherical edges. Re-read rectangular extents and reset to the beginning after a sweep. Expanded grids preserve planted crops.

```python
if get_water() < 0.3:
    use_item(Items.Water)
```

## 05 / Python-inspired language

The game interpreter is a documented subset, not system Python. It supports loops, conditionals, functions, closures, defaults, positional arguments, collections, modules and return values. It has no OS/file/network/stdlib access, exceptions or keyword arguments. New sum/enumerate/zip/sorted/reversed/round/all/any/pow/divmod/type/Unicode helpers and string methods are documented individually. Collection operations return snapshots rather than Python lazy iterators/views. Numbers use a shared numeric representation.

```python
for index, value in enumerate([10, 20], 1):
    print(index, value)
```

## 06 / Program targets

Top navigation opens Universe, Colony, Research, Programs, Manual, UFO, Records and Settings. Program center targets and files scroll independently. A worker, a planet manager and the universe controller each have at most one interpreter. Bind only stops and associates; Bind+run starts. Browsing another file never replaces running code. Green means running; an amber border means editing. At least one file remains. Numeric and Chinese file names are supported.

```python
program_bind(id, "bee_worker")
program_run(id)
```

## 07 / Native craft & hulls

Earth has drones, Floralia bees, Pelagia fishing boats, Aurelia mining drones. They cannot change planets. Each planet has separate slotTier and hullTier variables; both require paid local research. Hulls1×1,2×1,2×2,3×2,3×3 occupy real cells and must fit unlocked space. Worker paths reserve full footprints and queue when blocked. Bees flap wings, drones spin rotors, boats animate propellers and mining drones animate drills.

```python
worker_research(1, "slots")
worker_research(1, "size")
```

## 08 / Floralia botanical colony

Bees manage rose,tulip,lavender,lily,orchid and lotus batches; honey is collected from rose resources. Rose works on the first plot, tulip/lavender need width2, and lily/orchid/lotus width4. Water and compost arrive by UFO. Orchids need refined silver; lotus needs fishmeal. Inputs and outputs scale with hull area, while batch time uses0.8+0.2×sqrt(area).

```python
cultivate_rose(worker_self(), 4)
```

## 09 / Pelagia fisheries

Boats catch carp,trout,salmon,cod,tuna and koi using fish feed; koi also require lotus. Shoals are spatial: planet_scan reveals the species and a boat navigates to a matching accessible cell before processing. More species become available as the surface expands. Fishmeal processes caught carp. None of these functions creates free fish without input or time.

```python
tile = planet_scan(2, 0, 0)
catch_carp(worker_self(), 2)
```

## 10 / Aurelia mining & refining

Mining produces stone, copper_ore,tin_ore,silver_ore,gold_ore,platinum_ore and rough_diamond. Native drones navigate to matching deposits. Smelting produces copper,tin,bronze,silver,gold; platinum is refined and diamond is cut. Bronze consumes3 copper+1 tin+solar energy, not stone. Platinum needs roses and diamond cutting needs orchids. Deposits are renewable batch sources in this edition; energy is consumed for each mining/refining batch.

```python
mine_copper_ore(worker_self(), 2)
```

## 11 / UFO freight & energy

Only UFOs move items between planets. Build costs50 credits+20wood+10solar energy on Earth and provides120 battery. Launch atomically reserves full stock and battery: ceil(amount/capacity)×(2+planet-ID distance). Each flight leg lasts12+6×planet-ID distance seconds. Capacities24,72,144,288 and1–4 ships are researched independently using returned bronze/silver/platinum. Refuel at the current dock with local solar energy:5 battery/unit, maximum1000. Cancel refunds unused stock immediately; airborne cargo finishes its current leg and deposits at destination.

```python
ship = ufo_build()
if ship > 0:
    ufo_transfer(ship, 0, 1, "water", 24)
```

## 12 / Local swarms & messages

swarm_create names unique worker IDs from one planet. Several groups may coexist, and one file can run as independent copies. Variables and module instances are isolated; messages are deep-copied and inboxes hold16 values. fleet_receive is nonblocking and returns None when empty. Use separate planet managers and a universe program to coordinate different swarms.

```python
swarm_create("bee_team", [a, b])
swarm_run("bee_team", "bee_worker")
```

## 13 / Controller hierarchy

Create the 11-file universe example. Run universe_controller on Universe; it launches managers for unlocked planets. Each manager binds its native starter worker. Parent completion does not stop children. Managers do not consume worker slots. Worker errors stop only that interpreter. Physical work queues may continue after code stops; use worker_cancel or stop_all deliberately.

```python
planet_run(1, "flower_manager")
planet_run(2, "ocean_manager")
```

## 18 / Mothership, landing & freight

Locked worlds appear grey, each rotating on its own randomized axis. Select an unlocked planet to watch the shuttle descend from the mothership. Native workers stay local. Every interplanetary UFO route passes through the mother; use depot -1 to store and retrieve supplies. Cancellation deposits airborne cargo at its next dock, which may be the mother. Local solar energy is required to refuel.

```python
ufo_transfer(ship, 0, -1, 'water', 24)
ufo_wait(ship)
print(mothership_inventory('water'))
```

## 19 / Native research, growth & effects

Research has Universe, Earth, Flower, Ocean and Mineral sectors. Planet discovery is in Universe. Green is production, blue automation, amber ecology, violet transport. Dark nodes are unresearched; bright nodes have a purchased level. Native research changes real recipe speed/yields; costs double per level, five levels per branch. Growth now changes crop shape continuously. A close-up is selected for small fields; harvest bursts use a bounded shared pool. Full-size surfaces cull distant/back-facing crop chunks and batch the visible instances. There is no fixed campaign timer gate.

```python
nodes = planet_technologies(1)
planet_research(1, 'pollination')
```

## 14 / Saves, compatibility & debugging

0.13.2 saves are isolated under Automation/Universe0131. Old farm files are retained and not silently converted into a different universe. Save physical cargo, queues, battery, surfaces, swarms and file bindings. Interpreter stacks,inboxes and result objects are transient; restart controllers after loading and inspect existing queues. Original race farms remain separate. Ctrl/Cmd+Z undo, Shift+Ctrl/Cmd+Z redo, F9 breakpoints and external editing remain.

```python
state = worker_state(id)
```

## 15 / Earth & Python reference

Every retained Earth function and Python-inspired helper has an individual entry below, including defaults, results and examples. Keys are case-sensitive. No keyword arguments. get_world_size is a compatibility width query, not a rectangular height.

### `harvest()`

Harvest a mature crop; at treasure, award gold. Immature/empty plots return False.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
harvest()
```

### `can_harvest()`

Test maturity at the current plot; does not harvest.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if can_harvest():
    harvest()
```

### `plant(entity)`

Planting/crop research and seed inputs required; False if occupied or wrong ground.

**Parameters**: entity: Entities member

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if get_entity_type() == None:
    plant(Entities.Grass)
```

### `move(direction)`

Move one Earth cell. Locked edges block movement. Longitude wraps only at 360 columns; latitude never wraps.

**Parameters**: direction: North, East, South or West

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
move(East)
move(North)
```

### `can_move(direction)`

Sensors. Test the same walls/tail rules without moving.

**Parameters**: direction: North, East, South or West

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if can_move(North):
    move(North)
```

### `till()`

Switch grassland and soil; use before planting soil crops.

**Parameters**: No arguments.

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
till()
```

### `use_item(item, amount=1)`

Consume Water, Fertilizer or Weird_Substance; False if unsuitable or insufficient.

**Parameters**: item: cargo identifier or Items member; amount: positive integer, shipping/export maximum 1000000

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if get_water() < 0.3:
    use_item(Items.Water, 1)
```

### `get_pos_x()`

Sensors. Zero-based position and current field side length.

**Parameters**: No arguments.

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
x = get_pos_x()
```

### `get_pos_y()`

Sensors. Zero-based position and current field side length.

**Parameters**: No arguments.

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
y = get_pos_y()
```

### `get_world_size()`

Legacy Earth width query. It does not report height. Use get_world_width and get_world_height in rectangular surface programs.

**Parameters**: No arguments.

**Returns**: number

**Requires**: Bound native worker or planet manager. Universe context must use planet_surface(id).

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for i in range(get_world_size()):
    move(East)
```

### `get_entity_type()`

Sensors. Return an entity, None, or the ground namespace value.

**Parameters**: No arguments.

**Returns**: entity string or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
empty = get_entity_type() == None
```

### `get_ground_type()`

Sensors. Return an entity, None, or the ground namespace value.

**Parameters**: No arguments.

**Returns**: ground identifier

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if get_ground_type() != Grounds.Soil:
    till()
```

### `get_water()`

Sensors. Current moisture in [0,1].

**Parameters**: No arguments.

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
wet = get_water() >= 0.3
```

### `num_items(item)`

Sensors. Spendable current inventory, shared between drones.

**Parameters**: item: cargo identifier or Items member

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
available = num_items(Items.Wood)
```

### `num_unlocked(research)`

Purchased level of the named research.

**Parameters**: research: Unlocks member

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
opened = num_unlocked(Unlocks.Spaceflight) > 0
```

### `get_cost(entity_or_research, level=-1)`

Costs. Entity gives planting inputs; Unlocks gives next research cost. Optional level is zero-based purchased-level index.

**Parameters**: entity_or_research: Entities for seeds, Unlocks for research; level: optional zero-based purchased-level index; default -1 means current

**Returns**: resource-cost dict or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
cost = get_cost(Unlocks.Flower_Planet)
```

### `get_unlock_requirements(research)`

Costs. Keys unlock:<id> and produced:<item> give required discovery levels and cumulative production. Does not include payable price.

**Parameters**: research: Unlocks member

**Returns**: requirements dict or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
requirements = get_unlock_requirements(Unlocks.Spaceflight)
```

### `can_unlock(research)`

Costs. True only when discoveries, production totals and funds all suffice.

**Parameters**: research: Unlocks member

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
ready = can_unlock(Unlocks.Ocean_Planet)
```

### `num_produced(item)`

Sensors. Cumulative harvested production, including harvested Weird Substance; spending does not reduce it. Passive water/fertilizer supplies use num_items().

**Parameters**: item: cargo identifier or Items member

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
total = num_produced(Items.Carrot)
```

### `unlock(research)`

Automatic research. Pay and purchase the next level; missing requirements return False with no payment.

**Parameters**: research: Unlocks member

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if can_unlock(Unlocks.Flower_Planet):
    unlock(Unlocks.Flower_Planet)
```

### `get_companion()`

Sensors and companion research. Returns (entity, (x,y)) for eligible plants.

**Parameters**: No arguments.

**Returns**: (entity,(x,y)) or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
companion = get_companion()
```

### `measure(direction=None)`

Sensors. Flower petals/cactus size; maze treasure or next-apple coordinates. None when unsupported.

**Parameters**: direction: North, East, South or West

**Returns**: number, coordinate tuple or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
value = measure()
neighbor = measure(East)
```

### `swap(direction)`

Exchange adjacent cactus cells; does not buy seeds.

**Parameters**: direction: North, East, South or West

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if measure() > measure(East):
    swap(East)
```

### `clear()`

clear resets the current field and loses standing plants. do_a_flip consumes an action without moving.

**Parameters**: No arguments.

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
# Only use when you intentionally reset the standing field.
clear()
```

### `set_world_size(side)`

Temporary field resizing is available only in isolated classic sessions. Live spherical colonies grow through planet_expand and research, preserving cells.

**Parameters**: side: 1 to purchased maximum; 0 means maximum

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
set_world_size(2)
```

### `set_execution_speed(speed)`

World testing tools. Cap current execution speed; does not exceed purchased speed.

**Parameters**: speed: positive execution cap, never above research speed

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
set_execution_speed(1)
```

### `change_hat(hat)`

Select a hat; Dinosaur_Hat and Top_Hat require their research. Leaving tail mode settles bone.

**Parameters**: hat: Hats member; special hats require research

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
change_hat(Hats.Straw_Hat)
```

### `get_time()`

Timing tools. Current world seconds / this drone's instruction cost count.

**Parameters**: No arguments.

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
start = get_time()
```

### `get_tick_count()`

Timing tools. Current world seconds / this drone's instruction cost count.

**Parameters**: No arguments.

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
steps = get_tick_count()
```

### `random()`

Numeric utilities. Seeded uniform value in [0,1).

**Parameters**: No arguments.

**Returns**: number in [0,1)

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
choice = int(random() * 4)
```

### `print(values...)`

Debugging. Write to the journal. Frequent print adds execution cost.

**Parameters**: values...: zero or more script values

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
print('stock', fleet_stock(0, 'wood'))
```

### `quick_print(values...)`

Debugging. Write to the journal. Frequent print adds execution cost.

**Parameters**: values...: zero or more script values

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
quick_print('ready')
```

### `do_a_flip()`

clear resets the current field and loses standing plants. do_a_flip consumes an action without moving.

**Parameters**: No arguments.

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
while not can_harvest():
    do_a_flip()
```

### `num_drones()`

Count active workers, including the main task, and available capacity.

**Parameters**: No arguments.

**Returns**: active drone-code count

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
active = num_drones()
```

### `max_drones()`

Count active workers, including the main task, and available capacity.

**Parameters**: No arguments.

**Returns**: unified slot capacity

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
limit = max_drones()
```

### `spawn_drone(function, args...)`

From an Earth farmer, create a temporary independent farmer that executes a function. Completion releases its slot. Use permanent bindings for long-running tasks.

**Parameters**: function: function object, without calling it; args...: optional positional arguments

**Returns**: numeric drone ID or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
def worker(n):
    return n * 2
child = spawn_drone(worker, 7)
if child != None:
    answer = wait_for(child)
```

### `has_finished(handle)`

Check a worker without blocking.

**Parameters**: handle: spawn_drone result; cannot wait for yourself

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
done = has_finished(child)
```

### `wait_for(handle)`

Wait and return the worker result; the field and other workers keep advancing.

**Parameters**: handle: spawn_drone result; cannot wait for yourself

**Returns**: the worker return value

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
answer = wait_for(child)
```

### `leaderboard_run(category, file, speedup)`

Local competition research. Run a named program in a fixed preset and record success after termination.

**Parameters**: category: Leaderboards member; file: existing program name without .py; speedup: retained compatibility argument

**Returns**: None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
leaderboard_run(Leaderboards.Hay, 'hay_race', 64)
```

### `len(value)`

Return length.

**Parameters**: value: a script value

**Returns**: length as a number

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
size = len([1, 2])
```

### `str(value)`

Convert to a display string.

**Parameters**: value: a script value

**Returns**: string

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
text = str(123)
```

### `int(value)`

Convert to a number and truncate toward zero.

**Parameters**: value: a script value

**Returns**: number truncated toward zero

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
n = int(3.7)
```

### `float(value)`

Convert to a number.

**Parameters**: value: a script value

**Returns**: number

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
n = float('3.5')
```

### `bool(value)`

Return False for None, zero and empty collections; True otherwise.

**Parameters**: value: a script value

**Returns**: bool

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
valid = bool([1])
```

### `abs(number)`

Return absolute value.

**Parameters**: number: a numeric value

**Returns**: nonnegative number

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
distance = abs(-4)
```

### `min(values) / min(a, b, ...)`

Return the smallest numeric value from a nonempty collection or positional numbers.

**Parameters**: values: see signature and example

**Returns**: smallest number

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
n = min([2, 1, 3])
```

### `max(values) / max(a, b, ...)`

Return the largest numeric value from a nonempty collection or positional numbers.

**Parameters**: values: see signature and example

**Returns**: largest number

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
n = max(2, 7)
```

### `range(stop) / range(start, stop) / range(start, stop, step)`

Produce an integer list excluding the end; negative steps are supported, zero is rejected, at most 100000 entries.

**Parameters**: stop: exclusive integer end

**Returns**: integer list, excluding stop

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for i in range(5, 0, -1):
    do_a_flip()
```

### `list() / list(iterable)`

Create an empty mutable list or copy an iterable.

**Parameters**: No arguments.

**Returns**: new list

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = list((1, 2))
```

### `tuple() / tuple(iterable)`

Create an immutable tuple from an optional iterable.

**Parameters**: No arguments.

**Returns**: new immutable tuple

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
position = tuple([2, 3])
```

### `set() / set(iterable)`

Create a deduplicated set from an optional iterable.

**Parameters**: No arguments.

**Returns**: new deduplicated set

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
visited = set([1, 1, 2])
```

### `dict()`

Create an empty dictionary. Use a literal for nonempty dictionaries.

**Parameters**: No arguments.

**Returns**: empty dictionary

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
stock = dict()
stock['rose'] = 3
```

### `append(list, value)`

Append to a list; list.append(value) is also supported.

**Parameters**: list: see signature and example; value: a script value

**Returns**: None

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = [1]
append(a, 2)
```

### `add(set, value)`

Add to a set; set.add(value) is also supported.

**Parameters**: set: see signature and example; value: a script value

**Returns**: None

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
seen = set()
add(seen, 2)
```

### `insert(list, index, value)`

Insert into a list at a clamped index.

**Parameters**: list: see signature and example; index: see signature and example; value: a script value

**Returns**: None

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = [1]
insert(a, 0, 9)
```

### `pop(collection, key_or_index) / pop(list)`

Remove and return a list entry or dictionary key. A list defaults to its last entry; negative indices work.

**Parameters**: collection: list, set or dictionary as appropriate; key_or_index: dictionary key or list index

**Returns**: removed value

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = [1, 2]
last = pop(a)
```

### `remove(collection, value)`

Remove a matching list or set value. Missing values raise an error.

**Parameters**: collection: list, set or dictionary as appropriate; value: a script value

**Returns**: None

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = [1, 2]
remove(a, 1)
```

### `list.index(value)`

Find the first matching list entry. Missing values raise an error.

**Parameters**: value: a script value

**Returns**: first matching list index

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = [3, 7]
i = a.index(7)
```

### `dict.keys()`

Return dictionary keys as a list.

**Parameters**: No arguments.

**Returns**: key list

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = {'x': 1}
keys = a.keys()
```

### `dict.values()`

Return dictionary values as a list.

**Parameters**: No arguments.

**Returns**: value list

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = {'x': 1}
values = a.values()
```

### `dict.items()`

Return dictionary (key,value) pairs as a list; pairs can be unpacked in a for loop.

**Parameters**: No arguments.

**Returns**: list of (key,value) tuples

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = {'x': 1}
for key, value in a.items():
    n = value
```

### `dict.get(key, default=None)`

Read a dictionary key, falling back to the supplied default or None.

**Parameters**: key: see signature and example; default: see signature and example

**Returns**: value, supplied default, or None

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = {'x': 1}
value = a.get('missing', 0)
```

### `collection.clear()`

Clear a list, set or dictionary. This is distinct from the Earth clear() operation.

**Parameters**: No arguments.

**Returns**: None

**Requires**: Collections require Lists or Dictionaries research; numeric conversions are built in.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
a = [1, 2]
a.clear()
```

### `sum(values, start=0)`

Sum numeric items and an optional start.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = sum([1,2,3], 4)
```

### `enumerate(values, start=0)`

Return index/value tuples.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = enumerate([10,20], 1)
```

### `zip(iterable, ...)`

Return tuples up to the shortest input; zero inputs gives an empty list.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = zip([1,2], [3])
```

### `sorted(values, reverse=False)`

Stable sort of uniformly numeric or string values; positional reverse is a game extension. Original unchanged.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = sorted([3,1,2], True)
```

### `reversed(values)`

Return a reversed list copy.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = reversed([1,2])
```

### `round(number, digits=0)`

Round ties to even, digits -12 to12.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = round(2.5)
```

### `all(values)`

All truthy; empty returns True.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = all([])
```

### `any(values)`

Any truthy; empty returns False.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = any([])
```

### `pow(base, exponent)`

Finite numeric exponentiation, no modulus overload.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = pow(2, 8)
```

### `divmod(a, b)`

Floor quotient and matching remainder, including negative dividends.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = divmod(-7, 3)
```

### `type(value)`

Return a type-name string; script numbers use number.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = type([1])
```

### `ord(character)`

Unicode scalar codepoint, accepts one supplementary character.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = ord('中')
```

### `chr(codepoint)`

Convert a valid Unicode scalar; reject surrogate/invalid values.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = chr(20013)
```

### `bin(integer)`

Signed binary prefix string.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = bin(-5)
```

### `hex(integer)`

Signed lowercase hexadecimal prefix string.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = hex(255)
```

### `oct(integer)`

Signed octal prefix string.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Language collection research as required by the expression.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = oct(8)
```

### `text.lower()`

Invariant lowercase copy.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'ROSE'.lower()
```

### `text.upper()`

Invariant uppercase copy.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'rose'.upper()
```

### `text.strip()`

Trim whitespace at both ends.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = ' a '.strip()
```

### `text.split(separator=None)`

Whitespace splitting or an explicit nonempty separator.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'a,b'.split(',')
```

### `separator.join(strings)`

Join a sequence of strings; reject nonstring items.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = '-'.join(['a','b'])
```

### `text.replace(old, new)`

Replace all exact occurrences.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'rose'.replace('r','R')
```

### `text.startswith(prefix)`

Ordinal prefix test.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'rose'.startswith('ro')
```

### `text.endswith(suffix)`

Ordinal suffix test.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'rose'.endswith('se')
```

### `text.find(fragment)`

First UTF-16 string index or -1.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'rose'.find('s')
```

### `text.count(fragment)`

Count nonoverlapping matches; empty fragment returns length+1.

**Parameters**: See the signature; use positional arguments.

**Returns**: Value described above; commands return acceptance, not completion.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
result = 'aaaa'.count('aa')
```

## 16 / Universe, workers & UFO API

Use universe_ for global management, planet_ for a colony, worker_ for native craft, swarm_ for local groups and ufo_ exclusively for interplanetary freight. Kept fleet_/island_ names are compatibility spellings and are documented separately. fleet_ship and fleet_rebase are retired, not teleport shortcuts.

### `universe_planets()`

List exactly four planets and their unlocked flags.

**Parameters**: No arguments.

**Returns**: list[dict] with four stable IDs and unlocked flags

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
for planet in universe_planets():
    if planet['unlocked']:
        print(planet['id'], planet['name'])
```

### `universe_state()`

Read world time, credit balance, contract and worker totals.

**Parameters**: No arguments.

**Returns**: dict with clock,credits,contract,worker totals

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
state = universe_state()
```

### `universe_run(file)`

Bind and start the unique universe controller; existing planet workers continue independently.

**Parameters**: file: existing program filename without .py

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
universe_run('universe_controller')
```

### `planet_run(planet, file)`

Start one independent manager per unlocked planet.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked; file: existing program filename without .py

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
planet_run(1, 'flower_manager')
```

### `planet_stop(planet)`

Stop the manager only, leaving its worker programs alive.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
planet_stop(1)
```

### `planet_self()`

Return the worker planet, manager planet, or -1 in the universe controller.

**Parameters**: No arguments.

**Returns**: number: worker/manager planet, -1 in universe context

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
planet = planet_self()
```

### `planet_inventory(planet, item)`

Read available local stock. Reserved UFO cargo is already deducted. Earth inventory is the original farm inventory.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked; item: canonical item string or Items member; coins/gold_ore/gold are distinct

**Returns**: number: available unreserved stock

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
wood = planet_inventory(0, 'wood')
```

### `planet_products(planet)`

List recipe products native to this planet.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked

**Returns**: list[str] of native recipe item IDs

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
products = planet_products(3)
```

### `planet_surface(planet)`

Return width, height, bounds, longitude_wrap, worker_limit, hull_tier and native worker type.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked

**Returns**: dict: width,height,bounds,wrap,worker_limit,hull_tier,worker_type

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
surface = planet_surface(0)
```

### `planet_expand(planet)`

Pay research cost on Earth or local produce on other worlds to expand one stage. Preserve planted crops and centered positions. Coordinates are reindexed around the larger extent.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
planet_expand(1)
```

### `planet_scan(planet, x, y)`

Inspect unlocked latitude/longitude and the crop, fishing shoal or mineral deposit at a cell. Locked coordinates return None.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked; x: local unlocked longitude column, zero-based; y: local unlocked latitude row, zero-based

**Returns**: dict for an unlocked cell; None for locked/invalid coordinates

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
tile = planet_scan(3, 0, 0)
```

### `worker_build(planet, width=1, height=1)`

Build the native drone, bee, boat or mining drone. Width and height must be researched and fit the grid. Returns stable ID or -1.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked; width: researched footprint width1–3; height: researched footprint height1–3

**Returns**: numeric worker ID; -1 if blocked,full,unfunded or unavailable

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
id = worker_build(1, 2, 1)
```

### `worker_starter(planet)`

Return an existing native worker or create the first free 1×1 starter. Replacing free starters cannot generate refunds.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked

**Returns**: existing/free native worker ID; -1 if planet locked

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
bee = worker_starter(1)
```

### `worker_units() / worker_units(planet)`

List snapshots of all local workers, optionally filtered by planet. Fields include id, planet, role, x,y,width,height,jobs,status,program and errors.

**Parameters**: No arguments.

**Returns**: list[dict] of worker snapshots

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
for bee in worker_units(1):
    print(bee['id'], bee['program'])
```

### `worker_self()`

Return this bound worker ID; managers return -1.

**Parameters**: No arguments.

**Returns**: numeric native-worker ID; -1 in managers

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
me = worker_self()
```

### `worker_move(id, direction)`

Try one immediate native-surface step. False on cooldown, jobs, locked edge or collision; never moves between planets.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; direction: North,East,South,West; boundaries and whole footprints apply

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
worker_move(worker_self(), East)
```

### `worker_route(id, x, y)`

Queue an occupancy-aware route on the same planet. Await jobs==0 before issuing crop actions.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; x: local unlocked longitude column, zero-based; y: local unlocked latitude row, zero-based

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
worker_route(worker_self(), 1, 1)
```

### `worker_path(id, x, y)`

Inspect terrain-only coordinate path without moving; empty may mean already there or no fitting route.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; x: local unlocked longitude column, zero-based; y: local unlocked latitude row, zero-based

**Returns**: list[(x,y)]; empty for no route or already there

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
path = worker_path(worker_self(), 1, 1)
```

### `worker_work(id, item, batches)`

Queue a native recipe, selecting its planet from the worker. Missing inputs wait, wrong planet/role/extent returns False.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; item: canonical item string or Items member; coins/gold_ore/gold are distinct; batches: positive integer1–10000; default1

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
worker_work(worker_self(), 'rose', 4)
```

### `worker_cancel(id)`

Cancel physical jobs and refund a started batch input. Code continues unless stopped separately.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
program_stop(id)
worker_cancel(id)
```

### `worker_remove(id)`

Stop, cancel and dismantle a local worker. Paid hull cost is refunded; free starters and temporary workers refund zero.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
worker_remove(id)
```

### `worker_research(planet, kind)`

Purchase independent local slots or size using local materials. slots: 25×next level; size:40×next level. Earth hay, flowers roses, ocean carp, minerals stone.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked; kind: slots or size for workers; slots or cargo for UFO research

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
worker_research(1, 'slots')
```

### `worker_rotate(id)`

Rotate a landed idle footprint, preserving position; blocked rotations fail without changing orientation.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
worker_rotate(worker_self())
```

### `worker_wait(seconds)`

Yield this interpreter for 0–3600 game seconds; all other workers and UFOs continue.

**Parameters**: seconds: game seconds0–3600; yields the caller only

**Returns**: None; other programs and physical tasks continue

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
worker_wait(1)
```

### `worker_state(id)`

Read one worker snapshot or None if it no longer exists.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: worker snapshot dict or None

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
state = worker_state(worker_self())
```

### `swarm_create(name, ids)`

Create or update a named local group of unique existing IDs from one planet. Cross-planet groups are rejected.

**Parameters**: name: nonempty local swarm name, at most80 characters; ids: unique existing worker IDs, all on one planet

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
swarm_create('rose_team', [a, b])
```

### `swarm_run(name, file)`

Bind independent copies of one file to every member. Use worker_self and inboxes to assign differing jobs.

**Parameters**: name: nonempty local swarm name, at most80 characters; file: existing program filename without .py

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
swarm_run('rose_team', 'bee_worker')
```

### `swarm_stop(name)`

Stop member code while preserving physical tasks and bindings.

**Parameters**: name: nonempty local swarm name, at most80 characters

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
swarm_stop('rose_team')
```

### `swarm_members(name)`

Return member IDs, or an empty list for an unknown group.

**Parameters**: name: nonempty local swarm name, at most80 characters

**Returns**: list[number], empty when the swarm is unknown

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ids = swarm_members('rose_team')
```

### `ufo_build()`

Build a separate cargo UFO: 50 credits,20 Earth wood,10 Earth solar energy. Initially 120 battery units. Returns ID or -1.

**Parameters**: No arguments.

**Returns**: numeric UFO ID; -1 on insufficient slots,credits or Earth supplies

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ship = ufo_build()
```

### `ufo_transfer(id, source, destination, item, amount)`

Reserve real cargo and fuel, then fly via the orbital mothership. Source or destination −1 means the mother warehouse. Planet-to-planet deliveries always dock at the mother first.

**Parameters**: id: UFO ID; source and destination: -1 mothership or 0..3 planets, distinct; item: canonical ID; amount: integer 1..1000000.

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ufo_transfer(ship, 0, 1, 'water', 24)
```

### `ufo_fleet()`

Read cargo,reserved,remaining,eta,battery,capacity,planet,status and busy for every UFO. IDs are separate from local worker IDs.

**Parameters**: No arguments.

**Returns**: list[dict]: id, planet (-1 at mother), source, destination, leg_target, cargo, reserved, remaining, item, battery, eta, capacity, status, busy.

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ships = ufo_fleet()
```

### `ufo_cancel(id)`

Return unused reservations. Airborne cargo completes its current leg and deposits at the arrived dock, including the mothership when traveling toward it.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ufo_cancel(ship)
```

### `ufo_research(kind)`

Upgrade cargo or slots, up to three levels each. Earth payments:10 bronze,20 silver,30 platinum. Payload tiers 24,72,144,288; ship limit1–4.

**Parameters**: kind: slots or size for workers; slots or cargo for UFO research

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ufo_research('cargo')
```

### `ufo_refuel(id, amount)`

Docked only. Consume local solar energy; each unit adds5 battery, maximum1000. No remote fuel teleportation.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; amount: positive integer; UFO shipment1–1000000, refuel1–200

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ufo_refuel(ship, 10)
```

### `ufo_capacity()`

Return researched cargo capacity per trip.

**Parameters**: No arguments.

**Returns**: number:24,72,144 or288 units per payload

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
capacity = ufo_capacity()
```

### `ufo_wait(seconds)`

Yield a controller; inspect ufo_fleet busy/eta to wait for actual arrivals.

**Parameters**: seconds: game seconds0–3600; yields the caller only

**Returns**: None; this caller yields while flights advance

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ufo_wait(1)
```

### `get_world_width()`

Bound worker or planet manager surface width. Universe controllers must use planet_surface(id).

**Parameters**: No arguments.

**Returns**: number: current surface columns1–360

**Requires**: Bound native worker or planet manager. Universe context must use planet_surface(id).

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
width = get_world_width()
```

### `get_world_height()`

Surface height, independent from width and capped at128.

**Parameters**: No arguments.

**Returns**: number: current surface rows1–128

**Requires**: Bound native worker or planet manager. Universe context must use planet_surface(id).

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
height = get_world_height()
```

### `get_longitude()`

Return this bound worker anchor longitude in degrees.

**Parameters**: No arguments.

**Returns**: number: worker anchor longitude in degrees

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
longitude = get_longitude()
```

### `get_latitude()`

Return this bound worker anchor latitude in degrees, always inside the usable belt.

**Parameters**: No arguments.

**Returns**: number: worker anchor latitude in degrees

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
latitude = get_latitude()
```

### `harvest_area()`

Earth drone harvests all footprint cells. Returns successful harvest actions; merged pumpkin/cactus regions are settled only once.

**Parameters**: No arguments.

**Returns**: number: successful harvest actions, merged regions settle once

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
count = harvest_area()
```

### `plant_area(entity)`

Earth drone tries planting each footprint cell and returns planted count. Seeds are paid per success; existing plants stay intact.

**Parameters**: entity: original Earth Entities member

**Returns**: number: successful plantings; partial success is possible

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
count = plant_area(Entities.Carrot)
```

### `surface_distance(planet, ax, ay, bx, by)`

Grid Manhattan distance; longitude uses the short arc only when360 columns are open. Invalid/locked cells raise an error.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked; ax: start x column; ay: start y row; bx: destination x column; by: destination y row

**Returns**: number: grid distance with full-circle longitude shortcut

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
distance = surface_distance(0, 0, 0, 1, 1)
```

### `market_prices()`

Read local commodity export prices in credits.

**Parameters**: No arguments.

**Returns**: dict[str,number] of item export prices

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
prices = market_prices()
```

### `market_sell(planet, item, amount)`

Sell available unreserved stock to local buyers. This is an inventory sink, not interplanetary delivery.

**Parameters**: planet: integer0 Earth,1 Flowers,2 Ocean,3 Minerals; must be unlocked; item: canonical item string or Items member; coins/gold_ore/gold are distinct; amount: positive integer; UFO shipment1–1000000, refuel1–200

**Returns**: number: credits earned to cents; zero on failure

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
income = market_sell(3, 'stone', 20)
```

### `contract_current()`

Read the current project among 24 multi-product objectives; -1 goals use the mothership warehouse.

**Parameters**: No arguments.

**Returns**: contract dict; None after all eight contracts

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
contract = contract_current()
```

### `contract_submit()`

Atomically consume all ready contract goals and award credits.

**Parameters**: No arguments.

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
if contract_current()['ready']:
    contract_submit()
```

### `stop_all()`

Stop all interpreters, refund canceled local batches and safely finish UFO legs after refunding unused reservations.

**Parameters**: No arguments.

**Returns**: True after stopping code and initiating safe physical cancellation

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
stop_all()
```

### `product_info(item)`

Read the designated planet, named function, batch output, base seconds, roles, input recipe and export price. Hull area multiplies inputs and output.

**Parameters**: item: canonical item string or Items member; coins/gold_ore/gold are distinct

**Returns**: recipe dict, or None when no recipe exists

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
recipe = product_info('orchid')
print(recipe['inputs'])
```

### `program_bind(id, file)`

Save a file binding without starting it. Stop the old code on this drone while preserving physical jobs.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; file: existing program filename without .py

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_bind(drone_id, 'bee_worker')
```

### `program_run(id)`

Start the already bound file from its beginning. Replace the old interpreter on this drone only.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_run(drone_id)
```

### `program_stop(id)`

Stop one drone interpreter and clear its inbox. Keep its file binding and physical jobs.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_stop(drone_id)
```

### `program_state(id)`

Read file name, running flag, error and copied result for one drone.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: binding/running/error/result dict, or None

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
state = program_state(drone_id)
```

### `control_state()`

List network and planet-manager scope, bound program, running flag and error.

**Parameters**: No arguments.

**Returns**: list[dict]: scope,program,running,error

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for control in control_state():
    print(control['scope'], control['running'])
```

### `fleet_send(id, payload)`

Compatibility spelling; prefer worker_send. Send a deep-copied value to a native worker inbox, capacity16; full or missing worker returns False.

**Parameters**: id: stable numeric drone ID; payload: copied script value; inbox capacity 16

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_send(id, {'task': 'rose'})
```

### `fleet_receive()`

Compatibility spelling; prefer worker_receive. Nonblocking receive from this native worker inbox. Empty or manager contexts return None.

**Parameters**: No arguments.

**Returns**: next message or None

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
message = fleet_receive()
```

### `fleet_result(id)`

Compatibility spelling; prefer worker_result. Read a copied finished result variable or return value; running/no-result returns None.

**Parameters**: id: stable numeric drone ID

**Returns**: copied result or None

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
answer = fleet_result(id)
```

### `fleet_build(role, width, height, planet)`

Compatibility builder. role must exactly match the planet native type. Prefer worker_build(planet,width,height).

**Parameters**: role: farmer, irrigator, grower, hauler, engineer, scout or harvester; width: unlocked hull width; height: unlocked hull height; island: stable numeric island ID, check world_islands first

**Returns**: drone ID; -1 on failure

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
id = fleet_build('bee', 1, 1, 1)
```

### `fleet_cancel(id)`

Compatibility spelling; prefer worker_cancel. Cancel physical jobs and refund a started batch input. Code continues unless stopped separately.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_stop(id)
fleet_cancel(id)
```

### `fleet_halt_all()`

Compatibility spelling. Prefer the canonical universe/planet/worker API. Landed machines only. Refund reserved inputs and uncollected manifests; dismantling also refunds the hull cost.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_halt_all()
```

### `fleet_map(planet)`

Compatibility spelling; prefer planet_surface. Return width, height, bounds, longitude_wrap, worker_limit, hull_tier and native worker type.

**Parameters**: island: stable numeric island ID, check world_islands first

**Returns**: map and recipe dictionary

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
surface = fleet_map(0)
```

### `fleet_market()`

Compatibility spelling; prefer market_prices. Read local commodity export prices in credits.

**Parameters**: No arguments.

**Returns**: item-to-price dictionary

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
prices = fleet_market()
```

### `fleet_order()`

Compatibility spelling; prefer contract_current. Read current multi-product goals and live requirements, or None at completion.

**Parameters**: No arguments.

**Returns**: dict or None when all contracts are complete

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
contract = fleet_order()
```

### `fleet_plan(id, x, y)`

Compatibility spelling; prefer worker_path. Inspect terrain-only coordinate path without moving; empty may mean already there or no fitting route.

**Parameters**: id: stable numeric drone ID; x: logistics x coordinate 0–19; y: logistics y coordinate 0–13

**Returns**: list of coordinate pairs; empty if unreachable or already there

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
path = fleet_plan(worker_self(), 1, 1)
```

### `fleet_remove(id)`

Compatibility spelling; prefer worker_remove. Stop, cancel and dismantle a local worker. Paid hull cost is refunded; free starters and temporary workers refund zero.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_remove(id)
```

### `fleet_research(kind)`

Compatibility Earth-only local research; kind is slots or size. Prefer worker_research(planet,kind).

**Parameters**: kind: slots, size or a role name

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_research('slots')
```

### `fleet_rotate(id)`

Compatibility spelling; prefer worker_rotate. Rotate a landed idle footprint, preserving position; blocked rotations fail without changing orientation.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_rotate(worker_self())
```

### `fleet_route(id, x, y)`

Compatibility spelling; prefer worker_route. Queue an occupancy-aware route on the same planet. Await jobs==0 before issuing crop actions.

**Parameters**: id: stable numeric drone ID; x: logistics x coordinate 0–19; y: logistics y coordinate 0–13

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_route(worker_self(), 1, 1)
```

### `fleet_run(id, file)`

Compatibility spelling; prefer program_run. Bind and immediately run the named file on one native worker.

**Parameters**: id: stable numeric drone ID; file: existing program name without .py

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_run(id, 'bee_worker')
```

### `fleet_self()`

Compatibility spelling; prefer worker_self. Return this bound worker ID; managers return -1.

**Parameters**: No arguments.

**Returns**: drone ID; -1 in a manager

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
me = fleet_self()
```

### `fleet_sell(planet, item, amount)`

Compatibility spelling; prefer market_sell. Sell available unreserved stock to local buyers. This is an inventory sink, not interplanetary delivery.

**Parameters**: island: stable numeric island ID, check world_islands first; item: cargo identifier or Items member; amount: positive integer, shipping/export maximum 1000000

**Returns**: actual credits earned, rounded to cents; 0 on failure

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
income = fleet_sell(3, 'stone', 20)
```

### `fleet_start()`

Confirm Spaceflight has been researched; no separate charter or resource grant.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
ready = fleet_start()
```

### `fleet_state()`

Compatibility spelling; prefer universe_state. Read world time, credit balance, contract and worker totals.

**Parameters**: No arguments.

**Returns**: dict

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
state = fleet_state()
```

### `fleet_step(id, direction)`

Compatibility spelling; prefer worker_move. Try one immediate native-surface step. False on cooldown, jobs, locked edge or collision; never moves between planets.

**Parameters**: id: stable numeric drone ID; direction: North, East, South or West

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_step(worker_self(), East)
```

### `fleet_stock(planet, item)`

Compatibility spelling; prefer planet_inventory. Read available local stock. Reserved UFO cargo is already deducted. Earth inventory is the original farm inventory.

**Parameters**: island: stable numeric island ID, check world_islands first; item: cargo identifier or Items member

**Returns**: available quantity

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
wood = fleet_stock(0, 'wood')
```

### `fleet_stop(id)`

Compatibility spelling; prefer program_stop. Stop one drone interpreter and clear its inbox. Keep its file binding and physical jobs.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_stop(drone_id)
```

### `fleet_submit()`

Compatibility spelling; prefer contract_submit. Atomically consume all ready contract goals and award credits.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if contract_current()['ready']:
    fleet_submit()
```

### `fleet_units(planet=None)`

Compatibility spelling; prefer worker_units. List snapshots of all local workers, optionally filtered by planet. Fields include id, planet, role, x,y,width,height,jobs,status,program and errors.

**Parameters**: No arguments.

**Returns**: list of drone snapshot dictionaries

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for bee in fleet_units(1):
    print(bee['id'], bee['program'])
```

### `fleet_wait(seconds)`

Compatibility spelling; prefer worker_wait. Yield this interpreter for 0–3600 game seconds; all other workers and UFOs continue.

**Parameters**: seconds: game seconds, 0–3600

**Returns**: None

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_wait(1)
```

### `fleet_work(id, planet, item, batches)`

Compatibility production queue; planet must equal the native worker home. Prefer worker_work.

**Parameters**: id: stable numeric drone ID; island: stable numeric island ID, check world_islands first; item: cargo identifier or Items member; batches: integer 1–10000; default 1

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_work(worker_self(), 1, 'rose', 1)
```

### `group_create(name, ids)`

Compatibility spelling; prefer swarm_create. Create or update a named local group of unique existing IDs from one planet. Cross-planet groups are rejected.

**Parameters**: name: nonempty group name, up to 80 characters; ids: list of existing unique numeric drone IDs

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
group_create('rose_team', [a, b])
```

### `group_run(name, file)`

Compatibility spelling; prefer swarm_run. Bind independent copies of one file to every member. Use worker_self and inboxes to assign differing jobs.

**Parameters**: name: nonempty group name, up to 80 characters; file: existing program name without .py

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
group_run('rose_team', 'bee_worker')
```

### `group_stop(name)`

Compatibility spelling; prefer swarm_stop. Stop member code while preserving physical tasks and bindings.

**Parameters**: name: nonempty group name, up to 80 characters

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
group_stop('rose_team')
```

### `group_units(name)`

Compatibility spelling; prefer swarm_members. Return member IDs, or an empty list for an unknown group.

**Parameters**: name: nonempty group name, up to 80 characters

**Returns**: list of drone IDs

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
ids = group_units('rose_team')
```

### `island_products(planet)`

Compatibility spelling; prefer planet_products. List recipe products native to this planet.

**Parameters**: island: stable numeric island ID, check world_islands first

**Returns**: list of product strings

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
products = island_products(3)
```

### `island_run(planet, file)`

Compatibility spelling; prefer planet_run. Start one independent manager per unlocked planet.

**Parameters**: island: stable numeric island ID, check world_islands first; file: existing program name without .py

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
island_run(1, 'flower_manager')
```

### `island_self()`

Compatibility spelling; prefer planet_self. Return the worker planet, manager planet, or -1 in the universe controller.

**Parameters**: No arguments.

**Returns**: island ID or -1

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
planet = island_self()
```

### `island_stop(planet)`

Compatibility spelling; prefer planet_stop. Stop the manager only, leaving its worker programs alive.

**Parameters**: island: stable numeric island ID, check world_islands first

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
island_stop(1)
```

### `network_run(file)`

Compatibility spelling; prefer universe_run. Bind and start the unique universe controller; existing planet workers continue independently.

**Parameters**: file: existing program name without .py

**Returns**: bool

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
network_run('universe_controller')
```

### `world_islands()`

Compatibility spelling; prefer universe_planets. List exactly four planets and their unlocked flags.

**Parameters**: No arguments.

**Returns**: list of island dictionaries

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for planet in world_islands():
    if planet['unlocked']:
        print(planet['id'], planet['name'])
```

### `ufo_reposition(id, destination)`

Send an idle empty UFO to an unlocked dock. Pay 2+planet-ID distance battery; no item cargo is teleported.

**Parameters**: id: idle UFO ID; destination: -1 mother or unlocked planet 0..3.

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Spaceflight research, unlocked route endpoints and a valid UFO ID; refueling requires local dock stock.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

```python
ufo_reposition(ship, 0)
```

### `worker_send(id, payload)`

Send a deep-copied value to a native worker inbox, capacity16; full or missing worker returns False.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; payload: deep-copied script value; inbox holds16 values

**Returns**: bool: True on acceptance/success; False when state or resources do not permit it.

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
worker_send(id, {'task': 'rose'})
```

### `worker_receive()`

Nonblocking receive from this native worker inbox. Empty or manager contexts return None.

**Parameters**: No arguments.

**Returns**: oldest copied message, or None if empty/manager

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
message = worker_receive()
```

### `worker_result(id)`

Read a copied finished result variable or return value; running/no-result returns None.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space

**Returns**: copied result/return value, or None

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
answer = worker_result(id)
```

### `mothership_inventory(item)`

Read an item in the mothership depot. Does not collect or move cargo.

**Parameters**: item: canonical item ID

**Returns**: number: actual current stock

**Requires**: The respective planet or Spaceflight must be researched.

**Failure**: Invalid arguments stop the caller. Unmet research or resources returns False.

```python
stock = mothership_inventory('water')
```

### `planet_technologies(planet)`

List native research nodes with costs, parents, levels and effects. Universe uses planet −1.

**Parameters**: planet: -1 universe, 0 Earth, 1 flowers, 2 ocean, 3 minerals.

**Returns**: list[dict]: id, level, max, item, cost, parent, category, effect.

**Requires**: The respective planet or Spaceflight must be researched.

**Failure**: Invalid arguments stop the caller. Unmet research or resources returns False.

```python
nodes = planet_technologies(1)
```

### `planet_research(planet, technology)`

Pay for one level of planet-specific research. Universe technology pays Earth stock.

**Parameters**: planet: -1..3; technology: native technology ID such as pollination.

**Returns**: bool: False if locked, prerequisite missing, maxed or unaffordable.

**Requires**: The respective planet or Spaceflight must be researched.

**Failure**: Invalid arguments stop the caller. Unmet research or resources returns False.

```python
accepted = planet_research(1, 'pollination')
```

## 17 / Every production function

Each product function maps to exactly one native recipe. It enqueues work, never an immediate free item. The recipe table below is exported from the runtime. Distinguish coins, gold_ore and gold; raw minerals and refined products are separate inventory IDs.

### `make_compost(id, batches=1)`

Queue 4 compost per 1×1 batch on planet 0; requires drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 4 Compost / 18s / 4 Hay + 2 Carrot

```python
make_compost(worker_self(), 1)
```

### `prepare_fish_feed(id, batches=1)`

Queue 4 fish_feed per 1×1 batch on planet 0; requires drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 4 Fish feed / 12s / 1 Carrot + 1 Pumpkin

```python
prepare_fish_feed(worker_self(), 1)
```

### `pump_water(id, batches=1)`

Queue 4 water per 1×1 batch on planet 0; requires drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 4 Water / 8s / no material inputs

```python
pump_water(worker_self(), 1)
```

### `cultivate_rose(id, batches=1)`

Queue 3 rose per 1×1 batch on planet 1; requires bee. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Rose / 24s / 2 Water + 1 Compost

```python
cultivate_rose(worker_self(), 1)
```

### `cultivate_tulip(id, batches=1)`

Queue 4 tulip per 1×1 batch on planet 1; requires bee. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 4 Tulip / 30s / 2 Water + 1 Compost

```python
cultivate_tulip(worker_self(), 1)
```

### `cultivate_lavender(id, batches=1)`

Queue 3 lavender per 1×1 batch on planet 1; requires bee. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Lavender / 28s / 1 Water + 1 Compost

```python
cultivate_lavender(worker_self(), 1)
```

### `cultivate_lily(id, batches=1)`

Queue 2 lily per 1×1 batch on planet 1; requires bee. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Lily / 40s / 3 Water + 1 Fertilizer

```python
cultivate_lily(worker_self(), 1)
```

### `cultivate_orchid(id, batches=1)`

Queue 2 orchid per 1×1 batch on planet 1; requires bee. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Orchid / 65s / 2 Water + 1 Silver + 1 Lavender

```python
cultivate_orchid(worker_self(), 1)
```

### `cultivate_lotus(id, batches=1)`

Queue 2 lotus per 1×1 batch on planet 1; requires bee. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Lotus / 70s / 4 Water + 1 Fishmeal

```python
cultivate_lotus(worker_self(), 1)
```

### `collect_honey(id, batches=1)`

Queue 3 honey per 1×1 batch on planet 1; requires bee. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Honey / 20s / 1 Rose + 1 Water

```python
collect_honey(worker_self(), 1)
```

### `mine_stone(id, batches=1)`

Queue 4 stone per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 4 Stone / 18s / no material inputs

```python
mine_stone(worker_self(), 1)
```

### `mine_copper_ore(id, batches=1)`

Queue 3 copper_ore per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Copper ore / 22s / 1 Solar energy

```python
mine_copper_ore(worker_self(), 1)
```

### `mine_tin_ore(id, batches=1)`

Queue 3 tin_ore per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Tin ore / 24s / 1 Solar energy

```python
mine_tin_ore(worker_self(), 1)
```

### `mine_silver_ore(id, batches=1)`

Queue 2 silver_ore per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Silver ore / 32s / 2 Solar energy

```python
mine_silver_ore(worker_self(), 1)
```

### `mine_gold_ore(id, batches=1)`

Queue 2 gold_ore per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Gold ore / 42s / 3 Solar energy

```python
mine_gold_ore(worker_self(), 1)
```

### `mine_platinum_ore(id, batches=1)`

Queue 1 platinum_ore per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 1 Platinum ore / 54s / 4 Solar energy

```python
mine_platinum_ore(worker_self(), 1)
```

### `mine_rough_diamond(id, batches=1)`

Queue 1 rough_diamond per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 1 Rough diamond / 65s / 5 Solar energy

```python
mine_rough_diamond(worker_self(), 1)
```

### `smelt_copper(id, batches=1)`

Queue 3 copper per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Copper / 24s / 3 Copper ore + 1 Solar energy

```python
smelt_copper(worker_self(), 1)
```

### `smelt_tin(id, batches=1)`

Queue 3 tin per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Tin / 24s / 3 Tin ore + 1 Solar energy

```python
smelt_tin(worker_self(), 1)
```

### `smelt_bronze(id, batches=1)`

Queue 4 bronze per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 4 Bronze / 32s / 3 Copper + 1 Tin + 1 Solar energy

```python
smelt_bronze(worker_self(), 1)
```

### `smelt_silver(id, batches=1)`

Queue 2 silver per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Silver / 40s / 2 Silver ore + 2 Solar energy

```python
smelt_silver(worker_self(), 1)
```

### `smelt_gold(id, batches=1)`

Queue 2 gold per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Gold bullion / 50s / 2 Gold ore + 3 Solar energy

```python
smelt_gold(worker_self(), 1)
```

### `refine_platinum(id, batches=1)`

Queue 1 platinum per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 1 Platinum / 70s / 1 Platinum ore + 1 Rose + 4 Solar energy

```python
refine_platinum(worker_self(), 1)
```

### `cut_diamond(id, batches=1)`

Queue 1 diamond per 1×1 batch on planet 3; requires mining_drone. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 1 Cut diamond / 90s / 1 Rough diamond + 1 Orchid + 5 Solar energy

```python
cut_diamond(worker_self(), 1)
```

### `catch_carp(id, batches=1)`

Queue 3 carp per 1×1 batch on planet 2; requires boat. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Carp / 24s / 1 Fish feed

```python
catch_carp(worker_self(), 1)
```

### `catch_trout(id, batches=1)`

Queue 3 trout per 1×1 batch on planet 2; requires boat. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Trout / 32s / 1 Fish feed

```python
catch_trout(worker_self(), 1)
```

### `catch_salmon(id, batches=1)`

Queue 2 salmon per 1×1 batch on planet 2; requires boat. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Salmon / 45s / 2 Fish feed

```python
catch_salmon(worker_self(), 1)
```

### `catch_cod(id, batches=1)`

Queue 3 cod per 1×1 batch on planet 2; requires boat. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Cod / 48s / 2 Fish feed

```python
catch_cod(worker_self(), 1)
```

### `catch_tuna(id, batches=1)`

Queue 2 tuna per 1×1 batch on planet 2; requires boat. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 2 Tuna / 70s / 3 Fish feed

```python
catch_tuna(worker_self(), 1)
```

### `catch_koi(id, batches=1)`

Queue 1 koi per 1×1 batch on planet 2; requires boat. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 1 Koi / 90s / 1 Lotus + 2 Fish feed

```python
catch_koi(worker_self(), 1)
```

### `make_fishmeal(id, batches=1)`

Queue 3 fishmeal per 1×1 batch on planet 2; requires boat. Inputs and output scale with footprint area.

**Parameters**: id: stable native-worker ID; ufo_* uses its separate UFO ID space; batches: positive integer1–10000; default1

**Returns**: bool: accepted, not completed. Missing inputs wait; wrong role/planet/unavailable deposit returns False.

**Requires**: Unlocked target planet and the correct bound worker type.

**Failure**: Argument errors stop this interpreter. Unmet conditions return False, None or -1 as documented; other interpreters continue.

**Recipe**: 3 Fishmeal / 18s / 2 Carp

```python
make_fishmeal(worker_self(), 1)
```

## Canonical item glossary

| ID | Name |

|---|---|

| hay | Hay |

| wood | Wood |

| carrot | Carrot |

| pumpkin | Pumpkin |

| solar_energy | Solar energy |

| cactus | Cactus |

| bone | Bone |

| coins | Treasure coins |

| water | Water |

| fertilizer | Fertilizer |

| catalyst | Catalyst |

| compost | Compost |

| fish_feed | Fish feed |

| honey | Honey |

| rose | Rose |

| tulip | Tulip |

| lavender | Lavender |

| lily | Lily |

| orchid | Orchid |

| lotus | Lotus |

| stone | Stone |

| copper_ore | Copper ore |

| tin_ore | Tin ore |

| silver_ore | Silver ore |

| gold_ore | Gold ore |

| platinum_ore | Platinum ore |

| rough_diamond | Rough diamond |

| copper | Copper |

| tin | Tin |

| bronze | Bronze |

| silver | Silver |

| gold | Gold bullion |

| platinum | Platinum |

| diamond | Cut diamond |

| carp | Carp |

| trout | Trout |

| salmon | Salmon |

| cod | Cod |

| tuna | Tuna |

| koi | Koi |

| fishmeal | Fishmeal |

## Planet IDs and native workers

| ID | Planet | Native worker |

|---|---|---|

| 0 | Earth | drone |

| 1 | Floralia · Flower Planet | bee |

| 2 | Pelagia · Ocean Planet | boat |

| 3 | Aurelia · Mineral Planet | mining_drone |

## All 31 production recipes

| Item | Planet | Worker | Output | Seconds | Inputs | Function |

|---|---|---|---|---|---|---|

| compost | 0 | drone | 4 | 18.0 | 4 hay + 2 carrot | make_compost |

| fish_feed | 0 | drone | 4 | 12.0 | 1 carrot + 1 pumpkin | prepare_fish_feed |

| water | 0 | drone | 4 | 8.0 | — | pump_water |

| rose | 1 | bee | 3 | 24.0 | 2 water + 1 compost | cultivate_rose |

| tulip | 1 | bee | 4 | 30.0 | 2 water + 1 compost | cultivate_tulip |

| lavender | 1 | bee | 3 | 28.0 | 1 water + 1 compost | cultivate_lavender |

| lily | 1 | bee | 2 | 40.0 | 3 water + 1 fertilizer | cultivate_lily |

| orchid | 1 | bee | 2 | 65.0 | 2 water + 1 silver + 1 lavender | cultivate_orchid |

| lotus | 1 | bee | 2 | 70.0 | 4 water + 1 fishmeal | cultivate_lotus |

| honey | 1 | bee | 3 | 20.0 | 1 rose + 1 water | collect_honey |

| stone | 3 | mining_drone | 4 | 18.0 | — | mine_stone |

| copper_ore | 3 | mining_drone | 3 | 22.0 | 1 solar_energy | mine_copper_ore |

| tin_ore | 3 | mining_drone | 3 | 24.0 | 1 solar_energy | mine_tin_ore |

| silver_ore | 3 | mining_drone | 2 | 32.0 | 2 solar_energy | mine_silver_ore |

| gold_ore | 3 | mining_drone | 2 | 42.0 | 3 solar_energy | mine_gold_ore |

| platinum_ore | 3 | mining_drone | 1 | 54.0 | 4 solar_energy | mine_platinum_ore |

| rough_diamond | 3 | mining_drone | 1 | 65.0 | 5 solar_energy | mine_rough_diamond |

| copper | 3 | mining_drone | 3 | 24.0 | 3 copper_ore + 1 solar_energy | smelt_copper |

| tin | 3 | mining_drone | 3 | 24.0 | 3 tin_ore + 1 solar_energy | smelt_tin |

| bronze | 3 | mining_drone | 4 | 32.0 | 3 copper + 1 tin + 1 solar_energy | smelt_bronze |

| silver | 3 | mining_drone | 2 | 40.0 | 2 silver_ore + 2 solar_energy | smelt_silver |

| gold | 3 | mining_drone | 2 | 50.0 | 2 gold_ore + 3 solar_energy | smelt_gold |

| platinum | 3 | mining_drone | 1 | 70.0 | 1 platinum_ore + 1 rose + 4 solar_energy | refine_platinum |

| diamond | 3 | mining_drone | 1 | 90.0 | 1 rough_diamond + 1 orchid + 5 solar_energy | cut_diamond |

| carp | 2 | boat | 3 | 24.0 | 1 fish_feed | catch_carp |

| trout | 2 | boat | 3 | 32.0 | 1 fish_feed | catch_trout |

| salmon | 2 | boat | 2 | 45.0 | 2 fish_feed | catch_salmon |

| cod | 2 | boat | 3 | 48.0 | 2 fish_feed | catch_cod |

| tuna | 2 | boat | 2 | 70.0 | 3 fish_feed | catch_tuna |

| koi | 2 | boat | 1 | 90.0 | 1 lotus + 2 fish_feed | catch_koi |

| fishmeal | 2 | boat | 3 | 18.0 | 2 carp | make_fishmeal |

## Planet-exclusive research

| Planet | ID | Name | Base cost | Max level | Parent | Effect |

|---|---|---|---|---|---|---|

| -1 | navigation | Orbital navigation | 24 bronze | 5 |  | UFO travel speed +15% per level. |

| -1 | docking | Docking automation | 20 silver | 5 | navigation | Docking approach time decreases with each level. |

| 0 | ecology | Soil ecology | 120 hay | 5 |  | Original crop growth speed +12% per level. |

| 0 | retention | Water retention | 100 wood | 5 | ecology | Water evaporates more slowly; no automatic watering. |

| 1 | pollination | Pollination corridors | 60 rose | 5 |  | Flower cultivation speed +18% per level. |

| 1 | apiculture | Apiary networks | 50 honey | 5 | pollination | Honey yield +15% per level. |

| 1 | botany | Selective botany | 25 orchid | 5 | apiculture | Flower yield +12% per level. |

| 2 | sonar | Shoal sonar | 60 carp | 5 |  | Fish catch speed +18% per level. |

| 2 | schooling | Sustainable schools | 45 salmon | 5 | sonar | Fish catch yield +12% per level. |

| 2 | processing | Marine processing | 50 fishmeal | 5 | schooling | Fishmeal processing speed +20% per level. |

| 3 | drilling | Adaptive drills | 100 stone | 5 |  | Ore mining speed +18% per level. |

| 3 | refining | Precision refining | 60 bronze | 5 | drilling | Smelting and cutting speed +16% per level. |

| 3 | metallurgy | Materials recovery | 20 platinum | 5 | refining | Native mineral outputs +12% per level. |

Costs double per purchased level. Universe research pays Earth stock; planet research pays local stock.

## All 24 universe projects

### 1. Earth prepares for orbit

Keep original crops available while building transport supplies.

0 / wood × 100 · 0 / compost × 30

### 2. First botanical colony

Ship water and compost by UFO; let bees establish flowers.

1 / rose × 30 · 1 / tulip × 20

### 3. The mineral ladder

Mine copper and tin ores, then smelt true bronze.

3 / stone × 60 · 3 / bronze × 30

### 4. Blue-water fleet

Send fish feed and coordinate local fishing boats.

2 / carp × 30 · 2 / trout × 30 · 2 / salmon × 20

### 5. Pollination network

Use the six-flower chain and collect honey.

1 / lavender × 40 · 1 / lily × 30 · 1 / honey × 20

### 6. Precious returns

UFOs return processed silver and gold, not unrefined ore.

0 / silver × 30 · 0 / gold × 15

### 7. Across four worlds

Transfer fisheries inputs to flowers and botanical inputs to refineries.

1 / lotus × 20 · 3 / platinum × 10 · 2 / tuna × 20

### 8. The living universe

Complete independent swarms and interplanetary product deliveries.

0 / diamond × 12 · 0 / orchid × 30 · 0 / koi × 20 · 0 / honey × 40

### 9. The orbital pantry

Stock the mothership warehouse using actual UFO arrivals.

-1 / water × 45 · -1 / compost × 30 · -1 / fish_feed × 30 · -1 / solar_energy × 45

### 10. Six gardens

Maintain six flower supply chains and send the full collection to the mothership.

-1 / rose × 35 · -1 / tulip × 35 · -1 / lavender × 25 · -1 / lily × 20 · -1 / orchid × 10 · -1 / lotus × 10

### 11. The foundry ring

Deliver smelted metals rather than ores; use larger mining hulls.

-1 / copper × 60 · -1 / tin × 30 · -1 / bronze × 45 · -1 / silver × 25

### 12. Open ocean survey

Coordinate fishing routes for every common shoal.

-1 / carp × 45 · -1 / trout × 35 · -1 / salmon × 25 · -1 / cod × 35 · -1 / tuna × 20

### 13. Living soil

Flower honey and marine fishmeal support Earth's ecological programme.

0 / honey × 60 · 0 / fishmeal × 75 · 0 / compost × 120

### 14. Precision observatory

Combine bullion, platinum and botanical optics in orbit.

-1 / gold × 35 · -1 / platinum × 25 · -1 / orchid × 35

### 15. Ocean restoration

Produce lotus and koi through both marine and botanical logistics.

2 / lotus × 60 · -1 / koi × 25 · 1 / fishmeal × 90

### 16. Renewable infrastructure

Supply orbital infrastructure through four-world freight.

-1 / solar_energy × 450 · -1 / bronze × 120 · -1 / silver × 75 · -1 / honey × 75

### 17. Crystal telemetry

Diamonds require mining, flowers, silver and cutting; run independent swarms.

-1 / diamond × 35 · -1 / platinum × 45 · -1 / orchid × 60

### 18. Botanical seed vault

Build a balanced six-species reserve in the mothership.

-1 / rose × 180 · -1 / tulip × 150 · -1 / lavender × 150 · -1 / lily × 120 · -1 / orchid × 75 · -1 / lotus × 75

### 19. A silver ocean

Deliver fish and refined metals together without starving the colonies.

-1 / silver × 150 · -1 / salmon × 120 · -1 / tuna × 90 · -1 / koi × 50

### 20. Deep core observatory

Sustain rare-ore processing alongside ordinary metal supply.

-1 / diamond × 75 · -1 / gold × 120 · -1 / platinum × 90 · -1 / bronze × 225

### 21. Distributed life support

Balance local and orbital depots across every planet.

1 / water × 300 · 2 / fish_feed × 240 · 3 / solar_energy × 360 · -1 / honey × 180 · -1 / fishmeal × 180

### 22. Four-world exhibition

Deliver a curated botanical, marine and mineral collection.

-1 / orchid × 135 · -1 / lotus × 135 · -1 / koi × 105 · -1 / gold × 180 · -1 / diamond × 105

### 23. Orbital research city

Produce and transport an interdependent supply network at industrial scale.

-1 / diamond × 150 · -1 / platinum × 210 · -1 / silver × 300 · -1 / honey × 240 · -1 / tuna × 210

### 24. A self-sustaining universe

Complete the final simultaneous delivery to your mothership.

-1 / diamond × 210 · -1 / orchid × 270 · -1 / koi × 180 · -1 / bronze × 360 · -1 / lotus × 180 · -1 / honey × 360

## Complete universe example files

Create these using Programs → Create files. Run universe_controller on the Universe target. Supply materials by UFO before expecting outputs. These examples demonstrate hierarchy; they do not grant research/resources or solve every contract automatically.

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

## Verification and limits

See [the current validation report](reports/v0.13.2/README.md). The complete fresh reference walkthrough starts from one plot with zero inventory; timing depends on the documented worker and scheduling policy. Windows is cross-built and still needs native hardware acceptance. Download the public preview from the repository Releases page.
