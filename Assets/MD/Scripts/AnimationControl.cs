using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    int fullHP = 0;
    int currentHP1 = 0;
    int currentHP2 = 0;
    bool p1hasEntered2 = false;
    bool p1hasEntered3 = false;
    bool p1hasEntered4 = false;
    public static bool p1hasEnteredEnd = false;
    bool p2hasEntered2 = false;
    bool p2hasEntered3 = false;
    bool p2hasEntered4 = false;
    public static bool p2hasEnteredEnd = false;
    public static bool fieldEnd;


    public string animationName;
    public bool isPlaying = false;
    public static float extraDelay1;
    public static float extraDelay2;

    float time1 = 8f;
    float time2 = 6f;
    float delay = 0;
    public static float animationControl1;
    public static float animationControl2;
    public static float animationControl = 5f;
    public static float animationControl3;
    public static float animationControl4;
    public static float animationControlMate = 3f;
    // Start is called before the first frame update
    void Start()
    {
        fullHP = Program.I().ocgcore.lpLimit;
        extraDelay1 = 0;
        extraDelay2 = 0;
        p1hasEnteredEnd = false;
        p2hasEnteredEnd = false;
        fieldEnd = false;
        animationControl1 = 0;
        animationControl2 = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (fullHP == 0) fullHP = Program.I().ocgcore.lpLimit;
        currentHP1 = Program.I().ocgcore.life_0 > 0 ? Program.I().ocgcore.life_0 : 0;
        currentHP2 = Program.I().ocgcore.life_1 > 0 ? Program.I().ocgcore.life_1 : 0;
        ///////////////////////////////////////////////////////////////////////////////////////////////
        if (currentHP1 <= fullHP / 4 * 3 && !p1hasEntered2)
        {
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field1_, "DamagePhase1ToPhase2", "ToPhase2", "_PHASE1_P", delay + extraDelay1);
            StartCoroutine(enumerator);
            p1hasEntered2 = true;
            if (currentHP1 <= fullHP / 4 * 2) extraDelay1++;
        }
        if (currentHP1 <= fullHP / 4 * 2 && !p1hasEntered3)
        {
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field1_, "DamagePhase2ToPhase3", "ToPhase3", "_PHASE2_P", delay + extraDelay1);
            StartCoroutine(enumerator);
            p1hasEntered3 = true;
            if (currentHP1 <= fullHP / 4 * 1) extraDelay1++;
            BGMHandler.ChangeBGM("duel_climax");
        }
        if (currentHP1 <= fullHP / 4 * 1 && !p1hasEntered4)
        {
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field1_, "DamagePhase3ToPhase4", "ToPhase4", "_PHASE3_P", delay + extraDelay1);
            StartCoroutine(enumerator);
            p1hasEntered4 = true;
            if (currentHP1 == 0) extraDelay1++;
        }
        if (p1hasEnteredEnd && !fieldEnd)
        {
            if (!Ocgcore.inSkiping)
            {
                PlayAnimation(LoadAssets.mate1_, "Defeat");
                PlayAnimation(LoadAssets.mate2_, "Victory");
            }
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field1_, "DamagePhase4ToEnd", "ToEnd", "_PHASE4_P", delay + extraDelay1, true);
            StartCoroutine(enumerator);
            fieldEnd = true;
        }



        if (currentHP2 <= fullHP / 4 * 3 && !p2hasEntered2)
        {
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field2_, "DamagePhase1ToPhase2", "ToPhase2", "_PHASE1_R", delay + extraDelay2);
            StartCoroutine(enumerator);
            p2hasEntered2 = true;
            if (currentHP2 <= fullHP / 4 * 2) extraDelay2++;
        }
        if (currentHP2 <= fullHP / 4 * 2 && !p2hasEntered3)
        {
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field2_, "DamagePhase2ToPhase3", "ToPhase3", "_PHASE2_R", delay + extraDelay2);
            StartCoroutine(enumerator);
            p2hasEntered3 = true;
            if (currentHP2 <= fullHP / 4 * 1) extraDelay2++;
            BGMHandler.ChangeBGM("duel_climax");
        }
        if (currentHP2 <= fullHP / 4 * 1 && !p2hasEntered4)
        {
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field2_, "DamagePhase3ToPhase4", "ToPhase4", "_PHASE3_R", delay + extraDelay2);
            StartCoroutine(enumerator);
            p2hasEntered4 = true;
            if (currentHP2 == 0) extraDelay2++;
        }
        if (p2hasEnteredEnd && !fieldEnd)
        {
            if (!Ocgcore.inSkiping)
            {
                PlayAnimation(LoadAssets.mate1_, "Victory");
                PlayAnimation(LoadAssets.mate2_, "Defeat");
            }
            IEnumerator enumerator = PlayAnimationSet(LoadAssets.field2_, "DamagePhase4ToEnd", "ToEnd", "_PHASE4_R", delay + extraDelay2, true);
            StartCoroutine(enumerator);
            fieldEnd = true;
        }

        

        extraDelay1 = 0;
        extraDelay2 = 0;

        //Tap 动画
        animationControl1 -= Time.deltaTime;
        animationControl2 -= Time.deltaTime;
        animationControl3 -= Time.deltaTime;
        animationControl4 -= Time.deltaTime;
        if (animationControl1 < 0 && LoadAssets.fieldCollider1 != null && Program.InputGetMouseButtonDown_0 && Program.pointedCollider == LoadAssets.fieldCollider1)
        {
            PlayAnimation(LoadAssets.field1_, "TapAll");
            PlayParticle(LoadAssets.field1_, "Tap");
            PlayFieldSE(LoadAssets.field1_, "_TAP");
            animationControl1 = animationControl;
        }
        if (animationControl2 < 0 && LoadAssets.fieldCollider2 != null && Program.InputGetMouseButtonDown_0 && Program.pointedCollider == LoadAssets.fieldCollider2)
        {
            PlayAnimation(LoadAssets.field2_, "TapAll");
            PlayParticle(LoadAssets.field2_, "Tap");
            PlayFieldSE(LoadAssets.field2_, "_TAP");
            animationControl2 = animationControl;
        }

        if (animationControl3 < 0 && LoadAssets.mateCollider1 != null && Program.InputGetMouseButtonDown_0 && Program.pointedCollider == LoadAssets.mateCollider1)
        {
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0: PlayAnimation(LoadAssets.mate1_, "Tap", true);
                    break;
                case 1: PlayAnimation(LoadAssets.mate1_, "Tap1");
                    break;
                case 2: PlayAnimation(LoadAssets.mate1_, "Tap2");
                    break;
            }
            animationControl3 = animationControlMate;
        }
        if (animationControl4 < 0 && LoadAssets.mateCollider2 != null && Program.InputGetMouseButtonDown_0 && Program.pointedCollider == LoadAssets.mateCollider2)
        {
            int i = Random.Range(0, 3);
            switch (i)
            {
                case 0:
                    PlayAnimation(LoadAssets.mate2_, "Tap", true);
                    break;
                case 1:
                    PlayAnimation(LoadAssets.mate2_, "Tap1");
                    break;
                case 2:
                    PlayAnimation(LoadAssets.mate2_, "Tap2");
                    break;
            }
            animationControl4 = animationControlMate;
        }

        //Mate随机动画
        time1 -= Time.deltaTime;
        if(time1 < 0)
        {
            if (Random.Range(0, 2) > 0.5)
            {
                PlayAnimation(LoadAssets.mate1_, "Random1");
            }
            else PlayAnimation(LoadAssets.mate1_, "Random2");
            time1 = Random.Range(8,13);
        }
        time2 -= Time.deltaTime;
        if (time2 < 0)
        {
            if (Random.Range(0, 2) > 0.5)
            {
                PlayAnimation(LoadAssets.mate2_, "Random1");
            }
            else PlayAnimation(LoadAssets.mate2_, "Random2");
            time2 = Random.Range(8, 13);
        }
    }

    public static IEnumerator PlayAnimationSet(Transform target, string aniName, string parName = null, string SEName = null, float waitTime = 0, bool forced = false)
    {   
        yield return new WaitForSeconds(waitTime);
        AnimationControl ac = GameObject.Find("new_gameField(Clone)/Assets_Loader").GetComponent<AnimationControl>();
        PlayAnimation(target, aniName, forced);
        if (parName != null) PlayParticle(target, parName);
        if (SEName != null) PlayFieldSE(target, SEName);
    }
    public static void PlayAnimation(Transform animationContainer, string animationName, bool forced = false)
    {
        if (animationContainer == null) return;
        Animator[] allAnimator;
        allAnimator = animationContainer.GetComponentsInChildren<Animator>();
        foreach (Animator anim in allAnimator)
        {
            if (animationContainer.name.StartsWith("CD-"))
            {
                  if (animationName == "Victory" || animationName == "Tap" || animationName == "Tap1" || animationName == "Tap2")
                {
                    anim.SetTrigger("Attack");
                }
                if (animationName == "Defeat")
                {
                    anim.SetBool("isAttackPosition", false);
                }                    
                if (animationName == "Random1" || animationName == "Random2") return;
                if (animationName == "Attack") 
                {
                    anim.SetTrigger("Attack2");
                }
                if (animationName == "Damege" || animationName == "Cost")
                {
                    anim.SetTrigger("Attack3");
                }
            }
            else
            {
                if (forced)
                {
                    anim.CrossFade(animationName, 0.3f);
                    //anim.SetTrigger(animationName);
                }
                    
                else anim.SetTrigger(animationName);
            }
        }
        if (animationName == "TapAll")
        {
            if (animationContainer.name.StartsWith("Mat_001"))
            {
                animationContainer.Find("tap2").gameObject.SetActive(false);
                animationContainer.Find("tap2").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_004_near"))
            {
                animationContainer.Find("fxp_BG_light_near_02").gameObject.SetActive(false);
                animationContainer.Find("fxp_BG_light_near_02").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_004_far"))
            {
                animationContainer.Find("fxp_BG_light_far_02").gameObject.SetActive(false);
                animationContainer.Find("fxp_BG_light_far_02").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_006_near"))
            {
                animationContainer.Find("fxp_BG_holly_wave_shield").gameObject.SetActive(false);
                animationContainer.Find("fxp_BG_holly_wave_shield").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_006_far"))
            {
                animationContainer.Find("fxp_BG_holly_wave_spear").gameObject.SetActive(false);
                animationContainer.Find("fxp_BG_holly_wave_spear").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_011"))
            {
                animationContainer.Find("Tap").gameObject.SetActive(false);
                animationContainer.Find("Tap").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_012"))
            {
                animationContainer.Find("tap").gameObject.SetActive(false);
                animationContainer.Find("tap").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_016"))
            {
                animationContainer.Find("tap").gameObject.SetActive(false);
                animationContainer.Find("tap").gameObject.SetActive(true);
            }
            if (animationContainer.name.StartsWith("Mat_018"))
            {
                animationContainer.Find("Tap").gameObject.SetActive(false);
                animationContainer.Find("Tap").gameObject.SetActive(true);
            }
        }

        //Mat_016 控制
        if (animationName == "DamagePhase1ToPhase2" && animationContainer.name.StartsWith("Mat_016_near"))
        {
            animationContainer.Find("smoke_near_p1").gameObject.SetActive(false);
            animationContainer.Find("smoke_near_p1").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase1ToPhase2" && animationContainer.name.StartsWith("Mat_016_far"))
        {
            animationContainer.Find("smoke_far_p1").gameObject.SetActive(false);
            animationContainer.Find("smoke_far_p1").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase2ToPhase3" && animationContainer.name.StartsWith("Mat_016_near"))
        {
            animationContainer.Find("smoke_near_p2").gameObject.SetActive(false);
            animationContainer.Find("smoke_near_p2").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase2ToPhase3" && animationContainer.name.StartsWith("Mat_016_far"))
        {
            animationContainer.Find("smoke_far_p2").gameObject.SetActive(false);
            animationContainer.Find("smoke_far_p2").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase3ToPhase4" && animationContainer.name.StartsWith("Mat_016_near"))
        {
            animationContainer.Find("smoke_near_p3").gameObject.SetActive(false);
            animationContainer.Find("smoke_near_p3").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase3ToPhase4" && animationContainer.name.StartsWith("Mat_016_far"))
        {
            animationContainer.Find("smoke_far_p3").gameObject.SetActive(false);
            animationContainer.Find("smoke_far_p3").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase4ToEnd" && animationContainer.name.StartsWith("Mat_016_near"))
        {
            animationContainer.Find("smoke_near_p4").gameObject.SetActive(false);
            animationContainer.Find("smoke_near_p4").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase4ToEnd" && animationContainer.name.StartsWith("Mat_016_far"))
        {
            animationContainer.Find("smoke_far_p4").gameObject.SetActive(false);
            animationContainer.Find("smoke_far_p4").gameObject.SetActive(true);
        }

        //Mat_018 控制
        if (animationName == "DamagePhase1ToPhase2" && animationContainer.name.StartsWith("Mat_018_near"))
        {
            animationContainer.Find("fxp_Mat018_brk_ToPhase2_001_near").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase1ToPhase2" && animationContainer.name.StartsWith("Mat_018_far"))
        {
            animationContainer.Find("fxp_Mat018_brk_ToPhase2_001_far").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase2ToPhase3" && animationContainer.name.StartsWith("Mat_018_near"))
        {
            animationContainer.Find("fxp_Mat018_brk_ToPhase3_001_near").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase2ToPhase3" && animationContainer.name.StartsWith("Mat_018_far"))
        {
            animationContainer.Find("fxp_Mat018_brk_ToPhase3_001_far").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase3ToPhase4" && animationContainer.name.StartsWith("Mat_018_near"))
        {
            animationContainer.Find("fxp_Mat018_brk_ToPhase4_001_near").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase3ToPhase4" && animationContainer.name.StartsWith("Mat_018_far"))
        {
            animationContainer.Find("fxp_Mat018_brk_ToPhase4_001_far").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase4ToEnd" && animationContainer.name.StartsWith("Mat_018_near"))
        {
            animationContainer.Find("fxp_Mat018_brk_End_001_near").gameObject.SetActive(true);
        }
        if (animationName == "DamagePhase4ToEnd" && animationContainer.name.StartsWith("Mat_018_far"))
        {
            animationContainer.Find("fxp_Mat018_brk_End_001_far").gameObject.SetActive(true);
        }
    }    
    IEnumerator DelayShutdown(Transform trans, float time)
    {
        yield return new WaitForSeconds(time);
        trans.gameObject.SetActive(false);
    }

    public static void PlayParticle(Transform target, string name)
    {
        if(target == null) return;
        Transform[] allSon = target.GetComponentsInChildren<Transform>();
        foreach (Transform t in allSon)
        {
            if (t.name.ToLower().Contains(name.ToLower()))
            {
                Transform[] allGrandson = t.GetComponentsInChildren<Transform>();
                foreach (Transform grandson in allGrandson)
                {
                    var partical = grandson.GetComponent<ParticleSystem>();
                    if (partical != null) partical.Play();
                }
            }
        }
    }
    public static void PlayFieldSE(Transform target, string song)
    {
        string mat = "";
        if (target == LoadAssets.field1_) mat = LoadAssets.field1_.name.Substring(0, 7).ToUpper().Replace("MAT_", "MAT");
        if (target == LoadAssets.field2_) mat = LoadAssets.field2_.name.Substring(0, 7).ToUpper().Replace("MAT_", "MAT");
        string seFull = "SE_FIELD/" + mat + song;
        UIHelper.playSound(seFull, 1f);
        if (target == LoadAssets.field1_) UIHelper.playSound(seFull + "_P", 0.7f);
        if (target == LoadAssets.field2_) UIHelper.playSound(seFull + "_R", 0.7f);
    }
}
