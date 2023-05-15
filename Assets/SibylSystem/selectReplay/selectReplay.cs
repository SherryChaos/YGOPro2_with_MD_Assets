using System;
using System.Collections.Generic;
using System.IO;
using Percy;
using SevenZip.Compression.LZMA;
using UnityEngine;
using GameMessage = YGOSharp.OCGWrapper.Enums.GameMessage;

public class selectReplay : WindowServantSP
{
    private readonly string sort = "sortByTimeReplay";
    private bool opYRP;

    private PrecyOcg precy;

    private string selectedTrace = "";
    private UIselectableList superScrollView;

    public override void initialize()
    {
        SetWindow(Program.I().remaster_replayManager);
        UIHelper.registEvent(gameObject, "exit_", onClickExit);
        superScrollView = gameObject.GetComponentInChildren<UIselectableList>();
        superScrollView.selectedAction = onSelected;
        UIHelper.registEvent(gameObject, "sort_", onSort);
        UIHelper.registEvent(gameObject, "launch_", onLaunch);
        UIHelper.registEvent(gameObject, "rename_", onRename);
        UIHelper.registEvent(gameObject, "delete_", onDelete);
        UIHelper.registEvent(gameObject, "yrp_", onYrp);
        UIHelper.registEvent(gameObject, "ydk_", onYdk);
        UIHelper.registEvent(gameObject, "god_", onGod);
        UIHelper.registEvent(gameObject, "value_", onValue);
        setSortLable();
        superScrollView.install();
        SetActiveFalse();
    }

    private void onValue()
    {
        RMSshow_yesOrNo(
            "onValue",
            InterString.Get("您确定要删除所有未命名的录像？"),
            new messageSystemValue {hint = "yes", value = "yes"},
            new messageSystemValue {hint = "no", value = "no"});
    }

    private void setSortLable()
    {
        if (Config.Get(sort, "1") == "1")
            UIHelper.trySetLableText(gameObject, "sort_", InterString.Get("时间排序"));
        else
            UIHelper.trySetLableText(gameObject, "sort_", InterString.Get("名称排序"));
    }

    private void onLaunch()
    {
        if (!superScrollView.Selected()) return;
        if (!isShowed) return;
        KF_replay(superScrollView.selectedString);
    }

    private void onGod()
    {
        if (!superScrollView.Selected()) return;
        if (!isShowed) return;
        KF_replay(superScrollView.selectedString, true);
    }

    private void onSort()
    {
        if (Config.Get(sort, "1") == "1")
            Config.Set(sort, "0");
        else
            Config.Set(sort, "1");
        setSortLable();
        printFile();
    }

    private void onRename()
    {
        if (!superScrollView.Selected()) return;
        var name = superScrollView.selectedString;
        if (name.Length > 4 && name.Substring(name.Length - 4, 4) == ".yrp")
        {
            opYRP = true;
            RMSshow_input("onRename", InterString.Get("请输入重命名后的录像名"), name.Substring(0, name.Length - 4));
        }
        else
        {
            opYRP = false;
            RMSshow_input("onRename", InterString.Get("请输入重命名后的录像名"), name);
        }
    }

    private void onDelete()
    {
        if (!superScrollView.Selected()) return;
        RMSshow_yesOrNo(
            "onDelete",
            InterString.Get("删除[?],@n请确认。",
                superScrollView.selectedString),
            new messageSystemValue {hint = "yes", value = "yes"},
            new messageSystemValue {hint = "no", value = "no"});
    }

