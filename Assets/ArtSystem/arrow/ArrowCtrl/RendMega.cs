using UnityEngine;

public class RendMega : MonoBehaviour
{
    public Vector3 StartPosition = Vector3.zero;
    public Vector3 FingerPosition = Vector3.zero;
    public Transform SkillPosition;
    public Transform SkillPositionForm;
    public GameObject m_kPath;

    public float k = 3f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        MyRenderPath();
    }

    public void MyRenderPath()
    {
        m_kPath.transform.localScale = new Vector3(k, k, k);
        var v3FromPos = SkillPosition.position / k;
        var v3TargetPos = SkillPositionForm.position / k;
        var v3MidPos = (v3FromPos + v3TargetPos) / 2;

        v3MidPos += new Vector3(0, Vector3.Distance(v3TargetPos, v3FromPos) / 10f * 3f, 0);
        v3MidPos += (v3TargetPos - v3FromPos) / 10f * 3f;

        var V3Midvect = (v3TargetPos - v3FromPos) / 4;


        var v3FromAnchorIn = v3FromPos + new Vector3(0, -(v3MidPos.y / 3), 0);
        //Vector3 v3FromAnchorOut  = 	v3FromPos+new Vector3(0,(v3MidPos.y/3),0)  ;
        var v3FromAnchorOut = v3FromPos + V3Midvect;

        //Vector3 v3TarGetAnchorIn  = v3TargetPos +new Vector3(0,(v3MidPos.y/3),0) ;
        var v3TarGetAnchorIn = v3TargetPos + new Vector3(0, v3MidPos.y / 3, 0) - V3Midvect / 10;
        var v3TarGetAnchorOut = v3TargetPos + new Vector3(0, -(v3MidPos.y / 3), 0);

        var v3MidAnchorIn = v3MidPos - V3Midvect;
        var v3MidAnchorOut = v3MidPos + V3Midvect;


        m_kPath.GetComponent<MegaShapeArc>().SetKnotEx(0, 0, v3FromPos, v3FromAnchorIn, v3FromAnchorOut);
        m_kPath.GetComponent<MegaShapeArc>().SetKnotEx(0, 1, v3MidPos, v3MidAnchorIn, v3MidAnchorOut);
        m_kPath.GetComponent<MegaShapeArc>().SetKnotEx(0, 2, v3TargetPos, v3TarGetAnchorIn, v3TarGetAnchorOut);
    }
}