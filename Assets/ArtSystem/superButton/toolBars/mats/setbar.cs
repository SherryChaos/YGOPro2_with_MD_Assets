﻿using System;
using UnityEngine;

public class setbar : MonoBehaviour
{
    public void show()
    {
        try
        {
            if (Program.I().setting.isShowed)
                Program.I().setting.hide();
            else
                Program.I().setting.show();
        }
        catch (Exception)
        {
        }
    }

    public void book()
    {
        try
        {
            if (Program.I().book.isShowed)
                Program.I().book.hide();
            else
                Program.I().book.show();
        }
        catch (Exception)
        {
        }
    }
}