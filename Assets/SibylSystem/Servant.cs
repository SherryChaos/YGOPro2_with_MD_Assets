﻿using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;
using YGOSharp.OCGWrapper.Enums;
using Object = UnityEngine.Object;

public class Servant
{
    private readonly List<GameObject> allGameObjects = new List<GameObject>();

    private float buttomToScreen;

    private readonly List<Program.delayedTask> delayedTasks = new List<Program.delayedTask>();
    public GameObject gameObject;

    public bool isShowed;

    private GameObject preHover;

    private float RightToScreen;

    public GameObject toolBar;

    private readonly List<Action> updateActions = new List<Action>();

    private readonly List<Action> updateActions_s = new List<Action>();


    public Servant()
    {
        initialize();
        AddUpdateAction(preFrameFunction);
    }

    public virtual void initialize()
    {
    }

    public virtual void show()
    {
        if (isShowed == false)
        {
            isShowed = true;
            Program.notGo(fixScreenProblem);
            Program.go(50, fixScreenProblem);
        }
    }

    public virtual void hide()
    {
        RMSshow_clear();
        RMSshow_clearYNF();
        if (isShowed)
        {
            isShowed = false;
            Program.notGo(fixScreenProblem);
            Program.go(50, fixScreenProblem);
        }

        for (var i = 0; i < allGameObjects.Count; i++) Program.I().destroy(allGameObjects[i], 0, false, true);
        allGameObjects.Clear();
        updateActions_s.Clear();
        for (var i = 0; i < delayedTasks.Count; i++) Program.notGo(delayedTasks[i].act);
        delayedTasks.Clear();
    }

    public virtual void fixScreenProblem()
    {
        if (isShowed)
            applyShowArrangement();
        else
            applyHideArrangement();
    }

    public void safeObject(GameObject o)
    {
        allGameObjects.Add(o);
    }

    public virtual void preFrameFunction()
    {
    }

    public virtual void ES_mouseDownEmpty()
    {
    }

    public virtual void ES_mouseDownGameObject(GameObject gameObject)
    {
    }

    public virtual void ES_mouseUp()
    {
    }

    public virtual void ES_mouseDownRight()
    {
    }

    public virtual void ES_mouseUpRight()
    {
    }

    public virtual void ES_mouseUpEmpty()
    {
    }

    public virtual void ES_mouseUpGameObject(GameObject gameObject)
    {
    }

    public virtual void ES_HoverOverGameObject(GameObject gameObject)
    {
    }

    public void showBarOnly()
    {
        if (toolBar != null)
        {
            toolBar.transform.DOMove(
                Utils.UIToWorldPoint(new Vector3(Utils.UIWidth() - RightToScreen, buttomToScreen, 0)),
                0.6f);
            var items = toolBar.GetComponentsInChildren<toolShift>();
            foreach (var t in items) t.enabled = true;
        }
    }

    public void hideBarOnly()
    {
        if (toolBar != null)
        {
            toolBar.transform.DOMove(
                Utils.UIToWorldPoint(new Vector3(Utils.UIWidth() - RightToScreen, -100, 0)),
                0.6f);
            var items = toolBar.GetComponentsInChildren<toolShift>();
            foreach (var t in items) t.enabled = false;
        }
    }

    public virtual void applyShowArrangement()
    {
        showBarOnly();
    }

    public virtual void applyHideArrangement()
    {
        hideBarOnly();
    }

    public virtual void ES_quit()
    {
    }

    public void Update()
    {
        if (isShowed)
        {
            for (var i = 0; i < updateActions.Count; i++) updateActions[i]();
            for (var i = 0; i < updateActions_s.Count; i++) updateActions_s[i]();
            if (Program.InputGetMouseButtonDown_0)
            {
                if (Program.pointedGameObject == null)
                    ES_mouseDownEmpty();
                else
                    ES_mouseDownGameObject(Program.pointedGameObject);
            }

            if (Program.InputGetMouseButtonUp_0)
            {
                if (Program.pointedGameObject == null)
                    ES_mouseUpEmpty();
                else
                    ES_mouseUpGameObject(Program.pointedGameObject);
                ES_mouseUp();
            }

            if (Program.InputGetMouseButtonDown_1) ES_mouseDownRight();
            if (Program.InputGetMouseButtonUp_1) ES_mouseUpRight();
            if (preHover != Program.pointedGameObject)
            {
                preHover = Program.pointedGameObject;
                if (preHover != null)
                    ES_HoverOverGameObject(preHover);
            }
        }
    }

