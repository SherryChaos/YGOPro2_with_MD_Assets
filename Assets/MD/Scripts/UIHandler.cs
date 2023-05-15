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
        LoadBgFront(Config.Get("wallpaper_", "�����ǰ���"));
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
            case "�����ǰ���":
                return "0001";
            case "��Ӱ�����÷���":
                return "0002";
            case "����ս�� ����֮ʩ����":
                return "0003";
            case "����֮���� ��������":
                return "0004";
            case "������-����":
                return "0005";
            case "�ƽ��� �ƽ������":
                return "0006";
            case "˫�֮��ʿ ��˹����ķ":
                return "0007";
            case "���ǻ���Ⱥ":
                return "0008";
            case "�ػ���� �����":
                return "0009";
            case "Ԫ��Ӣ�� ���������":
                return "0010";
            case "����������":
                return "0011";
            case "δ��No.0 δ������ ����":
                return "0012";
            case "��ɫ�۰Ի������":
                return "0013";
            case "����������":
                return "0014";
            case "��ƽ�":
                return "0015";
            case "������������ǰ·":
                return "0016";
            case "��ħ��ʦ":
                return "0017";
            case "���뷢ѿ�Ĵ�����":
                return "0018";
            case "�������ܷ���":
                return "0019";
            case "No.41 ��˯ħ�� ˯����":
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
