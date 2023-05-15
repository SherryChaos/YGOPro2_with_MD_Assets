using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using YGOSharp.OCGWrapper.Enums;

public class GameField : OCGobject
{
    public enum ph
    {
        dp,
        sp,
        mp1,
        bp,
        mp2,
        ep
    }

    private GameObject big_string;

    private GameObject cookie_dark_hole;

    public ph currentPhase;

    private string currentString = "";
    public float delat;

    private readonly List<field_disabled_container> field_disabled_containers = new List<field_disabled_container>();

    private readonly int[] fieldCode = new int[2] {0, 0};
    private float fieldSprite_height = 819f;

    private readonly List<gameHiddenButton> gameHiddenButtons = new List<gameHiddenButton>();

    public bool isLong = false;

    private readonly UILabel label;

    private readonly UITexture leftT;

    public TextMeshPro LOCATION_DECK_0;
    public TextMeshPro LOCATION_DECK_1;
    public TextMeshPro LOCATION_EXTRA_0;
    public TextMeshPro LOCATION_EXTRA_1;
    public TextMeshPro LOCATION_GRAVE_0;
    public TextMeshPro LOCATION_GRAVE_1;
    public TextMeshPro LOCATION_REMOVED_0;
    public TextMeshPro LOCATION_REMOVED_1;
    public GameObject me_left_p_num;
    public GameObject me_right_p_num;
    public bool mePHole = false;
    private readonly UITexture midT;
    public GameObject op_left_p_num;
    public GameObject op_right_p_num;
    public bool opPHole = false;

    private int overCount;

    private bool P;
    private GameObject p_hole_me;
    private Transform p_hole_mel;
    private Transform p_hole_mer;
    private GameObject p_hole_op;
    private Transform p_hole_opl;
    private Transform p_hole_opr;
    public phaser Phase;
    private readonly UITexture phaseTexure;

    private bool prelong;

    private float prereal;

    public int retOfbp = -1;

    public int retOfEp = -1;

    public int retOfMp = -1;
    private readonly UITexture rightT;

    public List<thunder_locator> thunders = new List<thunder_locator>();

