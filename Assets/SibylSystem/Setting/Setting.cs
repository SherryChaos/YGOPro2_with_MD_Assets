using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Setting : WindowServant2D
{
    public int atk = 1800;
    public int star = 5;
    private EventDelegate onChange;

    public LAZYsetting setting;
    private UISlider sliderAlpha;
    private UISlider sliderSize;
    private UISlider sliderVsize;
    private UIPopupList _screen;

    public override void initialize()
    {
        gameObject = SetWindow(this, Program.I().new_ui_setting);
        setting = gameObject.GetComponentInChildren<LAZYsetting>();

        _screen = UIHelper.getByName<UIPopupList>(gameObject, "screen_");
        UIHelper.registEvent(gameObject, "exit_", onClickExit);
        UIHelper.registEvent(gameObject, "screen_", resizeScreen);
        UIHelper.registEvent(gameObject, "full_", resizeScreen);
        // UIHelper.registEvent(gameObject, "resize_", resizeScreen);
        UIHelper.getByName<UIToggle>(gameObject, "full_").value = Screen.fullScreen;
        UIHelper.getByName<UIToggle>(gameObject, "ignoreWatcher_").value =
            UIHelper.fromStringToBool(Config.Get("ignoreWatcher_", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "ignoreOP_").value =
            UIHelper.fromStringToBool(Config.Get("ignoreOP_", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "smartSelect_").value =
            UIHelper.fromStringToBool(Config.Get("smartSelect_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "autoChain_").value =
            UIHelper.fromStringToBool(Config.Get("autoChain_", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "handPosition_").value =
            UIHelper.fromStringToBool(Config.Get("handPosition_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "handmPosition_").value =
            UIHelper.fromStringToBool(Config.Get("handmPosition_", "1"));
        UIHelper.getByName<UIToggle>(gameObject, "spyer_").value = UIHelper.fromStringToBool(Config.Get("spyer_", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "resize_").canChange = false;
        // UIHelper.getByName<UIToggle>(gameObject, "resize_").value =
        // UIHelper.fromStringToBool(Config.Get("resize_", "0"));
        UIHelper.getByName<UIToggle>(gameObject, "longField_").value =
            UIHelper.fromStringToBool(Config.Get("longField_", "0"));
        if (QualitySettings.GetQualityLevel() < 3)
            UIHelper.getByName<UIToggle>(gameObject, "high_").value = false;
        else
            UIHelper.getByName<UIToggle>(gameObject, "high_").value = true;
        UIHelper.registEvent(gameObject, "ignoreWatcher_", save);
        UIHelper.registEvent(gameObject, "ignoreOP_", save);
        UIHelper.registEvent(gameObject, "smartSelect_", save);
        UIHelper.registEvent(gameObject, "autoChain_", save);
        UIHelper.registEvent(gameObject, "handPosition_", save);
        UIHelper.registEvent(gameObject, "handmPosition_", save);
        UIHelper.registEvent(gameObject, "spyer_", save);
        UIHelper.registEvent(gameObject, "high_", save);
        UIHelper.registEvent(gameObject, "longField_", onChangeLongField);
        UIHelper.registEvent(gameObject, "size_", onChangeBgmVol);
        //UIHelper.registEvent(gameObject, "alpha_", onChangeAlpha);
        UIHelper.registEvent(gameObject, "vSize_", onChangeVsize);
        sliderSize = UIHelper.getByName<UISlider>(gameObject, "size_");
        //sliderAlpha = UIHelper.getByName<UISlider>(gameObject, "alpha_");
        sliderVsize = UIHelper.getByName<UISlider>(gameObject, "vSize_");
        Program.go(2000, readVales);
        var collection = gameObject.GetComponentsInChildren<UIToggle>();
        for (var i = 0; i < collection.Length; i++)
            if (collection[i].name.Length > 0 && collection[i].name[0] == '*')
            {
                if (collection[i].name == "*MonsterCloud")
                    collection[i].value = UIHelper.fromStringToBool(Config.Get(collection[i].name, "0"));
                else
                    collection[i].value = UIHelper.fromStringToBool(Config.Get(collection[i].name, "1"));
            }

        setting.showoffATK.value = Config.Get("showoffATK", "1800");
        setting.showoffStar.value = Config.Get("showoffStar", "5");
        UIHelper.registEvent(setting.showoffATK.gameObject, onchangeClose);
        UIHelper.registEvent(setting.showoffStar.gameObject, onchangeClose);
        UIHelper.registEvent(setting.mouseEffect.gameObject, onchangeMouse);
        UIHelper.registEvent(setting.closeUp.gameObject, onchangeCloseUp);
        UIHelper.registEvent(setting.cloud.gameObject, onchangeCloud);
        UIHelper.registEvent(setting.Vpedium.gameObject, onCP);
        UIHelper.registEvent(setting.Vfield.gameObject, onCP);
        UIHelper.registEvent(setting.Vlink.gameObject, onCP);

        UIHelper.registEvent(gameObject, onchangeClose);
        UIHelper.registEvent(gameObject, onchangeClose);

        onchangeMouse();
        onchangeCloud();
        SetScreenSizeValue();
    }

    private void readVales()
    {
        try
        {
            setting.sliderVolum.forceValue(int.Parse(Config.Get("vol_", "750")) / 1000f);
            setting.sliderSize.forceValue(int.Parse(Config.Get("size_", "500")) / 1000f);
            setting.sliderSizeDrawing.forceValue(int.Parse(Config.Get("vSize_", "500")) / 1000f);
            //setting.sliderAlpha.forceValue(((float)(int.Parse(Config.Get("alpha_", "666")))) / 1000f);
            onChangeAlpha();
            onChangeBgmVol();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void onchangeCloud()
    {
        Program.MonsterCloud = setting.cloud.value;
    }

    public void onchangeMouse()
    {
        Program.I().mouseParticle.SetActive(setting.mouseEffect.value);
    }

    //private int dontResizeTwice = 2;

    public void SetScreenSizeValue()
    {
        var target = $"{Screen.width} x {Screen.height}";
        if (_screen.value != target) _screen.value = target;

        _screen.items = (Screen.fullScreen ? Screen.resolutions : WindowResolutions())
            .Select(r => $"{r.width} x {r.height}")
            .Distinct()
            .ToList();
    }

    private static IEnumerable<Resolution> WindowResolutions()
    {
        var resolutions = new List<Resolution>();
        var max = Screen.resolutions.Last();
        for (var height = 560; height <= max.height && height * 1300 / 700 <= max.width; height += 7 * 20)
        {
            resolutions.Add(new Resolution { width = height * 1300 / 700, height = height });
        }

        return resolutions;
    }

    private void onCP()
    {
        try
        {
            Program.I().ocgcore.realize(true);
        }
        catch (Exception e)
        {
        }
    }


    public void onchangeCloseUp()
    {
        if (setting.closeUp.value == false)
        {
            setting.sliderAlpha.forceValue(0);
            //setting.sliderSize.forceValue(0);
        }
        else
        {
            setting.sliderAlpha.forceValue(0.6666f);
            //setting.sliderSize.forceValue(1f);
        }

        //onChangeBgmVol();
        onChangeAlpha();
    }

    private void onchangeClose()
    {
        atk = 1800;
        star = 5;
        try
        {
            atk = int.Parse(setting.showoffATK.value);
        }
        catch (Exception)
        {
        }

        try
        {
            star = int.Parse(setting.showoffStar.value);
        }
        catch (Exception)
        {
        }

 
    }

    private void onChangeAlpha()
    {
        if (sliderAlpha != null) Program.transparency = 1.5f * sliderAlpha.value;
        Program.transparency = 1f;
    }

    private void onChangeLongField()
    {
        Program.longField = UIHelper.getByName<UIToggle>(gameObject, "longField_").value;
        onCP();
    }

    private void onChangeVsize()
    {
        if (sliderVsize != null) Program.verticleScale = 4f + 2f * sliderVsize.value;
    }

    private void onChangeBgmVol()
    {
        if (sliderSize != null)
        {
            BGMHandler.vol = UIHelper.getByName<UISlider>(gameObject, "size_").value;
            BGMHandler.ChangeBgmVol(UIHelper.getByName<UISlider>(gameObject, "size_").value);
        }
    }

    public float vol()
    {
        return UIHelper.getByName<UISlider>(gameObject, "vol_").value;
    }

    public override void preFrameFunction()
    {
        base.preFrameFunction();
    }

    private void onClickExit()
    {
        hide();
    }

    private void resizeScreen()
    {
        //if (dontResizeTwice > 0)
        //{
        //    dontResizeTwice--;
        //    return;
        //}
        //dontResizeTwice = 2;
        if (UIHelper.isMaximized())
            UIHelper.RestoreWindow();

        var mats = UIHelper.getByName<UIPopupList>(gameObject, "screen_").value
            .Split(new[] { " x " }, StringSplitOptions.RemoveEmptyEntries);
        Assert.IsTrue(mats.Length == 2);
        Screen.SetResolution(int.Parse(mats[0]), int.Parse(mats[1]),
            UIHelper.getByName<UIToggle>(gameObject, "full_").value);
        Program.go(100, () =>
        {
            SetScreenSizeValue();
            Program.I().fixScreenProblems();
        });
    }

    public void saveWhenQuit()//mark 保存设置
    {
        Config.Set("vol_", ((int)(UIHelper.getByName<UISlider>(gameObject, "vol_").value * 1000)).ToString());
        Config.Set("size_", ((int)(UIHelper.getByName<UISlider>(gameObject, "size_").value * 1000)).ToString());
        Config.Set("vSize_", ((int)(UIHelper.getByName<UISlider>(gameObject, "vSize_").value * 1000)).ToString());
        //Config.Set("alpha_", ((int)(UIHelper.getByName<UISlider>(gameObject, "alpha_").value * 1000)).ToString());
        Config.Set("longField_",
            UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "longField_").value));
        var collection = gameObject.GetComponentsInChildren<UIToggle>();
        for (var i = 0; i < collection.Length; i++)
            if (collection[i].name.Length > 0 && collection[i].name[0] == '*')
                Config.Set(collection[i].name, UIHelper.fromBoolToString(collection[i].value));
        Config.Set("showoffATK", setting.showoffATK.value);
        Config.Set("showoffStar", setting.showoffStar.value);
        // Config.Set("resize_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "resize_").value));
        Config.Set("maximize_", UIHelper.fromBoolToString(UIHelper.isMaximized()));

        Config.Set("field_1", GetFieldValue.field1);
        Config.Set("field_2", GetFieldValue.field2);
        Config.Set("avatarstand_1", GetFieldValue.avatarstand1);
        Config.Set("avatarstand_2", GetFieldValue.avatarstand2);
        Config.Set("mate_1", GetFieldValue.mate1);
        Config.Set("mate_2", GetFieldValue.mate2);
        Config.Set("grave_1", GetFieldValue.grave1);
        Config.Set("grave_2", GetFieldValue.grave2);
        Config.Set("wallpaper_", GetFieldValue.wallpaper);
    }

    public void save()
    {
        Config.Set("ignoreWatcher_",
            UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "ignoreWatcher_").value));
        Config.Set("ignoreOP_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "ignoreOP_").value));
        Config.Set("smartSelect_",
            UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "smartSelect_").value));
        Config.Set("autoChain_",
            UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "autoChain_").value));
        Config.Set("handPosition_",
            UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "handPosition_").value));
        Config.Set("handmPosition_",
            UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "handmPosition_").value));
        Config.Set("spyer_", UIHelper.fromBoolToString(UIHelper.getByName<UIToggle>(gameObject, "spyer_").value));
       
        if (UIHelper.getByName<UIToggle>(gameObject, "high_").value)
            QualitySettings.SetQualityLevel(5);
        else
            QualitySettings.SetQualityLevel(0);
    }

    public float soundValue()
    {
        return UIHelper.getByName<UISlider>(gameObject, "vol_").value;
    }
}
