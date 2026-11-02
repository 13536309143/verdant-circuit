# Verdant Circuit 0.13.0 — Complete Field Manual

[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)

This manual is generated from the shipped runtime reference: 24 chapters, 127 individual function/method entries, 28 recipes and 11 hierarchical programs. Examples run inside the game, not system Python.

## 01 / First harvest

Choose Enter Earth on the world screen. Start with a one-plot garden and no inventory. Harvest free grass, learn loops, planting and sensors, then develop the original crop chain. At Parallel Drones, continue to Earth & archipelago network. Modules, Dictionaries and Irrigation are also required.

This is one campaign. Earth is island 0 and its original inventory is the home depot. Flower, mineral and fish islands are later research branches. Physical shipping connects them; special-island cargo cannot appear instantly at home. Programming and strategy affect duration; no time certification forces you to wait.

```python
# Requires Loops. Stop when you have enough hay.
while True:
    harvest()
```

## 02 / Programs & states

The program center separates files from execution targets. Choose Earth farmer, Network, Island manager, a specific drone or a named group. Edit changes only the file being viewed. Bind only saves a target association and stops its old code; Bind + run starts the selected file on that target. One drone, island or network target has at most one active interpreter. Different targets run concurrently.

Green cards are active, amber borders mark the edited file, and binding labels list target IDs. Files used by a running program or imported library cannot be deleted. At least one file remains. Chinese and numeric names are accepted; imports use the file name without .py.

The studio target strip shows what Run affects. Floating editors also run on this selected target. Stop target leaves other targets running; Stop all stops managers, workers and landed physical work. Undo/redo, independent editor windows and external editing remain available.

## 03 / Language essentials

The interpreter uses Python-inspired syntax, not a full Python installation. Indent blocks with four spaces or one tab; do not mix them. Names and enum members are case-sensitive. Research the corresponding language nodes before using variables, branches, functions or collections. Unrestricted syntax in Settings removes language gates only; it does not grant crops or research.

Supported forms include if/elif/else, while, for, break/continue, parameterized functions, default arguments, return, global, imports, lists, immutable tuples, sets, dictionaries, indexing, unpacking and closures. Python standard-library modules and operating-system access are unavailable. Compare with ==, assign with =; negative indices count from the end.

Use print() during debugging, but frequent printing adds execution cost. A while loop without an exit continues until Stop; prefer inventory-based goals for finite jobs.

```python
# Requires Operators, Variables and Loops.
target = 200
while num_items(Items.Hay) < target:
    if can_harvest():
        harvest()
```

## 04 / Staged research

Research has three independent requirements: prior discoveries, cumulative production and a payable resource cost. Production totals count successful harvests even after you spend the resources; they are not a second payment. The tree shows current / required totals. Completing a crop unlocks further field, speed and yield tiers.

Field sizes: Planting → 2×2; Trees → 4×4; Pumpkins → 8×8; Cacti → 16×16; Tail exploration → 32×32. You still pay for each expansion. Existing purchased research is retained when updating; start a new farm to experience the complete staged campaign.

get_cost() reports the price, get_unlock_requirements() reports gates, and can_unlock() checks both gates and funds. unlock() returns False without spending when any requirement is missing. There is no elapsed-time certification gate. The tier lists below identify the discovery needed for each upgrade.

```python
# Requires Costs, Automatic research and Operators.
if can_unlock(Unlocks.Speed):
    unlock(Unlocks.Speed)
# Requires Debug for output.
print(get_unlock_requirements(Unlocks.Expand))
print(num_produced(Items.Wood))
```

## 05 / Crop supply chains

Grass yields hay. Bushes and trees yield wood. Carrots require tilled soil, hay and wood; pumpkins and sunflowers use carrots; cacti use pumpkins. Research increases seed costs as well as yields. Inspect get_cost(Entities.Carrot) before filling a field, and reserve enough inputs for multiple repair cycles.

Planting needs an empty compatible plot; dead pumpkins can be replaced directly. till() switches ground type. Clearing or expanding resets the field and loses standing plants. Protect your inventory before switching the whole field to another crop.

Trees grow more slowly for every adjacent tree, so alternate trees and bushes in a checkerboard. Normal field edges wrap: East from the last column returns to column zero. Maze and tail mode do not wrap. Base growth times are grass 3s, bush 4s, tree 6s, carrot 7s, pumpkin 11s, sunflower 8.5s and cactus 1.5s; water speeds growth.

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

## 06 / Water & infection

Irrigation research generates water every ten game seconds. One unit adds 0.25 moisture, up to 1.0; moisture decays gradually. Water only when get_water() is below your chosen threshold and supplies exist. A field near 50% moisture does not need watering under a 0.3 threshold; that is expected.

