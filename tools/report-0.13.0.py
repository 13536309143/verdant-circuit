#!/usr/bin/env python3
from pathlib import Path
import json,shutil,hashlib
root=Path(__file__).resolve().parents[1];base=root.parent;out=root/'docs/reports/v0.13.0';out.mkdir(parents=True,exist_ok=True)
modeldir=root/'InternalTestReports/0.13.0'
model=json.loads((modeldir/'unified.json').read_text());assert model['status']=='passed'
counts={}
for name in ['classic','language','balance','progression','program-names']:
 d=json.loads((modeldir/(name+'.json')).read_text());counts[name]=d['passed']
ui=json.loads((base/'work/0130-ui/report.json').read_text());assert ui['status']=='passed' and ui['runtimeErrors']==0
earthdir=base/'work/0130-earth-certified';earth=json.loads((earthdir/'report.json').read_text());assert earth['status']=='passed' and earth['maxedResearchNodes']==earth['researchNodes'] and not earth['resourceGrants']
source=root/'Assets/VerdantCircuit/Scripts/Automation';fingerprint=hashlib.sha256('\n'.join(p.read_text() for p in sorted(source.glob('*.cs'))).encode()).hexdigest();assert earth['sourceFingerprint']==fingerprint
reference=json.loads((root/'docs/reference/world-0.13.0.json').read_text())
summary={'version':'0.13.0','publication':'local-only','sourceFingerprint':fingerprint,'referenceFingerprint':hashlib.sha256((root/'Assets/VerdantCircuit/Resources/UnifiedReference.json').read_bytes()).hexdigest(),'classicAssertions':sum(counts.values()),'unifiedAssertions':model['assertions'],'totalAssertions':sum(counts.values())+model['assertions'],'uiChecks':ui['checks'],'uiScreenshots':ui['screenshots'],'uiLocales':ui['locales'],'runtimeUiErrors':0,'chapters':len(reference['chapters']),'functions':len(reference['functions']),'recipes':len(reference['recipes']),'programFiles':len(reference['examples']),'earthResearchGameSeconds':earth['gameSeconds'],'earthPhases':len(earth['phases']),'researchNodes':earth['researchNodes'],'earthWallSeconds':earth['wallSeconds'],'newContractFixtureGameSeconds':model['newCampaignGameSeconds'],'newContractFixture':'Begins after original 18 contracts with all research and seeded original Earth inputs; no specialist product grants. This is an integration fixture, not a zero-inventory full-game timing.'}
(out/'summary.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2)+'\n')
for p in modeldir.glob('*.json'):shutil.copy2(p,out/p.name)
shutil.copy2(root/'InternalTestReports-localization.json',out/'localization.json')
(out/'earth').mkdir(exist_ok=True);shutil.copy2(earthdir/'report.json',out/'earth/report.json');shutil.copy2(earthdir/'final-farm.json',out/'earth/final-farm.json');shutil.copytree(earthdir/'programs',out/'earth/programs',dirs_exist_ok=True)
(out/'ui').mkdir(exist_ok=True)
for p in (base/'work/0130-ui').glob('*'):
 if p.suffix in ('.png','.json'):shutil.copy2(p,out/'ui'/p.name)
images=root/'docs/images'
for name in ['world-en','world-zh','program-center-en','program-center-zh','flowers-en','minerals-en','fish-en','reference-en','reference-zh']:
 shutil.copy2(base/'work/0130-ui'/(name+'.png'),images/(name+'-0.13.0.png'))
# Copy only public evidence. Native save backups and private preferences stay outside the repository.
(out/'builds').mkdir(exist_ok=True)
for source,target in [(base/'outputs/HeXu-0.13.0.app.build-report.json','macOS.json'),(base/'outputs/HeXu-Windows-x64-0.13.0/build-report.json','windows-x64.json'),(base/'work/0130-native-smoke/report.json','native-mac.json')]:
 if source.exists():shutil.copy2(source,out/'builds'/target)
def duration(seconds):
 seconds=round(seconds);return f'{seconds//3600}h {(seconds%3600)//60}m {seconds%60}s'
text=f'''# 0.13.0 地球与群岛统一流程验证报告

本次是本地测试版本，尚未上传GitHub。运行代码指纹：`{fingerprint}`。

- 原有语言、农业、平衡、科技、命名回归：**{sum(counts.values())}项**。
- 统一世界、分层程序、配方、存档与物流：**{model['assertions']}项**。
- 合计 **{summary['totalAssertions']}项断言**通过。
- 实际Play模式双语界面：**{ui['checks']}项检查、{ui['screenshots']}张截图、0运行错误**。
- 游戏内手册：**{len(reference['chapters'])}章、{len(reference['functions'])}个独立函数／方法条目**，每项包含参数、返回值、前置、边界与示例；另含28份实际配方和11个分层完整文件。

## 连续流程与计时

地球原有科研路线由一格、零库存、零赠送科技开始，通过真实解释器、生产与付费研究推进。完成 **{earth['researchNodes']}个科研节点**及全部等级，执行 **{len(earth['phases'])}个记录阶段**，保留16项本地竞速成绩与13项原有成就。父世界计时 **{duration(earth['gameSeconds'])}**；独立竞速有各自时钟，不加进父世界，不计人工编程和思考。算法详见[逐阶段报告](earth/report.json)与[实际执行程序](earth/programs)。

新增六份多目标订单由成熟边界测试起点跑通：已完成旧18单、已解锁科技，提供原有地球原料，未赠送任何新特色产物。其花卉、矿物、渔业、跨岛循环、母港回运全部使用真实配方、面积消耗、队列与航行，耗时 **{duration(model['newCampaignGameSeconds'])}**。这是集成测试，不是从零开局的完整游戏时长，不能与地球测试直接相加宣称玩家耗时。

0.12.1 的7小时参考结果属于旧版本。本版尚未测量一条从零库存贯通全部24单的统一玩家路线，也未承诺不同策略具有固定时长。

## 关键覆盖

1. 原有harvest在绑定地球农务机后，实际收获原有地块与库存。
2. Earth库存和0号仓库为同一份物资，真实预留、分舱运输、卸货一次、回运原库存。
3. 三座特色岛独立科技分支，未解锁岛拒绝生产与运输，不依赖连续岛号。
4. 总控→岛管理→不同单机文件的实际独立执行，父程序结束后子程序继续。
5. 单机绑定替换、组成员独立变量与收件箱、返回值复制、单机报错隔离与CPU公平性。
6. 20个专属函数逐一进入实体加工，每项在加工中保存／载入，检查大机体投入与输出守恒。
7. 旧地球与旧0号岛库存只合并一次，校验不会消耗临时仓库或重复货物。
8. 免费初始农务机拆解重建不会凭空刷资金；停止临时子机释放机位与机群成员关系。
9. 模拟实验入口、模拟科技与simulate移除；原有竞速保留并更新最快重置范例。
10. 中英文世界入口、两栏程序中心、三岛画面、函数搜索、配方说明和保存绑定实测。

## 界面证据

![地球入口](ui/world-zh.png)

![程序中心](ui/program-center-zh.png)

![函数手册](ui/reference-zh.png)

## 实际限制

调用栈、消息和返回结果不写入存档；保存实体任务、货物、文件绑定与机群成员。载入后重新运行相应总控，并检查现有队列。

Mac与Windows构建结果见builds目录。本轮未运行新版Mac独立应用：检测到已有0.12.1玩家会话，未打断或覆盖存档；新版进入地球和生产操作通过Unity Play模式验证。当前Mac包带Unity试用水印、未签名公证；Windows由Mac交叉构建，尚未Windows真机验收。
'''
(out/'README.md').write_text(text)
kit=root/'docs/test-kits/unified-world-0.13.0';shutil.copytree(out,kit/'validation',dirs_exist_ok=True)
print(json.dumps(summary,ensure_ascii=False,indent=2))
