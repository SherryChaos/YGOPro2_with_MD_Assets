using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ionic.Zip;
using UnityEngine;
using YGOSharp;

public class Program : MonoBehaviour
{
    public static void gugugu()
    {
        PrintToChat(InterString.Get("非常抱歉，因为技术原因，此功能暂时无法使用。请关注官方网站获取更多消息。"));
    }

    #region Resources

    [Header("场景")]
    public Camera main_camera;
    public Light light;
    public AudioSource audio;

    [Header("容器")] public GameObject ui_back_ground_2d;
    public GameObject ui_windows_2d;
    public GameObject ui_main_2d;
    public GameObject ui_container_3d;
    public Camera camera_container_3d;
    public GameObject ui_main_3d;
    public Camera camera_main_3d;

    [Header("ui_back_ground_2d")] public Camera camera_back_ground_2d;
    public GameObject mod_simple_ngui_background_texture;
    public GameObject new_ui_cardDescription;
    public GameObject new_ui_search;
    public gameInfo new_ui_gameInfo;

    [Header("ui_windows_2d")] public Camera camera_windows_2d;
    public GameObject new_ui_menu;
    public GameObject remaster_deckManager;
    public GameObject new_ui_selectServer;
    public GameObject remaster_replayManager;
    public GameObject remaster_puzzleManager;
    public GameObject new_ui_aiRoom;

    [Header("ui_main_2d")] public Camera camera_main_2d;
    public GameObject new_ui_setting;
    public GameObject new_bar_room;
    public GameObject new_ui_searchDetailed;
    public GameObject new_ui_book;

    [Header("其他")] public GameObject mouseParticle;

    [Header("Prefab")] public facer face;
    public AudioClip zhankai;
    public GameObject mod_winExplode;
    public GameObject mod_loseExplode;
    public GameObject mod_audio_effect;
    public GameObject mod_ocgcore_card;
    public GameObject mod_ocgcore_card_cloude;
    public GameObject mod_ocgcore_card_number_shower;
    public GameObject mod_ocgcore_card_figure_line;
    public GameObject mod_ocgcore_hidden_button;
    public GameObject mod_ocgcore_coin;
    public GameObject mod_ocgcore_dice;
    public GameObject mod_simple_quad;
    public GameObject mod_simple_quad_star;
    public GameObject mod_simple_ngui_text;
    public GameObject mod_ocgcore_number;
    public GameObject mod_ocgcore_decoration_chain_selecting;
    public GameObject mod_ocgcore_decoration_card_selected;
    public GameObject mod_ocgcore_decoration_card_selecting;
    public GameObject mod_ocgcore_decoration_card_active;
    public GameObject mod_ocgcore_decoration_spsummon;
    public GameObject mod_ocgcore_decoration_thunder;
    public GameObject mod_ocgcore_decoration_trap_activated;
    public GameObject mod_ocgcore_decoration_magic_activated;
    public GameObject mod_ocgcore_decoration_magic_zhuangbei;
    public GameObject mod_ocgcore_decoration_removed;
    public GameObject mod_ocgcore_decoration_tograve;
    public GameObject mod_ocgcore_decoration_card_setted;
    public GameObject mod_ocgcore_blood;
    public GameObject mod_ocgcore_blood_screen;
    public GameObject mod_ocgcore_bs_atk_decoration;
    public GameObject mod_ocgcore_bs_atk_line_earth;
    public GameObject mod_ocgcore_bs_atk_line_water;
    public GameObject mod_ocgcore_bs_atk_line_fire;
    public GameObject mod_ocgcore_bs_atk_line_wind;
    public GameObject mod_ocgcore_bs_atk_line_dark;
    public GameObject mod_ocgcore_bs_atk_line_light;
    public GameObject mod_ocgcore_cs_chaining;
    public GameObject mod_ocgcore_cs_end;
    public GameObject mod_ocgcore_cs_bomb;
    public GameObject mod_ocgcore_cs_negated;
    public GameObject mod_ocgcore_cs_mon_earth;
    public GameObject mod_ocgcore_cs_mon_water;
    public GameObject mod_ocgcore_cs_mon_fire;
    public GameObject mod_ocgcore_cs_mon_wind;
    public GameObject mod_ocgcore_cs_mon_light;
    public GameObject mod_ocgcore_cs_mon_dark;
    public GameObject mod_ocgcore_ss_summon_earth;
    public GameObject mod_ocgcore_ss_summon_water;
    public GameObject mod_ocgcore_ss_summon_fire;
    public GameObject mod_ocgcore_ss_summon_wind;
    public GameObject mod_ocgcore_ss_summon_dark;
    public GameObject mod_ocgcore_ss_summon_light;
    public GameObject mod_ocgcore_ol_earth;
    public GameObject mod_ocgcore_ol_water;
    public GameObject mod_ocgcore_ol_fire;
    public GameObject mod_ocgcore_ol_wind;
    public GameObject mod_ocgcore_ol_dark;
    public GameObject mod_ocgcore_ol_light;
    public GameObject mod_ocgcore_ss_spsummon_normal;
    public GameObject mod_ocgcore_ss_spsummon_ronghe;
    public GameObject mod_ocgcore_ss_spsummon_tongtiao;
    public GameObject mod_ocgcore_ss_spsummon_yishi;
    public GameObject mod_ocgcore_ss_spsummon_link;
    public GameObject mod_ocgcore_ss_p_idle_effect;
    public GameObject mod_ocgcore_ss_p_sum_effect;
    public GameObject mod_ocgcore_ss_dark_hole;
    public GameObject mod_ocgcore_ss_link_mark;

