using System.Collections;
using UnityEngine;
using YGOSharp.OCGWrapper.Enums;

public class CardAnimation : MonoBehaviour
{
    bool moving = true;
    Vector3 currrentPosition;
    Vector3 currentRotation;
    public int activateType = 0;
    public bool negated = false;
    public bool activated = false;
    public bool waitForLanding = false;
    public bool bigLanding = false;
    BlackBehaviour bb;
    Material cardfaceMaterial;
    public int position;
    public bool isCutin = false;
    void Start()
    {
        bb = GameObject.Find("Main Camera/Black").GetComponent<BlackBehaviour>();
        cardfaceMaterial = transform.Find("card/face").GetComponent<Renderer>().material;
        currrentPosition = transform.position;
        currentRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated || waitForLanding) WaitForMoving();
        if (!moving && activated)
        {
            activated = false;
            moving = true;
            var activateEff = ABLoader.LoadAB("effects/fxp/fxp_bff_active_001");
            foreach (var renderer in activateEff.transform.GetComponentsInChildren<Renderer>())
            {
                renderer.sharedMaterial.renderQueue = 4004;
            }
            activateEff.transform.parent = transform.Find("card");
            activateEff.transform.localPosition = new Vector3(0f, 0f, -0.2f);
            activateEff.transform.localEulerAngles = new Vector3(90, 0, 0);
            activateEff.SetActive(false);
            StartCoroutine(ActivateEffDelay(activateEff, 0.167f));
            if (activateType == 0)
            {
                transform.Find("card").GetComponent<Animation>().Play("card_bff_active");
            }
            else
            {
                activateType = 0;
                transform.Find("card").GetComponent<Animation>().Play("card_bff_active_hand");
            }
            Destroy(activateEff, 1f);

            cardfaceMaterial.renderQueue = 4004;
            bb.FadeIn(0.167f);
            if (transform.Find("mod_simple_quad(Clone)") != null)
            {
                CloseUpControl cc = transform.Find("mod_simple_quad(Clone)").GetChild(0).GetComponent<CloseUpControl>();
                cc.FadeOut(0.167f);
;            }
            StartCoroutine(DelayActivateQuad(0.75f + 0.167f));
        }

        if (waitForLanding && !Ocgcore.inSkiping)
        {
            waitForLanding = false;
            if(bigLanding && isCutin)
            {
                if (position == (int)CardPosition.FaceUpAttack) transform.Find("card").GetComponent<Animation>().Play("card_land_atk_cutin");
                else transform.Find("card").GetComponent<Animation>().Play("card_land_def_cutin");
                if (transform.Find("mod_simple_quad(Clone)") != null)
                {
                    CloseUpControl cc = transform.Find("mod_simple_quad(Clone)").GetChild(0).GetComponent<CloseUpControl>();
                    cc.FadeOut(0f);
                }
                StartCoroutine(DelayActivateQuad(2f + 0.167f));
                bigLanding = false;
                isCutin = false;
            }
            else if(bigLanding)
            {
                if (position == (int)CardPosition.FaceUpAttack) transform.Find("card").GetComponent<Animation>().Play("card_land_atk");
                else transform.Find("card").GetComponent<Animation>().Play("card_land_def");
                if (transform.Find("mod_simple_quad(Clone)") != null)
                {
                    CloseUpControl cc = transform.Find("mod_simple_quad(Clone)").GetChild(0).GetComponent<CloseUpControl>();
                    cc.FadeOut(0f);
                }
                StartCoroutine(DelayActivateQuad(1f + 0.167f));
                bigLanding = false;
            }
        }

        if (negated)
        {
            negated = false;
            var negateEff = ABLoader.LoadAB("effects/fxp/fxp_bff_disable_001");
            negateEff.transform.parent = transform.Find("card");
            negateEff.transform.localPosition = new Vector3(0f, 0f, -0.2f);
            negateEff.transform.localEulerAngles = new Vector3(90, 0, 0);
            negateEff.SetActive(false);
            StartCoroutine(NegateEffDelay(negateEff, 0.167f));
            transform.Find("card").GetComponent<Animation>().Play("card_bff_disable");
            Destroy(negateEff, 1f);
            if (transform.Find("mod_simple_quad(Clone)") != null)
            {
                CloseUpControl cc = transform.Find("mod_simple_quad(Clone)").GetChild(0).GetComponent<CloseUpControl>();
                cc.FadeOut(0.167f);
            }
            StartCoroutine(DelayActivateQuad(0.5f + 0.167f));
        }        
    }
    IEnumerator ActivateEffDelay(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        go.SetActive(true);
        UIHelper.playSound("SE_DUEL/SE_CARDVIEW_01", 0.7f);
    }
    IEnumerator NegateEffDelay(GameObject go, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        go.SetActive(true);
        UIHelper.playSound("SE_DUEL/SE_EFFECT_INVALID", 0.7f);        
    }
    IEnumerator DelayActivateQuad(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        cardfaceMaterial.renderQueue = 3000;
        bb.FadeOut(0.167f);
        if (transform.Find("mod_simple_quad(Clone)") != null)
        {
            CloseUpControl cc = transform.Find("mod_simple_quad(Clone)").GetChild(0).GetComponent<CloseUpControl>();
            cc.FadeIn(0.25f);            
        }
    }
    void WaitForMoving()
    {
        if (Vector3.Distance(transform.position, currrentPosition) < 0.001f && Vector3.Angle(transform.eulerAngles, currentRotation) < 0.001f)
            moving = false;
        else
        {
            currrentPosition = transform.position;
            currentRotation = transform.eulerAngles;
        }
    }
}
