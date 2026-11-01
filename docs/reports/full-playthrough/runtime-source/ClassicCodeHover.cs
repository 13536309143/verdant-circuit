using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Verdant.Automation
{
    public sealed class ClassicCodeHover:MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public TMP_InputField field;public ClassicController controller;bool inside;
        public void OnPointerEnter(PointerEventData data){inside=true;}
        public void OnPointerExit(PointerEventData data){inside=false;controller.VariableHover(null,Vector2.zero);}
        void Update()
        {
            if(!inside||controller==null||field==null)return;int index=TMP_TextUtilities.FindIntersectingWord(field.textComponent,Input.mousePosition,null);if(index<0||index>=field.textComponent.textInfo.wordCount){controller.VariableHover(null,Vector2.zero);return;}
            string word=field.textComponent.textInfo.wordInfo[index].GetWord();controller.VariableHover(word,Input.mousePosition);
        }
    }
    public sealed partial class ClassicController
    {
        TMP_Text hoverText;RectTransform hoverPanel;
        public void VariableHover(string word,Vector2 position)
        {
            if(word==null){if(hoverPanel!=null)hoverPanel.gameObject.SetActive(false);return;}
            var task=Session.FocusedDrone?.task;object value;if(task==null||!task.CurrentScope.Try(word,out value)){if(hoverPanel!=null)hoverPanel.gameObject.SetActive(false);return;}
            if(hoverPanel==null){hoverPanel=Panel("Variable Value",canvas,new Vector2(.5f,.5f),new Vector2(.5f,.5f),Dark);hoverPanel.pivot=new Vector2(0,1);hoverPanel.sizeDelta=new Vector2(320,80);hoverText=Label("",hoverPanel,new Vector2(.03f,.04f),new Vector2(.97f,.96f),15,Color.white);}
            Vector2 local;RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas,position,null,out local);local+=new Vector2(16,-16);local.x=Mathf.Clamp(local.x,-canvas.rect.width*.5f,canvas.rect.width*.5f-320);local.y=Mathf.Clamp(local.y,-canvas.rect.height*.5f+80,canvas.rect.height*.5f);hoverPanel.anchoredPosition=local;hoverPanel.gameObject.SetActive(true);hoverPanel.SetAsLastSibling();hoverText.text=word+" = "+ScriptValue.Show(value);
        }
    }
}
