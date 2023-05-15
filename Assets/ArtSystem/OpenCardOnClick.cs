using System;
using UnityEngine;
using YGOSharp;

public class OpenCardOnClick : MonoBehaviour
{
    private void OnClick()
    {
        var lbl = GetComponent<UILabel>();

        if (lbl != null)
            try
            {
                var s = lbl.GetUrlAtPosition(UICamera.lastWorldPosition);
                if (string.IsNullOrEmpty(s)) return;
                var code = int.Parse(s);
                Program.I().cardDescription.setData(CardsManager.Get(code), GameTextureManager.myBack, "", true);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
    }
}