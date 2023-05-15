using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ABLoader : MonoBehaviour
{
    /// <summary>
    /// 读取并实例化单个Assetbundle并返回，默认返回第一个可以Instantiate的GameObject。
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject LoadAB(string path)
    {
        var ab = AssetBundle.LoadFromFile("assetbundle/" + path);
        var prefabs = ab.LoadAllAssets();
        foreach(Object prefab in prefabs)
        {
            if (typeof(GameObject).IsInstanceOfType(prefab))
            {
                ab.Unload(false);
                var go = Instantiate(prefab) as GameObject;
                return go;
            }
        }
        return null;
    }
    /// <summary>
    /// 读取并实例化单个文件夹中的所有Assetbundle，全部并为“abname”的新GameObject的子集后返回“abname”。
    /// </summary>
    /// <param name="path"></param>
    /// <param name="abname"></param>
    /// <returns></returns>
    public static GameObject LoadABFolder(string path, string abname = "NewABs")
    {
        List<AssetBundle> bundles = new List<AssetBundle>();
        GameObject gos = new GameObject(abname);
        DirectoryInfo direction = new DirectoryInfo("assetbundle/" + path);
        FileInfo[] files = direction.GetFiles("*");
        for (int i = 0; i < files.Length; i++)
        {
            bundles.Add(AssetBundle.LoadFromFile(files[i].FullName));
        }
        foreach (AssetBundle bundle in bundles)
        {
            var prefabs = bundle.LoadAllAssets();
            for (int j = 0; j < prefabs.Length; j++)
            {
                if (typeof(GameObject).IsInstanceOfType(prefabs[j]))
                {
                    var go = Instantiate(prefabs[j]) as GameObject;
                    go.transform.SetParent(gos.transform);
                }
            }
        }
        foreach (AssetBundle bundle in bundles)
        {
            bundle.Unload(false);
        }
        return gos;
    }

    public static void ChangeLayer(GameObject go, string layer, bool setAllChildrenActivate = false)
    {
        foreach (Transform t in go.transform.GetComponentsInChildren<Transform>(true))
        {
            if (setAllChildrenActivate) t.gameObject.SetActive(true);
            t.gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
}
