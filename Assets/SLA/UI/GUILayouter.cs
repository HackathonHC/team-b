using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SLA
{
    public class GUILayouter : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        public class GUI
        {
            public Transform target;
            public Vector2 position;
            public Vector2 size;
            public Canvas canvas;

            public void Execute()
            {
                var targetCanvas = canvas;
                if (targetCanvas == null)
                {
                    targetCanvas = target.GetComponentInParent<Canvas>();
                }
                var rect = targetCanvas.GetComponent<RectTransform>();

                var rectTransform = target as RectTransform;

                Vector3 pos;
                if (rectTransform != null)
                {
                    pos = position + new Vector2(rectTransform.pivot.x * size.x, (1f - rectTransform.pivot.y) * size.y);
                }
                else
                {
                    pos = position + size / 2f;
                }
                Vector3[] corners = new Vector3[4];
                rect.GetLocalCorners(corners);
                var localSize = corners[2] - corners[0];
                pos.x -= localSize.x / 2f;
                pos.y -= localSize.y / 2f;
                pos.y *= -1f;

                var screenPosition = targetCanvas.transform.localToWorldMatrix.MultiplyPoint3x4(pos);

                if (rectTransform != null)
                {
                    target.position = screenPosition;
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                }
                else
                {
                    var worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
                    target.position = new Vector3(worldPos.x, worldPos.y, target.position.z);
                }

                Debug.Log("layout done : " + target.gameObject.name);
            }
        }

        [SerializeField]
        GUI[] guis;

        public void Execute()
        {
            foreach(var it in guis)
            {
                it.Execute();
            }
            Debug.Log("layout done all");
        }

        void Start()
        {
            if (gameObject.tag != "EditorOnly")
            {
                Debug.LogWarning("GUILayouterはEditorOnlyにしてください");
            }
        }
#endif
    }
}

