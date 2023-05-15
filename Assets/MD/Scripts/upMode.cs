using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upMode : MonoBehaviour
{
    public static bool upmode;
    public static bool lockCamera;
    bool hasChanged;
    Transform ui_back_ground_2d;
    Transform ui_main_2d;
    Transform new_toolBar_watchRecord;
    Transform new_toolBar_watchDuel;
    void Start()
    {
        upmode = false;
        hasChanged = false;
        ui_back_ground_2d = GameObject.Find("ui_back_ground_2d").transform;
        ui_main_2d = GameObject.Find("ui_main_2d").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (upmode) upmode = false;
            else upmode = true;
            hasChanged = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (lockCamera) lockCamera = false;
            else lockCamera = true;
        }

        if (hasChanged)
        {
            if (upmode)
            {
                ui_back_ground_2d.gameObject.SetActive(false);
                if(new_toolBar_watchRecord != null) new_toolBar_watchRecord.gameObject.SetActive(false);
                else
                {
                    new_toolBar_watchRecord = ui_main_2d.Find("new_toolBar_watchRecord(Clone)");
                    if(new_toolBar_watchRecord != null) new_toolBar_watchRecord.gameObject.SetActive(false);
                }
                if (new_toolBar_watchDuel != null) new_toolBar_watchDuel.gameObject.SetActive(false);
                else
                {
                    new_toolBar_watchDuel = ui_main_2d.Find("new_toolBar_watchDuel(Clone)");
                    if (new_toolBar_watchDuel != null) new_toolBar_watchDuel.gameObject.SetActive(false);
                }
            }
            else
            {
                ui_back_ground_2d.gameObject.SetActive(true);
                if (new_toolBar_watchRecord != null) new_toolBar_watchRecord.gameObject.SetActive(true);
                else
                {
                    new_toolBar_watchRecord = ui_main_2d.Find("new_toolBar_watchRecord(Clone)");
                    if (new_toolBar_watchRecord != null) new_toolBar_watchRecord.gameObject.SetActive(true);
                }
                if (new_toolBar_watchDuel != null) new_toolBar_watchDuel.gameObject.SetActive(true);
                else
                {
                    new_toolBar_watchDuel = ui_main_2d.Find("new_toolBar_watchDuel(Clone)");
                    if (new_toolBar_watchDuel != null) new_toolBar_watchDuel.gameObject.SetActive(true);
                }
            }
            hasChanged = false;
        }
    }
}
