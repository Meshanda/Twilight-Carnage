using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GenericEnemyBehaviourSO EnemyBehaviourSO;
    [SerializeField] private GenericEnemyStatSO EnemyStatSO;

    private GameObject[] Players;
    private GameObject Target;

    private void Start()
    {
        //Players = GameObject.FindGameObjectsWithTag("Player");
        if(NetworkManager.Singleton.IsServer)
         StartCoroutine(ChoseTarget());
    }

    public void SetPlayer(GameObject[] players) 
    {
        Players = players;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            transform.LookAt(Target.transform);
            transform.position += transform.forward * Time.deltaTime * EnemyStatSO.BaseMovespeed;
        }
    }

    IEnumerator ChoseTarget()
    {
        while (true)
        {
            Target = EnemyBehaviourSO.ChoseTargettedPlayer(Players, gameObject);
            yield return new WaitForSeconds(2f);
        }
    }
}
