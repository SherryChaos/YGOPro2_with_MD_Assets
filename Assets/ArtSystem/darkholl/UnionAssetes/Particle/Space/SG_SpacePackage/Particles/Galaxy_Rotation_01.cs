using UnityEngine;

public class Galaxy_Rotation_01 : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // Slowly rotate the object around its X axis at 1 degree/second.
        transform.Rotate(Vector3.forward * Time.deltaTime * -10);
    }
}