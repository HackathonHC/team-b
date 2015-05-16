using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public static class RandomUtil
    {
        static public void Shuffle<T>( ref List<T> list )
        {
            for(int i=0 ; i<list.Count; ++i)
            {
                int j = Random.Range(0, list.Count);
                if (i != j)
                {
                    var tmp = list[i];
                    list[i] = list[j];
                    list[j] = tmp;
                }
            }
        }
    }
}
