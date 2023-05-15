using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DG.Tweening;
using UnityEngine;
using YGOSharp;
using YGOSharp.OCGWrapper.Enums;
using Object = UnityEngine.Object;

public class DeckManager : ServantWithCardDescription
{
    private float cameraAngle = Mathf.Atan(23 / 17.5f);

    private float cameraDistance = Vector3.Distance(new Vector3(0, 23f, -17.5f), Vector3.zero);

    private bool canSave;

    public MonoCardInDeckManager cardInDragging;

    public Deck deck = new Deck();
    public bool deckDirty;


    private readonly List<GameObject> diedCards = new List<GameObject>();

    private number_loader extra_unmber;

    private GameObject gameObjectDesk;
    private GameObject goLast;

    private number_loader m_unmber;

    private number_loader main_unmber;

    private number_loader s_number;

    private number_loader side_number;

    private number_loader t_unmber;
    private int timeLastDown;

    public override void show()
    {
        base.show();
        Program.I().main_camera.transform.position = new Vector3(0, 35, 0);
        Program.I().main_camera.transform.localEulerAngles = new Vector3(90, 0, 0);
        cameraAngle = 90;
        Program.cameraFacing = true;
        Program.cameraPosition = Program.I().main_camera.transform.position;
        camrem();
        Program.I().light.transform.eulerAngles = new Vector3(70, 0, 0);
        gameObjectDesk = create_s(Program.I().new_mod_tableInDeckManager);//卡组编辑
        //关闭UI
        UIHandler.CloseHomeUI();
        //切换BGM
        BGMHandler.ChangeBGM("deck");
        gameObjectDesk.layer = 16;
        gameObjectDesk.transform.position = new Vector3(0, 0, 0);
        gameObjectDesk.transform.eulerAngles = new Vector3(90, 0, 0);
        gameObjectDesk.transform.localScale = new Vector3(30, 30, 1);
        gameObjectDesk.GetComponent<Renderer>().material.mainTexture =
            Program.GetTextureViaPath("texture/duel/deckTable.png");
        //UIHelper.SetMaterialRenderingMode(gameObjectDesk.GetComponent<Renderer>().material, UIHelper.RenderingMode.Transparent);
        var rigidbody = gameObjectDesk.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        var boxCollider = gameObjectDesk.AddComponent<BoxCollider>();
        main_unmber =
            create_s(Program.I().mod_ocgcore_number, new Vector3(-16.5f, 0, 13.6f), new Vector3(90, 0, 0), true)
                .GetComponent<number_loader>();
        m_unmber = create_s(Program.I().mod_ocgcore_number, new Vector3(-16.5f, 0, 6.6f), new Vector3(90, 0, 0), true)
            .GetComponent<number_loader>();
        s_number = create_s(Program.I().mod_ocgcore_number, new Vector3(-16.5f, 0, 4.6f), new Vector3(90, 0, 0), true)
            .GetComponent<number_loader>();
        t_unmber = create_s(Program.I().mod_ocgcore_number, new Vector3(-16.5f, 0, 2.6f), new Vector3(90, 0, 0), true)
            .GetComponent<number_loader>();
        extra_unmber =
            create_s(Program.I().mod_ocgcore_number, new Vector3(-16.5f, 0, -5.3f), new Vector3(90, 0, 0), true)
                .GetComponent<number_loader>();
        side_number =
            create_s(Program.I().mod_ocgcore_number, new Vector3(-16.5f, 0, -11f), new Vector3(90, 0, 0), true)
                .GetComponent<number_loader>();
        switch (condition)
        {
            case Condition.editDeck:
                boxCollider.size = new Vector3(1, 1, 1);
                break;
            case Condition.changeSide:
                boxCollider.size = new Vector3(100, 100, 1);
                break;
        }

        clearAll();
    }

    public override void hide()
    {
        if (isShowed) hideDetail();
        for (var i = 0; i < deck.IMain.Count; i++) destroyCard(deck.IMain[i]);
        for (var i = 0; i < deck.IExtra.Count; i++) destroyCard(deck.IExtra[i]);
        for (var i = 0; i < deck.ISide.Count; i++) destroyCard(deck.ISide[i]);
        for (var i = 0; i < deck.IRemoved.Count; i++) destroyCard(deck.IRemoved[i]);
        deck = new Deck();
        deckDirty = false;
        Program.I().cardDescription.setTitle("");
        base.hide();
    }