    public GameObject new_ui_cardOnSearchList;
    public GameObject new_bar_editDeck;
    public GameObject new_bar_changeSide;
    public GameObject new_bar_duel;
    public GameObject new_bar_watchDuel;
    public GameObject new_bar_watchRecord;
    public GameObject new_mod_cardInDeckManager;
    public GameObject new_mod_tableInDeckManager;
    public GameObject new_ui_handShower;
    public GameObject new_ui_textMesh;
    public GameObject new_ui_superButton;
    public GameObject new_ui_superButtonTransparent;
    public GameObject new_ocgcore_field;
    public GameObject new_ocgcore_chainCircle;
    public GameObject new_ocgcore_wait;
    public GameObject remaster_tagRoom;
    public GameObject remaster_room;
    public GameObject ES_1;
    public GameObject ES_2;
    public GameObject ES_2Force;
    public GameObject ES_3cancle;
    public GameObject ES_Single_multiple_window;
    public GameObject ES_Single_option;
    public GameObject ES_multiple_option;
    public GameObject ES_input;
    public GameObject ES_position;
    public GameObject ES_position3;
    public GameObject ES_Tp;
    public GameObject ES_Face;
    public GameObject ES_FS;
    public GameObject Pro1_CardShower;
    public GameObject Pro1_superCardShower;
    public GameObject Pro1_superCardShowerA;
    public GameObject New_arrow;
    public GameObject New_selectKuang;
    public GameObject New_chainKuang;
    public GameObject New_phase;
    public GameObject New_turn_me;
    public GameObject New_turn_op;
    public GameObject New_decker;
    public GameObject New_winCaculator;
    public GameObject New_winCaculatorRecord;
    public GameObject New_ocgcore_placeSelector;

    public Sprite bp_b;
    public Sprite bp_r;
    public Sprite dp_b;
    public Sprite dp_r;
    public Sprite ep_b;
    public Sprite ep_r;
    public Sprite mp1_b;
    public Sprite mp1_r;
    public Sprite mp2_b;
    public Sprite mp2_r;
    public Sprite tc_b;
    public Sprite tc_r;
    public Sprite sp_b;
    public Sprite sp_r;
    public Sprite rs;
    public Sprite ts;

