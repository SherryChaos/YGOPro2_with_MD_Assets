﻿using UnityEngine;

public class TextMaster
{
    private readonly GameObject gameObject;

    public TextMaster(string hint, Vector3 position, bool isWorld)
    {
        if (isWorld)
        {
            gameObject = Program.I().ocgcore.create_s(
                Program.I().mod_simple_ngui_text,
                position,
                new Vector3(70, 0, 0),
                true,
                Program.I().ui_main_3d,
                false
            );
            UIHelper.clearITWeen(gameObject);
            iTween.ScaleTo(gameObject, new Vector3(0.03f, 0.03f, 0.03f), 0.6f);
        }
        else
        {
            gameObject = Program.I().ocgcore.create_s(
                Program.I().mod_simple_ngui_text,
                Program.I().camera_main_2d.ScreenToWorldPoint(position),
                new Vector3(0, 0, 0),
                true,
                Program.I().ui_main_2d
            );
        }

        UIHelper.trySetLableText(gameObject, hint);
    }

    public void dispose()
    {
        Program.I().ocgcore.destroy(gameObject, 0.6f, true);
    }
}