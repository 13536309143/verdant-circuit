using UnityEngine;

namespace Verdant.Automation
{
    public sealed partial class ClassicController
    {
        void BuildVolumeSetting()
        {
            var row=Surface("Audio Setting",modalBody,Vector2.zero,Vector2.one,new Color(Muted.r,Muted.g,Muted.b,.15f));
            row.gameObject.AddComponent<UnityEngine.UI.LayoutElement>().preferredHeight=140;
            Label(Tr("Audio","声音"),row,new Vector2(.025f,.73f),new Vector2(.79f,.98f),21,Ink);
            var percentage=Label(Mathf.RoundToInt(Volume*100)+"%",row,new Vector2(.81f,.73f),new Vector2(.975f,.98f),21,Accent);
            percentage.alignment=TMPro.TextAlignmentOptions.Right;
            Label(Tr("Drag to adjust music and effects.","拖动调整音乐与音效。"),row,new Vector2(.025f,.52f),new Vector2(.975f,.73f),14,Muted);
            var root=Panel("Volume Slider",row,new Vector2(.025f,.14f),new Vector2(.975f,.51f),Color.clear);
            root.GetComponent<UnityEngine.UI.Image>().raycastTarget=true;
            var slider=root.gameObject.AddComponent<UnityEngine.UI.Slider>();
            slider.minValue=0;slider.maxValue=1;slider.wholeNumbers=false;slider.direction=UnityEngine.UI.Slider.Direction.LeftToRight;
            Surface("Track",root,new Vector2(.015f,.42f),new Vector2(.985f,.58f),Muted);
            var fillArea=Panel("Fill Area",root,new Vector2(.015f,.42f),new Vector2(.985f,.58f),Color.clear);
            slider.fillRect=Surface("Fill",fillArea,Vector2.zero,Vector2.one,Accent);
            var handleArea=Panel("Handle Area",root,new Vector2(.015f,.5f),new Vector2(.985f,.5f),Color.clear);
            var handle=Surface("Handle",handleArea,new Vector2(.5f,.5f),new Vector2(.5f,.5f),Lime,true);handle.sizeDelta=new Vector2(28,28);
            var handleImage=handle.GetComponent<UnityEngine.UI.Image>();handleImage.raycastTarget=true;
            slider.handleRect=handle;slider.targetGraphic=handleImage;slider.SetValueWithoutNotify(Volume);
            var commit=root.gameObject.AddComponent<VerdantVolumeCommit>();commit.Commit=Save;
            slider.onValueChanged.AddListener(value=>
            {
                Volume=Mathf.Clamp01(value);Farm.Data.volume=Volume;Audio.SetVolume(Volume);
                percentage.text=Mathf.RoundToInt(Volume*100)+"%";commit.Changed();
            });
            Label(Tr("0% · Mute","0% · 静音"),row,new Vector2(.025f,.015f),new Vector2(.5f,.15f),12,Muted);
            Label("100%",row,new Vector2(.7f,.015f),new Vector2(.975f,.15f),12,Muted).alignment=TMPro.TextAlignmentOptions.Right;
        }
    }
}
