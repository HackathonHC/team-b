using UnityEngine;
using System.Runtime.InteropServices;

static public class Lang {
    [DllImport("__Internal")]
    private static extern string CurrentLanguage_();
    private const string SimplifiedChineseIOS = "zh-Hans";
    private const string SimplifiedChineseAndroid = "zh_CN";


    public static string Current() 
    {
#if UNITY_IOS
        /*
            zh-Hans　＝簡体字
            zh-Hant　＝繁体字
        */
        if (Application.platform != RuntimePlatform.OSXEditor) 
        {
            return CurrentLanguage_();
        } 
        else 
        {
            return "";
        }
#elif UNITY_ANDROID
        /*
            zh_CN　＝簡体字
            zh_TW　＝繁体字
        */

        AndroidJavaClass locale = new AndroidJavaClass("java.util.Locale");
        AndroidJavaObject current = locale.CallStatic<AndroidJavaObject>("getDefault");
        return current.Call<string>("toString");
#endif
        return null;
    }

    public static bool IsSimplifiedChinese()
    {
#if UNITY_IOS
        return Current() == SimplifiedChineseIOS;
#elif UNITY_ANDROID
        return Current() == SimplifiedChineseAndroid;
#else
        return false;
#endif
    }


    public static string GetLanguage()
    {
        string laungage = "";
        switch (Application.systemLanguage)
        {
        case SystemLanguage.Chinese:
            if (Lang.IsSimplifiedChinese())
            {
                laungage = SystemLanguage.Chinese.ToString();
            }
            else
            {
                laungage = "Taiwan";
            }
            break;
            
        case SystemLanguage.English:
            laungage = Application.systemLanguage.ToString();
            break;
        default:
            laungage = SystemLanguage.Japanese.ToString();
            break;
        }
        return laungage;
    }

    // for ngui
    public static void InitializeLocalization() 
    {
        if (PlayerPrefs.GetString("Language", "") == "")
        {
            PlayerPrefs.SetString("Language", GetLanguage());
        }
    }
}
