using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Verdant
{
    [Serializable] public sealed class Counter { public string key; public int value; public Counter(string k,int v){key=k;value=v;} }
    [Serializable] public sealed class Plot
    {
        public string crop=""; public bool tilled; public float growth, moisture=0.35f, fertility=0.4f; public int regrowth;
    }
    [Serializable] public sealed class FarmSave
    {
        public int schema=1, size=4, x, y, contract, selectedCrop, slot;
        public int coins=120, knowledge, energy, totalActions; public float clock, water=50;
        public float volume=0.35f; public bool music=true;
        public List<Plot> plots=new List<Plot>(); public List<string> tech=new List<string>();
        public List<string> achievements=new List<string>(); public List<Counter> metrics=new List<Counter>();
        public int droneCapacity=1,footprintTier,activeDrone,nextDroneId=1;public bool fleetPaused;public List<FarmDrone> drones=new List<FarmDrone>();
        public int cropTaskStart;public int[] stars=new int[6]; public string[] scripts=new string[1];
    }
    public sealed class VerdantFarm
    {
        public VerdantFleet Fleet;public FarmSave Data; public event Action<string> Message; public event Action Changed;
        public bool InChallenge { get {return challenge>=0;} }
        public int challenge=-1; public float challengeTime; public int challengeStart;
        private FarmSave mainSnapshot;
        string saveState="尚未保存",savedAt="";public string LastSaveMessage{get{return L10n.T(saveState)+savedAt;}}
        readonly string saveName;
        public string SavePath {get{return Path.Combine(Application.persistentDataPath,saveName);}}
        public int Chapter {get{return Mathf.Min(5,Data.contract/5);}}
        public Plot Here {get{return Data.plots[Data.y*Data.size+Data.x];}}
        public float ActionDelay {get{return Has("motor3")?0.08f:Has("motor2")?0.15f:Has("motor1")?0.23f:0.31f;}}
        public float Capacity {get{return Has("retention")?100:60;}}
        public int Season {get{return (int)(Data.clock/360)%4;}}
        public bool Rain {get{return Data.clock%210>175;}}
        public string SeasonName {get{return new[]{L10n.T("春 · 萌芽"),L10n.T("夏 · 丰茂"),L10n.T("秋 · 金穗"),L10n.T("冬 · 静养")}[Season];}}
        public VerdantFarm(bool load=true,string saveName="verdant-farm.json")
        {
            this.saveName=saveName;Data=Fresh(); if(load) Load();Fleet=new VerdantFleet(this);
        }
        static FarmSave Fresh()
        {
            var data=new FarmSave();
            for(int i=0;i<16;i++) data.plots.Add(new Plot());
            for(int i=0;i<data.scripts.Length;i++) data.scripts[i]=i==0?VerdantContent.Templates[0]:L10n.T("# 在这里编写你的农场程序\n");
            // A small living field makes the first action legible; no harvested resources are pre-awarded.
            for(int i=0;i<4;i++){data.plots[i].crop="grain";data.plots[i].growth=4+i*2;}
            return data;
        }
        public bool Has(string tech){return Data.tech.Contains(tech);}
        public int Count(string key){var c=Data.metrics.Find(v=>v.key==key);return c==null?0:c.value;}
        public void Add(string key,int amount=1){var c=Data.metrics.Find(v=>v.key==key);if(c==null)Data.metrics.Add(new Counter(key,amount));else c.value+=amount;}
        public bool Unlocked(string crop)
        {
            if(crop=="grain"||crop=="radish")return true;
            if(crop=="berry"||crop=="sunflower")return Has("water");
            return Has(crop);
        }
        public bool Ready(Plot p){var def=VerdantContent.Crop(p.crop);return def!=null&&p.growth>=def.seconds;}
        public void Tick(float dt)
        {
            Data.clock+=dt;
            Data.water=Mathf.Min(Capacity,Data.water+dt*(Rain?3:0.65f));
            float season=new[]{1.1f,1.25f,1f,0.82f}[Season];
            foreach(var p in Data.plots)
            {
                p.moisture=Mathf.Clamp01(p.moisture+dt*(Rain?0.035f:-(Has("retention")?0.0028f:0.0056f)));
                var def=VerdantContent.Crop(p.crop);if(def==null||Ready(p))continue;
                float hydration=p.moisture>0.2f?1.35f:0.68f;
                if((p.crop=="tea"||p.crop=="starbloom"||p.crop=="berry")&&p.moisture<=0.08f)hydration=0.18f;
                p.growth+=dt*hydration*season*(1+p.fertility*0.15f);
            }
            Fleet.Tick(dt);
            if(InChallenge)
            {
                challengeTime+=dt;
                int gained=Count(VerdantContent.ChallengeCrops[challenge])-challengeStart;
                if(gained>=VerdantContent.ChallengeTargets[challenge]) FinishChallenge(true);
                else if(challengeTime>=VerdantContent.ChallengeTimes[challenge]) FinishChallenge(false);
            }
            else CheckAchievements();
        }
        public int Neighbors(string crop,bool mature=false)
        {
            int n=0;int[] dx={1,-1,0,0},dy={0,0,1,-1};
            for(int i=0;i<4;i++)
            {int x=Data.x+dx[i],y=Data.y+dy[i];if(x<0||y<0||x>=Data.size||y>=Data.size)continue;
             var p=Data.plots[y*Data.size+x];if(p.crop==crop&&(!mature||Ready(p)))n++;}
            return n;
        }
        public bool Act(string action,string arg="",bool programmed=false)
        {
            if(Fleet!=null&&!Fleet.Applying&&Array.IndexOf(new[]{"plant","harvest","water","till","fertilize"},action)>=0)
            {var selected=Fleet.Find(Data.activeDrone);if(selected.width*selected.height>1||selected.kind!="general")return Fleet.Work(selected.id,action,arg,programmed);}
            bool success=false; string info="";
            switch(action)
            {
                case "move":
                    int dx=0,dy=0;
                    switch(arg.ToLowerInvariant()){case "east":dx=1;break;case "west":dx=-1;break;case "north":dy=1;break;case "south":dy=-1;break;default:throw new Exception(L10n.T("move() 的方向应为 East / West / North / South"));}
                    if(Data.drones.Count==1&&Fleet.Find(Data.activeDrone).width==1&&Fleet.Find(Data.activeDrone).height==1){Fleet.Find(Data.activeDrone).x=Data.x;Fleet.Find(Data.activeDrone).y=Data.y;}success=Fleet.Move(arg);break;
                case "plant":
                    var def=VerdantContent.Crop(arg);
                    if(def==null)throw new Exception(L10n.T("未知作物：")+arg);
                    if(!Unlocked(arg)){info=L10n.T("请先研究 ")+def.name;break;}
                    if(Here.crop!=""){info=L10n.T("这个格子已有作物");break;}
                    if((arg=="radish"||arg=="pumpkin"||arg=="starbloom")&&!Here.tilled){info=L10n.T("这类作物需要先 till() 翻土");break;}
                    if((arg=="berry"||arg=="tea"||arg=="starbloom")&&Here.moisture<0.2f){info=L10n.T("土壤太干，请先 water()");break;}
                    if(Data.coins<def.cost){info=L10n.T("金币不足；可免费播种稻穗恢复收入");break;}
                    if(arg=="starbloom"&&Data.energy<3){info=L10n.T("光能不足；请先收获向日葵");break;}
                    Data.coins-=def.cost;if(arg=="starbloom")Data.energy-=3;
                    Here.crop=arg;Here.growth=0;Here.regrowth=0;Add("planted");success=true;info=L10n.T("播种 ")+def.name;break;
                case "harvest":
                    if(!Ready(Here)){info=Here.crop==""?L10n.T("这里还没有作物"):L10n.T("作物尚未成熟");break;}
                    var crop=VerdantContent.Crop(Here.crop);
                    int bonus=0;
                    if(crop.id=="flax"&&Neighbors("grain")>0)bonus+=2;
                    if(crop.id=="pumpkin")bonus+=Neighbors("pumpkin",true)*2;
                    if(crop.id=="tea"){bonus+=Neighbors("sunflower")>0?2:0;bonus-=Neighbors("tea")>=2?1:0;}
                    int amount=Mathf.Max(1,crop.yield+(Has("yield1")?1:0)+(Has("yield2")?1:0)+Mathf.FloorToInt(Here.fertility*2)+bonus);
                    int research=crop.research+(crop.id=="starbloom"&&Neighbors("tea")>0?6:0);
                    if(Has("knowledge"))research=Mathf.CeilToInt(research*1.5f);
                    Data.coins+=amount*crop.value;Data.knowledge+=research;
                    Add(crop.id,amount);Add("stock:"+crop.id,amount);Add("harvested",amount);Add("earned",amount*crop.value);
                    if(bonus>0)Add("bonusHarvests");
                    if(crop.id=="sunflower"){int power=Has("solar")?8:4;Data.energy+=power;Add("energyEarned",power);}
                    Here.fertility=Mathf.Max(0,Here.fertility-(Has("fertile")?0.015f:0.03f));
                    if(crop.id=="berry"&&Here.regrowth<2){Here.regrowth++;Here.growth=0;}else{Here.crop="";Here.growth=0;}
                    info=crop.name+" +"+amount+L10n.T("  金币 +")+(amount*crop.value);success=true;break;
                case "till":if(Here.crop==""){Here.tilled=true;success=true;}else info=L10n.T("有作物时无法翻土");break;
                case "water":
                    if(!Has("water")){info=L10n.T("请先研究蓄水系统");break;}
                    if(Data.water<1){info=L10n.T("蓄水正在恢复");break;}
                    Data.water-=1;Here.moisture=1;Add("watered");success=true;break;
                case "fertilize":
                    if(!Has("compost")){info=L10n.T("请先研究循环堆肥");break;}
                    if(Data.coins<2){info=L10n.T("施肥需要 2 金币");break;}
                    if(Here.fertility>=0.95f){info=L10n.T("土壤已有充足肥力");break;}
                    Data.coins-=2;Here.fertility=Mathf.Min(1,Here.fertility+0.4f);Add("fertilized");success=true;break;
                case "wait":success=true;break;
                default:throw new Exception(L10n.T("未知动作：")+action);
            }
            Data.totalActions++;if(programmed)Add("programActions");
            if(info!=""&&(!programmed||!success||action=="harvest"))Say(info);
            Notify();return success;
        }
        public int Metric(string name)
        {
            if(name=="size")return Data.size;
            if(name.StartsWith("tech:"))return Has(name.Substring(5))?1:0;
            return Count(name);
        }
        public ContractDef CurrentContract{get{return Data.contract<VerdantContent.Contracts.Length?VerdantContent.Contracts[Data.contract]:null;}}
        public int ContractCropIndex {get{var c=CurrentContract;if(c==null)return -1;string id=c.metric=="energyEarned"?"sunflower":c.metric;return Array.FindIndex(VerdantContent.Crops,x=>x.id==id);}}
        public int ContractProgress {get{var c=CurrentContract;return c==null?0:ContractCropIndex>=0?Mathf.Max(0,Metric(c.metric)-Data.cropTaskStart):Metric(c.metric);}}
        public bool ContractReady {get{var c=CurrentContract;return c!=null&&ContractProgress>=c.amount;}}
        public bool Claim()
        {
            if(InChallenge){Say(L10n.T("试炼结束后再领取主线委托"));return false;}
            var c=CurrentContract;if(c==null||!ContractReady)return false;
            Data.coins+=c.reward;Data.knowledge+=c.knowledge;Data.contract++;Data.cropTaskStart=ContractCropIndex>=0?Metric(CurrentContract.metric):0;
            Say(L10n.T("委托完成：")+c.title+"  +"+c.reward+L10n.T(" 金币 / +")+c.knowledge+L10n.T(" 研究"));Notify();Save();return true;
        }
        public bool Research(string id)
        {
            var t=VerdantContent.Tech(id);if(t==null||Has(id))return false;
            if(Chapter<t.chapter||t.prerequisite!=""&&!Has(t.prerequisite)){Say(L10n.T("尚未满足章节或前置研究"));return false;}
            if(Data.coins<t.price||Data.knowledge<t.knowledge){Say(L10n.T("金币或研究点不足"));return false;}
            Data.coins-=t.price;Data.knowledge-=t.knowledge;Data.tech.Add(id);
            if(id=="field6")Resize(6);if(id=="field8")Resize(8);
            if(id=="fleet2")Data.droneCapacity=2;if(id=="fleet4")Data.droneCapacity=4;if(id=="fleet8")Data.droneCapacity=8;
            if(id=="body2")Data.footprintTier=Math.Max(Data.footprintTier,1);if(id=="body32")Data.footprintTier=Math.Max(Data.footprintTier,2);if(id=="body3")Data.footprintTier=3;
            Say(L10n.T("研究完成：")+t.name);Notify();Save();return true;
        }
        public void Resize(int size)
        {
            var plots=new List<Plot>();for(int y=0;y<size;y++)for(int x=0;x<size;x++)plots.Add(x<Data.size&&y<Data.size?Data.plots[y*Data.size+x]:new Plot());
            Data.size=size;Data.plots=plots;Notify();
        }
        public void StartChallenge(int index)
        {
            if(InChallenge||index<0||index>=6)return;
            if(index>Chapter){Say(L10n.T("完成对应主线章节后解锁此试炼"));return;}
            mainSnapshot=Clone(Data);challenge=index;challengeTime=0;
            Data=Fresh();Resize(6);Data.coins=2000;Data.knowledge=0;Data.energy=100;Data.water=100;
            Data.tech.Clear();foreach(var t in VerdantContent.Techs)Data.tech.Add(t.id);Data.droneCapacity=8;Data.footprintTier=3;VerdantFleet.Ensure(Data);
            foreach(var p in Data.plots){p.crop="";p.growth=0;p.tilled=false;}
            challengeStart=Count(VerdantContent.ChallengeCrops[index]);
            Say(L10n.T("试炼开始：")+VerdantContent.ChallengeNotes[index]);Notify();
        }
        public void FinishChallenge(bool won)
        {
            if(!InChallenge)return;int index=challenge;
            int stars=won?(challengeTime<VerdantContent.ChallengeTimes[index]*0.65f?3:challengeTime<VerdantContent.ChallengeTimes[index]*0.85f?2:1):0;
            Data=mainSnapshot;mainSnapshot=null;challenge=-1;
            if(won){bool first=Data.stars[index]==0;Data.stars[index]=Mathf.Max(Data.stars[index],stars);if(first){Data.coins+=400;Data.knowledge+=50;}Say(L10n.T("试炼通过！获得 ")+stars+L10n.T(" 星，已返回主线农场。"));}
            else Say(L10n.T("试炼已结束，主线农场和物资已恢复。可调整程序后再次尝试。"));
            Notify();Save();
        }
        void CheckAchievements()
        {
            bool[] done={Count("harvested")>=1,Count("harvested")>=100,Count("harvested")>=1000,Count("programActions")>=100,Count("programActions")>=5000,Count("watered")>=100,Count("bonusHarvests")>=100,Count("moves")>=10000,Data.size>=8,Count("starbloom")>=100,Data.tech.Count>=VerdantContent.Techs.Length,Array.TrueForAll(Data.stars,x=>x>0)};
            for(int i=0;i<done.Length;i++)if(done[i]&&!Data.achievements.Contains(i.ToString())){Data.achievements.Add(i.ToString());Data.knowledge+=10;Say(L10n.T("成就解锁：")+VerdantContent.AchievementNames[i]);}
        }
        public void Say(string s){if(Message!=null)Message(s);}
        public void Notify(){if(Changed!=null)Changed();}
        public void Save()
        {
            try
            {
                Directory.CreateDirectory(Application.persistentDataPath);
                string json=JsonUtility.ToJson(InChallenge?mainSnapshot:Data,true);
                string temp=SavePath+".tmp";File.WriteAllText(temp,json);
                if(File.Exists(SavePath))File.Copy(SavePath,SavePath+".bak",true);
                File.Copy(temp,SavePath,true);File.Delete(temp);
                saveState="已保存 ";savedAt=DateTime.Now.ToString("HH:mm");
            }catch(Exception e){saveState=L10n.T("保存失败");Say(L10n.T("保存失败：")+e.Message);}
        }
        void Load()
        {
            if(!File.Exists(SavePath))return;
            try{Data=ReadValidated(SavePath);saveState=L10n.T("已恢复存档");}
            catch(Exception){try{Data=ReadValidated(SavePath+".bak");saveState=L10n.T("已恢复备份存档");}catch(Exception){Data=Fresh();saveState=L10n.T("存档不可读取，已进入新农场");}}
        }
        static FarmSave ReadValidated(string path)
        {
            var s=JsonUtility.FromJson<FarmSave>(File.ReadAllText(path));
            if(s==null||s.schema!=1||(s.size!=4&&s.size!=6&&s.size!=8)||s.plots==null||s.plots.Count!=s.size*s.size||s.tech==null||s.metrics==null||s.achievements==null||s.scripts==null||s.scripts.Length<1||s.stars==null||s.stars.Length!=6||s.contract<0||s.contract>30||s.slot<0||s.slot>=s.scripts.Length||s.selectedCrop<0||s.selectedCrop>=8)throw new Exception(L10n.T("存档格式不匹配"));
            s.x=Mathf.Clamp(s.x,0,s.size-1);s.y=Mathf.Clamp(s.y,0,s.size-1);
            foreach(var p in s.plots)if(p==null||p.crop!=""&&VerdantContent.Crop(p.crop)==null)throw new Exception(L10n.T("无效地块"));
            VerdantFleet.Ensure(s);
            var ids=new HashSet<int>();foreach(var u in s.drones)
            {if(!ids.Add(u.id)||u.width<1||u.height<1||u.width>3||u.height>3||u.x<0||u.y<0||u.x+u.width>s.size||u.y+u.height>s.size||u.jobs.Count>64)throw new Exception(L10n.T("无人机存档无效"));}
            s.fleetPaused=s.drones.Exists(u=>u.jobs.Count>0);return s;
        }
        static FarmSave Clone(FarmSave save){return JsonUtility.FromJson<FarmSave>(JsonUtility.ToJson(save));}
    }
}
