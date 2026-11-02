# Verdant Circuit 0.12.1 — Field Manual

[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)

## 01 / First harvest

Start on one grass plot with no inventory. Grass matures naturally. Run harvest() until you own five hay, then research Loops. Load First harvest from Templates.

Next research Operators, Variables, Sensors and Planting. Research does not happen just because you own enough resources: select a node and pay, or later use unlock(). Free grass and bushes provide recovery if you run out of crop seeds.

This campaign aims at about three hours of game time to full research with a working automated strategy. 0.12.1 adds the multi-hour six-island logistics campaign after and alongside this research route. Your planning and editing time is additional. Research speed bonuses are part of normal 1× play; the Settings speed multiplier accelerates the entire world.

```python
# Requires Loops. Stop when you have enough hay.
while True:
    harvest()
```

## 02 / Programs & states

Program names may start with Chinese characters or digits: 123, 1_farm and 浇水程序 are valid. Use letters, digits or underscores, up to 80 characters, without .py. Duplicate names, including case-only variants, are rejected. Import a numeric file with from 123 import work or import 123 as farm. Chinese module names work directly, for example import 浇水程序.

Programs are reusable modules. The library shows a card for every file, a total count, active-file count and drone-slot usage. Green means the file has running workers; an amber border means it is open in the main editor. A file may be both running and selected. Paused tasks remain active; idle files have no live workers.

Edit switches the main editor without stopping existing work. Main Run replaces the current controller and its workers. Window opens a draggable editor; Run separately launches that file if a drone slot is free. Stop all terminates every worker. Closing an editor window leaves its code and running tasks intact. Running files cannot be deleted; at least one file is retained.

Use Cmd/Ctrl+Z to undo and Cmd/Ctrl+Shift+Z to redo. F9 toggles a breakpoint at the caret line. Pause, Step and the variable watch help diagnose a program.

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

Parallel research raises capacity from 1 to 2, 4, 8, 16 and 32. The main controller uses a slot. spawn_drone(function,args...) returns a handle or None if capacity is full. Workers get independent copies of variables and collections, but share the field and inventory. They can occupy the same cell without collision.

Use per-column or per-region jobs to avoid two workers planting the same plot or racing for seeds. Save handles, then wait_for() each result. has_finished() checks without waiting. Modifying a worker's global variable does not update the controller's copy; return a value instead. Completed workers release slots.

When spawning fails, let the main controller perform the job. A program that never joins or terminates its workers will not finish a resource competition.

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

## 14 / Experiments & races

Create a module named experiment. simulate(name,unlocks,items,globals,seed,speedup) runs a separate farm and returns game seconds. Unlocks means all research; dictionaries can select research levels. Input items and globals are isolated copies. Repeating a nonnegative seed reproduces the same initial randomness. Actual inventory and crops are unchanged. A simulation is capped at one game hour; nested simulations are unsupported.

Resource competitions use a standard 8×8 mid-tier farm and seed 41 with supplied inputs. Targets differ: hay 4096, wood 6000, carrots 2048, pumpkins 12000, power 8000, cactus 20000, gold 4096, bone 30000. Produce the target AND terminate all workers. _Single categories restrict capacity to one. Resource challenges allow one game hour; Fastest_Reset starts from zero and allows four, ending when local competition research is purchased.

Records stay on this computer. Old-rule scores are marked historical and are not compared with current scores. Achievements and successful challenges open a completion notice; unread notices survive restarting. There is no online Steam submission.

```python
# experiment.py, then call from a different program:
# def work lives in the experiment file; modules use the file name.
seconds = simulate('experiment', Unlocks, {Items.Carrot: 10000}, {}, 7, 64)
print(seconds)
```

## 15 / Diagnose & recover

Crop never appears: check its research, seed inventory, soil and whether the cell is empty. A failed plant() returns False; waiting on an empty plot cannot mature it.

Research stays locked with enough funds: read the next-level crop discovery and cumulative-production requirements. Speed and yield tiers no longer all unlock together.

