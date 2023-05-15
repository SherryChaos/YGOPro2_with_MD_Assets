using UnityEngine;

public class EventSEPlayer : MonoBehaviour
{
    void PlayAnimationEventSe(string se)
    {
        string seFull = "SE_MATE/" + se;
        UIHelper.playSound(seFull, 1f);
    }
}
