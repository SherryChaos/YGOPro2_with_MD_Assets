using UnityEngine;

public static class Utils
{
    public static int UIHeight()
    {
        return Program.I().ui_back_ground_2d.GetComponent<UIRoot>().activeHeight;
    }

    public static int UIWidth()
    {
        return UIHeight() * Screen.width / Screen.height;
    }

    public static Vector3 UIToWorldPoint(Vector3 point)
    {
        return Program.I().camera_back_ground_2d
            .ViewportToWorldPoint(new Vector3(point.x / UIWidth(), point.y / UIHeight(), point.z));
    }

    public static Vector3 WorldToUIPoint(Vector3 point)
    {
        var viewport = Program.I().camera_back_ground_2d.WorldToViewportPoint(point);
        return new Vector3(viewport.x * UIWidth(), viewport.y * UIHeight(), viewport.z);
    }
}