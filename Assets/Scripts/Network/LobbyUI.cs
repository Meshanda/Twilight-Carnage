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
    
    [SerializeField] private Button _readyButton;
    
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
        }

        GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

    private void OnDisable()
    {
        _readyButton.onClick.RemoveAllListeners();
        _mapLeftButton.onClick.RemoveAllListeners();
        _mapRightButton.onClick.RemoveAllListeners();
        
        GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
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
            
        await GameLobbyManager.Instance.SetPlayerReady(!isReady);
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
}
