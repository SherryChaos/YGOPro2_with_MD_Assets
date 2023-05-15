using System;
using UnityEngine;

public class MonoDelegate : MonoBehaviour
{
    public Action actionInMono;

    public void function()
    {
        if (actionInMono != null) actionInMono();
    }
}


public class MonoListener : MonoBehaviour
{
    public Action<GameObject> actionInMono;

    public void function()
    {
        if (actionInMono != null) actionInMono(gameObject);
    }
}

public class MonoListenerRMS_ized : MonoBehaviour
{
    public Action<GameObject, Servant.messageSystemValue> actionInMono;
    public Servant.messageSystemValue value;

    public void function()
    {
        var input = GetComponent<UIInput>();
        if (input != null)
        {
            value = new Servant.messageSystemValue();
            value.hint = input.name;
            value.value = input.value;
        }

        if (actionInMono != null) actionInMono(gameObject, value);
    }
}