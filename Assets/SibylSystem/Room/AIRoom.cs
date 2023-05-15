﻿// using System;
// using System.IO;
// using Random = UnityEngine.Random;
//
// public class AIRoom : WindowServantSP
// {
//     private PrecyOcg precy;
//
//     public void launch(string playerDek, string aiDeck, string aiScript, bool playerGo, bool suffle, int life, bool god,
//         int rule)
//     {
//         if (precy != null) precy.dispose();
//         precy = new PrecyOcg();
//         precy.startAI(playerDek, aiDeck, aiScript, playerGo, suffle, life, god, rule);
//         RMSshow_none(InterString.Get("AI模式还在开发中，您在AI模式下遇到的BUG不会在联机的时候出现。"));
//     }
//
//     #region ui
//
//     private UIselectableList superScrollView;
//     private readonly string sort = "sortByTimeDeck";
//     private string suiji = "";
//
//     private UIPopupList list_aideck;
//     private UIPopupList list_airank;
//
//     public override void initialize()
//     {
//         suiji = InterString.Get("随机卡组");
//         SetWindow(Program.I().new_ui_aiRoom);
//         superScrollView = gameObject.GetComponentInChildren<UIselectableList>();
//         superScrollView.selectedAction = onSelected;
//         list_aideck = UIHelper.getByName<UIPopupList>(gameObject, "aideck_");
//         list_airank = UIHelper.getByName<UIPopupList>(gameObject, "rank_");
//         list_aideck.value = Config.Get("list_aideck", suiji);
//         list_airank.value = Config.Get("list_airank", "ai");
//         UIHelper.registEvent(gameObject, "aideck_", onSave);
//         UIHelper.registEvent(gameObject, "rank_", onSave);
//         UIHelper.registEvent(gameObject, "start_", onStart);
//         UIHelper.registEvent(gameObject, "exit_", onClickExit);
//         UIHelper.trySetLableText(gameObject, "percyHint", InterString.Get("人机模式"));
//         superScrollView.install();
//         SetActiveFalse();
//     }
//
//     private void onSelected()
//     {
//         Config.Set("deckInUse", superScrollView.selectedString);
//     }
//
//     private void onSave()
//     {
//         Config.Set("list_aideck", list_aideck.value);
//         Config.Set("list_airank", list_airank.value);
//     }
//
//     private void onClickExit()
//     {
//         if (Program.exitOnReturn)
//             Program.I().menu.onClickExit();
//         else
//             Program.I().shiftToServant(Program.I().menu);
//     }
//
//     private void onStart()
//     {
//         if (!isShowed) return;
//         var l = 8000;
//         try
//         {
//             l = int.Parse(UIHelper.getByName<UIInput>(gameObject, "life_").value);
//         }
//         catch (Exception)
//         {
//         }
//
//         var aideck = "";
//         if (Config.Get("list_aideck", suiji) == suiji)
//             aideck = "ai/ydk/" + list_aideck.items[Random.Range(1, list_aideck.items.Count)] + ".ydk";
//         else
//             aideck = "ai/ydk/" + Config.Get("list_aideck", suiji) + ".ydk";
//         launch("deck/" + Config.Get("deckInUse", "miaowu") + ".ydk", aideck,
//             "ai/" + Config.Get("list_airank", "ai") + ".lua", UIHelper.getByName<UIToggle>(gameObject, "first_").value,
//             UIHelper.getByName<UIToggle>(gameObject, "unrand_").value, l,
//             UIHelper.getByName<UIToggle>(gameObject, "god_").value,
//             UIHelper.getByName<UIToggle>(gameObject, "mr4_").value ? 4 : 3);
//     }
//
//     private void printFile()
//     {
//         var deckInUse = Config.Get("deckInUse", "miaowu");
//         superScrollView.clear();
//         var fileInfos = new DirectoryInfo("deck").GetFiles();
//         if (Config.Get(sort, "1") == "1")
//             Array.Sort(fileInfos, UIHelper.CompareTime);
//         else
//             Array.Sort(fileInfos, UIHelper.CompareName);
//         for (var i = 0; i < fileInfos.Length; i++)
//             if (fileInfos[i].Name.Length > 4)
//                 if (fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 4, 4) == ".ydk")
//                     if (fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4) == deckInUse)
//                         superScrollView.add(fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4));
//         for (var i = 0; i < fileInfos.Length; i++)
//             if (fileInfos[i].Name.Length > 4)
//                 if (fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 4, 4) == ".ydk")
//                     if (fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4) != deckInUse)
//                         superScrollView.add(fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4));
//         list_aideck.Clear();
//         fileInfos = new DirectoryInfo("ai/ydk").GetFiles();
//         Array.Sort(fileInfos, UIHelper.CompareName);
//         list_aideck.AddItem(suiji);
//         for (var i = 0; i < fileInfos.Length; i++)
//             if (fileInfos[i].Name.Length > 4)
//                 if (fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 4, 4) == ".ydk")
//                     list_aideck.AddItem(fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4));
//         list_airank.Clear();
//         fileInfos = new DirectoryInfo("ai").GetFiles();
//         Array.Sort(fileInfos, UIHelper.CompareName);
//         for (var i = 0; i < fileInfos.Length; i++)
//             if (fileInfos[i].Name.Length > 4)
//                 if (fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 4, 4) == ".lua")
//                     list_airank.AddItem(fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4));
//     }
//
//     public override void show()
//     {
//         base.show();
//         printFile();
//         superScrollView.selectedString = Config.Get("deckInUse", "miaowu");
//         superScrollView.toTop();
//         Program.charge();
//     }
//
//     #endregion
// }