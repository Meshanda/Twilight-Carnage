using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy/EnemyBehaviour/BaseEnemy", fileName = "New BaseEnemyBehaviour")]
    public class BaseEnemyBehaviour : GenericEnemyBehaviourSO
    {
        public override GameObject ChoseTargettedPlayer(GameObject[] Players, GameObject Enemy)
        {
            float currentSmallestDistance = float.MaxValue;
            int smallestDistanceIndex = 0;
            for (int i = 0; i < Players.Length; i++)
            {
                if (currentSmallestDistance < Vector3.Distance(Enemy.transform.position, Players[i].transform.position))
                {
                    currentSmallestDistance = Vector3.Distance(Enemy.transform.position, Players[i].transform.position);
                    smallestDistanceIndex = i;
                }
            }
            return Players[smallestDistanceIndex];
        }
    }

}