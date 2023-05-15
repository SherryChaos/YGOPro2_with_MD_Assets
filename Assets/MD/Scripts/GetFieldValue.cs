using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFieldValue : MonoBehaviour
{
    public UIPopupList field_1;
    public UIPopupList field_2;
    public UIPopupList avatarstand_1;
    public UIPopupList avatarstand_2;
    public UIPopupList mate_1;
    public UIPopupList mate_2;
    public UIPopupList grave_1;
    public UIPopupList grave_2;
    public UIPopupList wallpaper_;
    public UISlider bgmVol_;

    public static string field1;
    public static string field2;
    public static string avatarstand1;
    public static string avatarstand2;
    public static string mate1;
    public static string mate2;
    public static string grave1;
    public static string grave2;
    public static string wallpaper;

    // Start is called before the first frame update
    void Start()
    {
        field_1.value = Config.Get("field_1", "森林");
        field_2.value = Config.Get("field_2", "森林");
        avatarstand_1.value = Config.Get("avatarstand_1", "森林");
        avatarstand_2.value = Config.Get("avatarstand_2", "森林");
        mate_1.value = Config.Get("mate_1", "无");
        mate_2.value = Config.Get("mate_2", "无");
        grave_1.value = Config.Get("grave_1", "无");
        grave_2.value = Config.Get("grave_2", "无");
        wallpaper_.value = Config.Get("wallpaper_", "青眼亚白龙");

        field1 = Config.Get("field_1", "森林");
        field2 = Config.Get("field_2", "森林");
        avatarstand1 = Config.Get("avatarstand_1", "森林");
        avatarstand2 = Config.Get("avatarstand_2", "森林");
        mate1 = Config.Get("mate_1", "无");
        mate2 = Config.Get("mate_2", "无");
        grave1 = Config.Get("grave_1", "无");
        grave2 = Config.Get("grave_2", "无");
        wallpaper = Config.Get("wallpaper_", "青眼亚白龙");

        EventDelegate.Add(field_1.onChange, Field1Change);
        EventDelegate.Add(field_2.onChange, Field2Change);
        EventDelegate.Add(avatarstand_1.onChange, AvatarStand1Change);
        EventDelegate.Add(avatarstand_2.onChange, AvatarStand2Change);
        EventDelegate.Add(mate_1.onChange, Mate1Change);
        EventDelegate.Add(mate_2.onChange, Mate2Change);
        EventDelegate.Add(grave_1.onChange, Grave1Change);
        EventDelegate.Add(grave_2.onChange, Grave2Change);
        EventDelegate.Add(wallpaper_.onChange, WallpaperChange);
    }

    void Field1Change()
    {
        field1 = field_1.value;
        LoadAssets.CreateField1(field_1.value);
    }
    void Field2Change()
    {
        field2 = field_2.value;
        LoadAssets.CreateField2(field_2.value);
    }
    void AvatarStand1Change()
    {
        avatarstand1 = avatarstand_1.value;
        LoadAssets.CreateAvatarStand1(avatarstand_1.value);
    }
    void AvatarStand2Change()
    {
        avatarstand2 = avatarstand_2.value;
        LoadAssets.CreateAvatarStand2(avatarstand_2.value);
    }
    void Mate1Change()
    {
        mate1 = mate_1.value;
        LoadAssets.CreateMate1(mate_1.value);
    }
    void Mate2Change()
    {
        mate2 = mate_2.value;
        LoadAssets.CreateMate2(mate_2.value);
    }
    void Grave1Change()
    {
        grave1 = grave_1.value;
        LoadAssets.CreateGrave1(grave_1.value);
    }
    void Grave2Change()
    {
        grave2 = grave_2.value;
        LoadAssets.CreateGrave2(grave_2.value);
    }
    void WallpaperChange()
    {
        wallpaper = wallpaper_.value;
        UIHandler.LoadBgFront(wallpaper_.value);
    }
}
