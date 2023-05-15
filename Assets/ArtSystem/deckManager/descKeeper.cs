using UnityEngine;

public class descKeeper : MonoBehaviour
{
    public UITexture card;

    public UITexture back;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (back.width < card.width) back.width = card.width + 2;
        back.transform.localPosition = new Vector3(back.width / 2f, 0);
        var leftTop = new Vector3(-back.width / 2 + 2 + back.transform.localPosition.x,
            +back.height / 2 - 2 + back.transform.localPosition.y);
        card.transform.localPosition = new Vector3(leftTop.x + card.width / 2, leftTop.y - card.height / 2);
        Program.I().cardDescription.width = back.width - 2;
        Program.I().cardDescription.cHeight = card.height;
    }
}