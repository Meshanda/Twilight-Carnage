using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;

    private void Start()
    {
        _lobbyCodeText.text = "Lobby code: " + GameLobbyManager.Instance.GetLobbyCode();
    }
}
