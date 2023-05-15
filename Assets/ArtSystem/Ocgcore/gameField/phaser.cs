using System;
using UnityEngine;

public class phaser : MonoBehaviour
{
    public BoxCollider colliderMp2;
    public BoxCollider colliderBp;
    public BoxCollider colliderEp;

    public UILabel labDp;
    public UILabel labSp;
    public UILabel labMp1;
    public UILabel labBp;
    public UILabel labMp2;
    public UILabel labEp;
    public Action bpAction;
    public Action epAction;

    public Action mp2Action;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Program.InputGetMouseButtonUp_0)
        {
            if (Program.pointedCollider == colliderMp2)
                if (mp2Action != null)
                    mp2Action();
            if (Program.pointedCollider == colliderBp)
                if (bpAction != null)
                    bpAction();
            if (Program.pointedCollider == colliderEp)
                if (epAction != null)
                    epAction();
        }
    }
}