Fertilizer research generates supplies on the same ten-second cycle. Fertilizer adds growth and infects that plant. When merged pumpkins or cacti are harvested, Weird Substance is proportional to the infected fraction; one infected plant cannot convert half the entire field. Fertilizer also performs the normal pumpkin death check when it reaches maturity.

Weird Substance toggles infection on the current plot and its distinct neighbors. On a bush, enough of it creates a maze instead: side × 2^(maze level−1). On treasure, the same quantity reuses the maze and awards gold, up to 300 reuses.

```python
# Add inside a farming loop after Irrigation and Sensors.
if get_water() < 0.3 and num_items(Items.Water) > 0:
    use_item(Items.Water)
```

## 07 / Giant pumpkins

A pumpkin has a 20% chance to die on reaching maturity. Scan the square repeatedly and replace Dead_Pumpkin cells; do not harvest healthy individual plants during repair. Only harvest after every cell in the desired square is a mature pumpkin. The game merges the largest complete square containing the harvest point.

A side n square produces n² × min(n,6) × its per-plant research yield. Check the entire square, not just one ready cell. Budget seeds for replacement. For infection production, apply fertilizer to the number of mature plants you need before harvesting; read num_produced() for milestone progress and num_items() for spendable inventory.

```python
# Full runnable repair algorithm: Templates → Growing giant pumpkins.
# Preconditions: empty tilled field, sufficient carrot seeds.
if get_entity_type() == Entities.Dead_Pumpkin:
    plant(Entities.Pumpkin)
```

## 08 / Sunflower priority

measure() on a sunflower returns 7–15 petals. If at least ten sunflowers remain, harvesting the current maximum earns an 8× bonus. Picking a smaller flower breaks the bonus sequence until the sunflower field is cleared. Growing more flowers without measuring can waste most of the potential power.

Plant the field, wait for maturity without repeatedly walking, then scan petal values from 15 down to 7. Research and energy accelerate instructions; movement and instruction execution consume power. Avoid endless full-field searches for each individual flower and moving loops that spend all the power while waiting.

Power is a supply, not a permanent speed unlock. Keep a reserve and budget carrot inputs for the next generation of flowers.

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

## 09 / Companion planting

After companion research, get_companion() returns a requested crop and target coordinate, or None for an ineligible plot. Grass, bushes, trees and carrots can request companions. The target must contain the requested type when the original plant is harvested; the companion itself does not have to be mature.

Read and save the request, place the companion, return to the original plant, then harvest when mature. Do not assume a request survives harvesting and replanting. Level 1 multiplies eligible harvests by 2; each additional level adds 0.5, reaching 4.5 at level 6. Before research there is no companion bonus.

For automation, maintain a coordinate-to-request map and resolve conflicting requests; blindly overwriting the whole field often destroys useful crops.

```python
request = get_companion()
if request != None:
    entity, position = request
    target_x, target_y = position
    print(entity, target_x, target_y)
```

## 10 / Cactus sorting

Each mature cactus has a measured size from 0 to 9. swap(East) or swap(North) exchanges adjacent cacti. A connected mature region chains only when sizes are nondecreasing to the east and north. One out-of-order pair prevents the region-wide chain.

Sort every row, then every column. The shipped bubble-sort template is simple and reliable; optimize comparisons and movement when you understand its invariant. Sorting columns after rows preserves the row order. Chain output for m plants is per-plant yield × m × min(m,16), preserving a reward for larger sorted fields with bounded scaling.

Fund pumpkin seeds before clearing the previous crop. Sorting a large field before cacti mature does not satisfy the harvest condition.

```python
# Full algorithm: Templates → Sorting cacti.
if measure() > measure(East):
    swap(East)
```

## 11 / Maze search

Plant a bush, then use Weird Substance in the required quantity to generate a connected maze. can_move() tests walls, move() reports success, and measure() returns the treasure coordinate. Harvest at treasure for gold. Harvesting a hedge clears the maze without a treasure reward.

Use depth-first search with a visited set and a direction stack. Explore unvisited neighbors; when stuck, reverse the last move. For reusable mazes, a visited set is essential because walls can be removed and loops appear. Treasure targets are chosen far enough along the actual passages, not at the current tile.

At treasure, use Weird Substance instead of harvesting to receive gold and relocate the target while preserving the layout. Re-search using a fresh visited set. Reuse consumes supplies and is limited to 300 relocations; afterwards harvest and generate a new maze.

```python
# Requires Mazes, Planting and Sensors. Use on an empty plot.
plant(Entities.Bush)
amount = get_world_size() * 2**(num_unlocked(Unlocks.Mazes)-1)
use_item(Items.Weird_Substance, amount)
# Then run Templates → Searching a maze.
```

