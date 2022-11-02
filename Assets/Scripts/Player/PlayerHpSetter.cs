using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHpSetter : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<float> _hp;
    [SerializeField] private float _maxHp;  

    // Start is called before the first frame update
    void Start()
    {
        _hp.Value = _maxHp;
    }


    public void TakeDamage( float hp) 
    {
        if (NetworkManager.Singleton.IsServer)
        {
            _hp.Value -= hp;
            return;
        }

        UpdateServerRPC(hp);
    }

    [ServerRpc(RequireOwnership = true)]
    void UpdateServerRPC(float hp) 
    {
        _hp.Value -= hp;
    }
}
