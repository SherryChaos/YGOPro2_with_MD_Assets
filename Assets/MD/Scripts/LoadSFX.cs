using System.Collections;
using UnityEngine;
using YGOSharp.OCGWrapper.Enums;

public class LoadSFX : MonoBehaviour
{
    public void DelayDecoration(Vector3 pos, int position, string sfx = "无", string sound = "无", float t = 0, bool singleFile = false)
    {
        StartCoroutine(Fx(pos, position ,sfx ,sound, t, singleFile));
    }
    IEnumerator Fx(Vector3 pos, int position, string sfx = "无", string sound = "无", float t =0, bool singleFile = false)
    {
        if(!Ocgcore.inSkiping) yield return new WaitForSeconds(t);
        GameObject decoration;
        if (sfx != "无" && !singleFile)
        {
            decoration = ABLoader.LoadABFolder(sfx, "Fx");
            decoration.transform.position = new Vector3(pos.x, pos.y - 0.1f, pos.z);
            if(position == (int)CardPosition.FaceUpDefence)
                decoration.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
            decoration.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Destroy(decoration, 10f);
        }
        else if (sfx != "无" && singleFile)
        {
            decoration = ABLoader.LoadAB(sfx);
            decoration.transform.position = new Vector3(pos.x, pos.y - 0.1f, pos.z);
            decoration.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            Destroy(decoration, 10f);
        }
        if (sound != "无") UIHelper.playSound(sound, 0.7f);
    }
    public static GameObject Decoration(string path, bool singleFile, Transform parent)
    {
        GameObject decoration;
        if (singleFile)
        {
            decoration = ABLoader.LoadAB(path);
            decoration.transform.parent = parent;
        }
        else
        {
            decoration = ABLoader.LoadABFolder(path);
            decoration.transform.parent = parent;
        }
        return decoration;
    }
}