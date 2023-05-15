using UnityEngine;

public class spining : MonoBehaviour
{
    private void Update()
    {
        transform.localEulerAngles += Vector3.forward * Program.deltaTime * 160;
    }
}