Pumpkins never merge: inspect every cell for dead or immature plants. Cactus chains fail: find an east/north inversion or an immature member. Power disappears: stop repeated walking and reduce scans. Maze gets stuck: use visited and backtracking; normal wrapping does not apply. Drone globals do not return: use wait_for() return values.

Use F9 breakpoints, Pause and Step to inspect one line at a time. Syntax errors include line numbers. Save backups end with .bak. Farm Library creates independent saves and program folders. External editor mode exports .py and __builtins__.py hints, then watches saves; avoid editing a file in two editors simultaneously. Audio level is saved when you release the slider.

Full progression after an update is best tested on a new farm. Updating does not take back resources earned under earlier balance rules.

## 16 / API reference

All public game APIs are listed below with prerequisites and return values. Entities, Items, Grounds, Unlocks, Hats and Leaderboards are namespaces. Calls are case-sensitive. Successful research is paid from shared inventory. A cost may exist even when its discovery or production requirements are unmet. A fully researched node returns None for cost and requirements.

Examples in other chapters may be fragments intended for an existing field or loop; each chapter names its assumptions. Templates provide complete algorithms for the major crop mechanics.

### `harvest() → bool`

Harvest a mature crop; at treasure, award gold. Immature/empty plots return False.

### `can_harvest() → bool`

Test maturity at the current plot; does not harvest.

### `plant(entity) → bool`

Planting/crop research and seed inputs required; False if occupied or wrong ground.

### `till()`

Switch grassland and soil; use before planting soil crops.

### `move(direction) → bool`

Move North/East/South/West; walls and tails may block it. Ordinary fields wrap.

### `can_move(direction) → bool`

Sensors. Test the same walls/tail rules without moving.

### `use_item(item, amount=1) → bool`

Consume Water, Fertilizer or Weird_Substance; False if unsuitable or insufficient.

### `get_pos_x() / get_pos_y() / get_world_size() → number`

Sensors. Zero-based position and current field side length.

### `get_entity_type() / get_ground_type()`

Sensors. Return an entity, None, or the ground namespace value.

### `get_water() → number`

Sensors. Current moisture in [0,1].

### `num_items(item) → number`

Sensors. Spendable current inventory, shared between drones.

### `num_produced(item) → number`

Sensors. Cumulative harvested production, including harvested Weird Substance; spending does not reduce it. Passive water/fertilizer supplies use num_items().

### `num_unlocked(unlock) → number`

Purchased level of the named research.

### `get_cost(entity_or_unlock, level=-1) → dict | None`

Costs. Entity gives planting inputs; Unlocks gives next research cost. Optional level is zero-based purchased-level index.

### `get_unlock_requirements(unlock) → dict | None`

Costs. Keys unlock:<id> and produced:<item> give required discovery levels and cumulative production. Does not include payable price.

### `can_unlock(unlock) → bool`

Costs. True only when discoveries, production totals and funds all suffice.

### `unlock(unlock) → bool`

Automatic research. Pay and purchase the next level; missing requirements return False with no payment.

### `get_companion() → tuple | None`

Sensors and companion research. Returns (entity, (x,y)) for eligible plants.

### `measure(direction=None)`

Sensors. Flower petals/cactus size; maze treasure or next-apple coordinates. None when unsupported.

### `swap(direction) → bool`

Exchange adjacent cactus cells; does not buy seeds.

### `spawn_drone(function, args...) → handle | None`

Parallel research. Independent memory copy; None when slots are full.

### `wait_for(handle) → value`

Wait and return the worker result; the field and other workers keep advancing.

### `has_finished(handle) → bool`

Check a worker without blocking.

### `num_drones() / max_drones() → number`

Count active workers, including the main task, and available capacity.

### `simulate(file, unlocks, items, globals, seed, speedup) → seconds`

Simulation research. Isolated program execution; fixed seed reproduces its starting randomness.

### `leaderboard_run(category, file, speedup)`

Local competition research. Run a named program in a fixed preset and record success after termination.

