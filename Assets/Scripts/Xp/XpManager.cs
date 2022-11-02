using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class XpManager : NetworkBehaviour
{
    [SerializeField] private ScriptableObjects.Variables.FloatVariable _lvlSo, _xpSo;
    [SerializeField] private NetworkVariable<float> xp, lvl;
    [SerializeField] private NetworkVariable<float> _lvlThreshold;
    [SerializeField] private bool _bLaunchedUpdate = false;
    // Start is called before the first frame update
    void Start()
    {
        _lvlSo.value = 0;
        _xpSo.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(lvl.Value > _lvlSo.value)
            _lvlSo.value = lvl.Value;
        if (_xpSo.value > xp.Value) 
        {
            if(NetworkManager.Singleton.IsServer)
            {
                xp.Value = _xpSo.value;
            }
            else if(!_bLaunchedUpdate)
            {
                _bLaunchedUpdate = true;
                UpdateXpServerRPC(_xpSo.value - xp.Value);
            }
        }
        if(NetworkManager.Singleton.IsServer && xp.Value > _lvlThreshold.Value) 
        {
            xp.Value = xp.Value - _lvlThreshold.Value;
            lvlUpClientRpc(xp.Value);
            lvl.Value++;
            SetThreshold();
        }

    }

    

    private void SetThreshold() 
    {
        _lvlThreshold.Value *= 1.5f;
    }
    [ClientRpc]
    private void lvlUpClientRpc(float xp)
    {
        _xpSo.value = xp;
    }

    [ClientRpc]
    private void XpUpdateClientRpc(float xp)
    {
        _xpSo.value = xp;
        _bLaunchedUpdate = false;
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateXpServerRPC(float addXp) 
    {
        xp.Value += addXp;
        _xpSo.value += addXp;
        XpUpdateClientRpc(_xpSo.value);
    }
}