## 12 / Long-tail routes

The Dinosaur Hat enables the apple-and-tail mode. Apples consume cactus inventory. The tail occupies cells, prevents crossing itself, and disables field wrapping. can_move() and move() obey tail occupancy; measure() at the apple provides the next target. Only one drone can control the tail.

An even-sided empty field supports a Hamiltonian cycle: cover every tile along a nonintersecting route and return along the first column. Follow the cycle instead of greedily heading for each apple. Keep at least side²+1 cactus available. Stop or switch to Straw_Hat to settle the current tail.

Bone = floor(length × min(length,64) × (1 + 0.25×(research level−1))). After length 64, each extra segment still adds production, while the length multiplier stays bounded. The template ends when its full route becomes blocked.

```python
# Full algorithm: Templates → Tail coverage.
change_hat(Hats.Dinosaur_Hat)
# Follow a safe route, then settle:
change_hat(Hats.Straw_Hat)
```

## 13 / Independent drones

Earth farmers run original crop functions against the same Earth field and inventory. Each bound farmer has its own coordinates and variables. Permanent workers are created with fleet_build and bound with program_bind/program_run. Legacy spawn_drone(function,args...) creates a temporary farmer from an Earth worker; completion releases its slot and wait_for returns its value.

For groups use group_create(name,[ids...]) and group_run(name,file). Members execute independent copies of the file; fleet_self identifies the current member. They do not share mutable variables. Use inbox messages to coordinate. Managers consume no physical drone slot. Farmer bodies are 1×1; specialist hulls use the researched rectangular traffic system. Earth crop workers retain the original overflight behavior on the crop grid.

## 15 / Diagnose & recover

Crop never appears: check its research, seed inventory, soil and whether the cell is empty. A failed plant() returns False; waiting on an empty plot cannot mature it.

Research stays locked with enough funds: read the next-level crop discovery and cumulative-production requirements. Speed and yield tiers no longer all unlock together.

Pumpkins never merge: inspect every cell for dead or immature plants. Cactus chains fail: find an east/north inversion or an immature member. Power disappears: stop repeated walking and reduce scans. Maze gets stuck: use visited and backtracking; normal wrapping does not apply. Drone globals do not return: use wait_for() return values.

Use F9 breakpoints, Pause and Step to inspect one line at a time. Syntax errors include line numbers. Save backups end with .bak. Farm Library creates independent saves and program folders. External editor mode exports .py and __builtins__.py hints, then watches saves; avoid editing a file in two editors simultaneously. Audio level is saved when you release the slider.

Full progression after an update is best tested on a new farm. Updating does not take back resources earned under earlier balance rules.

## 16 / API reference

All public game APIs are listed below with prerequisites and return values. Entities, Items, Grounds, Unlocks, Hats and Leaderboards are namespaces. Calls are case-sensitive. Successful research is paid from shared inventory. A cost may exist even when its discovery or production requirements are unmet. A fully researched node returns None for cost and requirements.

Examples in other chapters may be fragments intended for an existing field or loop; each chapter names its assumptions. Templates provide complete algorithms for the major crop mechanics.

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

Move North/East/South/West; walls and tails may block it. Ordinary fields wrap.

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

Sensors. Zero-based position and current field side length.

**Parameters**: No arguments.

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

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

### `num_produced(item)`

Sensors. Cumulative harvested production, including harvested Weird Substance; spending does not reduce it. Passive water/fertilizer supplies use num_items().

**Parameters**: item: cargo identifier or Items member

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
total = num_produced(Items.Carrot)
```

### `num_unlocked(research)`

Purchased level of the named research.

**Parameters**: research: Unlocks member

**Returns**: number

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
opened = num_unlocked(Unlocks.Archipelago) > 0
```

### `get_cost(entity_or_research, level=-1)`

Costs. Entity gives planting inputs; Unlocks gives next research cost. Optional level is zero-based purchased-level index.

**Parameters**: entity_or_research: Entities for seeds, Unlocks for research; level: optional zero-based purchased-level index; default -1 means current

**Returns**: resource-cost dict or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
cost = get_cost(Unlocks.Flower_Island)
```

### `get_unlock_requirements(research)`

Costs. Keys unlock:<id> and produced:<item> give required discovery levels and cumulative production. Does not include payable price.

**Parameters**: research: Unlocks member

**Returns**: requirements dict or None

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
requirements = get_unlock_requirements(Unlocks.Archipelago)
```

### `can_unlock(research)`

Costs. True only when discoveries, production totals and funds all suffice.

