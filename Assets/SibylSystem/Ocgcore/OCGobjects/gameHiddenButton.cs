using System;
using UnityEngine;
using YGOSharp;
using YGOSharp.OCGWrapper.Enums;

public class gameHiddenButton : OCGobject
{
    private bool excited;

    public TextMaster hintText;
    public CardLocation location;

    public int player;

    private readonly GPS ps;

    public gameHiddenButton(CardLocation l, int p)
    {
        ps = new GPS();
        ps.controller = (uint) p;
        ps.location = (uint) l;
        ps.position = 0;
        ps.sequence = 0;
        Program.I().ocgcore.AddUpdateAction_s(Update);
        player = p;
        location = l;
        gameObject = create(Program.I().mod_ocgcore_hidden_button);
    }

    public void dispose()
    {
        Program.I().ocgcore.RemoveUpdateAction_s(Update);
    }

    public void Update()
    {
        if (gameObject != null)
        {
            gameObject.transform.position = Program.I().ocgcore.get_point_worldposition(ps);
            if (Program.pointedGameObject == gameObject)
            {
                if (excited == false)
                    excite();
                if (Program.InputGetMouseButtonUp_0) showAll();
            }
            else
            {
                if (excited)
                {
                    excited = false;
                    calm();
                }
            }
        }
    }

    private void showAll()
    {
        if (location == CardLocation.Grave && Program.I().ocgcore.cantCheckGrave)
        {
            Program.I().cardDescription.RMSshow_none(InterString.Get("不能确认墓地里的卡"));
            return;
        }

        var allShow = true;
        for (var i = 0; i < Program.I().ocgcore.cards.Count; i++)
            if (Program.I().ocgcore.cards[i].gameObject.activeInHierarchy)
                if ((Program.I().ocgcore.cards[i].p.location & (uint) location) > 0)
                    if (Program.I().ocgcore.cards[i].p.controller == player)
                        if (Program.I().ocgcore.cards[i].isShowed == false)
                            if (Program.I().ocgcore.cards[i].prefered)
                                allShow = false;
        for (var i = 0; i < Program.I().ocgcore.cards.Count; i++)
            if (Program.I().ocgcore.cards[i].gameObject.activeInHierarchy)
                if ((Program.I().ocgcore.cards[i].p.location & (uint) location) > 0)
                    if (Program.I().ocgcore.cards[i].p.controller == player)
                    {
                        if (allShow)
                        {
                            Program.I().ocgcore.cards[i].isShowed = true;
                        }
                        else
                        {
                            if (Program.I().ocgcore.cards[i].prefered) Program.I().ocgcore.cards[i].isShowed = true;
                        }
                    }

        Program.I().ocgcore.realize();
        Program.I().ocgcore.toNearest();
        Program.I().audio.clip = Program.I().zhankai;
        Program.I().audio.Play();
    }

    private void calm()
    {
        if (Program.I().ocgcore.condition == Ocgcore.Condition.duel && Program.I().ocgcore.InAI == false &&
            Program.I().room.mode != 2)
            if (player == 0)
                if (location == CardLocation.Deck)
                {
                    if (Program.I().book.lab != null)
                    {
                        destroy(Program.I().book.lab.gameObject);
                        Program.I().book.lab = null;
                    }

                    return;
                }

        if (player == 1)
            if (location == CardLocation.Deck)
            {
                if (Program.I().book.labop != null)
                {
                    destroy(Program.I().book.labop.gameObject);
                    Program.I().book.labop = null;
                }

                return;
            }

        for (var i = 0; i < Program.I().ocgcore.cards.Count; i++)
            if (Program.I().ocgcore.cards[i].gameObject.activeInHierarchy)
                if ((Program.I().ocgcore.cards[i].p.location & (uint) location) > 0)
                    if (Program.I().ocgcore.cards[i].p.controller == player &&
                        Program.I().ocgcore.cards[i].isShowed == false)
                        Program.I().ocgcore.cards[i].ES_safe_card_move_to_original_place();
        if (hintText != null)
        {
            hintText.dispose();
            hintText = null;
        }
    }

