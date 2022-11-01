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
    
    [Space(10)]
    [Header("Skin Selection")]
    [SerializeField] private Button _skinLeftButton;
    [SerializeField] private Button _skinRightButton;
    [SerializeField] private TextMeshProUGUI _skinName;
    [SerializeField] private SkinSelectionData _skinSlectionData;
    private int _currentSkinIndex = 0;

    private void OnEnable()
    {
        _readyButton.onClick.AddListener(OnReadyPressed);
        _mapLeftButton.onClick.AddListener(OnMapLeftButtonClicked);
        _mapRightButton.onClick.AddListener(OnMapRightButtonClicked);
        _skinLeftButton.onClick.AddListener(OnSkinLeftButtonClicked);
        _skinRightButton.onClick.AddListener(OnSkinRightButtonClicked);

        GameLobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
    }

    private void OnDisable()
    {
        _readyButton.onClick.RemoveAllListeners();
        _mapLeftButton.onClick.RemoveAllListeners();
        _mapRightButton.onClick.RemoveAllListeners();
        _skinLeftButton.onClick.RemoveAllListeners();
        _skinRightButton.onClick.RemoveAllListeners();
        
        GameLobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
    }

    private void OnMapLeftButtonClicked()
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
    }

    private void OnMapRightButtonClicked()
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
    }

    private void OnSkinLeftButtonClicked()
    {
        if (_currentSkinIndex - 1 >= 0)
        {
            _currentSkinIndex++;
        }
        else
        {
            _currentSkinIndex = _skinSlectionData.Skins.Count - 1;
        }

        UpdateSkin();
    }

    private void OnSkinRightButtonClicked()
    {
        if (_currentSkinIndex + 1 <= _skinSlectionData.Skins.Count - 1)
        {
            _currentSkinIndex++;
        }
        else
        {
            _currentSkinIndex = 0;
        }

        UpdateSkin();
    }
    private async void OnReadyPressed()
    {
        bool succeeded = await GameLobbyManager.Instance.SetPlayerReady();
        if (succeeded)
        {
            _readyButton.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _lobbyCodeText.text = "Lobby code: " + GameLobbyManager.Instance.GetLobbyCode();
    }

    private void UpdateMap()
    {
        _mapImage.color = _mapSelectionData.Maps[_currentMapIndex].MapThumbnail;
        _mapName.text = _mapSelectionData.Maps[_currentMapIndex].MapName;
    }

    private void UpdateSkin()
    {
        _skinName.text = _skinSlectionData.Skins[_currentSkinIndex].SkinName;
    }
    
    private void OnLobbyUpdated()
    {
        _currentMapIndex = GameLobbyManager.Instance.GetMapIndex();
        UpdateMap();
    }
}
