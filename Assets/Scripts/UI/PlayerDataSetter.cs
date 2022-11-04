using System.Collections;
using System.Collections.Generic;
using Network;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerDataSetter : NetworkBehaviour
{
    [SerializeField] private TextMeshPro _nameLabel;
    [SerializeField] private Transform _modelTransform;
    [SerializeField] private SkinSelectionData _skinSelectionData;

    private NetworkVariable<FixedString64Bytes> _name = new NetworkVariable<FixedString64Bytes>();
    private bool _dataSet;
    private LobbyPlayerData _lobbyData;
    
    public override void OnNetworkSpawn()
    {
        UpdateNamesServerRpc();
        
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateNamesServerRpc()
    {
        foreach (var networkClient in NetworkManager.Singleton.ConnectedClientsList)
        {
            networkClient.PlayerObject.GetComponent<PlayerDataSetter>().RefreshPlayerDataClientRpc();
        }
    }

    [ClientRpc]
    private void RefreshPlayerDataClientRpc()
    {
        if (_lobbyData != null) 
            SetPlayerData(_lobbyData);
    }

    public void SetPlayerData(LobbyPlayerData lobbyData)
    {
        if (_dataSet) return;

        _lobbyData = lobbyData;
        ChangeNameServerRpc(lobbyData.Gamertag);

        _dataSet = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangeNameServerRpc(string newName)
    {
        _name.Value = newName;
        ChangeNameClientRpc(_name.Value.ToString());
    }

    [ClientRpc]
    private void ChangeNameClientRpc(string nameValue)
    {
        _nameLabel.text = nameValue;
    }

}
