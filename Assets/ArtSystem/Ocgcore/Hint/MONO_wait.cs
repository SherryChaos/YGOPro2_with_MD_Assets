using UnityEngine;

public class MONO_wait : MonoBehaviour
{
    public UILabel lab;
    private float a;

    private string s = "";

    // Use this for initialization
    private void Start()
    {
        s = InterString.Get("等待对方行动中");
    }

    // Update is called once per frame
    private void Update()
    {
        a += Time.deltaTime;
        var t = "";
        for (var i = 0; i < (int) (a * 60) / 20 % 4; i++) t += InterString.Get("…");
        lab.text = t + s + t;
    }
}