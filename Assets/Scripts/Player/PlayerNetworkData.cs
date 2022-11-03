using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkData : MonoBehaviour
{
    [SerializeField] private PlayerShootData _playerShootDataDefault;
    [SerializeField] private PlayerShootData _playerShootData;
    
    public PlayerShootData PlayerShootData
    {
        get => _playerShootData;
        set => _playerShootData = value;
    }

    private void Start()
    {
        ResetValueToDefault();
    }

    private void OnDisable()
    {
        ResetValueToDefault();
    }

    private void ResetValueToDefault()
    {
        _playerShootData.Damage = _playerShootDataDefault.Damage;
        _playerShootData.BulletSpeed = _playerShootDataDefault.BulletSpeed;
        _playerShootData.NbShoot = _playerShootDataDefault.NbShoot;
        _playerShootData.NbEnemyTouch = _playerShootDataDefault.NbEnemyTouch;
    }
}
