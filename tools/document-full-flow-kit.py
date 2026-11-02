#!/usr/bin/env python3
"""Render the copyable player test kit, measured results and exact reference route."""
from pathlib import Path
import json, hashlib, argparse, zipfile
root=Path(__file__).resolve().parents[1]
kit=root/'docs/test-kits/full-flow-0.11.3'
parser=argparse.ArgumentParser();parser.add_argument('--report',type=Path);args=parser.parse_args()
report=json.loads(args.report.read_text()) if args.report else None
if report and report['status']!='passed':raise SystemExit('Only publish a passed validation report')
def clock(t):
    t=round(t);return f'{t//3600:02}:{t%3600//60:02}:{t%60:02}'
text='''# 禾序 0.11.3：完整流程测试代码与操作手册

适用：Verdant Circuit / 禾序 **0.11.3**（沿用 0.11.2 的三小时平衡规则）。这是在游戏内运行的类 Python 程序；不要放到 Unity 的 Assets 中，也不要用系统 Python 执行。

本文件包含全部 **25 个程序的完整代码**，覆盖零资源开局、34 项科技全部满级（120 个付费等级）、7 项机制验证及 16 类本地竞速。先完成手动开局，再运行自动控制器。所有资源正常生产、研究正常付费，保持“允许未解锁语法”关闭。

## 1. 准备与导入

1. 打开 0.11.3，在农场库新建一个测试农场，保留自己的原存档。应从 1×1、零库存、零研究开始。
2. 测平衡时使用设置中的 **1 倍世界速度**；快速检查可用游戏提供的更高倍率。科技中的执行速度升级仍正常生效。本文的时间是模拟游戏时间，人工读代码、编辑和点击的时间另外计算。
3. 推荐批量导入：设置 → 外部编辑器 → **导出并监视 / Export and watch**。找到设置中显示的当前农场程序文件夹，把压缩包 `programs` 里的全部 `.py` 文件复制进去，等日志出现“已重新载入外部代码”。不要替换存档；保留游戏生成的 `__builtins__.py`。
4. 导入后若仍停留在主编辑器，点一下别处，再打开程序库确认 `boot`、`farm_lib`、`campaign`、`races` 等文件都已出现。不要同时在游戏与外部编辑器修改同一个文件。
5. 也可逐个新建程序并粘贴本文代码。程序名称必须和小标题一致，**新建名称不填 `.py`**，缩进保留四个空格。

导入未解锁的代码本身没有影响；提前运行会提示需要研究。只运行步骤指定的程序。`farm_lib`、`experiment`、`race_*` 是其他程序调用的模块，不用单独点击运行。

每一步完成后先关闭任务完成提示，再启动下一步。主运行按钮会替换当前控制程序及其工作机；测试期间不要在浮动窗口另外启动生产。切换查看其他文件不会停止当前程序。开始下一项测试前，确认上一项已自然结束或点击“全部停止”。

## 2. 零资源开局：严格按表操作

下表的目标数是**当前库存**，不是累计产量。科技名称同时给出代码枚举，方便中英文界面对照。每一行研究都需要手动在科技树点击，先后顺序照表执行；不要提前购买额外升级。

| 步骤 | 运行代码 / 手动操作 | 完成条件与下一步 |
|---|---|---|
| 01 | 打开 `boot`，反复点击运行 | 草成熟后才能收获。攒到 **5 干草库存**，研究循环 `Unlocks.Loops`。未成熟时运行没有收获，等待后再点。 |
| 02 | 运行 `start` | 干草库存达到 **60** 后手动停止。 |
| 03 | 依次研究运算符 `Operators`、变量 `Variables`、传感器 `Senses`、种植 `Plant`、扩田 `Expand` 第1级 | 总支出48干草，农田变为 **2×2**。 |
| 04 | `hay` 第一行目标改成 `target = 50`，运行到自动结束 | 研究干草增产 `Grass` 第1级，支付50干草。 |
| 05 | `hay` 改为 `target = 200`，运行到自动结束 | 保留这些干草，先不买其他科技。 |
| 06 | `wood` 改为 `target = 30`，运行到自动结束 | 此时未解锁树，程序种免费灌木。累计干草已超过林木发现门槛。 |
| 07 | 依次研究林木 `Trees` 第1级、扩田 `Expand` 第2级、干草增产 `Grass` 第2级、执行速度 `Speed` 第1级 | 农田变为 **4×4**；干草支出24+145+20=189。 |
| 08 | `wood` 改为 `target = 900`，运行到自动结束 | 树与灌木棋盘种植，避免相邻树生长减速。 |
| 09 | 依次研究函数 `Functions`、列表 `Lists`、成本查询 `Costs`、计时 `Timing`、帽子 `Hats`、胡萝卜 `Carrots` 第1级、浇水 `Watering` 第1级 | 共245木材；此时累计木材已达到胡萝卜发现要求600。 |
| 10 | `hay` 改为 `target = 500`，运行到自动结束 | 准备胡萝卜种子原料。 |
| 11 | 运行 `carrot`，目标保持160 | 库存达到 **至少160胡萝卜** 后自动结束。每株消耗2干草+2木材；开始前应有至少400干草与400木材。 |
| 12 | 依次研究字典 `Dicts`、导入 `Import`、自动解锁 `Auto_Unlock`、调试 `Debug`、高级调试 `Debug_2`、实用工具 `Utilities` | 消耗140胡萝卜、5干草、160木材。开局脚本所需语言与观察工具齐全。 |
| 13 | 打开 `campaign`，点击运行 | `farm_lib` 必须已导入。接下来无需手动购买科技。 |

`hay`、`wood` 每次运行都会清田；只在指定阶段运行。`carrot` 会检查整轮种植预算，原料不足会结束：补足干草和木材后重跑本步即可，不必重开农场。

## 3. 全科技自动推进

运行 `campaign` 后，控制器会按正常科技条件生产、支付和升级，依次推进伴生、南瓜、肥料、向日葵、仙人掌、迷宫、长尾、多机、模拟、竞速、遗物与礼帽。无需人工轮换每种作物代码。

- 扩田遵循 **2×2 → 4×4 → 8×8 → 16×16 → 32×32**，达到对应作物发现后才购买。
- 首次发现需要累计产量：林木160干草、胡萝卜600木材、南瓜1500胡萝卜、向日葵8000南瓜、仙人掌6000能量、迷宫30000仙人掌、长尾30000金币。
- 每次生产先准备种子成本，草和木材可免费恢复；使用真实库存及 `get_cost()` 付费。水、肥料随已购研究自然补充。
- 控制台出现 `discovery` 表示完成一个发现阶段。运行中可能长时间没有新日志，观察累计产量、地块、无人机位置是否变化。
- 最终输出 **`campaign_complete True`** 与 **`all_research_maxed True`**，程序自然结束。科技树应有34项满级，合计120级；农田32×32，无人机容量32。

此控制器以单控制程序的串行生产为主，便于复现和排查。多机满负载另有专门验证。不同算法、随机种子、反复清田和人工停顿都会影响总时间；三小时是平衡基准路线的目标，不保证所有写法恰好用时三小时。

### 生产算法说明

| 系统 | 控制器采用的算法与观察点 |
|---|---|
| 干草 | 全田循环扫描，只收成熟草；草自然再生。 |
| 木材 | 树/灌木棋盘格，检查空格后再播种，防止树四邻减速。 |
| 胡萝卜 | 先按整轮播种计算干草和木材需求，再逐格收获补种。 |
| 南瓜 | 全田耕地，修补枯死南瓜，逐格催熟；全部成熟后合并收获。感染比例参与奇异物质产出。 |
| 向日葵 | 先测量全田花瓣，再优先收高花瓣，减少能量惩罚；能量会随指令执行消耗。 |
| 仙人掌 | 逐行、逐列比较和交换，建立东/北非递减排列，再触发连锁收获。 |
| 奇异物质 | 在有产量的作物上催熟感染，再收获；并非仅添加肥料就直接入库。 |
| 迷宫金币 | 批量备齐奇异物质，生成迷宫，用已访问集合和路径栈做深度优先搜索，遇死路回溯，找到宝藏再收获。 |
| 长尾骨头 | 偶数边长上的 Hamilton 回路：保留边缘通道，蛇形遍历，避免自身封路；消耗仙人掌生成苹果，最后换回草帽结算。 |
| 自动研究 | 递归补齐前置发现、累计产量、材料；扩田/执行速度/增产逐级检查作物发现。 |

## 4. 通关后的七项机制测试

等 `campaign` 完全结束后，按下列顺序运行。可单独重跑；这些测试会清田并消耗真实库存，建议先备份测试农场。

| 顺序 | 运行程序 | 预期结果 |
|---|---|---|
| 14 | `verify_companion` | `companion_ok True`；实际随机伴生搭配带来额外干草产量。 |
| 15 | `verify_maze` | `maze_reuse_ok True`；生成并复用迷宫，两次宝藏的金币增量符合当前等级。 |
| 16 | `verify_drones` | 瞬时达到32台活跃工作机；最后 `parallel_complete True capacity 32`，子机均已等待回收。显示本程序还剩主机1台是正常的。 |
| 17 | `verify_simulation` | 同种子7重复结果相同，种子8结果不同；`simulation_ok True`。需保留模块 `experiment`。 |
| 18 | `verify_tail` | `tail_ok True` 并打印 `bone_gain` 正值；实际走满长尾回路并结算骨头。 |
| 19 | `verify_hat` | `hat_ok True`；礼帽切换成功，然后换回草帽。 |
| 20 | `verify_debug` | `temporary_size_ok True` 与指令数；运行时临时2×2，**自然结束后恢复32×32**及正常执行限制。 |

## 5. 全部16类本地竞速

运行 `races`。保留所有9个 `race_*` 模块和 `experiment`，不单独启动它们。程序逐项输出 `challenge_start` / `challenge_passed`，最终输出 **`all_16_challenges_finished True`**。

| 资源 / 类型 | 竞速类别 | 被调用模块 |
|---|---|---|
| 干草 | Hay / Hay_Single | race_hay |
| 木材 | Wood / Wood_Single | race_wood |
| 胡萝卜 | Carrots / Carrots_Single | race_carrots |
| 南瓜 | Pumpkins / Pumpkins_Single | race_pumpkins |
| 向日葵 | Sunflowers / Sunflowers_Single | race_sunflowers |
| 仙人掌 | Cactus / Cactus_Single | race_cactus |
| 迷宫 | Maze / Maze_Single | race_maze |
| 长尾 | Dinosaur | race_dinosaur |
| 重置通关 | Fastest_Reset | race_reset |

竞速与模拟使用独立农场及独立计时，不会扣掉主农场库存。资源竞速按游戏预设的8×8中阶研究运行；单机类别限制1台。Fastest_Reset 从自己的零库存预设出发，允许预设语法，目标为获得遗物。`race_reset` 是竞速程序，**普通零解锁开局请使用前面的手动步骤**。

模拟/竞速调用最后的64是独立模拟加速参数，和主农场设置倍率是两件事。查看本地竞速列表：新测试农场应产生16类纪录。完成整套流程后，成就应为 **13/13**。

## 6. 异常排查与内部验收

- `Research needed`：运行阶段提前了，或步骤12尚未完成；查看报错对应的研究，不要开未解锁语法来掩盖漏项。
- `Module not found`：检查同一测试农场中是否有 `farm_lib`、`experiment` 和9个 `race_*` 程序，文件名不要加空格或改大小写。
- 点击运行无反应：先关闭完成提示、退出暂停/单步状态；确认点击的是目标程序的主运行按钮。
- 水满却不浇水：脚本仅在水分低于0.3且有水道具时浇水。水道具库存的自动补充与地块含水量不是同一数值。
- 数量下降：研究、播种、迷宫、苹果与能量执行都有真实消耗；查看累计生产而不只看当前库存。控制器可能先生产原料再生产目标资源。
- 正在运行却想看其他代码：可以切换查看；不要点另一个文件的运行按钮，否则会替换现有任务。
- 本套验证只覆盖当前实现：无人机共享农场与资源、变量独立；不包含尚未实现的不同尺寸实体机体、实体碰撞/扣款及浮空岛。请勿将这些旧设计需求当作当前版本已有功能。

内部测试记录建议：种子（若可见）、系统、游戏版本、世界倍率、每步开始/结束时钟、研究等级、预期与实际输出、异常截图或报错行号。流程为：零库存 → 正常开局 → 全科技 → 七机制 → 十六竞速 → 检查13成就。界面、音量、撤销、中文/数字程序名称等操作回归需要另外人工检查，自动脚本不能替代界面验收。

## 7. 本次实际验证记录

'''
if report:
    milestone=next(m for m in report['milestones'] if m['id']=='guide:all-research-maxed')
    text+=f'''验证状态：**通过**。种子 `{report['seed']}`，Unity `{report['unityVersion']}`，使用游戏真实解释器，固定每帧1/60秒；从零库存和零研究开始，未赠物资，未开放锁定语法。

- 全科技满级：主农场游戏时钟 **{clock(milestone['gameSeconds'])}**（{milestone['gameSeconds']:.3f} 秒）。
- 加上机制与竞速驱动后的主农场时钟：**{clock(report['gameSeconds'])}**。独立竞速耗时另列，不与主农场相加。
- 34/34科技满级，13/13成就，16/16竞速纪录；32台工作机峰值通过验证。
- 这里是实际逻辑运行时间；加速验证不渲染交互界面，不包含人工点击、编辑或阅读时间。

| 阶段 | 主农场开始 | 主农场结束 | 主农场耗时 |
|---|---:|---:|---:|
'''
    for p in report['phases']:
        text+=f"| {p['index']:02} · {p['title']} | {clock(p['gameStart'])} | {clock(p['gameEnd'])} | {clock(p['gameEnd']-p['gameStart'])} |\n"
    text+='\n独立竞速记录（与主农场时间分开）：\n\n| 类别 | 独立耗时 |\n|---|---:|\n'
    for m in report['milestones']:
        if m['id'].startswith('race:'):
            _,cat,secs=m['id'].split(':');text+=f'| {cat} | {float(secs):.3f} 秒 |\n'
    text+='\n原始记录：压缩包内 `validation/report.json`。逐阶段文件可用于对照预期，而非直接覆盖自己的存档。\n'
