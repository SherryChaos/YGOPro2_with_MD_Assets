using UnityEngine;

public class ContinuesButton : MonoBehaviour
{
    private UIButton btn;

    private bool isTrigging;

    // Update is called once per frame
    private float time;

    // Use this for initialization
    private void Start()
    {
        btn = GetComponentInChildren<UIButton>();
        var trigger = gameObject.GetComponentInChildren<UIEventTrigger>();
        if (trigger == null) trigger = gameObject.AddComponent<UIEventTrigger>();
        trigger.onRelease.Add(new EventDelegate(this, "off"));
        trigger.onPress.Add(new EventDelegate(this, "on"));
    }

    private void Update()
    {
        if (isTrigging)
            if (btn != null)
            {
                time += Time.deltaTime;
                if (time > 0.2f)
                {
                    time = 0;
                    EventDelegate.Execute(btn.onClick);
                }
            }
    }

    private void on()
    {
        isTrigging = true;
        time = 0;
    }

    private void off()
    {
        isTrigging = false;
        time = 0;
    }
}