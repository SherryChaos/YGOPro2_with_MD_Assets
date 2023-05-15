using System.IO;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        //var ab = ABLoader.LoadAB("effects/fxp/fxp_atkdak_s1_001");
        //ab.transform.GetChild(0).gameObject.AddComponent<NewBehaviourScript>();
        ABLoader.LoadAB("effects/back/back0001");

        //var ab = ABLoader.LoadABFolder("effects/back/back0001");
        //ab.AddComponent<AnimationTest>();
    }
}
