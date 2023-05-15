using UnityEngine;

public class mouseParticle : MonoBehaviour
{
    public Camera camera;
    public ParticleSystem e1;
    public ParticleSystem e2;
    public Transform trans;

    private float time = 0;

    // Use this for initialization
    private void Start()
    {
        camera.depth = 99999;
    }

    // Update is called once per frame
    private void Update()
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10;
        trans.position = camera.ScreenToWorldPoint(screenPoint);

        if (Input.GetMouseButtonDown(0))
        {
            e1.Play();
            e2.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            e1.Stop();
            e2.Stop();
        }
    }
}