using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardHighlight : MonoBehaviour
{
    Transform chl;
    void Start()
    {
        chl = Instantiate(LoadAssets.cardHighlight.gameObject, this.transform).transform;
        //chl.SetActive(true);
        chl.localPosition = new Vector3(0, -0.01f, 0);
    }


    void Update()
    {
        if (this.transform.position.y < chl.transform.position.y)
        {
            chl.localPosition *= -1f;
            chl.localEulerAngles = new Vector3(chl.localEulerAngles.x +180, chl.localEulerAngles.y, chl.localEulerAngles.z);
        }
    }
}
