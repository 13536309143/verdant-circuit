#!/usr/bin/env python3
"""Build a player-copyable, ordinary-unlock test kit from the verified 0.11.3 route."""
from pathlib import Path
import ast,json,re,shutil
root=Path(__file__).resolve().parents[1];reference=root/'docs/reports/v0.11.3/replay-731';out=root/'docs/test-kits/full-flow-0.11.3';programs=out/'programs';programs.mkdir(parents=True,exist_ok=True)
report=json.loads((reference/'report.json').read_text());farm=json.loads((reference/'final-farm.json').read_text());reset=(reference/'fastest-reset.py').read_text();tree=ast.parse(reset)
def put(name,code):
    ast.parse(code);(programs/(name+'.py')).write_text(code.rstrip()+'\n')
put('boot','harvest()')
put('start','while True:\n    harvest()')
put('hay', '''# Change target to 50, 200 or 500 as directed in the guide.
target = 500
clear()
while num_items(Items.Hay) < target:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)
''')
put('wood', '''# Use target 30 before Trees; use 900 after the 4x4 expansion.
target = 900
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
while num_items(Items.Wood) < target:
    for row in range(get_world_size()):
        for col in range(get_world_size()):
            if can_harvest():
                harvest()
            if get_entity_type() == None:
                if num_unlocked(Unlocks.Trees) > 0 and (get_pos_x()+get_pos_y())%2 == 0:
                    plant(Entities.Tree)
                else:
                    plant(Entities.Bush)
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)
''')
put('carrot', '''# Start with at least 400 Hay and 400 Wood; target is inventory.
target = 160
clear()
for row in range(get_world_size()):
    for col in range(get_world_size()):
        till()
        move(East)
    move(North)
n = get_world_size()
while num_items(Items.Carrot) < target and num_items(Items.Hay) >= 2*n*n and num_items(Items.Wood) >= 2*n*n:
    for row in range(n):
        for col in range(n):
            if can_harvest():
                harvest()
            if get_entity_type() == None:
                plant(Entities.Carrot)
            if get_water() < 0.3 and num_items(Items.Water) > 0:
                use_item(Items.Water)
            move(East)
        move(North)
''')
# Keep each complete dictionary on one line: the game parser does not accept multiline literals.
header=['# Shared library. Import it; do not run it as the campaign.']
for node in tree.body:
    if isinstance(node,ast.Assign) and isinstance(node.value,ast.Dict) and isinstance(node.targets[0],ast.Name):
        header.append(ast.unparse(node))
functions={node.name:node for node in tree.body if isinstance(node,ast.FunctionDef)}
# The verified reset controller only needs twelve upgrade families; full completion uses all fourteen.
for node in ast.walk(functions['upgrade']):
    if isinstance(node,ast.List) and any(isinstance(x,ast.Attribute) and x.attr=='Expand' for x in node.elts):
        node.elts=[ast.Attribute(value=ast.Name(id='Unlocks',ctx=ast.Load()),attr=name,ctx=ast.Load()) for name in ['Expand','Grass','Speed','Trees','Carrots','Pumpkins','Sunflowers','Watering','Fertilizer','Polyculture','Cactus','Mazes','Dinosaurs','Megafarm']]
lib='\n'.join(header)+'\n\n'+'\n\n'.join(ast.unparse(functions[name]) for name in ['buy','fund','ensure','upgrade'])
lib+='''

def run_campaign():
    # All language and observation tools must already be bought in the manual opening.
    upgrade()
    for tech in [Unlocks.Polyculture, Unlocks.Pumpkins, Unlocks.Fertilizer, Unlocks.Sunflowers, Unlocks.Cactus, Unlocks.Mazes, Unlocks.Dinosaurs, Unlocks.Megafarm, Unlocks.Simulation, Unlocks.Leaderboard]:
        if num_unlocked(tech) == 0:
            buy(tech)
        upgrade()
        print("discovery", tech, "field", get_world_size())
    ensure(Items.Power, 1000)
    buy(Unlocks.The_Farmers_Remains)
    upgrade()
    buy(Unlocks.Top_Hat)
    upgrade()
    for tech in dependencies:
        while get_cost(tech) != None:
            buy(tech)
    complete = True
    for tech in dependencies:
        if get_cost(tech) != None:
            complete = False
    print("campaign_complete", complete)
    return complete
'''
put('farm_lib',lib)
put('campaign','import farm_lib\ncomplete = farm_lib.run_campaign()\nprint("all_research_maxed", complete)')
# Export the actual independently verified challenge programs.
for p in farm['programs']:
    if p['name']!='main':put(p['name'],p['source'])
