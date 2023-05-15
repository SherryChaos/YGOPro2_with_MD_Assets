using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using YGOSharp.OCGWrapper.Enums;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public static class UIHelper
{
    public delegate bool WNDENUMPROC(IntPtr hwnd, IntPtr lParam);

    public enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent
    }

    private static IntPtr myHWND = IntPtr.Zero;

    public static Dictionary<string, Texture2D> faces = new Dictionary<string, Texture2D>();

    [DllImport("user32")]
    private static extern bool FlashWindow(IntPtr handle, bool invert);

    [DllImport("user32", SetLastError = true)]
    private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

    [DllImport("user32", SetLastError = true)]
    private static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref IntPtr lpdwProcessId);

    [DllImport("user32")]
    private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString,
        int nMaxCount);

    [DllImport("user32")]
    private static extern bool IsZoomed(IntPtr hWnd);

    [DllImport("user32")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("kernel32")]
    private static extern void SetLastError(uint dwErrCode);

    private static IntPtr GetProcessWnd()
    {
        if (myHWND != IntPtr.Zero)
            return myHWND;

        var ptrWnd = IntPtr.Zero;
        var pid = (IntPtr) Process.GetCurrentProcess().Id; // 当前进程 ID

        var bResult = EnumWindows(delegate(IntPtr hwnd, IntPtr mypid)
        {
            var id = IntPtr.Zero;

            var ClassName = new StringBuilder(256);
            GetClassNameW(hwnd, ClassName, ClassName.Capacity);

            if (string.Compare(ClassName.ToString(), "UnityWndClass", true, CultureInfo.InvariantCulture) == 0)
            {
                GetWindowThreadProcessId(hwnd, ref id);
                if (id == mypid) // 找到进程对应的主窗口句柄
                {
                    ptrWnd = hwnd; // 把句柄缓存起来
                    SetLastError(0); // 设置无错误
                    return false; // 返回 false 以终止枚举窗口
                }
            }

            return true;
        }, pid);

        if (!bResult && Marshal.GetLastWin32Error() == 0) myHWND = ptrWnd;

        return myHWND;
    }

    public static void Flash()
    {
        FlashWindow(GetProcessWnd(), true);
    }

    public static bool isMaximized()
    {
#if UNITY_STANDALONE_WIN
        return IsZoomed(GetProcessWnd());
#else
        // not a easy thing to check window status on non-windows desktop...
        return false;
#endif
    }

    public static void MaximizeWindow()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(GetProcessWnd(), 3); // SW_MAXIMIZE
