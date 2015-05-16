using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class Location : MonoBehaviour 
    {
        public static Location Instance {get; private set;}
        public LocationInfo locationInfo;
        private System.Action<bool> _onFinishedSearching;

        void Awake()
        {
            Instance = this;
        }

        public void Search(System.Action<bool> callback=null)
        {
            if (Input.location.isEnabledByUser) 
            {
                _onFinishedSearching = callback;
                StartCoroutine(GetLocation());
            }
            else
            {
                print("not enable");
                if (callback != null)
                {
                    callback(false);
                }
            }
        }

        IEnumerator GetLocation()
        {
            // ロケーションを問合せる前にInput.locationをスタート
            Input.location.Start();
            Debug.Log("location.Start()");

            // 初期化できるまで待つ
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) 
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            // Service didn't initialize in 20 seconds
            if (maxWait < 1)
            {
                print("Timed out");
                if (_onFinishedSearching != null)
                {
                    _onFinishedSearching(false);
                }
                yield break;
            }
            Debug.Log("service initializes --> success");

            // 接続状況
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                print("Unable to determine device location");
                if (_onFinishedSearching != null)
                {
                    _onFinishedSearching(false);
                }
                yield break;
            }
            else
            {
                // Access granted and location value could be retrieved
                // latitude 緯度
                // longitude 経度
                // altitude 標高
                // horizontalAccuracy 水平精度
                // verticalAccuracy 垂直精度
                // timestamp タイムスタンプ
                Debug.Log("Location: " +
                          Input.location.lastData.latitude + " " + 
                          Input.location.lastData.longitude + " " +
                          Input.location.lastData.altitude + " " +
                          Input.location.lastData.horizontalAccuracy + " " +
                          Input.location.lastData.verticalAccuracy + " " +
                          Input.location.lastData.timestamp);
                locationInfo = Input.location.lastData;
            }
            Debug.Log("Connection --> OK");

            // Stop service if there is no need to query location updates continuously
            Input.location.Stop();    
            Debug.Log("Service --> Stop");

            if (_onFinishedSearching != null)
            {
                _onFinishedSearching(true);
            }
        }
    }
}
