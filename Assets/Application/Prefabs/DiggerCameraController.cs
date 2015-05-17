using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class DiggerCameraController : MonoBehaviour
    {
        const float Offset = 1f;

        void Start()
        {
            if (GameData.Instance.playerType != PlayerType.Digger)
            {
                enabled = false;
            }
            transform.position = new Vector3(transform.position.x, 100f, transform.position.z);
        }

        void LateUpdate()
        {
            float y = Bomber.Instance.transform.position.y + Offset;
            if (transform.localPosition.y > y)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
            }
        }
    }
}
