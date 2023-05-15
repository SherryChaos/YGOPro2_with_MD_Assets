using System;
using DefaultNamespace;
using UnityEngine;

public class lazyPlayer : MonoBehaviour
{
    public UIToggle prep;

    public UIButton prepAsButton;

    public UIButton prepAsCollider;

    public UIButton kickAsButton;

    public UISprite prepAsTexture;

    public UILabel UILabel_name;

    public UITexture face;

    public GameObject line;

    public Transform transformOfPrepFore;

    private bool canKick;

    private int me;

    private bool mIfMe;

    private string mName;

    private bool NotNull;

    public Action<int> onKick = null;

    public Action<int, bool> onPrepareChanged = null;

    // Use this for initialization
    private void Start()
    {
        UIHelper.registEvent(prepAsButton, OnPrepClicked);
        UIHelper.registEvent(kickAsButton, OnKickClicked);
        // ini();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void ini()
    {
        me = int.Parse(gameObject.name);
        setIfprepared(false);
        setIfMe(false);
        SetNotNull(false);
        SetIFcanKick(false);
        SetName("");
    }

    private void OnPrepClicked()
    {
        if (onPrepareChanged != null) onPrepareChanged(me, prep.value);
    }

    private void OnKickClicked()
    {
        Program.DEBUGLOG("OnKickClicked  " + me);
        if (onKick != null) onKick(me);
        //setIfMe(!getIfMe());//bb
    }

    public void SetIFcanKick(bool canKick_)
    {
        canKick = canKick_;
        if (canKick)
            kickAsButton.gameObject.SetActive(true);
        else
            kickAsButton.gameObject.SetActive(false);
    }

    public bool getIfcanKick()
    {
        return canKick;
    }

    public void SetNotNull(bool notNull_)
    {
        NotNull = notNull_;
        if (NotNull)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            setIfprepared(false);
            transformOfPrepFore.localScale = Vector3.zero;
            transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public bool getIfNull()
    {
        return NotNull;
    }

    public void setIfMe(bool isMe)
    {
        mIfMe = isMe;
        if (mIfMe)
        {
            line.SetActive(true);
            prepAsTexture.color = Color.white;
            prepAsCollider.isEnabled = true;
        }
        else
        {
            line.SetActive(false);
            prepAsTexture.color = Color.gray;
            prepAsCollider.isEnabled = false;
        }
    }

    public bool getIfMe()
    {
        return mIfMe;
    }

    public void SetName(string name)
    {
        mName = name;
        UILabel_name.text = name;
        MyCard.LoadAvatar(name, texture => face.mainTexture = texture);
    }

    public string getName()
    {
        return mName;
    }

    public void setIfprepared(bool preped)
    {
        prep.value = preped;
    }

    public bool getIfPreped()
    {
        return prep.value;
    }
}