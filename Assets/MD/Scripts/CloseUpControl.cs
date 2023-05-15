using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public class CloseUpControl : MonoBehaviour
{
    public float cycle = 3.1415926f;
    Material material;
    float time;
    public float alpha = 1.0f;
    void Start()
    {
        time = 0;
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        GetComponent<Animation>().Play("quad_cutin");
        material.SetFloat("GlowControl", ((float)Math.Sin(time * cycle/2 - 1f) + 1f) / 2f);
        material.SetFloat("Alpha", alpha);
    }
    public void FadeIn(float time )
    {
        DOTween.To(()=> alpha, x => alpha = x, 1f , time);
    }
    public void FadeOut(float time)
    {
        DOTween.To(() => alpha, x => alpha = x, 0f, time);
    }
}
