using Spine.Unity;
using System;
using System.Globalization;
using System.IO;
using UnityEngine;
using YGOSharp;
using YGOSharp.OCGWrapper.Enums;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;

public  class CutinLoader : MonoBehaviour
{
    public static int id;
    public static int level;
    public static int attribute;
    public static int type;
    public static string cardName;
    public static int atk;
    public static int def;
    public static uint controller;
    public GameObject nameNear;
    public GameObject nameFar;
    Transform spine;
    static string path = "spine/";
    static string path2 = "effects/summonmonster_04backeff/";

    public bool test;
    public string testSpinePath;

    void Start()
    {
        id = 0;
    }
    void Update()
    {
        if (id != 0 && (Program.I().ocgcore.currentMessage == GameMessage.SpSummoning || Program.I().ocgcore.currentMessage == GameMessage.Summoning))
        {
            LoadCutin();
        }
    }
    void LoadCutin()
    {
        //Spine
        GameObject go;
        if (HasCutin(id) == 1 || test && !testSpinePath.StartsWith("u"))//官方Spine
        {
            if (test) go = ABLoader.LoadABFolder(path + testSpinePath, "Spine");
            else go = ABLoader.LoadABFolder(path + id.ToString(), "Spine");
            ABLoader.ChangeLayer(go, "fx_3d");
            //id = 27204311;
            if (id == 27204311)//陨石
            {
            }
            else
            {
                if (id == 88307361 || id== 74889525)
                {
                    go.transform.GetChild(0).GetChild(1).SetParent(go.transform.GetChild(0).GetChild(0));
                    spine = go.transform.GetChild(0).GetChild(0).GetChild(0);
                }
                else spine = go.transform.GetChild(0).GetChild(0).GetChild(0);
                spine.GetComponent<SkeletonAnimation>().state.SetAnimation(1, "animation", false);
                spine.GetComponent<MeshRenderer>().sortingOrder = 1;
            }            
        }
        else //自制Spine
        {
            if(test) go = ABLoader.LoadABFolder(path + testSpinePath, "spine");
            else go = ABLoader.LoadABFolder(path + "u" + id.ToString(), "spine");
        }
        go.transform.localPosition = new Vector3 (0, 0, -0.1f);
        Program.I().destroy(go, 1.7f);

        //Sound + BackEffects
        string sound = "SE_DUEL/SE_MONSTER_CUTIN_EARTH";
        string pathBack = path2 + "summonmonster_bgeah_s2";

        if (GameStringHelper.differ(attribute, (long)CardAttribute.Water))
        {
            pathBack = path2 + "summonmonster_bgwtr_s2";
            sound = "SE_DUEL/SE_MONSTER_CUTIN_WATER";
        }
        if (GameStringHelper.differ(attribute, (long)CardAttribute.Fire))
        {
            pathBack = path2 + "summonmonster_bgfie_s2";
            sound = "SE_DUEL/SE_MONSTER_CUTIN_FIRE";
        }
        if (GameStringHelper.differ(attribute, (long)CardAttribute.Wind))
        {
            pathBack = path2 + "summonmonster_bgwid_s2";
            sound = "SE_DUEL/SE_MONSTER_CUTIN_WIND";
        }
        if (GameStringHelper.differ(attribute, (long)CardAttribute.Light))
        {
            pathBack = path2 + "summonmonster_bglit_s2";
            sound = "SE_DUEL/SE_MONSTER_CUTIN_LIGHT";
        }
        if (GameStringHelper.differ(attribute, (long)CardAttribute.Dark))
        {
            pathBack = path2 + "summonmonster_bgdak_s2";
            sound = "SE_DUEL/SE_MONSTER_CUTIN_DARK";
        }
        if (GameStringHelper.differ(attribute, (long)CardAttribute.Divine))
        {
            pathBack = path2 + "summonmonster_bgdve_s2";
            sound = "SE_DUEL/SE_MONSTER_CUTIN_DIVINE";
        }

        UIHelper.playSound(sound, 0.7f);

        GameObject back = ABLoader.LoadAB(pathBack);
        ABLoader.ChangeLayer(back, "fx_3d", true);
        Transform eff_flame = back.transform.Find("Eff_Flame");
        eff_flame.localScale = new Vector3(2.61f, 1.48f, 1f);
        eff_flame.GetComponent<SpriteRenderer>().DOFade(0f, 1.8f);

        Transform eff_bg00 = back.transform.Find("Eff_Bg00");
        eff_bg00.localScale = new Vector3(25f, 25f, 1f);
        eff_flame.GetComponent<SpriteRenderer>().DOFade(0f, 1.8f);

        Destroy(back, 1.7f);

        //name
        if (controller == 0) nameBarIns(nameNear);
        else nameBarIns(nameFar);
    }
    void nameBarIns(GameObject go)
    {
        var nameBar = Instantiate(go);
        nameBar.transform.localPosition = new Vector3(8.59f, 0f, 0f);
        ABLoader.ChangeLayer(nameBar, "fx_3d");
        TextBehaviour tb = nameBar.GetComponent<TextBehaviour>();
        if ((type & (int)CardType.Link) > 0) tb.type = "link";
        if ((type & (int)CardType.Xyz) > 0) tb.type = "rank";
        tb.cardName = cardName;
        tb.level = level;
        tb.atk = atk;
        tb.def = def;
        Destroy(nameBar, 1.63f);

        id = 0;
    }

    public static int HasCutin(int num)
    {
        if (Directory.Exists("assetbundle/spine/" + num.ToString())) return 1;
        if(Directory.Exists("assetbundle/spine/u" + num.ToString())) return 2;
        return 0;
    }
}