    public GameField()
    {
        gameObject = create(Program.I().new_ocgcore_field, getGoodPosition(), Vector3.zero, false,
            null, false);//Program.I().ui_container_3d, false);
        UIHelper.getByName(gameObject, "obj_0").transform.localScale = Vector3.zero;
        UIHelper.getByName(gameObject, "obj_1").transform.localScale = Vector3.zero;
        Phase = gameObject.GetComponentInChildren<phaser>();
        Phase.bpAction = onBP;
        Phase.epAction = onEP;
        Phase.mp2Action = onMP;
        leftT = UIHelper.getByName<UITexture>(gameObject, "leftT");
        midT = UIHelper.getByName<UITexture>(gameObject, "midT");
        rightT = UIHelper.getByName<UITexture>(gameObject, "rightT");
        phaseTexure = UIHelper.getByName<UITexture>(gameObject, "phaseT");
        midT.border = new Vector4(0, 500, 0, 230);

        leftT.mainTexture = null;
        midT.mainTexture = null;
        rightT.mainTexture = null;
        phaseTexure.mainTexture = null;

        me_left_p_num = create(Program.I().mod_ocgcore_number);
        me_right_p_num = create(Program.I().mod_ocgcore_number);
        op_left_p_num = create(Program.I().mod_ocgcore_number);
        op_right_p_num = create(Program.I().mod_ocgcore_number);

        Program.I().ocgcore.AddUpdateAction_s(Update);

        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Deck, 0));
        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Extra, 0));
        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Grave, 0));
        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Removed, 0));
        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Deck, 1));
        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Extra, 1));
        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Grave, 1));
        gameHiddenButtons.Add(new gameHiddenButton(CardLocation.Removed, 1));

        LOCATION_DECK_0 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, -20, 0))
            .GetComponent<TextMeshPro>();
        LOCATION_EXTRA_0 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, 20, 0))
            .GetComponent<TextMeshPro>();
        LOCATION_GRAVE_0 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, 0, 0))
            .GetComponent<TextMeshPro>();
        LOCATION_REMOVED_0 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, 0, 0))
            .GetComponent<TextMeshPro>();


        LOCATION_DECK_1 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, -20, 0))
            .GetComponent<TextMeshPro>();
        LOCATION_EXTRA_1 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, 20, 0))
            .GetComponent<TextMeshPro>();
        LOCATION_GRAVE_1 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, 0, 0))
            .GetComponent<TextMeshPro>();
        LOCATION_REMOVED_1 = create(Program.I().new_ui_textMesh, Vector3.zero, new Vector3(70, 0, 0))
            .GetComponent<TextMeshPro>();

        //mark 回合阶段
        label = //create(Program.I().mod_simple_ngui_text, new Vector3(0, 0, -14.5f), new Vector3(70, 0, 0), false, Program.I().main_camera.gameObject, false).GetComponent<UILabel>();
        create(Program.I().mod_simple_ngui_text, new Vector3(0, 0, -14.5f), new Vector3(70, 0, 0), false, Program.I().ui_container_3d, false).GetComponent<UILabel>();

        label.fontSize = 56;
        label.overflowMethod = UILabel.Overflow.ShrinkContent;
        label.alignment = NGUIText.Alignment.Left;
        label.width = 1600;
        label.height = 80;
        label.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        label.text = "";
        overCount = 0;

        loadNewField();
    }

    private void onBP()
    {
        var m = new BinaryMaster();
        m.writer.Write(retOfbp);
        Program.I().ocgcore.sendReturn(m.get());
    }

    private void onEP()
    {
        var m = new BinaryMaster();
        m.writer.Write(retOfEp);
        Program.I().ocgcore.sendReturn(m.get());
    }

    private void onMP()
    {
        var m = new BinaryMaster();
        m.writer.Write(retOfMp);
        Program.I().ocgcore.sendReturn(m.get());
    }

    public void loadOldField()
    {
        if (File.Exists("texture/duel/field.png"))
        {
            var textureField = UIHelper.GetTexture2D("texture/duel/field.png");
            var textureFieldSliced = UIHelper.sliceField(textureField);
            leftT.mainTexture = textureFieldSliced[0];
            midT.mainTexture = textureFieldSliced[1];
            rightT.mainTexture = textureFieldSliced[2];
        }
        else
        {
            leftT.mainTexture = new Texture2D(100, 100);
            midT.mainTexture = new Texture2D(100, 100);
            rightT.mainTexture = new Texture2D(100, 100);
        }

        phaseTexure.mainTexture = null;
        gameObject.GetComponentInChildren<lazyBTNMOVER>().shift(false);
    }

    public void loadNewField()
    {
        if (File.Exists("texture/duel/newfield.png"))
        {
            var textureField = UIHelper.GetTexture2D("texture/duel/newfield.png");
            var textureFieldSliced = UIHelper.sliceField(textureField);
            leftT.mainTexture = textureFieldSliced[0];
            midT.mainTexture = textureFieldSliced[1];
            rightT.mainTexture = textureFieldSliced[2];
        }
        else
        {
            leftT.mainTexture = new Texture2D(100, 100);
            midT.mainTexture = new Texture2D(100, 100);
            rightT.mainTexture = new Texture2D(100, 100);
        }

        phaseTexure.mainTexture = null;
        gameObject.GetComponentInChildren<lazyBTNMOVER>().shift(true);
    }

    public void relocatePnums(bool p)
    {
        if (Program.I().ocgcore.MasterRule >= 4)
        {
            P = p;
            if (p)
            {
                me_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_left_p_num.transform.eulerAngles = new Vector3(30, -45, 0);
                me_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_right_p_num.transform.eulerAngles = new Vector3(30, 45, 0);
                op_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_left_p_num.transform.eulerAngles = new Vector3(20, -45, 0);
                op_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_right_p_num.transform.eulerAngles = new Vector3(20, 45, 0);
            }
            else
            {
                me_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_left_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
                me_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_right_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
                op_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_left_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
                op_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_right_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
            }
        }
        else
        {
            P = p;
            if (p)
            {
                me_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_left_p_num.transform.eulerAngles = new Vector3(0, -45, 0);
                me_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_right_p_num.transform.eulerAngles = new Vector3(0, 45, 0);
                op_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_left_p_num.transform.eulerAngles = new Vector3(0, -45, 0);
                op_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_right_p_num.transform.eulerAngles = new Vector3(0, 45, 0);
            }
            else
            {
                me_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_left_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
                me_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                me_right_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
                op_left_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_left_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
                op_right_p_num.transform.localScale = new Vector3(3, 3, 3);
                op_right_p_num.transform.eulerAngles = new Vector3(90, 0, 0);
            }
        }
    }

    public void dispose()
    {
        Program.I().ocgcore.RemoveUpdateAction_s(Update);
    }

    private static Vector3 getGoodPosition()
    {
        return new Vector3(0, 0, 0);
    }

    public void Update()
    {
        delat = (isLong ? 40f + 60f * ((1.21f - Program.fieldSize) / 0.21f) : 0f) / 110f * 5f;
        fieldSprite_height +=
            ((isLong ? 819f + 40f + 60f * ((1.21f - Program.fieldSize) / 0.21f) : 819f) - fieldSprite_height) *
            (Program.deltaTime * 4);
        midT.height = (int) fieldSprite_height;

        var position = midT.gameObject.transform.localPosition;
        position.y = fieldSprite_height - 819f;
        position.y /= 2;
        midT.gameObject.transform.localPosition = position;

        gameObject.transform.localPosition = getGoodPosition();
        gameObject.transform.localScale = new Vector3(Program.fieldSize, Program.fieldSize, Program.fieldSize);
        leftT.transform.localScale =
            new Vector3(1f / Program.fieldSize, 1f / Program.fieldSize, 1f / Program.fieldSize);
        leftT.transform.localPosition = new Vector3((-1f + 1f / Program.fieldSize) * leftT.width / 3.5f, 0, 0);
        rightT.transform.localScale =
            new Vector3(1f / Program.fieldSize, 1f / Program.fieldSize, 1f / Program.fieldSize);
        rightT.transform.localPosition = new Vector3((1f - 1f / Program.fieldSize) * rightT.width / 3.5f, 0, 0);

        relocateTextMesh(LOCATION_DECK_0, 0, CardLocation.Deck, new Vector3(-1.5f, 0, 5f));
        relocateTextMesh(LOCATION_EXTRA_0, 0, CardLocation.Extra, new Vector3(1.5f, 0, 5f));
        relocateTextMesh(LOCATION_REMOVED_0, 0, CardLocation.Removed, new Vector3(3.5f, 0, 0));
        relocateTextMesh(LOCATION_GRAVE_0, 0, CardLocation.Grave, new Vector3(3.5f, 0, 0));

        relocateTextMesh(LOCATION_DECK_1, 1, CardLocation.Deck, new Vector3(1.5f, 0, -5f));
        relocateTextMesh(LOCATION_EXTRA_1, 1, CardLocation.Extra, new Vector3(-1.5f, 0, -5f));
        relocateTextMesh(LOCATION_REMOVED_1, 1, CardLocation.Removed, new Vector3(-3.5f, 0, 0));
        relocateTextMesh(LOCATION_GRAVE_1, 1, CardLocation.Grave, new Vector3(-3.5f, 0, 0));

        label.transform.localPosition = new Vector3(-6,0,-3);//new Vector3(-5f * (Program.fieldSize - 1), 0, -15.5f * Program.fieldSize);//mark 回合阶段位置

        if (prelong != isLong)
        {
            prelong = isLong;
            for (var i = 0; i < field_disabled_containers.Count; i++)
                if (field_disabled_containers[i].p.location == (uint) CardLocation.SpellZone)
                    if (field_disabled_containers[i].p.controller == 1)
                    {
                        field_disabled_containers[i].position =
                            Program.I().ocgcore.get_point_worldposition(field_disabled_containers[i].p);
                        if (field_disabled_containers[i].game_object != null)
                            field_disabled_containers[i].game_object.transform.position =
                                field_disabled_containers[i].position;
                    }
        }

        var real = (Program.fieldSize - 1) * 0.9f + 1f;
        if (mePHole)
        {
            if (p_hole_me == null)
            {
                p_hole_me = create(Program.I().mod_ocgcore_ss_p_idle_effect, new Vector3(0, 0, 0));
                p_hole_mel = p_hole_me.transform.Find("l");
                p_hole_mer = p_hole_me.transform.Find("r");
                prereal = 0;
            }
        }
        else
        {
            if (p_hole_me != null)
            {
                destroy(p_hole_me, 0, false, true);
                p_hole_mel = null;
                p_hole_mer = null;
                prereal = 0;
            }
        }

        if (opPHole)
        {
            if (p_hole_op == null)
            {
                p_hole_op = create(Program.I().mod_ocgcore_ss_p_idle_effect, new Vector3(0, 0, 0));
                p_hole_opl = p_hole_op.transform.Find("l");
                p_hole_opr = p_hole_op.transform.Find("r");
                prereal = 0;
            }
        }
        else
        {
            if (p_hole_op != null)
            {
                destroy(p_hole_op, 0, false, true);
                p_hole_opl = null;
                p_hole_opr = null;
                prereal = 0;
            }
        }

        if (prereal != real)
        {
            prereal = real;
            if (Program.I().ocgcore.MasterRule >= 4)
            {
                if (p_hole_mel != null && p_hole_mer != null)
                {
                    p_hole_mel.localPosition = new Vector3(-19.2f, 0, -16f);//new Vector3(-10.1f * real, 0, -11.5f * real);
                    p_hole_mer.localPosition = new Vector3(19.2f, 0, -16f);//new Vector3(9.62f * real, 0, -11.5f * real);
                }

                if (p_hole_opl != null && p_hole_opr != null)
                {
                    p_hole_opl.localPosition = new Vector3(-18.8f, 0, 19.7f); //new Vector3(-10.1f * real, 0, 11.5f * real);
                    p_hole_opr.localPosition = new Vector3(18.8f, 0, 19.7f); //new Vector3(9.62f * real, 0, 11.5f * real);
                }
            }
            else
            {
                if (p_hole_mel != null && p_hole_mer != null)
                {
                    p_hole_mel.localPosition = new Vector3(-15.2f * real, 0, -9f);
                    p_hole_mer.localPosition = new Vector3(14.65f * real, 0, -9f);
                }

                if (p_hole_opl != null && p_hole_opr != null)
                {
                    p_hole_opl.localPosition = new Vector3(-15.2f * real, 0, 9f);
                    p_hole_opr.localPosition = new Vector3(14.65f * real, 0, 9f);
                }
            }
        }

        if (Program.I().ocgcore.MasterRule >= 4)//mark 场地灵摆数字
        {
            if (P)
            {
                me_left_p_num.transform.position = new Vector3(-16.3f, 9.3f, -18.7f);
                //UIHelper.getCamGoodPosition(new Vector3(-10.1f * real + 1, 5, -11.5f * real - 1), -3f);
                me_right_p_num.transform.position = new Vector3(16.3f, 9.3f, -18.7f);
                //UIHelper.getCamGoodPosition(new Vector3(9.62f * real - 1, 5, -11.5f * real - 1), -3f);
                op_left_p_num.transform.position = new Vector3(-16.7f, 8.6f, 15.6f);
                //UIHelper.getCamGoodPosition(new Vector3(-10.1f * real + 1, 5, 11.5f * real - 1), -3f);
                op_right_p_num.transform.position = new Vector3(16.7f, 8.6f, 15.6f);
                //UIHelper.getCamGoodPosition(new Vector3(9.62f * real - 1, 5, 11.5f * real - 1), -3f);
            }
            else
            {
                me_left_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(-10.1f * real, 0, -11.5f * real), -1f);
                me_right_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(9.62f * real, 0, -11.5f * real), -1f);
                op_left_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(-10.1f * real, 0, 11.5f * real), -1f);
                op_right_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(9.62f * real, 0, 11.5f * real), -1f);
            }
        }
        else
        {
            if (P)
            {
                me_left_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(-15.2f * real, 5, -10f), -3f);
                me_right_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(14.65f * real, 5, -10f), -3f);
                op_left_p_num.transform.position = UIHelper.getCamGoodPosition(new Vector3(-15.2f * real, 5, 8f), -3f);
                op_right_p_num.transform.position = UIHelper.getCamGoodPosition(new Vector3(14.65f * real, 5, 8f), -3f);
            }
            else
            {
                me_left_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(-15.2f * real, 0f, -9f), -1f);
                me_right_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(14.65f * real, 0f, -9f), -1f);
                op_left_p_num.transform.position = UIHelper.getCamGoodPosition(new Vector3(-15.2f * real, 0f, 9f), -1f);
                op_right_p_num.transform.position =
                    UIHelper.getCamGoodPosition(new Vector3(14.65f * real, 0f, 9f), -1f);
            }
        }
    }

    private static void relocateTextMesh(TextMeshPro obj, uint con, CardLocation loc, Vector3 poi)
    {
        obj.transform.position = UIHelper.getCamGoodPosition(Program.I().ocgcore.get_point_worldposition(new GPS
        {
            controller = con,
            location = (uint) loc
        }) + poi, -2);
    }

    public void set(int player, int code)
    {
        if (player >= 0)
            if (player < 2)
                if (fieldCode[player] != code)
                {
                    fieldCode[player] = code;
                    if (code > 0)
                    {
                        Texture2D tex = null;
                        var found = false;
                        foreach (var zip in GameZipManager.Zips)
                        {
                            if (zip.Name.ToLower().EndsWith("script.zip"))
                                continue;
                            foreach (var file in zip.EntryFileNames)
                                if (Regex.IsMatch(file.ToLower(), "field/" + code + "\\.(jpg|png)$"))
                                {
                                    var ms = new MemoryStream();
                                    var e = zip[file];
                                    e.Extract(ms);
                                    tex = new Texture2D(1024, 600);
                                    tex.LoadImage(ms.ToArray());
                                    found = true;
                                    break;
                                }

                            if (found)
                                break;
                        }

                        if (tex == null) tex = UIHelper.GetTexture2D("picture/field/" + code + ".png");
                        if (tex == null) tex = UIHelper.GetTexture2D("picture/field/" + code + ".jpg");
                        if (tex != null)
                        {
                            UIHelper.getByName<UITexture>(gameObject, "field_" + player).mainTexture = tex;
                            UIHelper.clearITWeen(UIHelper.getByName(gameObject, "obj_" + player));
                            iTween.ScaleTo(UIHelper.getByName(gameObject, "obj_" + player), new Vector3(1, 1, 1), 0.5f);
                        }
                        else
                        {
                            UIHelper.clearITWeen(UIHelper.getByName(gameObject, "obj_" + player));
                            iTween.ScaleTo(UIHelper.getByName(gameObject, "obj_" + player), new Vector3(0, 0, 0), 0.5f);
                        }
                    }
                    else
                    {
                        UIHelper.clearITWeen(UIHelper.getByName(gameObject, "obj_" + player));
                        iTween.ScaleTo(UIHelper.getByName(gameObject, "obj_" + player), new Vector3(0, 0, 0), 0.5f);
                    }
                }
    }

    public void shiftBlackHole(int a, Vector3 v = default)
    {
        overCount += a;
        if (overCount < 0) overCount = 0;
        if (overCount > 2) overCount = 2;
        if (overCount == 2) shiftBlackHole(true, v);
    }

    public void shiftBlackHole(bool on, Vector3 v = default)
    {
        if (on)
        {
            if (cookie_dark_hole == null)
            {
                Program.I().mod_ocgcore_ss_dark_hole.transform.localScale = Vector3.zero;
                cookie_dark_hole = create(Program.I().mod_ocgcore_ss_dark_hole, v);
                iTween.ScaleTo(cookie_dark_hole, new Vector3(6, 6, 6), 1f);
                cookie_dark_hole.transform.eulerAngles = new Vector3(90, 0, 0);
            }
        }
        else
        {
            if (cookie_dark_hole != null)
            {
                iTween.ScaleTo(cookie_dark_hole, iTween.Hash(
                    "delay", 1f,
                    "x", 0,
                    "y", 0,
                    "z", 0,
                    "time", 1f
                ));
                iTween.MoveTo(cookie_dark_hole, iTween.Hash(
                    "delay", 1f,
                    "position", v,
                    "time", 1f
                ));
                destroy(cookie_dark_hole, 1.4f);
            }
        }
    }

    public void setHint(string hint)
    {
        currentString = "T" + Program.I().ocgcore.turns + " " + hint;
        realize();
    }

    public void setHintLogical(string hint)
    {
        currentString = "T" + Program.I().ocgcore.turns + " " + hint;
    }

    public void animation_show_big_string(Sprite sprite, bool only = false)
    {
        if (Ocgcore.inSkiping) return;
        if (only) destroy(big_string);
        big_string = create(Program.I().New_phase, Program.I().ocgcore.centre(), Vector3.zero, false,
            Program.I().ui_main_2d, true, Vector3.one);
        big_string.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        Program.I().ocgcore.Sleep(60);//40
        big_string.AddComponent<animation_screen_lock2>();
        destroy(big_string, 3f);
    }

    public void animation_show_big_string_tc_me(bool only = false)
    {
        if (Ocgcore.inSkiping) return;
        if (only) destroy(big_string);
        big_string = create(Program.I().New_turn_me, Program.I().ocgcore.centre(), Vector3.zero, false,
            Program.I().ui_main_2d, true, Vector3.one);
        Program.I().ocgcore.Sleep(80);//40
        big_string.AddComponent<animation_screen_lock2>();
        destroy(big_string, 3f);
    }

    public void animation_show_big_string_tc_op(bool only = false)
    {
        if (Ocgcore.inSkiping) return;
        if (only) destroy(big_string);
        big_string = create(Program.I().New_turn_op, Program.I().ocgcore.centre(), Vector3.zero, false,
            Program.I().ui_main_2d, true, Vector3.one);
        Program.I().ocgcore.Sleep(80);//40
        big_string.AddComponent<animation_screen_lock2>();
        destroy(big_string, 3f);
    }

    public void set_point_disabled(GPS gps, bool disabled)
    {
        //temp

        /*
        if (Program.I().ocgcore.MasterRule >= 4)
            if (gps.location == (int) CardLocation.SpellZone)
                if (gps.position == 0 || gps.position == 4)
                    disabled = false;
        */

        field_disabled_container container = null;

        foreach (var cont in field_disabled_containers)
            if (cont.p.controller == gps.controller)
                if (cont.p.location == gps.location)
                    if (cont.p.sequence == gps.sequence)
                    {
                        container = cont;
                        break;
                    }

        if (container == null)
        {
            container = new field_disabled_container
            {
                p = gps,
                position = Program.I().ocgcore.get_point_worldposition(gps)
            };
            field_disabled_containers.Add(container);
        }

        container.disabled = disabled;
    }

    public void realize()
    {
        if (Phase.colliderBp.enabled)
            Phase.labBp.gradientTop = Color.white;
        else
            Phase.labBp.gradientTop = Color.grey;
        if (Phase.colliderEp.enabled)
            Phase.labEp.gradientTop = Color.white;
        else
            Phase.labEp.gradientTop = Color.grey;
        if (Phase.colliderMp2.enabled)
            Phase.labMp2.gradientTop = Color.white;
        else
            Phase.labMp2.gradientTop = Color.grey;
        Phase.labDp.gradientTop = Color.grey;
        Phase.labSp.gradientTop = Color.grey;
        Phase.labMp1.gradientTop = Color.grey;
        switch (currentPhase)
        {
            case ph.dp:
                Phase.labDp.gradientTop = Color.green;
                break;
            case ph.sp:
                Phase.labSp.gradientTop = Color.green;
                break;
            case ph.mp1:
                Phase.labMp1.gradientTop = Color.green;
                break;
            case ph.bp:
                Phase.labBp.gradientTop = Color.green;
                break;
            case ph.mp2:
                Phase.labMp2.gradientTop = Color.green;
                break;
            case ph.ep:
                Phase.labEp.gradientTop = Color.green;
                break;
        }

        for (var i = 0; i < field_disabled_containers.Count; i++)
            if (field_disabled_containers[i].disabled)
            {
                if (field_disabled_containers[i].game_object == null)
                {
                    field_disabled_containers[i].game_object = create(Program.I().mod_simple_quad,
                        field_disabled_containers[i].position, new Vector3(90, 0, 0));
                    field_disabled_containers[i].game_object.transform.localScale = Vector3.zero;
                    iTween.ScaleTo(field_disabled_containers[i].game_object, new Vector3(4, 4, 4), 1f);
                    field_disabled_containers[i].game_object.GetComponent<Renderer>().material.mainTexture =
                        GameTextureManager.negated;
                    field_disabled_containers[i].game_object.GetComponent<Renderer>().material.color = Color.clear;
                }
            }
            else
            {
                destroy(field_disabled_containers[i].game_object, 0.6f, true, true);
            }

        label.text = currentString;

        //if (Program.I().setting.setting.closeUp.value)
        //{
        //    if (label.gameObject.activeInHierarchy==false)  
        //    {
        //        label.gameObject.SetActive(true);
        //    }
        //    label.text = currentString;
        //}
        //else
        //{
        //    if (label.gameObject.activeInHierarchy == true)
        //    {
        //        label.gameObject.SetActive(false);
        //    }
        //}
    }

    public void clearDisabled()
    {
        for (var i = 0; i < field_disabled_containers.Count; i++) field_disabled_containers[i].disabled = false;
    }

    public void animation_show_lp_num(int player, bool up, int count)//mark 控制血量数字
    {
        var color = 0;
        if (up) color = 3;
        Vector3 position;
        Vector3 screen_p;
        if (player == 0)
        {
            screen_p = new Vector3(Program.I().ocgcore.getScreenCenter(), 200f, 10);//new Vector3(Program.I().ocgcore.getScreenCenter(), 100f, 5);
            position = Program.I().main_camera.ScreenToWorldPoint(new Vector3(Program.I().ocgcore.getScreenCenter(),200f, 5));
        }
        else
        {
            screen_p = new Vector3(Program.I().ocgcore.getScreenCenter(), Screen.height - 200f, 10);//new Vector3(Program.I().ocgcore.getScreenCenter(), Screen.height - 100f, 5);
            position = Program.I().main_camera.ScreenToWorldPoint(new Vector3(Program.I().ocgcore.getScreenCenter(),Screen.height - 200f, 5));
        }


        var obj = create(Program.I().mod_ocgcore_number);
        obj.GetComponent<number_loader>().set_number(count, color);
        obj.AddComponent<animation_screen_lock>().screen_point = screen_p;
        obj.transform.position = position;
        obj.transform.localScale = Vector3.zero;
        obj.transform.eulerAngles = new Vector3(70, 0, 0);
        iTween.ScaleTo(obj, new Vector3(0.6f, 0.6f, 0.6f), 0.18f);
        destroy(obj, 1f);
    }


    public void animation_screen_blood(int player, int amount_)
    {
        var amount = amount_;
        if (amount > 8000) amount = 8000;
        var count = amount / 250;
        for (var i = 0; i < count; i++)
            if (player == 0)
                create(
                    Program.I().mod_ocgcore_blood,
                    new Vector3(
                        Random.Range(-20, 20),
                        0,
                        Random.Range(-5, -25)
                    )
                );
            else
                create(
                    Program.I().mod_ocgcore_blood,
                    new Vector3(
                        Random.Range(-20, 20),
                        0,
                        Random.Range(5, 25)
                    )
                );
        if (player == 0)
        {
            Program.I().ocgcore.Sleep((int) (60 * (float) amount / 2500f));
            iTween.ShakePosition(Program.I().main_camera.gameObject, iTween.Hash(
                "x", amount / 1500f,
                "y", amount / 1500f,
                "z", amount / 1500f,
                "time", amount / 2500f
            ));
            var obj_ = create(Program.I().mod_ocgcore_blood_screen);
            obj_.AddComponent<animation_screen_lock>().screen_point =
                new Vector3(
                    Program.I().ocgcore.getScreenCenter(),
                    100f,
                    0.5f + 4000f / amount);
            destroy(obj_, 2.5f);
            for (var i = 0; i < amount / 1000; i++)
            {
                var obj = create(Program.I().mod_ocgcore_blood_screen);
                obj.AddComponent<animation_screen_lock>().screen_point =
                    new Vector3(
                        Screen.width / (float) Random.Range(10, 30) * 10f,
                        Screen.height / (float) Random.Range(10, 30) * 10f,
                        0.5f + 4000f / amount);
                destroy(obj, 2.5f);
            }
        }
    }

    //GameObject big_string;
    //public void animation_show_big_string(string str)
    //{
    //    if (this.big_string!=null) 
    //    {
    //        destroy(this.big_string);
    //    }
    //    big_string = create(Program.I().mod_ocgcore_card_number_shower);
    //    TMPro.TextMeshPro text_mesh = big_string.GetComponent<TMPro.TextMeshPro>();
    //    TMPro.TextContainer text_container = big_string.GetComponent<TMPro.TextContainer>();
    //    text_container.width = 60;
    //    text_container.height = 10;
    //    text_mesh.text = str;
    //    text_mesh.alignment = TMPro.TextAlignmentOptions.Center;


    //    Vector3 screenP = Program.I().main_camera.WorldToScreenPoint(Vector3.zero);
    //    screenP.z = 18f;
    //    int bun = Screen.height / 3;
    //    if (screenP.y > Screen.height / 2 + bun)
    //    {
    //        screenP.y = Screen.height / 2 + bun;
    //    }
    //    if (screenP.y < Screen.height / 2 - bun)
    //    {
    //        screenP.y = Screen.height / 2 - bun;
    //    }
    //    big_string.transform.position = Program.I().main_camera.ScreenToWorldPoint(screenP);

    //    big_string.AddComponent<animation_screen_lock2>();
    //    big_string.transform.localScale = Vector3.zero;
    //    iTween.ScaleTo(big_string, new Vector3(0.7f, 0.7f, 0.7f), 0.3f);
    //    iTween.RotateTo(big_string, new Vector3(60, 0, 0), 0.3f);
    //    iTween.ScaleTo(big_string, iTween.Hash(
    //                       "delay", 0.6f,
    //                       "x", 0,
    //                       "y", 0,
    //                       "z", 0,
    //                       "time", 0.3f
    //                       ));
    //    destroy(big_string, 3f);
    //    Program.I().ocgcore.Sleep(30);
    //}

    private class field_disabled_container
    {
        public bool disabled;
        public GameObject game_object;
        public GPS p;
        public Vector3 position;
    }
}