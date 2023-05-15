using UnityEngine;

public class pngSord : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        gameObject.GetComponent<Renderer>().material.mainTexture = GameTextureManager.attack;
        gameObject.transform.localScale =
            new Vector3(1, 1f / GameTextureManager.attack.width * GameTextureManager.attack.height, 1)*0.5f;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}