    public override void preFrameFunction()
    {
        base.preFrameFunction();
        if (cardInDragging != null)
        {
            if (detailPanelShiftedTemp == false) shiftDetailPanel(true);
        }
        else
        {
            if (detailPanelShiftedTemp) shiftDetailPanel(false);
        }

        camrem();
        cameraDistance = 29 - 3.1415926f / 180f * (cameraAngle - 60f) * 13f;
        Program.cameraPosition = new Vector3(0, cameraDistance * Mathf.Sin(3.1415926f / 180f * cameraAngle)*3,
            -cameraDistance * Mathf.Cos(3.1415926f / 180f * cameraAngle)*3);
        if (Program.TimePassed() - lastRefreshTime > 80)
        {
            lastRefreshTime = Program.TimePassed();
            FromObjectDeckToCodedDeck();
            main_unmber.set_number(deck.Main.Count, 3);
            side_number.set_number(deck.Side.Count, 5);
            extra_unmber.set_number(deck.Extra.Count, 4);
            int m = 0, s = 0, t = 0;
            foreach (var item in deck.IMain)
            {
                if ((item.cardData.Type & (int) CardType.Monster) > 0) m++;
                if ((item.cardData.Type & (int) CardType.Spell) > 0) s++;
                if ((item.cardData.Type & (int) CardType.Trap) > 0) t++;
            }

            m_unmber.set_number(m, 1);
            s_number.set_number(s, 2);
            t_unmber.set_number(t, 0);
        }

        if (Program.InputEnterDown)
            if (condition == Condition.editDeck)
                onClickSearch();
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetMouseButtonDown(2)) onClickDetail();
    }

    private void camrem()
    {
        var l = Program.I().cardDescription.width + Screen.width * 0.03f;
        var r = Screen.width - 230f;
        if (detailShowed)
            if (gameObjectDetailedSearch != null)
                r -= 230 * gameObjectDetailedSearch.transform.localScale.x;
        Program.reMoveCam((l + r) / 2f);
    }

    public override void ES_HoverOverGameObject(GameObject gameObject)
    {
        var cardInDeck = gameObject.GetComponent<MonoCardInDeckManager>();
        if (cardInDeck != null) Program.I().cardDescription.setData(cardInDeck.cardData, GameTextureManager.myBack);
        var cardInSearchResult = gameObject.GetComponent<cardPicLoader>();
        if (cardInSearchResult != null)
            Program.I().cardDescription.setData(cardInSearchResult.data, GameTextureManager.myBack);
    }

    public override void ES_mouseDownGameObject(GameObject gameObject)
    {
        var doubleClick = false;
        if (goLast == gameObject)
            if (Program.TimePassed() - timeLastDown < 300)
                doubleClick = true;
        goLast = gameObject;
        timeLastDown = Program.TimePassed();
        var cardInDeck = gameObject.GetComponent<MonoCardInDeckManager>();
        var cardInSearchResult = gameObject.GetComponent<cardPicLoader>();
        if (cardInDeck != null && !cardInDeck.dying)
        {
            if (doubleClick && condition == Condition.editDeck && checkBanlistAvail(cardInDeck.cardData.Id))
            {
                var card = createCard();
                card.transform.position = cardInDeck.transform.position;
                cardInDeck.cardData.cloneTo(card.cardData);
                card.gameObject.layer = 16;
                deck.IMain.Add(card);
                deckDirty = true;
                ArrangeObjectDeck(true);
                ShowObjectDeck();
            }
            else
            {
                cardInDragging = cardInDeck;
                cardInDeck.beginDrag();
            }
        }
        else if (cardInSearchResult != null)
        {
            if (condition == Condition.editDeck)
                if (checkBanlistAvail(cardInSearchResult.data.Id))
                    if ((cardInSearchResult.data.Type & (uint) CardType.Token) == 0)
                    {
                        var card = createCard();
                        card.transform.position = card.getGoodPosition(4);
                        card.cardData = cardInSearchResult.data;
                        card.gameObject.layer = 16;
                        deck.IMain.Add(card);
                        cardInDragging = card;
                        card.beginDrag();
                    }
        }
    }

    public override void ES_mouseUp()
    {
        if (cardInDragging != null)
        {
            if (cardInDragging.getIfAlive()) deckDirty = true;
            ArrangeObjectDeck(true);
            ShowObjectDeck();

            cardInDragging.endDrag();
            cardInDragging = null;
        }
    }

    public override void ES_mouseUpRight()
    {
        if (Program.pointedGameObject != null)
        {
            if (condition == Condition.editDeck)
            {
                var cardInDeck = Program.pointedGameObject.GetComponent<MonoCardInDeckManager>();
                if (cardInDeck != null)
                {
                    cardInDeck.killIt();
                    ArrangeObjectDeck(true);
                    ShowObjectDeck();
                }

                var cardInSearchResult = Program.pointedGameObject.GetComponent<cardPicLoader>();
                if (cardInSearchResult != null)
                {
                    CreateMonoCard(cardInSearchResult.data);
                    ShowObjectDeck();
                }
            }
            else
            {
                var cardInDeck = Program.pointedGameObject.GetComponent<MonoCardInDeckManager>();
                if (cardInDeck != null)
                {
                    var isSide = false;
                    for (var i = 0; i < deck.ISide.Count; i++)
                        if (cardInDeck == deck.ISide[i])
                            isSide = true;
                    if (isSide)
                    {
                        if (cardInDeck.cardData.IsExtraCard())
                        {
                            deck.IExtra.Add(cardInDeck);
                            deck.ISide.Remove(cardInDeck);
                        }
                        else
                        {
                            deck.IMain.Add(cardInDeck);
                            deck.ISide.Remove(cardInDeck);
                        }
                    }
                    else
                    {
                        deck.ISide.Add(cardInDeck);
                        deck.IMain.Remove(cardInDeck);
                        deck.IExtra.Remove(cardInDeck);
                    }

                    ShowObjectDeck();
                }
            }
        }
    }

    private void CreateMonoCard(Card data)
    {
        if (checkBanlistAvail(data.Id))
        {
            var card = createCard();
            card.transform.position = card.getGoodPosition(4);
            card.cardData = data;
            card.gameObject.layer = 16;
            if (data.IsExtraCard())
            {
                deck.IExtra.Add(card);
                deck.Extra.Add(card.cardData.Id);
            }
            else
            {
                deck.IMain.Add(card);
                deck.Main.Add(card.cardData.Id);
            }

            deckDirty = true;
        }
    }

    public void loadDeckFromYDK(string path)
    {
        FromYDKtoCodedDeck(path, out deck);
        FormCodedDeckToObjectDeck();
        deckDirty = false;
    }

    public static void FromYDKtoCodedDeck(string path, out Deck deck)
    {
        deck = new Deck();
        try
        {
            var text = File.ReadAllText(path);
            var st = text.Replace("\r", "");
            var lines = st.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);
            var flag = -1;
            foreach (var line in lines)
                if (line == "#main")
                {
                    flag = 1;
                }
                else if (line == "#extra")
                {
                    flag = 2;
                }
                else if (line == "!side")
                {
                    flag = 3;
                }
                else
                {
                    var code = 0;
                    try
                    {
                        code = int.Parse(line);
                    }
                    catch (Exception)
                    {
                    }

                    if (code > 100)
                    {
                        var card = CardsManager.Get(code);
                        if (card.Id > 0 && flag != 3)
                        {
                            if (card.IsExtraCard())
                            {
                                deck.Extra.Add(code);
                                deck.Deck_O.Extra.Add(code);
                            }
                            else
                            {
                                deck.Main.Add(code);
                                deck.Deck_O.Main.Add(code);
                            }
                        }
                        else
                        {
                            switch (flag)
                            {
                                case 1:
                                {
                                    deck.Main.Add(code);
                                    deck.Deck_O.Main.Add(code);
                                }
                                    break;
                                case 2:
                                {
                                    deck.Extra.Add(code);
                                    deck.Deck_O.Extra.Add(code);
                                }
                                    break;
                                case 3:
                                {
                                    deck.Side.Add(code);
                                    deck.Deck_O.Side.Add(code);
                                }
                                    break;
                            }
                        }
                    }
                }
        }
        catch (Exception e)
        {
        }
    }

    public Deck getRealDeck()
    {
        if (canSave) return deck;

        var r = new Deck();
        foreach (var item in deck.Deck_O.Main)
        {
            r.Main.Add(item);
            r.Deck_O.Main.Add(item);
        }

        foreach (var item in deck.Deck_O.Side)
        {
            r.Side.Add(item);
            r.Deck_O.Side.Add(item);
        }

        foreach (var item in deck.Deck_O.Extra)
        {
            r.Extra.Add(item);
            r.Deck_O.Extra.Add(item);
        }

        return r;
    }

    private void ArrangeObjectDeck(bool order = false)
    {
        var deckTemp = deck.getAllObjectCardAndDeload();
        if (order)
            deckTemp.Sort((left, right) =>
            {
                var leftPosition = left.gameObject.transform.position;
                var rightPosition = right.gameObject.transform.position;
                if (leftPosition.y > 3f) leftPosition = MonoCardInDeckManager.refLectPosition(leftPosition);
                if (rightPosition.y > 3f) rightPosition = MonoCardInDeckManager.refLectPosition(rightPosition);
                if (leftPosition.z > -3 && rightPosition.z > -3)
                {
                    var l = leftPosition.x + 1000f * (int) ((13f - leftPosition.z) / 3.7f);
                    var r = rightPosition.x + 1000f * (int) ((13f - rightPosition.z) / 3.7f);
                    if (l < r)
                        return -1;
                    return 1;
                }

                if (leftPosition.z > -3 && rightPosition.z < -3) return 1;

                if (leftPosition.z < -3 && rightPosition.z > -3) return -1;

                {
                    var l = leftPosition.x;
                    var r = rightPosition.x;
                    if (l < r)
                        return -1;
                    return 1;
                }
            });
        for (var i = 0; i < deckTemp.Count; i++)
        {
            var p = deckTemp[i].gameObject.transform.position;
            if (deckTemp[i].getIfAlive())
            {
                if (p.z > -8)
                {
                    if (deckTemp[i].cardData.IsExtraCard())
                        deck.IExtra.Add(deckTemp[i]);
                    else
                        deck.IMain.Add(deckTemp[i]);
                }
                else
                {
                    deck.ISide.Add(deckTemp[i]);
                }
            }
            else
            {
                deck.IRemoved.Add(deckTemp[i]);
            }
        }
    }

    private void SortObjectDeck()
    {
        Deck.sort((List<MonoCardInDeckManager>) deck.IMain);
        Deck.sort((List<MonoCardInDeckManager>) deck.IExtra);
        Deck.sort((List<MonoCardInDeckManager>) deck.ISide);
        deckDirty = true;
    }

    private void RandObjectDeck()
    {
        Deck.rand((List<MonoCardInDeckManager>) deck.IMain);
        deckDirty = true;
    }

    private MonoCardInDeckManager createCard()
    {
        MonoCardInDeckManager r = null;
        if (diedCards.Count > 0)
        {
            r = diedCards[0].AddComponent<MonoCardInDeckManager>();
            diedCards.RemoveAt(0);
        }

        if (r == null)
        {
            r = Program.I().create(Program.I().new_mod_cardInDeckManager).AddComponent<MonoCardInDeckManager>();
            r.gameObject.transform.Find("back").gameObject.GetComponent<Renderer>().material.mainTexture =
                GameTextureManager.myBack;
            r.gameObject.transform.Find("face").gameObject.GetComponent<Renderer>().material.mainTexture =
                GameTextureManager.myBack;
        }

        r.gameObject.transform.position = new Vector3(0, 5, 0);
        r.gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
        r.gameObject.transform.localScale = new Vector3(0, 0, 0);
        iTween.ScaleTo(r.gameObject, new Vector3(3, 4, 1), 0.4f);
        r.gameObject.SetActive(true);
        return r;
    }

    private void destroyCard(MonoCardInDeckManager c)
    {
        try
        {
            c.gameObject.SetActive(false);
            diedCards.Add(c.gameObject);
            Object.DestroyImmediate(c);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void FormCodedDeckToObjectDeck()
    {
        canSave = false;
        safeGogo(4000, () => { canSave = true; });
        var indexOfLogic = 0;
        var hangshu = UIHelper.get_decklieshuArray(deck.Main.Count);
        foreach (var item in deck.Main)
        {
            var v = UIHelper.get_hang_lieArry(indexOfLogic, hangshu);
            var toVector = new Vector3(UIHelper.get_left_right_index(-12.5f, 12.5f, (int) v.y, hangshu[(int) v.x]),
                0.5f + v.y / 3f + v.x / 3f, 11.8f - v.x * 4f);
            var data = CardsManager.Get(item);
            safeGogo(indexOfLogic * 25, () =>
            {
                var card = createCard();
                card.cardData = data;
                card.gameObject.layer = 16;
                deck.IMain.Add(card);
                card.tweenToVectorAndFall(toVector, new Vector3(90, 0, 0));
            });
            indexOfLogic++;
        }

        indexOfLogic = 0;
        foreach (var item in deck.Extra)
        {
            var toVector =
                new Vector3(UIHelper.get_left_right_indexZuo(-12.5f, 12.5f, indexOfLogic, deck.Extra.Count, 10),
                    0.5f + indexOfLogic / 3f, -6.2f);
            var data = CardsManager.Get(item);
            safeGogo(indexOfLogic * 90, () =>
            {
                var card = createCard();
                card.cardData = data;
                card.gameObject.layer = 16;
                deck.IExtra.Add(card);
                card.tweenToVectorAndFall(toVector, new Vector3(90, 0, 0));
            });
            indexOfLogic++;
        }

        indexOfLogic = 0;
        foreach (var item in deck.Side)
        {
            var toVector =
                new Vector3(UIHelper.get_left_right_indexZuo(-12.5f, 12.5f, indexOfLogic, deck.Side.Count, 10),
                    0.5f + indexOfLogic / 3f, -12f);
            var data = CardsManager.Get(item);
            safeGogo(indexOfLogic * 90, () =>
            {
                var card = createCard();
                card.cardData = data;
                card.gameObject.layer = 16;
                deck.ISide.Add(card);
                card.tweenToVectorAndFall(toVector, new Vector3(90, 0, 0));
            });
            indexOfLogic++;
        }
    }

    private void ShowObjectDeck()
    {
        var k = (float) (1.5 * 0.1 / 0.130733633);
        var hangshu = UIHelper.get_decklieshuArray(deck.IMain.Count);
        for (var i = 0; i < deck.IMain.Count; i++)
        {
            var v = UIHelper.get_hang_lieArry(i, hangshu);
            var toAngle = new Vector3(90, 0, 0);
            if ((int) v.y > 0)
            {
                toAngle = new Vector3(87, -90, -90);
                if (hangshu[(int) v.x] > 10) toAngle = new Vector3(87f - (hangshu[(int) v.x] - 10f) * 0.4f, -90, -90);
            }

            var toVector =
                new Vector3(UIHelper.get_left_right_indexZuo(-12.5f, 12.5f, (int) v.y, hangshu[(int) v.x], 10),
                    0.6f + Mathf.Sin((90 - toAngle.x) / 180f * Mathf.PI) * k, 11.8f - v.x * 4f);
            deck.IMain[i].tweenToVectorAndFall(toVector, toAngle);
        }

        for (var i = 0; i < deck.IExtra.Count; i++)
        {
            var toAngle = new Vector3(90, 0, 0);
            if (i > 0)
            {
                toAngle = new Vector3(87, -90, -90);
                if (deck.IExtra.Count > 10) toAngle = new Vector3(87f - (deck.IExtra.Count - 10f) * 0.4f, -90, -90);
            }

            var toVector = new Vector3(UIHelper.get_left_right_indexZuo(-12.5f, 12.5f, i, deck.IExtra.Count, 10),
                0.6f + Mathf.Sin((90 - toAngle.x) / 180f * Mathf.PI) * k, -6.2f);
            deck.IExtra[i].tweenToVectorAndFall(toVector, toAngle);
        }

        for (var i = 0; i < deck.ISide.Count; i++)
        {
            var toAngle = new Vector3(90, 0, 0);
            if (i > 0)
            {
                toAngle = new Vector3(87, -90, -90);
                if (deck.ISide.Count > 10) toAngle = new Vector3(87f - (deck.ISide.Count - 10f) * 0.4f, -90, -90);
            }

            var toVector = new Vector3(UIHelper.get_left_right_indexZuo(-12.5f, 12.5f, i, deck.ISide.Count, 10),
                0.6f + Mathf.Sin((90 - toAngle.x) / 180f * Mathf.PI) * k, -12f);
            deck.ISide[i].tweenToVectorAndFall(toVector, toAngle);
        }
    }

    public void FromObjectDeckToCodedDeck(bool order = false)
    {
        ArrangeObjectDeck(order);
        deck.Main.Clear();
        deck.Extra.Clear();
        deck.Side.Clear();
        foreach (var item in deck.IMain) deck.Main.Add(item.cardData.Id);
        foreach (var item in deck.IExtra) deck.Extra.Add(item.cardData.Id);
        foreach (var item in deck.ISide) deck.Side.Add(item.cardData.Id);
    }

    public void setGoodLooking(bool side = false)
    {
        try
        {
            Program.I().cardDescription.setData(CardsManager.Get(deck.Main[0]), GameTextureManager.myBack);
        }
        catch (Exception e) { }

        if (side)
        {
            var result = new List<Card>();
            foreach (var item in Program.I().ocgcore.sideReference) result.Add(CardsManager.Get(item.Value));
            print(result);
            UIHelper.trySetLableText(gameObjectSearch, "title_", result.Count.ToString());
        }
        else
        {
            UIHelper.trySetLableText(gameObjectSearch, "title_", InterString.Get("在此搜索卡片，拖动加入卡组"));
        }

        Program.go(50, superScrollView.toTop);
        Program.go(100, superScrollView.toTop);
        Program.go(200, superScrollView.toTop);
        Program.go(300, superScrollView.toTop);
        Program.go(400, superScrollView.toTop);
        Program.go(500, superScrollView.toTop);
        if (side)
        {
            UIInput_search.value = InterString.Get("对手使用过的卡↓");
            UIInput_search.isSelected = false;
        }
        else
        {
            UIInput_search.value = "";
            UIInput_search.isSelected = true;
        }
    }

    #region UI

    public enum Condition
    {
        editDeck = 1,
        changeSide = 2
    }

    public Condition condition = Condition.editDeck;

    public GameObject gameObjectSearch;

    public GameObject gameObjectDetailedSearch;

    private UIPopupList UIPopupList_main;

    private UIPopupList UIPopupList_ban;

    private UIPopupList UIPopupList_second;

    private UIPopupList UIPopupList_race;

    private UIPopupList UIPopupList_attribute;

    private UIPopupList UIPopupList_pack;

    private UIInput UIInput_level;

    private UIInput UIInput_atk;

    private UIInput UIInput_def;

    private UIInput UIInput_search;

    private readonly UIToggle[] UIToggle_effects = new UIToggle[32];

    private SuperScrollView superScrollView;

    private UIPopupList UIPopupList_banlist;

    public override void initialize()
    {
        gameObjectSearch = Program.I().new_ui_search;
        gameObjectDetailedSearch = Program.I().new_ui_searchDetailed;
        UIHelper.InterGameObject(gameObjectSearch);
        UIHelper.InterGameObject(gameObjectDetailedSearch);
        shiftCondition(Condition.editDeck);
        UIHelper.registEvent(gameObjectSearch, "detailed_", onClickDetail);
        UIHelper.registEvent(gameObjectSearch, "search_", onClickSearch);
        UIPopupList_main = UIHelper.getByName<UIPopupList>(gameObjectDetailedSearch, "main_");
        UIPopupList_ban = UIHelper.getByName<UIPopupList>(gameObjectDetailedSearch, "ban_");
        UIPopupList_second = UIHelper.getByName<UIPopupList>(gameObjectDetailedSearch, "second_");
        UIPopupList_race = UIHelper.getByName<UIPopupList>(gameObjectDetailedSearch, "race_");
        UIPopupList_attribute = UIHelper.getByName<UIPopupList>(gameObjectDetailedSearch, "attribute_");
        UIPopupList_pack = UIHelper.getByName<UIPopupList>(gameObjectDetailedSearch, "pack_");
        UIInput_search = UIHelper.getByName<UIInput>(gameObjectSearch, "input_");
        UIInput_search.value = "";
        UIInput_level = UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "level_");
        UIInput_atk = UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "atk_");
        UIInput_def = UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "def_");
        for (var i = 0; i < 32; i++)
        {
            UIToggle_effects[i] = UIHelper.getByName<UIToggle>(gameObjectDetailedSearch, "T (" + (i + 1) + ")");
            UIHelper.trySetLableText(UIToggle_effects[i].gameObject, GameStringManager.get_unsafe(1100 + i));
            UIToggle_effects[i].GetComponentInChildren<UILabel>().overflowMethod = UILabel.Overflow.ClampContent;
        }

        UIPopupList_pack.Clear();
        UIPopupList_pack.AddItem(GameStringManager.get_unsafe(1310));
        foreach (var item in PacksManager.packs) UIPopupList_pack.AddItem(item.fullName);
        UIPopupList_main.Clear();
        UIPopupList_main.AddItem(GameStringManager.get_unsafe(1310));
        UIPopupList_main.AddItem(GameStringManager.get_unsafe(1312));
        UIPopupList_main.AddItem(GameStringManager.get_unsafe(1313));
        UIPopupList_main.AddItem(GameStringManager.get_unsafe(1314));
        UIPopupList_ban.Clear();
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1310));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1316));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1317));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1318));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1481));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1482));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1483));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1484));
        UIPopupList_ban.AddItem(GameStringManager.get_unsafe(1485));
        clearAll();
        UIHelper.registEvent(UIPopupList_main.gameObject, onUIPopupList_main);
        UIHelper.registEvent(UIPopupList_second.gameObject, onUIPopupList_second);
        superScrollView = new SuperScrollView
        (
            UIHelper.getByName<UIPanel>(gameObjectSearch, "panel_"),
            UIHelper.getByName<UIScrollBar>(gameObjectSearch, "bar_"),
            itemOnListProducer,
            86
        );
        Program.go(500, () =>
        {
            var cs = new List<MonoCardInDeckManager>();
            for (var i = 0; i < 300; i++) cs.Add(createCard());
            for (var i = 0; i < 300; i++) destroyCard(cs[i]);
        });
    }

    private GameObject itemOnListProducer(string[] args)
    {
        var returnValue = create(Program.I().new_ui_cardOnSearchList, Vector3.zero, Vector3.zero, false,
            Program.I().ui_back_ground_2d);
        UIHelper.getRealEventGameObject(returnValue).name = args[0];
        UIHelper.trySetLableText(returnValue, args[2]);
        var cardPicLoader = UIHelper.getRealEventGameObject(returnValue).AddComponent<cardPicLoader>();
        cardPicLoader.uiTexture = UIHelper.getByName<UITexture>(returnValue, "pic_");
        cardPicLoader.code = int.Parse(args[0]);
        cardPicLoader.data = CardsManager.Get(int.Parse(args[0]));
        cardPicLoader.ico = UIHelper.getByName<ban_icon>(returnValue);
        cardPicLoader.ico.show(3);
        return returnValue;
    }

    public override void applyHideArrangement()
    {
        base.applyHideArrangement();
        Program.cameraFacing = false;
        gameObjectSearch.transform.DOMove(
            Utils.UIToWorldPoint(new Vector3(Utils.UIWidth() + 600, (float) Utils.UIHeight() / 2, 600)),
            1.2f);
        refreshDetail();
    }

    public override void applyShowArrangement()
    {
        base.applyShowArrangement();
        Program.cameraFacing = true;
        var tex = UIHelper.getByName<UITexture>(gameObjectSearch, "under_");
        tex.height = Utils.UIHeight();
        gameObjectSearch.transform.DOMove(
            Utils.UIToWorldPoint(new Vector3(Utils.UIWidth() - tex.width / 2, (float) Utils.UIHeight() / 2, 600)),
            1.2f);
        refreshDetail();
    }

    private void onLF()
    {
        currentBanlist = BanlistManager.GetByName(UIPopupList_banlist.value);
    }

    public void shiftCondition(Condition condition)
    {
        this.condition = condition;
        switch (condition)
        {
            case Condition.editDeck:
                UIHelper.setParent(gameObjectSearch, Program.I().ui_back_ground_2d);
                CreateBar(Program.I().new_bar_editDeck, 0, 230);
                UIPopupList_banlist = UIHelper.getByName<UIPopupList>(toolBar, "lfList_");
                var banlistNames = BanlistManager.getAllName();
                UIPopupList_banlist.items = banlistNames;
                UIPopupList_banlist.value = UIPopupList_banlist.items[0];
                currentBanlist = BanlistManager.GetByName(UIPopupList_banlist.items[0]);
                UIHelper.registEvent(toolBar, "rand_", rand);
                UIHelper.registEvent(toolBar, "sort_", sort);
                UIHelper.registEvent(toolBar, "clear_", clear);
                UIHelper.registEvent(toolBar, "home_", home);
                UIHelper.registEvent(toolBar, "save_", () => { onSave(); });
                UIHelper.registEvent(toolBar, "lfList_", onLF);
                UIHelper.registEvent(toolBar, "copy_", onCopy);
                break;
            case Condition.changeSide:
                UIHelper.setParent(gameObjectSearch, Program.I().ui_main_2d);
                CreateBar(Program.I().new_bar_changeSide, 0, 230);
                UIPopupList_banlist = null;
                UIHelper.registEvent(toolBar, "rand_", rand);
                UIHelper.registEvent(toolBar, "sort_", sort);
                UIHelper.registEvent(toolBar, "finish_", home);
                UIHelper.registEvent(toolBar, "input_", onChat);
                break;
        }
    }

    private void onCopy()
    {
        var deckName = Config.Get("deckInUse", "miaowu");
        var newname = InterString.Get("[?]的副本", deckName);
        var newnamer = newname;
        var i = 1;
        while (File.Exists("deck/" + newnamer + ".ydk"))
        {
            newnamer = newname + i;
            i++;
        }

        RMSshow_input("onRename", InterString.Get("新的卡组名"), newnamer);
    }

    public override void ES_RMS(string hashCode, List<messageSystemValue> result)
    {
        base.ES_RMS(hashCode, result);
        if (hashCode == "onRename")
        {
            var raw = Config.Get("deckInUse", "miaowu");
            Config.Set("deckInUse", result[0].value);
            if (onSave())
                Program.I().cardDescription.setTitle(result[0].value);
            else
                Config.Set("deckInUse", raw);
        }
    }

    public Action returnAction = null;

    public bool onSave()
    {
        try
        {
            if (
                deck.IMain.Count <= 60
                &&
                deck.IExtra.Count <= 15
                &&
                deck.ISide.Count <= 15
            )
            {
                var deckInUse = Config.Get("deckInUse", "miaowu");
                if (canSave)
                {
                    ArrangeObjectDeck();
                    FromObjectDeckToCodedDeck(true);
                    var value = "#created by ygopro2\r\n#main\r\n";
                    for (var i = 0; i < deck.Main.Count; i++) value += deck.Main[i] + "\r\n";
                    value += "#extra\r\n";
                    for (var i = 0; i < deck.Extra.Count; i++) value += deck.Extra[i] + "\r\n";
                    value += "!side\r\n";
                    for (var i = 0; i < deck.Side.Count; i++) value += deck.Side[i] + "\r\n";
                    File.WriteAllText("deck/" + deckInUse + ".ydk", value, Encoding.UTF8);
                }
                else
                {
                    var value = "#created by ygopro2\r\n#main\r\n";
                    for (var i = 0; i < deck.Deck_O.Main.Count; i++) value += deck.Deck_O.Main[i] + "\r\n";
                    value += "#extra\r\n";
                    for (var i = 0; i < deck.Deck_O.Extra.Count; i++) value += deck.Deck_O.Extra[i] + "\r\n";
                    value += "!side\r\n";
                    for (var i = 0; i < deck.Deck_O.Side.Count; i++) value += deck.Deck_O.Side[i] + "\r\n";
                    File.WriteAllText("deck/" + deckInUse + ".ydk", value, Encoding.UTF8);
                }

                deckDirty = false;
                RMSshow_none(InterString.Get("卡组[?]已经被保存。", deckInUse));
                return true;
            }

            RMSshow_none(InterString.Get("卡组内卡片张数超过限制。"));
            return false;
        }
        catch (Exception)
        {
            RMSshow_none(InterString.Get("保存失败！"));
            return false;
        }
    }

    public void onChat()
    {
        Program.I().room.onSubmit(UIHelper.getByName<UIInput>(toolBar, "input_").value);
        UIHelper.getByName<UIInput>(toolBar, "input_").value = "";
    }

    private void home()
    {
        if (returnAction != null) returnAction();
    }

    private void sort()
    {
        //animationCameraPan();
        ArrangeObjectDeck();
        SortObjectDeck();
        ShowObjectDeck();
    }

    private void rand()
    {
        //animationCameraPan();
        ArrangeObjectDeck();
        RandObjectDeck();
        ShowObjectDeck();
    }

    private void clear()
    {
        var deckTemp = deck.getAllObjectCard();
        foreach (var item in deckTemp)
            try
            {
                UIHelper.clearITWeen(item.gameObject);
                var rid = item.gameObject.GetComponent<Rigidbody>();
                if (rid == null) rid = item.gameObject.AddComponent<Rigidbody>();
                rid.AddForce(0.7f *
                             (item.transform.position +
                              new Vector3(0, 30 - Vector3.Distance(item.transform.position, Vector3.zero), 0)) /
                             Program.deltaTime);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

        deckDirty = true;
    }

    private bool detailShowed;

    private void showDetail()
    {
        detailShowed = true;
        refreshDetail();
    }

    private void hideDetail()
    {
        clearAll();
        detailShowed = false;
        refreshDetail();
    }


    private bool detailPanelShiftedTemp;

    private void shiftDetailPanel(bool dragged)
    {
        detailPanelShiftedTemp = dragged;
        if (isShowed && detailShowed)
        {
            if (dragged)
                gameObjectDetailedSearch.GetComponent<UITexture>().color = new Color(1, 1, 1, 0.7f);
            else
                gameObjectDetailedSearch.GetComponent<UITexture>().color = Color.white;
        }
    }


    private void refreshDetail()
    {
        if (gameObjectDetailedSearch != null)
        {
            if (isShowed)
            {
                // if (Screen.height < 700)
                // {
                //     gameObjectDetailedSearch.transform.localScale = new Vector3(Screen.height / 700f,
                //         Screen.height / 700f, Screen.height / 700f);
                //     if (detailShowed)
                //     {
                //         gameObjectDetailedSearch.GetComponent<UITexture>().height = 700;
                //         iTween.MoveTo(gameObjectDetailedSearch,
                //             Program.I().camera_main_2d.ScreenToWorldPoint(
                //                 new Vector3(Screen.width - 230 - 115f * Screen.height / 700f, Screen.height * 0.5f, 0)),
                //             0.6f);
                //         reShowBar(0, 230 + 230 * Screen.height / 700f);
                //     }
                //     else
                //     {
                //         gameObjectDetailedSearch.GetComponent<UITexture>().height = 700;
                //         iTween.MoveTo(gameObjectDetailedSearch,
                //             Program.I().camera_main_2d.ScreenToWorldPoint(
                //                 new Vector3(Screen.width - 230 - 115f * Screen.height / 700f, Screen.height * 1.5f, 0)),
                //             0.6f);
                //         reShowBar(0, 230);
                //     }
                // }
                // else
                // {
                gameObjectDetailedSearch.transform.localScale = Vector3.one;
                if (detailShowed)
                {
                    gameObjectDetailedSearch.GetComponent<UITexture>().height = Utils.UIHeight();
                    gameObjectDetailedSearch.transform.DOMove(
                        Utils.UIToWorldPoint(new Vector3(Utils.UIWidth() - 345f,
                            Utils.UIHeight() * 0.5f, 0)), 0.6f);
                    reShowBar(0, 460);
                }
                else
                {
                    gameObjectDetailedSearch.GetComponent<UITexture>().height = Utils.UIHeight();
                    gameObjectDetailedSearch.transform.DOMove(
                        Utils.UIToWorldPoint(new Vector3(Utils.UIWidth() - 345f,
                            Utils.UIHeight() * 1.5f, 0)), 0.6f);
                    reShowBar(0, 230);
                }
                // }
            }
            else
            {
                gameObjectDetailedSearch.transform.localScale = Vector3.zero;
            }
        }
    }

    private void onClickDetail()
    {
        if (detailShowed)
            hideDetail();
        else
            showDetail();
    }

    public override void ES_mouseDownEmpty()
    {
        //if (detailShowed)
        //{
        //    hideDetail();
        //}
    }

    private void onExitDetail()
    {
        if (detailShowed) hideDetail();
    }

    private void clearAll()
    {
        try
        {
            seconds.Clear();
            for (var i = 0; i < 32; i++) UIToggle_effects[i].value = false;
            UIPopupList_pack.value = GameStringManager.get_unsafe(1310);
            UIPopupList_main.value = GameStringManager.get_unsafe(1310);
            UIPopupList_ban.value = GameStringManager.get_unsafe(1310);
            UIPopupList_second.Clear();
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1310));
            UIPopupList_second.value = GameStringManager.get_unsafe(1310);
            UIPopupList_race.Clear();
            UIPopupList_race.AddItem(GameStringManager.get_unsafe(1310));
            UIPopupList_race.value = GameStringManager.get_unsafe(1310);
            UIPopupList_attribute.Clear();
            UIPopupList_attribute.AddItem(GameStringManager.get_unsafe(1310));
            UIPopupList_attribute.value = GameStringManager.get_unsafe(1310);
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "stars_").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "p_").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "atk_").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "def_").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "year_").value = "";

            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "stars_UP").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "p_UP").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "atk_UP").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "def_UP").value = "";
            UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "year_UP").value = "";
        }
        catch (Exception e)
        {
            //UnityEngine.Debug.Log(e);
        }
    }

    private readonly List<string> seconds = new List<string>();

    private void onUIPopupList_second()
    {
        Program.notGo(printSecond);
        Program.go(100, printSecond);
    }

    private void printSecond()
    {
        Program.go(50, tempStep2);
        if (UIPopupList_main.value == GameStringManager.get_unsafe(1312))
        {
            if (UIPopupList_second.value == GameStringManager.get_unsafe(1310))
            {
                seconds.Clear();
            }
            else
            {
                seconds.Remove(UIPopupList_second.value);
                seconds.Add(UIPopupList_second.value);
                var all = "";
                foreach (var item in seconds) all += item + " ";
                if (all == "") all = GameStringManager.get_unsafe(1310);
                UIPopupList_second.value = all;
            }
        }
        else
        {
            seconds.Clear();
            seconds.Add(UIPopupList_second.value);
        }
    }

    private void tempStep2()
    {
        Program.notGo(printSecond);
    }

    private void onUIPopupList_main()
    {
        UIPopupList_second.Clear();
        UIPopupList_second.AddItem(GameStringManager.get_unsafe(1310));
        UIPopupList_second.value = GameStringManager.get_unsafe(1310);
        UIPopupList_race.Clear();
        UIPopupList_race.AddItem(GameStringManager.get_unsafe(1310));
        UIPopupList_race.value = GameStringManager.get_unsafe(1310);
        UIPopupList_attribute.Clear();
        UIPopupList_attribute.AddItem(GameStringManager.get_unsafe(1310));
        UIPopupList_attribute.value = GameStringManager.get_unsafe(1310);
        if (UIPopupList_main.value == GameStringManager.get_unsafe(1312))
        {
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1054));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1055));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1056));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1057));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1063));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1073));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1062));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1061));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1060));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1059));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1071));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1072));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1074));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1075));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1076));
            for (var i = 1020; i <= 1044; i++) UIPopupList_race.AddItem(GameStringManager.get_unsafe(i));
            for (var i = 1010; i <= 1016; i++) UIPopupList_attribute.AddItem(GameStringManager.get_unsafe(i));
        }

        if (UIPopupList_main.value == GameStringManager.get_unsafe(1313))
        {
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1054));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1066));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1067));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1057));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1068));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1069));
        }

        if (UIPopupList_main.value == GameStringManager.get_unsafe(1314))
        {
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1054));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1067));
            UIPopupList_second.AddItem(GameStringManager.get_unsafe(1070));
        }
    }

    private void onClickSearch()
    {
        doSearch();
    }

    private int lastRefreshTime;

    private string UIInput_searchValueLast = "";

    private void doSearch()
    {
        superScrollView.toTop();
        Program.go(50, process);
    }

    private void process()
    {
        var result = CardsManager.searchAdvanced
        (
            getName(),
            getLevel(),
            getAttack(),
            getDefence(),
            getP(),
            getYear(),
            getLevel_UP(),
            getAttack_UP(),
            getDefence_UP(),
            getP_UP(),
            getYear_UP(),
            getOT(),
            getPack(),
            getBanFilter(),
            currentBanlist,
            getTypeFilter(),
            getTypeFilter2(),
            getRaceFilter(),
            getAttributeFilter(),
            getCatagoryFilter()
        );
        print(result);
        UIHelper.trySetLableText(gameObjectSearch, "title_", result.Count.ToString());
        UIInput_search.isSelected = true;
    }

    public Banlist currentBanlist;

    private bool checkBanlistAvail(int cardid)
    {
        return deck.GetCardCount(cardid) < currentBanlist.GetQuantity(cardid);
    }

    private bool isBanned(int cardid)
    {
        return currentBanlist.GetQuantity(cardid) == 0;
    }

    private List<Card> PrintedResult = new List<Card>();

    private void print(List<Card> result)
    {
        if (superScrollView != null)
        {
            PrintedResult = result;
            if (condition == Condition.editDeck) currentBanlist = BanlistManager.GetByName(UIPopupList_banlist.value);
            if (condition == Condition.changeSide) currentBanlist = BanlistManager.GetByHash(Program.I().room.lflist);
            var args = new List<string[]>();
            foreach (var item in result)
            {
                var arg = new string[5];
                arg[0] = item.Id.ToString();
                arg[1] = "3";
                arg[2] = item.Name + "\n" + GameStringHelper.getSearchResult(item);
                args.Add(arg);
            }

            superScrollView.print(args);
            superScrollView.toTop();
        }
    }

    private bool ifType(string str)
    {
        var re = false;
        foreach (var item in seconds)
            if (str == item)
            {
                re = true;
                break;
            }

        return re;
    }

    private uint getTypeFilter()
    {
        uint returnValue = 0;
        if (UIPopupList_main.value == GameStringManager.get_unsafe(1312)) returnValue = (uint) CardType.Monster;
        if (UIPopupList_main.value == GameStringManager.get_unsafe(1313)) returnValue = (uint) CardType.Spell;
        if (UIPopupList_main.value == GameStringManager.get_unsafe(1314)) returnValue = (uint) CardType.Trap;
        return returnValue;
    }

    private uint getTypeFilter2()
    {
        uint returnValue = 0;
        if (UIPopupList_main.value == GameStringManager.get_unsafe(1312))
        {
            if (ifType(GameStringManager.get_unsafe(1054)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Normal;
            if (ifType(GameStringManager.get_unsafe(1055)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Effect;
            if (ifType(GameStringManager.get_unsafe(1056)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Fusion;
            if (ifType(GameStringManager.get_unsafe(1057)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Ritual;
            if (ifType(GameStringManager.get_unsafe(1063)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Synchro;
            if (ifType(GameStringManager.get_unsafe(1073)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Xyz;
            if (ifType(GameStringManager.get_unsafe(1062)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Tuner;
            if (ifType(GameStringManager.get_unsafe(1061)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Dual;
            if (ifType(GameStringManager.get_unsafe(1060)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Union;
            if (ifType(GameStringManager.get_unsafe(1059)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Spirit;
            if (ifType(GameStringManager.get_unsafe(1071)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Flip;
            if (ifType(GameStringManager.get_unsafe(1072)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Toon;
            if (ifType(GameStringManager.get_unsafe(1074)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Pendulum;
            if (ifType(GameStringManager.get_unsafe(1075)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.SpSummon;
            if (ifType(GameStringManager.get_unsafe(1076)))
                returnValue |= (uint) CardType.Monster + (uint) CardType.Link;
        }

        if (UIPopupList_main.value == GameStringManager.get_unsafe(1313))
        {
            if (ifType(GameStringManager.get_unsafe(1054))) returnValue |= (uint) CardType.Spell;
            if (ifType(GameStringManager.get_unsafe(1066)))
                returnValue |= (uint) CardType.Spell + (uint) CardType.QuickPlay;
            if (ifType(GameStringManager.get_unsafe(1067)))
                returnValue |= (uint) CardType.Spell + (uint) CardType.Continuous;
            if (ifType(GameStringManager.get_unsafe(1057)))
                returnValue |= (uint) CardType.Spell + (uint) CardType.Ritual;
            if (ifType(GameStringManager.get_unsafe(1068)))
                returnValue |= (uint) CardType.Spell + (uint) CardType.Equip;
            if (ifType(GameStringManager.get_unsafe(1069)))
                returnValue |= (uint) CardType.Spell + (uint) CardType.Field;
        }

        if (UIPopupList_main.value == GameStringManager.get_unsafe(1314))
        {
            if (ifType(GameStringManager.get_unsafe(1054))) returnValue |= (uint) CardType.Trap;
            if (ifType(GameStringManager.get_unsafe(1067)))
                returnValue |= (uint) CardType.Trap + (uint) CardType.Continuous;
            if (ifType(GameStringManager.get_unsafe(1070)))
                returnValue |= (uint) CardType.Trap + (uint) CardType.Counter;
        }

        return returnValue;
    }

    private int getBanFilter()
    {
        var returnValue = -233;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1316)) returnValue = 0;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1317)) returnValue = 1;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1318)) returnValue = 2;
        return returnValue;
    }

    private int getOT()
    {
        var returnValue = -233;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1481)) returnValue = 1;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1482)) returnValue = 2;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1483)) returnValue = 8;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1484)) returnValue = 4;
        if (UIPopupList_ban.value == GameStringManager.get_unsafe(1485)) returnValue = 3;
        return returnValue;
    }

    private uint getRaceFilter()
    {
        uint returnValue = 0;
        for (var i = 0; i < 25; i++)
            if (UIPopupList_race.value == GameStringManager.get_unsafe(1020 + i))
                returnValue |= (uint) Math.Pow(2, i);
        return returnValue;
    }

    private uint getAttributeFilter()
    {
        uint returnValue = 0;
        for (var i = 0; i < 7; i++)
            if (UIPopupList_attribute.value == GameStringManager.get_unsafe(1010 + i))
                returnValue |= (uint) Math.Pow(2, i);
        return returnValue;
    }

    private uint getCatagoryFilter()
    {
        uint returnValue = 0;
        for (var i = 0; i < 32; i++)
            if (UIToggle_effects[i].value)
                returnValue |= (uint) Math.Pow(2, i);
        return returnValue;
    }

    private int getAttack()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "atk_").value);
        }
        catch (Exception)
        {
            returnValue = -2;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "atk_").value == "") returnValue = -233;
        return returnValue;
    }

    private int getDefence()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "def_").value);
        }
        catch (Exception)
        {
            returnValue = -2;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "def_").value == "") returnValue = -233;
        return returnValue;
    }

    private int getLevel()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "stars_").value);
        }
        catch (Exception)
        {
            returnValue = 0;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "stars_").value == "") returnValue = -233;
        return returnValue;
    }

    private int getP()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "p_").value);
        }
        catch (Exception)
        {
            returnValue = 0;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "p_").value == "") returnValue = -233;
        return returnValue;
    }

    private int getYear()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "year_").value);
        }
        catch (Exception)
        {
            returnValue = 0;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "year_").value == "") returnValue = -233;
        return returnValue;
    }

    private int getAttack_UP()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "atk_UP").value);
        }
        catch (Exception)
        {
            returnValue = -2;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "atk_UP").value == "") returnValue = -233;
        return returnValue;
    }

    private int getDefence_UP()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "def_UP").value);
        }
        catch (Exception)
        {
            returnValue = -2;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "def_UP").value == "") returnValue = -233;
        return returnValue;
    }

    private int getLevel_UP()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "stars_UP").value);
        }
        catch (Exception)
        {
            returnValue = 0;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "stars_UP").value == "") returnValue = -233;
        return returnValue;
    }

    private int getP_UP()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "p_UP").value);
        }
        catch (Exception)
        {
            returnValue = 0;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "p_UP").value == "") returnValue = -233;
        return returnValue;
    }

    private int getYear_UP()
    {
        var returnValue = 0;
        try
        {
            returnValue = int.Parse(UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "year_UP").value);
        }
        catch (Exception)
        {
            returnValue = 0;
        }

        if (UIHelper.getByName<UIInput>(gameObjectDetailedSearch, "year_UP").value == "") returnValue = -233;
        return returnValue;
    }

    private string getName()
    {
        return UIInput_search.value;
    }

    private string getPack()
    {
        if (UIPopupList_pack.value == GameStringManager.get_unsafe(1310)) return "";
        return UIPopupList_pack.value;
    }

    #endregion
}