    public void OnQuit()
    {
        ES_quit();
    }

    public GameObject create(
        GameObject mod,
        Vector3 position = default,
        Vector3 rotation = default,
        bool fade = false,
        GameObject father = null,
        bool allParamsInWorld = true,
        Vector3 wantScale = default
    )
    {
        var re = Program.I().create(mod, position, rotation, fade, father, allParamsInWorld, wantScale);
        return re;
    }

    public GameObject create_s(
        GameObject mod,
        Vector3 position = default,
        Vector3 rotation = default,
        bool fade = false,
        GameObject father = null,
        bool allParamsInWorld = true,
        Vector3 wantScale = default
    )
    {
        var re = Program.I().create(mod, position, rotation, fade, father, allParamsInWorld, wantScale);
        allGameObjects.Add(re);
        return re;
    }

    public void destroy(GameObject obj, float time = 0, bool fade = false, bool instantNull = false)
    {
        allGameObjects.Remove(obj);
        Program.I().destroy(obj, time, fade, instantNull);
    }

    public void AddUpdateAction(Action action)
    {
        updateActions.Add(action);
    }

    public void RemoveUpdateAction(Action action)
    {
        updateActions.Remove(action);
    }

    public void AddUpdateAction_s(Action action)
    {
        updateActions_s.Add(action);
    }

    public void RemoveUpdateAction_s(Action action)
    {
        updateActions_s.Remove(action);
    }

    public void SetBar(GameObject mod, float buttomToScreen, float RightToScreen)
    {
        this.buttomToScreen = buttomToScreen;
        this.RightToScreen = RightToScreen;
        // if (toolBar != null) Object.DestroyImmediate(toolBar);
        // 可能会有多次调用出错的东西，可能需要在初始化过的对象上跳过一部分操作
        toolBar = mod;
        UIHelper.InterGameObject(toolBar);
        fixScreenProblem();
    }

    public void CreateBar(GameObject mod, float buttomToScreen, float RightToScreen)
    {
        if (toolBar != null) Object.DestroyImmediate(toolBar);
        SetBar(create(
            mod,
            Utils.UIToWorldPoint(new Vector3(Utils.UIWidth() - RightToScreen, -100, 0)),
            new Vector3(0, 0, 0),
            false,
            Program.I().ui_main_2d
        ), buttomToScreen, RightToScreen);
    }

    public void reShowBar(float buttomToScreen, float RightToScreen)
    {
        this.buttomToScreen = buttomToScreen;
        this.RightToScreen = RightToScreen;
        if (isShowed) showBarOnly();
    }

    public void safeGogo(int delay_, Action act_)
    {
        Program.go(delay_, act_);
        delayedTasks.Add(new Program.delayedTask
        {
            act = act_,
            timeToBeDone = delay_ + Program.TimePassed()
        });
    }

    #region remasterMessageSystem

    public Vector3 centre(bool fix = false)
    {
        if (Program.I().ocgcore.isShowed || Program.I().deckManager.isShowed)
        {
            var screenP = Program.I().main_camera.WorldToScreenPoint(Vector3.zero);
            screenP.z = 0;
            if (fix)
            {
                if (screenP.y > Screen.height - 350f) screenP.y = Screen.height - 350f;
                if (screenP.y < 350f) screenP.y = 350f;
            }

            return Program.I().camera_main_2d.ScreenToWorldPoint(screenP);
        }

        return Program.I().camera_main_2d.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    }

    private Vector3 MSentre()
    {
        if (Program.I().ocgcore.isShowed)
        {
            var real = (Program.fieldSize - 1) * 0.9f + 1f;
            var screenP = Program.I().main_camera.WorldToScreenPoint(new Vector3(0, 0, -5.65f * real));
            screenP.z = 0;
            return Program.I().camera_main_2d.ScreenToWorldPoint(screenP);
        }

        if (Program.I().deckManager.isShowed)
        {
            var screenP = Program.I().main_camera.WorldToScreenPoint(Vector3.zero);
            screenP.z = 0;
            return Program.I().camera_main_2d.ScreenToWorldPoint(screenP);
        }

        return Program.I().camera_main_2d.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    }

