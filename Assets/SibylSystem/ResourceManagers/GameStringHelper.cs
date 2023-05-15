﻿using System;
using System.Collections.Generic;
using YGOSharp;
using YGOSharp.OCGWrapper.Enums;

public class GameStringHelper
{
    public static string fen = "/";
    public static string xilie = "";
    public static string opHint = "";
    public static string licechuwai = "";
    public static string biaoceewai = "";
    public static string teshuzhaohuan = "";
    public static string yijingqueren = "";

    public static string _zhukazu = "";
    public static string _fukazu = "";
    public static string _ewaikazu = "";
    public static string _guaishou = "";
    public static string _mofa = "";
    public static string _xianjing = "";
    public static string _ronghe = "";
    public static string _lianjie = "";
    public static string _tongtiao = "";
    public static string _chaoliang = "";

    public static string _wofang = "";
    public static string _duifang = "";

    public static string kazu = "";
    public static string mudi = "";
    public static string chuwai = "";
    public static string ewai = "";
    public static string SemiNomi = "";

    public static bool differ(long a, long b)
    {
        var r = false;
        if ((a & b) > 0) r = true;
        return r;
    }

    public static string attribute(long a)
    {
        var r = "";
        var passFirst = false;
        for (var i = 0; i < 7; i++)
            if ((a & (1 << i)) > 0)
            {
                if (passFirst) r += fen;
                r += GameStringManager.get_unsafe(1010 + i);
                passFirst = true;
            }

        return r;
    }

    public static string race(long a)
    {
        var r = "";
        var passFirst = false;
        for (var i = 0; i < 25; i++)
            if ((a & (1 << i)) > 0)
            {
                if (passFirst) r += fen;
                r += GameStringManager.get_unsafe(1020 + i);
                passFirst = true;
            }

        return r;
    }

    public static string zone(long data)
    {
        var strs = new List<string>();
        for (var filter = 0x1L; filter <= 0x1L << 32; filter <<= 1)
        {
            var str = "";
            var s = filter & data;
            if (s != 0)
            {
                if ((s & 0x60) != 0)
                {
                    str += GameStringManager.get_unsafe(1081);
                    data &= ~0x600000;
                }
                else if ((s & 0xffff) != 0)
                {
                    str += GameStringManager.get_unsafe(102);
                }
                else if ((s & 0xffff0000) != 0)
                {
                    str += GameStringManager.get_unsafe(103);
                    s >>= 16;
                }

                if ((s & 0x1f) != 0)
                {
                    str += GameStringManager.get_unsafe(1002);
                }
                else if ((s & 0xff00) != 0)
                {
                    s >>= 8;
                    if ((s & 0x1f) != 0)
                        str += GameStringManager.get_unsafe(1003);
                    else if ((s & 0x20) != 0)
                        str += GameStringManager.get_unsafe(1008);
                    else if ((s & 0xc0) != 0)
                        str += GameStringManager.get_unsafe(1009);
                }

                var seq = 1;
                for (var i = 0x1; i < 0x100; i <<= 1)
                {
                    if ((s & i) != 0)
                        break;
                    ++seq;
                }

                str += "(" + seq + ")";
                strs.Add(str);
            }
        }

        return string.Join(", ", strs.ToArray());
    }

    public static string mainType(long a)
    {
        var r = "";
        var passFirst = false;
        for (var i = 0; i < 3; i++)
            if ((a & (1 << i)) > 0)
            {
                if (passFirst) r += fen;
                r += GameStringManager.get_unsafe(1050 + i);
                passFirst = true;
            }

        return r;
    }

    public static string secondType(long a)
    {
        var r = "";
        var passFirst = false;
        for (var i = 4; i < 27; i++)
            if ((a & (1 << i)) > 0)
            {
                if (passFirst) r += fen;
                r += GameStringManager.get_unsafe(1050 + i);
                passFirst = true;
            }

        if (r == "") r += GameStringManager.get_unsafe(1054);
        return r;
    }

    public static string getName(Card card)
    {
        var limitot = "";
        if ((card.Ot & 0x1) > 0)
            limitot += "[OCG]";
        if ((card.Ot & 0x2) > 0)
            limitot += "[TCG]";
        if ((card.Ot & 0x4) > 0)
            limitot += "[Custom]";
        if ((card.Ot & 0x8) > 0)
            limitot += InterString.Get("[简中]");
        var re = "";
        try
        {
            re += "[b]" + card.Name + "[/b]";
            re += "\n";
            re += "[sup]" + limitot + "[/sup]";
            re += "\n";
            re += "[sup]" + card.Id + "[/sup]";
            re += "\n";
        }
        catch (Exception e)
        {
        }

        if (differ(card.Attribute, (int) CardAttribute.Earth)) re = "[F4A460]" + re + "[-]";
        if (differ(card.Attribute, (int) CardAttribute.Water)) re = "[D1EEEE]" + re + "[-]";
        if (differ(card.Attribute, (int) CardAttribute.Fire)) re = "[F08080]" + re + "[-]";
        if (differ(card.Attribute, (int) CardAttribute.Wind)) re = "[B3EE3A]" + re + "[-]";
        if (differ(card.Attribute, (int) CardAttribute.Light)) re = "[EEEE00]" + re + "[-]";
        if (differ(card.Attribute, (int) CardAttribute.Dark)) re = "[FF00FF]" + re + "[-]";

        return re;
    }

