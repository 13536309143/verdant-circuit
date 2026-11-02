#!/usr/bin/env python3
from pathlib import Path
import json,shutil,hashlib
root=Path(__file__).resolve().parents[1];base=root.parent;out=root/'docs/reports/v0.13.1';out.mkdir(parents=True,exist_ok=True)
modeldir=root/'InternalTestReports/0.13.1';model=json.loads((modeldir/'universe.json').read_text());campaign=json.loads((modeldir/'campaign.json').read_text());ui=json.loads((base/'work/0131-ui/report.json').read_text());native=json.loads((base/'work/0131-native-smoke/report.json').read_text())
for r in [model,campaign,ui,native]:assert r['status']=='passed'
source=root/'Assets/VerdantCircuit/Scripts/Automation';fingerprint=hashlib.sha256('\n'.join(p.read_text() for p in sorted(source.glob('*.cs'))).encode()).hexdigest();reference=json.loads((root/'docs/reference/world-0.13.1.json').read_text())
summary=dict(version='0.13.1',publication='local-only',sourceFingerprint=fingerprint,referenceFingerprint=hashlib.sha256((root/'Assets/VerdantCircuit/Resources/UnifiedReference.json').read_bytes()).hexdigest(),shaderFingerprint=hashlib.sha256((root/'Assets/VerdantCircuit/Resources/PlanetSurface.shader').read_bytes()).hexdigest(),modelChecks=model['checks'],campaignChecks=campaign['checks'],totalAssertions=model['checks']+campaign['checks'],uiChecks=ui['checks'],uiScreenshots=ui['screenshots'],uiLocales=ui['locales'],runtimeUiErrors=ui['runtimeErrors'],nativeMac=native['status'],nativeMacScope=native['scope'],windowsNative='not tested',chapters=len(reference['chapters']),functions=len(reference['functions']),recipes=len(reference['recipes']),programFiles=len(reference['examples']),contractFixture=campaign)
(out/'summary.json').write_text(json.dumps(summary,ensure_ascii=False,indent=2)+'\n')
for p in modeldir.glob('*.json'):shutil.copy2(p,out/p.name)
for group,src in [('ui',base/'work/0131-ui'),('native-mac',base/'work/0131-native-smoke')]:
 (out/group).mkdir(exist_ok=True)
 for p in src.iterdir():
  if p.is_file() and p.suffix in ('.png','.json'):shutil.copy2(p,out/group/p.name)
images=root/'docs/images'
for name in ['universe-en','earth-en','flowers-en','bee-close','ocean-en','minerals-en','programs-en','reference-en','reference-zh','earth-full-belt-zh']:
 shutil.copy2(base/'work/0131-ui'/(name+'.png'),images/(name+'-0.13.1.png'))
(out/'builds').mkdir(exist_ok=True)
for source,name in [(base/'outputs/HeXu-0.13.1.app.build-report.json','macOS.json'),(base/'outputs/HeXu-Windows-x64-0.13.1/build-report.json','windows-x64.json')]:
 report=json.loads(source.read_text());assert report['result']=='succeeded' and report['errors']==0 and report['warnings']==0;shutil.copy2(source,out/'builds'/name)
