using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GenericEnemyBehaviourSO EnemyBehaviourSO;
    [SerializeField] private GenericEnemyStatSO EnemyStatSO;
    [SerializeField] private float TimeBetweenRetarget;

    private float Health;
    private GameObject[] Players;
    private GameObject Target;

    private void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(ChoseTarget());
        Health = EnemyStatSO.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            transform.LookAt(Target.transform);
            transform.position += transform.forward * Time.deltaTime * EnemyStatSO.Movespeed;
        }
        Debug.Log("Remaining Health : " + Health);
    }

    public void TakeDamage(float Hit)
    {
        Health -= Hit;
        if (Health <= 0)
        {
            Death();
        }
    }
    
    void Death()
    {
        //TODO: Implement xp
        Debug.Log("Enemy is Dead.");
        Destroy(gameObject);
    }

    IEnumerator ChoseTarget()
    {
        while (true)
        {
            Target = EnemyBehaviourSO.ChoseTargettedPlayer(Players, gameObject);
            yield return new WaitForSeconds(TimeBetweenRetarget);
        }
    }
}
