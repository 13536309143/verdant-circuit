# 0.12.1 design: a programmable island network

The farming route teaches loops, spatial planting, sorting, search and parallel work. The island campaign adds a second problem: satisfying several local consumers with a limited physical fleet. A single crop loop can no longer solve every task. The player's code chooses production roles, batch sizes, intermediate reserves, cargo allocation and concurrent island programs.

## Independent progression

Three fleet variables are deliberately independent:

| Variable | Range | Discovery | Payment |
|---|---|---|---|
| Slot limit | 3–8 | Completed orders 2/4/6/8/10 | 30/50/70/90/110 credits |
| Hull tier | 1×1, 2×1, 2×2, 3×2, 3×3 | Orders 3/6/11/15 for larger hulls | 35/60/85/110 credits |
| Specialism | Six roles | Engineer5, harvester6, scout8; three starter types | 35 credits per extra role |

Discoveries gate research; buying a larger frame does not create another slot. A large frame pays for its complete footprint, reserves area-sized recipe inputs, produces area-sized batches and carries more cargo. It also moves more slowly and loses the small-frame shortcut.

The current processing formula is `base_seconds × (0.8 + 0.2 × sqrt(area))`; harvesters multiply grain processing time by0.8. Movement uses `0.45 + 0.075 × (area−1)`, plus0.06 for haulers. These are physical production and movement costs, not mission timers.

## Production graph and discoveries

Hearth produces grain; Reed provides efficient water; Ember mines ore and produces alloy; Glass refines crystal; Bloom makes medicine; Zenith assembles cores. Each island has its own depot. Recipes use only that island's available inventory.

Islands require both order discovery and the corresponding crop discovery: Carrots, Pumpkins, Cactus, Mazes, Dinosaurs and Autonomy beacon. Players can interleave the two campaigns. The tested reference completes farming research first so its timing is easy to reproduce.

The18contracts introduce irrigation budgeting, two-way shipping, larger footprints, shared lanes, intermediate reserves, crystal and medicine dependencies, heavy lift and six-island delivery. Goals are simultaneous stock requirements. Later briefs also check actual hull use, distinct concurrent program names and simultaneous flights. They never require an arbitrary number of seconds to pass.

## Deterministic physical scheduling

Each island is a20×14grid. Bodies occupy axis-aligned rectangles; every occupied cell must be terrain and free of another landed body. Builds, moves, rotations and landings commit atomically on one simulation thread. Broad bridges form a directed circulation loop, while1×1machines retain a narrow shortcut. Island variants move the eastbound bridge and add a right-bank obstruction.

Each quarter-second step runs bounded per-machine code, records contract telemetry, updates movement/flight delays and landing reservations, then advances physical jobs with rotating unit priority. Local BFS considers terrain, lane direction, the complete footprint and live occupancy. A waiting machine can yield to a free adjacent cell. Docking requires one of four explicit pad anchors; accepting an arbitrary nearby point previously blocked every pad and was removed during testing.

Routes are collision-free by construction. This does not promise that every player-created traffic arrangement is deadlock-free: manually parking a large body across a required approach can block useful work. The player can cancel, move or dismantle landed machines. In-flight cargo waits for a complete landing footprint and is not discarded.

Air transport uses abstract connected flight segments, not continuous aerodynamic collision simulation. Ground bodies and landing pads are the spatial scheduling problem in this version.

## Reservations and accounting

A shipping command is accepted only when real source stock is available. Acceptance moves goods into a reserved manifest. The machine must travel to a source pad, load its hold, cross connected islands and unload. A queued manifest, carried cargo and local stock are distinct accounts.

Processing reserves area-sized inputs when work starts. Cancellation returns the active recipe inputs and uncollected manifests; landed cargo returns to its current depot. Dismantling refunds the paid hull cost. Stop all terminates code, cancels landed work, and lets airborne goods land before halting their later jobs.

For every item:

```text
produced = recipe consumption + submitted orders + exports
         + local stock + reserved manifests + carried cargo
```

For fleet credits:

```text
balance = 100 charter credits + order rewards + hull refunds + export income
          − hull purchases − research − emergency fees
```

Normal queued routes wait without fees. Unsafe manual moves brake and charge at most two credits with a cooldown. Credits cannot go negative. Surplus exports provide a production-based recovery path after repair penalties. Sales consume available goods and cannot sell a reserved manifest. Monetary transactions round to cents, and the UI displays cents to avoid false affordability at integer research boundaries.

## Hierarchical programming

The default example has ten files: entry, recipe planner, shared worker library and seven specialist entry files. The planner expands recipe demand in reverse topological order and accounts for incoming shipments and in-progress batches. It sends copied commands to independent per-body interpreters.

`fleet_run` binds a file; ordinary module imports call shared functions; `fleet_send` and `fleet_receive` exchange copied values through16-message inboxes; `fleet_result` retrieves a completed value. Each interpreter has its own module scopes and a bounded instruction budget. Heavy code cannot accumulate negative scheduling debt that cancels a later sleep. Self-dispatch atomically replaces the old task, and a failed unit program leaves peers running.

The optional six-file example uses another architecture: a finite master launches four autonomous island programs, then ends. Water, grain, ore/alloy and cargo programs continue independently by checking their own inventory requirements. This validates code-based island control beyond the default central planner.

Physical queues, timers, manifests, depots and funding accounts persist. Interpreter stacks, transient inboxes and return objects do not. Reloading a farm resumes physical work; explicitly restart the coordinator or a worker program, checking existing jobs before duplication.

## Verification and intended duration

The reference campaign is designed to exceed five hours at1×through distinct mechanics. It has no enforced minimum playtime, and efficient overlap should be rewarded. Exact three-seed timing, normal-cost playthroughs, CPU/error isolation, mid-recipe/mid-flight reloads, material/funding reconciliation and platform limits are in the [0.12.1 report](../reports/v0.12.1/README.md).
