using System.Collections;
using System.Collections.Generic;
using Network;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerDataSetter : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameLabel;
    [SerializeField] private Transform _modelTransform;
    [SerializeField] private SkinSelectionData _skinSelectionData;

    private bool _dataSet;

    public void SetPlayerData(LobbyPlayerData lobbyData)
    {
        if (_dataSet) return;
        
        SetPlayerName(lobbyData.Gamertag);
        SetPlaySkin(lobbyData.SkinIndex);

        _dataSet = true;
    }

    private void SetPlaySkin(int skinIndex)
    {
        var model = _skinSelectionData.Skins[skinIndex].Model;
        var animator = Instantiate(model,_modelTransform).GetComponent<Animator>();
        
        NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerAnimation>().SetAnimator(animator);
    }

    private void SetPlayerName(string gamertag)
    {
        _nameLabel.text = gamertag;
    }
}
