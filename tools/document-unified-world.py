#!/usr/bin/env python3
"""Export human references and copyable programs from Unity's runtime metadata."""
from pathlib import Path
import json,hashlib,ast
root=Path(__file__).resolve().parents[1]
data=json.loads((root/'docs/reference/world-0.13.0.json').read_text())
kit=root/'docs/test-kits/unified-world-0.13.0';(kit/'programs').mkdir(parents=True,exist_ok=True)
for example in data['examples']:
 ast.parse(example['code']);(kit/'programs'/(example['name']+'.py')).write_text(example['code'])
recipes={r['item']:r for r in data['recipes']}
for en,suffix in [(True,''),(False,'.zh-CN')]:
 title='Verdant Circuit 0.13.0 — Complete Field Manual' if en else '禾序 0.13.0 — 地球与群岛完整手册'
 parts=['# '+title,'[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)', 'This manual is generated from the shipped runtime reference: 24 chapters, 127 individual function/method entries, 28 recipes and 11 hierarchical programs. Examples run inside the game, not system Python.' if en else '本手册从游戏实际接口与配方导出：24个章节、127个独立函数／方法条目、28份配方、11个分层控制程序。示例在游戏中运行，不是在电脑系统Python中运行。']
 for c in data['chapters']:
  parts+=['## '+c['enTitle' if en else 'zhTitle'],c['en' if en else 'zh']]
  if c.get('code'):parts+=['```python\n'+c['code']+'\n```']
  categories={'api':['earth','builtin'],'world_api':['fleet','control'],'world_products':['product']}.get(c['id'],[])
  for f in data['functions']:
   if f['category'] not in categories:continue
   parts+=['### `'+f['signature']+'`',f['en' if en else 'zh']]
   for field,eng,zh in [('parameters','Parameters','参数'),('returns','Returns','返回值'),('requirements','Requires','解锁与使用条件'),('errors','Failure','失败与边界情况')]:parts+=['**'+(eng if en else zh)+'**: '+f.get(field+'En' if en else field,'')]
   if f['category']=='product':
    r=next(r for r in data['recipes'] if r['function']==f['name']);parts+=['**'+('Recipe' if en else '配方')+'**: '+f"island {r['island']} / {r['role']} / {r['output']} {r['item']} / {r['seconds']:g}s; "+(' + '.join(f"{v['amount']:g} {v['id']}" for v in r['inputs']) or ('no inputs' if en else '无原料'))]
   parts+=['```python\n'+f['example']+'\n```']
 parts+=['## '+('All island IDs' if en else '全部岛屿编号'),'| ID | '+('Island | Research | Original order threshold' if en else '岛屿 | 科技 | 原物流订单门槛')+' |','|---|---|---|---|']
 for island in data['islands']:parts+=['| '+str(island['id'])+' | '+island['en' if en else 'zh']+' | '+(island['research'] or 'Earth')+' | '+str(island['order'])+' |']
 parts+=['## '+('All production recipes' if en else '完整生产配方'),'Base 1×1 values. Inputs and outputs scale with hull area; processing seconds multiply by 0.8 + 0.2 × sqrt(area), and harvesters use a further 0.8 multiplier.' if en else '以下为1×1基础值。投入与产出乘机体面积，实际加工秒数乘0.8＋0.2×sqrt(面积)，采收／渔业机再乘0.8。','| Item | Island | Role | Output | Seconds | Inputs | Function |','|---|---|---|---|---|---|---|']
 for r in data['recipes']:parts+=['| '+r['item']+' | '+str(r['island'])+' | '+r['role']+' | '+str(r['output'])+' | '+str(r['seconds'])+' | '+(' + '.join(f"{v['amount']:g} {v['id']}" for v in r['inputs']) or '—')+' | '+r['function']+' |']
 parts+=['## '+('Complete hierarchical example files' if en else '分层控制完整程序文件'),'Create these with Programs → Create the unified example. Bind unified_controller to Network. Managers need unlocked targets, spare drone slots and credits. Arrange input shipments separately; the example demonstrates hierarchy, not an automatic solution to every contract.' if en else '在程序中心点击“创建统一控制范例”即可生成这些文件，把unified_controller绑定到联合总控。管理程序需要解锁目标岛、有空余机位与资金；补给需另行安排，范例展示调用层级，不会自动解决全部订单。']
 for example in data['examples']:parts+=['### '+example['name']+'.py','```python\n'+example['code']+'\n```']
 parts+=['## '+('Test scope and save behavior' if en else '验证范围与存档行为'),'See [the validation report](reports/v0.13.0/README.md). Physical jobs, cargo, bindings and group membership are saved; program stacks, inboxes and results are not. Restart a controller after loading. Existing Earth and old island-0 storage merge once. This local build has not been published to GitHub.' if en else '详见[验证报告](reports/v0.13.0/README.md)。实体任务、货物、程序绑定和机群成员会保存，调用栈、收件箱和结果不保存；载入后重启相应总控。旧地球库存与旧0号岛仓库只合并一次。本版是本地测试版本，尚未发布GitHub。']
 manual='\n'.join(line.rstrip() for line in '\n\n'.join(parts).splitlines())+'\n';(root/('docs/MANUAL'+suffix+'.md')).write_text(manual)
 # The kit is portable: keep both manual files beside its README.
 (kit/('MANUAL'+suffix+'.md')).write_text(manual.replace('reports/v0.13.0/README.md','validation/README.md'))
 quick=('# '+('Unified Earth & Islands — 0.13.0' if en else '地球与群岛统一流程 — 0.13.0')+'\n\n'+
 ('[Full function manual](MANUAL.md) · [中文](README.zh-CN.md)\n\n1. Enter Earth and develop the original crop route.\n2. Research Parallel Drones, Modules, Dictionaries and Irrigation, then Archipelago (500 treasure gold).\n3. Open Flowers (2000 pumpkin), Minerals (5000 cactus) and/or Fish (20000 carrot) from Research.\n4. In Programs choose a target on the left, a file on the right, then Bind + run.\n5. Create the unified example; bind unified_controller to Network. It calls island managers, which bind independent drone programs.\n6. Ship actual Earth inputs. Example: fleet_ship(hauler, 0, 6, "water", 24). Wait for real delivery before processing.\n7. Build cross-island recipes, return products and fulfill the 24 contracts.\n\n' if en else '[完整函数手册](MANUAL.zh-CN.md) · [English](README.md)\n\n1. 进入地球，沿原有作物科技路线推进。\n2. 解锁并行无人机、模块、字典和灌溉，再以500宝藏金币研究地球与群岛网络。\n3. 在科技树分别开放花卉岛（2000南瓜）、矿物岛（5000仙人掌）或鱼类岛（20000胡萝卜）。\n4. 程序中心左侧选目标，右侧选文件，再绑定并运行。\n5. 创建统一控制范例，把unified_controller绑定到联合总控，它调用各岛管理，再绑定单机程序。\n6. 实际运输地球原料，例如fleet_ship(运输机号, 0, 6, "water", 24)，等实际送达后再加工。\n7. 建立跨岛高级配方，运输特产回地球，完成24份订单。\n\n')+
 ('Programs are stored in programs/. File names omit .py in game calls. The 11 examples require programming research and appropriate island/role unlocks. They teach control hierarchy; no money, resources or technology are granted.\n\n' if en else '完整程序在programs/。游戏调用文件名不写.py。11个范例需要相应语法、岛屿和机型科技，只演示控制层级，不赠送资金、物资或研究。\n\n'))
 for e in data['examples']:quick+='### '+e['name']+'\n\n```python\n'+e['code']+'\n```\n\n'
 (kit/('README'+suffix+'.md')).write_text(quick)
 (root/('docs/WORLD-0.13.0'+suffix+'.md')).write_text(quick.replace('(MANUAL.md)','(MANUAL.md)').replace('(README.zh-CN.md)','(WORLD-0.13.0.zh-CN.md)').replace('(README.md)','(WORLD-0.13.0.md)'))
manifest={'version':data['version'],'chapters':len(data['chapters']),'functions':len(data['functions']),'recipes':len(data['recipes']),'programs':{p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in sorted((kit/'programs').glob('*.py'))}}
(kit/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n')
print('Generated complete bilingual manuals and',len(manifest['programs']),'copyable programs.')