    private enum messageSystemType
    {
        none,
        onlyYes,
        yesOrNo,
        yesOrNoOrCancle,
        yesOrNoOrSee,
        singleChoice,
        multipleChoice,
        input,
        position,
        tp
    }

    private messageSystemType currentMStype = messageSystemType.none;

    public string currentMShash;

    private GameObject currentMSwindow;

    public class messageSystemValue
    {
        public string hint = "";
        public string value = "";
    }

    public virtual void ES_RMS(string hashCode, List<messageSystemValue> result)
    {
        RMSshow_clear();
    }

    private void ES_RMSpremono(GameObject gameObjectClicked, messageSystemValue value)
    {
        List<messageSystemValue> re;
        switch (currentMStype)
        {
            case messageSystemType.onlyYes:
            case messageSystemType.yesOrNo:
            case messageSystemType.yesOrNoOrCancle:
            case messageSystemType.yesOrNoOrSee:
            case messageSystemType.singleChoice:
            case messageSystemType.input:
            case messageSystemType.position:
            case messageSystemType.tp:
                re = new List<messageSystemValue>();
                re.Add(value);
                ES_RMS(currentMShash, re);
                break;
            case messageSystemType.multipleChoice:
                var exist = false;
                for (var i = 0; i < RMSshow_multipleChoice_selected.Count; i++)
                    if (RMSshow_multipleChoice_selected[i] == value)
                        exist = true;
                var lab = gameObjectClicked.GetComponentInChildren<UILabel>();
                if (exist)
                {
                    RMSshow_multipleChoice_selected.Remove(value);
                    if (lab != null)
                    {
                        var c = lab.color;
                        c.a = 1f;
                        lab.color = c;
                    }
                }
                else
                {
                    RMSshow_multipleChoice_selected.Add(value);
                    if (lab != null)
                    {
                        var c = lab.color;
                        c.a = 0.3f;
                        lab.color = c;
                    }
                }

                if (RMSshow_multipleChoice_selected.Count == RMSshow_multipleChoice_count)
                    ES_RMS(currentMShash, RMSshow_multipleChoice_selected);
                break;
        }
    }

    public void RMSshow_clear()
    {
        currentMStype = messageSystemType.none;
        currentMShash = "NULL";
        if (currentMSwindow != null)
        {
            destroy(currentMSwindow, 0.1f, false, true);
            currentMSwindow = null;
        }
    }

    public void RMSshow_clearYNF()
    {
        if (yesOrNoForce != null)
        {
            destroy(yesOrNoForce, 0.1f, false, true);
            yesOrNoForce = null;
        }
    }

    public bool IfNoMessage()
    {
        return currentMShash == "NULL";
    }

    public void RMSshow_onlyYes(string hashCode, string hint, messageSystemValue yes)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.onlyYes;
        currentMSwindow = create
        (
            Program.I().ES_1,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.trySetLableText(currentMSwindow, "hint_", hint);
        UIHelper.registEvent(currentMSwindow, "yes_", ES_RMSpremono, yes);
    }

    public void RMSshow_yesOrNo(string hashCode, string hint, messageSystemValue yes, messageSystemValue no)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.yesOrNo;
        currentMSwindow = create
        (
            Program.I().ES_2,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.trySetLableText(currentMSwindow, "hint_", hint);
        UIHelper.registEvent(currentMSwindow, "yes_", ES_RMSpremono, yes);
        UIHelper.registEvent(currentMSwindow, "no_", ES_RMSpremono, no);
    }

    private GameObject yesOrNoForce;

