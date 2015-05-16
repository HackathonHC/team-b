using UnityEngine;
using System.Collections;

namespace TB.Battles
{
    public class Battle : MonoBehaviour
    {
        [SerializeField]
        GameObject _personPrefab;

        [SerializeField]
        GameObject _tetrisPlayerPrefab;

        [SerializeField]
        GameObject _fieldPrefab;

        void Start()
        {
            Instantiate(_personPrefab);
            var field = Instantiate(_fieldPrefab).GetComponent<Field>();
            field.Initialize(10, 9, 1);
            var tetrisPlayer = Instantiate(_tetrisPlayerPrefab).GetComponent<TetrisPlayer>();
            tetrisPlayer.Initialize(field, 5);
        }
    }
}
