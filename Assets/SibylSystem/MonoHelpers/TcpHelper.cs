﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using YGOSharp;
using YGOSharp.Network.Enums;
using YGOSharp.OCGWrapper.Enums;

public static class TcpHelper
{
    public static TcpClient tcpClient;

    private static NetworkStream networkStream;

    private static bool canjoin = true;

    public static bool onDisConnected;

    private static readonly List<byte[]> datas = new List<byte[]>();

    private static readonly object locker = new object();

    public static Deck deck;

    public static List<string> deckStrings = new List<string>();

    public static List<Package> packagesInRecord = new List<Package>();

    public static string lastRecordName = "";

    public static void join(string ipString, string name, string portString, string pswString, string version)
    {
        if (canjoin)
        {
            if (tcpClient == null || tcpClient.Connected == false)
            {
                canjoin = false;
                try
                {
                    tcpClient = new TcpClientWithTimeout(ipString, int.Parse(portString), 3000).Connect();
                    networkStream = tcpClient.GetStream();
                    var t = new Thread(receiver);
                    t.Start();
                    CtosMessage_PlayerInfo(name);
                    CtosMessage_JoinGame(pswString, version);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    Program.DEBUGLOG("onDisConnected 10");
                }

                canjoin = true;
            }
        }
        else
        {
            onDisConnected = true;
            Program.DEBUGLOG("onDisConnected 1");
        }
    }

    public static void receiver()
    {
        try
        {
            while (tcpClient != null && networkStream != null && tcpClient.Connected && Program.Running)
            {
                var data = SocketMaster.ReadPacket(networkStream);
                addDateJumoLine(data);
            }

            onDisConnected = true;
            Program.DEBUGLOG("onDisConnected 2");
        }
        catch (Exception e)
        {
            onDisConnected = true;
            Program.DEBUGLOG("onDisConnected 3");
        }
    }

