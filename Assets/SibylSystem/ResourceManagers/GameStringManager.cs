﻿using System;
using System.Collections.Generic;
using System.IO;
using YGOSharp;

public static class GameStringManager
{
    public static List<hashedString> hashedStrings = new List<hashedString>();

    public static List<hashedString> xilies = new List<hashedString>();

    public static int helper_stringToInt(string str)
    {
        var return_value = 0;
        try
        {
            if (str.Length > 2 && str.Substring(0, 2) == "0x")
                return_value = Convert.ToInt32(str, 16);
            else
                return_value = int.Parse(str);
        }
        catch (Exception)
        {
        }

        return return_value;
    }

    public static void initialize(string path)
    {
        var text = File.ReadAllText(path);
        initializeContent(text);
    }

    public static void initializeContent(string text)
    {
        var st = text.Replace("\r", "");
        var lines = st.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
            if (line.Length > 1 && line.Substring(0, 1) == "!")
            {
                var mats = line.Substring(1, line.Length - 1).Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                if (mats.Length > 2)
                {
                    var a = new hashedString();
                    a.region = mats[0];
                    try
                    {
                        a.hashCode = helper_stringToInt(mats[1]);
                    }
                    catch (Exception e)
                    {
                        Program.DEBUGLOG(e);
                    }

                    a.content = "";
                    for (var i = 2; i < mats.Length; i++) a.content += mats[i] + " ";
                    a.content = a.content.Substring(0, a.content.Length - 1);
                    if (get(a.region, a.hashCode) == "")
                    {
                        hashedStrings.Add(a);
                        if (a.region == "setname") xilies.Add(a);
                    }
                }
            }
    }

    public static string get(string region, int hashCode)
    {
        var re = "";
        foreach (var s in hashedStrings)
            if (s.region == region && s.hashCode == hashCode)
            {
                re = s.content;
                break;
            }

        return re;
    }

    internal static string get_unsafe(int hashCode)
    {
        var re = "";
        foreach (var s in hashedStrings)
            if (s.region == "system" && s.hashCode == hashCode)
            {
                re = s.content;
                break;
            }

        return re;
    }

    internal static string get(int description)
    {
        var a = "";
        if (description < 10000)
        {
            a = get("system", description);
        }
        else
        {
            var code = description >> 4;
            var index = description & 0xf;
            try
            {
                a = CardsManager.Get(code).Str[index];
            }
            catch (Exception e)
            {
                Program.DEBUGLOG(e);
            }
        }

        return a;
    }

    internal static string formatLocation(uint location, uint sequence)
    {
        if (location == 0x8)
        {
            if (sequence < 5)
                return get(1003);
            if (sequence == 5)
                return get(1008);
            return get(1009);
        }

        uint filter = 1;
        var i = 1000;
        for (; filter != 0x100 && filter != location; filter <<= 1)
            ++i;
        if (filter == location)
            return get(i);
        return "???";
    }

    internal static string formatLocation(GPS gps)
    {
        return formatLocation(gps.location, gps.sequence);
    }

    public class hashedString
    {
        public string content = "";
        public int hashCode;
        public string region = "";
    }
}