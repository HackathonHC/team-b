using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace SLA
{
    public class ScreenshotManager : MonoBehaviour
    {
        #if UNITY_EDITOR

        [SerializeField] int width = 640;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Directory.CreateDirectory("screenshots");
                string t = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                string path = "screenshots/" + t + ".png";
                Application.CaptureScreenshot(path, width / Screen.width);

                Debug.Log("Screenshot saved as " + path);
            }
        }

        #endif
    }
}
