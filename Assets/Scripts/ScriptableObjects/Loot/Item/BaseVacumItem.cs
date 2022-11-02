using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Loot/Item/BaseVacumItem", fileName = "New BaseVacumItem")]
    public class BaseVacumItem : GenericItemSO
    {
        [SerializeField] private float _vacumDuration;
        [SerializeField] private float _vacumRange;
        public override void ItemFunction()
        {
            //TODO: Do suck function
        }
    }
}

