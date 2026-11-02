#!/usr/bin/env python3
"""Publish the local 0.12.1 validation evidence, only for the exact tested runtime."""
from pathlib import Path
import json,hashlib,shutil,re,csv
root=Path(__file__).resolve().parents[1];work=root.parent/'work';out=root/'docs/reports/v0.12.1';out.mkdir(parents=True,exist_ok=True)
source=root/'Assets/VerdantCircuit/Scripts/Automation'
fingerprint=hashlib.sha256('\n'.join(p.read_text() for p in sorted(source.glob('*.cs'))).encode()).hexdigest()
quoted=r'"(?:[^"\\]|\\.)*"';definitions=(source/'ArchipelagoData.cs').read_text()
contracts=[]
pattern=r'new FleetContract\(('+quoted+r'),('+quoted+r'),('+quoted+r'),('+quoted+r'),(\d+),([^\n]+)'
for m in re.finditer(pattern,definitions):
    en,zh,lesson_en,lesson_zh=[json.loads(v) for v in m.groups()[:4]]
    goals=[dict(item=item,island=int(island),amount=int(amount)) for item,island,amount in re.findall(r'G\("(\w+)",(\d+),(\d+)\)',m[6])]
    contracts.append(dict(en=en,zh=zh,lesson_en=lesson_en,lesson_zh=lesson_zh,reward=int(m[5]),goals=goals))
assert len(contracts)==18
rewards=sum(c['reward'] for c in contracts)
def amount(stocks,item):return sum(i['amount'] for i in stocks if i['id']==item)
def hms(seconds):
    t=round(seconds);return f'{t//3600:02}:{t%3600//60:02}:{t%60:02}'
runs=[]
for seed in [731,17,20261102]:
    folder=work/f'0121-certified-{seed}';report=json.loads((folder/'report.json').read_text());farm=json.loads((folder/'final-farm.json').read_text());fleet=farm['archipelago']
    assert report['status']=='passed' and report['sourceFingerprint']==fingerprint,(seed,report['status'],report['sourceFingerprint'],fingerprint)
    assert report['freshZeroInventory'] and not report['unrestrictedSyntax'] and not report['resourceGrants'] and report['usesActualInterpreter']
    assert report['maxedResearchNodes']==34 and report['achievements']==13 and len(farm['records'])==16
    assert sum(i['amount'] for i in farm['unlocks'])==120
    assert fleet['order']==18 and fleet['sizeLevel']==4 and fleet['slotLevel']==5 and len(fleet['units'])==8
    assert len(set(fleet['usedRoles']))==6 and fleet['programFaults']==0 and fleet['nearMisses']==0 and fleet['peakPrograms']>=3
    assert abs(fleet['credits']-(100+rewards+fleet['refunds']+fleet['saleIncome']-fleet['buildSpent']-fleet['researchSpent']-fleet['fees']))<.0001
    ledger=[]
    for item in ['water','grain','ore','alloy','crystal','medicine','core']:
        made=sum(amount(i['made'],item) for i in fleet['islands']);stock=sum(amount(i['items'],item) for i in fleet['islands'])
        consumed=amount(fleet['consumed'],item);submitted=amount(fleet['submitted'],item);exported=amount(fleet['exported'],item)
        held=sum(j['reserved'] for u in fleet['units'] for j in u['jobs'] if j['kind']=='ship' and j['item']==item);cargo=sum(u['cargo'] for u in fleet['units'] if u['cargoItem']==item)
        balance=made-consumed-submitted-exported-stock-held-cargo;assert abs(balance)<.0001,(seed,item,balance)
        ledger.append(dict(item=item,made=made,consumed=consumed,submitted=submitted,exported=exported,stock=stock,reserved=held,cargo=cargo,balance=balance))
    campaign=next(m['gameSeconds'] for m in report['milestones'] if m['id']=='complete:0.12.1-campaign');research=next(m['gameSeconds'] for m in report['milestones'] if m['id']=='guide:all-research-maxed')
    runs.append(dict(seed=seed,campaignSeconds=campaign,researchSeconds=research,islandOrderSeconds=sum(fleet['orderTimes']),finalSeconds=report['gameSeconds'],wallSeconds=report['wallSeconds'],ledger=ledger,fleet=fleet))
    target=out/f'seed-{seed}';target.mkdir(exist_ok=True)
    for name in ['report.json','final-farm.json']:shutil.copy2(folder/name,target/name)
    shutil.copytree(folder/'programs',target/'programs',dirs_exist_ok=True)
    (target/'cargo-ledger.json').write_text(json.dumps(ledger,indent=2)+'\n')
