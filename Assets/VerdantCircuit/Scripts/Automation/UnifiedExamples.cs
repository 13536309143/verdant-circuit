using System;
namespace Verdant.Automation
{
    public static class UnifiedProductNames
    {
        static readonly string[] Ids={"hay","wood","carrot","pumpkin","solar_energy","cactus","bone","coins","water","fertilizer","catalyst","compost","fish_feed","honey","rose","tulip","lavender","lily","orchid","lotus","stone","copper_ore","tin_ore","silver_ore","gold_ore","platinum_ore","rough_diamond","copper","tin","bronze","silver","gold","platinum","diamond","carp","trout","salmon","cod","tuna","koi","fishmeal"};
        static readonly string[] En={"Hay","Wood","Carrot","Pumpkin","Solar energy","Cactus","Bone","Treasure coins","Water","Fertilizer","Catalyst","Compost","Fish feed","Honey","Rose","Tulip","Lavender","Lily","Orchid","Lotus","Stone","Copper ore","Tin ore","Silver ore","Gold ore","Platinum ore","Rough diamond","Copper","Tin","Bronze","Silver","Gold bullion","Platinum","Cut diamond","Carp","Trout","Salmon","Cod","Tuna","Koi","Fishmeal"};
        static readonly string[] Zh={"干草","木材","胡萝卜","南瓜","太阳能","仙人掌","骨头","宝藏金币","水","肥料","催化剂","堆肥","鱼饵","蜂蜜","玫瑰","郁金香","薰衣草","百合","兰花","荷花","石头","铜矿石","锡矿石","银矿石","金矿石","铂矿石","金刚石原石","铜锭","锡锭","青铜","白银","黄金锭","铂金","钻石","鲤鱼","鳟鱼","鲑鱼","鳕鱼","金枪鱼","锦鲤","鱼粉"};
        public static string Name(string item){int index=Array.IndexOf(Ids,item);return index<0?item:L10n.Code=="en"?En[index]:Zh[index];}
    }
    public static class UnifiedExamples
    {
        public static readonly string[] Names={"universe_controller","earth_manager","flower_manager","ocean_manager","mineral_manager","earth_worker","bee_worker","boat_worker","miner_worker","ufo_dispatch","worker_helpers"};
        public static string Code(string file)
        {
            switch(file)
            {
                case "universe_controller":return "# Bind to Universe. Managers stay alive independently.\nplanet_run(0, 'earth_manager')\nfor planet in universe_planets():\n    if planet['unlocked']:\n        if planet['id'] == 1:\n            planet_run(1, 'flower_manager')\n        elif planet['id'] == 2:\n            planet_run(2, 'ocean_manager')\n        elif planet['id'] == 3:\n            planet_run(3, 'mineral_manager')\n";
                case "earth_manager":return Manager(0,"earth_worker");case "flower_manager":return Manager(1,"bee_worker");case "ocean_manager":return Manager(2,"boat_worker");case "mineral_manager":return Manager(3,"miner_worker");
                case "earth_worker":return "# Bounded latitude rows: sweep east, then return west at each row.\nwhile True:\n    for y in range(get_world_height()):\n        for x in range(get_world_width()):\n            if can_harvest():\n                harvest()\n            if x + 1 < get_world_width():\n                move(East)\n        for x in range(get_world_width() - 1):\n            move(West)\n        if y + 1 < get_world_height():\n            move(North)\n    for y in range(get_world_height() - 1):\n        move(South)\n";
                case "bee_worker":return Worker("cultivate_rose");case "boat_worker":return Worker("catch_carp");case "miner_worker":return Worker("mine_stone");
                case "ufo_dispatch":return "# Run from Universe, after Spaceflight and Flower Planet are researched.\nships = ufo_fleet()\nif len(ships) == 0:\n    ship = ufo_build()\nelse:\n    ship = ships[0]['id']\nif ship > 0:\n    if planet_inventory(0, 'water') >= 12:\n        ufo_transfer(ship, 0, 1, 'water', 12)\n";
                default:return "def idle(id):\n    state = worker_state(id)\n    return state != None and state['jobs'] == 0\n";
            }
        }
        static string Manager(int planet,string file){return "# One free initial native worker per unlocked planet.\nworker = worker_starter("+planet+")\nif worker > 0:\n    program_bind(worker, '"+file+"')\n    program_run(worker)\n";}
        static string Worker(string function){return "from worker_helpers import idle\nwhile True:\n    if idle(worker_self()):\n        "+function+"(worker_self(), 1)\n    worker_wait(1)\n";}
    }
}
