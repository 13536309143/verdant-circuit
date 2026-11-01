using System.Collections;
using TMPro;
using UnityEngine;
namespace Verdant
{
    public sealed class VerdantApp:MonoBehaviour
    {
        public TMP_FontAsset uiFont,codeFont;
        IEnumerator Start()
        {
            yield return L10n.Initialize();Application.targetFrameRate=60;
            var controller=new GameObject("Verdant Automation").AddComponent<Automation.ClassicController>();
            controller.transform.SetParent(transform);controller.Initialize();
        }
    }
}
