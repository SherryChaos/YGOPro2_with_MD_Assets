using UnityEngine;

public class handShower : MonoBehaviour
{
    public Texture2D[] pics = new Texture2D[3];
    public int op;
    public int me;
    public UITexture texture_0;
    public UITexture texture_1;
    public GameObject GameObject_0;
    public GameObject GameObject_1;

    private void Start()
    {
        pics[0] = GameTextureManager.GetUI("jiandao");
        pics[1] = GameTextureManager.GetUI("shitou");
        pics[2] = GameTextureManager.GetUI("bu");
        texture_0.mainTexture = pics[me];
        texture_1.mainTexture = pics[op];
        GameObject_0.transform.position = Program.I().camera_main_2d
            .ScreenToWorldPoint(new Vector3(Screen.width / 2, -Screen.height * 1.5f, 0));
        iTween.MoveToAction(GameObject_0,
            Program.I().camera_main_2d.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2 - 80, 0)), 1f,
            () => { Destroy(GameObject_0, 0.3f); });
        GameObject_1.transform.position = Program.I().camera_main_2d
            .ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 1.5f, 0));
        iTween.MoveToAction(GameObject_1,
            Program.I().camera_main_2d.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2 + 80, 0)), 1f,
            () => { Destroy(GameObject_1, 0.3f); });
    }
}