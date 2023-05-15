using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBehaviour : MonoBehaviour
{
    public float alpha;
    Material material;
    float time;
    void Start()
    {
        time = 0;
        alpha = 0f;
        material = GetComponent<MeshRenderer>().material;
    }
    void Update()
    {
        material.SetFloat("Alpha", alpha);
        if(alpha > 0.25f)
        {
            time += Time.deltaTime;
        }
        if(time > 3f)
        {
            FadeOut(0.2f);
        }
        if(alpha == 0f)
        {
            time = 0;
        }
    }
    public void FadeIn(float time)
    {
        DOTween.To(() => alpha, x => alpha = x, 0.5f, time);
    }
    public void FadeOut(float time)
    {
        DOTween.To(() => alpha, x => alpha = x, 0f, time);
    }
}