    #endregion

    #region Initializement

    private static Program instance;

    public static Program I()
    {
        return instance;
    }

    public static int TimePassed()
    {
        return (int) (Time.time * 1000f);
    }

    private readonly List<GameObject> allObjects = new List<GameObject>();

    private void loadResource(GameObject g)
    {
        try
        {
            var obj = Instantiate(g);
            obj.SetActive(false);
            allObjects.Add(obj);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void loadResources()
    {
        loadResource(mod_audio_effect);
        loadResource(mod_ocgcore_card);
        loadResource(mod_ocgcore_card_cloude);
        loadResource(mod_ocgcore_card_number_shower);
        loadResource(mod_ocgcore_card_figure_line);
        loadResource(mod_ocgcore_hidden_button);
        loadResource(mod_ocgcore_coin);
        loadResource(mod_ocgcore_dice);

        loadResource(mod_ocgcore_decoration_chain_selecting);
        loadResource(mod_ocgcore_decoration_card_selected);
        loadResource(mod_ocgcore_decoration_card_selecting);
        loadResource(mod_ocgcore_decoration_card_active);
        loadResource(mod_ocgcore_decoration_spsummon);
        loadResource(mod_ocgcore_decoration_thunder);
        loadResource(mod_ocgcore_cs_mon_earth);
        loadResource(mod_ocgcore_cs_mon_water);
        loadResource(mod_ocgcore_cs_mon_fire);
        loadResource(mod_ocgcore_cs_mon_wind);
        loadResource(mod_ocgcore_cs_mon_light);
        loadResource(mod_ocgcore_cs_mon_dark);
        loadResource(mod_ocgcore_decoration_trap_activated);
        loadResource(mod_ocgcore_decoration_magic_activated);
        loadResource(mod_ocgcore_decoration_magic_zhuangbei);

        loadResource(mod_ocgcore_decoration_removed);
        loadResource(mod_ocgcore_decoration_tograve);
        loadResource(mod_ocgcore_decoration_card_setted);
        loadResource(mod_ocgcore_blood);
        loadResource(mod_ocgcore_blood_screen);


        loadResource(mod_ocgcore_bs_atk_decoration);
        loadResource(mod_ocgcore_bs_atk_line_earth);
        loadResource(mod_ocgcore_bs_atk_line_water);
        loadResource(mod_ocgcore_bs_atk_line_fire);
        loadResource(mod_ocgcore_bs_atk_line_wind);
        loadResource(mod_ocgcore_bs_atk_line_dark);
        loadResource(mod_ocgcore_bs_atk_line_light);

        loadResource(mod_ocgcore_cs_chaining);
        loadResource(mod_ocgcore_cs_end);
        loadResource(mod_ocgcore_cs_bomb);
        loadResource(mod_ocgcore_cs_negated);

        loadResource(mod_ocgcore_ss_summon_earth);
        loadResource(mod_ocgcore_ss_summon_water);
        loadResource(mod_ocgcore_ss_summon_fire);
        loadResource(mod_ocgcore_ss_summon_wind);
        loadResource(mod_ocgcore_ss_summon_dark);
        loadResource(mod_ocgcore_ss_summon_light);

        loadResource(mod_ocgcore_ol_earth);
        loadResource(mod_ocgcore_ol_water);
        loadResource(mod_ocgcore_ol_fire);
        loadResource(mod_ocgcore_ol_wind);
        loadResource(mod_ocgcore_ol_dark);
        loadResource(mod_ocgcore_ol_light);

        loadResource(mod_ocgcore_ss_spsummon_normal);
        loadResource(mod_ocgcore_ss_spsummon_ronghe);
        loadResource(mod_ocgcore_ss_spsummon_tongtiao);
        loadResource(mod_ocgcore_ss_spsummon_link);
        loadResource(mod_ocgcore_ss_spsummon_yishi);
        loadResource(mod_ocgcore_ss_p_idle_effect);
        loadResource(mod_ocgcore_ss_p_sum_effect);
        loadResource(mod_ocgcore_ss_dark_hole);
        loadResource(mod_ocgcore_ss_link_mark);
    }

    public static float transparency = 0;
    public float targetFrame = 144f;
    public int me_HP = 8000;

    //public static bool YGOPro1 = true;

    public static float getVerticalTransparency()
    {
        if (I().setting.setting.closeUp.value == false) return 0;
        return transparency;
    }


    public static Vector3 cameraPosition = new Vector3(0, 95f, -37f);//mark 决斗时摄像机位置
    public static Vector3 cameraRotation = new Vector3(70, 0, 0);
    public static bool cameraFacing = false;

    public static float verticleScale = 5f;

    private void initialize()
    {
        UIHelper.iniFaces();
        initializeALLcameras();
        fixALLcamerasPreFrame();
        backGroundPic = new BackGroundPic();
        servants.Add(backGroundPic);
        backGroundPic.fixScreenProblem();
        InterString.initialize("config/translation.conf");
        GameTextureManager.initialize();
        Config.initialize("config/config.conf");

        if (!Directory.Exists("expansions"))
            try
            {
                Directory.CreateDirectory("expansions");
            }
            catch
            {
            }

        if (!Directory.Exists("replay"))
            try
            {
                Directory.CreateDirectory("replay");
            }
            catch
            {
            }

        var fileInfos = new FileInfo[0];

        if (Directory.Exists("expansions"))
        {
            fileInfos = new DirectoryInfo("expansions").GetFiles();
            foreach (var file in fileInfos)
            {
                if (file.Name.ToLower().EndsWith(".ypk"))
                    GameZipManager.Zips.Add(new ZipFile("expansions/" + file.Name));
                if (file.Name.ToLower().EndsWith(".conf")) GameStringManager.initialize("expansions/" + file.Name);
                if (file.Name.ToLower().EndsWith(".cdb")) CardsManager.initialize("expansions/" + file.Name);
            }
        }

        if (Directory.Exists("cdb"))
        {
            fileInfos = new DirectoryInfo("cdb").GetFiles();
            foreach (var file in fileInfos)
            {
                if (file.Name.ToLower().EndsWith(".conf")) GameStringManager.initialize("cdb/" + file.Name);
                if (file.Name.ToLower().EndsWith(".cdb")) CardsManager.initialize("cdb/" + file.Name);
            }
        }

        if (Directory.Exists("diy"))
        {
            fileInfos = new DirectoryInfo("diy").GetFiles();
            foreach (var file in fileInfos)
            {
                if (file.Name.ToLower().EndsWith(".conf")) GameStringManager.initialize("diy/" + file.Name);
                if (file.Name.ToLower().EndsWith(".cdb")) CardsManager.initialize("diy/" + file.Name);
            }
        }

        if (Directory.Exists("data"))
        {
            fileInfos = new DirectoryInfo("data").GetFiles();
            foreach (var file in fileInfos)
                if (file.Name.ToLower().EndsWith(".zip"))
                    GameZipManager.Zips.Add(new ZipFile("data/" + file.Name));
        }

        foreach (var zip in GameZipManager.Zips)
        {
            if (zip.Name.ToLower().EndsWith("script.zip"))
                continue;
            foreach (var file in zip.EntryFileNames)
            {
                if (file.ToLower().EndsWith(".conf"))
                {
                    var ms = new MemoryStream();
                    var e = zip[file];
                    e.Extract(ms);
                    GameStringManager.initializeContent(Encoding.UTF8.GetString(ms.ToArray()));
                }

                if (file.ToLower().EndsWith(".cdb"))
                {
                    var e = zip[file];
                    var tempfile = Path.Combine(Path.GetTempPath(), file);
                    e.Extract(Path.GetTempPath(), ExtractExistingFileAction.OverwriteSilently);
                    CardsManager.initialize(tempfile);
                    File.Delete(tempfile);
                }
            }
        }

        GameStringManager.initialize("config/strings.conf");
        BanlistManager.initialize("config/lflist.conf");

        CardsManager.updateSetNames();

        if (Directory.Exists("pack"))
        {
            fileInfos = new DirectoryInfo("pack").GetFiles();
            foreach (var file in fileInfos)
                if (file.Name.ToLower().EndsWith(".db"))
                    PacksManager.initialize("pack/" + file.Name);
            PacksManager.initializeSec();
        }

        initializeALLservants();
        loadResources();
    }

    private void readParams()
    {
        var args = Environment.GetCommandLineArgs();
        string nick = null;
        string host = null;
        string port = null;
        string password = null;
        string deck = null;
        string replay = null;
        string puzzle = null;
        var join = false;
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i].ToLower() == "-n" && args.Length > i + 1)
            {
                nick = args[++i];
            }

            if (args[i].ToLower() == "-h" && args.Length > i + 1) host = args[++i];
            if (args[i].ToLower() == "-p" && args.Length > i + 1) port = args[++i];
            if (args[i].ToLower() == "-w" && args.Length > i + 1)
            {
                password = args[++i];
            }

            if (args[i].ToLower() == "-d" && args.Length > i + 1)
            {
                deck = args[++i];
            }

            if (args[i].ToLower() == "-r" && args.Length > i + 1)
            {
                replay = args[++i];
            }

            if (args[i].ToLower() == "-s" && args.Length > i + 1)
            {
                puzzle = args[++i];
            }

            if (args[i].ToLower() == "-j")
            {
                join = true;
                Config.Set("deckInUse", deck);
            }
        }