else:text+='生成时尚未附加验证报告，请以正式交付包中的记录为准。\n'
text+='''
## 8. 全部25个程序：可直接复制

下列每节对应一个独立游戏程序。不要把25段代码拼成一个文件。代码保留英文标识，适用于中英文界面。

'''
order=['boot','start','hay','wood','carrot','farm_lib','campaign','verify_companion','verify_maze','verify_drones','experiment','verify_simulation','verify_tail','verify_hat','verify_debug','race_hay','race_wood','race_carrots','race_pumpkins','race_sunflowers','race_cactus','race_maze','race_dinosaur','race_reset','races']
for name in order:text+=f'### {name}\n\n```python\n'+(kit/'programs'/f'{name}.py').read_text()+'```\n\n'
text+='''## 9. 382步参考回放

附带 `REFERENCE-382-STEPS.md`：已通过的种子731基准路线，每一步实际代码、购买研究与库存都有记录。该路线的完整通关主时钟约3小时。它和本手册的简化自动控制器采用不同的生产调度，不能把两条路线的中间库存混用。

基准回放中的某些生产步骤之前，由外部测试调度器先完成前置采购。参考文档按真实时间顺序逐项展开这些采购；正常手动测试优先照本手册的推荐步骤执行。
'''
(root/'docs/FULL-FLOW-TEST-0.11.3.md').write_text(text)
(kit/'README.zh-CN.md').write_text(text)
# Every original phase in order, including research purchases and their prerequisites.
reference=root/'docs/reports/v0.11.3/replay-731';raw=json.loads((reference/'report.json').read_text())
ref=f'''# 禾序 0.11.3：382步真实基准回放

种子731，{len(raw['phases'])}步，状态 `{raw['status']}`，总主农场游戏时钟 {clock(raw['gameSeconds'])}。

这是逐步骤参考记录。推荐使用同包 README 的分段开局与自动控制器，更适合人工测试。以下代码需逐步执行，购买步骤需在科技树点击，不是一个可拼接运行的程序。库存为该固定回放的结果，随机种子和人工等待会造成差异。普通模式保持未解锁语法关闭；竞速预设拥有独立的语法权限。

'''
for p in raw['phases']:
    ref+=f"## {p['index']:03} · {p['title']}\n\n类型：`{p['kind']}`；状态：`{p['status']}`；主农场 {clock(p['gameStart'])} → {clock(p['gameEnd'])}。\n\n{p['algorithm']}\n\n"
    if p.get('program'):ref+='```python\n'+(reference/p['program']).read_text().rstrip()+'\n```\n\n'
    else:ref+='在科技树完成本步骤标题对应的购买。\n\n'
    ref+='结束库存：'+ '，'.join(f"{i['id']}={i['amount']:.3f}" for i in p['after'])+'。\n\n'
    ref+='结束研究：`'+', '.join(p['research'])+'`。\n\n'
