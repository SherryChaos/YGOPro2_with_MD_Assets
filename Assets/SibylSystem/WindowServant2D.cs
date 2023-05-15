using DG.Tweening;
using UnityEngine;

public class WindowServant2D : Servant
{
    public override void applyHideArrangement()
    {
        if (gameObject != null)
        {
            UIHelper.clearITWeen(gameObject);
            gameObject.transform.DOMove(
                Program.I().camera_main_2d.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 1.5f, 0)),
                0.6f);
        }
    }

    public override void applyShowArrangement()
    {
        if (gameObject != null)
        {
            UIHelper.clearITWeen(gameObject);
            gameObject.transform.DOMove(
                Program.I().camera_main_2d.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)),
                0.6f);
        }
    }

    public override void hide()
    {
        base.hide();
        Program.ShiftUIenabled(Program.I().ui_main_3d, true);
    }

    public override void show()
    {
        base.show();
        Program.ShiftUIenabled(Program.I().ui_main_3d, false);
    }

    public static GameObject SetWindow(Servant servant, GameObject mod)
    {
        var re = mod;
        UIHelper.InterGameObject(re);
        return re;
    }
}