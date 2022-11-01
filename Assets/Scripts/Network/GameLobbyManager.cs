using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;

namespace Network
{
    public class GameLobbyManager : GenericSingleton<GameLobbyManager>
    {
        private List<LobbyPlayerData> _lobbyPlayerDatas = new List<LobbyPlayerData>();

        private LobbyPlayerData _localLobbyPlayerData;
        
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
            var playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, "HostPlayer");

            return await LobbyManager.Instance.CreateLobby(4, true, playerData.Serialize());
        }

        public string GetLobbyCode()
        {
            return LobbyManager.Instance.GetLobbyCode();
        }

        public async Task<bool> JoinLobby(string lobbyCode)
        {
            var playerData = new LobbyPlayerData();
            playerData.Initialize(AuthenticationService.Instance.PlayerId, "JoinPlayer");

            return await LobbyManager.Instance.JoinLobby(lobbyCode, playerData.Serialize());
        }
        
        private void OnLobbyUpdated(Lobby lobby)
        {
            var playerData = LobbyManager.Instance.GetPlayersData();
            _lobbyPlayerDatas.Clear();

            foreach (var data in playerData)
            {
                var lobbyPlayerData = new LobbyPlayerData();
                lobbyPlayerData.Initialize(data);

                if (lobbyPlayerData.Id == AuthenticationService.Instance.PlayerId)
                {
                    _localLobbyPlayerData = lobbyPlayerData;
                }
                
                _lobbyPlayerDatas.Add(lobbyPlayerData);
            }
            
            GameLobbyEvents.OnLobbyUpdated?.Invoke();
        }

        public List<LobbyPlayerData> GetPlayers()
        {
            return _lobbyPlayerDatas;
        }
    }
}
        