    public static void addDateJumoLine(byte[] data)
    {
        Monitor.Enter(datas);
        try
        {
            datas.Add(data);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        Monitor.Exit(datas);
    }

    public static void preFrameFunction()
    {
        if (datas.Count > 0)
            if (Monitor.TryEnter(datas))
            {
                for (var i = 0; i < datas.Count; i++)
                    try
                    {
                        var memoryStream = new MemoryStream(datas[i]);
                        var r = new BinaryReader(memoryStream);
                        var ms = (StocMessage) r.ReadByte();
                        switch (ms)
                        {
                            case StocMessage.GameMsg:
                                Program.I().room.StocMessage_GameMsg(r);
                                break;
                            case StocMessage.ErrorMsg:
                                Program.I().room.StocMessage_ErrorMsg(r);
                                break;
                            case StocMessage.SelectHand:
                                Program.I().room.StocMessage_SelectHand(r);
                                break;
                            case StocMessage.SelectTp:
                                Program.I().room.StocMessage_SelectTp(r);
                                break;
                            case StocMessage.HandResult:
                                Program.I().room.StocMessage_HandResult(r);
                                break;
                            case StocMessage.TpResult:
                                Program.I().room.StocMessage_TpResult(r);
                                break;
                            case StocMessage.ChangeSide:
                                Program.I().room.StocMessage_ChangeSide(r);
                                SaveRecord();
                                break;
                            case StocMessage.WaitingSide:
                                Program.I().room.StocMessage_WaitingSide(r);
                                SaveRecord();
                                break;
                            case StocMessage.DeckCount:
                                Program.I().room.StocMessage_DeckCount(r);
                                break;
                            case StocMessage.CreateGame:
                                Program.I().room.StocMessage_CreateGame(r);
                                break;
                            case StocMessage.JoinGame:
                                Program.I().room.StocMessage_JoinGame(r);
                                break;
                            case StocMessage.TypeChange:
                                Program.I().room.StocMessage_TypeChange(r);
                                break;
                            case StocMessage.LeaveGame:
                                Program.I().room.StocMessage_LeaveGame(r);
                                break;
                            case StocMessage.DuelStart:
                                Program.I().room.StocMessage_DuelStart(r);
                                break;
                            case StocMessage.DuelEnd:
                                Program.I().room.StocMessage_DuelEnd(r);
                                SaveRecord();
                                break;
                            case StocMessage.Replay:
                                Program.I().room.StocMessage_Replay(r);
                                SaveRecord();
                                break;
                            case StocMessage.TimeLimit:
                                Program.I().ocgcore.StocMessage_TimeLimit(r);
                                break;
                            case StocMessage.Chat:
                                Program.I().room.StocMessage_Chat(r);
                                break;
                            case StocMessage.HsPlayerEnter:
                                Program.I().room.StocMessage_HsPlayerEnter(r);
                                break;
                            case StocMessage.HsPlayerChange:
                                Program.I().room.StocMessage_HsPlayerChange(r);
                                break;
                            case StocMessage.HsWatchChange:
                                Program.I().room.StocMessage_HsWatchChange(r);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        // Program.DEBUGLOG(e);
                    }

                datas.Clear();
                Monitor.Exit(datas);
            }

        if (onDisConnected)
        {
            onDisConnected = false;
            Program.I().ocgcore.returnServant = Program.I().selectServer;
            if (tcpClient != null)
                if (tcpClient.Connected)
                {
                    tcpClient.Client.Shutdown(0);
                    tcpClient.Close();
                }

            tcpClient = null;
            if (Program.I().ocgcore.isShowed == false)
            {
                if (Program.I().menu.isShowed == false) Program.I().shiftToServant(Program.I().selectServer);
                Program.I().cardDescription.RMSshow_none(InterString.Get("连接被断开。"));
                packagesInRecord.Clear();
            }
            else
            {
                Program.I().cardDescription.RMSshow_none(InterString.Get("对方离开游戏，您现在可以截图。"));
                packagesInRecord.Clear();
                Program.I().ocgcore.forceMSquit();
            }
        }
    }

    public static void Send(Package message)
    {
        if (tcpClient != null && tcpClient.Connected)
        {
            var t = new Thread(sender);
            t.Start(message);
        }
    }

    // public static void SendSync(Package message)
    // {
    //     if (tcpClient != null && tcpClient.Connected)
    //     {
    //         sender(message);
    //     }
    // }

    private static void sender(object o)
    {
        try
        {
            lock (locker)
            {
                var message = (Package) o;
                var data = message.Data.get();
                var memstream = new MemoryStream();
                var b = new BinaryWriter(memstream);
                b.Write(BitConverter.GetBytes((short) data.Length + 1), 0, 2);
                b.Write(BitConverter.GetBytes((byte) message.Fuction), 0, 1);
                b.Write(data, 0, data.Length);
                var s = memstream.ToArray();
                Debug.Log(BitConverter.ToString(s));
                tcpClient.Client.Send(s);
            }
        }
        catch (Exception e)
        {
            onDisConnected = true;
            Program.DEBUGLOG("onDisConnected 5");
        }
    }

    public static void CtosMessage_Response(byte[] response)
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.Response;
        message.Data.writer.Write(response);
        Send(message);
    }

    public static void CtosMessage_UpdateDeck(Deck deckFor)
    {
        if (deckFor.Main.Count == 0)
            return;
        deckStrings.Clear();
        deck = deckFor;
        var message = new Package();
        message.Fuction = (int) CtosMessage.UpdateDeck;
        message.Data.writer.Write(deckFor.Main.Count + deckFor.Extra.Count);
        message.Data.writer.Write(deckFor.Side.Count);
        for (var i = 0; i < deckFor.Main.Count; i++)
        {
            message.Data.writer.Write(deckFor.Main[i]);
            var c = CardsManager.Get(deckFor.Main[i]);
            deckStrings.Add(c.Name);
        }

        for (var i = 0; i < deckFor.Extra.Count; i++) message.Data.writer.Write(deckFor.Extra[i]);
        for (var i = 0; i < deckFor.Side.Count; i++) message.Data.writer.Write(deckFor.Side[i]);
        Send(message);
    }

    public static void CtosMessage_HandResult(int res)
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.HandResult;
        message.Data.writer.Write((byte) res);
        Send(message);
    }

