using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Verdant.Automation;

public static partial class VerdantFullPlaythrough
{
    static string SimpleCropCode(string item,string target)
    {
        string plant=item=="wood"?"if num_unlocked(Unlocks.Trees)>0 and (get_pos_x()+get_pos_y())%2==0:\n    plant(Entities.Tree)\nelse:\n    plant(Entities.Bush)":"plant(Entities.Carrot)";
        string body="if can_harvest():\n    harvest()"+(item=="hay"?"":"\nif get_entity_type()==None:\n"+Indent(plant,4));
        return (item=="hay"?"clear()":ClearSoil())+"\nwhile num_items(Items."+itemSymbols[item]+")<"+target+":\n"+Indent(Sweep(body),4);
    }
    static string PumpkinCode(string target,bool infection)
    {
        string body="if get_ground_type()!=Grounds.Soil:\n    till()\nif get_entity_type()==None or get_entity_type()==Entities.Dead_Pumpkin:\n    plant(Entities.Pumpkin)\nif get_entity_type()!=Entities.Pumpkin or not can_harvest():\n    complete=False";
        return "clear()\nwhile num_items(Items."+(infection?"Weird_Substance":"Pumpkin")+")<"+target+":\n    complete=True\n"+Indent(Sweep(body),4)+"\n"+Indent(infection?"if complete and num_items(Items.Fertilizer)>0:\n    use_item(Items.Fertilizer)\n    harvest()":"if complete:\n    harvest()",4);
    }
    static string CactusCode(string target)
    {
        return "clear()\nwhile num_items(Items.Cactus)<"+target+":\n"+Indent(Sweep("if get_ground_type()!=Grounds.Soil:\n    till()\nif get_entity_type()==None:\n    plant(Entities.Cactus)"),4)+"\n    complete=False\n    while not complete:\n        complete=True\n"+Indent(Sweep("if not can_harvest():\n    complete=False"),8)+"\n"+Indent(ClassicExamples.Sources[4],4);
    }
    static string PowerCode(string target)
    {
        return "clear()\nwhile num_items(Items.Power)<"+target+":\n"+Indent(Sweep("if get_ground_type()!=Grounds.Soil:\n    till()\nif get_entity_type()==None:\n    plant(Entities.Sunflower)"),4)+"\n"+Indent(Sweep("while not can_harvest():\n    pass"),4)+"\n    for petals in range(15,6,-1):\n"+Indent(Sweep("if get_entity_type()==Entities.Sunflower and measure()==petals:\n    harvest()"),8);
    }
    static void RunCompetitionMatrix()
    {
        var programs=new Dictionary<string,string>{{"Hay",SimpleCropCode("hay","1000")},{"Wood",SimpleCropCode("wood","1000")},{"Carrots",SimpleCropCode("carrot","1000")},{"Pumpkins",PumpkinCode("1000",false)},{"Sunflowers",PowerCode("1000")},{"Cactus",CactusCode("1000")},{"Maze",MazeStart()+"\n"+ClassicExamples.Sources[5]},{"Dinosaur",ClearSoil()+"\n"+ClassicExamples.Sources[8]}};
        foreach(var pair in programs)
        {
            string file="race_"+pair.Key.ToLowerInvariant();farm.Data.programs.Add(new ClassicProgram(file,pair.Value));
            Execute("本地竞速 "+pair.Key,"游戏自带的种子 41、8×8 与供给预设；新增 1000 单位后所有任务终止。","leaderboard_run(Leaderboards."+pair.Key+",'"+file+"',64)",7200,"race");
            if(pair.Key!="Dinosaur")Execute("单机竞速 "+pair.Key,"相同官方本地预设，限制为一台无人机；仍使用实际解释器。","leaderboard_run(Leaderboards."+pair.Key+"_Single,'"+file+"',64)",7200,"race");
        }
        string reset=ResetProgram();farm.Data.programs.Add(new ClassicProgram("race_reset",reset));
        Execute("最快重置完整竞速","从一格零物资开始，遵循竞速预设允许的语言功能，通过真实生产支付科技成本，直到解锁本地竞速。","leaderboard_run(Leaderboards.Fastest_Reset,'race_reset',64)",7200,"race");
        AuditMazePreset();
        Check(farm.Data.records.Count==16,"The complete 9 UI / 7 single-drone competition matrix did not record all results.");
    }
    static void AuditMazePreset()
    {
        var world=new ClassicFarm(41);world.Data.allowLockedSyntax=true;foreach(var t in ClassicFarm.Techs)world.Data.unlocks.Add(new Stock(t.id,t.max));world.Resize(8);foreach(string item in ClassicFarm.Items)if(item!="gold")world.Add(item,1000000000);
        var audit=new ClassicSession(world);audit.Start(MazeStart()+"\nat_start=(get_entity_type()==Entities.Treasure)");for(int i=0;i<1000&&audit.Running;i++)audit.FixedTick();Check(audit.Error==""&&!audit.Running,"Maze preset audit failed.");bool atStart=ScriptValue.Truth(audit.drones[0].task.context.scope.values["at_start"]);int treasureIndex=world.treasure;audit.Start(ClassicExamples.Sources[5]);for(int i=0;i<10000&&audit.Running;i++)audit.FixedTick();Check(audit.Error==""&&!audit.Running,"Maze traversal audit failed.");var visited=(HashSet<object>)audit.drones[0].task.context.scope.values["visited"];
        File.WriteAllText(System.IO.Path.Combine(output,"maze-preset-audit.json"),"{\"seed\":41,\"size\":8,\"treasureIndex\":"+treasureIndex+",\"visitedCells\":"+visited.Count+",\"treasureAtStart\":"+(atStart?"true":"false")+",\"scope\":\"separate native competition preset audit, not main progression\"}");
    }
    static string ResetProgram()
    {
        string code="def buy(tech):\n    cost=get_cost(tech)\n    if cost==None:\n        return\n    for item in cost:\n        ensure(item,cost[item])\n    unlock(tech)\n\ndef fund(entity,amount):\n    cost=get_cost(entity)\n    for item in cost:\n        ensure(item,cost[item]*amount)\n\ndef ensure(item,target):\n    if num_items(item)>=target:\n        return\n    n=get_world_size()\n";
        code+="    if item==Items.Hay:\n"+Indent(SimpleCropCode("hay","target"),8)+"\n    elif item==Items.Wood:\n"+Indent(SimpleCropCode("wood","target"),8);
        code+="\n    elif item==Items.Carrot:\n        if num_unlocked(Unlocks.Carrots)==0:\n            buy(Unlocks.Carrots)\n        amount=(int((target-num_items(item))/(n*n*2**(num_unlocked(Unlocks.Carrots)-1)))+3)*n*n\n        fund(Entities.Carrot,amount)\n"+Indent(SimpleCropCode("carrot","target"),8);
        code+="\n    elif item==Items.Pumpkin or item==Items.Weird_Substance:\n        if num_unlocked(Unlocks.Pumpkins)==0:\n            buy(Unlocks.Pumpkins)\n        infected=item==Items.Weird_Substance\n        if infected and num_unlocked(Unlocks.Fertilizer)==0:\n            buy(Unlocks.Fertilizer)\n        yield_amount=n*n*min(n,6)*2**(num_unlocked(Unlocks.Pumpkins)-1)\n        if infected:\n            yield_amount=yield_amount/2\n        amount=(int((target-num_items(item))/yield_amount)+3)*n*n*3\n        fund(Entities.Pumpkin,amount)\n        if infected:\n"+Indent(PumpkinCode("target",true),12)+"\n        else:\n"+Indent(PumpkinCode("target",false),12);
        code+="\n    elif item==Items.Cactus:\n        if num_unlocked(Unlocks.Cactus)==0:\n            buy(Unlocks.Cactus)\n        amount=(int((target-num_items(item))/(n*n*n*n*2**(num_unlocked(Unlocks.Cactus)-1)))+3)*n*n\n        fund(Entities.Cactus,amount)\n"+Indent(CactusCode("target"),8);
        code+="\n    elif item==Items.Bone:\n        while num_items(Items.Bone)<target:\n            ensure(Items.Cactus,n*n+1)\n"+Indent(ClearSoil()+"\n"+ClassicExamples.Sources[8],12);
        code+="\n    elif item==Items.Gold:\n        while num_items(Items.Gold)<target:\n            ensure(Items.Weird_Substance,n*2**(num_unlocked(Unlocks.Mazes)-1))\n"+Indent(MazeStart()+"\n"+ClassicExamples.Sources[5],12);
        code+="\n\nbuy(Unlocks.Plant)\nfor i in range(3):\n    buy(Unlocks.Expand)\nfor i in range(8):\n    buy(Unlocks.Grass)\n    buy(Unlocks.Speed)\nfor i in range(8):\n    buy(Unlocks.Trees)\nbuy(Unlocks.Carrots)\nbuy(Unlocks.Watering)\nbuy(Unlocks.Pumpkins)\nbuy(Unlocks.Fertilizer)\nbuy(Unlocks.Cactus)\nbuy(Unlocks.Dinosaurs)\nbuy(Unlocks.Simulation)\nbuy(Unlocks.Mazes)\nbuy(Unlocks.Leaderboard)";
        File.WriteAllText(System.IO.Path.Combine(output,"fastest-reset.py"),code+"\n");return code;
    }
}
