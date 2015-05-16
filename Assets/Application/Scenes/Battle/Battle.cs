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
            Instantiate(_tetrisPlayerPrefab);
            var fieldObject = Instantiate(_fieldPrefab) as GameObject;
            fieldObject.GetComponent<Field>().Initialize(10, 9, 1);
        }
    }
}