# Copy actual suite results, logs and screenshots.
suite=root/'InternalTestReports/0.12.1';(out/'suites').mkdir(exist_ok=True)
for p in suite.glob('*.json'):shutil.copy2(p,out/'suites'/p.name)
for name in ['0121-final-verify.log','0121-final-opening.log','0121-final-fleet-ui.log','0121-final-base-ui.log']:
    if (work/name).exists():
        lines=(work/name).read_text(errors='replace').splitlines()
        lines=[line for line in lines if not any(key in line.lower() for key in ['licensing','entitlement','license system','serial number','access token'])]
        (out/'suites'/name).write_text('\n'.join(lines).replace(str(root),'<project>').replace('/Users/nice/','<user>/')+'\n')
for name,destination in [('0121-final-opening','opening'),('0121-final-fleet-ui','fleet-ui')]:
    shutil.copytree(work/name,out/destination,dirs_exist_ok=True)
localization=json.loads((root/'InternalTestReports-localization.json').read_text());assert localization['gaps']==0
(out/'suites/localization.json').write_text(json.dumps(localization,indent=2)+'\n')
images=root/'docs/images';images.mkdir(exist_ok=True)
for original,name in [(work/'0121-final-opening/03-title-en.png','opening-en-0.12.1.png'),(work/'0121-final-opening/04-title-zh.png','opening-zh-0.12.1.png'),(work/'0121-final-fleet-ui/island-operations-en.png','archipelago-en-0.12.1.png'),(work/'0121-final-fleet-ui/island-operations-zh.png','archipelago-zh-0.12.1.png')]:shutil.copy2(original,images/name)
with (out/'order-times.csv').open('w') as f:
    writer=csv.writer(f);writer.writerow(['order','title_en','title_zh']+[str(r['seed']) for r in runs])
    for i,c in enumerate(contracts):writer.writerow([i+1,c['en'],c['zh']]+[r['fleet']['orderTimes'][i] for r in runs])