### `get_time() / get_tick_count() → number`

Timing tools. Current world seconds / this drone's instruction cost count.

### `set_world_size(side)`

World testing tools. Temporarily resize to an already purchased maximum; restored when execution ends. 0 means maximum.

### `set_execution_speed(speed)`

World testing tools. Cap current execution speed; does not exceed purchased speed.

### `change_hat(hat) → bool`

Select a hat; Dinosaur_Hat and Top_Hat require their research. Leaving tail mode settles bone.

### `random() → number`

Numeric utilities. Seeded uniform value in [0,1).

### `print(values...) / quick_print(values...)`

Debugging. Write to the journal. Frequent print adds execution cost.

### `clear() / do_a_flip()`

clear resets the current field and loses standing plants. do_a_flip consumes an action without moving.

### `fleet_start() → bool`

Sign once: Carrots + 100 Wood + 100 Carrots; grants 100 fleet credits once.

### `fleet_state() / fleet_order() / fleet_units() → dict / list`

Read independent slot/size/role progress, current multi-goal contract and physical agents. Unit snapshots include incoming shipments, pending production, code errors and inbox size.

### `fleet_stock(island, item) → number`

Available local stock, excluding accepted shipping reservations. Cargo names are strings: water, grain, ore, alloy, crystal, medicine, core.

### `fleet_map(island) → dict`

Terrain rows from bottom to top, recipes, role requirements, coordinates and island links. Island IDs are 0–5.

### `fleet_build(role, width, height, island) → id | -1`

Pays credits and respects independent role, hull, slot and parking-space limits. Rotated hulls are supported.

### `fleet_research("slots" | "size" | role) → bool`

Pay fleet credits after the named order discovery. Buying slots does not enlarge a hull.

### `fleet_work(id, island, item, batches) → bool`

Queue a role-compatible local recipe. One batch scales inputs and outputs by occupied area; inputs reserve when processing begins.

### `fleet_ship(id, source, target, item, amount) → bool`

Haulers only. Atomically reserves available source stock, then physically loads, flies and unloads; no instant cargo transfer.

### `fleet_rebase(id, island) / fleet_route(id, x, y) → bool`

Queue inter-island travel or a local route. Terrain, lane direction and every occupied hull cell are checked.

### `fleet_plan(id, x, y) → list`

Inspect a terrain-aware local path. Live reservations can make actual execution wait or reroute.

### `fleet_step(id, direction) / fleet_rotate(id) → bool`

Idle machines only. Failed unsafe steps trigger emergency braking and at most two credits in fees. Rotation must fit.

### `fleet_cancel(id) / fleet_remove(id) / fleet_halt_all() → bool`

Landed machines only. Refund reserved inputs and uncollected manifests; dismantling also refunds the hull cost.

### `fleet_submit() → bool`

Consumes all current contract goals once, after hull/program/flight checklist conditions are met; rewards credits and discovers the next order.

### `fleet_run(id, file) / fleet_stop(id) → bool`

Start an independent named-file interpreter or stop code. Stop does not cancel already queued physical work.

### `fleet_self() → id`

Bound physical machine ID inside a fleet program; -1 in the main controller.

### `fleet_send(id, payload) / fleet_receive() → bool / value`

Send a copied value to a bounded 16-message inbox. Receive is nonblocking and returns None when empty; only bound workers receive their own inbox.

### `fleet_result(id) → value | None`

Read a copied result variable from a finished program; transient and not saved.

### `fleet_wait(seconds)`

Yield the caller while physical queues and other programs advance; range 0–3600. Not an unlock timer.

### `fleet_market() / fleet_sell(island, item, amount) → dict / credits`

Inspect export prices or sell real available local stock to recover credits. Reserved and in-flight cargo cannot be sold.

## 17 / Specialist fleets

