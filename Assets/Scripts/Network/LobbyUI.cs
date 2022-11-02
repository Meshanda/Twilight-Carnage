using System;
using System.Collections;
using System.Collections.Generic;
using Network;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;

    [Space(10)] 
    [Header("Ready Button")] 
    [SerializeField] private Button _startButton;
    
    [Space(10)]
    [Header("Ready Button")]
    [SerializeField] private Button _readyButton;
    [SerializeField] private TextMeshProUGUI _readyText;
    
    [Space(10)]
    [Header("Map Selection")]
    [SerializeField] private Image _mapImage;
    [SerializeField] private Button _mapLeftButton;
    [SerializeField] private Button _mapRightButton;
    [SerializeField] private TextMeshProUGUI _mapName;
    [SerializeField] private MapSelectionData _mapSelectionData;
    private int _currentMapIndex = 0;
    
    private void OnEnable()
    {
        _readyButton.onClick.AddListener(OnReadyPressed);

        if (GameLobbyManager.Instance.IsHost)
        {
            _mapLeftButton.onClick.AddListener(OnMapLeftButtonClicked);
            _mapRightButton.onClick.AddListener(OnMapRightButtonClicked);

            GameLobbyEvents.OnLobbyReady += OnLobbyReady;
        }

        GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

   

    private void OnDisable()
    {
        _readyButton.onClick.RemoveAllListeners();
        _mapLeftButton.onClick.RemoveAllListeners();
        _mapRightButton.onClick.RemoveAllListeners();
        
        GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        GameLobbyEvents.OnLobbyReady -= OnLobbyReady;

    }

    private void Start()
    {
        _lobbyCodeText.text = "Lobby code: " + GameLobbyManager.Instance.GetLobbyCode();

        if (!GameLobbyManager.Instance.IsHost)
        {
            _mapLeftButton.gameObject.SetActive(false);
            _mapRightButton.gameObject.SetActive(false);
        }
    }
    
    private async void OnMapLeftButtonClicked()
    {
        if (_currentMapIndex - 1 >= 0)
        {
            _currentMapIndex--;
        }
        else
        {
            _currentMapIndex = _mapSelectionData.Maps.Count - 1;
        }

        UpdateMap();
        GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex);

    }

    private async void OnMapRightButtonClicked()
    {
        if (_currentMapIndex + 1 <= _mapSelectionData.Maps.Count - 1)
        {
            _currentMapIndex++;
        }
        else
        {
            _currentMapIndex = 0;
        }

        UpdateMap();
        GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex);
    }
    private async void OnReadyPressed()
    {
        var isReady = GameLobbyManager.Instance.IsPlayerReady();
        _readyText.text = !isReady ? "Not Ready" : "Ready";
        
        await GameLobbyManager.Instance.SetPlayerReady();
    }

    private void UpdateMap()
    {
        _mapImage.color = _mapSelectionData.Maps[_currentMapIndex].MapThumbnail;
        _mapName.text = _mapSelectionData.Maps[_currentMapIndex].MapName;
    }
    
    private void OnLobbyUpdated()
    {
        _currentMapIndex = GameLobbyManager.Instance.GetMapIndex();
        UpdateMap();
    }
    
    private void OnLobbyReady()
    {
        _startButton.gameObject.SetActive(true);
    }
}