    public static string getType(Card card)
    {
        var re = "";
        try
        {
            if (differ(card.Type, (long) CardType.Monster)) re += "[ff8000]" + mainType(card.Type);
            else if (differ(card.Type, (long) CardType.Spell)) re += "[7FFF00]" + mainType(card.Type);
            else if (differ(card.Type, (long) CardType.Trap)) re += "[dda0dd]" + mainType(card.Type);
            else re += "[ff8000]" + mainType(card.Type);
            re += "[-]";
        }
        catch (Exception e)
        {
        }

        return re;
    }

    public static string getSmall(Card data)
    {
        var re = "";

        try
        {
            if ((data.Type & (int) CardType.Monster) > 0)
            {
                re += "[ff8000]";
                re += "[" + secondType(data.Type) + "]";

                if ((data.Type & (int) CardType.Link) == 0)
                {
                    if ((data.Type & (int) CardType.Xyz) > 0)
                        re += " " + race(data.Race) + fen + attribute(data.Attribute) + fen + data.Level +
                              "[sup]☆[/sup]";
                    else
                        re += " " + race(data.Race) + fen + attribute(data.Attribute) + fen + data.Level +
                              "[sup]★[/sup]";
                }
                else
                {
                    re += " " + race(data.Race) + fen + attribute(data.Attribute);
                }

                if (data.LScale > 0) re += fen + data.LScale + "[sup]P[/sup]";
                re += "\n";
                if (data.Attack < 0)
                {
                    re += "[sup]ATK[/sup]?  ";
                }
                else
                {
                    if (data.rAttack > 0)
                    {
                        var pos = data.Attack - data.rAttack;
                        if (pos > 0) re += "[sup]ATK[/sup]" + data.Attack + "(↑" + pos + ")  ";
                        if (pos < 0) re += "[sup]ATK[/sup]" + data.Attack + "(↓" + (-pos) + ")  ";
                        if (pos == 0) re += "[sup]ATK[/sup]" + data.Attack + "  ";
                    }
                    else
                    {
                        re += "[sup]ATK[/sup]" + data.Attack + "  ";
                    }
                }

                if ((data.Type & (int) CardType.Link) == 0)
                {
                    if (data.Defense < 0)
                    {
                        re += "[sup]DEF[/sup]?";
                    }
                    else
                    {
                        if (data.rAttack > 0)
                        {
                            var pos = data.Defense - data.rDefense;
                            if (pos > 0) re += "[sup]DEF[/sup]" + data.Defense + "(↑" + pos + ")";
                            if (pos < 0) re += "[sup]DEF[/sup]" + data.Defense + "(↓" + (-pos) + ")";
                            if (pos == 0) re += "[sup]DEF[/sup]" + data.Defense;
                        }
                        else
                        {
                            re += "[sup]DEF[/sup]" + data.Defense;
                        }
                    }
                }
                else
                {
                    re += "[sup]LINK[/sup]" + data.Level;
                }
            }
            else if ((data.Type & ((int) CardType.Spell + (int) CardType.Trap)) > 0)
            {
                re += (data.Type & (int) CardType.Spell) > 0 ? "[7FFF00]" : "[DDA0DD]";
                re += secondType(data.Type);
                if (data.LScale > 0) re += fen + data.LScale + "[sup]P[/sup]";

                var original = CardsManager.GetCard(data.Id);

                if ((original.Type & (int) CardType.Monster) > 0)
                {
                    re += "\n";
                    re += "[999999]";
                    re += "[" + secondType(original.Type) + "]";

                    if ((original.Type & (int) CardType.Link) == 0)
                    {
                        if ((original.Type & (int) CardType.Xyz) > 0)
                            re += " " + race(original.Race) + fen + attribute(original.Attribute) + fen +
                                  original.Level + "[sup]☆[/sup]";
                        else
                            re += " " + race(original.Race) + fen + attribute(original.Attribute) + fen +
                                  original.Level + "[sup]★[/sup]";
                    }
                    else
                    {
                        re += " " + race(original.Race) + fen + attribute(original.Attribute);
                    }

                    if (original.LScale > 0) re += fen + original.LScale + "[sup]P[/sup]";
                    re += "\n";
                    if (original.Attack < 0)
                        re += "[sup]ATK[/sup]?  ";
                    else
                        re += "[sup]ATK[/sup]" + original.Attack + "  ";
                    if ((original.Type & (int) CardType.Link) == 0)
                    {
                        if (original.Defense < 0)
                            re += "[sup]DEF[/sup]?";
                        else
                            re += "[sup]DEF[/sup]" + original.Defense;
                    }
                    else
                    {
                        re += "[sup]LINK[/sup]" + original.Level;
                    }

                    re += "[-]";
                }
            }
            else
            {
                re += "[ff8000]";
            }

            if (data.Alias > 0)
                if (data.Alias != data.Id)
                {
                    var name = CardsManager.Get(data.Alias).Name;
                    if (name != data.Name) re += "\n[" + CardsManager.Get(data.Alias).Name + "]";
                }

            if (data.Setcode > 0)
            {
                re += "\n";
                re += xilie;
                re += getSetName(data.Setcode);
            }

            re += "[-]";
        }
        catch (Exception e)
        {
        }

        return re;
    }

