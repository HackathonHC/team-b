using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Result : MonoBehaviour
    {
        [SerializeField]
        GameObject _win;

        [SerializeField]
        GameObject _lose;

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void TrySet(ResultType result)
        {
            if (gameObject.activeSelf)
            {
                return;
            }

            gameObject.SetActive(true);

            if ((result == ResultType.DiggerWin) == (GameData.Instance.playerType == PlayerType.Digger))
            {
                _win.SetActive(true);
                _lose.SetActive(false);
            }
            else
            {
                _win.SetActive(false);
                _lose.SetActive(true);
            }
        }

        public void OnClickBackToTitleButton()
        {
            SLA.PhotonManager.Instance.Disconnect(null);
            Application.LoadLevel("Title");
        }
    }
}
