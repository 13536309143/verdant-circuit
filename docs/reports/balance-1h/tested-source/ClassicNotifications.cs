using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Verdant.Automation
{
    [Serializable]
    public sealed class CompletionNotice
    {
        public string kind,id;
        public double seconds;
    }

    public sealed partial class ClassicController
    {
        RectTransform completionOverlay;
        TMP_Text completionDetail;
        bool CompletionOpen {get{return completionOverlay!=null;}}
        List<CompletionNotice> PendingCompletions
        {
            get
            {
                if(Farm.Data.completionNotices==null)Farm.Data.completionNotices=new List<CompletionNotice>();
                return Farm.Data.completionNotices;
            }
        }

        void NotifyChallengeCompleted(string category,double seconds)
        {
            PendingCompletions.Add(new CompletionNotice{kind="race",id=category,seconds=seconds});
            Save();
        }

        void TickCompletionNotice()
        {
            if(PendingCompletions.Count==0)return;
            if(!CompletionOpen)
            {
                // Leave the current dialog intact and display its pending notice after closing it.
                if(modal!=null)return;
                Editor.DeactivateInputField();
                foreach(var field in codeWindows.Values)if(field!=null)field.DeactivateInputField();
                completionOverlay=Panel("Task Completion",canvas,Vector2.zero,Vector2.one,new Color(0,0,0,.45f));
                completionOverlay.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
                var card=Panel("Completion Card",completionOverlay,new Vector2(.23f,.22f),new Vector2(.77f,.78f),Paper);
                Label(Tr("Task completed","任务已完成"),card,new Vector2(.06f,.79f),new Vector2(.94f,.95f),32,Accent);
                completionDetail=Label("",card,new Vector2(.06f,.25f),new Vector2(.94f,.78f),21,Ink);
                completionDetail.enableWordWrapping=true;
                Button(Tr("View records","查看纪录"),card,new Vector2(.06f,.07f),new Vector2(.47f,.19f),()=>{DismissCompletion();Open("records");});
                Button(Tr("Got it","知道了"),card,new Vector2(.53f,.07f),new Vector2(.94f,.19f),DismissCompletion);
                Audio.PlayCue(880);
            }
            completionDetail.text=string.Join("\n",PendingCompletions.Take(6).Select(n=>n.kind=="achievement"
                ?Tr("Achievement: ","成就：")+AchievementLabel(n.id)
                :Tr("Challenge: ","竞速：")+ChallengeName(n.id)+" / "+n.seconds.ToString("F3")+"s"));
            if(PendingCompletions.Count>6)completionDetail.text+="\n"+Tr("More completed goals: ","其他已完成目标：")+(PendingCompletions.Count-6);
            completionDetail.text+="\n\n"+Tr("Saved automatically. View Records for details.","已自动记录，详细结果可在纪录中查看。");
            completionOverlay.SetAsLastSibling();
        }

        static string ChallengeName(string category)
        {
            string id=category.Replace("_","").ToLowerInvariant();bool single=id.EndsWith("single");if(single)id=id.Substring(0,id.Length-6);
            switch(id){case "carrots":id="carrot";break;case "pumpkins":id="pumpkin";break;case "sunflowers":id="power";break;case "maze":id="gold";break;case "dinosaur":id="bone";break;case "fastestreset":id="reset";break;}
            string name=id=="reset"?Tr("Fastest reset","最快重置"):ClassicFarm.Items.Contains(id)?ItemName(id):category;
            return name+(single?Tr(" (single drone)","（单机）"):"");
        }

        void DismissCompletion()
        {
            if(!CompletionOpen)return;
            PendingCompletions.Clear();
            completionOverlay.gameObject.SetActive(false);
            Destroy(completionOverlay.gameObject);completionOverlay=null;completionDetail=null;
            Save();
        }
    }
}
