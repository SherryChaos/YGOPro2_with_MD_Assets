using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Menu : WindowServantSP
{
    private static int lastTime;
    private bool msgPermissionShowed;

    private bool msgUpdateShowed;
    private string uptxt = "";

    private string upurl = "";

    //GameObject screen;
    public override void initialize()
    {
        SetWindow(Program.I().new_ui_menu);
        UIHelper.registEvent(gameObject, "setting_", onClickSetting);
        UIHelper.registEvent(gameObject, "deck_", onClickSelectDeck);
        UIHelper.registEvent(gameObject, "online_", onClickOnline);
        UIHelper.registEvent(gameObject, "replay_", onClickReplay);
        UIHelper.registEvent(gameObject, "single_", onClickPizzle);
        UIHelper.registEvent(gameObject, "ai_", Program.gugugu);
        UIHelper.registEvent(gameObject, "exit_", onClickExit);
        Program.I().StartCoroutine(checkUpdate());
    }

    public override void show()
    {
        base.show();
        Program.charge();
    }

    private IEnumerator checkUpdate()
    {
        yield return new WaitForSeconds(1);
        var verFile = File.ReadAllLines("config/ver.txt", Encoding.UTF8);
        if (verFile.Length == 0) // 放一个空的 ver.txt 以关闭自动更新功能
            yield break;
        if (verFile.Length != 2 || !Uri.IsWellFormedUriString(verFile[1], UriKind.Absolute))
        {
            Program.PrintToChat(InterString.Get("YGOPro2 自动更新：[ff5555]未设置更新服务器，无法检查更新。[-]@n请从官网重新下载安装完整版以获得更新。"));
            yield break;
        }

        var ver = verFile[0];
        var url = verFile[1];
        var www = UnityWebRequest.Get(url);
        www.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
        www.SetRequestHeader("Pragma", "no-cache");
        yield return www.Send();
        try
        {
            var result = www.downloadHandler.text;
            var lines = result.Replace("\r", "").Split("\n");
            var mats = lines[0].Split(":.:");
            if (ver != mats[0])
            {
                upurl = mats[1];
                for (var i = 1; i < lines.Length; i++) uptxt += lines[i] + "\n";
            }
            else
            {
                Program.PrintToChat(InterString.Get("YGOPro2 自动更新：[55ff55]当前已是最新版本。[-]"));
            }
        }
        catch (Exception e)
        {
            Program.PrintToChat(InterString.Get("YGOPro2 自动更新：[ff5555]检查更新失败！[-]"));
        }
    }

    public override void ES_RMS(string hashCode, List<messageSystemValue> result)
    {
        base.ES_RMS(hashCode, result);
        if (hashCode == "update" && result[0].value == "1") Application.OpenURL(upurl);
    }

    public override void preFrameFunction()
    {
        base.preFrameFunction();
        if (Program.noAccess && !msgPermissionShowed)
        {
            msgPermissionShowed = true;
            Program.PrintToChat(InterString.Get("[b][FF0000]NO ACCESS!! NO ACCESS!! NO ACCESS!![-][/b]") + "\n" +
                                InterString.Get("访问程序目录出错，软件大部分功能将无法使用。@n请将 YGOPro2 安装到其他文件夹，或以管理员身份运行。"));
        }
        else if (upurl != "" && !msgUpdateShowed)
        {
            msgUpdateShowed = true;
            RMSshow_yesOrNo("update",
                InterString.Get("[b]发现更新！[/b]") + "\n" + uptxt + "\n" + InterString.Get("是否打开下载页面？"),
                new messageSystemValue {value = "1", hint = "yes"}, new messageSystemValue {value = "0", hint = "no"});
        }
    }

    public void onClickExit()
    {
        Program.I().quit();
        Program.Running = false;
        TcpHelper.SaveRecord();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void onClickOnline()
    {
        Program.I().shiftToServant(Program.I().selectServer);
    }

    // private void onClickAI()
    // {
    //     Program.I().shiftToServant(Program.I().aiRoom);
    // }

    private void onClickPizzle()
    {
        Program.I().shiftToServant(Program.I().puzzleMode);
    }

    private void onClickReplay()
    {
        Program.I().shiftToServant(Program.I().selectReplay);
    }

    private void onClickSetting()
    {
        Program.I().setting.show();
    }

    private void onClickSelectDeck()
    {
        Program.I().shiftToServant(Program.I().selectDeck);
    }
}