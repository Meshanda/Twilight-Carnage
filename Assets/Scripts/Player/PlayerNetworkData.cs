using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworkData : MonoBehaviour
{
    [SerializeField] private PlayerShootData _playerShootData;
    
    public PlayerShootData PlayerShootData
    {
        get => _playerShootData;
        set => _playerShootData = value;
    }
}
