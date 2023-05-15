using System.IO;
using UnityEngine;

public class LoadBG : MonoBehaviour
{
    void Start()
    {
        string path = "D:/Unity/YGOProUnity_V2-master/assetbundle/duelentry/";
        if (Directory.Exists(path))
        {
            DirectoryInfo direction = new DirectoryInfo(path);
            FileInfo[] files = direction.GetFiles("*");
            for (int i = 0; i < files.Length; i++)
            {
                var ab = AssetBundle.LoadFromFile(files[i].FullName);
                var prefabs = ab.LoadAllAssets();
                for (int j = 0; j < prefabs.Length; j++)
                {
                    if (typeof(GameObject).IsInstanceOfType(prefabs[j])) Instantiate(prefabs[j]);
                }
            }
        }
        var bg = GameObject.Find("DuelEntry(Clone)");
        bg.transform.parent = GameObject.Find("Main Camera").transform;
        bg.transform.localPosition = new Vector3(0, 0, 200);
        bg.transform.localScale = new Vector3(18.4f, 18.4f, 18.4f);
        bg.transform.localRotation = Quaternion.identity;
        bg.transform.Find("ROOT_Soul").gameObject.SetActive(false);
    }
}
