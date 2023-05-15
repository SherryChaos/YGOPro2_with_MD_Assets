using UnityEngine;

public class blinding : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        gameObject.GetComponent<Renderer>().material.SetFloat("_Alpha", (1f + Mathf.Sin(Time.time * 10)) / 2f);
    }
}