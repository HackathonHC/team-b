using UnityEngine;
using System.Collections;

namespace SLA
{
    public class HandheldUtil
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        private static AndroidJavaObject unityPlayer;
        private static AndroidJavaObject currentActivity;
        
        private static AndroidJavaObject vibrator;
        private static int frame = 0;
        
        private static void Initialize() {
            if (unityPlayer == null)
            {
                unityPlayer = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
                currentActivity = unityPlayer.GetStatic<AndroidJavaObject>( "currentActivity" );
                
                vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
            }
        }
        public static void Destruct() {
            vibrator.Dispose();
            
            currentActivity.Dispose();
            unityPlayer.Dispose();
        }
#else
        private static void Initialize() {
        }
        public static void Destruct() {
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        public static void vibrate(long msec) {
            Initialize();
            if (frame != Time.frameCount)
            {
                frame = Time.frameCount;
                vibrator.Call("vibrate", msec);
            }
        }
#else
        public static void vibrate(long msec) {
        }
#endif
    }
}