    private List<byte[]> getYRPbuffer(string path)
    {
        if (path.Substring(path.Length - 4, 4) == ".yrp") return new List<byte[]> {File.ReadAllBytes(path)};
        var returnValue = new List<byte[]>();
        try
        {
            var collection = TcpHelper.readPackagesInRecord(path);
            foreach (var item in collection)
                if (item.Fuction == (int) GameMessage.sibyl_replay)
                {
                    var replay = item.Data.reader.ReadToEnd();
                    // TODO: don't include other replays
                    returnValue.Add(replay);
                }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return returnValue;
    }

    private YRP getYRP(byte[] buffer)
    {
        var returnValue = new YRP();
        try
        {
            var reader = new BinaryReader(new MemoryStream(buffer));
            returnValue.ID = reader.ReadInt32();
            returnValue.Version = reader.ReadInt32();
            returnValue.Flag = reader.ReadInt32();
            returnValue.Seed = reader.ReadUInt32();
            returnValue.DataSize = reader.ReadInt32();
            returnValue.Hash = reader.ReadInt32();
            returnValue.Props = reader.ReadBytes(8);
            var raw = reader.ReadToEnd();
            if ((returnValue.Flag & 0x1) > 0)
            {
                var lzma = new Decoder();
                lzma.SetDecoderProperties(returnValue.Props);
                var decompressed = new MemoryStream();
                lzma.Code(new MemoryStream(raw), decompressed, raw.LongLength, returnValue.DataSize, null);
                raw = decompressed.ToArray();
            }

            reader = new BinaryReader(new MemoryStream(raw));
            if ((returnValue.Flag & 0x2) > 0)
            {
                Program.I().room.mode = 2;
                returnValue.playerData.Add(new YRP.PlayerData());
                returnValue.playerData.Add(new YRP.PlayerData());
                returnValue.playerData.Add(new YRP.PlayerData());
                returnValue.playerData.Add(new YRP.PlayerData());
                returnValue.playerData[0].name = reader.ReadUnicode(20);
                returnValue.playerData[1].name = reader.ReadUnicode(20);
                returnValue.playerData[2].name = reader.ReadUnicode(20);
                returnValue.playerData[3].name = reader.ReadUnicode(20);
                returnValue.StartLp = reader.ReadInt32();
                returnValue.StartHand = reader.ReadInt32();
                returnValue.DrawCount = reader.ReadInt32();
                returnValue.opt = reader.ReadInt32();
                Program.I().ocgcore.MasterRule = returnValue.opt >> 16;
                for (var i = 0; i < 4; i++)
                {
                    var count = reader.ReadInt32();
                    for (var i2 = 0; i2 < count; i2++) returnValue.playerData[i].main.Add(reader.ReadInt32());
                    count = reader.ReadInt32();
                    for (var i2 = 0; i2 < count; i2++) returnValue.playerData[i].extra.Add(reader.ReadInt32());
                }
            }
            else
            {
                returnValue.playerData.Add(new YRP.PlayerData());
                returnValue.playerData.Add(new YRP.PlayerData());
                returnValue.playerData[0].name = reader.ReadUnicode(20);
                returnValue.playerData[1].name = reader.ReadUnicode(20);
                returnValue.StartLp = reader.ReadInt32();
                returnValue.StartHand = reader.ReadInt32();
                returnValue.DrawCount = reader.ReadInt32();
                returnValue.opt = reader.ReadInt32();
                Program.I().ocgcore.MasterRule = returnValue.opt >> 16;
                for (var i = 0; i < 2; i++)
                {
                    var count = reader.ReadInt32();
                    for (var i2 = 0; i2 < count; i2++) returnValue.playerData[i].main.Add(reader.ReadInt32());
                    count = reader.ReadInt32();
                    for (var i2 = 0; i2 < count; i2++) returnValue.playerData[i].extra.Add(reader.ReadInt32());
                }
            }

            while (reader.BaseStream.Position < reader.BaseStream.Length)
                returnValue.gameData.Add(reader.ReadBytes(reader.ReadByte()));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return returnValue;
    }

    private void onYdk()
    {
        if (!superScrollView.Selected()) return;
        try
        {
            YRP yrp;
            if (File.Exists("replay/" + superScrollView.selectedString))
                yrp = getYRP(File.ReadAllBytes("replay/" + superScrollView.selectedString));
            else
                yrp = getYRP(getYRPbuffer("replay/" + superScrollView.selectedString + ".yrp3d")[0]);
            for (var i = 0; i < yrp.playerData.Count; i++)
            {
                var value = "#created by ygopro2\r\n#main\r\n";
                for (var i2 = 0; i2 < yrp.playerData[i].main.Count; i2++) value += yrp.playerData[i].main[i2] + "\r\n";
                value += "#extra\r\n";
                for (var i2 = 0; i2 < yrp.playerData[i].extra.Count; i2++)
                    value += yrp.playerData[i].extra[i2] + "\r\n";
                var name = "deck/" + superScrollView.selectedString + "_" + (i + 1) + ".ydk";
                File.WriteAllText(name, value);
                RMSshow_none(InterString.Get("卡组入库：[?]", name));
            }

            if (yrp.playerData.Count == 0)
            {
                RMSshow_none(InterString.Get("录像没有录制完整。"));
                RMSshow_none(InterString.Get("MATCH局中可能只有最后一局决斗才包含卡组信息。"));
            }
        }
        catch (Exception)
        {
            RMSshow_none(InterString.Get("录像没有录制完整。"));
            RMSshow_none(InterString.Get("MATCH局中可能只有最后一局决斗才包含卡组信息。"));
        }
    }

    private void onYrp()
    {
        if (!superScrollView.Selected()) return;
        try
        {
            if (File.Exists("replay/" + superScrollView.selectedString + ".yrp3d"))
            {
                var replays = getYRPbuffer("replay/" + superScrollView.selectedString + ".yrp3d");
                for (var i = 1; i <= replays.Count; i++)
                {
                    var filename = "replay/" + superScrollView.selectedString + "-Game" + i + ".yrp";
                    File.WriteAllBytes(filename, replays[i - 1]);
                    RMSshow_none(InterString.Get("录像入库：[?]", filename));
                }

                printFile();
            }
            else
            {
                RMSshow_none(InterString.Get("录像没有录制完整。"));
                RMSshow_none(InterString.Get("MATCH局中可能只有最后一局决斗才包含旧版录像信息。"));
            }
        }
        catch (Exception)
        {
            RMSshow_none(InterString.Get("录像没有录制完整。"));
            RMSshow_none(InterString.Get("MATCH局中可能只有最后一局决斗才包含旧版录像信息。"));
        }
    }

    public override void ES_RMS(string hashCode, List<messageSystemValue> result)
    {
        base.ES_RMS(hashCode, result);
        if (hashCode == "onRename")
            try
            {
                if (opYRP)
                    File.Move("replay/" + superScrollView.selectedString, "replay/" + result[0].value + ".yrp");
                else
                    File.Move("replay/" + superScrollView.selectedString + ".yrp3d",
                        "replay/" + result[0].value + ".yrp3d");
                printFile();
                RMSshow_none(InterString.Get("重命名成功。"));
            }
            catch (Exception)
            {
                RMSshow_none(InterString.Get("重命名失败！请检查输入的文件名，以及文件夹权限。"));
            }

        if (hashCode == "onDelete")
            if (result[0].value == "yes")
                try
                {
                    if (File.Exists("replay/" + superScrollView.selectedString + ".yrp3d"))
                    {
                        File.Delete("replay/" + superScrollView.selectedString + ".yrp3d");
                        RMSshow_none(InterString.Get("[?]已经被删除。", superScrollView.selectedString));
                        printFile();
                    }

                    if (File.Exists("replay/" + superScrollView.selectedString))
                    {
                        File.Delete("replay/" + superScrollView.selectedString);
                        RMSshow_none(InterString.Get("[?]已经被删除。", superScrollView.selectedString));
                        printFile();
                    }
                }
                catch (Exception)
                {
                }

        if (hashCode == "onValue")
            if (result[0].value == "yes")
            {
                var fileInfos = new DirectoryInfo("replay").GetFiles();
                for (var i = 0; i < fileInfos.Length; i++)
                    if (fileInfos[i].Name.Length == 21 || fileInfos[i].Name.Length == 25)
                        if (fileInfos[i].Name[2] == '-')
                            if (fileInfos[i].Name[5] == '「')
                                if (fileInfos[i].Name[8] == '：')
                                    try
                                    {
                                        File.Delete("replay/" + fileInfos[i].Name);
                                    }
                                    catch (Exception)
                                    {
                                    }

                RMSshow_none(InterString.Get("清理完毕。"));
                printFile();
            }
    }

    private void onSelected()
    {
        if (selectedTrace == superScrollView.selectedString) KF_replay(selectedTrace);
        selectedTrace = superScrollView.selectedString;
    }

    public void KF_replay(string name, bool god = false)
    {
        string filename = name;
        if (!File.Exists(filename))
        {
            if (File.Exists("replay/" + name + ".yrp3d"))
                filename = "replay/" + name + ".yrp3d";
            if (name.Length > 4 && name.ToLower().Substring(name.Length - 4, 4) == ".yrp")
                filename = "replay/" + name;
            if (!File.Exists(filename))
                return;
        }
        bool yrp3d = filename.Length > 6 && filename.ToLower().Substring(filename.Length - 6, 6) == ".yrp3d";
        try
        {
            if (yrp3d)
            {
                if (god)
                {
                    RMSshow_none(InterString.Get("您正在观看旧版的录像（上帝视角），不保证稳定性。"));
                    if (precy != null)
                        precy.dispose();
                    precy = new PrecyOcg();
                    var replays = getYRPbuffer(filename);
                    var collections =
                        TcpHelper.getPackages(precy.ygopro.getYRP3dBuffer(getYRP(replays[replays.Count - 1])));
                    pushCollection(collections);
                }
                else
                {
                    var collection = TcpHelper.readPackagesInRecord(filename);
                    pushCollection(collection);
                }
            }
            else
            {
                RMSshow_none(InterString.Get("您正在观看旧版的录像（上帝视角），不保证稳定性。"));
                if (precy != null)
                    precy.dispose();
                precy = new PrecyOcg();
                var collections =
                    TcpHelper.getPackages(
                        precy.ygopro.getYRP3dBuffer(getYRP(File.ReadAllBytes(filename))));
                pushCollection(collections);
            }
        }
        catch (Exception)
        {
            RMSshow_none(InterString.Get("录像没有录制完整。"));
            RMSshow_none(InterString.Get("MATCH局中可能只有最后一局决斗才包含旧版录像信息。"));
        }
    }

    private void pushCollection(List<Package> collection)
    {
        Program.I().ocgcore.returnServant = Program.I().selectReplay;
        Program.I().ocgcore.handler = a => { };
        Program.I().ocgcore.name_0 = Config.Get("name", "一秒一喵机会");
        Program.I().ocgcore.name_0_c = Program.I().ocgcore.name_0;
        Program.I().ocgcore.name_1 = "Percy AI";
        Program.I().ocgcore.name_0_tag = "---";
        Program.I().ocgcore.name_1_tag = "---";
        Program.I().ocgcore.timeLimit = 240;
        Program.I().ocgcore.lpLimit = 8000;
        Program.I().ocgcore.isFirst = true;
        Program.I().shiftToServant(Program.I().ocgcore);
        Program.I().ocgcore.InAI = false;
        Program.I().ocgcore.shiftCondition(Ocgcore.Condition.record);
        Program.I().ocgcore.flushPackages(collection);
    }

    public override void show()
    {
        base.show();
        printFile();
        Program.charge();
    }

    private void printFile()
    {
        superScrollView.clear();
        var fileInfos = new DirectoryInfo("replay").GetFiles();
        if (Config.Get(sort, "1") == "1")
            Array.Sort(fileInfos, UIHelper.CompareTime);
        else
            Array.Sort(fileInfos, UIHelper.CompareName);
        for (var i = 0; i < fileInfos.Length; i++)
            if (fileInfos[i].Name.Length > 6)
            {
                if (fileInfos[i].Name.Length > 6 &&
                    fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 6, 6) == ".yrp3d")
                    superScrollView.add(fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 6));
                if (fileInfos[i].Name.Length > 4 &&
                    fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 4, 4) == ".yrp")
                    superScrollView.add(fileInfos[i].Name);
            }
    }

    private void onClickExit()
    {
        if (Program.exitOnReturn)
            Program.I().menu.onClickExit();
        else
            Program.I().shiftToServant(Program.I().menu);
    }
}