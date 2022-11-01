using System.Collections;
using System.Collections.Generic;
using Network;
using TMPro;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshPro _playerName;

    private LobbyPlayerData _data;
    
    public void SetData(LobbyPlayerData data)
    {
        _data = data;
        _playerName.text = _data.Gamertag;
        gameObject.SetActive(true);
    }
}
