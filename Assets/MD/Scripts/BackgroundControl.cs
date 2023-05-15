using UnityEngine;
using DG.Tweening;

public class BackgroundControl : MonoBehaviour
{
    public Sprite wp001_1;
    public Sprite wp001_2;
    public Sprite wp002_1;
    public Sprite wp002_2;
    public Sprite wp003_1;
    public Sprite wp003_2;
    public Sprite wp004_1;
    public Sprite wp004_2;
    public Sprite wp005_1;
    public Sprite wp005_2;
    public Sprite wp006_1;
    public Sprite wp006_2;
    public Sprite wp007_1;
    public Sprite wp007_2;
    public Sprite wp008_1;
    public Sprite wp008_2;
    public Sprite wp009_1;
    public Sprite wp009_2;
    public Sprite wp010_1;
    public Sprite wp010_2;
    public Sprite wp011_1;
    public Sprite wp011_2;
    public Sprite wp012_1;
    public Sprite wp012_2;
    public Sprite wp013_1;
    public Sprite wp013_2;
    public Sprite wp014_1;
    public Sprite wp014_2;
    public Sprite wp017_1;
    public Sprite wp017_2;

    Transform wallPaperM;
    Transform wallPaperB;
    struct WallPaper
    {
        public int direction;
        public Vector3 mPoint;
        public Vector3 bPoint;
    }

    WallPaper wallPaper_;
    WallPaper wallPaper_01;
    WallPaper wallPaper_02;
    WallPaper wallPaper_03;
    WallPaper wallPaper_04;
    WallPaper wallPaper_05;
    WallPaper wallPaper_06;
    WallPaper wallPaper_07;
    WallPaper wallPaper_08;
    WallPaper wallPaper_09;
    WallPaper wallPaper_10;
    WallPaper wallPaper_11;
    WallPaper wallPaper_12;
    WallPaper wallPaper_13;
    WallPaper wallPaper_14;
    WallPaper wallPaper_17;

    string wallPaper;
    bool hasStarted = false;
    Sequence mIn;
    Sequence bIn;
    Sequence mLoop;
    Sequence bLoop;


    void Start()
    {
        //wallPaper = GetFieldValue.wallpaper;
        wallPaperM = this.transform.Find("WallPaper01");
        wallPaperB = this.transform.Find("WallPaper02");
        StructInitialize();
        Initialize(); 
        StartPlay();
        var bg = ABLoader.LoadAB("effects/back/back0001").transform;
        bg.SetParent(this.transform);
        bg.localPosition = Vector3.zero;
        bg.localRotation = Quaternion.identity;
        bg.localScale = Vector3.one;
    }

    //void Update()
    //{
    //    if (wallPaper != GetFieldValue.wallpaper)
    //    {
    //        wallPaper = GetFieldValue.wallpaper;
    //        Initialize();
    //        StartPlay();
    //    }
    //}
    void StartPlay()
    {
        DOTween.KillAll();
        float x = 1f;

        switch (wallPaper_.direction)
        {
            case 1:
                wallPaperM.DOLocalMoveY(x, 1).From(true).SetRelative().SetEase(Ease.OutCubic);
                wallPaperB.DOLocalMoveY(x, 1.3f).From(true).SetRelative().SetEase(Ease.OutCubic);
                break;
            case 2:
                wallPaperM.DOLocalMoveY(-x, 1).From(true).SetRelative().SetEase(Ease.OutCubic);
                wallPaperB.DOLocalMoveY(-x, 1.3f).From(true).SetRelative().SetEase(Ease.OutCubic);
                break;
            case 3:
                wallPaperM.DOLocalMoveX(-x, 1).From(true).SetRelative().SetEase(Ease.OutCubic);
                wallPaperB.DOLocalMoveX(-x, 1.3f).From(true).SetRelative().SetEase(Ease.OutCubic);
                break;
            case 4:
                wallPaperM.DOLocalMoveX(x, 1).From(true).SetRelative().SetEase(Ease.OutCubic);
                wallPaperB.DOLocalMoveX(x, 1.3f).From(true).SetRelative().SetEase(Ease.OutCubic);
                break;
        }
        Invoke("Loop", 2);
    }