**Parameters**: research: Unlocks member

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
ready = can_unlock(Unlocks.Fish_Island)
```

### `unlock(research)`

Automatic research. Pay and purchase the next level; missing requirements return False with no payment.

**Parameters**: research: Unlocks member

**Returns**: bool

**Requires**: Original crop/tool research applies. Crop movement and actions require an Earth farmer; inspections can also run in managers.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if can_unlock(Unlocks.Flower_Island):
    unlock(Unlocks.Flower_Island)
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

World testing tools. Temporarily resize to an already purchased maximum; restored when execution ends. 0 means maximum.

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

## 17 / Unified drones & traffic

All persistent machines appear in one fleet list, including Earth farmers. Slot capacity follows purchased parallel-drone research and fleet slot upgrades. Count, hull area and role research are independent. A bigger machine scales recipe inputs and output by area; duration scales more slowly, but the body needs wider lanes.

Physical jobs are queued, up to 64 per drone. Broad passages use a one-way loop, smaller hulls can take narrow shortcuts, and docking/landing reserves the full footprint. Unsafe manual movement may incur repair fees; automatic routes wait or yield rather than overlap. Engineer research opens after order 6 or Mineral Island; harvester after order 7 or Fish Island. Ordinary recipe work requires its assigned role.

## 18 / One Earth supply network

Earth (0) uses the original inventory, not a second warehouse. fleet_stock(0,'wood') and num_items(Items.Wood) agree. Orders, exports, crop planting and research compete for the same available Earth resources. Transport reserves stock at acceptance, carries limited holds across actual flights, and unloads once. Returning an island product to Earth makes it available in the original home inventory.

The five original logistics outposts remain as stepping stones. Three new research branches open Floralia (6), Aurelia (7) and Pelagia (8) independently. Query world_islands and check unlocked; do not assume all numbers below the unlocked count are open. New six-product contracts follow the original logistics orders, ending with synchronized returns to Earth.

## 19 / Managers, workers & modules

Create the unified example from Programs. Bind unified_controller to Network. It starts earth_manager and the unlocked flower/mineral/fish managers, then terminates. The managers and workers continue independently. Each island has at most one manager; every drone keeps its own file binding. Re-running a manager reuses a matching drone and may restart that worker, so inspect existing jobs before adding work.

Specialist workers import production_helpers and enqueue one batch when idle. Arrange real deliveries first: rose needs water and compost, carp needs hay and water. Managers may fail to build if all slots are occupied; free a slot, research more capacity or reassign a machine. The example teaches hierarchy and does not automatically complete every order.

Physical queues, cargo and bindings are saved. Interpreter stacks, inboxes and results are not. After loading, restart the appropriate controller; stopping a parent does not recursively stop children. Worker errors are isolated, shown on its card and program_state. CPU budgets keep a tight loop from freezing other interpreters.

## 20 / Earth → three islands

1. Enter Earth; complete the original crop and language discoveries.
2. Research Parallel Drones, Modules, Dictionaries and Irrigation. Purchase Earth & archipelago network for 500 treasure gold. There is no second charter fee.
3. Research Flower Island (2000 pumpkin), Mineral Island (5000 cactus), or Fish Island (20000 carrot). The branches are independent.
4. Build the appropriate specialist and a hauler, respecting slot capacity.
5. Bind and run programs; transport real inputs before waiting for outputs.
6. Return products to Earth, complete orders and expand the network.

## 21 / Every product function

Each named product function queues work on its designated island. It does not create output immediately. Batch input and output multiply by hull area. Use the recipe table below; batch seconds are base 1×1 values. Gold ore is gold_ore and is separate from treasure coins (gold). Bronze is a processed alloy, not a raw natural ore. Fish hatchery batches represent managed fisheries rather than instant catches.

### `grow_rose(id, batches=1)`

Queue rose production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 6 / grower / 3 rose / 24s; 2 water + 1 compost

```python
grow_rose(fleet_self(), 4)
```

### `grow_tulip(id, batches=1)`

Queue tulip production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 6 / grower / 4 tulip / 30s; 2 water + 1 wood

```python
grow_tulip(fleet_self(), 4)
```

### `grow_lavender(id, batches=1)`

Queue lavender production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 6 / grower / 3 lavender / 28s; 1 water + 2 hay

```python
grow_lavender(fleet_self(), 4)
```

### `grow_lily(id, batches=1)`

Queue lily production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 6 / grower / 2 lily / 40s; 3 water + 1 fertilizer

```python
grow_lily(fleet_self(), 4)
```

### `grow_orchid(id, batches=1)`

Queue orchid production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 6 / grower / 2 orchid / 65s; 2 water + 1 silver + 1 lavender

```python
grow_orchid(fleet_self(), 4)
```

### `grow_lotus(id, batches=1)`

Queue lotus production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 6 / grower / 2 lotus / 70s; 4 water + 1 fishmeal

```python
grow_lotus(fleet_self(), 4)
```

### `mine_stone(id, batches=1)`

Queue stone production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 7 / engineer / 4 stone / 18s; no inputs

```python
mine_stone(fleet_self(), 4)
```

### `smelt_bronze(id, batches=1)`

Queue bronze production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 7 / engineer / 2 bronze / 32s; 3 stone + 2 wood

```python
smelt_bronze(fleet_self(), 4)
```

### `mine_silver(id, batches=1)`

Queue silver production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 7 / engineer / 2 silver / 45s; 4 stone + 1 power

```python
mine_silver(fleet_self(), 4)
```

### `mine_gold(id, batches=1)`

Queue gold_ore production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 7 / engineer / 1 gold_ore / 60s; 2 silver + 1 bronze + 2 power

```python
mine_gold(fleet_self(), 4)
```

### `refine_platinum(id, batches=1)`

Queue platinum production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 7 / engineer / 1 platinum / 85s; 1 gold_ore + 1 rose + 3 power

```python
refine_platinum(fleet_self(), 4)
```

### `cut_diamond(id, batches=1)`

Queue diamond production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 7 / engineer / 1 diamond / 110s; 1 platinum + 1 orchid + 4 power

```python
cut_diamond(fleet_self(), 4)
```

### `catch_carp(id, batches=1)`

Queue carp production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 8 / harvester / 3 carp / 24s; 2 hay + 1 water

```python
catch_carp(fleet_self(), 4)
```

### `catch_trout(id, batches=1)`

Queue trout production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 8 / harvester / 3 trout / 32s; 2 grain + 1 water

```python
catch_trout(fleet_self(), 4)
```

### `catch_salmon(id, batches=1)`

Queue salmon production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 8 / harvester / 2 salmon / 45s; 1 lavender + 2 grain

```python
catch_salmon(fleet_self(), 4)
```

### `catch_cod(id, batches=1)`

Queue cod production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 8 / harvester / 3 cod / 48s; 2 grain + 1 bronze

```python
catch_cod(fleet_self(), 4)
```

### `catch_tuna(id, batches=1)`

Queue tuna production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 8 / harvester / 2 tuna / 70s; 1 salmon + 2 power

```python
catch_tuna(fleet_self(), 4)
```

### `catch_koi(id, batches=1)`

Queue koi production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 8 / harvester / 1 koi / 90s; 1 lotus + 2 carp

```python
catch_koi(fleet_self(), 4)
```

### `make_fishmeal(id, batches=1)`

Queue fishmeal production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 8 / harvester / 3 fishmeal / 18s; 2 carp

```python
make_fishmeal(fleet_self(), 4)
```

### `make_compost(id, batches=1)`

Queue compost production on its specialist island. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted. This is equivalent to fleet_work with the recipe island and item. Missing inputs wait; no free output is granted.

**Parameters**: id: stable numeric drone ID; batches: integer 1–10000; default 1

**Returns**: bool: queue accepted, not completed

**Requires**: Unlocked recipe island and the role shown in product_info; enough inputs for processing.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

**Recipe**: island 0 / grower / 4 compost / 18s; 4 hay + 2 carrot

```python
make_compost(fleet_self(), 4)
```

## 22 / Every network function

Reference entries specify arguments, results, requirements, failure behavior and examples. Pass the stable drone ID returned by fleet_build, not its list position. Island managers use island_self; workers use fleet_self. Checking a queue and accepting a command are different from completion.

The API uses positional arguments only. Examples mentioning drone_id or hauler require an existing machine ID. Files must already exist in Programs.

Inventory calls return available stock. Unit snapshots expose job counts, cargo, flight status, program state and errors.

### `fleet_start()`

Confirm the unified network is enabled after buying Archipelago research. There is no additional charter fee.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_start()
```

