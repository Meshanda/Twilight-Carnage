using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Loot/Item/BaseXPItem", fileName = "New BaseXPItem")]
    public class BaseXPItem : GenericItemSO
    {
        [SerializeField] private float _baseXPValue;
        
        public override void ItemFunction()
        {
            //TODO: do something give xp
        }
    }
}