    void Loop()
    {
        DOTween.KillAll();
        mLoop = DOTween.Sequence();
        //mLoop.AppendInterval(1);
        mLoop.Append(wallPaperM.DOLocalMoveY(0.2f, 5).SetRelative().SetEase(Ease.InOutCubic));
        //mLoop.AppendInterval(1);
        mLoop.Append(wallPaperM.DOLocalMoveY(-0.2f, 5).SetRelative().SetEase(Ease.InOutCubic));
        mLoop.SetLoops(-1);
        bLoop = DOTween.Sequence();
        //bLoop.AppendInterval(1);
        bLoop.Append(wallPaperB.DOLocalMoveY(0.1f, 5).SetRelative().SetEase(Ease.InOutCubic));
        bLoop.Join(wallPaperB.GetComponent<SpriteRenderer>().DOFade(1, 5).SetEase(Ease.InOutCubic));
        //bLoop.AppendInterval(1);
        bLoop.Append(wallPaperB.DOLocalMoveY(-0.1f, 5).SetRelative().SetEase(Ease.InOutCubic));
        bLoop.Join(wallPaperB.GetComponent<SpriteRenderer>().DOFade(0.2f, 5).SetEase(Ease.InOutCubic));
        bLoop.SetLoops(-1);
        mLoop.Play().SetDelay(3);
        bLoop.Play().SetDelay(3);
    }
    void StructInitialize()
    {
        wallPaper_01.direction = 4;
        wallPaper_01.mPoint = new Vector3(7.9f, 0.9f, -1f);
        wallPaper_01.bPoint = new Vector3(3f, 0.5f, 0f);
        wallPaper_02.direction = 2;
        wallPaper_02.mPoint = new Vector3(4.8f, -0.85f, -1f);
        wallPaper_02.bPoint = new Vector3(4.2f, -1f, 0f);
        wallPaper_03.direction = 1;
        wallPaper_03.mPoint = new Vector3(6.84f, 1.95f, -1f);
        wallPaper_03.bPoint = new Vector3(6.45f, -1.54f, 0f);
        wallPaper_04.direction = 2;
        wallPaper_04.mPoint = new Vector3(3.8f, -0.66f, -1f);
        wallPaper_04.bPoint = new Vector3(4.7f, -1.5f, 0f);
        wallPaper_05.direction = 1;
        wallPaper_05.mPoint = new Vector3(5.4f, -0.42f, -1f);
        wallPaper_05.bPoint = new Vector3(5.3f, 0.48f, 0f);
        wallPaper_06.direction = 1;
        wallPaper_06.mPoint = new Vector3(4.72f, -0.9f, -1f);
        wallPaper_06.bPoint = new Vector3(4.14f, 4.3f, 0f);
        wallPaper_07.direction = 2;
        wallPaper_07.mPoint = new Vector3(3.76f, -0.54f, -1f);
        wallPaper_07.bPoint = new Vector3(4.67f, -0.89f, 0f);
        wallPaper_08.direction = 2;
        wallPaper_08.mPoint = new Vector3(3.75f, 0.54f, -1f);
        wallPaper_08.bPoint = new Vector3(3.62f, 0.68f, 0f);
        wallPaper_09.direction = 2;
        wallPaper_09.mPoint = new Vector3(4.23f, -0.52f, -1f);
        wallPaper_09.bPoint = new Vector3(3.83f, -0.4f, 0f);
        wallPaper_10.direction = 2;
        wallPaper_10.mPoint = new Vector3(5.3f, 0.63f, -1f);
        wallPaper_10.bPoint = new Vector3(5.3f, 0.63f, 0f);
        wallPaper_11.direction = 2;
        wallPaper_11.mPoint = new Vector3(2.77f, 1.2f, -1f);
        wallPaper_11.bPoint = new Vector3(4f, -0.14f, 0f);
        wallPaper_12.direction = 2;
        wallPaper_12.mPoint = new Vector3(2.14f, 0.75f, -1f);
        wallPaper_12.bPoint = new Vector3(4.46f, 0.16f, 0f);
        wallPaper_13.direction = 1;
        wallPaper_13.mPoint = new Vector3(3.7f, -1.8f, -1f);
        wallPaper_13.bPoint = new Vector3(3.94f, 0.42f, 0f);
        wallPaper_14.direction = 2;
        wallPaper_14.mPoint = new Vector3(4.34f, -1.9f, -1f);
        wallPaper_14.bPoint = new Vector3(3.44f, 0.52f, 0f);
        wallPaper_14.direction = 2;
        wallPaper_17.mPoint = new Vector3(4f, -2f, -1f);
        wallPaper_17.bPoint = new Vector3(3.5f, 0.9f, 0f);
    }
    void Initialize()
    {
        switch (wallPaper)
        {
            case "�����ǰ���":
                wallPaper_ = wallPaper_01;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp001_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp001_2;
                break;
            case "��Ӱ�����÷���":
                wallPaper_ = wallPaper_02;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp002_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp002_2;
                break;
            case "����ս�� ����֮ʩ����":
                wallPaper_ = wallPaper_03;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp003_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp003_2;
                break;
            case "����֮���� ��������":
                wallPaper_ = wallPaper_04;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp004_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp004_2;
                break;
            case "������-����":
                wallPaper_ = wallPaper_05;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp005_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp005_2;
                break;
            case "�ƽ��� �ƽ������":
                wallPaper_ = wallPaper_06;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp006_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp006_2;
                break;
            case "˫�֮��ʿ ��˹����ķ":
                wallPaper_ = wallPaper_07;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp007_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp007_2;
                break;
            case "���ǻ���Ⱥ":
                wallPaper_ = wallPaper_08;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp008_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp008_2;
                break;
            case "�ػ���� �����":
                wallPaper_ = wallPaper_09;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp009_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp009_2;
                break;
            case "Ԫ��Ӣ�� ���������":
                wallPaper_ = wallPaper_10;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp010_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp010_2;
                break;
            case "����������":
                wallPaper_ = wallPaper_11;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp011_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp011_2;
                break;
            case "δ��No.0 δ������ ����":
                wallPaper_ = wallPaper_12;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp012_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp012_2;
                break;
            case "��ɫ�۰Ի������":
                wallPaper_ = wallPaper_13;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp013_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp013_2;
                break;
            case "����������":
                wallPaper_ = wallPaper_14;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp014_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp014_2;
                break;
            case "��ħ��ʦ":
                wallPaper_ = wallPaper_17;
                wallPaperM.GetComponent<SpriteRenderer>().sprite = wp017_1;
                wallPaperB.GetComponent<SpriteRenderer>().sprite = wp017_2;
                break;


            default:
                break;
        }
        wallPaperM.localPosition = wallPaper_.mPoint;
        wallPaperB.localPosition = wallPaper_.bPoint;
        wallPaperB.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.2f);
    }
}
