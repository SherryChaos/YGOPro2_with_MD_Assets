using System.Collections.Generic;
using UnityEngine;

public class OCGobject
{
    public List<GameObject> allObjects = new List<GameObject>();
    public GameObject gameObject;

    public GameObject create(
        GameObject mod,
        Vector3 position = default,
        Vector3 rotation = default,
        bool fade = false,
        GameObject father = null,
        bool allParamsInWorld = true,
        Vector3 wantScale = default
    )
    {
        var g = Program.I().ocgcore.create_s(mod, position, rotation, fade, father, allParamsInWorld, wantScale);
        allObjects.Add(g);
        return g;
    }

    public void destroy(GameObject obj, float time = 0, bool fade = false, bool instantNull = false)
    {
        allObjects.Remove(obj);
        Program.I().ocgcore.destroy(obj, time, fade, instantNull);
    }
}