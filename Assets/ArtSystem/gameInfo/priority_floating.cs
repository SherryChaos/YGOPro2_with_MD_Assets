using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class priority_floating : MonoBehaviour
{
    private int open;

    // Use this for initialization
    private void Start()
    {
        open = Program.TimePassed() - Random.Range(0, 500);
    }

    // Update is called once per frame
    private void Update()
    {
        gameObject.transform.localPosition =
            new Vector3(0, 10 * (float) Math.Sin(3.1415936f * (Program.TimePassed() - open) / 500f), 0);
    }
}