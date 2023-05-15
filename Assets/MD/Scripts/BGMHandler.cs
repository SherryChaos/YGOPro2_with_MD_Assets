using DefaultNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BGMHandler : MonoBehaviour
{
    public static float vol;
    AudioSource audioSource;
    static int bgmIndex;
    static bool climax;
    static bool keycard;
    public static string fieldID = "001";
    private void Start()
    {
        bgmIndex = 1;
        audioSource = GetComponent<AudioSource>();
        PlayBGM("menu");
        vol = 0.7f;
    }

    public static void PlayBGM(string type)
    {
        AudioSource audioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
        AudioClip clip = GetClip(GetBgmByType(type));
        audioSource.clip = clip;
        audioSource.Play();
    }
    public static void ChangeBGM(string type)
    {
        BGMHandler handler = GameObject.Find("BGM").GetComponent<BGMHandler>();
        if (type == "duel_climax" && !climax)
        {
            climax = true;
            handler.StartBgmChange(type);
        }
        else if(type == "duel_climax" && climax)
        {
        }
        else if (type == "duel_keycard" && !keycard && !climax)
        {
            keycard = true;
            handler.StartBgmChange(type);
        }
        else if (type == "duel_keycard" && (keycard || climax))
        {
        }
        else
        {
            keycard = false;
            climax = false;
            handler.StartBgmChange(type);
        }   

    }

    void StartBgmChange(string type)
    {
        StartCoroutine(ChangeBGMFade(type, vol, vol));
    }
    public static void ChangeBgmVol(float vol)
    {
        AudioSource audioSource = GameObject.Find("BGM").GetComponent<AudioSource>();
        audioSource.volume = vol;
    }

    static AudioClip GetClip(string p)
    {
        var path = "sound/BGM/" + p + ".mp3";
        if (File.Exists(path) == false) path = "sound/BGM/" + p + ".wav";
        if (File.Exists(path) == false) path = "sound/BGM/" + p + ".ogg";
        if (File.Exists(path) == false) return null;
        path = Environment.CurrentDirectory.Replace("\\", "/") + "/" + path;
        path = "file:///" + path;
        WWW www = new WWW(path);
        return www.GetAudioClip(true, true);
    }

    IEnumerator ChangeBGMFade(string type, float vol, float currentVol, bool changed = false)
    {
        if(currentVol > 0 && !changed)
        {
            currentVol = currentVol - 0.01f;
            audioSource.volume = currentVol;
            yield return new WaitForSeconds(0.01f);
            yield return ChangeBGMFade(type, vol, currentVol);
            yield break;
        }
        if(currentVol <= 0 || changed)
        {
            if (!changed)
            {
                audioSource.clip = GetClip(GetBgmByType(type));
                audioSource.Play();
                changed = true;
            }
            currentVol = currentVol + 0.01f;
            audioSource.volume = currentVol;
            if (currentVol < vol)
            {
                yield return new WaitForSeconds(0.01f);
                yield return ChangeBGMFade(type, vol, currentVol, changed);
            }
        }
    }
    static string GetBgmByType(string type)
    {
        switch (type)
        {
            case "menu":
                return "BGM_MENU_01";
            case "deck":
                return "BGM_MENU_02";
            case "duel_normal":
                return "BGM_DUEL_NORMAL_" + GetBgmID();
            case "duel_keycard":
                return "BGM_DUEL_KEYCARD_" + GetBgmID();
            case "duel_climax":
                return "BGM_DUEL_CLIMAX_" + GetBgmID();
            default:
                return "BGM_MENU_01";
        }
    }
    static string GetBgmID()
    {
        GameObject field = GameObject.Find("new_gameField(Clone)");
        switch (fieldID)
        {
            case "001":
                return "02";
            case "002":
                return "01";
            case "003":
                return "04";
            case "004":
                return "02";
            case "005":
                return "04";
            case "006":
                return "07";
            case "007":
                return "05";
            case "008":
                return "11";
            case "009":
                return "06";
            case "010":
                return "08";
            case "011":
                return "07";
            case "012":
                return "08";
            case "014":
                return "11";
            case "015":
                return "06";
            case "016":
                return "09";
            case "017":
                return "09";
            case "018":
                return "11";
            case "019":
                return "10";
            case "020":
                return "04";
            default:
                return "01";
        }
    }
    static string Index(int n)
    {
        if(n < 9)
        {
            return "0" + n.ToString(); 
        }
        else
        {
            return n.ToString();
        }
    }
}