### `fleet_state()`

Read credits, slot limit, hull tier, order and unlocked_islands. islands is a count; unlocked_islands contains actual stable IDs.

**Parameters**: No arguments.

**Returns**: dict

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
state = fleet_state()
for island in state['unlocked_islands']:
    print(island)
```

### `fleet_order()`

Read independent slot/size/role progress, current multi-goal contract and physical agents. Unit snapshots include incoming shipments, pending production, code errors and inbox size.

**Parameters**: No arguments.

**Returns**: dict or None when all contracts are complete

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
order = fleet_order()
if order != None and order['ready']:
    fleet_submit()
```

### `fleet_units()`

Read independent slot/size/role progress, current multi-goal contract and physical agents. Unit snapshots include incoming shipments, pending production, code errors and inbox size.

**Parameters**: No arguments.

**Returns**: list of drone snapshot dictionaries

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for drone in fleet_units():
    print(drone['id'], drone['program'])
```

### `fleet_stock(island, item)`

Read available stock in a local depot. Island 0 is the original Earth inventory; reservations are immediately removed from available stock.

**Parameters**: island: stable numeric island ID, check world_islands first; item: cargo identifier or Items member

**Returns**: available quantity

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
wood = fleet_stock(0, 'wood')
```

### `fleet_build(role, width, height, island)`

