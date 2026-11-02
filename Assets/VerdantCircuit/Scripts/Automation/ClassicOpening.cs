using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Verdant.Automation
{
    /// <summary>A self-contained illustrated prologue. It never reads or writes farm state.</summary>
    public sealed class ClassicOpening : MonoBehaviour
    {
        public const float Duration=15.5f;
        public float Elapsed {get;private set;}
        public bool Playing {get;private set;}
        public Canvas Screen {get;private set;}
        public Camera Camera {get;private set;}
        public event Action Finished;
        ClassicOpeningArt art;
        CanvasGroup group,storyGroup,titleGroup;
        TMP_Text chapter,story,code,title,tagline,skipLabel,footer;
        RectTransform storyRect,titleRect,progress;
        TMP_FontAsset font;
        bool finished;
        int lastChapter=-1;
        static Color C(string hex){return ClassicView.ColorOf(hex);}
        static string Tr(string english,string chinese){return L10n.T(chinese);}

        public void Initialize(TMP_FontAsset uiFont)
        {
            font=uiFont;Playing=true;
            gameObject.layer=5;
            var cameraObject=new GameObject("Opening Camera",typeof(Camera));cameraObject.transform.SetParent(transform,false);Camera=cameraObject.GetComponent<Camera>();
            Camera.clearFlags=CameraClearFlags.SolidColor;Camera.backgroundColor=C("102822");Camera.cullingMask=1<<5;Camera.depth=100;Camera.orthographic=true;Camera.nearClipPlane=.1f;Camera.farClipPlane=10;
            var screen=new GameObject("Opening Canvas",typeof(RectTransform),typeof(Canvas),typeof(UnityEngine.UI.CanvasScaler),typeof(UnityEngine.UI.GraphicRaycaster),typeof(CanvasGroup));screen.layer=5;screen.transform.SetParent(transform,false);
            Screen=screen.GetComponent<Canvas>();Screen.renderMode=RenderMode.ScreenSpaceCamera;Screen.worldCamera=Camera;Screen.planeDistance=1;Screen.sortingOrder=200;
            var scaler=screen.GetComponent<UnityEngine.UI.CanvasScaler>();scaler.uiScaleMode=UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1600,1000);scaler.matchWidthOrHeight=.5f;
            group=screen.GetComponent<CanvasGroup>();group.blocksRaycasts=true;
            var background=Box("Background",screen.transform,Vector2.zero,Vector2.one,C("102822"));background.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var illustration=new GameObject("Living Circuit",typeof(RectTransform),typeof(CanvasRenderer),typeof(ClassicOpeningArt));illustration.layer=5;illustration.transform.SetParent(screen.transform,false);Stretch((RectTransform)illustration.transform,Vector2.zero,Vector2.one);art=illustration.GetComponent<ClassicOpeningArt>();art.raycastTarget=false;
            Text("FIELDWORKS  /  INTERACTIVE SYSTEMS",screen.transform,new Vector2(.055f,.91f),new Vector2(.8f,.95f),15,C("9DB4A6"));
            footer=Text("",screen.transform,new Vector2(.055f,.046f),new Vector2(.7f,.085f),15,C("B8CBBB"));
            var storyRoot=Box("Story",screen.transform,new Vector2(.065f,.39f),new Vector2(.49f,.82f),Color.clear);storyRect=storyRoot;storyGroup=storyRoot.gameObject.AddComponent<CanvasGroup>();
            chapter=Text("",storyRoot,new Vector2(0,.8f),new Vector2(1,.93f),15,C("D4E69A"));
            story=Text("",storyRoot,new Vector2(0,.40f),new Vector2(1,.78f),43,C("F2F2DC"));
            code=Text("",storyRoot,new Vector2(0,0),new Vector2(.95f,.33f),21,C("A1CCB2"));code.font=L10n.CodeFont??font;code.enableWordWrapping=false;
            var titleRoot=Box("Title",screen.transform,new Vector2(.065f,.29f),new Vector2(.66f,.79f),Color.clear);titleRect=titleRoot;titleGroup=titleRoot.gameObject.AddComponent<CanvasGroup>();
            Text("A FIELD OF POSSIBILITIES",titleRoot,new Vector2(0,.81f),new Vector2(1,.94f),15,C("D4E69A"));
            title=Text("",titleRoot,new Vector2(-.006f,.25f),new Vector2(1,.83f),74,C("F4F2DA"));title.fontStyle=FontStyles.Bold;title.characterSpacing=3;
            tagline=Text("",titleRoot,new Vector2(0,.02f),new Vector2(.9f,.2f),24,C("C1D5C3"));
            var skip=Box("Skip Opening",screen.transform,new Vector2(.81f,.045f),new Vector2(.95f,.095f),C("29453A"));skip.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var button=skip.gameObject.AddComponent<UnityEngine.UI.Button>();button.targetGraphic=skip.GetComponent<UnityEngine.UI.Image>();button.onClick.AddListener(Skip);
            skipLabel=Text("",skip,new Vector2(.04f,.08f),new Vector2(.96f,.92f),16,C("F2F2DC"));skipLabel.alignment=TextAlignmentOptions.Center;
            var track=Box("Timeline",screen.transform,new Vector2(.055f,.025f),new Vector2(.945f,.0275f),C("355042"));progress=Box("Progress",track,Vector2.zero,Vector2.one,C("D4E69A"));progress.anchorMax=new Vector2(0,1);
            L10n.Changed+=Localize;Localize();RenderFrame(0);
        }
        void Localize()
        {
            font=L10n.UIFont??font;foreach(var text in Screen.GetComponentsInChildren<TMP_Text>())text.font=text==code?(L10n.CodeFont??font):font;
            title.text=Tr("VERDANT\nCIRCUIT","禾 序");tagline.text=Tr("Cultivate a living algorithm.","让每一行代码，长出新的可能。");
            skipLabel.text=Tr("Skip  /  Esc","跳过  /  Esc");footer.text=Tr("One plot. One instruction. A world in motion.","一方田，一行代码，一个运转的世界。");lastChapter=-1;RenderFrame(Elapsed);
        }
        void Update()
        {
            if(!Playing)return;
            if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Return)){Skip();return;}
            if(Input.GetKeyDown(KeyCode.F8))L10n.Set(L10n.Code=="en"?"zh-Hans":"en");
            Advance(Time.unscaledDeltaTime);
        }
        public void Advance(float seconds)
        {
            if(!Playing)return;Elapsed=Mathf.Clamp(Elapsed+Mathf.Max(0,seconds),0,Duration);RenderFrame(Elapsed);if(Elapsed>=Duration)Complete();
        }
        // Also used by the Editor capture check: this changes only illustration state.
        public void RenderFrame(float seconds)
        {
            float t=Mathf.Clamp(seconds,0,Duration);if(art==null)return;art.Seconds=t;art.SetVerticesDirty();
            int act=t<4?0:t<8?1:2;
            if(lastChapter!=act)
            {
                lastChapter=act;
                chapter.text=act==0?Tr("01  /  THE FIRST INSTRUCTION","01  /  第一条指令"):act==1?Tr("02  /  FIND YOUR RHYTHM","02  /  找到节奏"):Tr("03  /  LET IT GROW","03  /  生生不息");
                story.text=act==0?Tr("A quiet field.\nA spark of intent.","静静的田野，\n等待第一条指令。"):act==1?Tr("Write a pattern.\nWatch it take root.","写下规律，\n让想法生根。"):Tr("Small instructions.\nLiving possibilities.","微小的指令，\n生长的可能。");
            }
            string source=t<4?"harvest()":t<8?"while True:\n    harvest()\n    move(East)":"for row in range(get_world_size()):\n    harvest()\n    move(North)";
            float local=t<4?t:t<8?t-4:t-8;code.text=source;code.maxVisibleCharacters=Mathf.Clamp((int)(local*32),0,source.Length);
            float titleIn=Mathf.SmoothStep(0,1,Mathf.Clamp01((t-10.2f)/1.3f));storyGroup.alpha=Mathf.Clamp01(t/.8f)*(1-titleIn);titleGroup.alpha=titleIn;
            storyRect.anchoredPosition=new Vector2(0,Mathf.Lerp(-14,0,Mathf.Clamp01(t/1.5f)));titleRect.anchoredPosition=new Vector2(0,Mathf.Lerp(-20,0,titleIn));
            group.alpha=Mathf.Clamp01(t/.65f)*(1-Mathf.Clamp01((t-(Duration-.65f))/.65f));progress.anchorMax=new Vector2(t/Duration,1);
        }
        public void Skip(){if(Playing)Complete();}
        void Complete()
        {
            if(finished)return;finished=true;Playing=false;group.blocksRaycasts=false;Screen.gameObject.SetActive(false);Camera.gameObject.SetActive(false);
            if(EventSystem.current!=null)EventSystem.current.SetSelectedGameObject(null);if(Finished!=null)Finished();Destroy(gameObject);
        }
        void OnDisable(){if(Playing)Complete();}
        void OnDestroy(){L10n.Changed-=Localize;}
        static void Stretch(RectTransform r,Vector2 min,Vector2 max){r.anchorMin=min;r.anchorMax=max;r.offsetMin=r.offsetMax=Vector2.zero;}
        static RectTransform Box(string name,Transform parent,Vector2 min,Vector2 max,Color color)
        {
            var o=new GameObject(name,typeof(RectTransform),typeof(UnityEngine.UI.Image));o.layer=5;o.transform.SetParent(parent,false);var r=(RectTransform)o.transform;Stretch(r,min,max);var image=o.GetComponent<UnityEngine.UI.Image>();image.color=color;image.raycastTarget=false;return r;
        }
        TMP_Text Text(string value,Transform parent,Vector2 min,Vector2 max,int size,Color color)
        {
            var o=new GameObject("Caption",typeof(RectTransform),typeof(TextMeshProUGUI));o.layer=5;o.transform.SetParent(parent,false);Stretch((RectTransform)o.transform,min,max);var text=o.GetComponent<TextMeshProUGUI>();text.font=font;text.fontSize=size;text.enableAutoSizing=true;text.fontSizeMax=size;text.fontSizeMin=size*.72f;text.color=color;text.text=value;text.raycastTarget=false;text.alignment=TextAlignmentOptions.MidlineLeft;return text;
        }
    }
}
