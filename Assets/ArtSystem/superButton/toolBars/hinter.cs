using UnityEngine;

public class hinter : MonoBehaviour
{
    public string str;
    private bool loaded;

    private GameObject obj;

    private UIEventTrigger trigger;

    // Use this for initialization
    private void Start()
    {
        trigger = GetComponent<UIEventTrigger>();
        if (trigger == null) trigger = gameObject.AddComponent<UIEventTrigger>();
        trigger.onHoverOver.Add(new EventDelegate(this, "in_"));
        trigger.onHoverOut.Add(new EventDelegate(this, "out_"));
        trigger.onPress.Add(new EventDelegate(this, "out_"));
    }

    // Update is called once per frame
    private void Update()
    {
        if (loaded == false)
            if (InterString.loaded)
            {
                loaded = true;
                str = InterString.Get(str);
            }
    }

    private void in_()
    {
        var screenPosition = Program.I().camera_main_2d
            .WorldToScreenPoint(gameObject.GetComponentInChildren<UITexture>().gameObject.transform.position);
        screenPosition.y += 90;
        screenPosition.z = 0;
        var worldPositin = Program.I().camera_main_2d.ScreenToWorldPoint(screenPosition);
        obj = Program.I().create(Program.I().mod_simple_ngui_text, worldPositin, Vector3.zero, true,
            Program.I().ui_main_2d);
        obj.GetComponent<UILabel>().text = str;
        obj.GetComponent<UILabel>().effectStyle = UILabel.Effect.Outline;
        Program.I().destroy(obj, 5f);
    }

    private void out_()
    {
        if (obj != null) Program.I().destroy(obj, 0.6f, true, true);
    }
}