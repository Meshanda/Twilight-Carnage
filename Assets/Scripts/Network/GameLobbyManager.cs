using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class GameLobbyManager : GenericSingleton<GameLobbyManager>
    {
        private List<LobbyPlayerData> _lobbyPlayerDatas = new List<LobbyPlayerData>();

        private LobbyPlayerData _localLobbyPlayerData;
        private LobbyData _lobbyData;
        private const int MAX_NUMBER_OF_PLAYERS = 4;

        public bool IsHost => _localLobbyPlayerData.Id == LobbyManager.Instance.GetHostId();
        
        private void OnEnable()
        {
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
        }

        public async Task<bool> CreateLobby()
        {
            _localLobbyPlayerData = new LobbyPlayerData();
            _localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer");
            
            _lobbyData = new LobbyData();
            _lobbyData.Initialize(0);

            return await LobbyManager.Instance.CreateLobby(MAX_NUMBER_OF_PLAYERS, true, _localLobbyPlayerData.Serialize(), _lobbyData.Serialize());
        }

        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        public async Task<bool> JoinLobby(string lobbyCode)
        {
            _localLobbyPlayerData = new LobbyPlayerData();
            _localLobbyPlayerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");

            return await LobbyManager.Instance.JoinLobby(lobbyCode, _localLobbyPlayerData.Serialize());
        }
        
        private async void OnLobbyUpdated(Lobby lobby)
        {
            var playerData = LobbyManager.Instance.GetPlayersData();
            _lobbyPlayerDatas.Clear();

            int numberOfPlayerReady = 0;
            foreach (var data in playerData)
            {
                var lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Initialize(data);

                if (lobbyPlayerData.IsReady)
                {
                    numberOfPlayerReady++;
                }

                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    _localLobbyPlayerData = lobbyPlayerData;
                }
                
                _lobbyPlayerDatas.Add(lobbyPlayerData);
            }

            _lobbyData = new LobbyData();
            _lobbyData.Initialize(lobby.Data);

            GameLobbyEvents.OnLobbyUpdated?.Invoke();

            if (numberOfPlayerReady == lobby.Players.Count)
            {
                GameLobbyEvents.OnLobbyReady?.Invoke();
            }
            else
            {
                GameLobbyEvents.OnLobbyUnReady?.Invoke();
            }

            if (_lobbyData.RelayJoinCode != default)
            {
                await JoinRelayServer(_lobbyData.RelayJoinCode);

                //SceneManager.LoadSceneAsync(_lobbyData.SceneName);
            }
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return _lobbyPlayerDatas;
        }

        public async Task<bool> SetPlayerReady()
        {
            _localLobbyPlayerData.IsReady = !_localLobbyPlayerData.IsReady;
            return await LobbyManager.Instance.UpdatePlayerData(_localLobbyPlayerData.Id, _localLobbyPlayerData.Serialize());
        }

        public bool IsPlayerReady()
        {
            return _lobbyPlayerDatas != null && _localLobbyPlayerData.IsReady;
        }

        public int GetMapIndex()
        {
            return _lobbyData.MapIndex;
        }

        public async Task<bool> SetSelectedMap(int currentMapIndex, string sceneName)
        {
            _lobbyData.MapIndex = currentMapIndex;
            _lobbyData.SceneName = sceneName;

            return await LobbyManager.Instance.UpdateLobbyData(_lobbyData.Serialize());
        }

        public async Task StartGame()
        {
            var relayJoinCode = await RelayManager.Instance.CreateRelay(MAX_NUMBER_OF_PLAYERS);

            _lobbyData.RelayJoinCode = relayJoinCode;
            await LobbyManager.Instance.UpdateLobbyData(_lobbyData.Serialize());

            var allocationId = RelayManager.Instance.GetAllocationId();
            var connectionData = RelayManager.Instance.GetConnectionData();

            await LobbyManager.Instance.UpdatePlayerData(_localLobbyPlayerData.Id, _localLobbyPlayerData.Serialize(), allocationId, connectionData);

            //SceneManager.LoadSceneAsync(_lobbyData.SceneName);
        }
        
        private async Task<bool> JoinRelayServer(string relayJoinCode)
        {
            await RelayManager.Instance.JoinRelay(relayJoinCode);
            
            var allocationId = RelayManager.Instance.GetAllocationId();
            var connectionData = RelayManager.Instance.GetConnectionData();

            await LobbyManager.Instance.UpdatePlayerData(_localLobbyPlayerData.Id, _localLobbyPlayerData.Serialize(), allocationId, connectionData);
            
            return true;
        }

        public int GetLocalSkinIndex()
        {
            return _localLobbyPlayerData.SkinIndex;
        }

        public async Task<bool> SetLocalSkinIndex(int skinIndex)
        {
            _localLobbyPlayerData.SkinIndex = skinIndex;
            
            return await LobbyManager.Instance.UpdatePlayerData(_localLobbyPlayerData.Id, _localLobbyPlayerData.Serialize());
        }
    }
}
        