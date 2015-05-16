using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Gauge : MonoBehaviour
    {
        Transform _valueObject;
        Transform ValueObject
        {
            get
            {
                return _valueObject ?? (_valueObject = this.transform.Find("Value"));
            }
        }


        public void SetValue(float value)
        {
            ValueObject.localScale = new Vector3(Mathf.Clamp01(value), ValueObject.transform.localScale.y, ValueObject.transform.localScale.z);
        }
    }
}