#endif
    }

    public static void RestoreWindow()
    {
#if UNITY_STANDALONE_WIN
        ShowWindow(GetProcessWnd(), 9); // SW_RESTORE
#endif
    }

    public static bool shouldMaximize()
    {
        return fromStringToBool(Config.Get("maximize_", "0"));
    }

    public static void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch (renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int) BlendMode.One);
                material.SetInt("_DstBlend", (int) BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int) BlendMode.One);
                material.SetInt("_DstBlend", (int) BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int) BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int) BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int) BlendMode.One);
                material.SetInt("_DstBlend", (int) BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

    internal static void registEvent(UIButton btn, Action function)
    {
        if (btn != null)
        {
            var d = btn.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = btn.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(d, "function"));
        }
    }

    internal static Texture2D[] sliceField(Texture2D textureField_)
    {
        var textureField = ScaleTexture(textureField_, 1024, 819);
        var returnValue = new Texture2D[3];
        returnValue[0] = new Texture2D(textureField.width, textureField.height);
        returnValue[1] = new Texture2D(textureField.width, textureField.height);
        returnValue[2] = new Texture2D(textureField.width, textureField.height);
        var zuo = textureField.width * 69f / 320f;
        var you = textureField.width * 247f / 320f;
        for (var w = 0; w < textureField.width; w++)
        for (var h = 0; h < textureField.height; h++)
        {
            var c = textureField.GetPixel(w, h);
            if (c.a < 0.05f) c.a = 0;
            if (w < zuo)
            {
                returnValue[0].SetPixel(w, h, c);
                returnValue[1].SetPixel(w, h, new Color(0, 0, 0, 0));
                returnValue[2].SetPixel(w, h, new Color(0, 0, 0, 0));
            }
            else if (w > you)
            {
                returnValue[2].SetPixel(w, h, c);
                returnValue[0].SetPixel(w, h, new Color(0, 0, 0, 0));
                returnValue[1].SetPixel(w, h, new Color(0, 0, 0, 0));
            }
            else
            {
                returnValue[1].SetPixel(w, h, c);
                returnValue[0].SetPixel(w, h, new Color(0, 0, 0, 0));
                returnValue[2].SetPixel(w, h, new Color(0, 0, 0, 0));
            }
        }

        returnValue[0].Apply();
        returnValue[1].Apply();
        returnValue[2].Apply();
        return returnValue;
    }

    private static Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        var result = new Texture2D(targetWidth, targetHeight, source.format, false);

        var incX = 1.0f / targetWidth;
        var incY = 1.0f / targetHeight;

        for (var i = 0; i < result.height; ++i)
        for (var j = 0; j < result.width; ++j)
        {
            var newColor = source.GetPixelBilinear(j / (float) result.width, i / (float) result.height);
            result.SetPixel(j, i, newColor);
        }

        result.Apply();
        return result;
    }

    public static T getByName<T>(GameObject father, string name) where T : Component
    {
        T return_value = null;
        var all = father.transform.GetComponentsInChildren<T>();
        for (var i = 0; i < all.Length; i++)
            if (all[i].name == name)
                return_value = all[i];
        return return_value;
    }

    public static void InterGameObject(GameObject father)
    {
        var all = father.transform.GetComponentsInChildren<UILabel>();
        for (var i = 0; i < all.Length; i++)
            if (all[i].name.Length > 1 && all[i].name[0] == '!' || all[i].name == "yes_" || all[i].name == "no_")
                all[i].text = InterString.Get(all[i].text);
    }

    public static GameObject getByName(GameObject father, string name)
    {
        GameObject return_value = null;
        var all = father.transform.GetComponentsInChildren<Transform>();
        for (var i = 0; i < all.Length; i++)
            if (all[i].name == name)
                return_value = all[i].gameObject;
        return return_value;
    }

    public static T getByName<T>(GameObject father) where T : Component
    {
        var return_value = father.transform.GetComponentInChildren<T>();
        return return_value;
    }

    public static UILabel getLabelName(GameObject father, string name)
    {
        UILabel return_value = null;
        var all = father.transform.GetComponentsInChildren<UILabel>();
        for (var i = 0; i < all.Length; i++)
            if (all[i].name == name
                ||
                all[i].transform.parent != null && all[i].transform.parent.name == name
                ||
                all[i].transform.parent.parent != null && all[i].transform.parent.parent.name == name
                ||
                all[i].transform.parent.parent.parent != null && all[i].transform.parent.parent.parent.name == name
            )
                return_value = all[i];
        for (var i = 0; i < all.Length; i++)
            if (all[i].name == name)
                return_value = all[i];
        return return_value;
    }

    internal static int[] get_decklieshuArray(int count)
    {
        var ret = new int[4];
        ret[0] = 10;
        ret[1] = 10;
        ret[2] = 10;
        ret[3] = 10;
        for (var i = 41; i <= count; i++)
        {
            var index = i % 4;
            index--;
            if (index == -1) index = 3;
            ret[index]++;
        }

        return ret;
    }


    public static void trySetLableText(GameObject father, string name, string text)
    {
        var l = getLabelName(father, name);
        if (l != null)
            l.text = text;
        else
            Program.DEBUGLOG("NO Lable" + name);
    }

    public static string tryGetLableText(GameObject father, string name)
    {
        var l = getLabelName(father, name);
        if (l != null) return l.text;

        return "";
    }

    public static string[] Split(this string str, string s)
    {
        return str.Split(new[] {s}, StringSplitOptions.RemoveEmptyEntries);
    }

    public static void registEvent(GameObject father, string name,
        Action<GameObject, Servant.messageSystemValue> function, Servant.messageSystemValue value, string name2 = "")
    {
        var input = getByName<UIInput>(father, name);
        if (input != null)
        {
            var d = input.gameObject.GetComponent<MonoListenerRMS_ized>();
            if (d == null) d = input.gameObject.AddComponent<MonoListenerRMS_ized>();
            d.actionInMono = function;
            d.value = value;
            input.onSubmit.Clear();
            input.onSubmit.Add(new EventDelegate(d, "function"));
            var btns = getByName<UIButton>(father, name2);
            if (btns != null)
            {
                btns.onClick.Clear();
                btns.onClick.Add(new EventDelegate(d, "function"));
            }

            return;
        }

        var btn = getByName<UIButton>(father, name);
        if (btn != null)
        {
            var d = btn.gameObject.GetComponent<MonoListenerRMS_ized>();
            if (d == null) d = btn.gameObject.AddComponent<MonoListenerRMS_ized>();
            d.actionInMono = function;
            d.value = value;
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(d, "function"));
        }
    }


    public static void registEventbtn(GameObject father, string name, Action function)
    {
        var btn = getByName<UIButton>(father, name);
        if (btn != null)
        {
            var d = btn.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = btn.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(d, "function"));
        }
    }

    public static void registEvent(GameObject father, string name, Action function)
    {
        var slider = getByName<UISlider>(father, name);
        if (slider != null)
        {
            var d = slider.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = slider.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            slider.onChange.Add(new EventDelegate(d, "function"));
            return;
        }

        var list = getByName<UIPopupList>(father, name);
        if (list != null)
        {
            var d = list.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = list.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            list.onChange.Add(new EventDelegate(d, "function"));
            return;
        }

        var tog = getByName<UIToggle>(father, name);
        if (tog != null)
        {
            var d = tog.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = tog.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            tog.onChange.Clear();
            tog.onChange.Add(new EventDelegate(d, "function"));
            return;
        }

        var input = getByName<UIInput>(father, name);
        if (input != null)
        {
            var d = input.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = input.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            input.onSubmit.Clear();
            input.onSubmit.Add(new EventDelegate(d, "function"));
            return;
        }

        var bar = getByName<UIScrollBar>(father, name);
        if (bar != null)
        {
            var d = bar.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = bar.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            bar.onChange.Clear();
            bar.onChange.Add(new EventDelegate(d, "function"));
            return;
        }

        var btn = getByName<UIButton>(father, name);
        if (btn != null)
        {
            var d = btn.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = btn.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(d, "function"));
        }
    }

    public static void addButtonEvent_toolShift(GameObject father, string name, Action function)
    {
        var btn = getByName<UIButton>(father, name);
        if (btn != null)
        {
            var d = btn.gameObject.GetComponent<MonoDelegate>();
            if (d == null) d = btn.gameObject.AddComponent<MonoDelegate>();
            d.actionInMono = function;
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(btn.gameObject.GetComponent<toolShift>(), "shift"));
            btn.onClick.Add(new EventDelegate(d, "function"));
        }
    }

    public static void registClickListener(GameObject father, string name, Action<GameObject> ES_listenerForGameObject)
    {
        var btn = getByName<UIButton>(father, name);
        if (btn != null)
        {
            var d = btn.gameObject.GetComponent<MonoListener>();
            if (d == null) d = btn.gameObject.AddComponent<MonoListener>();
            d.actionInMono = ES_listenerForGameObject;
            btn.onClick.Clear();
            btn.onClick.Add(new EventDelegate(d, "function"));
        }
    }

    public static Vector2 get_hang_lie(int index, int meihangdegeshu)
    {
        var return_value = Vector2.zero;
        return_value.x = index / meihangdegeshu;
        return_value.y = index % meihangdegeshu;
        return return_value;
    }

    internal static Vector2 get_hang_lieArry(int v, int[] hangshu)
    {
        var return_value = Vector2.zero;
        for (var i = 0; i < 4; i++)
            if (v < hangshu[i])
            {
                return_value.x = i;
                return_value.y = v;
                return return_value;
            }
            else
            {
                v -= hangshu[i];
            }

        return return_value;
    }

    public static int get_zuihouyihangdegeshu(int zongshu, int meihangdegeshu)
    {
        var re = 0;
        re = zongshu % meihangdegeshu;
        if (re == 0) re = meihangdegeshu;
        return re;
    }

    public static bool get_shifouzaizuihouyihang(int zongshu, int meihangdegeshu, int index)
    {
        return index / meihangdegeshu == zongshu / meihangdegeshu;
    }

    public static int get_zonghangshu(int zongshu, int meihangdegeshu)
    {
        return (zongshu - 1) / meihangdegeshu + 1;
    }

    public static void registEvent(UIScrollView uIScrollView, Action function)
    {
        uIScrollView.onScrolled = new UIScrollView.OnDragNotification(function);
    }

    public static void registEvent(UIScrollBar scrollBar, Action function)
    {
        var d = scrollBar.gameObject.GetComponent<MonoDelegate>();
        if (d == null) d = scrollBar.gameObject.AddComponent<MonoDelegate>();
        d.actionInMono = function;
        scrollBar.onChange.Clear();
        scrollBar.onChange.Add(new EventDelegate(d, "function"));
    }

    public static void registUIEventTriggerForClick(GameObject gameObject, Action<GameObject> listenerForClicked)
    {
        var boxCollider = gameObject.transform.GetComponentInChildren<BoxCollider>();
        boxCollider.gameObject.name = gameObject.name;
        if (boxCollider != null)
        {
            var uIEventTrigger = boxCollider.gameObject.AddComponent<UIEventTrigger>();
            var d = boxCollider.gameObject.AddComponent<MonoListener>();
            d.actionInMono = listenerForClicked;
            uIEventTrigger.onClick.Add(new EventDelegate(d, "function"));
        }
    }

    public static void registUIEventTriggerForHoverOver(GameObject gameObject, Action<GameObject> listenerForHoverOver)
    {
        var boxCollider = gameObject.transform.GetComponentInChildren<BoxCollider>();
        if (boxCollider != null)
        {
            var uIEventTrigger = boxCollider.gameObject.AddComponent<UIEventTrigger>();
            var d = boxCollider.gameObject.AddComponent<MonoListener>();
            d.actionInMono = listenerForHoverOver;
            uIEventTrigger.onHoverOver.Add(new EventDelegate(d, "function"));
        }
    }

    internal static GameObject getRealEventGameObject(GameObject gameObject)
    {
        GameObject re = null;
        var boxCollider = gameObject.transform.GetComponentInChildren<BoxCollider>();
        if (boxCollider != null) re = boxCollider.gameObject;
        return re;
    }

    internal static GameObject getGameObject(GameObject gameObject, string name)
    {
        var t = getByName<Transform>(gameObject, name);
        if (t != null)
            return t.gameObject;
        return null;
    }

    internal static void trySetLableText(GameObject gameObject, string p)
    {
        try
        {
            gameObject.GetComponentInChildren<UILabel>().text = p;
        }
        catch (Exception)
        {
        }
    }

    internal static void registEvent(GameObject gameObject, Action act)
    {
        registEvent(gameObject, gameObject.name, act);
    }

    internal static void trySetLableTextList(GameObject father, string text)
    {
        try
        {
            var p = father.GetComponentInChildren<UITextList>();
            p.Clear();
            p.Add(text);
        }
        catch (Exception)
        {
            Program.DEBUGLOG("NO LableList");
        }
    }

    internal static int get_decklieshu(int zongshu)
    {
        var return_value = 10;
        for (var i = 0; i < 100; i++)
            if ((zongshu + i) % 4 == 0)
            {
                return_value = (zongshu + i) / 4;
                break;
            }

        return return_value;
    }

    internal static void clearITWeen(GameObject gameObject)
    {
        var iTweens = gameObject.GetComponents<iTween>();
        for (var i = 0; i < iTweens.Length; i++) Object.DestroyImmediate(iTweens[i]);
    }

    internal static float get_left_right_index(float left, float right, int i, int count)
    {
        float return_value = 0;
        if (count == 1)
        {
            return_value = left + right;
            return_value /= 2;
        }
        else
        {
            return_value = left + (right - left) * i / (count - 1);
        }

        return return_value;
    }

    internal static float get_left_right_indexZuo(float v1, float v2, int v3, int count, int v4)
    {
        if (count >= v4)
            return get_left_right_index(v1, v2, v3, count);
        return get_left_right_index(v1, v2, v3, v4);
    }

    internal static float get_left_right_indexEnhanced(float left, float right, int i, int count, int illusion)
    {
        float return_value = 0;
        if (count > illusion)
        {
            if (count == 1)
            {
                return_value = left + right;
                return_value /= 2;
            }
            else
            {
                return_value = left + (right - left) * i / (count - 1);
            }
        }
        else
        {
            if (illusion == 1)
            {
                return_value = left + right;
                return_value /= 2;
            }
            else
            {
                var l = left;
                var r = right;
                var per = (right - left) / (illusion - 1);
                var length = per * (count + 1);
                l = (left + right) / 2f - length / 2f;
                r = (left + right) / 2f + length / 2f;
                return_value = l + per * (i + 1);
            }
        }

        return return_value;
    }

    internal static void registUIEventTriggerForMouseDown(GameObject gameObject,
        Action<GameObject> listenerForMouseDown)
    {
        var boxCollider = gameObject.transform.GetComponentInChildren<BoxCollider>();
        if (boxCollider != null)
        {
            var uIEventTrigger = boxCollider.gameObject.AddComponent<UIEventTrigger>();
            var d = boxCollider.gameObject.AddComponent<MonoListener>();
            d.actionInMono = listenerForMouseDown;
            uIEventTrigger.onPress.Add(new EventDelegate(d, "function"));
        }
    }

    internal static void iniFaces()
    {
        try
        {
            var fileInfos = new DirectoryInfo("texture/face").GetFiles();
            for (var i = 0; i < fileInfos.Length; i++)
                if (fileInfos[i].Name.Length > 4)
                    if (fileInfos[i].Name.Substring(fileInfos[i].Name.Length - 4, 4) == ".png")
                    {
                        var name = fileInfos[i].Name.Substring(0, fileInfos[i].Name.Length - 4);
                        if (!faces.ContainsKey(name))
                            try
                            {
                                faces.Add(name, GetTexture2D("texture/face/" + fileInfos[i].Name));
                            }
                            catch (Exception e)
                            {
                                //Debug.Log(e);
                            }
                    }
        }
        catch (Exception e)
        {
            //Debug.Log(e);
        }
    }

    internal static Texture2D getFace(string name)
    {
        Texture2D re = null;
        if (faces.TryGetValue(name, out re))
            if (re != null)
                return re;
        var buffer = Encoding.UTF8.GetBytes(name);
        var sum = 0;
        for (var i = 0; i < buffer.Length; i++) sum += buffer[i];
        sum = sum % 100;
        return Program.I().face.faces[sum];
    }

    public static Texture2D GetTexture2D(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }
        var pic = new Texture2D(0, 0);
        pic.LoadImage(File.ReadAllBytes(path));
        return pic;
    }

    public static async Task<Texture2D> GetTexture2DAsync(string path)
    {
        var pic = new Texture2D(0, 0);
        // Unity < 2021.2
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
        var data = new byte[stream.Length];
        await stream.ReadAsync(data, 0, (int) stream.Length);
        pic.LoadImage(data);
        // Unity >= 2021.2
        // pic.LoadImage(await File.ReadAllBytesAsync(path));
        return pic;
    }

    public static async Task<Texture2D> GetTexture2DFromZipAsync(ZipFile zip, string file)
    {
        var pic = new Texture2D(0, 0);
        MemoryStream stream = new MemoryStream();
        ZipEntry entry = zip[file];
        entry.Extract(stream);
        //await Task.Run(() => { entry.Extract(stream); });
        pic.LoadImage(stream.ToArray());
        return pic;
    }

    internal static void shiftButton(UIButton btn, bool enabled)
    {
        if (enabled)
            btn.gameObject.transform.localScale = new Vector3(1, 1, 1);
        else
            btn.gameObject.transform.localScale = new Vector3(0, 0, 0);
        //try
        //{
        //    BoxCollider boxCollider = btn.gameObject.GetComponentInChildren<BoxCollider>();
        //    UILabel label = btn.gameObject.GetComponentInChildren<UILabel>();
        //    label.text = hint;
        //    boxCollider.enabled = enabled;
        //    if (enabled)
        //    {
        //        label.color = Color.white;
        //    }
        //    else
        //    {
        //        label.color = Color.gray;
        //    }
        //}
        //catch (Exception)   
        //{
        //}
    }

    internal static void shiftUIToggle(UIToggle tog, bool canClick, bool canChange, string hint)
    {
        try
        {
            tog.canChange = canChange;
            var boxCollider = tog.gameObject.GetComponentInChildren<BoxCollider>();
            var label = tog.gameObject.GetComponentInChildren<UILabel>();
            label.text = hint;
            boxCollider.enabled = canClick;
            if (canClick)
                getByName<UISprite>(tog.gameObject, "Background").color = Color.white;
            //getByName<UISprite>(tog.gameObject, "Checkmark").color = Color.white;
            else
                getByName<UISprite>(tog.gameObject, "Background").color = Color.black;
            //getByName<UISprite>(tog.gameObject, "Checkmark").color = Color.gray;
        }
        catch (Exception)
        {
        }
    }

    internal static string getBufferString(byte[] buffer)
    {
        var returnValue = "";
        foreach (var item in buffer) returnValue += (int) item + ".";
        return returnValue;
    }

    internal static string getTimeString()
    {
        return DateTime.Now.ToString("MM-dd「HH：mm：ss」");
    }

    internal static bool fromStringToBool(string s)
    {
        return s == "1";
    }

    internal static string fromBoolToString(bool s)
    {
        if (s)
            return "1";
        return "0";
    }

    internal static Vector3 getCamGoodPosition(Vector3 v, float l)
    {
        var screenposition = Program.I().main_camera.WorldToScreenPoint(v);
        return Program.I().main_camera.ScreenToWorldPoint(new Vector3(screenposition.x, screenposition.y,
            screenposition.z + l));
    }

    public const string sort = "sortByTimeDeck";

    public static IOrderedEnumerable<FileInfo> SortDeck(this IOrderedEnumerable<FileInfo> source)
    {
        return Config.Get(sort, "1") == "1"
            ? source.ThenByDescending(f => f.LastWriteTime)
            : source.ThenBy(f => f.Name);
    }

    public static IEnumerable<string> GetDecks(string search = "")
    {
        var deckInUse = Config.Get("deckInUse", "miaowu");
        var deckInUsePath = Path.GetFullPath($"deck/{deckInUse}.ydk");
        return new DirectoryInfo("deck").EnumerateFiles("*.ydk", SearchOption.AllDirectories)
            .Where(f => search == "" || f.Name.Contains(search))
            .OrderByDescending(f => f.FullName == deckInUsePath)
            .ThenBy(f => f.DirectoryName)
            .SortDeck()
            .Select(f =>
                f.DirectoryName == Path.GetFullPath("deck")
                    ? Path.GetFileNameWithoutExtension(f.Name)
                    : $"{Path.GetFileName(f.DirectoryName)}/{Path.GetFileNameWithoutExtension(f.Name)}");
    }

    public static int CompareTime(object x, object y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var xInfo = (FileInfo) x;
        var yInfo = (FileInfo) y;
        return yInfo.LastWriteTime.CompareTo(xInfo.LastWriteTime);
    }

    public static int CompareName(object x, object y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;
        var xInfo = (FileInfo) x;
        var yInfo = (FileInfo) y;
        return xInfo.FullName.CompareTo(yInfo.FullName);
    }

    internal static void playSound(string p, float vol)
    {
        if (Ocgcore.inSkiping) return;
        var path = "sound/" + p + ".mp3";
        if (File.Exists(path) == false) path = "sound/" + p + ".wav";
        if (File.Exists(path) == false) path = "sound/" + p + ".ogg";
        if (File.Exists(path) == false) return;
        path = Environment.CurrentDirectory.Replace("\\", "/") + "/" + path;
        path = "file:///" + path;
        var audio_helper = Program.I().ocgcore.create_s(Program.I().mod_audio_effect);
        audio_helper.GetComponent<audio_helper>().play(path, vol* Program.I().setting.soundValue());
        Program.I().destroy(audio_helper, 10f);
    }
   
    internal static string getGPSstringLocation(GPS p1)
    {
        var res = "";
        if (p1.controller == 0)
            res += "";
        else
            res += InterString.Get("对方");
        if ((p1.location & (uint) CardLocation.Deck) > 0) res += InterString.Get("卡组");
        if ((p1.location & (uint) CardLocation.Extra) > 0) res += InterString.Get("额外");
        if ((p1.location & (uint) CardLocation.Grave) > 0) res += InterString.Get("墓地");
        if ((p1.location & (uint) CardLocation.Hand) > 0) res += InterString.Get("手牌");
        if ((p1.location & (uint) CardLocation.MonsterZone) > 0) res += InterString.Get("前场");
        if ((p1.location & (uint) CardLocation.Removed) > 0) res += InterString.Get("除外");
        if ((p1.location & (uint) CardLocation.SpellZone) > 0) res += InterString.Get("后场");
        return res;
    }

    //internal static string getGPSstringPosition(GPS p1) 
    //{
    //    string res = "";
    //    if ((p1.location & (UInt32)CardLocation.Overlay) > 0)
    //    {
    //        res += InterString.Get("(被叠放)");
    //    }
    //    else
    //    {
    //        if ((p1.position & (UInt32)CardPosition.FaceUpAttack) > 0)
    //        {
    //            res += InterString.Get("(表侧攻击)");
    //        }
    //        else if ((p1.position & (UInt32)CardPosition.FaceUp_DEFENSE) > 0)
    //        {
    //            res += InterString.Get("(表侧守备)");
    //        }
    //        else if ((p1.position & (UInt32)CardPosition.FaceDownAttack) > 0)
    //        {
    //            res += InterString.Get("(里侧攻击)");
    //        }
    //        else if ((p1.position & (UInt32)CardPosition.FaceDown_DEFENSE) > 0)
    //        {
    //            res += InterString.Get("(里侧守备)");
    //        }
    //        else if ((p1.position & (UInt32)CardPosition.Attack) > 0)
    //        {
    //            res += InterString.Get("(攻击)");
    //        }
    //        else if ((p1.position & (UInt32)CardPosition.POS_DEFENSE) > 0)
    //        {
    //            res += InterString.Get("(守备)");
    //        }
    //        else if ((p1.position & (UInt32)CardPosition.FaceUp) > 0)
    //        {
    //            res += InterString.Get("(表侧)");
    //        }
    //        else if ((p1.position & (UInt32)CardPosition.POS_DEFENSE) > 0)
    //        {
    //            res += InterString.Get("(里侧)");
    //        }
    //    }

    //    return res;
    //}

    internal static string getGPSstringName(gameCard card, bool green = false)
    {
        var res = "";
        res += getGPSstringLocation(card.p) + "\n「" + getSuperName(card.get_data().Name, card.get_data().Id) + "」";
        if (green) return "[00ff00]" + res + "[-]";
        return res;
    }

    internal static string getSuperName(string name, int code)
    {
        var res = "";
        res = "[url=" + code + "][u]" + name + "[/u][/url]";
        return res;
    }

    internal static string getDName(string name, int code)
    {
        var res = "";
        res = "「[url=" + code + "][u]" + name + "[/u][/url]」";
        return res;
    }

    internal static float getScreenDistance(GameObject a, GameObject b)
    {
        var sa = Program.I().main_camera.WorldToScreenPoint(a.transform.position);
        sa.z = 0;
        var sb = Program.I().main_camera.WorldToScreenPoint(b.transform.position);
        sb.z = 0;
        return Vector3.Distance(sa, sb);
    }

    internal static void setParent(GameObject child, GameObject parent)
    {
        child.transform.SetParent(parent.transform, true);
        var Transforms = child.GetComponentsInChildren<Transform>();
        foreach (var achild in Transforms)
            achild.gameObject.layer = parent.layer;
    }

    internal static Vector3 get_close(Vector3 input_vector, Camera cam, float l)
    {
        var o = Vector3.zero;
        var scr = cam.WorldToScreenPoint(input_vector);
        scr.z -= l;
        o = cam.ScreenToWorldPoint(scr);
        return o;
    }
}