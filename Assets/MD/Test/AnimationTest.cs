using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    public string animationName;
    public int layer;
    public float normalizedTime;
    public bool isPlaying= false;

    Animator[] allAnimator;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        allAnimator = this.GetComponentsInChildren<Animator>();
        foreach (Animator anim in allAnimator)
        {
            Debug.Log("组件名称：" + anim.name);

            AnimationClip[] animationClips = anim.runtimeAnimatorController.animationClips;

            foreach (AnimationClip clip in animationClips)
            {
                Debug.Log("动画名称：" + clip.name);
            }
            AnimatorControllerParameter[] parameter = anim.parameters;
            foreach (var param in parameter) Debug.Log(": 参数名称：" + param.name + " 参数类型："+param.type);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            isPlaying = false;
            i = 0;
            foreach (Animator anim in allAnimator)
            {
                //anim.SetTrigger(animationName);
                anim.Play(animationName, layer, normalizedTime);
                //anim.CrossFade(animationName, 0.3f);
                i++;
            }
            Debug.Log("set次数：" + i);
        }
    }
}