    private void excite()
    {
        excited = true;
        if (location == CardLocation.Grave && Program.I().ocgcore.cantCheckGrave) return;
        Card data = null;
        var tailString = "";
        uint con = 0;
        for (var i = 0; i < Program.I().ocgcore.cards.Count; i++)
            if (Program.I().ocgcore.cards[i].gameObject.activeInHierarchy)
                if ((Program.I().ocgcore.cards[i].p.location & (uint) location) > 0)
                    if (Program.I().ocgcore.cards[i].p.controller == player)
                        if (Program.I().ocgcore.cards[i].isShowed == false)
                        {
                            data = Program.I().ocgcore.cards[i].get_data();
                            tailString = Program.I().ocgcore.cards[i].tails.managedString;
                            con = Program.I().ocgcore.cards[i].p.controller;
                        }

        Program.I().cardDescription.setData(data, con == 0 ? GameTextureManager.myBack : GameTextureManager.opBack,
            tailString, data != null);
        if (Program.I().ocgcore.condition == Ocgcore.Condition.duel && Program.I().ocgcore.InAI == false &&
            Program.I().room.mode != 2)
            if (player == 0)
                if (location == CardLocation.Deck)
                {
                    if (Program.I().book.lab != null)
                    {
                        destroy(Program.I().book.lab.gameObject);
                        Program.I().book.lab = null;
                    }


                    Program.I().book.lab =
                        create(Program.I().New_decker, Vector3.zero, Vector3.zero, false, Program.I().ui_main_2d)
                            .GetComponent<UILabel>();
                    Program.I().book.realize();


                    var screenPosition = Input.mousePosition;
                    screenPosition.x -= 90;
                    screenPosition.y += Program.I().book.lab.height / 4;
                    screenPosition.z = 0;
                    var worldPositin = Program.I().camera_main_2d.ScreenToWorldPoint(screenPosition);
                    Program.I().book.lab.transform.position = worldPositin;

                    return;
                }


        if (player == 1)
            if (location == CardLocation.Deck)
            {
                if (Program.I().book.labop != null)
                {
                    destroy(Program.I().book.labop.gameObject);
                    Program.I().book.labop = null;
                }


                Program.I().book.labop =
                    create(Program.I().New_decker, Vector3.zero, Vector3.zero, false, Program.I().ui_main_2d)
                        .GetComponent<UILabel>();
                Program.I().book.realize();


                var screenPosition = Input.mousePosition;
                screenPosition.x -= 90;
                screenPosition.y -= Program.I().book.labop.height / 4;
                screenPosition.z = 0;
                var worldPositin = Program.I().camera_main_2d.ScreenToWorldPoint(screenPosition);
                Program.I().book.labop.transform.position = worldPositin;

                return;
            }

        var count = 0;
        for (var i = 0; i < Program.I().ocgcore.cards.Count; i++)
            if (Program.I().ocgcore.cards[i].gameObject.activeInHierarchy)
                if ((Program.I().ocgcore.cards[i].p.location & (uint) location) > 0)
                    if (Program.I().ocgcore.cards[i].p.controller == player)
                        count++;
        var count_show = 0;
        for (var i = 0; i < Program.I().ocgcore.cards.Count; i++)
            if (Program.I().ocgcore.cards[i].gameObject.activeInHierarchy)
                if ((Program.I().ocgcore.cards[i].p.location & (uint) location) > 0)
                    if (Program.I().ocgcore.cards[i].p.controller == player &&
                        Program.I().ocgcore.cards[i].isShowed == false)
                        count_show++;
        if (hintText != null)
        {
            hintText.dispose();
            hintText = null;
        }

        if (count > 0) hintText = new TextMaster(count.ToString(), Input.mousePosition, false);
        var qidian = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 80);
        var zhongdian = new Vector3(2f * Program.I().ocgcore.getScreenCenter() - Input.mousePosition.x,
            Input.mousePosition.y, 50f);
        var i_real = 0;
        for (var i = 0; i < Program.I().ocgcore.cards.Count; i++)
            if (Program.I().ocgcore.cards[i].gameObject.activeInHierarchy)
                if ((Program.I().ocgcore.cards[i].p.location & (uint) location) > 0)
                    if (Program.I().ocgcore.cards[i].p.controller == player)
                        if (Program.I().ocgcore.cards[i].isShowed == false)
                        {
                            var screen_vector_to_move = Vector3.zero;
                            var gezi = 8;
                            if (count_show > 8) gezi = count_show;
                            var index = count_show - 1 - i_real;
                            i_real++;
                            screen_vector_to_move =
                                new Vector3(0, 50f * (float)Math.Sin(index / (float)count * 3.1415926f), 0)
                                +
                                qidian
                                +
                                index / (float)(gezi - 1) * (zhongdian - qidian);
                            //iTween.MoveTo(Program.I().ocgcore.cards[i].gameObject, Camera.main.ScreenToWorldPoint(screen_vector_to_move), 0.5f);
                            //iTween.RotateTo(Program.I().ocgcore.cards[i].gameObject, new Vector3(-30, 0, 0), 0.1f);
                            Program.I().ocgcore.cards[i].TweenTo(Camera.main.ScreenToWorldPoint(screen_vector_to_move),
                                new Vector3(-20, 0, 0), true);// mark 卡组额外等确认时浮起
                        }

        if (count_show > 0)
        {
            Program.I().audio.clip = Program.I().zhankai;
            Program.I().audio.Play();
        }
    }
}