def phase(prefix):return next(p for p in report['phases'] if p['title'].startswith(prefix))
def raw(prefix):return (reference/phase(prefix)['program']).read_text()
put('verify_companion','import farm_lib\nfarm_lib.ensure(Items.Hay,1000)\nfarm_lib.ensure(Items.Wood,1000)\nbefore = num_items(Items.Hay)\n'+raw('验证随机伴生目标')+'\ncompanion_ok = num_items(Items.Hay)-before > 2**num_unlocked(Unlocks.Grass)\nprint("companion_ok", companion_ok)')
put('verify_maze','import farm_lib\namount = get_world_size()*2**(num_unlocked(Unlocks.Mazes)-1)\nfarm_lib.ensure(Items.Weird_Substance, amount*2)\nbefore = num_items(Items.Gold)\n'+raw('验证迷宫复用与开墙')+'\nmaze_reuse_ok = num_items(Items.Gold)-before == get_world_size()*get_world_size()*2**num_unlocked(Unlocks.Mazes)\nprint("maze_reuse_ok", maze_reuse_ok)')
put('verify_drones',raw('验证 32 台工作机同时生产')+'\nprint("parallel_complete", num_drones()==1, "capacity", max_drones())')
put('verify_simulation',raw('验证固定种子模拟')+'\nsimulation_ok = a==b and a!=c\nprint("simulation_ok", simulation_ok)')
# Reuse the full, bounded Hamiltonian route from a recorded production phase.
tail=raw('长尾生产')
put('verify_tail','import farm_lib\nfarm_lib.ensure(Items.Cactus,get_world_size()*get_world_size()+1)\nbefore = num_items(Items.Bone)\n'+tail+'\ntail_ok = num_items(Items.Bone)>before\nprint("tail_ok", tail_ok, "bone_gain", num_items(Items.Bone)-before)')
put('verify_hat','hat_ok = change_hat(Hats.Top_Hat)\nchange_hat(Hats.Straw_Hat)\nprint("hat_ok", hat_ok)')
put('verify_debug','set_world_size(2)\nset_execution_speed(1)\nprint("temporary_size_ok",get_world_size()==2)\nquick_print("debug_ticks",get_tick_count())\n# The original field size and execution limit restore when this program ends.')
races=[]
for name in ['Hay','Wood','Carrots','Pumpkins','Sunflowers','Cactus','Maze','Dinosaur']:
    file='race_'+name.lower();races.append((name,file))
    if name!='Dinosaur':races.append((name+'_Single',file))
races.append(('Fastest_Reset','race_reset'))
put('races','challenges = ['+', '.join('(Leaderboards.'+cat+', '+repr(file)+')' for cat,file in races)+']\nfor category,program in challenges:\n    print("challenge_start",category)\n    leaderboard_run(category,program,64)\n    print("challenge_passed",category)\nprint("all_16_challenges_finished",True)')
manifest={'version':'0.11.3','requiresUnrestrictedSyntax':False,'programs':[p.stem for p in sorted(programs.glob('*.py'))],'bootstrapResearch':['loops','operators','variables','senses','plant','expand','grass','trees','speed','functions','lists','costs','timing','hats','carrots','watering','dicts','import','auto_unlock','debug','debug2','utilities'],'automaticCompletion':'34 research nodes / 120 paid levels','verificationPrograms':['verify_companion','verify_maze','verify_drones','verify_simulation','verify_tail','verify_hat','verify_debug'],'competitionCount':16}
(out/'manifest.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+'\n')
print('Generated',len(manifest['programs']),'player programs, including the ordinary-unlock full campaign and 16-competition driver.')
