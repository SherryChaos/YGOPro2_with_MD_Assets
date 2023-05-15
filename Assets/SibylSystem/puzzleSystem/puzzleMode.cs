using System;
using System.Collections.Generic;
using System.IO;

public class puzzleMode : WindowServantSP
{
    private PrecyOcg precy;


    private string selectedString = "miaomiaomiao";

    private UIselectableList superScrollView;

    public override void initialize()
    {
        SetWindow(Program.I().remaster_puzzleManager);
        UIHelper.registEvent(gameObject, "exit_", onClickExit);
        superScrollView = gameObject.GetComponentInChildren<UIselectableList>();
        superScrollView.selectedAction = onSelected;
        superScrollView.install();
        SetActiveFalse();
    }

    private void onSelected()
    {
        if (!isShowed) return;
        if (selectedString == superScrollView.selectedString) KF_puzzle(superScrollView.selectedString);
        selectedString = superScrollView.selectedString;
    }

    public void KF_puzzle(string name)
    {
        launch("puzzle/" + name + ".lua");
    }

    public override void show()
    {
        base.show();
        printFile();
    }

    private void printFile()
    {
        superScrollView.clear();
        var args = new List<string[]>();
        var fileInfos = new DirectoryInfo("puzzle").GetFiles();
        Array.Sort(fileInfos, UIHelper.CompareName);
        for (var i = 0; i < fileInfos.Length; i++)
            if (fileInfos[i].Name.Length > 4)
                if (fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 4, 4) == ".lua")
                    superScrollView.add(fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4));
    }

    private void onClickExit()
    {
        if (Program.exitOnReturn)
            Program.I().menu.onClickExit();
        else
            Program.I().shiftToServant(Program.I().menu);
    }

    public void launch(string path)
    {
        if (precy != null) precy.dispose();
        precy = new PrecyOcg();
        precy.startPuzzle(path);
    }
}