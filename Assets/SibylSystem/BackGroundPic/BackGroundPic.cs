using System.IO;
using UnityEngine;

public class BackGroundPic : Servant
{
    private GameObject backGround;

    public override void initialize()
    {
        backGround = Program.I().mod_simple_ngui_background_texture;
        if (!File.Exists("texture/common/desk.jpg")) return;
        backGround.SetActive(true);
        backGround.GetComponent<UITexture>().mainTexture = UIHelper.GetTexture2D("texture/common/desk.jpg");
        backGround.GetComponent<UITexture>().depth = -100;
    }

    public override void applyShowArrangement()
    {
        if (!backGround.activeInHierarchy) return;
        var component = backGround.GetComponent<UITexture>();
        var texture = component.mainTexture;

        if (texture.width <= texture.height * Screen.width / Screen.height)
        {
            // 图窄屏幕宽，用宽度
            component.width = Utils.UIWidth() + 2;
            component.height = component.width * texture.height / texture.width;
        }
        else
        {
            // 图宽屏幕窄，用高度
            component.height = Utils.UIHeight() + 2;
            component.width = component.height * texture.width / texture.height;
        }
    }

    public override void applyHideArrangement()
    {
        applyShowArrangement();
    }
}