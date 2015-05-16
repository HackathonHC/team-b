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

        void Start()
        {
            Instantiate(_personPrefab);
            Instantiate(_tetrisPlayerPrefab);
        }
    }
}