(kit/'REFERENCE-382-STEPS.md').write_text(ref)
manifest=json.loads((kit/'manifest.json').read_text())
manifest['sha256']={p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in sorted((kit/'programs').glob('*.py'))}
if report:
    (kit/'validation').mkdir(exist_ok=True)
    (kit/'validation/report.json').write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n')
    manifest['validation']={'status':report['status'],'seed':report['seed'],'allResearchGameSeconds':milestone['gameSeconds'],'finalGameSeconds':report['gameSeconds'],'runtimeSourceFingerprint':report['sourceFingerprint']}
(kit/'manifest.json').write_text(json.dumps(manifest,ensure_ascii=False,indent=2)+'\n')
outputs=root.parent/'outputs';outputs.mkdir(exist_ok=True)
(outputs/'禾序-0.11.3-完整流程测试.md').write_text(text)
with zipfile.ZipFile(outputs/'HeXu-FullFlow-TestKit-0.11.3.zip','w',zipfile.ZIP_DEFLATED) as z:
    for p in sorted(kit.rglob('*')):
        if p.is_file():z.write(p,Path('HeXu-FullFlow-TestKit-0.11.3')/p.relative_to(kit))
print('Documented',len(order),'programs;',len(raw['phases']),'reference steps; ZIP created in',outputs)
