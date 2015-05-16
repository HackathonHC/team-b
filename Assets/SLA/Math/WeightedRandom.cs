using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLA
{
    public class WeightedRandom<T>
    {
        List<int> mMaxKeys;
        List<T> mItems;
        
        public WeightedRandom()
        {
            mMaxKeys = new List<int>();
            mItems = new List<T>();
        }

        public WeightedRandom(Dictionary<T, int> table)
        {
            mMaxKeys = new List<int>();
            mItems = new List<T>();

            foreach(var pair in table)
            {
                Add (pair.Value, pair.Key);
            }
        }

        public WeightedRandom(int capacity)
        {
            mMaxKeys = new List<int>(capacity);
            mItems = new List<T>(capacity);
        }
        
        public void Add(int weight, T item){
            if (weight > 0)
            {
                if (mMaxKeys.Count > 0)
                {
                    mMaxKeys.Add(mMaxKeys[mMaxKeys.Count - 1] + weight);
                }
                else{
                    mMaxKeys.Add(weight);
                }
                
                mItems.Add(item);
            }
        }
        
        public T Select()
        {
            int key = Random.Range(0, mMaxKeys[mMaxKeys.Count - 1]) + 1;
            int index = mMaxKeys.BinarySearch(key);
            if (index < 0)
            {
                index = ~index;
            }
            return mItems[index];
        }

        public float Probability(T t)
        {
            float targetKeyLength = 0f;
            for(int i=0 ; i<mItems.Count ; ++i)
            {
                if (mItems[i].Equals(t))
                {
                    if (i == 0)
                    {
                        targetKeyLength += mMaxKeys[i];
                    }
                    else
                    {
                        targetKeyLength += mMaxKeys[i] - mMaxKeys[i - 1];
                    }
                }
            }
            return targetKeyLength / mMaxKeys[mMaxKeys.Count - 1];
        }
    }
}
