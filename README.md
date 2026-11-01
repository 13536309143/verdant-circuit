# Verdant Circuit · 禾序

**Program the farm. Discover the algorithm.**

**English** · [简体中文](README.zh-CN.md) · [Download 0.11.2](https://github.com/13536309143/verdant-circuit/releases/tag/v0.11.2) · [Test guide](docs/TESTING.md)

Verdant Circuit is a single-player programming farm, independently implemented in Unity. Write Python-inspired code to grow resources, unlock technology and solve different agricultural algorithms.

**0.11.2 adds a staged campaign built around roughly three hours of normal game time.** Crop discoveries open the next field, execution-speed and yield tiers. The research tree shows required discoveries, cumulative production and payable costs. Program cards distinguish active work from the file you are editing, and a searchable bilingual field manual explains every major system.

Updating from 0.11.1 preserves inventory and purchased research. Create a new farm to experience the complete staged progression. Earlier Releases and tags remain available.

![Automation studio](docs/images/discovery-studio-en.png)

## Playable systems

| System | Included in 0.11.2 |
|---|---|
| Production | Grass, bushes, trees, carrots, pumpkins, sunflowers and cacti; hay, wood, power, cactus, bone, gold and supplies |
| Spatial crops | Tree spacing, randomized companion requests, giant pumpkin squares and dead-plant replacement |
| Algorithm crops | Petal measurement and sunflower priority; neighboring cactus swaps and two-dimensional sorted harvests |
| Infection | Fertilizer accelerates growth and infects plants; catalyst toggles local infection and generates mazes |
| Mazes | Seeded connected hedge mazes, treasure, reusable layouts and wall removal |
| Tail mode | Apple targets, growing occupied tails, blocked movement and bone production |
| Drones | Independent function execution, isolated variable/collection copies, spawn handles, joins and returned results; no inter-drone collision |
| Progression | 34 resource-funded research nodes / 120 levels; crop-gated expansion, speed and yields; cumulative-production milestones and automatic research |
| Programming | Parameters, defaults, returned values, recursion, closures, modules, lists, tuples, sets, dictionaries, indices and unpacking |
| Code studio | Program cards with file/active counts and editing/running/paused states; draggable code windows, 100-state undo/redo, pause/step, breakpoints, variable watch and hover |
| Experiments | Configurable isolated simulations with deterministic seeds and local fixed-start competition |
| Field manual | 16 searchable bilingual chapters, 33 API groups, crop strategies, code fragments and troubleshooting |
| Feedback | Resource statistics, personal bests, persistent local achievements and challenge records |
| Files | Independent save/backup, exported `.py` files, interface hints, external-file watching and output log |
| Languages | English by default; Simplified Chinese through Settings or F8 |

This is an independent reimplementation of programming-farm mechanics, not the original Steam application. It uses project-owned code, geometry and synthesized audio. Its interface, economy curves, execution accounting and local challenge targets are defined by this project. See [implementation scope](docs/PARITY-0.11.2.md) for evidence and limits.

![Program cards — UI demonstration farm](docs/images/program-cards-en.png)

## Download and run

Get the complete package from [v0.11.2 Releases](https://github.com/13536309143/verdant-circuit/releases/tag/v0.11.2):

- **Mac:** `HeXu-macOS-0.11.2.zip` → extract and open `HeXu-0.11.2.app`. Intel and Apple Silicon are included.
- **Windows x64:** `HeXu-Windows-x64-0.11.2.zip` → extract the entire folder and open `HeXu.exe`. Keep the data folder, UnityPlayer.dll and Mono runtime beside it.
- **Source:** `HeXu-UnityProject-0.11.2.zip` → extract and add its root in Unity Hub.

No Unity installation is needed to play. Checksums are supplied in the release. The Mac app is not distribution-signed or notarized. Windows is cross-built on Mac; native Windows acceptance remains outstanding.

## First steps

1. The fresh farm has one plot and no inventory. Let its grass mature for a few seconds.
2. Run `harvest()` to collect hay. Repeat until you can research **Loops**.
3. Open Templates and create **First harvest**. Its loop will keep harvesting the starting plot.
4. Research planting, expansion, operators, variables and sensors. Use new production to fund further branches.
5. Advanced systems require new programs: repair pumpkin squares, sort cacti, search mazes or coordinate drones.

```python
while True:
    harvest()
```

After unlocking the required language features and field expansion:

```python
while True:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            move(East)
        move(North)
```

Expansion follows discoveries: **Planting 2×2 → Trees 4×4 → Pumpkins 8×8 → Cacti 16×16 → Tail exploration 32×32**. Each tier is paid separately. Speed and yield levels follow their own discovery lists; producing enough hay alone cannot unlock every tier. Cumulative production counts harvests even after the resource is spent.

[Read the complete field manual](docs/MANUAL.md) · [Three-hour balance and measured playthrough](docs/BALANCE-3H.en.md)

Use Settings → Unrestricted syntax when experimenting without the language-research gates. Crop availability and resource costs still apply.

| Control | Action |
|---|---|
| Run / stop | Studio buttons; Cmd/Ctrl+Enter runs the selected program |
| Undo / redo | Cmd/Ctrl+Z / Cmd/Ctrl+Shift+Z or Ctrl+Y |
| Pause / step | Studio buttons |
| Research tree | F2; drag to pan, scroll to zoom, 1–4 to switch areas |
| Breakpoint | F9 at the caret line, or enter line numbers in Settings |
| Variable value | Hover a variable in code, or read the watch panel |
| Multiple files | Programs; import a file by its name without `.py` |
| Floating code window | Programs → Window; drag its title to move it |
| Field inspection / zoom | Click a plot / scroll over the farm |
| Language | F8 or Settings |
| Settings / close dialog | Esc |

Browsing another file keeps existing work running. Main Run starts a fresh controller and replaces current tasks. A floating window can launch another independent program if a drone slot is available. Stop halts all tasks. Worker drones disappear after completing their functions; handles remain usable for `wait_for()` results. Programs resume through explicit Run after loading, rather than restoring interpreter stacks.

Research is a connected discovery tree, including programming syntax. Use **F2**, drag to pan, scroll to zoom, or keys **1–4** to jump between the overview and three branches. Select a node for its effect, prerequisites and live resource requirements.

## Interfaces and experiments

The in-game Guide lists the implemented APIs. Useful groups:

```python
# Actions and observations
harvest()
plant(Entities.Carrot)
till()
move(North)
can_move(East)
use_item(Items.Water)
get_water()
measure()
swap(East)
get_companion()

# Research and resources
num_items(Items.Hay)
get_cost(Unlocks.Speed)
get_unlock_requirements(Unlocks.Speed)
can_unlock(Unlocks.Speed)
num_produced(Items.Wood)
num_unlocked(Unlocks.Speed)
unlock(Unlocks.Speed)

# Independent workers
def work(amount):
    return amount * 2

handle = spawn_drone(work, 7)
if handle != None:
    result = wait_for(handle)
    print(result)
```

These calls are an API index rather than a complete farming program. The language is not full Python and does not expose the operating system or Python's standard library.

Create a separate program named `experiment`, then run:

```python
duration = simulate("experiment", Unlocks, {Items.Carrot: 10000}, {}, 7, 64)
print(duration)
```

Simulation starts from a separate configured farm and returns elapsed game time. A fixed seed makes repeated starts reproducible. Simulations and resource challenges are bounded to one game hour; the staged Fastest Reset challenge allows four; nested simulation is unsupported. Current-source resource competitions use a mid-tier 8×8 preset and **different production quantities by resource**, followed by program termination; see the in-game challenge list. Historical records remain separate. They use this project's presets and do not submit to the original game's Steam leaderboards. Steam integration is deferred; local records and achievements are available now.

Completed achievements and successful challenges display a grouped completion notice. Acknowledge it or open Records; acknowledged achievements do not repeat, and unread notices survive restarting.

## External editor and saves

Settings shows the actual save path. The new format lives under:

```text
Application.persistentDataPath/Automation/<farm>.json
```

Settings → Farm library → Manage creates and loads independent farms, each with its own progress, programs and records. The default file is `farm-0.11.2.json`. The previous save is retained as `.bak`. Code and output are stored under the same Automation folder. Enable **External editor → Export and watch** to export `.py` files plus `__builtins__.py` hints; saved external edits are reloaded. Avoid editing the same file in two places simultaneously. Use the Programs panel to remove files from the game.

## Develop and build

Use **Unity 2022.3.62f3c1**, matching `ProjectSettings/ProjectVersion.txt`, with an active license and the relevant Mac / Windows Mono build module. Dependencies are pinned in `Packages/`, including Unity Localization and Addressables. Unity MCP is optional.

```bash
git clone https://github.com/13536309143/verdant-circuit.git
cd verdant-circuit
```

Open `Assets/VerdantCircuit/Scenes/VerdantCircuit.unity` and press Play. Release tools are under **Tools → 禾序 → 发行**. Close this project's Editor before command-line builds:

```bash
export VERDANT_UNITY_EDITOR="/path/to/Unity.app/Contents/MacOS/Unity"
bash tools/build-macos.sh
bash tools/build-windows.sh
```

After editing manual or UI copy, run `python3 tools/update-content.py` to regenerate the bilingual registry and Markdown manuals. The build scripts generate Unity localization assets, run model/interpreter checks, build and package into `Builds/release/`. They do not upload or sign packages. This is a local single-player game; no backend server is required.

The [0.11.2 playthrough and balance report](docs/BALANCE-3H.en.md) records three zero-inventory runs, all 34 nodes / 120 paid levels, 13 achievements, 16 competition records, timing and reproducible algorithms. The [one-hour](docs/BALANCE-1H.md) and [earlier playthrough](docs/FULL_PLAYTHROUGH.md) reports are historical baselines.

The rebuilt implementation is in `Assets/VerdantCircuit/Scripts/Automation/`. Test reports and the manual acceptance procedure are in [docs/TESTING.md](docs/TESTING.md). The old standalone fleet/island Python prototype under `tools/` is historical and does not validate this interpreter or world.

## Release history

- **0.11.2 — Staged Discovery:** three-hour progression, crop-gated upgrades, program cards, expanded manual, discovery tree, completion notices and audio slider.
- **0.11.1 — Automation Rebuild:** resource economy, language runtime, crop algorithms and independent function drones.
- **0.10.2 / 0.10.1:** earlier designs; preserved as historical releases.

## Credits

Inspired by the programming-farm genre and *The Farmer Was Replaced*. No executable, proprietary game code, artwork, soundtrack or manual from the Steam installation is included. Fonts and engine packages keep their original licenses; see [third-party notices](THIRD_PARTY_NOTICES.md). No project-wide open-source license has been selected.
