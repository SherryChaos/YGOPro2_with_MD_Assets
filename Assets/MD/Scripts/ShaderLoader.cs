using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShaderLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var renderers = this.gameObject.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            foreach (var material in renderers[i].sharedMaterials)
            {
                string shaderName = material.shader.name;
                if (shaderName != null)
                {
                    AssetBundle ab = AssetBundle.LoadFromFile("assetbundle/shader/" + shaderName.Replace("/", "#"));
                    if (ab != null)
                    {
                        Shader shader = ab.LoadAsset<Shader>("shader");
                        material.shader = shader;
                        ab.Unload(false);
                    }
                    else
                    {
                        Debug.LogErrorFormat("Shader: {0} is not exist!", shaderName);
                    }
                }
                else
                {
                    renderers[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
