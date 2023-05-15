using DG.Tweening;
using UnityEngine;
using YGOSharp;

public class MonoCardInDeckManager : MonoBehaviour
{
    public bool dying;

    private bool bool_physicalON;
    private Card _cardData = new Card();
    private bool died;

    private bool isDraging;
    private Banlist loaded_banlist;

    public Card cardData
    {
        get => _cardData;
        set
        {
            _cardData = value;
            LoadCard();
        }
    }

    private async void LoadCard()
    {
        gameObject.transform.Find("face").GetComponent<Renderer>().material.mainTexture =
            await GameTextureManager.GetCardPicture(_cardData.Id);
    }

    private void Update()
    {
        if (Program.I().deckManager.currentBanlist != loaded_banlist)
        {
            var ico = GetComponentInChildren<ban_icon>();
            loaded_banlist = Program.I().deckManager.currentBanlist;
            ico.show(loaded_banlist?.GetQuantity(_cardData.Id) ?? 3);
        }

        if (isDraging) gameObject.transform.position += (getGoodPosition(4) - gameObject.transform.position) * 0.3f;
        if (Vector3.Distance(Vector3.zero, gameObject.transform.position) > 50 && bool_physicalON) killIt();
    }

    public void killIt()
    {
        if (Program.I().deckManager.condition == DeckManager.Condition.changeSide)
        {
            gameObject.transform.position = new Vector3(0, 5, 0);
            endDrag();
            if (Program.I().deckManager.cardInDragging == this) Program.I().deckManager.cardInDragging = null;
            var rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null) rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.Sleep();
        }
        else
        {
            dying = true;
            died = true;
            gameObject.SetActive(false);
        }
    }

    public Vector3 getGoodPosition(float height)
    {
        var x = Input.mousePosition.x;
        var y = Input.mousePosition.y;
        var to_ltemp = Program.I().main_camera.ScreenToWorldPoint(new Vector3(x, y, 1));
        var dv = to_ltemp - Program.I().main_camera.transform.position;
        if (dv.y == 0) dv.y = 0.01f;
        to_ltemp.x = (height - Program.I().main_camera.transform.position.y)
            * dv.x / dv.y + Program.I().main_camera.transform.position.x;
        to_ltemp.y = (height - Program.I().main_camera.transform.position.y)
            * dv.y / dv.y + Program.I().main_camera.transform.position.y;
        to_ltemp.z = (height - Program.I().main_camera.transform.position.y)
            * dv.z / dv.y + Program.I().main_camera.transform.position.z;
        return to_ltemp;
    }

    public void beginDrag()
    {
        physicalOFF();
        physicalHalfON();
        isDraging = true;
        Program.go(1, () => { iTween.RotateTo(gameObject, new Vector3(90, 0, 0), 0.6f); });
    }

    public void endDrag()
    {
        physicalON();
        isDraging = false;
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || getIfAlive() == false)
        {
            var form_position = getGoodPosition(4);
            var to_position = getGoodPosition(0);
            var delta_position = to_position - form_position;
            GetComponent<Rigidbody>().AddForce(delta_position * 1000);
            dying = true;
        }
    }

    public void tweenToVectorAndFall(Vector3 position, Vector3 rotation)
    {
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null) rigidbody.Sleep();

        transform.DOMove(position, 0.2f).OnComplete(physicalON);
        transform.DORotate(rotation, 0.15f);
        // iTween.MoveTo(gameObject, iTween.Hash(
        //                     "delay", delay,
        //                     "x", position.x,
        //                     "y", position.y,
        //                     "z", position.z,
        //                     "time", 0.2f,
        //                     "oncomplete", (Action)physicalON
        //                     ));
        // iTween.RotateTo(gameObject, iTween.Hash(
        //                   "delay", delay,
        //                   "x", rotation.x,
        //                   "y", rotation.y,
        //                   "z", rotation.z,
        //                   "time", 0.15f
        //                   ));
        physicalOFF();
    }

    private void physicalON()
    {
        bool_physicalON = true;
        GetComponent<BoxCollider>().enabled = true;
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null) rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.Sleep();
        rigidbody.useGravity = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void physicalHalfON()
    {
        bool_physicalON = false;
        GetComponent<BoxCollider>().enabled = true;
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody == null) rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void physicalOFF()
    {
        bool_physicalON = false;
        GetComponent<BoxCollider>().enabled = false;
        var rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null) rigidbody.useGravity = false;
    }

    public bool getIfAlive()
    {
        var ret = true;
        if (died) ret = false;
        if (gameObject.transform.position.y < -0.5f) ret = false;
        var to_ltemp = refLectPosition(gameObject.transform.position);
        if (to_ltemp.x < -15.2f) ret = false;
        if (to_ltemp.x > 15.2f) ret = false;

        if (Program.I().deckManager.condition == DeckManager.Condition.changeSide) ret = true;
        return ret;
    }

    public static Vector3 refLectPosition(Vector3 pos)
    {
        var to_ltemp = pos;
        var dv = to_ltemp - Program.I().main_camera.transform.position;
        if (dv.y == 0) dv.y = 0.01f;
        to_ltemp.x = (0 - Program.I().main_camera.transform.position.y)
            * dv.x / dv.y + Program.I().main_camera.transform.position.x;
        to_ltemp.y = (0 - Program.I().main_camera.transform.position.y)
            * dv.y / dv.y + Program.I().main_camera.transform.position.y;
        to_ltemp.z = (0 - Program.I().main_camera.transform.position.y)
            * dv.z / dv.y + Program.I().main_camera.transform.position.z;
        return to_ltemp;
    }
}