    public static string getSearchResult(Card data)
    {
        var re = "";

        try
        {
            if ((data.Type & 0x1) > 0)
            {
                re += "[ff8000]";

                if ((data.Type & (int) CardType.Link) == 0)
                {
                    if ((data.Type & (int) CardType.Xyz) > 0)
                        re += race(data.Race) + fen + attribute(data.Attribute) + fen + data.Level + "[sup]☆[/sup]";
                    else
                        re += race(data.Race) + fen + attribute(data.Attribute) + fen + data.Level + "[sup]★[/sup]";
                }
                else
                {
                    re += race(data.Race) + fen + attribute(data.Attribute);
                }

                if (data.LScale > 0) re += fen + data.LScale + "[sup]P[/sup]";
                re += "\n";
                if (data.Attack < 0)
                {
                    re += "[sup]ATK[/sup]?  ";
                }
                else
                {
                    if (data.rAttack > 0)
                    {
                        var pos = data.Attack - data.rAttack;
                        if (pos > 0) re += "[sup]ATK[/sup]" + data.Attack + "(↑" + pos + ")  ";
                        if (pos < 0) re += "[sup]ATK[/sup]" + data.Attack + "(↓" + (-pos) + ")  ";
                        if (pos == 0) re += "[sup]ATK[/sup]" + data.Attack + "  ";
                    }
                    else
                    {
                        re += "[sup]ATK[/sup]" + data.Attack + "  ";
                    }
                }

                if ((data.Type & (int) CardType.Link) == 0)
                {
                    if (data.Defense < 0)
                    {
                        re += "[sup]DEF[/sup]?";
                    }
                    else
                    {
                        if (data.rAttack > 0)
                        {
                            var pos = data.Defense - data.rDefense;
                            if (pos > 0) re += "[sup]DEF[/sup]" + data.Defense + "(↑" + pos + ")";
                            if (pos < 0) re += "[sup]DEF[/sup]" + data.Defense + "(↓" + (-pos) + ")";
                            if (pos == 0) re += "[sup]DEF[/sup]" + data.Defense;
                        }
                        else
                        {
                            re += "[sup]DEF[/sup]" + data.Defense;
                        }
                    }
                }
                else
                {
                    re += "[sup]LINK[/sup]" + data.Level;
                }
            }
            else if ((data.Type & 0x2) > 0)
            {
                re += "[7FFF00]";
                re += secondType(data.Type);
                if (data.LScale > 0) re += fen + data.LScale + "[sup]P[/sup]";
            }
            else if ((data.Type & 0x4) > 0)
            {
                re += "[dda0dd]";
                re += secondType(data.Type);
            }
            else
            {
                re += "[ff8000]";
            }

            if (data.Alias > 0)
                if (data.Alias != data.Id)
                {
                    var name = CardsManager.Get(data.Alias).Name;
                    if (name != data.Name) re += "\n[" + CardsManager.Get(data.Alias).Name + "]";
                }

            re += "[-]";
        }
        catch (Exception e)
        {
        }

        return re;
    }

    public static string getSetName(long Setcode)
    {
        var setcodes = new int[4];
        for(var j = 0; j < 4; j++)
        {
            setcodes[j] = (int)((Setcode >> j * 16) & 0xffff);
        }
        var returnValue = new List<string>();
        for (var i = 0; i < GameStringManager.xilies.Count; i++)
        {
            var currentHash = GameStringManager.xilies[i].hashCode;
            for(var j = 0; j < 4; j++)
            {
                if (currentHash == setcodes[j])
                {
                    var setArray = GameStringManager.xilies[i].content.Split('\t');
                    var setString = setArray[0];
                    //if (setArray.Length > 1)
                    //{
                    //    setString += "[sup]" + setArray[1] + "[/sup]";
                    //}
                    returnValue.Add(setString);
                }
            }
        }
        return String.Join("|", returnValue.ToArray());
    }
}