Build a persistent drone using network credits. Role, hull and slot limits are checked separately. Farmers are 1×1 and Earth-only.

**Parameters**: role: farmer, irrigator, grower, hauler, engineer, scout or harvester; width: unlocked hull width; height: unlocked hull height; island: stable numeric island ID, check world_islands first

**Returns**: drone ID; -1 on failure

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
drone = fleet_build('grower', 1, 1, 6)
```

### `fleet_remove(id)`

Landed machines only. Refund reserved inputs and uncollected manifests; dismantling also refunds the hull cost.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
removed = fleet_remove(drone_id)
```

### `fleet_research(kind)`

Purchase slots, hull size or a specialist role. Mineral research also opens engineers; fisheries research opens harvesters.

**Parameters**: kind: slots, size or a role name

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_research('engineer')
```

### `fleet_work(id, island, item, batches)`

Queue a role-compatible local recipe. One batch scales inputs and outputs by occupied area; inputs reserve when processing begins.

**Parameters**: id: stable numeric drone ID; island: stable numeric island ID, check world_islands first; item: cargo identifier or Items member; batches: integer 1–10000; default 1

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
accepted = fleet_work(drone_id, 7, 'stone', 4)
```

### `fleet_ship(id, source, destination, item, amount)`

Haulers only. Atomically reserves available source stock, then physically loads, flies and unloads; no instant cargo transfer.

**Parameters**: id: stable numeric drone ID; source: unlocked source island ID; destination: unlocked destination island ID, different from source; item: cargo identifier or Items member; amount: positive integer, shipping/export maximum 1000000

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
ok = fleet_ship(hauler, 0, 6, 'water', 24)
```

### `fleet_rebase(id, destination)`

Queue inter-island travel or a local route. Terrain, lane direction and every occupied hull cell are checked.

**Parameters**: id: stable numeric drone ID; destination: unlocked destination island ID, different from source

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_rebase(drone_id, 7)
```

### `fleet_route(id, x, y)`

Queue inter-island travel or a local route. Terrain, lane direction and every occupied hull cell are checked.

**Parameters**: id: stable numeric drone ID; x: logistics x coordinate 0–19; y: logistics y coordinate 0–13

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_route(drone_id, 15, 2)
```

### `fleet_step(id, direction)`

Idle machines only. Failed unsafe steps trigger emergency braking and at most two credits in fees. Rotation must fit.

**Parameters**: id: stable numeric drone ID; direction: North, East, South or West

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_step(drone_id, East)
```

### `fleet_rotate(id)`

Idle machines only. Failed unsafe steps trigger emergency braking and at most two credits in fees. Rotation must fit.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
rotated = fleet_rotate(drone_id)
```

### `fleet_cancel(id)`

Landed machines only. Refund reserved inputs and uncollected manifests; dismantling also refunds the hull cost.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_stop(drone_id)
fleet_cancel(drone_id)
```

### `fleet_submit()`

Consumes all current contract goals once, after hull/program/flight checklist conditions are met; rewards credits and discovers the next order.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
if fleet_order() != None and fleet_order()['ready']:
    fleet_submit()
```

### `fleet_wait(seconds)`

Yield the caller while physical queues and other programs advance; range 0–3600. Not an unlock timer.

**Parameters**: seconds: game seconds, 0–3600

**Returns**: None

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_wait(1)
```

### `fleet_plan(id, x, y)`

Inspect a terrain-aware local path. Live reservations can make actual execution wait or reroute.

**Parameters**: id: stable numeric drone ID; x: logistics x coordinate 0–19; y: logistics y coordinate 0–13

**Returns**: list of coordinate pairs; empty if unreachable or already there

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
path = fleet_plan(drone_id, 15, 2)
```

### `fleet_run(id, file)`

Start an independent named-file interpreter or stop code. Stop does not cancel already queued physical work.

**Parameters**: id: stable numeric drone ID; file: existing program name without .py

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_run(drone_id, 'rose_worker')
```

### `fleet_stop(id)`

