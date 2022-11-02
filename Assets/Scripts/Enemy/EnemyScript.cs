using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.Serialization;

public class EnemyScript : NetworkBehaviour
{
    [SerializeField] private GenericEnemyBehaviourSO enemyBehaviourSO;
    [SerializeField] private GenericEnemyStatSO enemyStatSO;
    [SerializeField] private GenericLootTableSO enemyLootTableSO;
    [SerializeField] private float timeBetweenRetarget;

    private NetworkVariable<float> _health = new NetworkVariable<float>();
    private GameObject[] _players;
    private GameObject _target;
    private Transform _thisTransform;
    private void Start()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        if(NetworkManager.Singleton.IsServer)
         StartCoroutine(ChoseTarget());
    }

    public void SetPlayer(GameObject[] players) 
    {
        _players = players;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target)
        {
            _thisTransform.LookAt(_target.transform);
            _thisTransform.position += _thisTransform.forward * (enemyStatSO.Movespeed * Time.deltaTime);
        }
    }

    public void TakeDamage(float hit)
    {
        _health.Value -= hit;
        if (_health.Value <= 0)
        {
            Death();
        }
    }
    
    void Death()
    {
        GenericItemSO GISO =  enemyLootTableSO.DroppedItem();
        if (GISO)
        {
            GameObject gameObject = Instantiate(GISO.GetItemPrefab(), transform.position, new Quaternion());
            gameObject.GetComponent<NetworkObject>().Spawn();
        }
        BaseXPItem BXPI = enemyLootTableSO.DroppedXP();
        if (BXPI)
        {
            GameObject gameObject = Instantiate(BXPI.GetItemPrefab(), transform.position, new Quaternion());
            gameObject.GetComponent<NetworkObject>().Spawn();
        }
        Destroy(gameObject);
    }

    IEnumerator ChoseTarget()
    {
        while (true)
        {
            _target = enemyBehaviourSO.ChoseTargettedPlayer(_players, gameObject);
            yield return new WaitForSeconds(timeBetweenRetarget);
        }
    }
}
