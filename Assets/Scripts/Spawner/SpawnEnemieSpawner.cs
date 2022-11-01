using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnEnemieSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _players;
    [SerializeField] private GameObject _enemySpawner;

    [SerializeField] private float _distanceFromPlayer;
    [SerializeField] private float _timeInit;
    private float _time = 0 ;

    [SerializeField, Range(0,1)] private float _vectorOffset;

    private Vector3 _opposingVectorDebug = Vector3.zero;
    private int _playerIndexDebug = 0;

    private int _budget = 1;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;

        if (_time > 0 || !NetworkManager.Singleton.IsServer)
            return;

        _time = _timeInit;

        Spawn();

    }

    private void Spawn() 
    {
        if (_players == null)
            return;
        for(int j = 0; j < _players.Count; j++)
        {
            int playerIndex = j;

            if (_players[playerIndex] == null)
                Debug.LogError("_playerNull WTF");

            Vector3 opposingVector = Vector3.zero;

            for (int i = 0; i < _players.Count; i++)
            {
                if (i == playerIndex || _players[i] == null)
                    continue;

                opposingVector += (_players[i].transform.position - _players[playerIndex].transform.position).normalized;
            }

            opposingVector = -opposingVector.normalized;

            opposingVector.z += Random.Range(-0.75f, 0.75f);
            opposingVector.x += Random.Range(-0.75f, 0.75f);

            opposingVector = opposingVector.normalized;

            GameObject go = Instantiate(_enemySpawner, _players[playerIndex].transform.position + opposingVector * _distanceFromPlayer, Quaternion.identity);
            go.GetComponent<SpawnEnemieWave>().SpawnPool(_budget, _players);
            _opposingVectorDebug = opposingVector;

            _playerIndexDebug = playerIndex;

            go.transform.LookAt(_players[j].transform);
        }
        nextBudget();
    }
    
    private void nextBudget() 
    {
        _budget = _budget * 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_players[_playerIndexDebug].transform.position, _players[_playerIndexDebug].transform.position + _opposingVectorDebug * _distanceFromPlayer);
    }
}