        if (join)
        {
            shiftToServant(selectServer);
            selectServer.KF_onlineGame(nick, host, port, "0x233", password);
            exitOnReturn = true;
        }
        else if (deck != null)
        {
            selectDeck.KF_editDeck(deck);
            exitOnReturn = true;
        }
        else if (replay != null)
        {
            selectReplay.KF_replay(replay);
            exitOnReturn = true;
        }
        else if (puzzle != null)
        {
            puzzleMode.KF_puzzle(puzzle);
            exitOnReturn = true;
        }
    }

    private static int lastChargeTime;

    public static void charge()
    {
        if (TimePassed() - lastChargeTime > 5 * 60 * 1000)
        {
            lastChargeTime = TimePassed();
            try
            {
                GameTextureManager.clearAll();
                Resources.UnloadUnusedAssets();
                GC.Collect();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    #endregion

    #region Tools

    public static GameObject pointedGameObject;

    public static Collider pointedCollider;

    public static bool InputGetMouseButtonDown_0;

    public static bool InputGetMouseButton_0;

    public static bool InputGetMouseButtonUp_0;

    public static bool InputGetMouseButtonDown_1;

    public static bool InputGetMouseButtonUp_1;

    public static bool InputEnterDown;

    public static float wheelValue;

    public class delayedTask
    {
        public Action act;
        public int timeToBeDone;
    }

    private static readonly List<delayedTask> delayedTasks = new List<delayedTask>();

    public static void go(int delay_, Action act_)
    {
        delayedTasks.Add(new delayedTask
        {
            act = act_,
            timeToBeDone = delay_ + TimePassed()
        });
    }

    public static void notGo(Action act_)
    {
        var rem = new List<delayedTask>();
        for (var i = 0; i < delayedTasks.Count; i++)
            if (delayedTasks[i].act == act_)
                rem.Add(delayedTasks[i]);
        for (var i = 0; i < rem.Count; i++) delayedTasks.Remove(rem[i]);
        rem.Clear();
    }

    private int rayFilter;

    public void initializeALLcameras()
    {
        for (var i = 0; i < 32; i++)
        {
            if (i == 15) continue;
            rayFilter |= (int) Math.Pow(2, i);
        }
    }

    public static float deltaTime = 1f / 120f;

    public void fixALLcamerasPreFrame()//mark 摄像机位置调整
    {
        if (upMode.lockCamera) cameraPosition = new Vector3(0, 95, -37);
        deltaTime = Time.deltaTime;
        if (deltaTime > 1f / targetFrame) deltaTime = 1f / targetFrame;
        if (main_camera != null)
        {
            main_camera.transform.position +=
                (cameraPosition - main_camera.transform.position) * deltaTime * 7f;//3.5f;
            camera_container_3d.transform.localPosition = main_camera.transform.position;
            if (cameraFacing == false)
                main_camera.transform.localEulerAngles +=
                    (cameraRotation - main_camera.transform.localEulerAngles) * deltaTime * 3.5f;
            else
                main_camera.transform.LookAt(Vector3.zero);
            camera_container_3d.transform.localEulerAngles = main_camera.transform.localEulerAngles;
            camera_container_3d.fieldOfView = main_camera.fieldOfView;
            camera_container_3d.rect = main_camera.rect;
        }
    }

    public void fixScreenProblems()
    {
        foreach (var t in servants)
            t.fixScreenProblem();
    }

    public GameObject create(
        GameObject mod,
        Vector3 position = default,
        Vector3 rotation = default,
        bool fade = false,
        GameObject father = null,
        bool allParamsInWorld = true,
        Vector3 wantScale = default
    )
    {
        var scale = mod.transform.localScale;
        if (wantScale != default) scale = wantScale;
        var return_value = Instantiate(mod);
        if (position != default)
            return_value.transform.position = position;
        else
            return_value.transform.position = Vector3.zero;
        if (rotation != default)
            return_value.transform.eulerAngles = rotation;
        else
            return_value.transform.eulerAngles = Vector3.zero;
        if (father != null)
        {
            return_value.transform.SetParent(father.transform, false);
            return_value.layer = father.layer;
            if (allParamsInWorld)
            {
                return_value.transform.position = position;
                return_value.transform.localScale = scale;
                return_value.transform.eulerAngles = rotation;
            }
            else
            {
                return_value.transform.localPosition = position;
                return_value.transform.localScale = scale;
                return_value.transform.localEulerAngles = rotation;
            }
        }
        else
        {
            return_value.layer = 0;
        }

        var Transforms = return_value.GetComponentsInChildren<Transform>();
        foreach (var child in Transforms) child.gameObject.layer = return_value.layer;
        if (fade)
        {
            return_value.transform.localScale = Vector3.zero;
            iTween.ScaleToE(return_value, scale, 0.3f);
        }

        return return_value;
    }

    public void destroy(GameObject obj, float time = 0, bool fade = false, bool instantNull = false)
    {
        try
        {
            if (obj != null)
            {
                if (fade)
                {
                    iTween.ScaleTo(obj, Vector3.zero, 0.4f);
                    Destroy(obj, 0.6f);
                }
                else
                {
                    if (time != 0) Destroy(obj, time);
                    else Destroy(obj);
                }

                if (instantNull) obj = null;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    //public static void shiftCameraPan(Camera camera, bool enabled)
    //{
    //    cameraPaning = enabled;
    //    PanWithMouse panWithMouse = camera.gameObject.GetComponent<PanWithMouse>();
    //    if (panWithMouse == null)
    //    {
    //        panWithMouse = camera.gameObject.AddComponent<PanWithMouse>();
    //    }
    //    panWithMouse.enabled = enabled;
    //    if (enabled == false)
    //    {
    //        iTween.RotateTo(camera.gameObject, new Vector3(60, 0, 0), 0.6f);
    //    }
    //}

    public static void reMoveCam(float xINscreen)//mark remove camera
    {
        var all = Screen.width / 2f;
        var it = xINscreen - Screen.width / 2f;
        var val = it / all;
        //I().main_camera.rect = new Rect(val, 0, 1, 1);
        I().camera_container_3d.rect = I().main_camera.rect;
        I().camera_main_3d.rect = I().main_camera.rect;
    }

    public static void ShiftUIenabled(GameObject ui, bool enabled)
    {
        var all = ui.GetComponentsInChildren<BoxCollider>();
        for (var i = 0; i < all.Length; i++) all[i].enabled = enabled;
    }

    public static Texture2D GetTextureViaPath(string path)
    {
        var file = new FileStream(path, FileMode.Open, FileAccess.Read);
        file.Seek(0, SeekOrigin.Begin);
        var data = new byte[file.Length];
        file.Read(data, 0, (int) file.Length);
        file.Close();
        file.Dispose();
        file = null;
        var pic = new Texture2D(1024, 600);
        pic.LoadImage(data);
        return pic;
    }

    #endregion

    #region Servants

    private readonly List<Servant> servants = new List<Servant>();

    public Servant backGroundPic;
    public Menu menu;
    public Setting setting;
    public selectDeck selectDeck;
    public selectReplay selectReplay;
    public Room room;
    public CardDescription cardDescription;
    public DeckManager deckManager;
    public Ocgcore ocgcore;
    public SelectServer selectServer;
    public Book book;
    public puzzleMode puzzleMode;
    // public AIRoom aiRoom;

    private void initializeALLservants()
    {
        menu = new Menu();
        servants.Add(menu);
        setting = new Setting();
        servants.Add(setting);
        selectDeck = new selectDeck();
        servants.Add(selectDeck);
        room = new Room();
        servants.Add(room);
        cardDescription = new CardDescription();
        deckManager = new DeckManager();
        servants.Add(deckManager);
        ocgcore = new Ocgcore();
        servants.Add(ocgcore);
        selectServer = new SelectServer();
        servants.Add(selectServer);
        book = new Book();
        servants.Add(book);
        selectReplay = new selectReplay();
        servants.Add(selectReplay);
        puzzleMode = new puzzleMode();
        servants.Add(puzzleMode);
        // aiRoom = new AIRoom();
        // servants.Add(aiRoom);
    }

    public void shiftToServant(Servant to)
    {
        if (to != backGroundPic && backGroundPic.isShowed) backGroundPic.hide();
        if (to != menu && menu.isShowed) menu.hide();
        if (to != setting && setting.isShowed) setting.hide();
        if (to != selectDeck && selectDeck.isShowed) selectDeck.hide();
        if (to != room && room.isShowed) room.hide();
        if (to != deckManager && deckManager.isShowed) deckManager.hide();
        if (to != ocgcore && ocgcore.isShowed) ocgcore.hide();
        if (to != selectServer && selectServer.isShowed) selectServer.hide();
        if (to != selectReplay && selectReplay.isShowed) selectReplay.hide();
        if (to != puzzleMode && puzzleMode.isShowed) puzzleMode.hide();
        // if (to != aiRoom && aiRoom.isShowed) aiRoom.hide();

        if (to == backGroundPic && backGroundPic.isShowed == false) backGroundPic.show();
        if (to == menu && menu.isShowed == false) menu.show();
        if (to == setting && setting.isShowed == false) setting.show();
        if (to == selectDeck && selectDeck.isShowed == false) selectDeck.show();
        if (to == room && room.isShowed == false) room.show();
        if (to == deckManager && deckManager.isShowed == false) deckManager.show();
        if (to == ocgcore && ocgcore.isShowed == false) ocgcore.show();
        if (to == selectServer && selectServer.isShowed == false) selectServer.show();
        if (to == selectReplay && selectReplay.isShowed == false) selectReplay.show();
        if (to == puzzleMode && puzzleMode.isShowed == false) puzzleMode.show();
        // if (to == aiRoom && aiRoom.isShowed == false) aiRoom.show();
    }

    #endregion

    #region MonoBehaviors

    private void Awake()
    {
        if (Screen.width < 100 || Screen.height < 100) Screen.SetResolution(1366, 768, false);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
        // mouseParticle = Instantiate(new_mouse);
        instance = this;
        initialize();
        gameStart();
        readParams();
    }

    private void Start()
    {
        LoadAssets.LoadSFX();
    }


    private int preWid;

    private int preheight;

    public static float _padScroll;

    private void OnGUI()
    {
        if (Event.current.type == EventType.ScrollWheel)
            _padScroll = -Event.current.delta.y / 100;
        else
            _padScroll = 0;
    }

    private void Update()
    {
        if (preWid != Screen.width || preheight != Screen.height)
        {
            Resources.UnloadUnusedAssets();
            onRESIZED();
        }

        fixALLcamerasPreFrame();
        wheelValue = UICamera.GetAxis("Mouse ScrollWheel") * 50;
        pointedGameObject = null;
        pointedCollider = null;
        var line = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(line, out hit, 1000, rayFilter))
        {
            pointedGameObject = hit.collider.gameObject;
            pointedCollider = hit.collider;
        }

        var hoverobject = UICamera.Raycast(Input.mousePosition) ? UICamera.lastHit.collider.gameObject : null;
        if (hoverobject != null)
            if (hoverobject.layer == 11 || pointedGameObject == null)
            {
                pointedGameObject = hoverobject;
                pointedCollider = UICamera.lastHit.collider;
            }

        InputGetMouseButtonDown_0 = Input.GetMouseButtonDown(0);
        InputGetMouseButtonUp_0 = Input.GetMouseButtonUp(0);
        InputGetMouseButtonDown_1 = Input.GetMouseButtonDown(1);
        InputGetMouseButtonUp_1 = Input.GetMouseButtonUp(1);
        InputEnterDown = Input.GetKeyDown(KeyCode.Return);
        InputGetMouseButton_0 = Input.GetMouseButton(0);
        for (var i = 0; i < servants.Count; i++) servants[i].Update();
        TcpHelper.preFrameFunction();
        delayedTask remove = null;
        while (true)
        {
            remove = null;
            for (var i = 0; i < delayedTasks.Count; i++)
                if (TimePassed() > delayedTasks[i].timeToBeDone)
                {
                    remove = delayedTasks[i];
                    try
                    {
                        remove.act();
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e);
                    }

                    break;
                }

            if (remove != null)
                delayedTasks.Remove(remove);
            else
                break;
        }
    }

    private void onRESIZED()
    {
        preWid = Screen.width;
        preheight = Screen.height;
        //if (setting != null)
        //    setting.setScreenSizeValue();
        notGo(fixScreenProblems);
        go(500, fixScreenProblems);
    }

    public static void DEBUGLOG(object o)
    {
#if UNITY_EDITOR
        Debug.Log(o);
#endif
    }

    public static void PrintToChat(object o)
    {
        try
        {
            instance.cardDescription.mLog(o.ToString());
        }
        catch
        {
            DEBUGLOG(o);
        }
    }

    private void gameStart()
    {
        if (UIHelper.shouldMaximize()) UIHelper.MaximizeWindow();
        backGroundPic.show();
        shiftToServant(menu);
    }

    public static bool Running = true;

    public static bool MonsterCloud = false;
    public static float fieldSize = 1;
    public static bool longField = false;

    public static bool noAccess = false;

    public static bool exitOnReturn;

    private void OnApplicationQuit()
    {
        TcpHelper.SaveRecord();
        cardDescription.save();
        setting.saveWhenQuit();
        for (var i = 0; i < servants.Count; i++) servants[i].OnQuit();
        Running = false;
        try
        {
            TcpHelper.tcpClient.Close();
        }
        catch (Exception e)
        {
            //adeUnityEngine.Debug.Log(e);
        }

        foreach (var zip in GameZipManager.Zips) zip.Dispose();
    }

    public void quit()
    {
        OnApplicationQuit();
    }

    #endregion
}