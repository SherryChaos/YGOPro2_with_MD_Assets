using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public string bgName;
    static GameObject homeUI;
    void Start()
    {
        LoadBgFront(Config.Get("wallpaper_", "青眼亚白龙"));
        homeUI = GameObject.Find("UI/HomeUI");
    }

    public static void LoadBgFront(string name)
    {
        name = BgMaping(name);
        Transform transform = GameObject.Find("UI/HomeUI/RootWallpaper/Wallpaper").transform;
        foreach(Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if (t.name.StartsWith("Front"))
            {
                Destroy(t.gameObject);
            }
        }
        GameObject frontLoader = ABLoader.LoadABFolder("wallpaper/front" + name, "front");
        RectTransform front = frontLoader.transform.GetChild(0).GetComponent<RectTransform>();
        front.parent = GameObject.Find("UI/HomeUI/RootWallpaper/Wallpaper").transform;
        Destroy(frontLoader);
        //front.localPosition = new Vector3(0, 0, 0.1f);
        front.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0f, front.rect.width);

        foreach (ParticleSystem p in front.GetComponentsInChildren<ParticleSystem>(true))
            p.Play();
    }
    static string BgMaping(string name)
    {
        switch (name)
        {
            case "青眼亚白龙":
                return "0001";
            case "神影依·拿非利":
                return "0002";
            case "铁兽战线 凶鸟之施莱格":
                return "0003";
            case "宵星之机神 丁吉尔苏":
                return "0004";
            case "闪刀姬-燎里":
                return "0005";
            case "黄金卿 黄金国巫妖":
                return "0006";
            case "双穹之骑士 阿斯特拉姆":
                return "0007";
            case "流星辉巧群":
                return "0008";
            case "守护神官 马哈德":
                return "0009";
            case "元素英雄 真诚新宇侠":
                return "0010";
            case "流天类星龙":
                return "0011";
            case "未来No.0 未来龙皇 霍普":
                return "0012";
            case "异色眼霸弧灵摆龙":
                return "0013";
            case "访问码语者":
                return "0014";
            case "鬼计节":
                return "0015";
            case "星遗物引导的前路":
                return "0016";
            case "黑魔术师":
                return "0017";
            case "丘与发芽的春化精":
                return "0018";
            case "北极天熊辐射":
                return "0019";
            case "No.41 泥睡魔兽 睡梦貘":
                return "0041";
            default:
                return "0001";
        }
    }
    public static void CloseHomeUI()
    {
        homeUI.SetActive(false);
    }
    public static void OpenHomeUI()
    {
        homeUI.SetActive(true);
        foreach (ParticleSystem p in homeUI.GetComponentsInChildren<ParticleSystem>(true))
            p.Play();
    }
}
