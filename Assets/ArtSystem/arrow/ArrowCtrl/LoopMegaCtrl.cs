using UnityEngine;

public class LoopMegaCtrl : MonoBehaviour
{
    public GameObject Path;
    public int m_iID;
    public GameObject m_kFirstOb;
    public int m_iCount;
    private bool m_bneedtoFollow = true;
    private float m_fBeforeLenth;
    private float m_ObjectLenth;
    private float m_offsetpercent;

    // Use this for initialization
    private float m_offsetZ;
    private float m_pathLenth;

    private void Start()
    {
        //物体长度  	--CardGame
        m_ObjectLenth = 0.9f * 0.02f;
        //this.GetComponent<MegaWorldPathDeform>().animate = false;
        //this.GetComponent<MegaWorldPathDeform>().percent = 0;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void LateUpdate()
    {
        LoopMegaAntmation();
    }

    private void LoopMegaAntmation()
    {
        //if(m_offsetZ>0)
        //{
        //获得线段长度		--CardGame
        if (m_bneedtoFollow && m_kFirstOb != null)
        {
            GetComponent<MegaWorldPathDeform>().percent = m_kFirstOb.GetComponent<MegaWorldPathDeform>().percent;
            m_bneedtoFollow = false;
        }

//		if(m_iCount>=5)
//		{
//			m_bneedtoFollow = false;
//			m_iCount = 0;
//		}


        m_pathLenth = Path.GetComponent<MegaShapeArc>().GetCurveLength(0);

//		if(m_fBeforeLenth>m_pathLenth)
//		{
//
//			//this.GetComponent<MegaWorldPathDeform>().Offset -= new Vector3(0,0,(m_fBeforeLenth - m_pathLenth)/5*m_iID);
//			this.GetComponent<MegaWorldPathDeform>().animate = false;
//
//		}
//		else if(m_fBeforeLenth<m_pathLenth)
//		{
//			//this.GetComponent<MegaWorldPathDeform>().Offset -= new Vector3(0,0,(m_fBeforeLenth - m_pathLenth)/5*m_iID);
//			this.GetComponent<MegaWorldPathDeform>().animate = false; 
//		}
//		else
//		{
//				this.GetComponent<MegaWorldPathDeform>().animate = true;
//		}
        m_fBeforeLenth = m_pathLenth;

        m_offsetZ = -GetComponent<MegaWorldPathDeform>().Offset.z;
        m_offsetpercent = (m_ObjectLenth + m_offsetZ) / m_pathLenth * 100;

        //Debug.Log(this.name+":" + m_offsetpercent);


        GetComponent<MegaWorldPathDeform>().path = Path.GetComponent<MegaShapeArc>();

        if (GetComponent<MegaWorldPathDeform>().percent >= 100 + m_offsetpercent)
        {
            //调整位置方法	--CardGame
            if (m_kFirstOb != null)
            {
                Debug.Log("else follow first");

                GetComponent<MegaWorldPathDeform>().percent = m_kFirstOb.GetComponent<MegaWorldPathDeform>().percent;
                m_bneedtoFollow = true;
            }
            else
            {
                Debug.Log("first to 0");
                GetComponent<MegaWorldPathDeform>().percent = 0;
            }
        }
    }
}