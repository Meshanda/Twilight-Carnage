using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    public abstract class GenericItemSO : ScriptableObject
    { 
        [SerializeField] private GameObject _itemPrefab;

        public abstract void ItemFunction();

        public GameObject GetItemPrefab()
        {
            return _itemPrefab;
        }
    }
}

