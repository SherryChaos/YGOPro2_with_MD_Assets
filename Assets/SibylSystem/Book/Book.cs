using System;
using System.Collections.Generic;
using UnityEngine;
using YGOSharp.OCGWrapper.Enums;

public class Book : WindowServant2D
{
    private string changcha = "";

    public string deckString = "";
    private string kacha = "";
    public UILabel lab = null;
    public UILabel labop = null;

    private readonly List<string> lines = new List<string>();
    public string opString = "";

    private lazyBookbtns texts;
    private string xuecha = "";

    public override void initialize()
    {
        kacha = InterString.Get("卡差:");
        changcha = InterString.Get("场差:");
        xuecha = InterString.Get("血差:");
        gameObject = SetWindow(this, Program.I().new_ui_book);
        texts = gameObject.GetComponentInChildren<lazyBookbtns>();
        texts.textlist.scrollValue = 1;
        texts.textlist.lockDrag = true;
        UIHelper.registEvent(gameObject, "exit_", hide);
        applyHideArrangement();
    }

    private string formatS(string from, int c, bool k)
    {
        var returnValue = "";
        if (k) return @from + c;
        if (c < 0)
            returnValue = @from + "[ff0000]" + c + "[-]";
        else
            returnValue = @from + "[00ff00]" + c + "[-]";
        return returnValue;
    }

    public override void preFrameFunction()
    {
        base.preFrameFunction();
        if (isShowed)
        {
            gameObject.transform.position = Utils.UIToWorldPoint(
                new Vector3(Program.I().cardDescription.width / 2,
                    (700 - Program.I().cardDescription.cHeight) / 2, 0));
            texts.back.width = (int) Program.I().cardDescription.width;
            texts.back.height = 700 - (int) Program.I().cardDescription.cHeight;
        }
    }

    public override void applyShowArrangement()
    {
        if (gameObject != null) gameObject.SetActive(true);
    }

    public override void applyHideArrangement()
    {
        if (gameObject != null) gameObject.SetActive(false);
    }

    public override void hide()
    {
        base.hide();
        Program.notGo(fixScreenProblem);
        fixScreenProblem();
    }

    public override void show()
    {
        base.show();
        Program.I().cardDescription.shiftCardShower(true);
        Program.notGo(fixScreenProblem);
        fixScreenProblem();
        realize();
    }

    public void realize()
    {
        MultiStringMaster master;

        if (lab != null)
        {
            deckString = "";
            master = new MultiStringMaster();
            foreach (var item in TcpHelper.deckStrings) master.Add(item);
            foreach (var item in Program.I().ocgcore.cards)
            {
                if (item.p.location == (uint) CardLocation.Search) continue;
                if (item.p.location == (uint) CardLocation.Unknown) continue;
                if (item.p.location == (uint) CardLocation.Deck) continue;
                if (item.get_data().Id <= 0) continue;
                if (item.controllerBased == 0) master.remove(item.get_data().Name);
            }

            deckString += master.managedString.TrimEnd('\n');
            lab.text = deckString;
        }

        if (labop != null)
        {
            opString = "";
            master = new MultiStringMaster();
            foreach (var item in Program.I().ocgcore.cards)
            {
                if (item.p.location == (uint) CardLocation.Search) continue;
                if (item.get_data().Id <= 0) continue;
                if (item.controllerBased == 1) master.Add(item.get_data().Name);
            }

            opString += master.managedString.TrimEnd('\n');
            if (Program.I().ocgcore.cantCheckGrave)
                labop.text = InterString.Get("不能查看对手使用过的卡");
            else if (master.strings.Count > 0)
                labop.text = InterString.Get("[ff5555]对手使用过：@n[?][-]", opString);
            else
                labop.text = InterString.Get("请等待对手出牌来获取情报");
        }

        if (isShowed == false) return;


        var fieldCards = new int[2] {0, 0};
        var handCards = new int[2] {0, 0};
        var resourceCards = new int[2] {0, 0};
        var died = false;
        foreach (var item in Program.I().ocgcore.cards)
        {
            if (item.p.location == (uint) CardLocation.Search) continue;
            if (item.p.location == (uint) CardLocation.Unknown) continue;
            for (var i = 0; i < 2; i++)
                if (item.p.controller == i)
                {
                    if (item.p.location == (uint) CardLocation.MonsterZone ||
                        item.p.location == (uint) CardLocation.SpellZone) fieldCards[i]++;
                    if (item.p.location == (uint) CardLocation.Hand) handCards[i]++;
                    if (item.p.location == (uint) CardLocation.Grave || item.p.location == (uint) CardLocation.Removed)
                        resourceCards[i]++;
                }
        }

        if (!died)
            texts.lable.text = InterString.Get("消息记录") + "\n" +
                               formatS(kacha, fieldCards[0] + handCards[0] - (fieldCards[1] + handCards[1]), false) +
                               " " +
                               formatS(changcha, fieldCards[0] - fieldCards[1], false) + " " +
                               formatS(xuecha,
                                   Program.I().ocgcore.gameInfo.swaped
                                       ? Program.I().ocgcore.life_1 - Program.I().ocgcore.life_0
                                       : Program.I().ocgcore.life_0 - Program.I().ocgcore.life_1
                                   , false);
        var all = "";
        foreach (var item in lines) all += item + "\n";
        try
        {
            all = all.Substring(0, all.Length - 1);
        }
        catch (Exception e)
        {
        }

        try
        {
            texts.textlist.Clear();
            texts.textlist.Add(all);
        }
        catch (Exception)
        {
            Program.DEBUGLOG("NO LableList");
        }
    }

    public void add(string str)
    {
        lines.Add(str);
        realize();
    }

    public void clear()
    {
        lines.Clear();
        realize();
    }
}