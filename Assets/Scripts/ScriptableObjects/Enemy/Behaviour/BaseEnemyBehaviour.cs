using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Enemy/EnemyBehaviour/BaseEnemy", fileName = "New BaseEnemyBehaviour")]
    public class BaseEnemyBehaviour : GenericEnemyBehaviourSO
    {
        public override GameObject ChoseTargettedPlayer(GameObject[] players, GameObject enemy)
        {
            
            float currentSmallestDistance = float.MaxValue;
            int smallestDistanceIndex = 0;
            for (int i = 0; i < players.Length; i++)
            {
<<<<<<< HEAD
                if (currentSmallestDistance  > Vector3.Distance(enemy.transform.position, players[i].transform.position))
=======

                if (currentSmallestDistance > Vector3.Distance(Enemy.transform.position, Players[i].transform.position))
>>>>>>> dev
                {
                    currentSmallestDistance = Vector3.Distance(enemy.transform.position, players[i].transform.position);
                    smallestDistanceIndex = i;
                }
            }
            return players[smallestDistanceIndex];
        }
    }

}