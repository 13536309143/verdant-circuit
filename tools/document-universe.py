#!/usr/bin/env python3
from pathlib import Path
import json,hashlib,ast,re
root=Path(__file__).resolve().parents[1];d=json.loads((root/'docs/reference/world-0.13.2.json').read_text());kit=root/'docs/test-kits/universe-0.13.2';(kit/'programs').mkdir(parents=True,exist_ok=True)
for e in d['examples']:ast.parse(e['code']);(kit/'programs'/(e['name']+'.py')).write_text(e['code'])
source=(root/'Assets/VerdantCircuit/Scripts/Automation/UnifiedExamples.cs').read_text()
def array(name):return re.findall(r'"([^"\n]*)"',re.search(r'string\[\] '+name+r'=\{(.*?)\};',source,re.S)[1])
ids,enNames,zhNames=array('Ids'),array('En'),array('Zh');labelsEn=dict(zip(ids,enNames));labelsZh=dict(zip(ids,zhNames))
for en,suffix in [(True,''),(False,'.zh-CN')]:
 labels=labelsEn if en else labelsZh
 text=['# '+('Verdant Circuit 0.13.2 — Universe Manual' if en else '禾序0.13.2 — 四星宇宙完整手册'),'[English](MANUAL.md) · [简体中文](MANUAL.zh-CN.md)',f"{len(d['chapters'])} "+('chapters' if en else '章')+f" · {len(d['functions'])} "+('individual function/method entries' if en else '个独立函数／方法条目')+f" · {len(d['recipes'])} "+('recipes' if en else '份配方')+' · 11 '+('complete program files' if en else '个完整程序文件')]
 for c in d['chapters']:
  text+=['## '+c['enTitle' if en else 'zhTitle'],c['en' if en else 'zh']]
  if c['code']:text+=['```python\n'+c['code']+'\n```']
  categories={'api':['earth','builtin'],'world_api':['control','fleet','legacy'],'world_products':['product']}.get(c['id'],[])
  for f in d['functions']:
   if f['category'] not in categories:continue
   text+=['### `'+f['signature']+'`',f['en' if en else 'zh']]
   for key,english,chinese in [('parameters','Parameters','参数'),('returns','Returns','返回值'),('requirements','Requires','使用条件'),('errors','Failure','失败与边界')]:text+=['**'+(english if en else chinese)+'**: '+f[key+'En' if en else key]]
   if f['category']=='product':
    recipe=next(r for r in d['recipes'] if r['function']==f['name']);text+=['**'+('Recipe' if en else '实际配方')+'**: '+f"{recipe['output']} {labels.get(recipe['item'],recipe['item'])} / {recipe['seconds']:g}s / "+(' + '.join(f"{v['amount']:g} {labels.get(v['id'],v['id'])}" for v in recipe['inputs']) or ('no material inputs' if en else '不消耗原料'))]
   text+=['```python\n'+f['example']+'\n```']
 text+=['## '+('Canonical item glossary' if en else '规范物品名称对照'),'| ID | '+('Name' if en else '名称')+' |','|---|---|']
 for item in ids:text+=['| '+item+' | '+labels[item]+' |']
 text+=['## '+('Planet IDs and native workers' if en else '星球编号与本地载具'),'| ID | Planet | Native worker |','|---|---|---|']
 for p,role in zip(d['islands'],['drone','bee','boat','mining_drone']):text+=['| '+str(p['id'])+' | '+p['en' if en else 'zh']+' | '+role+' |']
 text+=['## '+('All 31 production recipes' if en else '全部31份生产配方'),'| Item | Planet | Worker | Output | Seconds | Inputs | Function |','|---|---|---|---|---|---|---|']
 for r in d['recipes']:text+=['| '+r['item']+' | '+str(r['island'])+' | '+r['role']+' | '+str(r['output'])+' | '+str(r['seconds'])+' | '+(' + '.join(f"{v['amount']:g} {v['id']}" for v in r['inputs']) or '—')+' | '+r['function']+' |']
 text+=['## '+('Planet-exclusive research' if en else '各星球专属科技'),'| Planet | ID | Name | Base cost | Max level | Parent | Effect |','|---|---|---|---|---|---|---|']
 for t in d['technologies']:text+=['| '+str(t['planet'])+' | '+t['id']+' | '+t['en' if en else 'zh']+' | '+str(t['cost'])+' '+t['item']+' | '+str(t['max'])+' | '+t['parent']+' | '+t['effectEn' if en else 'effectZh']+' |']
 text+=['Costs double per purchased level. Universe research pays Earth stock; planet research pays local stock.' if en else '费用随已购等级逐级翻倍；宇宙科技从地球库存支付，星球科技使用本地库存。','## '+('All 24 universe projects' if en else '全部24个宇宙项目')]
 for index,project in enumerate(d['projects']):text+=['### '+str(index+1)+'. '+project['en' if en else 'zh'],project['lessonEn' if en else 'lessonZh'],' · '.join(str(g['planet'])+' / '+g['item']+' × '+str(g['amount']) for g in project['goals'])]
 text+=['## '+('Complete universe example files' if en else '宇宙分层控制完整代码'),'Create these using Programs → Create files. Run universe_controller on the Universe target. Supply materials by UFO before expecting outputs. These examples demonstrate hierarchy; they do not grant research/resources or solve every contract automatically.' if en else '在程序中心创建范例文件，将universe_controller绑定到宇宙总控。先通过UFO补给再等待产出。范例展示分层调用，不赠送科技与资源，也不会自动解决全部订单。']
 for e in d['examples']:text+=['### '+e['name']+'.py','```python\n'+e['code']+'\n```']
 text+=['## '+('Verification and limits' if en else '验证与限制'),'See [the current validation report](reports/v0.13.2/README.md). The complete fresh reference walkthrough starts from one plot with zero inventory; timing depends on the documented worker and scheduling policy. Windows is cross-built and still needs native hardware acceptance. Download the public preview from the repository Releases page.' if en else '详见[当前验证报告](reports/v0.13.2/README.md)。从一格零库存贯通24个项目的参考路线记录实际生产与运输时间；策略、机体和并行调度会改变耗时。Windows为交叉构建，仍需真机验收。公开测试版可从仓库Releases页面下载。']
 manual='\n'.join(line.rstrip() for line in '\n\n'.join(text).splitlines())+'\n';(root/('docs/MANUAL'+suffix+'.md')).write_text(manual);(kit/('MANUAL'+suffix+'.md')).write_text(manual.replace('reports/v0.13.2/README.md','validation/README.md'))
 quick=['# '+('Four-planet universe — 0.13.2' if en else '四星宇宙 — 0.13.2 操作与代码'),'[English](README.md) · [简体中文](README.zh-CN.md)',
 '1. Choose Earth, initially the only unlocked planet. Harvest the first plot and develop the original crops.\n2. Purchase Spaceflight after Parallel Drones, Modules, Dictionaries and Irrigation.\n3. Research Flower, Ocean and Mineral planets. Enter one to receive its native starter worker.\n4. Use Colony for local surface, count and hull research. Top navigation always switches between tools.\n5. Use Programs to select Universe/planet/swarm/worker on the left and files on the right.\n6. Create the 11 files below. Run universe_controller on Universe.\n7. Build a UFO, send actual Earth water/compost/fish feed/solar energy, and check arrival through ufo_fleet.\n8. Establish flowers, fish catches and mineral refining. Bring products back and fulfill 24 projects.\n9. Full extents are360×128; read width and height independently, and never assume latitude wraps.' if en else '1. 在宇宙首页选择地球，初始只有地球开放，从一格收获开始发展原有作物。\n2. 在并行无人机、模块、字典和灌溉后研究星际航行。\n3. 分别研究花卉、海洋和矿物星球，进入时获得各自本地初始载具。\n4. 在殖民地面板研究表层范围、集群数量和机体尺寸。顶栏切换各工具。\n5. 程序中心左选宇宙／星球／集群／单体，右选文件。\n6. 创建下方11文件，把universe_controller绑定到宇宙总控运行。\n7. 建UFO，实际运送地球水、堆肥、鱼饵和太阳能，通过ufo_fleet检查到货。\n8. 建立花田、鱼群捕捞、矿物冶炼，回运产物并完成24个项目。\n9. 满级360×128，宽高分别查询，纬度永远不环绕。',
 '[Complete function manual](MANUAL.md)' if en else '[完整函数手册](MANUAL.zh-CN.md)',
 'This version uses a separate Automation/Universe0131 save namespace. Older farms remain intact. In orbit view, scroll to zoom and drag with the right mouse button to rotate. Focus worker gives a close view of active craft animation.' if en else '本版使用独立Automation/Universe0131存档空间，旧农场保留。全景模式滚轮缩放、右键拖拽旋转；聚焦载具可近看扇翅、旋翼等动画。']
 for e in d['examples']:quick+=['### '+e['name']+'.py','```python\n'+e['code']+'\n```']
 content='\n\n'.join(quick)+'\n';(kit/('README'+suffix+'.md')).write_text(content);(root/('docs/UNIVERSE-0.13.2'+suffix+'.md')).write_text(content.replace('(README.md)','(UNIVERSE-0.13.2.md)').replace('(README.zh-CN.md)','(UNIVERSE-0.13.2.zh-CN.md)'))
manifest=dict(version='0.13.2',chapters=len(d['chapters']),functions=len(d['functions']),recipes=len(d['recipes']),programs={p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in sorted((kit/'programs').glob('*.py'))});(kit/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n');print('Universe manuals and 11 programs generated.')