    public static void CtosMessage_TpResult(bool tp)
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.TpResult;
        if (tp)
            message.Data.writer.Write((byte) 1);
        else
            message.Data.writer.Write((byte) 0);
        Send(message);
    }

    public static void CtosMessage_PlayerInfo(string name)
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.PlayerInfo;
        message.Data.writer.WriteUnicode(name, 20);
        Send(message);
    }

    public static void CtosMessage_CreateGame()
    {
    }

    public static void CtosMessage_JoinGame(string psw, string version)
    {
        deckStrings.Clear();
        var message = new Package();
        message.Fuction = (int) CtosMessage.JoinGame;
        //Config.ClientVersion = (uint)GameStringManager.helper_stringToInt(version);
        message.Data.writer.Write((short) Config.ClientVersion);
        message.Data.writer.Write((byte) 204);
        message.Data.writer.Write((byte) 204);
        message.Data.writer.Write(0);
        message.Data.writer.WriteUnicode(psw, 20);
        Send(message);
    }

    public static void CtosMessage_LeaveGame()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.LeaveGame;
        Send(message);
    }

    public static void CtosMessage_Surrender()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.Surrender;
        Send(message);
    }

    public static void CtosMessage_TimeConfirm()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.TimeConfirm;
        Send(message);
    }

    public static void CtosMessage_Chat(string str)
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.Chat;
        message.Data.writer.WriteUnicode(str, str.Length + 1);
        Send(message);
    }

    public static void CtosMessage_HsToDuelist()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.HsToDuelist;
        Send(message);
    }

    public static void CtosMessage_HsToObserver()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.HsToObserver;
        Send(message);
    }

    public static void CtosMessage_HsReady()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.HsReady;
        Send(message);
    }

    public static void CtosMessage_HsNotReady()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.HsNotReady;
        Send(message);
    }

    public static void CtosMessage_HsKick(int pos)
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.HsKick;
        message.Data.writer.Write((byte) pos);
        Send(message);
    }

    public static void CtosMessage_HsStart()
    {
        var message = new Package();
        message.Fuction = (int) CtosMessage.HsStart;
        Send(message);
    }

    public static List<Package> readPackagesInRecord(string path)
    {
        List<Package> re = null;
        try
        {
            re = getPackages(File.ReadAllBytes(path));
        }
        catch (Exception e)
        {
            re = new List<Package>();
            Debug.Log(e);
        }

        return re;
    }

    public static List<Package> getPackages(byte[] buffer)
    {
        var re = new List<Package>();
        try
        {
            BinaryReader reader;
            using (reader = new BinaryReader(new MemoryStream(buffer)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    var p = new Package();
                    p.Fuction = reader.ReadByte();
                    p.Data = new BinaryMaster(reader.ReadBytes((int) reader.ReadUInt32()));
                    re.Add(p);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return re;
    }

    public static void SaveRecord()
    {
        try
        {
            if (packagesInRecord.Count > 10)
            {
                var write = false;
                var i = 0;
                var startI = 0;
                foreach (var item in packagesInRecord)
                {
                    i++;
                    try
                    {
                        if (item.Fuction == (int) GameMessage.Start)
                        {
                            write = true;
                            startI = i;
                        }

                        if (item.Fuction == (int) GameMessage.ReloadField)
                        {
                            write = true;
                            startI = i;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }
                }

                if (write)
                {
                    if (startI > packagesInRecord.Count) startI = packagesInRecord.Count;
                    packagesInRecord.Insert(startI, Program.I().ocgcore.getNamePacket());
                    if (File.Exists("replay/" + lastRecordName + ".yrp3d"))
                        File.Delete("replay/" + lastRecordName + ".yrp3d");
                    lastRecordName = UIHelper.getTimeString();
                    var stream = File.Create("replay/" + lastRecordName + ".yrp3d");
                    var writer = new BinaryWriter(stream);
                    foreach (var item in packagesInRecord)
                    {
                        writer.Write((byte) item.Fuction);
                        writer.Write((uint) item.Data.getLength());
                        writer.Write(item.Data.get());
                    }

                    stream.Flush();
                    writer.Close();
                    stream.Close();
                }
            }
            //packagesInRecord.Clear();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static void AddRecordLine(Package p)
    {
        if (Program.I().ocgcore.condition != Ocgcore.Condition.record) packagesInRecord.Add(p);
    }
}

public class Package
{
    public BinaryMaster Data;
    public int Fuction;

    public Package()
    {
        Fuction = (int) CtosMessage.Response;
        Data = new BinaryMaster();
    }
}

public class BinaryMaster
{
    private MemoryStream memstream;
    public BinaryReader reader;
    public BinaryWriter writer;

    public BinaryMaster(byte[] raw = null)
    {
        if (raw == null)
            memstream = new MemoryStream();
        else
            memstream = new MemoryStream(raw);
        reader = new BinaryReader(memstream);
        writer = new BinaryWriter(memstream);
    }

    public void set(byte[] raw)
    {
        memstream = new MemoryStream(raw);
        reader = new BinaryReader(memstream);
        writer = new BinaryWriter(memstream);
    }

    public byte[] get()
    {
        var bytes = memstream.ToArray();
        return bytes;
    }

    public int getLength()
    {
        return (int) memstream.Length;
    }

    public override string ToString()
    {
        var return_value = "";
        var bytes = get();
        for (var i = 0; i < bytes.Length; i++)
        {
            return_value += ((int) bytes[i]).ToString();
            if (i < bytes.Length - 1) return_value += ",";
        }

        return return_value;
    }
}

public static class BinaryExtensions
{
    public static void WriteUnicode(this BinaryWriter writer, string text, int len)
    {
        try
        {
            var unicode = Encoding.Unicode.GetBytes(text);
            var result = new byte[len * 2];
            for (var i = 0; i < result.Length; i++) result[i] = 204;
            var max = len * 2 - 2;
            Array.Copy(unicode, result, unicode.Length > max ? max : unicode.Length);
            result[unicode.Length] = 0;
            result[unicode.Length + 1] = 0;
            writer.Write(result);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public static string ReadUnicode(this BinaryReader reader, int len)
    {
        var unicode = reader.ReadBytes(len * 2);
        var text = Encoding.Unicode.GetString(unicode);
        text = text.Substring(0, text.IndexOf('\0'));
        return text;
    }

    public static string ReadALLUnicode(this BinaryReader reader)
    {
        var unicode = reader.ReadToEnd();
        var text = Encoding.Unicode.GetString(unicode);
        text = text.Substring(0, text.IndexOf('\0'));
        return text;
    }

    public static byte[] ReadToEnd(this BinaryReader reader)
    {
        return reader.ReadBytes((int) (reader.BaseStream.Length - reader.BaseStream.Position));
    }

    public static GPS ReadGPS(this BinaryReader reader)
    {
        var a = new GPS();
        a.controller = (uint) Program.I().ocgcore.localPlayer(reader.ReadByte());
        a.location = reader.ReadByte();
        a.sequence = reader.ReadByte();
        a.position = reader.ReadByte();
        return a;
    }

    public static GPS ReadShortGPS(this BinaryReader reader)
    {
        var a = new GPS();
        a.controller = (uint) Program.I().ocgcore.localPlayer(reader.ReadByte());
        a.location = reader.ReadByte();
        a.sequence = reader.ReadByte();
        a.position = (int) CardPosition.FaceUpAttack;
        return a;
    }

    public static void readCardData(this BinaryReader r, gameCard cardTemp = null)
    {
        var cardToRefresh = cardTemp;
        var flag = r.ReadInt32();
        var code = 0;
        var gps = new GPS();

        if ((flag & (int) Query.Code) != 0) code = r.ReadInt32();
        if ((flag & (int) Query.Position) != 0)
        {
            gps = r.ReadGPS();
            cardToRefresh = null;
            cardToRefresh = Program.I().ocgcore.GCS_cardGet(gps, false);
        }

        if (cardToRefresh == null) return;

        var data = cardToRefresh.get_data();

        if ((flag & (int) Query.Code) != 0)
            if (data.Id != code)
            {
                data = CardsManager.Get(code);
                data.Id = code;
            }

        if ((flag & (int) Query.Position) != 0) cardToRefresh.p = gps;


        if (data.Id > 0)
            if ((cardToRefresh.p.location & (uint) CardLocation.Hand) > 0)
                if (cardToRefresh.p.controller == 1)
                    cardToRefresh.p.position = (int) CardPosition.FaceUpAttack;

        if ((flag & (int) Query.Alias) != 0)
            data.Alias = r.ReadInt32();
        if ((flag & (int) Query.Type) != 0)
            data.Type = r.ReadInt32();

        var l1 = 0;
        if ((flag & (int) Query.Level) != 0) l1 = r.ReadInt32();
        var l2 = 0;
        if ((flag & (int) Query.Rank) != 0) l2 = r.ReadInt32();

        if ((flag & (int) Query.Attribute) != 0)
            data.Attribute = r.ReadInt32();
        if ((flag & (int) Query.Race) != 0)
            data.Race = r.ReadInt32();
        if ((flag & (int) Query.Attack) != 0)
            data.Attack = r.ReadInt32();
        if ((flag & (int) Query.Defence) != 0)
            data.Defense = r.ReadInt32();
        if ((flag & (int) Query.BaseAttack) != 0)
            r.ReadInt32();
        if ((flag & (int) Query.BaseDefence) != 0)
            r.ReadInt32();
        if ((flag & (int) Query.Reason) != 0)
            r.ReadInt32();
        if ((flag & (int) Query.ReasonCard) != 0)
            r.ReadInt32();
        if ((flag & (int) Query.EquipCard) != 0)
            cardToRefresh.addTarget(Program.I().ocgcore.GCS_cardGet(r.ReadGPS(), false));
        if ((flag & (int) Query.TargetCard) != 0)
        {
            var count = r.ReadInt32();
            for (var i = 0; i < count; ++i)
                cardToRefresh.addTarget(Program.I().ocgcore.GCS_cardGet(r.ReadGPS(), false));
        }

        if ((flag & (int) Query.OverlayCard) != 0)
        {
            var overs = Program.I().ocgcore.GCS_cardGetOverlayElements(cardToRefresh);
            var count = r.ReadInt32();
            for (var i = 0; i < count; ++i)
                if (i < overs.Count)
                    overs[i].set_code(r.ReadInt32());
                else
                    r.ReadInt32();
        }

        if ((flag & (int) Query.Counters) != 0)
        {
            var count = r.ReadInt32();
            for (var i = 0; i < count; ++i)
                r.ReadInt32();
        }

        if ((flag & (int) Query.Owner) != 0)
            r.ReadInt32();
        if ((flag & (int) Query.Status) != 0)
        {
            var status = r.ReadInt32();
            cardToRefresh.disabled = (status & 0x0001) == 0x0001;
            cardToRefresh.SemiNomiSummoned = (status & 0x0008) == 0x0008;
        }

        if ((flag & (int) Query.LScale) != 0)
            data.LScale = r.ReadInt32();
        if ((flag & (int) Query.RScale) != 0)
            data.RScale = r.ReadInt32();
        var l3 = 0;
        if ((flag & (int) Query.Link) != 0)
        {
            l3 = r.ReadInt32(); //link value
            data.LinkMarker = r.ReadInt32();
        }

        if ((flag & (int) Query.Level) != 0 || (flag & (int) Query.Rank) != 0 || (flag & (int) Query.Link) != 0)
        {
            if (l1 > l2)
                data.Level = l1;
            else
                data.Level = l2;
            if (l3 > data.Level)
                data.Level = l3;
        }

        cardToRefresh.set_data(data);
        //
    }
}

public class SocketMaster
{
    private static byte[] ReadFull(NetworkStream stream, int length)
    {
        var buf = new byte[length];
        var rlen = 0;
        while (rlen < buf.Length)
        {
            var currentLength = stream.Read(buf, rlen, buf.Length - rlen);
            rlen += currentLength;
            if (currentLength == 0)
            {
                TcpHelper.onDisConnected = true;
                Program.DEBUGLOG("onDisConnected 6");
                break;
            }
        }

        return buf;
    }

    public static byte[] ReadPacket(NetworkStream stream)
    {
        var hdr = ReadFull(stream, 2);
        var plen = BitConverter.ToUInt16(hdr, 0);
        var buf = ReadFull(stream, plen);
        return buf;
    }
}

public class TcpClientWithTimeout
{
    protected string _hostname;
    protected int _port;
    protected int _timeout_milliseconds;
    protected bool connected;
    protected TcpClient connection;
    protected Exception exception;

    public TcpClientWithTimeout(string hostname, int port, int timeout_milliseconds)
    {
        _hostname = hostname;
        _port = port;
        _timeout_milliseconds = timeout_milliseconds;
    }

    public TcpClient Connect()
    {
        // kick off the thread that tries to connect
        connected = false;
        exception = null;
        var thread = new Thread(BeginConnect);
        thread.IsBackground = true; // 作为后台线程处理
        // 不会占用机器太长的时间
        thread.Start();

        // 等待如下的时间
        thread.Join(_timeout_milliseconds);

        if (connected)
        {
            // 如果成功就返回TcpClient对象
            thread.Abort();
            return connection;
        }

        if (exception != null)
        {
            // 如果失败就抛出错误
            thread.Abort();
            TcpHelper.onDisConnected = true;
            Program.DEBUGLOG("onDisConnected 7");
            throw exception;
        }

        // 同样地抛出错误
        thread.Abort();
        var message = string.Format("TcpClient connection to {0}:{1} timed out",
            _hostname, _port);
        TcpHelper.onDisConnected = true;
        Program.DEBUGLOG("onDisConnected 8");
        throw new TimeoutException(message);
    }

    protected void BeginConnect()
    {
        try
        {
            connection = new TcpClient(_hostname, _port);
            // 标记成功，返回调用者
            connected = true;
        }
        catch (Exception ex)
        {
            // 标记失败
            exception = ex;
        }
    }
}