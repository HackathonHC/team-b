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

        [SerializeField]
        UnityEngine.RectTransform _mask;

        float _width = 0f;

        public void SetValue(float value)
        {
            if (_width == 0f)
            {
                var corners = new Vector3[4];
                _mask.GetLocalCorners(corners);
                _width = corners[2].x - corners[0].x;
            }
            _mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value * _width);
        }
    }
}
