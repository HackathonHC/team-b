using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class ObjectPool
    {
        static ObjectPool _instance;
        static public ObjectPool Instance
        {
            get
            {
                return _instance ?? (_instance = new ObjectPool());
            }
        }

        ObjectPool()
        {
        }

        Dictionary<string, Stack<GameObject>> _objects = new Dictionary<string, Stack<GameObject>>();

        public void Push(string name, GameObject obj)
        {
            if (!_objects.ContainsKey(name))
            {
                _objects.Add(name, new Stack<GameObject>());
            }
            _objects[name].Push(obj);
            obj.SetActive(false);
        }

        public GameObject Pop(string name)
        {
            Stack<GameObject> stack;
            if (_objects.TryGetValue(name, out stack))
            {
                while(stack.Count > 0)
                {
                    var obj = stack.Pop();
                    if (!obj)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
    }
}
