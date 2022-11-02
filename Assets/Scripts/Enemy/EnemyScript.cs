using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyScript : NetworkBehaviour
{
    [SerializeField] private GenericEnemyBehaviourSO EnemyBehaviourSO;
    [SerializeField] private GenericEnemyStatSO EnemyStatSO;
    [SerializeField] private float TimeBetweenRetarget;

    private NetworkVariable<float> Health = new NetworkVariable<float>();
    private GameObject[] Players;
    private GameObject Target;
    private Transform thisTransform;
    private void Start()
    {
        thisTransform = transform;
        if (NetworkManager.Singleton.IsServer)
        {
            Health.Value = EnemyStatSO.MaxHealth;   
        }
        Players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(ChoseTarget());
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            thisTransform.LookAt(Target.transform);
            thisTransform.position += thisTransform.forward * (EnemyStatSO.Movespeed * Time.deltaTime);
        }
        Debug.Log("Remaining Health : " + Health.Value);
    }

    public void TakeDamage(float Hit)
    {
        Health.Value -= Hit;
        if (Health.Value <= 0)
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
