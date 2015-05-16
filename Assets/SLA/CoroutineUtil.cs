using UnityEngine;
using System.Collections;

namespace SLA
{
    static public class CoroutineUtil
    {
        // yield return StartCoroutine(CoroutineUtil.WaitForRealTimeSeconds(1f)) のように使う。
        // yield return CoroutineUtil.WaitForRealTimeSeconds(1f) ではないので注意。
        static public IEnumerator WaitForRealTimeSeconds(float duration)
        {
            var startedTime = Time.realtimeSinceStartup;
            yield return null;

            while(Time.realtimeSinceStartup < startedTime + duration)
            {
                yield return  null;
            }
        }

        static public IEnumerator WaitForFixedTimeSeconds(float duration)
        {
            var startedTime = Time.fixedTime;
            yield return null;
            
            while(Time.fixedTime < startedTime + duration)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
