using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    [SerializeField] private List<Transform> _players;
    [SerializeField] private GameObject[] _enemy;

    [SerializeField] private float _distanceFromPlayer;
    [SerializeField] private float _timeInit;
    private float _time = 0 ;

    [SerializeField, Range(0,1)] private float _vectorOffset;

    private Vector3 _opposingVectorDebug = Vector3.zero;
    private int playerIndexDebug = 0;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;

        if (_time > 0)
            return;

        _time = _timeInit;

        Spawn();

    }

    private void Spawn() 
    {
        if (_players == null)
            return;
        int playerIndex = Random.Range(0, _players.Count);

        if (_players[playerIndex] == null)
            Debug.LogError("_playerNull WTF");

        Vector3 opposingVector = Vector3.zero;

        for(int i = 0; i < _players.Count; i++) 
        {
            if (i == playerIndex || _players[i] == null)
                continue;

            opposingVector += (_players[i].position - _players[playerIndex].position).normalized;
        }
        
        opposingVector = -opposingVector.normalized;

        opposingVector.z += Random.Range(-0.75f, 0.75f);
        opposingVector.x += Random.Range(-0.75f, 0.75f);

        opposingVector = opposingVector.normalized;

        Instantiate(_enemy[0], _players[playerIndex].position +opposingVector * _distanceFromPlayer, Quaternion.identity);
        _opposingVectorDebug = opposingVector;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(_players[0].position, _players[0].position + _opposingVectorDebug * _distanceFromPlayer);
    }
}