rows='\n'.join(f"| {r['seed']} | {hms(r['researchSeconds'])} | {hms(r['islandOrderSeconds'])} | **{hms(r['campaignSeconds'])}** | {hms(r['finalSeconds'])} | {r['wallSeconds']:.2f}s |" for r in runs)
checks=sum(json.loads((out/'suites'/name).read_text())['passed'] for name in ['classic.json','language.json','balance.json','progression.json','program-names.json'])
physical=int(re.search(r'FLEET CORE PASSED / (\d+)',(work/'0121-final-verify.log').read_text())[1]);scenarios=json.loads((out/'suites/fleet-scenarios.json').read_text())['checks']
opening=json.loads((out/'opening/report.json').read_text());ui=json.loads((out/'fleet-ui/report.json').read_text());play=json.loads((out/'suites/playmode.json').read_text())
text=f'''# 0.12.1 · Full campaign validation / 完整验证报告

[English operations guide](../../ARCHIPELAGO-0.12.1.md) · [完整中文代码与测试步骤](../../ARCHIPELAGO-0.12.1.zh-CN.md)

**Three fresh zero-inventory runs passed on the exact runtime below.** Each run completed the 34-node / 120-level farming route, 12 additional fleet research levels, all 18 island contracts, all six specialist roles, a fleet of eight, 3×3 hulls, seven distinct concurrent specialist programs, 13 farming achievements and 16 local race categories. No unrestricted syntax or test resource grants were used in these campaign runs. Fixture-based unit/UI tests are separate.

**三个种子均从零资源、零研究正常推进。** 原料正常生产、研究与建造正常付费，没有按秒数放行的认证。表中是1倍模拟游戏时间，保留正常科技执行加速；人工编辑、思考与点击另计，独立竞速的计时不并入主农场。

| Seed | Farming research | Island orders | Main campaign complete | Including base verifiers | Accelerated calculation |
|---|---:|---:|---:|---:|---:|
{rows}

The reference route completes farming research first, then island logistics. Players can overlap them, use other hull mixes, or improve the dispatch algorithm; these measurements are not an enforced minimum or a speedrun record. 世界倍率提高会缩短真实等待；高效并行也可更快。时长来自不同生产链、通行限制、独立程序和多目标交付，而非固定等待门槛。

## Coverage / 覆盖范围

- {checks} retained farming/language/balance/progression/naming assertions, {physical} physical-fleet assertions and {scenarios} fleet scenario assertions: **{checks+physical+scenarios} total**, including repeated conservation checks throughout simulated intervals.
- {opening['checks']} opening checks, five real rendered captures, English/Chinese and compact aspect ratio; pointer skip, same-frame input suppression, replay, automatic finish and original pause/state restoration.
- {ui['checks']} fleet UI checks with both locales and real independent worker execution. Live imported libraries appear active and cannot be deleted. UI fixtures use prepared research to exercise the interface; they are not campaign timing evidence.
- Retained real UI regression: {play['frames']} advancing frames, {play['runtimeErrors']} runtime errors; first harvest, program naming, external-file import/export, profiles, floating editors, tree navigation, completion notices, program cards and volume slider.
- Localization: {localization['checkedEntries']} locale entries checked, {localization['gaps']} gaps. The authored manual contains 19 chapters and 52 API groups.
- Mid-recipe and mid-flight save/reload match uninterrupted execution; cancellation/refunds and stop-all reconcile material totals. Corrupt hulls and non-finite inventories are rejected. Older farms with no island state remain loadable.
- Heavy user loops cannot freeze simulation or invalidate a later wait; self-dispatch cannot let the old task erase the new task. One worker exception leaves peers running. Messages and returned values are copied. The autonomous example's finite master ends while four island programs continue producing alloy.
- Repair-fee bankruptcy can recover by producing and exporting real goods; unavailable, reserved and negative-quantity exports are rejected. Exported goods and income enter their respective ledgers.

## Operations and algorithms / 操作与算法

1. Follow the paid manual opening: harvest five hay, buy Loops, accumulate inventory and buy language/sensor/planting tools. Expand2×2, discover trees, expand4×4, produce wood and carrots, buy dictionary/import/automatic research tools.
2. Execute the delivered farm library: checkerboard trees, budgeted carrot seeds, pumpkin repair/merge, priority sunflower harvest, two-axis cactus sorting, infected-material production, DFS maze search and Hamiltonian tail coverage. The crop-gated expansion/speed/yield economy remains intact.
3. Produce charter materials and pay100Wood+100Carrots. Buy fleet count, hull and role upgrades separately using contract credits.
4. `island_controller` calls `island_planner`. Reverse-topological recipe planning reserves goal stock and considers both incoming cargo and in-progress area-sized batches. Messages go to independent water/grain/foundry/crystal/medicine/archive/cargo programs, all importing a shared worker library.
5. Route complete occupied rectangles through each20×14island grid. Broad lanes form a directed circulation loop; small hulls have a shortcut. Atomic occupancy, docking pad anchors and landing queues prevent overlap. Cargo is reserved at acceptance, loaded physically, flown through connected islands and unloaded once.
6. Deliver all goals together. Later orders also check actual hull use, distinct program concurrency and simultaneous flights. Run base mechanism verifiers and16local races after the main campaign.

## Per-order time / 各单耗时

| # | Order / 订单 | Seed731 | Seed17 | Seed20261102 |
|---|---|---:|---:|---:|
'''
for i,c in enumerate(contracts):text+=f"| {i+1:02} | {c['en']} / {c['zh']} | "+' | '.join(hms(r['fleet']['orderTimes'][i]) for r in runs)+' |\n'
text+='\n## Reconciliation / 对账\n\nAll seven cargo balances are zero in every seed:\n\n`produced − recipe inputs − submitted − exported − depot stock − reserved manifests − carried cargo = 0`\n\nCredit accounts also reconcile:\n\n`100 charter credits + contract rewards + hull refunds + exports − hull purchases − research − emergency fees = current credits`\n\n'
r=runs[0];text+='| Cargo | Produced | Consumed | Submitted | Exported | Depot + manifest + cargo | Difference |\n|---|---:|---:|---:|---:|---:|---:|\n'
for l in r['ledger']:text+=f"| {l['item']} | {l['made']:.0f} | {l['consumed']:.0f} | {l['submitted']:.0f} | {l['exported']:.0f} | {l['stock']+l['reserved']+l['cargo']:.0f} | {l['balance']:.0f} |\n"
text+=f'''\nReference seed731 credit ledger:100 + {rewards} + {r['fleet']['refunds']:.0f} + {r['fleet']['saleIncome']:.0f} − {r['fleet']['buildSpent']:.0f} − {r['fleet']['researchSpent']:.0f} − {r['fleet']['fees']:.0f} = **{r['fleet']['credits']:.0f}**. The completion route pays no emergency fees and needs no exports; recovery exports are exercised by separate scenarios.\n
## Reproduce / 复现

Use Unity2022.3.62f3c1. `VerdantRelease.Verify` runs model/interpreter/scenario assertions. `VerdantOpeningChecks.Run`, `VerdantFleetUiChecks.Run` and `VerdantAutomationPlayChecks.Run` require `VERDANT_AUTOMATION_QA=1` and use isolated editor profiles. Run them without `-quit`; the checks exit on completion.

For the full campaign, call `VerdantFullPlaythrough.RunPlayerGuide` without `-quit`, with `VERDANT_FULL_0121=1`, `VERDANT_PLAYTHROUGH_SEED=<seed>` and an absolute `VERDANT_PLAYTHROUGH_OUTPUT` directory. It loads the exact delivered sources and uses the game's normal interpreter/physics; it does not emulate the solution in Python.

Automation runtime source SHA-256: `{fingerprint}`. Each seed folder contains the report, final farm with all module sources, executed phase snippets and material reconciliation. These are generated test saves, not personal player saves.

## Scope and platform limits / 范围与平台限制

Island sites share an island-local depot. Air channels are abstract flight segments; footprint conflicts are resolved on land and during landing. This version does not simulate detailed mid-air aerodynamics. Python call stacks, inboxes and result objects are transient; physical queues and cargo persist. Restart the coordinator after loading.

Build reports are included when available. Windows packages are cross-built on macOS; native Windows execution requires separate hardware and is not claimed here. Automated coverage is evidence for these routes and cases, not a guarantee for every player algorithm, hardware setup or long-running save.

![Opening](../../images/opening-en-0.12.1.png)

![Island operations UI fixture](../../images/archipelago-en-0.12.1.png)
'''
(out/'README.md').write_text(text)
summary=dict(version='0.12.1',status='passed',sourceFingerprint=fingerprint,modelAssertions=checks+physical+scenarios,openingChecks=opening['checks'],fleetUiChecks=ui['checks'],legacyUiFrames=play['frames'],runtimeErrors=0,runs=[{k:v for k,v in r.items() if k not in ['fleet','ledger']} for r in runs])
(out/'summary.json').write_text(json.dumps(summary,indent=2)+'\n')
kit=root/'docs/test-kits/full-flow-0.12.1';(kit/'validation').mkdir(exist_ok=True)
shutil.copy2(out/'summary.json',kit/'validation/summary.json');shutil.copy2(out/'README.md',kit/'validation/REPORT.md')
for seed in [731,17,20261102]:shutil.copy2(out/f'seed-{seed}/report.json',kit/f'validation/seed-{seed}.json')
print('Validated exact runtime',fingerprint,'and all three runs; wrote report and kit evidence.')