Start an independent named-file interpreter or stop code. Stop does not cancel already queued physical work.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_stop(drone_id)
```

### `fleet_send(id, payload)`

Send a copied value to a bounded 16-message inbox. Receive is nonblocking and returns None when empty; only bound workers receive their own inbox.

**Parameters**: id: stable numeric drone ID; payload: copied script value; inbox capacity 16

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_send(hauler, {'from': 0, 'to': 6, 'item': 'water', 'amount': 12})
```

### `fleet_receive()`

Send a copied value to a bounded 16-message inbox. Receive is nonblocking and returns None when empty; only bound workers receive their own inbox.

**Parameters**: No arguments.

**Returns**: next message or None

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
message = fleet_receive()
```

### `fleet_self()`

Bound physical machine ID inside a fleet program; -1 in the main controller.

**Parameters**: No arguments.

**Returns**: drone ID; -1 in a manager

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
me = fleet_self()
```

### `fleet_result(id)`

Read a copied result variable from a finished program; transient and not saved.

**Parameters**: id: stable numeric drone ID

**Returns**: copied result or None

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
answer = fleet_result(drone_id)
```

### `fleet_map(island)`

Terrain rows from bottom to top, recipes, role requirements, coordinates and island links. Island IDs are 0–5.

**Parameters**: island: stable numeric island ID, check world_islands first

**Returns**: map and recipe dictionary

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
layout = fleet_map(7)
```

### `fleet_halt_all()`

Landed machines only. Refund reserved inputs and uncollected manifests; dismantling also refunds the hull cost.

**Parameters**: No arguments.

**Returns**: bool

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
fleet_halt_all()
```

### `fleet_market()`

Inspect export prices or sell real available local stock to recover credits. Reserved and in-flight cargo cannot be sold.

**Parameters**: No arguments.

**Returns**: item-to-price dictionary

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
prices = fleet_market()
```

### `fleet_sell(island, item, amount)`

Inspect export prices or sell real available local stock to recover credits. Reserved and in-flight cargo cannot be sold.

**Parameters**: island: stable numeric island ID, check world_islands first; item: cargo identifier or Items member; amount: positive integer, shipping/export maximum 1000000

**Returns**: actual credits earned, rounded to cents; 0 on failure

**Requires**: Unlocked network and correct drone role for physical jobs; inspections may be queried earlier.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
income = fleet_sell(7, 'stone', 20)
```

### `world_islands()`

List stable island IDs, keys, names, unlocked flags, required research and manager bindings. Check unlocked rather than assuming consecutive IDs.

**Parameters**: No arguments.

**Returns**: list of island dictionaries

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for island in world_islands():
    if island['unlocked']:
        print(island['id'], island['name'])
```

### `island_products(island)`

List production recipes available on the specified island. Locked islands return an empty list.

**Parameters**: island: stable numeric island ID, check world_islands first

**Returns**: list of product strings

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
products = island_products(6)
```

### `product_info(item)`

Read the designated island, named function, batch output, base seconds, roles, input recipe and export price. Hull area multiplies inputs and output.

**Parameters**: item: cargo identifier or Items member

**Returns**: recipe dictionary or None

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
recipe = product_info('orchid')
print(recipe['inputs'])
```

### `island_self()`

Return the current island of a drone, the assigned island of a manager, or -1 for the network controller.

**Parameters**: No arguments.

**Returns**: island ID or -1

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
island = island_self()
```

### `program_bind(id, file)`

Save a file binding without starting it. Stop the old code on this drone while preserving physical jobs.

**Parameters**: id: stable numeric drone ID; file: existing program name without .py

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_bind(drone_id, 'rose_worker')
```

### `program_run(id)`

Start the already bound file from its beginning. Replace the old interpreter on this drone only.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_run(drone_id)
```

### `program_stop(id)`

Stop one drone interpreter and clear its inbox. Keep its file binding and physical jobs.

**Parameters**: id: stable numeric drone ID

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
program_stop(drone_id)
```

### `program_state(id)`

Read file name, running flag, error and copied result for one drone.

**Parameters**: id: stable numeric drone ID

**Returns**: program snapshot dictionary or None

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
state = program_state(drone_id)
```

### `network_run(file)`

Bind and start the one network controller. Replacing or stopping it leaves island managers and drone programs independent.

**Parameters**: file: existing program name without .py

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
network_run('unified_controller')
```

### `island_run(island, file)`

Bind and start the one manager for an unlocked island. Other island managers and workers run independently.

**Parameters**: island: stable numeric island ID, check world_islands first; file: existing program name without .py

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
island_run(6, 'flower_manager')
```

### `island_stop(island)`

Stop only this island manager; its dispatched drone workers keep running.

**Parameters**: island: stable numeric island ID, check world_islands first

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
island_stop(6)
```

### `control_state()`

List network and island-manager scope, bound program, running flag and error.

**Parameters**: No arguments.

