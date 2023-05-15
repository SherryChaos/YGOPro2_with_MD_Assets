using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 1;
    public Transform from;
    public Transform to;
    public RendMega mega;

    public MegaShapeArc arc;

    // Use this for initialization
    private void Start()
    {
        updateSpeed();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void updateSpeed()
    {
        var list = GetComponentsInChildren<AnimUnit>();
        foreach (var item in list) item.m_fspeed = speed;
    }
}