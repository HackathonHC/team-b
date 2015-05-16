using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class DistanceMap<T>
    {
        public Dictionary<T, int> distanceMap;
        public Dictionary<T, List<T>> routes;
        public T pivot;
        
        public List<T> ComputeRouteTo(T start)
        {
            if (routes.ContainsKey(start))
            {
                var result = new List<T>();
                var current = start;
                while(current != null)
                {
                    result.Add(current);
                    if (!routes.ContainsKey(current))
                    {
                        break;
                    }
                    var list = routes[current];
                    current = list[Random.Range(0, list.Count)];
                }

                result.Reverse();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
