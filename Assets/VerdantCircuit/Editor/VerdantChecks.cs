using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verdant;

public static class VerdantChecks
{
    static int passed;static readonly List<string> results=new List<string>();
    static void Check(bool value,string name){if(!value)throw new Exception("FAIL: "+name);passed++;results.Add(name);}
    static VerdantFarm Farm(){return new VerdantFarm(false,"verdant-verification.json");}
    static FarmVM Run(VerdantFarm f,string code,int limit=10000)
    {var vm=new FarmVM(f,new FarmCompiler().Compile(code));for(int i=0;i<limit&&!vm.Finished;i++)vm.Step();return vm;}
    public static string RunAll()
    {
        passed=0;results.Clear();
        Check(VerdantContent.Crops.Length==8&&VerdantContent.Contracts.Length==30&&VerdantContent.Techs.Length==18&&VerdantContent.Templates.Length==9&&VerdantContent.TemplateNames.Length==9,"内容目录完整");
        foreach(string code in VerdantContent.Templates)new FarmCompiler().Compile(code);Check(true,"9 个范例均通过语法检查");
        var f=Farm();f.Data.x=3;f.Act("move","East");Check(f.Data.x==0,"边界环绕");
        Run(f,"n = 0\nfor i in range(4):\n    n = n + 1\n    move(East)");Check(f.Data.x==0&&f.Count("programActions")==4,"for 循环与变量");
        var vm=Run(f,"n = 0\nfor i in range(10, 0, -2):\n    if i == 6:\n        continue\n    n = n + 1\n    if n == 3:\n        break");Check(FarmVM.Number(vm.Variables["n"])==3,"range 步长、break 与 continue");
        vm=Run(f,"a = 0\nif False:\n    a = 10\nelif 2 + 3 * 4 == 14:\n    a = 2\nelse:\n    a = 9\nrepeat 3:\n    a = a + 1");Check(FarmVM.Number(vm.Variables["a"])==5,"表达式优先级、elif 与 repeat");
        int before=f.Count("moves");Run(f,"def step():\n    move(North)\n    return\nrepeat 4:\n    step()");Check(f.Count("moves")==before+4&&f.Count("functionCalls")==4,"函数与 return");
        bool invalid=false;try{new FarmCompiler().Compile("while True\n    harvest()");}catch(FarmCodeException e){invalid=e.Line==1;}Check(invalid,"语法错误携带原始行号");
        invalid=false;try{Run(f,"print(1 / 0)");}catch(FarmCodeException e){invalid=e.Line==1;}Check(invalid,"运行时除零保护");
        invalid=false;try{Run(f,"def recurse():\n    recurse()\nrecurse()");}catch(FarmCodeException){invalid=true;}Check(invalid,"递归深度保护");
        var runner=new VerdantRunner(f);runner.Start("while True:\n    pass");runner.Tick(1);Check(runner.Running&&runner.VM.Executed==160,"空无限循环按帧让出执行");runner.Stop();
        f=Farm();f.Data.x=1;f.Data.y=1;Check(!f.Act("plant","radish"),"根系作物要求翻土");f.Act("till");Check(f.Act("plant","radish"),"翻土后可播种");f.Tick(100);int coins=f.Data.coins;Check(f.Ready(f.Here)&&f.Act("harvest")&&f.Data.coins>coins&&f.Count("radish")>0,"生长、收获、收入与研究点");
        f=Farm();f.Data.coins=0;f.Data.x=1;f.Data.y=1;Check(f.Act("plant","grain"),"零金币仍可免费播种，避免软锁");
        f=Farm();f.Data.tech.Add("water");f.Data.x=1;f.Data.y=1;f.Act("water");f.Act("plant","berry");for(int i=0;i<3;i++){f.Tick(200);Check(f.Act("harvest"),"蓝莓第 "+(i+1)+" 次收获");}Check(f.Here.crop=="","再生次数结束后腾出地块");
        f=Farm();f.Resize(6);Check(f.Data.plots.Count==36&&f.Data.plots[0].crop=="grain","扩田保持原地块状态");
        f=Farm();f.Add("moves",8);Check(f.Claim()&&f.Data.contract==1,"委托进度与奖励");
        f.Data.coins=1000;f.Data.knowledge=100;Check(f.Research("motor1")&&f.Has("motor1"),"研究消耗与解锁");
        f=Farm();VerdantPrograms.Add(f.Data);VerdantPrograms.Add(f.Data);f.Data.coins=777;f.Data.scripts[2]="print(42)";f.Save();var loaded=new VerdantFarm(true,"verdant-verification.json");Check(loaded.Data.coins==777&&loaded.Data.scripts[2]=="print(42)","存读档保留资源和程序");
        f.Data.coins=888;f.Save();File.WriteAllText(f.SavePath,"broken");loaded=new VerdantFarm(true,"verdant-verification.json");Check(loaded.Data.coins==777,"损坏存档可恢复上一份备份");
        f=Farm();f.Data.coins=555;f.StartChallenge(0);f.Add("grain",90);f.Tick(.1f);Check(!f.InChallenge&&f.Data.coins==955&&f.Data.stars[0]>0,"试炼通过恢复主线并发放奖励");
        f=Farm();f.Data.coins=555;f.StartChallenge(0);f.FinishChallenge(false);Check(f.Data.coins==555&&!f.InChallenge,"退出试炼不消耗主线物资");
        f=Farm();f.Data.contract=10;f.Add("stock:grain",30);f.Add("stock:radish",8);Check(VerdantEconomy.Craft(f,VerdantEconomy.Recipes[0])&&f.Count("stock:grain")==0&&f.Count("stock:ricebox")==1,"加工消耗材料并产生补给品");
        var order=VerdantEconomy.Order(f,0);f.Add("stock:"+order.a,order.amountA);f.Add("stock:"+order.b,order.amountB);Check(VerdantEconomy.Deliver(f,0)&&f.Count("orders")==1&&VerdantEconomy.Order(f,0).title!=order.title,"订单交付消耗库存并持续更新");
        // A representative automated farm must produce a sustainable harvest through the actual VM.
        f=Farm();vm=new FarmVM(f,new FarmCompiler().Compile(VerdantContent.Templates[0]));float time=0;while(time<120){for(int j=0;j<160;j++){if(vm.Step())break;}f.Tick(.32f);time+=.32f;}Check(f.Count("grain")>100&&f.Data.coins>300,"真实范例连续种收并形成正收入");
        // Match real Update ordering and frame-time steps, including retention after a full rain.
        foreach(bool retained in new[]{false,true})
        foreach(float phase in new[]{0f,175f,210f})
        foreach(int speed in new[]{1,8})
        {
            var irrigation=Farm();irrigation.Resize(6);irrigation.Data.contract=14;irrigation.Data.clock=phase;
            irrigation.Data.tech.Add("water");irrigation.Data.tech.Add("motor1");if(retained)irrigation.Data.tech.Add("retention");
            foreach(var plot in irrigation.Data.plots){plot.crop="grain";plot.growth=0;plot.moisture=1;}
            var irrigationRunner=new VerdantRunner(irrigation);irrigationRunner.Start(VerdantContent.Templates[8]);
            float delta=speed/60f,elapsed=0;
            while(elapsed<1260&&irrigation.Count("watered")<80&&irrigationRunner.Error=="")
            {irrigation.Tick(delta);irrigationRunner.Tick(delta);elapsed+=delta;}
            Check(irrigationRunner.Error==""&&irrigation.Count("watered")>=80,
                "灌溉范例达到80次：保水="+retained+" / 天气相位="+phase+" / "+speed+"倍");
        }
        f=Farm();f.Data.tech.Add("water");f.Data.clock=176;f.Data.water=0;f.Here.moisture=0.5f;f.Tick(1);
        Check(f.Data.water>0&&f.Here.moisture>0.5f&&f.Count("watered")==0,"自然回水和降雨不冒充成功浇水");
        var history=new CodeHistory();for(int i=0;i<=20;i++)history.Record("revision "+i,i,i);
        bool validHistory=true;for(int i=19;i>=0;i--)validHistory&=history.Undo().Text=="revision "+i;for(int i=1;i<=20;i++)validHistory&=history.Redo().Text=="revision "+i;
        Check(validHistory,"20步撤销与重做");history.Undo();history.Record("branch",0,0);Check(history.Redo()==null,"编辑分支清空重做");
        f=Farm();bool singleDefault=f.Data.scripts.Length==1;for(int i=0;i<11;i++)VerdantPrograms.Add(f.Data);f.Data.scripts[11]="print(101)";f.Save();loaded=new VerdantFarm(true,"verdant-verification.json");
        Check(singleDefault&&loaded.Data.scripts.Length==12&&loaded.Data.scripts[11]=="print(101)","默认一个程序且动态列表存档");while(f.Data.scripts.Length>1)VerdantPrograms.Remove(f.Data,0);Check(!VerdantPrograms.Remove(f.Data,0),"至少保留一个程序");
        f=Farm();f.Data.contract=2;f.Add("grain",VerdantContent.Contracts[2].amount);Check(f.ContractReady,"数量达标可立即领奖，无时间门槛");
        f=Farm();f.Data.contract=5;f.Add("programActions",160);f.Add("radish",5000);Check(f.Claim()&&f.ContractProgress==0,"接取作物任务不计历史收获");
        f.Add("radish",VerdantContent.Contracts[6].amount-1);Check(!f.ContractReady,"数量差一个不能领奖");f.Add("radish");Check(f.ContractReady,"任务新增收获达标可以领奖");
        return JsonUtility.ToJson(new Report{passed=passed,checks=results.ToArray()},true);
    }
    static FarmVM campaignVM;
    static VerdantFarm campaign;
    static int campaignTemplate=-1;
    static int campaignActions;
    static readonly List<string> campaignLog=new List<string>();
    static void CampaignStep(int template)
    {
        if(campaignVM==null||template!=campaignTemplate||campaignVM.Finished)
        {campaignVM=new FarmVM(campaign,new FarmCompiler().Compile(template==-2?"while True:\n    for row in range(size()):\n        for col in range(size()):\n            if ready():\n                harvest()\n            if crop() == empty:\n                if (x() + y()) % 2 == 0:\n                    plant(flax)\n                else:\n                    plant(grain)\n            move(East)\n        move(North)":VerdantContent.Templates[template]));campaignTemplate=template;}
        for(int i=0;i<160;i++)if(campaignVM.Step())break;
        campaign.Tick(Mathf.Max(.02f,campaignVM.Wait));
        if(++campaignActions>160000)throw new Exception("主线回放超过动作预算："+campaign.Data.contract);
    }
    static void Buy(string id)
    {
        if(campaign.Has(id))return;var t=VerdantContent.Tech(id);
        if(t==null||campaign.Chapter<t.chapter)throw new Exception("科技章节不可达："+id);
        if(t.prerequisite!="")Buy(t.prerequisite);
        while(campaign.Data.coins<t.price||campaign.Data.knowledge<t.knowledge)CampaignStep(0);
        if(!campaign.Research(id))throw new Exception("研究失败："+id);
    }
    public static string Playthrough()
    {
        campaign=Farm();campaignVM=null;campaignTemplate=-1;campaignActions=0;campaignLog.Clear();
        while(campaign.CurrentContract!=null)
        {
            var c=campaign.CurrentContract;string metric=c.metric;int index=campaign.Data.contract;
            if(metric.StartsWith("tech:"))Buy(metric.Substring(5));
            else if(metric=="size")Buy(c.amount==6?"field6":"field8");
            else
            {
                int template=0;
                if(metric=="radish")template=1;
                if(metric=="flax"){Buy("flax");template=-2;}
                if(metric=="berry"){Buy("water");template=2;}
                if(metric=="energyEarned"||metric=="sunflower"){Buy("water");template=3;}
                if(metric=="pumpkin"){Buy("pumpkin");template=4;}
                if(metric=="bonusHarvests"){Buy("flax");template=-2;}
                if(metric=="tea"){Buy("tea");Buy("water");template=5;}
                if(metric=="functionCalls")template=7;
                if(metric=="starbloom"){Buy("starbloom");Buy("solar");template=6;}
                if(metric=="fertilized")Buy("compost");
                if(metric=="watered"){Buy("water");Buy("retention");template=8;}
                while(!campaign.ContractReady)
                {
                    if(metric=="fertilized")campaign.Act("fertilize");
                    CampaignStep(template);
                }
            }
            if(!campaign.Claim())throw new Exception("无法领取委托："+c.title);
            campaignLog.Add((index+1)+". "+c.title+" / 游戏时间 "+Mathf.RoundToInt(campaign.Data.clock)+" 秒");
        }
        return JsonUtility.ToJson(new CampaignReport{contracts=campaign.Data.contract,actions=campaignActions,harvested=campaign.Count("harvested"),coins=campaign.Data.coins,gameSeconds=campaign.Data.clock,trace=campaignLog.ToArray()},true);
    }
    [Serializable] public class CampaignReport{public int contracts,actions,harvested,coins;public float gameSeconds;public string[] trace;}
    [Serializable] public class Report{public int passed;public string[] checks;}
}
