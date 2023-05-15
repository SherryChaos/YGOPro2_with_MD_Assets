using TMPro;
using UnityEngine;

public class chainMono : MonoBehaviour
{
    public Renderer circle;
    public TextMeshPro text;
    public bool flashing = true;
    private float all;
    private bool p = true;

    private void Update()
    {
        if (flashing)
        {
            all += Program.deltaTime;
            if (all > 0.05)
            {
                all = 0;
                p = !p;
                if (p)
                {
                    circle.gameObject.SetActive(false);
                    text.gameObject.SetActive(false);
                }
                else
                {
                    circle.gameObject.SetActive(true);
                    text.gameObject.SetActive(true);
                }
            }
        }
    }
}