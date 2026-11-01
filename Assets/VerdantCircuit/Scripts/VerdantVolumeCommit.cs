using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Verdant
{
    public sealed class VerdantVolumeCommit:MonoBehaviour,IPointerUpHandler,IEndDragHandler,IDeselectHandler
    {
        public Action Commit;
        bool pending;
        public void Changed(){pending=true;}
        public void Flush(){if(!pending)return;pending=false;Commit?.Invoke();}
        public void OnPointerUp(PointerEventData data){Flush();}
        public void OnEndDrag(PointerEventData data){Flush();}
        public void OnDeselect(BaseEventData data){Flush();}
        void OnDisable(){Flush();}
    }
}
