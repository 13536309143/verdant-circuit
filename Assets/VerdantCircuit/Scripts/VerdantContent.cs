using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verdant
{
    [Serializable] public class CropDef
    {
        public string id, name, note; public int cost, value, yield, research;
        public float seconds; public string color; public int chapter;
        public CropDef(string id, string name, int cost, int value, int yield, float seconds, int research, int chapter, string color, string note)
        { this.id=id; this.name=name; this.cost=cost; this.value=value; this.yield=yield; this.seconds=seconds; this.research=research; this.chapter=chapter; this.color=color; this.note=note; }
    }
    [Serializable] public class TechDef
    {
        public string id, name, note, prerequisite; public int price, knowledge, chapter;
        public TechDef(string id,string name,string note,int price,int knowledge,int chapter,string prerequisite="")
        {this.id=id;this.name=name;this.note=note;this.price=price*3;this.knowledge=knowledge*2;this.chapter=chapter;this.prerequisite=prerequisite;}
    }
    [Serializable] public class ContractDef
    {
        public string title, story, metric, hint; public int amount, reward, knowledge;
        public ContractDef(string title,string story,string metric,int amount,int reward,int knowledge,string hint)
        {this.title=title;this.story=story;this.metric=metric;this.amount=amount;this.reward=reward;this.knowledge=knowledge;this.hint=hint;}
    }
    public static class VerdantContent
    {
        public const string Version="0.10.1";
        public static readonly string[] Chapters={"初抵禾湾","循环的力量","知水知土","共生花园","精密农艺","星辉计划"};
        public static readonly CropDef[] Crops={
            new CropDef("grain","稻穗",0,3,3,10,1,0,"D8BA60","免费播种；稳定的起步作物，湿润时生长更快。"),
            new CropDef("radish","红萝卜",2,7,2,17,2,0,"E68F6E","需要翻土；肥沃土壤让单次收获增加。"),
            new CropDef("flax","蓝亚麻",4,10,2,23,3,1,"8BA6DC","相邻稻穗可提升产量；适合交错种植。"),
            new CropDef("berry","蓝莓",6,9,3,28,3,2,"7889D1","需湿润土壤；收获后留下植株再生两次。"),
            new CropDef("sunflower","向日葵",8,14,2,32,4,2,"F0BF56","成熟收获可积累光能，为星辉花供应能源。"),
            new CropDef("pumpkin","蜜南瓜",12,18,2,40,5,3,"DE914C","需要翻土；相邻成熟南瓜越多，连片收获越丰厚。"),
            new CropDef("tea","云雾茶",16,24,2,48,6,4,"6AA688","需要灌溉；同类过密会减产，与花卉相邻则增产。"),
            new CropDef("starbloom","星辉花",24,40,2,58,9,5,"C1A2DE","需要翻土、灌溉及每次 3 光能；邻接茶树提高研究收益。")
        };
        public static readonly TechDef[] Techs={
            new TechDef("motor1","轻量马达","动作间隔缩短 25%",90,8,0),
            new TechDef("yield1","良种培育","全部作物基础产量 +1",140,14,0),
            new TechDef("field6","开垦东岸","农田由 4×4 扩展到 6×6",260,25,1,"yield1"),
            new TechDef("flax","纤维作物","解锁蓝亚麻与伴生增产",180,18,1,"field6"),
            new TechDef("water","蓄水系统","解锁灌溉、蓝莓与向日葵",230,24,2,"flax"),
            new TechDef("compost","循环堆肥","解锁施肥，肥力可增加产量",290,32,2,"water"),
            new TechDef("motor2","伺服马达","动作进一步提速",450,45,2,"motor1"),
            new TechDef("retention","保水土层","浇水保留时间翻倍",370,36,2,"water"),
            new TechDef("pumpkin","连片农艺","解锁蜜南瓜及邻接收获加成",500,50,3,"retention"),
            new TechDef("field8","开垦西坡","农田扩展到 8×8",900,85,3,"field6"),
            new TechDef("yield2","种质改良","全部作物基础产量再 +1",780,70,3,"yield1"),
            new TechDef("solar","聚光采集","向日葵光能产出翻倍",640,60,3,"water"),
            new TechDef("tea","云雾茶园","解锁云雾茶与花茶共生",1000,100,4,"yield2"),
            new TechDef("fertile","活性土壤","土壤肥力消耗减半",950,85,4,"compost"),
            new TechDef("motor3","量子调度","最低动作间隔降至 0.08 秒",1500,125,4,"motor2"),
            new TechDef("starbloom","星辉温室","解锁星辉花；消耗光能播种",1800,160,5,"tea"),
            new TechDef("knowledge","生态实验室","收获研究点增加 50%",1500,130,5,"fertile"),
            new TechDef("mastery","禾湾自治站","最终工程：证明农场可以持续自运转",3000,260,5,"starbloom")
        };
        public static readonly ContractDef[] Contracts={
            C("第一步","欢迎来到禾湾。先认识你的巡田机器人。","moves",8,35,5,"方向键或 WASD 移动；点击农田也可以导航。"),
            C("播下希望","免费的稻穗，是这片土地的第一份礼物。","planted",8,60,8,"按空格播种，或执行 plant(grain)。"),
            C("第一份收成","金色穗头出现时，稻穗就成熟了。","grain",2000,100,12,"按 E 收获；脚本用 if ready(): 判断成熟。"),
            C("从重复中解放","让代码完成你已经学会的动作。","programActions",30,120,15,"载入「稻穗巡田」模板，点击运行。"),
            C("马达的低语","一台更敏捷的机器人，能照看更大的田。","tech:motor1",1,150,20,"在「科技」页购买轻量马达。"),
            C("循环巡田","重复不是负担，而是一段可以复用的程序。","programActions",160,180,20,"while True: 会持续运行，随时可以停止。"),
            C("根系向下","萝卜喜欢松软的土。先翻土，再播种。","radish",1000,190,24,"till() 翻土；plant(radish) 播种。"),
            C("向东开垦","让更多土地加入你的循环。","size",6,220,25,"研究「开垦东岸」，循环用 size() 自动适配。"),
            C("蓝色纤维","在稻穗旁栽培亚麻，观察共生的收益。","flax",1400,260,30,"研究纤维作物；相邻稻穗可增加亚麻产量。"),
            C("一千次小事","真正的自动化，是许多小动作的可靠组合。","programActions",500,300,35,"用 for y in range(size()): 扫描整片农田。"),
            C("知水知土","水分会改变生长速度，也会随时间消散。","tech:water",1,300,35,"购买蓄水系统后，water() 可补充当前格水分。"),
            C("一片蓝莓","多年生作物，让收获不再意味着重新开始。","berry",2400,380,42,"蓝莓需湿润土壤；每株可连续收获三次。"),
            C("把阳光留下","收下向日葵，也把光能留给未来的实验。","sunflower",1300,430,45,"每次收获向日葵可得到光能。"),
            C("土壤的循环","肥力提高的不只是生长速度，还有每次收获。","fertilized",20,450,50,"先研究循环堆肥；施肥每次花费 2 金币。"),
            C("水的节奏","让灌溉成为程序的一部分。","watered",80,480,52,"if moisture() < 0.6: water()；保水土层与降雨会让水分保持在 50% 以上。"),
            C("蜜色连片","让南瓜相邻成熟，再收获它们。","pumpkin",1200,600,65,"相邻成熟南瓜可提高当次收获，顺序也会影响收益。"),
            C("共生的证据","不同植物之间，也存在可以编程的关系。","bonusHarvests",35,650,70,"稻穗与亚麻、花卉与茶树都能形成增益。"),
            C("西坡新田","更大的农田，需要更好的循环策略。","size",8,700,75,"研究「开垦西坡」，自动扫描可以覆盖 64 个格子。"),
            C("可靠的调度","在更大的世界里，让程序保持可靠。","programActions",1800,750,80,"点击暂停可查看当前执行行；单步便于排错。"),
            C("丰收工程","把一份小小的收获，扩展成稳定的供给。","harvested",650,800,85,"尝试良种与保水研究，比较每分钟收入。"),
            C("山雾里的茶","茶树需要水，也需要为根系留出空间。","tea",1250,950,95,"同类过密会减产；可在茶树间种向日葵。"),
            C("把经验写成函数","用函数封装耕作动作，再在循环中调用它。","functionCalls",30,1000,100,"def tend(): 定义函数；tend() 调用。"),
            C("让每一滴水有用","高效率来自判断，而非机械重复。","bonusHarvests",100,1100,110,"结合 moisture()、ready() 与 crop() 决定下一步。"),
            C("千穗入仓","禾湾已不再是一块无人照管的土地。","harvested",1400,1200,120,"仓库可查看各作物库存、累计收获与规则。"),
            C("精密的心跳","更短的动作周期，也要求更清晰的程序。","tech:motor3",1,1300,130,"研究伺服马达后，解锁量子调度。"),
            C("星辉计划","把阳光转化成新的作物，把作物转化成知识。","tech:starbloom",1,1400,140,"星辉花需要茶园研究；预留向日葵积累的光能。"),
            C("第一缕星光","星辉花需要灌溉、松土和光能共同支持。","starbloom",500,1600,160,"星辉花每次播种消耗 3 光能；茶树可提高研究收益。"),
            C("一万次协作","每个动作，都来自你写下的一条规则。","programActions",10000,1800,180,"检查脚本是否会补种、浇水并处理尚未成熟的植株。"),
            C("自治站上线","研究最终工程，为农场接通自治系统。","tech:mastery",1,2000,200,"完成星辉花与生态实验室等研究，可继续优化经济。"),
            C("禾湾长明","你完成了主线。田地会继续生长，优化也没有终点。","harvested",3000,3000,300,"完成六项试炼、收集成就，或继续在自由农场中优化。")
        };
        static ContractDef C(string a,string b,string c,int d,int e,int f,string g){return new ContractDef(a,b,c,d,e,f,g);}
        public static CropDef Crop(string id){return Array.Find(Crops,c=>c.id==id);}
        public static TechDef Tech(string id){return Array.Find(Techs,t=>t.id==id);}
        public static readonly string[] ChallengeNames={"稻穗速递","根系实验","节水栽培","连片南瓜","花茶共生","星光回路"};
        public static readonly string[] ChallengeCrops={"grain","radish","berry","pumpkin","tea","starbloom"};
        public static readonly int[] ChallengeTargets={90,80,80,90,65,50};
        public static readonly float[] ChallengeTimes={150,180,210,240,240,300};
        public static readonly string[] ChallengeNotes={"150 秒内收获 90 稻穗，体会扫描循环。","180 秒内收获 80 萝卜，处理翻土与补种。","210 秒内收获 80 蓝莓，安排水分与再生。","240 秒内收获 90 南瓜，运用邻接加成。","240 秒内收获 65 茶叶，避免同类拥挤。","300 秒内收获 50 星辉花，维持光能供应。"};
        public static readonly string[] AchievementNames={"第一份收获","百穗入仓","千穗入仓","循环之友","自动化工程师","灌溉师","共生研究员","万步巡田","八方沃土","星辉育种者","研究先驱","试炼收藏家"};
        public static readonly string[] AchievementNotes={"累计收获 1 单位作物","累计收获 100 单位作物","累计收获 1000 单位作物","脚本完成 100 个动作","脚本完成 5000 个动作","灌溉 100 次","触发 100 次共生或邻接增产","移动 10000 格","开垦 8×8 农田","收获 100 星辉花","完成 18 项研究","通过全部 6 项试炼"};
        public static readonly string[] TemplateNames={"稻穗巡田","萝卜农艺","蓝莓灌溉","阳光采集","南瓜连片","花茶间作","星辉温室","函数化农场","灌溉巡田"};
        public static readonly string[] Templates={
            "# 从每一个格子开始\nwhile True:\n    for row in range(size()):\n        for col in range(size()):\n            if ready():\n                harvest()\n            if crop() == empty:\n                plant(grain)\n            move(East)\n        move(North)",
            "while True:\n    for row in range(size()):\n        for col in range(size()):\n            if ready():\n                harvest()\n            if crop() == empty:\n                till()\n                plant(radish)\n            move(East)\n        move(North)",
            "while True:\n    for row in range(size()):\n        for col in range(size()):\n            if moisture() < 0.3:\n                water()\n            if ready():\n                harvest()\n            if crop() == empty:\n                plant(berry)\n            move(East)\n        move(North)",
            "while True:\n    for row in range(size()):\n        for col in range(size()):\n            if ready():\n                harvest()\n            if crop() == empty:\n                plant(sunflower)\n            if moisture() < 0.3:\n                water()\n            move(East)\n        move(North)",
            "while True:\n    for row in range(size()):\n        for col in range(size()):\n            if crop() == empty:\n                till()\n                plant(pumpkin)\n            if moisture() < 0.3:\n                water()\n            move(East)\n        move(North)\n    wait(45)\n    for row in range(size()):\n        for col in range(size()):\n            if ready():\n                harvest()\n            move(East)\n        move(North)",
            "while True:\n    for row in range(size()):\n        for col in range(size()):\n            if moisture() < 0.3:\n                water()\n            if ready():\n                harvest()\n            if crop() == empty:\n                if (x() + y()) % 2 == 0:\n                    plant(tea)\n                else:\n                    plant(sunflower)\n            move(East)\n        move(North)",
            "while True:\n    for row in range(size()):\n        for col in range(size()):\n            if moisture() < 0.3:\n                water()\n            if ready():\n                harvest()\n            if crop() == empty:\n                till()\n                if (x() + y()) % 2 == 0:\n                    plant(sunflower)\n                else:\n                    plant(starbloom)\n            move(East)\n        move(North)",
            "def tend():\n    if ready():\n        harvest()\n    if crop() == empty:\n        plant(grain)\n    move(East)\n\nwhile True:\n    for row in range(size()):\n        for col in range(size()):\n            tend()\n        move(North)",
            "while True:\n    for row in range(size()):\n        for col in range(size()):\n            if moisture() < 0.6:\n                water()\n            if ready():\n                harvest()\n            if crop() == empty:\n                plant(grain)\n            move(East)\n        move(North)"
        };
        public const string Manual="禾序使用一种受 Python 启发的农场语言。它只操作游戏，不会访问电脑文件或网络。\n\n【动作】\nmove(East) / move(West) / move(North) / move(South)\n跨越边界会环绕到另一侧，便于整田扫描。\nplant(grain)  播种；harvest()  收获\ntill()  把空地翻成耕地；water()  浇水\nfertilize()  施肥；wait(3)  等待 3 游戏秒\nprint(表达式)  输出到日志\n\n【传感器】\nready()  当前格是否成熟\ncrop()  当前作物，空地为 empty\nmoisture()  水分 0~1；fertility()  肥力 0~1\nx() / y()  机器人零起点坐标\nsize()  农田边长；energy()  光能\ncount(grain)  作物库存；money()  金币\nneighbors(pumpkin)  四邻格同类数量\n\n【语法】\n使用四个空格缩进。支持 # 注释、变量、+ - * / %、比较、and / or / not、if / elif / else、while、for ... in range(...)、repeat 4:、无参数 def 函数、break、continue、return。\nrange() 支持 1~3 个参数。函数在调用前定义；最多嵌套 24 层。\n\n例：\nif ready():\n    harvest()\nelse:\n    wait(1)\n\n【作物代码】\ngrain 稻穗 / radish 红萝卜 / flax 蓝亚麻\nberry 蓝莓 / sunflower 向日葵 / pumpkin 蜜南瓜\ntea 云雾茶 / starbloom 星辉花\n\n【调试】\n编辑支持100步撤销/重做：Cmd/Ctrl+Z，Cmd/Ctrl+Shift+Z。程序列表可以增删，最少一个；切换查看不停止当前执行，点击运行会替换正在执行的程序。\n运行前会检查语法。错误显示原始行号；当前行也会显示在编辑器下方。暂停后可单步执行，停止后仍保留所有收获。最多执行 500000 条指令后安全停止；无限循环每次都会让出画面更新。\n\n【进度】\n作物委托按本任务接取后的新增收获计算，每种作物有不同数量要求；不再设置时间条件。历史库存不会直接抵扣后续作物任务。\n委托按累计进度计算，完成后请点击领取。每五份委托开启下一章。作物需要相应研究；科技花费金币与研究点。主线完成后可继续自由经营。\n\n【保存】\n程序列表和农场会每 30 秒自动保存，退出时也会保存。手动保存会保留上一份备份。试炼使用独立临时农场，不会消耗主线物资。";
    }
}
