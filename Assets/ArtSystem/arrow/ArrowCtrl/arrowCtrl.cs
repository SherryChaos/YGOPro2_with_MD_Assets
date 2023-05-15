using UnityEngine;

public class arrowCtrl : MonoBehaviour
{
    public GameObject[] m_karrow;

    // Use this for initialization
    private void Start()
    {
    }


    public void AllAlphaZero()
    {
        for (var i = 0; i < m_karrow.Length; i++)
            if (m_karrow[i].GetComponent<AnimUnit>() != null)
                m_karrow[i].GetComponent<AnimUnit>().SetAllAlphaZero();
    }
}