using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Config
{
    public static uint ClientVersion = 0x1354;

    private static readonly List<oneString> translations = new List<oneString>();

    private static readonly List<oneString> uits = new List<oneString>();

    private static string path;

    private static bool loaded;

    public static bool getEffectON(string raw)
    {
        return true;
    }

    public static void initialize(string path)
    {
        Config.path = path;
        if (File.Exists(path) == false) File.Create(path).Close();
        var txtString = File.ReadAllText(path);
        var lines = txtString.Replace("\r", "").Split("\n");
        for (var i = 0; i < lines.Length; i++)
        {
            var mats = lines[i].Split("->");
            if (mats.Length == 2)
            {
                var s = new oneString();
                s.original = mats[0];
                s.translated = mats[1];
                translations.Add(s);
            }
        }
    }

    public static string Getui(string original)
    {
        if (loaded == false)
        {
            loaded = true;
            var lines = File.ReadAllText("texture/ui/config.txt").Replace("\r", "").Replace(" ", "").Split("\n");
            for (var i = 0; i < lines.Length; i++)
            {
                var mats = lines[i].Split("=");
                if (mats.Length == 2)
                {
                    var s = new oneString();
                    s.original = mats[0];
                    s.translated = mats[1];
                    uits.Add(s);
                }
            }
        }

        var return_value = "";
        for (var i = 0; i < uits.Count; i++)
            if (uits[i].original == original)
            {
                return_value = uits[i].translated;
                break;
            }

        return return_value;
    }

    internal static float getFloat(string v)
    {
        var getted = 0;
        try
        {
            getted = int.Parse(Get(v, "0"));
        }
        catch (Exception)
        {
        }

        return getted / 100000f;
    }

    internal static void setFloat(string v, float f)
    {
        Set(v, ((int) (f * 100000f)).ToString());
    }

    public static string Get(string original, string defau)
    {
        var return_value = defau;
        var finded = false;
        for (var i = 0; i < translations.Count; i++)
            if (translations[i].original == original)
            {
                return_value = translations[i].translated;
                finded = true;
                break;
            }

        if (finded == false)
            if (path != null)
            {
                File.AppendAllText(path, original + "->" + defau + "\r\n");
                var s = new oneString();
                s.original = original;
                s.translated = defau;
                return_value = defau;
                translations.Add(s);
            }

        return return_value;
    }

    public static void Set(string original, string setted)
    {
        var finded = false;
        for (var i = 0; i < translations.Count; i++)
            if (translations[i].original == original)
            {
                finded = true;
                translations[i].translated = setted;
            }

        if (finded == false)
        {
            var s = new oneString();
            s.original = original;
            s.translated = setted;
            translations.Add(s);
        }

        var all = "";
        for (var i = 0; i < translations.Count; i++)
            all += translations[i].original + "->" + translations[i].translated + "\r\n";
        try
        {
            File.WriteAllText(path, all);
        }
        catch (Exception e)
        {
            Program.noAccess = true;
            Debug.Log(e);
        }
    }

    private class oneString
    {
        public string original = "";
        public string translated = "";
    }
}