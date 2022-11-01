using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemieWave : MonoBehaviour
{
    [SerializeField] private List<tableEnemiStat> table;
    [SerializeField] private float _betweenObject;
    private List<GameObject> _toSpawn;
    
    public void SpawnPool(int budget) //add what gameobject to spawn with the current budget
    {

        _toSpawn = FillToSpawn(budget);
       Spawn();

    }

    private List<GameObject> FillToSpawn(int budget) 
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = table.Count - 1; i >= 0; i--)
        {
            if (budget >= table[i].cost)
            {
                list.Add(table[i].enemie);
                list.AddRange(FillToSpawn(budget - table[i].cost));
                break;
            }
        }
        return list;
    }
    private void Spawn()
    {
        Debug.Log(_toSpawn.Count);
        //Debug.Log(nbreWave);
        for (int i = 0; i < _toSpawn.Count; i++)
        {
            Vector3 offSet = Vector3.zero;
            int direction = 1;

            if (i % 2 == 0)
                direction = -1;

            if (i != 0)
            {
                offSet = transform.right;
            }

            offSet = offSet * direction * Mathf.RoundToInt((i / 2f) + 0.1f) * _betweenObject ;
            Instantiate(_toSpawn[i], transform.position + offSet, transform.rotation, transform);
        }
    }

}

[Serializable]
public struct tableEnemiStat 
{
    public GameObject enemie;
    public float hp;
    public float speed;
    public int cost;

}
