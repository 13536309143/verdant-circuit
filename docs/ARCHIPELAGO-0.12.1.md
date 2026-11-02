# Archipelago Operations · 0.12.1

[English](ARCHIPELAGO-0.12.1.md) · [完整中文手册与全部代码](ARCHIPELAGO-0.12.1.zh-CN.md)

Six island depots, seven cargo goods, eight recipes, six machine roles, five hull tiers and 18 multi-product contracts extend the farming campaign. Slot count, hull footprint and specialism are independent paid research tracks. Progress is tied to discoveries and useful production, with no elapsed-time certification gates.

## Start a network

Discover Carrots, research the language tools used by the example (Loops, Operators, Variables, Functions, Lists, Dictionaries, Import, Debugging and Timing), and prepare **100 Wood + 100 Carrots**. Open **Islands** and sign the charter. It pays those resources once and provides 100 fleet credits.

Select **Create modular example (10 files)**. This preserves existing programs of the same name. Run `island_controller` from the main editor. Alternatively, import the 35-file full-flow test kit through Settings → External editor → Export and watch. The farming route and fleet route use their normal resource costs.

The left panel shows a selected island's actual terrain, facilities and occupied machine footprints. Select a machine on the map or a card. Role and hull buttons cycle the build configuration. Use the research buttons to unlock slots, larger hulls or the selected role. The command panel queues processing batches or a full cargo hold; select a program file to bind an independent interpreter to the selected machine.

## Three levels of code

```text
island_controller
└── island_planner.run_coordinator()
    ├── island_water      → Reed irrigation
    ├── island_grain      → Hearth grain and harvesting
    ├── island_foundry    → Ember ore / alloy
    ├── island_crystal    → Glass crystal
    ├── island_medicine   → Bloom medicine
    ├── island_archive    → Zenith cores
    └── island_cargo      → inter-island shipping
        All specialists import fleet_worker.run(...).
```

`import` calls shared functions synchronously. `fleet_run(id, file)` starts a separate interpreter attached to a physical machine. The worker receives `unit_id`, its initial `island_id`, and `fleet_self()`. Each worker has independent variables, module state and a bounded inbox. `fleet_send(id, payload)` copies a message; `fleet_receive()` returns the next message for that worker or `None`. `fleet_result(id)` reads a completed program's `result` variable.

The coordinator expands recipe dependencies, accounts for incoming shipments and in-progress batches, purchases ordinary fleet research, and sends jobs to specialists. You may replace one specialist, write an island-level dispatcher, or replace the whole planner. Worker exceptions stop that program and appear on its card; other interpreters and physical queues continue.

## Islands and products

| ID | Island | Products | Order discovery + main-farm discovery |
|---|---|---|---|
| 0 | Hearth Garden | Grain; early water | Charter + Carrots |
| 1 | Reed Reservoir | Higher-yield water | 2 completed orders + Pumpkins |
| 2 | Ember Foundry | Ore and alloy | 5 orders + Cactus |
| 3 | Glass Reef | Crystal | 8 orders + Mazes |
| 4 | Bloom Conservatory | Medicine | 11 orders + Dinosaurs |
| 5 | Zenith Archive | Cores | 14 orders + Autonomy beacon |

Grain consumes water. Alloy consumes ore, grain and water. Crystal consumes alloy and water. Medicine consumes grain, crystal and water. Cores consume alloy, crystal and medicine. Sites share their island's depot; **inter-island goods must be reserved, loaded, flown and unloaded**. Main-farm `Items.Water` and island cargo `"water"` are distinct stocks.

`fleet_map(island)` describes terrain, recipes, coordinates and island links. Connections are 0–1, 0–2, 1–2, 1–3, 2–3, 2–4, 3–4, 3–5 and 4–5. Non-adjacent destinations require multiple physical flight segments.

## Bodies, lanes and funding

Slots start at 3 and increase to 8. Hulls unlock as 1×1 → 2×1 → 2×2 → 3×2 → 3×3, including rotated footprints. Engineers, scouts and harvesters have their own order gates. Irrigators, growers and haulers are initially available.

A hull costs `10 + 5 × occupied area`. A hauler carries `12 × area`. Processing inputs and outputs scale with area, while movement becomes slower. Broad channels form a directed circulation loop; smaller machines can use narrow shortcuts. Routes check every body cell and reserve movements atomically. Idle vehicles clear docking pads. Automatic queues wait without repair charges. Unsafe manual `fleet_step` calls trigger an emergency brake and a fee capped at two credits, with a cooldown preventing repeated per-frame fees.

Accepted shipping commands reserve available source stock immediately. A manifest and cargo in flight cannot be spent again. Cancellation refunds uncollected manifests and reserved recipe inputs; dismantling a landed machine refunds its paid hull cost. Flights must land before cancellation or dismantling. **Stop all** ends the coordinator and specialists, cancels landed queues, and lets airborne cargo land safely before stopping.

Export surplus through `fleet_sell(island,item,amount)` or the Export 12 button to recover working capital after repair penalties. `fleet_market()` returns prices; reserved or in-flight cargo cannot be sold. Exports consume real goods and never bypass order discoveries. The optional six-file `examples` kit launches a three-island autonomous network whose master can end while four specialist programs continue producing.

## Contracts and saved work

Each contract lists island, item, available stock and target. All goals must be ready together. Delivery consumes those quantities once and awards fleet credits. Later briefs certify actual use of larger hulls, distinct concurrent programs and simultaneous flights. The checklist shows measured peaks and requirements; it does not wait for an arbitrary number of seconds.

Save files contain local depots, hulls, queues, reserved inputs, processing timers, flight manifests, order completion times and credit accounts. As with the main interpreter, Python call stacks are not saved; transient messages and return objects are not saved either. After loading, restart the coordinator or rebind specialist files. Inspect existing jobs before sending duplicates. A supervising coordinator may restart stopped specialist code; stop the coordinator before taking manual control.

The farming route retains its 34 nodes / 120 levels, 13 achievements and 16 independent local race categories. Fleet research and orders are additional progression. World speed settings accelerate the full game; ordinary research/power execution bonuses remain active. Human programming time and the independent clocks used by races are separate from main-campaign timing.

See the [Chinese guide and complete code](ARCHIPELAGO-0.12.1.zh-CN.md), [field manual](MANUAL.md) and [validation report](reports/v0.12.1/README.md). Native Windows execution still requires Windows hardware; a successful macOS cross-build does not establish that result.