text=f'''# 禾序 0.13.1 四星宇宙验证报告

本次仅在本地开发与打包，未上传GitHub。运行代码指纹：`{fingerprint}`。完整元数据及原始报告见同目录JSON。

## 结论与范围

- 模型、函数、存档和边界验证：**{model['checks']}项**。
- 八份宇宙订单连续集成流程：**{campaign['checks']}项**。
- 合计 **{summary['totalAssertions']}项断言**通过。
- Unity实际Play模式：**{ui['checks']}项检查、{ui['locales']}种语言、{ui['screenshots']}张截图、0运行错误**。
- Mac独立应用启动、开场动画和四星宇宙画面通过；测试前后校验并恢复本版独立存档空间，未停止已有旧版游戏进程。
- Mac通用架构与Windows x64 Mono构建均为**0错误、0警告**；Windows尚未真机运行。
- 交付17章中英文手册、215个独立函数／方法条目、31份生产配方、11个可直接创建的分层程序文件。

## 连续订单与计时

八份订单逐单使用实际资源扣除、生产时间、载具路径、UFO燃料及到货逻辑，累计 **67个实际运输载荷**，测试模型计时 **7412秒（2小时03分32秒）**。各单耗时：{', '.join(str(x) for x in campaign['orderSeconds'])}秒。

**此测试从已解锁科研、已展开的8×8星球和提供原有地球原料的集成起点开始，不赠送特色加工产物。** 时间不含初始基础设施准备、人工编程与思考，不是从一格零库存开局的完整玩家耗时，也不能与历史版本时间相加。当前未认证从零开局的五小时完整路线。详细步骤和最终实体状态见[campaign.json](campaign.json)与[campaign-final.json](campaign-final.json)。

另有独立用例验证一格、零库存、只解锁地球的开局及绑定无人机实际收获原有草地。

## 地图、生产与程序覆盖

1. 四星编号固定，未解锁星球不能生产或发货；地球保留原作物、科技、南瓜、仙人掌、迷宫和恐龙尾迹等原有机制。
2. 表层逐级从1×1展开至360×128。全尺寸网格真实构建184320个顶点，边界为南北纬64度；只有360列时经度首尾相接，纬度始终有界。
3. 扩展保留已有作物、库存、工人和路径；矩形地块、边缘与经度接缝有专门验证。
4. 本地drone、bee、boat、mining_drone不能跨星变种；每星球独立解锁数量和机体尺寸，机群不能混入另一星球成员。
5. 路径校验整个机体的占地和占用队列。采矿与捕捞寻找实际矿脉、鱼群；31份配方逐项核验真实投入和产出。
6. UFO接受任务时完整预留货物和所需电量；多舱运输、空船调位、到港补能、缺燃料拒绝、取消、中途存档与资源守恒有实测。
7. 宇宙→星球→集群→单机独立执行、文件绑定、消息复制、程序报错隔离、暂停和单步均有覆盖。
8. coins、gold_ore与gold严格区分；整数接口拒绝小数、非有限数、字符串和布尔值，避免静默截断。
9. Python风格数值、序列及字符串新函数有解释器用例；所有公开接口逐项匹配独立手册条目。
10. 旧运输捷径不再公开；跨星移动仅由UFO负责。兼容调用在手册中明确标注。

## 界面与动画证据

顶栏导航、中英文切换、星球色系、程序目标绑定、UFO管理、函数精确搜索和滚动面板均在Play模式检查。蜂翅和旋翼在连续帧间实际改变角度。

![四星宇宙](ui/universe-en.png)

![花卉星球的蜜蜂近景](ui/bee-close.png)

![完整球面带](ui/earth-full-belt-zh.png)

![中文函数手册](ui/reference-zh.png)

## 存档及实际限制

本版使用独立的`Automation/Universe0131`目录，旧农场保留。旧存档不转换为新宇宙；新建农场仍从一格开始。实体任务、货物、机群和文件绑定保存；解释器调用栈、消息和返回结果不保存，载入后需重新运行总控并检查现有任务。

11个示例用于展示分层控制，不赠送研究与物资，也不会自动完成全部订单。自建解释器仅支持手册列出的Python风格子集，不支持标准库、操作系统和关键字参数。

Mac独立包此次仅验收自动开场及宇宙首页，详细玩法由Unity Play模式和模型流程覆盖。Mac包未签名公证并带当前Unity试用水印。Windows为Mac交叉构建，仍需Windows真机验收。当前美术使用程序生成的简化模型和动画。
'''
(out/'README.md').write_text(text)
shutil.copytree(out,root/'docs/test-kits/universe-0.13.1/validation',dirs_exist_ok=True)
print(json.dumps(summary,ensure_ascii=False,indent=2))
