using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

namespace SLA
{
    public static class SystemUtil
    {
        static public string SystemVersion()
        {
            var osStr = SystemInfo.operatingSystem;
            var result = System.Text.RegularExpressions.Regex.Match(osStr, @"\b\d+(\.\d+)*\b");
            if (result.Success)
            {
                return result.Value;
            }
            else
            {
                return null;
            }
        }

        static public int[] SplitSystemVersions()
        {
            string[] versions = SystemVersion().Split('.');
            if (versions != null)
            {
                int[] result = new int[versions.Length];
                for(int i=0 ; i<versions.Length ; ++i)
                {
                    result[i] = int.Parse(versions[i]);
                }
                return result;
            }
            else{
                return null;
            }
        }

        static public int AndroidAPILevel()
        {
    #if !UNITY_EDITOR && UNITY_ANDROID
            var osStr = SystemInfo.operatingSystem;
            var result = System.Text.RegularExpressions.Regex.Match(osStr, @"API-\d+");
            if (result.Success)
            {
                return int.Parse(result.Value.Substring(4));
            }
            else
            {
                return 0;
            }
    #else
            return 0;
    #endif
        }
    }
}