Open Islands after discovering Carrots. The charter costs 100 Wood and 100 Carrots, and grants 100 fleet credits once. Island machines are separate from the legacy 1–32 crop-script workers. Three independent research tracks govern fleet slots (3–8), hull footprint (1×1, 2×1, 2×2, 3×2, 3×3, including rotated variants), and specialisms. A larger hull processes a larger batch and carries more, but moves more slowly and must fit every occupied cell. Irrigators pump water; growers produce grain or medicine; engineers mine and refine ore/alloy or assemble cores; scouts refine crystal; haulers move cargo; harvesters accelerate grain production.

Orders pay fleet credits. Each role and hull has an explicit order gate. Export surplus depot goods with fleet_sell to recover credits after repair penalties; exports consume real available goods and cannot sell reserved manifests. Dismantling a landed machine returns its full paid hull cost; cancellation refunds reserved recipe inputs and uncollected cargo. In-flight machines must land before cancellation or dismantling. Automatic routes wait without fees; unsafe manual fleet_step moves trigger an emergency brake and a bounded repair fee. A footprint never overlaps another machine. Avoid blocking ports with manually parked machines.

```python
# After signing the charter. Each call pays its real fleet credit cost.
worker = fleet_build("irrigator",1,1,0)
if worker > 0:
    fleet_work(worker,0,"water",8)
```

## 18 / Islands, recipes & orders

Six local depots form a production graph: Hearth grows grain; Reed supplies water; Ember turns ore + grain + water into alloy; Glass needs alloy + water for crystal; Bloom needs grain + crystal + water for medicine; Zenith combines alloy + crystal + medicine into cores. Island opening requires both its order discovery and the corresponding main-farm discovery: Carrots / Pumpkins / Cactus / Mazes / Dinosaurs / Autonomy beacon. Goods are local: num_items(Items.Water) is not fleet_stock(island,"water").

Use fleet_map to inspect each island and its graph. Broad channels form an eastbound/westbound loop; small machines can use narrow shortcuts. Shipments reserve real source stock when accepted, then a physical hauler travels to a dock, loads its hold, crosses each connected channel and waits for a complete landing footprint. Reserved cargo and cargo in flight are not available for another recipe or order.

18 orders require several products and destinations simultaneously. Deliver all consumes the listed local stock once. Later briefs also certify actual hull use, distinct concurrent programs and simultaneous flights; their checklists show current peaks and required values. No elapsed-time certification is used. The six-island campaign follows the farming campaign; editing, debugging and optimal overlapping production can change total completion time.

```python
# Requires a hauler and discovered connected islands. Amount must be in source stock.
accepted = fleet_ship(hauler,1,0,"water",24)
print(accepted)
```

## 19 / A controller for every island

Use the Islands screen to create the 10-file modular example. island_controller imports island_planner; the coordinator allocates physical machines, expands recipe dependencies and sends jobs to individual inboxes. fleet_worker is a shared function library. Each island_water / island_grain / island_foundry / island_crystal / island_medicine / island_archive / island_cargo program imports that library with different allowed recipes.

fleet_run(id,file) binds an independent interpreter to a physical machine. unit_id and the initial island_id are injected; fleet_self() returns the bound machine ID. A worker can import more files, define functions and return results. Inboxes copy message data, and each worker has its own variables and module state. fleet_send sends a job; fleet_receive receives the next message for this worker. fleet_result retrieves a finished result variable. One worker error stops that program and is shown on its card; other workers and physical queues continue. Infinite loops have a bounded instruction budget per simulation tick.

Queued physical jobs, reservations and in-flight cargo are saved. Running Python call stacks, message inboxes and return objects are not saved. After loading, restart the coordinator or explicitly bind the worker program again; check existing jobs before sending duplicates. Stop code ends the interpreter; an active coordinator may restart it, so use Stop all before taking manual control. Cancel queue cancels landed physical jobs. The main farm Run button starts the coordinator, whereas Bind + run on a fleet card starts that machine’s program.

```python
# Main controller; create a program named water_worker first.
if fleet_run(worker,"water_worker"):
    fleet_send(worker,["water",8])
# In water_worker: use fleet_self(), fleet_receive(), fleet_work() and fleet_wait().
```