**Returns**: list of controller dictionaries

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
for control in control_state():
    print(control['scope'], control['running'])
```

### `group_create(name, ids)`

Create or replace a persistent named group of existing unique drone IDs. Each member runs an independent interpreter, even when using the same file.

**Parameters**: name: nonempty group name, up to 80 characters; ids: list of existing unique numeric drone IDs

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
group_create('flower_team', [rose_drone, tulip_drone])
```

### `group_units(name)`

List current members of a named group. Dismantling a drone removes its membership.

**Parameters**: name: nonempty group name, up to 80 characters

**Returns**: list of drone IDs

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
members = group_units('flower_team')
```

### `group_run(name, file)`

Bind and run the same file independently on each group member. Dispatch by fleet_self, island_self or inbox messages.

**Parameters**: name: nonempty group name, up to 80 characters; file: existing program name without .py

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
group_run('flower_team', 'rose_worker')
```

### `group_stop(name)`

Stop all group members’ code, preserving bindings and physical queues.

**Parameters**: name: nonempty group name, up to 80 characters

**Returns**: bool

**Requires**: Archipelago research for network/group control; target islands must be unlocked.

**Failure**: Wrong argument count or types raise an error in the caller only. False, None or -1 mean unmet conditions as specified above. Physical job acceptance is separate from completion.

```python
group_stop('flower_team')
```

## 23 / Control patterns & recovery

Direct: call grow_rose(id,4) from a manager. Bound: program_bind(id,'rose_worker'), then program_run(id). Group: group_create('team',[a,b]), then group_run('team','worker'). Module: import production_helpers; each interpreter gets its own module variables. Message: fleet_send(id, payload), then fleet_receive inside that worker. Results: assign result in a finite worker and read fleet_result(id).

For mixed products, bind different files, branch on fleet_self/island_self, or receive a message with the requested recipe. A productive circular chain starts with basic carp and lavender before lotus and koi; gold_ore needs bronze and silver, platinum needs roses, diamonds need orchids.

If production waits: check product_info inputs, local stock, reservations, the correct role and unlocked island. If traffic waits: inspect hull footprint and docking; cancel landed jobs before reassigning. If a manager keeps restarting a worker, stop the manager or Stop all before editing assignments.

## 24 / Local races & records

Local races remain available from Records. They use preset independent farms with a fixed seed; finish the requested production and terminate the program to record a result. Historical rules are labeled separately. The simulation laboratory, its research and simulate() have been removed. Existing live farms are not reset by a race.

## 25 / Namespaces & inventory IDs

Entities selects original Earth crops; Items selects inventory resources; Unlocks selects research, never a crop seed. North/East/South/West are directions, Grounds selects terrain, Hats selects appearance or exploration behavior, and Leaderboards selects local race categories.

Use string cargo IDs with fleet_stock, fleet_ship and product_info. Items.Rose is equivalent to 'rose', Items.Gold_Ore to 'gold_ore', while Items.Gold remains treasure currency. Island IDs are stable: Earth 0; logistics outposts 1–5; Flowers 6, Minerals 7, Fish 8. Drone IDs are allocated separately and are not island IDs.

Save stores physical inventory, jobs, reservations, bindings, groups and unread notices. Call stacks, messages and results are transient. Existing home and island-0 warehouses merge once on upgrade. Back up your farm before comparing old versions.

## All island IDs

| ID | Island | Research | Original order threshold |

|---|---|---|---|

| 0 | Earth · Home Garden | Earth | 0 |

| 1 | Reed Reservoir | archipelago | 2 |

| 2 | Ember Foundry | archipelago | 5 |

| 3 | Glass Reef | archipelago | 8 |

| 4 | Bloom Conservatory | archipelago | 11 |

| 5 | Zenith Archive | archipelago | 14 |

| 6 | Floralia · Flower Island | flower_island | 0 |

| 7 | Aurelia · Mineral Island | mineral_island | 0 |

| 8 | Pelagia · Fish Island | fish_island | 0 |

## All production recipes

Base 1×1 values. Inputs and outputs scale with hull area; processing seconds multiply by 0.8 + 0.2 × sqrt(area), and harvesters use a further 0.8 multiplier.

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

## Complete hierarchical example files

Create these with Programs → Create the unified example. Bind unified_controller to Network. Managers need unlocked targets, spare drone slots and credits. Arrange input shipments separately; the example demonstrates hierarchy, not an automatic solution to every contract.

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

## Test scope and save behavior

See [the validation report](validation/README.md). Physical jobs, cargo, bindings and group membership are saved; program stacks, inboxes and results are not. Restart a controller after loading. Existing Earth and old island-0 storage merge once. This local build has not been published to GitHub.
