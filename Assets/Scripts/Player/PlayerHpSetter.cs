using ScriptableObjects.Event;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHpSetter : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> _hp;
    [SerializeField] private float _maxHp;
    [SerializeField] private GameEventSO _death;

    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            _hp.Value = _maxHp;
        }
    }


    public void TakeDamage( float hp)
    {
        if (!IsOwner)
            return;
        if (_hp.Value <= 0) 
        {
            if(_death != null)
                _death.Raise();
            gameObject.SetActive(false);
            DeathServerRPC();
        }

        if (NetworkManager.Singleton.IsServer)
        {
            _hp.Value -= hp;
            return;
        }

        UpdateServerRPC(hp);
        
            return;
    }   

    [ServerRpc]
    void UpdateServerRPC(float hp) 
    {
        _hp.Value -= hp;
    }

    [ServerRpc(RequireOwnership = false)]
    void DeathServerRPC()
    {
        gameObject.SetActive(false);
    }
}
