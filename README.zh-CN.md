# 禾序 · Verdant Circuit

**从一格农田开始，用代码经营一个小宇宙。**

[English](README.md) · **简体中文** · [下载 v0.13.2](https://github.com/13536309143/verdant-circuit/releases/tag/v0.13.2) · [完整手册](docs/MANUAL.zh-CN.md)

禾序是一款用Unity独立开发的单人编程与生产游戏。从第一格草地起步，把球面农场逐步自动化，再通过轨道母舰连接花卉、海洋和矿物殖民地。你可以为单体载具、集群、星球管理和宇宙总控分别编写类Python程序。

| 当前版本 | 运行平台 | 语言 | 开发环境 |
|---|---|---|---|
| **0.13.2 · 轨道母舰**，公开测试版，已包含Night主题补丁 | macOS Intel／Apple Silicon · Windows x64 | 默认英文，可选简体中文 | Unity **2022.3.62f3c1** |

![地球、三颗未开放星球与轨道母舰](docs/images/universe-en-0.13.2.png)

## 下载与启动

在[0.13.2发行页](https://github.com/13536309143/verdant-circuit/releases/tag/v0.13.2)下载适合的平台完整压缩包。

| 下载 | 包含内容 | 启动方式 |
|---|---|---|
| [Mac版](https://github.com/13536309143/verdant-circuit/releases/download/v0.13.2/HeXu-macOS-0.13.2.zip) | Apple Silicon与Intel通用应用 | 解压后打开`HeXu-0.13.2.app` |
| [Windows x64版](https://github.com/13536309143/verdant-circuit/releases/download/v0.13.2/HeXu-Windows-x64-0.13.2.zip) | 程序、数据及Mono运行库 | **解压整个文件夹**后运行`HeXu.exe` |
| [Unity源码](https://github.com/13536309143/verdant-circuit/releases/download/v0.13.2/HeXu-Source-0.13.2.zip) | 工程、双语文档及构建工具 | 用对应版本Unity打开 |
| [程序示例包](https://github.com/13536309143/verdant-circuit/releases/download/v0.13.2/HeXu-Program-Kit-0.13.2.zip) | 11个完整文件、双语手册及验证记录 | 按包内中文版README操作 |
| [验证资料](https://github.com/13536309143/verdant-circuit/releases/download/v0.13.2/HeXu-TestEvidence-0.13.2.zip) | 规则、界面、计时、构建与Night补丁报告 | 查看包内README |

可用[SHA256校验文件](https://github.com/13536309143/verdant-circuit/releases/download/v0.13.2/SHA256SUMS-0.13.2.txt)核对下载完整性。[GitHub历史发行](https://github.com/13536309143/verdant-circuit/releases)继续保留。

运行游戏无需安装Unity。Mac包未签名公证，带当前Unity许可的试用水印；Windows由Mac交叉构建，仍需Windows真机验收。本版不提供Linux、浏览器或Steam发行包。

## 第一次收获

1. 在宇宙首页选择**地球**，穿梭机从母舰降落；初始只有地球开放。
2. 等待第一格草成熟，画面上的生长进度达到100%。
3. 运行初始的`harvest()`程序，等待草重新成熟后重复收获；积累五份干草，进入科技研究**循环**。
4. 使用下方循环持续收获，再按物资条件研究种植、传感器、变量及逐级扩田。
5. 在**程序**中新建文件，绑定单体、星球或宇宙目标并运行。切换查看其他文件不会停止原有工作。

```python
# 先解锁循环，绑定到地球无人机。
while True:
    harvest()
```

顶栏打开宇宙、殖民地、科技、程序、手册、UFO、纪录及设置。**F8**切换语言，**Ctrl/Cmd+Enter**运行，**Ctrl/Cmd+Z**撤销。滚轮缩放球面，右键拖动旋转，点击**聚焦载具**查看近景。界面固定使用**Night**，各星球保留独立强调色。

[操作指南](docs/UNIVERSE-0.13.2.zh-CN.md)包含发展步骤与11文件控制范例。示例需要真实解锁及补给，不会自动解决整条主线。

## 四颗星球，一套供应网络

| 编号 | 星球／仓库 | 本地载具 | 主要生产 |
|---|---|---|---|
| `0` | 地球 | 无人机 | 原有作物、水、堆肥、鱼饵及太阳能 |
| `1` | 芳华·花卉星球 | 小蜜蜂 | 六种花卉及蜂蜜 |
| `2` | 沧澜·海洋星球 | 渔船 | 六种鱼类、对应鱼群及鱼粉 |
| `3` | 鎏金·矿物星球 | 采矿无人机 | 原矿开采、冶炼、精炼及钻石切割 |
| `-1` | 轨道母舰 | UFO停靠及仓储 | 独立库存与星际中转 |

地球保留巨型南瓜、仙人掌排序、伴生种植、迷宫和长尾探索。后期殖民地属于同一发展过程，**24个联合生产项目、31份配方、13项专属科技**把各星球连接起来。

- **球面农场：**通过付费研究从`1×1`扩到`360×128`，最多46080个一度地块。纬度覆盖南北各64°，保留两极；横向360列全开才环绕，南北始终有界。扩地保留作物，旧地块居中，局部坐标会重新编号。
- **本地集群：**各星球分别解锁数量与`1×1、2×1、2×2、3×2、3×3`机体。投入和产出按面积变化，路径检查完整占地。大型机体需要预留通行空间。
- **真实物流：**本地载具留在本星球。UFO预留货物、消耗电量、经母舰航行，到港后才入库。补能消耗当前泊位太阳能；取消时退回未装船预留，在途货物完成当前航段，可能进入母舰仓库。
- **分区科技：**宇宙分区负责星球解锁与轨道科研，各星球有独立研究。生产、自动化、生态和运输采用不同颜色，已研究节点变亮。
- **可见的工作：**连续生长的作物、收获碎光、星球自转、大气、蜂翅、旋翼、船桨与钻头；通过可见范围筛选、批量绘制和特效池控制渲染负载。

`coins`表示宝藏金币，`gold_ore`是金矿石，`gold`是黄金锭。原矿和精炼产物分别使用不同的函数及库存编号。

## 用程序组织协作

```text
宇宙总控
├── 地球管理 → 无人机程序
├── 花卉管理 → 蜜蜂集群
├── 海洋管理 → 渔船船队
└── 矿物管理 → 采矿无人机集群
              ↕ UFO经过母舰运输
```

每个单体或管理目标同一时间运行一个解释器。集群成员运行同一文件的独立副本，通过模块、消息副本及返回值协作。父程序结束后，子程序仍可继续；停止代码也不一定取消实体生产队列，需按手册调用取消接口。

研究星际航行并准备UFO建造物资后，可把一舱水送入母舰：

```python
ship = ufo_build()
if ship > 0 and planet_inventory(0, "water") >= 24:
    if ufo_transfer(ship, 0, -1, "water", 24):
        ufo_wait(ship)
        print(mothership_inventory("water"))
```

解释器是**类Python子集**，支持函数、模块、集合及手册中的数值／字符串工具，不提供Python标准库、操作系统访问或关键字参数。保留暂停、单步、断点、变量查看、撤销重做及外部文件编辑。

## 手册与验证范围

| 文档 | 内容 |
|---|---|
| [完整中文手册](docs/MANUAL.zh-CN.md) · [English manual](docs/MANUAL.md) | 19章、218个函数／方法，参数、返回值、失败条件、配方与项目 |
| [中文操作与代码](docs/UNIVERSE-0.13.2.zh-CN.md) · [English walkthrough](docs/UNIVERSE-0.13.2.md) | 发展步骤及11个完整分层文件 |
| [测试指南](docs/TESTING.md) | 可复现的Editor命令及人工验收步骤 |
| [主线与渲染验证](docs/reports/v0.13.2/README.md) | 452项规则断言、64项界面检查、从零24项目流程及压力测量 |
| [Night补丁验证](docs/reports/v0.13.2-night/README.md) | 66项界面检查、旧主题兼容及双平台构建报告 |

参考路线从**一格、零库存、零科研**开始，使用每星球一台付费`2×2`载具与报告中的调度策略，完成24个主线项目约需**38小时15分游戏时间**。更多载具和更好的并行算法可显著缩短时间；这**不是所有玩家的最低时长，也不是最快通关纪录**，未计人工编程时间及可选收集目标。Night补丁只调整界面，报告分别记录对应代码版本。

## 存档与升级

先保存并退出旧游戏，再打开新版。0.13.2沿用0.13.1的宇宙进度，位于`Application.persistentDataPath/Automation/Universe0131`，更早农场文件独立保留。设置中可查看实际存档路径，旧主题偏好自动忽略。

保存星球、库存、载具、电量、货物、实体队列、集群和文件绑定；解释器调用栈、收件箱与返回对象为临时状态。读档后先查看现有任务，再启动对应总控。跨版本前请备份，旧版不保证能够读取新版新增字段。

## 开发与构建

```bash
git clone https://github.com/13536309143/verdant-circuit.git
cd verdant-circuit
```

用**Unity 2022.3.62f3c1**打开工程，等待包导入后打开`Assets/VerdantCircuit/Scenes/VerdantCircuit.unity`，点击Play。为相同Editor安装Mac Build Support及／或**Windows Build Support (Mono)**，可在Mac交叉构建Windows x64 Mono版。

```bash
# 改为本机实际安装的Unity编辑器路径。
export VERDANT_UNITY_EDITOR="/Applications/Unity/Hub/Editor/2022.3.62f3c1/Unity.app/Contents/MacOS/Unity"
./tools/build-macos.sh
./tools/build-windows.sh
```

构建脚本刷新双语表、检查规则，将应用、压缩包、日志及校验文件写入`Builds/release`，不会自动上传GitHub。Unity MCP为可选开发工具，运行游戏不需要连接服务。[测试指南](docs/TESTING.md)还提供界面检查及从零主线回放命令。

## 素材与许可

本项目独立实现，使用自行生成的视觉资源，不包含《The Farmer Was Replaced》的源代码或美术素材。字体、Unity组件等见[第三方声明](THIRD_PARTY_NOTICES.md)。公开源码不等同于开放许可，目前尚未指定项目统一开源许可证。

[更新日志](CHANGELOG.md) · [历史发行](https://github.com/13536309143/verdant-circuit/releases) · [反馈问题](https://github.com/13536309143/verdant-circuit/issues)
