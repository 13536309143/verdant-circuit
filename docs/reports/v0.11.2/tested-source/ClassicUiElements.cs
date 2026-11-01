using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Verdant.Automation
{
    public sealed class ClassicButtonMotion:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        bool inside;
        public void OnPointerEnter(PointerEventData e){inside=true;}
        public void OnPointerExit(PointerEventData e){inside=false;}
        void OnDisable(){inside=false;transform.localScale=Vector3.one;}
        void Update(){transform.localScale=Vector3.Lerp(transform.localScale,Vector3.one*(inside?1.025f:1),1-Mathf.Exp(-18*Time.unscaledDeltaTime));}
    }

    public sealed class ClassicLineNumbers:MonoBehaviour
    {
        public TMP_InputField field;
        public TMP_Text label;
        string previous;
        void LateUpdate()
        {
            if(field==null||label==null)return;
            if(previous!=field.text)
            {
                previous=field.text;int lines=1;
                foreach(char c in previous)if(c=='\n')lines++;
                var text=new System.Text.StringBuilder();
                for(int i=1;i<=Math.Min(lines,1500);i++){if(i>1)text.Append('\n');text.Append(i);}
                label.text=text.ToString();
            }
            var source=field.textComponent.rectTransform;
            label.font=field.textComponent.font;label.fontSize=field.textComponent.fontSize;
            label.rectTransform.anchoredPosition=new Vector2(0,source.anchoredPosition.y);
            label.rectTransform.sizeDelta=new Vector2(label.rectTransform.sizeDelta.x,source.sizeDelta.y);
        }
    }

    public sealed class ClassicSprout:UnityEngine.UI.MaskableGraphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();var r=GetPixelAdjustedRect();
            Add(vh,r,new Vector2(.46f,.15f),new Vector2(.53f,.64f),new Vector2(.58f,.64f),new Vector2(.53f,.15f));
            Add(vh,r,new Vector2(.5f,.48f),new Vector2(.13f,.58f),new Vector2(.17f,.85f),new Vector2(.5f,.71f));
            Add(vh,r,new Vector2(.54f,.62f),new Vector2(.83f,.91f),new Vector2(.95f,.68f),new Vector2(.57f,.47f));
        }
        void Add(VertexHelper vh,Rect r,params Vector2[] p)
        {int start=vh.currentVertCount;foreach(var v in p)vh.AddVert(new Vector3(r.x+v.x*r.width,r.y+v.y*r.height),color,Vector2.zero);vh.AddTriangle(start,start+1,start+2);vh.AddTriangle(start,start+2,start+3);}
    }

    public sealed class ClassicTreeScroll:UnityEngine.UI.ScrollRect
    {
        public float Zoom {get{return content.localScale.x;}}
        public override void OnScroll(PointerEventData e)
        {
            Vector2 local;RectTransformUtility.ScreenPointToLocalPointInRectangle(content,e.position,e.pressEventCamera,out local);
            Vector3 before=content.TransformPoint(local);SetZoom(Zoom*Mathf.Exp(e.scrollDelta.y*.10f));
            content.position+=before-content.TransformPoint(local);StopMovement();
        }
        public void SetZoom(float zoom){content.localScale=Vector3.one*Mathf.Clamp(zoom,.24f,1.35f);StopMovement();}
        public void Focus(Vector2 topLeft,float zoom)
        {SetZoom(zoom);content.anchoredPosition=new Vector2(-topLeft.x*Zoom+24,topLeft.y*Zoom-24);StopMovement();}
        public void Fit(){Canvas.ForceUpdateCanvases();Focus(Vector2.zero,Mathf.Min(viewport.rect.width/content.rect.width,viewport.rect.height/content.rect.height)*.97f);}
    }
}
