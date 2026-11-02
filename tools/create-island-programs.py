#!/usr/bin/env python3
"""Author the hierarchical controller and specialist modules used by the game."""
from pathlib import Path
import ast
root=Path(__file__).resolve().parents[1];programs=root/'docs/test-kits/full-flow-0.12.1/programs';resources=root/'Assets/VerdantCircuit/Resources'
# This coordinator source is preserved as a readable source template.
source=(root/'tools/island-coordinator-source.py').read_text()
source=source.replace('fleet_work(worker["id"],destination,item,1)','fleet_send(worker["id"],["work",destination,item,1])')
source=source.replace('if worker["jobs"] != 0:', 'if worker["jobs"] != 0 or worker["inbox"] != 0:')
source=source.replace('if unit["role"] != "hauler" or unit["jobs"] != 0:', 'if unit["role"] != "hauler" or unit["jobs"] != 0 or unit["inbox"] != 0:')
source=source.replace('fleet_ship(unit["id"],source,destination,item,amount)', 'fleet_send(unit["id"],["ship",source,destination,item,amount])')
source=source.replace('fleet_stock(source,item)-reserve','fleet_stock(source,item)-reserve-reserved[key]')
needle='    demand = {}'
source=source.replace(needle,'''    # Each physical machine runs its own named program with independent variables.
    for unit in units:
        if unit["program_running"]:
            continue
        file = "island_cargo"
        if unit["role"] == "irrigator":
            file = "island_water"
        elif unit["role"] == "grower" or unit["role"] == "harvester":
            file = "island_grain"
            if unit["island"] == 4:
                file = "island_medicine"
        elif unit["role"] == "scout":
            file = "island_crystal"
        elif unit["role"] == "engineer":
            file = "island_foundry"
            if unit["island"] == 5:
                file = "island_archive"
        fleet_run(unit["id"],file)
    demand = {}''')
# A library provides the global recipe tables; one call owns the coordinator loop.
start=source.index('if not fleet_start():')
source=source[:start]+'def run_coordinator():\n'+''.join('    '+line if line.strip() else line for line in source[start:].splitlines(True))
source+='    for unit in fleet_units():\n        fleet_stop(unit["id"])\n'
def put(name,code):
    ast.parse(code);(programs/(name+'.py')).write_text(code);(resources/('Fleet-'+name+'.txt')).write_text(code)
put('island_planner',source)
put('island_controller','import island_planner\nisland_planner.run_coordinator()\n')
put('fleet_worker','''# Imported by specialist programs. Each machine has its own interpreter and inbox.
def run(allowed):
    identifier = fleet_self()
    while True:
        own = None
        for unit in fleet_units():
            if unit["id"] == identifier:
                own = unit
                break
        if own == None:
            break
        if own["jobs"] == 0:
            command = fleet_receive()
            if command != None:
                if command[0] == "work" and command[2] in allowed:
                    fleet_work(identifier,command[1],command[2],command[3])
                elif command[0] == "ship" and "cargo" in allowed:
                    fleet_ship(identifier,command[1],command[2],command[3],command[4])
        fleet_wait(0.5)
''')
for name,allowed in {'water':['water'],'grain':['grain'],'foundry':['ore','alloy'],'crystal':['crystal'],'medicine':['medicine'],'archive':['core'],'cargo':['cargo']}.items():
    put('island_'+name,'from fleet_worker import run\nrun('+repr(allowed)+')\n')
# Retain the old asset name as the entry point for code which loads Source.
(resources/'ArchipelagoController.txt').write_text((programs/'island_controller.py').read_text())
print('Authored',len(list(programs.glob('*.py'))),'coordinator and specialist programs')
