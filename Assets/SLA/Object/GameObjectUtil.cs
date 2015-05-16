using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public static class GameObjectUtil 
    {
        static public GameObject FindNearestObject(ICollection<GameObject> objects, Vector3 pos)
        {
            float minSqrDistance = float.MaxValue;
            GameObject result = null;
            foreach(var it in objects)
            {
                if (it)
                {
                    var distance = pos - it.transform.position;
                    float currentSqrDistance = distance.sqrMagnitude;
                    if (result == null || minSqrDistance > currentSqrDistance)
                    {
                        result = it;
                        minSqrDistance = currentSqrDistance;
                    }
                }
            }
            
            return result;
        }

        static public T FindNearestObject<T>(ICollection<T> objects, Vector3 pos)
            where T : Component
        {
            float minSqrDistance = float.MaxValue;
            T result = null;
            foreach(T it in objects)
            {
                if (it)
                {
                    var distance = pos - it.transform.position;
                    float currentSqrDistance = distance.sqrMagnitude;
                    if (result == null || minSqrDistance > currentSqrDistance)
                    {
                        result = it;
                        minSqrDistance = currentSqrDistance;
                    }
                }
            }

            return result;
        }

        static public T FindNearestObject<T>(ICollection<T> objects, Vector3 pos, Bounds targetArea)
            where T : Component
        {
            float minSqrDistance = float.MaxValue;
            T result = null;
            foreach(T it in objects)
            {
                if (it)
                {
                    if (targetArea.Contains(it.transform.position))
                    {
                        var distance = pos - it.transform.position;
                        float currentSqrDistance = distance.sqrMagnitude;
                        if (result == null || minSqrDistance > currentSqrDistance)
                        {
                            result = it;
                            minSqrDistance = currentSqrDistance;
                        }
                    }
                }
            }
            
            return result;
        }

        static public T FindNearestObject<T>(ICollection<T> objects, Ray ray, float maxAngle)
            where T : Component
        {
            float minSqrDistance = float.MaxValue;
            T result = null;
            foreach(T it in objects)
            {
                if (it)
                {
                    var distance = it.transform.position - ray.origin;
                    if (Vector3.Angle(ray.direction, distance) <= maxAngle)
                    {
                        float currentSqrDistance = distance.sqrMagnitude;
                        if (result == null || minSqrDistance > currentSqrDistance)
                        {
                            result = it;
                            minSqrDistance = currentSqrDistance;
                        }
                    }
                }
            }
            return result;
        }

        static public List<GameObject> FindChildrenWithLayer(GameObject parent, int layerMask)
        {
            var result = new List<GameObject>();
            if (((1 << parent.layer) & layerMask) != 0)
            {
                result.Add(parent);
            }
            foreach(Transform child in parent.transform)
            {
                result.AddRange(FindChildrenWithLayer(child.gameObject, layerMask));
            }
            return result;
        }
    }
}