    public void RMSshow_yesOrNoForce(string hint, messageSystemValue yes, messageSystemValue no)
    {
        RMSshow_clearYNF();
        yesOrNoForce = create
        (
            Program.I().ES_2Force,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(yesOrNoForce);
        UIHelper.trySetLableText(yesOrNoForce, "hint_", hint);
        UIHelper.registEvent(yesOrNoForce, "yes_", ES_RMSpremonoForceYesNo, yes);
        UIHelper.registEvent(yesOrNoForce, "no_", ES_RMSpremonoForceYesNo, no);
    }

    private void ES_RMSpremonoForceYesNo(GameObject gameObjectClicked, messageSystemValue value)
    {
        ES_RMS_ForcedYesNo(value);
    }

    public virtual void ES_RMS_ForcedYesNo(messageSystemValue result)
    {
        destroy(yesOrNoForce, 0.6f, true, true);
    }

    public void RMSshow_FS(string hashCode, messageSystemValue first, messageSystemValue second)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.yesOrNo;
        currentMSwindow = create
        (
            Program.I().ES_FS,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.registEvent(currentMSwindow, "yes_", ES_RMSpremono, first);
        UIHelper.registEvent(currentMSwindow, "no_", ES_RMSpremono, second);
    }

    public void RMSshow_yesOrNoOrCancle(string hashCode, string hint, messageSystemValue yes, messageSystemValue no,
        messageSystemValue cancle)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.yesOrNoOrCancle;
        currentMSwindow = create
        (
            Program.I().ES_3cancle,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.trySetLableText(currentMSwindow, "hint_", hint);
        UIHelper.registEvent(currentMSwindow, "yes_", ES_RMSpremono, yes);
        UIHelper.registEvent(currentMSwindow, "no_", ES_RMSpremono, no);
        UIHelper.registEvent(currentMSwindow, "cancle_", ES_RMSpremono, cancle);
    }

    public void RMSshow_singleChoice(string hashCode, List<messageSystemValue> options)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.singleChoice;
        currentMSwindow = create
        (
            Program.I().ES_Single_multiple_window,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        var sp = UIHelper.getByName<UISprite>(currentMSwindow, "under");
        sp.height = 70 + options.Count * 48;
        for (var i = 0; i < options.Count; i++)
        {
            var btn = create
            (
                Program.I().ES_Single_option,
                new Vector3(-2, sp.height / 2 - 59 - 48 * i, 0),
                Vector3.zero,
                false,
                sp.gameObject,
                false
            );
            UIHelper.trySetLableText(btn, "[u]" + options[i].hint);
            UIHelper.registEvent(btn, btn.name, ES_RMSpremono, options[i]);
        }

        UIHelper.InterGameObject(currentMSwindow);
    }

    private int RMSshow_multipleChoice_count;

    private readonly List<messageSystemValue> RMSshow_multipleChoice_selected = new List<messageSystemValue>();

    public void RMSshow_multipleChoice(string hashCode, int selectCount, List<messageSystemValue> options)
    {
        RMSshow_multipleChoice_count = selectCount;
        RMSshow_multipleChoice_selected.Clear();
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.multipleChoice;
        currentMSwindow = create
        (
            Program.I().ES_Single_multiple_window,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        var sp = UIHelper.getByName<UISprite>(currentMSwindow, "under");
        sp.height = 70 + UIHelper.get_zonghangshu(options.Count, 5) * 40;
        sp.width = 470;
        for (var i = 0; i < options.Count; i++)
        {
            var v = UIHelper.get_hang_lie(i, 5);
            var hang = v.x;
            var lie = v.y;
            var btn = create
            (
                Program.I().ES_multiple_option,
                new Vector3(-162 + lie * 80, sp.height / 2 - 55 - 40 * hang, 0),
                Vector3.zero,
                false,
                sp.gameObject,
                false
            );
            UIHelper.trySetLableText(btn, "[u]" + options[i].hint);
            UIHelper.registEvent(btn, btn.name, ES_RMSpremono, options[i]);
        }

        UIHelper.InterGameObject(currentMSwindow);
    }

    public void RMSshow_position(string hashCode, int code, messageSystemValue atk, messageSystemValue def)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.position;
        currentMSwindow = create
        (
            Program.I().ES_position,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.registEvent(currentMSwindow, "atk_", ES_RMSpremono, atk);
        UIHelper.registEvent(currentMSwindow, "def_", ES_RMSpremono, def);

        var atkpic = UIHelper.getByName<UITexture>(currentMSwindow, "atkPic_");
        var defbutton = UIHelper.getByName<UIButton>(currentMSwindow, "def_");
        if (int.Parse(atk.value) == (int) CardPosition.FaceUpDefence)
        {
            atkpic.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            defbutton.transform.localPosition = new Vector3(72.8f, 2f, 0f);
        }
        else
        {
            atkpic.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            defbutton.transform.localPosition = new Vector3(62.8f, 0f, 0f);
        }

        var cardPicLoader = currentMSwindow.AddComponent<cardPicLoader>();
        cardPicLoader.uiTexture = atkpic;
        cardPicLoader.code = code;
        cardPicLoader = currentMSwindow.AddComponent<cardPicLoader>();
        cardPicLoader.uiTexture = UIHelper.getByName<UITexture>(currentMSwindow, "defPic_");
        cardPicLoader.code = int.Parse(def.value) == (int) CardPosition.FaceDownDefence ? 0 : code;
    }
    public void RMSshow_position3(string hashCode, int code)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.position;
        currentMSwindow = create
            (
            Program.I().ES_position3,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d,
            true,
            new Vector3(((float)Screen.height) / 700f, ((float)Screen.height) / 700f, ((float)Screen.height) / 700f)
            );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.registEvent(currentMSwindow, "upAtk_", ES_RMSpremono, new messageSystemValue { value = "1", hint = "Face-Up Attack" });
        UIHelper.registEvent(currentMSwindow, "upDef_", ES_RMSpremono, new messageSystemValue { value = "4", hint = "Face-Up Defense" });
        UIHelper.registEvent(currentMSwindow, "downDef_", ES_RMSpremono, new messageSystemValue { value = "8", hint = "Face-Down Defense" });

        UITexture upatkpic = UIHelper.getByName<UITexture>(currentMSwindow, "upAtkPic_");
        UITexture updefpic = UIHelper.getByName<UITexture>(currentMSwindow, "upDefPic_");
        UITexture downdefpic = UIHelper.getByName<UITexture>(currentMSwindow, "downDefPic_");

        cardPicLoader cardPicLoader_ = currentMSwindow.AddComponent<cardPicLoader>();
        cardPicLoader_.uiTexture = upatkpic;
        cardPicLoader_.code = code;
        cardPicLoader_ = currentMSwindow.AddComponent<cardPicLoader>();
        cardPicLoader_.uiTexture = updefpic;
        cardPicLoader_.code = code;
        cardPicLoader_ = currentMSwindow.AddComponent<cardPicLoader>();
        cardPicLoader_.uiTexture = downdefpic;
        cardPicLoader_.code = 0;
    }

    public void RMSshow_tp(string hashCode, messageSystemValue jiandao, messageSystemValue shitou,
        messageSystemValue bu)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.tp;
        currentMSwindow = create
        (
            Program.I().ES_Tp,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.registEvent(currentMSwindow, "jiandao_", ES_RMSpremono, jiandao);
        UIHelper.registEvent(currentMSwindow, "shitou_", ES_RMSpremono, shitou);
        UIHelper.registEvent(currentMSwindow, "bu_", ES_RMSpremono, bu);
    }

    public void RMSshow_input(string hashCode, string hint, string default_)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.input;
        currentMSwindow = create
        (
            Program.I().ES_input,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        UIHelper.trySetLableText(currentMSwindow, "hint_", hint);
        UIHelper.registEvent(currentMSwindow, "input_", ES_RMSpremono, null, "yes_");
        UIHelper.getByName<UIInput>(currentMSwindow, "input_").value = default_;
        Program.go(100, () => { UIHelper.getByName<UIInput>(currentMSwindow, "input_").isSelected = true; });
    }

    public void RMSshow_none(string hint)
    {
        Program.I().cardDescription.mLog(hint);
    }

    public void RMSshow_face(string hashCode, string name)
    {
        RMSshow_clear();
        currentMShash = hashCode;
        currentMStype = messageSystemType.onlyYes;
        currentMSwindow = create
        (
            Program.I().ES_Face,
            MSentre(),
            Vector3.zero,
            true,
            Program.I().ui_main_2d
        );
        UIHelper.InterGameObject(currentMSwindow);
        MyCard.LoadAvatar(name,
            texture => UIHelper.getByName<UITexture>(currentMSwindow, "face_").mainTexture = texture);
        UIHelper.registEvent(currentMSwindow, "yes_", ES_RMSpremono, new messageSystemValue());
    }

    #endregion
}