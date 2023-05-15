using System;
using UnityEngine;

public class UImouseHint : MonoBehaviour
{
    public GameObject point;
    public Camera cam;


    private bool drag;

    // Use this for initialization
    private void Start()
    {
        try
        {
            cam = gameObject.transform.GetComponentInParent<UIRoot>().GetComponentInChildren<Camera>();
        }
        catch (Exception)
        {
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (drag)
            point.transform.position = cam.ScreenToWorldPoint(new Vector3(
                Input.mousePosition.x + 5,
                Input.mousePosition.y - 25, 0
            ));
    }

    public void begin()
    {
        drag = true;
        point.SetActive(true);
    }

    public void end()
    {
        drag = false;
        point.SetActive(false);
    }
}