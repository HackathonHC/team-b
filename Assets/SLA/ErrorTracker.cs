using UnityEngine;
using System.Collections;

public class ErrorTracker : MonoBehaviour
{ 

// GooglePlayAnalyticsを入れたら有効にする

#if false
    #if !UNITY_EDITOR
    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }
    
    void HandleLog(string condition, string stackTrace, LogType type)
    {
        if (GoogleAnalyticsV3.getInstance() != null)
        {
            switch(type)
            {
            case LogType.Exception:
                GoogleAnalyticsV3.getInstance().LogException(condition + "," + stackTrace, true);
                break;
            case LogType.Error:
                GoogleAnalyticsV3.getInstance().LogException(condition + "," + stackTrace, false);
                break;
            default:
                break;
            }
        }
    }
    #endif
#endif
}
