using UnityEngine;
namespace Verdant.Automation
{
    public static class ClassicFleetExample
    {
        public static readonly string[] Names={"island_controller","island_planner","fleet_worker","island_water","island_grain","island_foundry","island_crystal","island_medicine","island_archive","island_cargo"};
        public static string ProgramFor(FleetUnit unit){switch(unit.role){case "irrigator":return "island_water";case "hauler":return "island_cargo";case "scout":return "island_crystal";case "engineer":return unit.island==5?"island_archive":"island_foundry";default:return unit.island==4?"island_medicine":"island_grain";}}
        public static string Code(string name){var source=Resources.Load<TextAsset>("Fleet-"+name);return source==null?"# Missing example module.":source.text;}
        public static string Source {get{var source=Resources.Load<TextAsset>("ArchipelagoController");return source==null?"# Controller asset is missing.":source.text;}}
    }
}
