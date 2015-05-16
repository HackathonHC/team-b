using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class Graph<T>
    {
        public Dictionary<T, Dictionary<T, int>> edgeWeights = new Dictionary<T, Dictionary<T, int>>();

        public void Link(T from, T to, int weight)
        {
            if (!edgeWeights.ContainsKey(from))
            {
                edgeWeights.Add(from, new Dictionary<T, int>());
            }
            edgeWeights[from].Add(to, weight);
        }

        public void DoubleOrderedUnlink(T a, T b)
        {
            Unlink(a, b);
            Unlink(b, a);
        }

        public void Unlink(T a, T b)
        {
            if (edgeWeights.ContainsKey(a))
            {
                if (edgeWeights[a].ContainsKey(b))
                {
                    edgeWeights[a].Remove(b);
                }
            }
        }

        public void Remove(T t)
        {
            if (edgeWeights.ContainsKey(t))
            {
                foreach(var nextNodes in edgeWeights[t].Keys)
                {
                    edgeWeights[nextNodes].Remove(t);
                }
                edgeWeights.Remove(t);
            }
        }

        public Graph<T> Clone()
        {
            var result = new Graph<T>();
            result.edgeWeights = new Dictionary<T, Dictionary<T, int>>(edgeWeights.Count);
            foreach(var it in edgeWeights)
            {
                result.edgeWeights.Add(it.Key, new Dictionary<T, int>(it.Value));
            }
            return result;
        }

        public DistanceMap<T> ComputeDistancesFrom(T pivot)
        {
            var result = new DistanceMap<T>();
            result.distanceMap = new Dictionary<T, int>(edgeWeights.Count);
            result.routes = new Dictionary<T, List<T>>(edgeWeights.Count);
            result.pivot = pivot;

            var tasks = new SortedDictionary<int, Queue<T>>();
            tasks.Add(0, new Queue<T>());
            tasks[0].Enqueue(pivot);

            result.distanceMap.Add(pivot, 0);

            while(tasks.Count > 0)
            {
                var e = tasks.GetEnumerator();
                e.MoveNext();
                var first = e.Current;
                T from = first.Value.Dequeue();
                if (first.Value.Count == 0)
                {
                    tasks.Remove(first.Key);
                }
                
                foreach(var to in edgeWeights[from])
                {
                    int newDistance = result.distanceMap[from] + to.Value;

                    bool refresh = true;
                    if (result.distanceMap.ContainsKey(to.Key))
                    {
                        int oldDistance = result.distanceMap[to.Key];
                        refresh = (newDistance < oldDistance);

                        if (newDistance == oldDistance)
                        {
                            result.routes[to.Key].Add(from);
                        }
                    }

                    if (refresh)
                    {
                        if (!result.routes.ContainsKey(to.Key))
                        {
                            result.routes.Add(to.Key, new List<T>{from});
                        }
                        else
                        {
                            result.routes[to.Key].Clear();
                            result.routes[to.Key].Add(from);
                        }

                        if (!result.distanceMap.ContainsKey(to.Key))
                        {
                            result.distanceMap.Add(to.Key, newDistance);
                        }
                        else
                        {
                            result.distanceMap[to.Key] = newDistance;
                        }
                        
                        if (!tasks.ContainsKey(newDistance))
                        {
                            tasks.Add(newDistance, new Queue<T>());
                        }
                        tasks[newDistance].Enqueue(to.Key);
                    }
                }
            }
            return result;
        }
    }
}
