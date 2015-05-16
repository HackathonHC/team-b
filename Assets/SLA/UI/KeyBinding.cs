using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SLA
{
    [RequireComponent(typeof(Button))]
    public class KeyBinding : MonoBehaviour
    {
        [SerializeField]
        KeyCode keyCode;

        Button _button;
        Button Button
        {
            get
            {
                return _button ?? (_button = GetComponent<Button>());
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(keyCode))
            {
                Button.onClick.Invoke();
            }
        }
    }
}
