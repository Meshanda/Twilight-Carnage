using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GenericEnemyBehaviourSO EnemyBehaviourSO;
    [SerializeField] private GenericEnemyStatSO EnemyStatSO;

    private GameObject[] Players;
    private GameObject Target;

    private void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ChoseTarget());
        if (Target)
        {
            transform.LookAt(Target.transform);
            transform.position += transform.forward * Time.deltaTime * EnemyStatSO.BaseMovespeed;
        }
    }

    IEnumerator ChoseTarget()
    {
        Target = EnemyBehaviourSO.ChoseTargettedPlayer(Players, gameObject);
        yield return new WaitForSeconds(2f);
    }
}
