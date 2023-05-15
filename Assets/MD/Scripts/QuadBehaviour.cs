using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadBehaviour : MonoBehaviour
{
    Transform parent;
    public float k_verticle = 1f;
    Transform child;
    bool needScale = true;
    void Start()
    {
        if (parent == null)
            parent = transform.parent;
        child = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(parent != null)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -parent.localEulerAngles.y, transform.localEulerAngles.z);
        if (needScale && child.GetComponent<Renderer>().material.mainTexture != null)
        {
            Texture texture = child.GetComponent<Renderer>().material.mainTexture;
            k_verticle = texture.width / (float)texture.height;
            if (k_verticle > 0f && k_verticle < 1f)
            {
                child.localScale = new Vector3(k_verticle, 1f, 1f);
                needScale = false;
            }
            if (k_verticle > 1f)
            {
                child.localScale = new Vector3(1, 1f / k_verticle, 1f);
                needScale = false;
            }
        }
    }
}
