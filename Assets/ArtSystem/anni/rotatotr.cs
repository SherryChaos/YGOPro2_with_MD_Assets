using UnityEngine;

public class rotatotr : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 80);
    }
}