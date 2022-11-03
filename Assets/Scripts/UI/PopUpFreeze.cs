using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DelegateToolBox;
using Unity.Netcode;
using UnityEngine;

public class PopUpFreeze : NetworkBehaviour
{
    [SerializeField] private Canvas _canvas;
    
    private float _freezeSpeed = 1.0f; // 1 -> 1sec/1 = 1sec, 0.5 -> 1sec/0.5 = 2sec, 2 -> 1sec/2 = 0.5sec
    private WaitForEndOfFrame _cachedYield = new WaitForEndOfFrame();

    private bool _isAnimating;

    private int _clientsReady = 0;
    
    // PowerUps
    [SerializeField] private IntEffect _damageUp;
    [SerializeField] private IntEffect _numberUp;
    [SerializeField] private IntEffect _pierceUp;
    [SerializeField] private FloatEffect _speedUp;

    private PlayerShootData _playerShootData;
    
    private void Start()
    {
    }

    public override void OnNetworkSpawn()
    {
        _playerShootData = NetworkManager.SpawnManager.GetLocalPlayerObject().GetComponent<PlayerNetworkData>().PlayerShootData;
        base.OnNetworkSpawn();
    }

    public void OnEventTriggerred() // Unity Event
    {
        ToggleFreeze();
    }

    private void ToggleFreeze()
    {
        if (IsTimeFreezed())
        {
            StopFreezeClientRpc();
        }
        else
        {
            StartFreezeClientRpc();
        }
    }
    
    private bool IsTimeFreezed()
    {
        return Time.timeScale <= 0;
    }

    private void StopFreezeTimeCoroutine()
    {
        StopCoroutine(nameof(FreezeTimeCoroutine));
    }

    private IEnumerator FreezeTimeCoroutine(bool freeze = true)
    {
        if (freeze)
        {
            while (Time.timeScale > 0)
            {
                
                Time.timeScale = Mathf.Max(Time.timeScale - Time.unscaledDeltaTime * _freezeSpeed, 0);
                yield return _cachedYield;
            }

            TimeFreezed();
        }
        else
        {
            while (Time.timeScale < 1)
            {
                Time.timeScale = Mathf.Min(Time.timeScale + Time.unscaledDeltaTime * _freezeSpeed, 1);
                yield return _cachedYield;
            }

            TimeUnfreezed();
        }
    }

    [ClientRpc]
    public void StartFreezeClientRpc()
    {
        StartFreeze();
    }

    [ClientRpc]
    void StopFreezeClientRpc()
    {
        StopFreeze();
    }
    
    public void StartFreeze()
    {
        if (_isAnimating)
            return;

        _isAnimating = true;

        _clientsReady = 0;
        
        StopFreezeTimeCoroutine();
        StartCoroutine(FreezeTimeCoroutine());
        // ...
    }
    
    private void TimeFreezed()
    {
        _isAnimating = false;
        
        // show UI
        _canvas.enabled = true;
    }
    
    public void StopFreeze()
    {
        if (_isAnimating)
            return;

        _isAnimating = true;
        
        StopFreezeTimeCoroutine();
        StartCoroutine(FreezeTimeCoroutine(false));
        // hide UI
        _canvas.enabled = false;
    }
    
    private void TimeUnfreezed()
    {
        _isAnimating = false;
        
        // ...
    }


    public void OnPowerUpSelected(int number)
    {
        switch (number)
        {
            case 1:
                _playerShootData.ApplyEffect(_damageUp);
                break;
            case 2:
                _playerShootData.ApplyEffect(_speedUp);
                break;
            case 3:
                _playerShootData.ApplyEffect(_pierceUp);
                break;
            default:
                _playerShootData.ApplyEffect(_numberUp);
                break;
        }

        _canvas.enabled = false;
        ClientReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void ClientReadyServerRpc()
    {
        _clientsReady++;
        if (NetworkManager.ConnectedClients.Count <= _clientsReady)
        {
            StopFreezeClientRpc();
        }
    }
    
}
