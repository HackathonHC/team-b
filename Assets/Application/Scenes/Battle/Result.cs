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

        bool _isOver = false;

        void Start()
        {
            gameObject.SetActive(false);
        }

        public bool IsOver
        {
            get
            {
                return _isOver;
            }
        }

        public void TrySet(ResultType result)
        {
            if (IsOver)
            {
                return;
            }
            _isOver = true;
            Battle.Instance.StartCoroutine(SetCoroutine(result));
        }

        IEnumerator SetCoroutine(ResultType result)
        {
            yield return new WaitForSeconds(2f);

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
