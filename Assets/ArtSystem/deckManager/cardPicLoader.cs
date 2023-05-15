using UnityEngine;
using YGOSharp;

public class cardPicLoader : MonoBehaviour
{
    private int _code;
    public Texture2D defaults;
    public ban_icon ico;
    public Collider coli;
    public UITexture uiTexture;
    public Banlist loaded_banlist;
    public Card data { get; set; }

    public int code
    {
        get => _code;
        set
        {
            _code = value;
            LoadCard();
        }
    }

    private async void LoadCard()
    {
        uiTexture.mainTexture = await GameTextureManager.GetCardPicture(code);
        if (uiTexture.mainTexture == null) return;
        uiTexture.aspectRatio = (float) uiTexture.mainTexture.width / uiTexture.mainTexture.height;
        uiTexture.forceWidth((int) (uiTexture.height * uiTexture.aspectRatio));
        loaded_banlist = null;
    }

    private void Update()
    {        
        if (coli != null && Program.InputGetMouseButtonDown_0 && Program.pointedCollider == coli)
            Program.I().cardDescription.setData(CardsManager.Get(_code), GameTextureManager.myBack, "", true);
        if (Program.I().deckManager != null)
        {
            if (loaded_banlist != Program.I().deckManager.currentBanlist)
            {
                loaded_banlist = Program.I().deckManager.currentBanlist;
                if (ico != null)
                {
                    if (loaded_banlist == null)
                    {
                        ico.show(3);
                        return;
                    }

                    ico.show(loaded_banlist.GetQuantity(_code));
                }
            }
        }
    }

    public void clear()
    {
        _code = 0;
        ico.show(3);
        uiTexture.mainTexture = null;
    }

    public void relayer(int l)
    {
        uiTexture.depth = 50 + l * 2;
        var t = ico.gameObject.GetComponent<UITexture>();
        t.depth = 51 + l * 2;
    }
}