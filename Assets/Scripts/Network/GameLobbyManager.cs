using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class GameLobbyManager : GenericSingleton<GameLobbyManager>
{
    public async Task<bool> CreateLobby()
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GamerTag", "HostPlayer"}
        };

        return await LobbyManager.Instance.CreateLobby(4, true, playerData);
    }

    public string GetLobbyCode()
    {
        return LobbyManager.Instance.GetLobbyCode();
    }

    public async Task<bool> JoinLobby(string lobbyCode)
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GamerTag", "JoinPlayer"}
        };

        return await LobbyManager.Instance.JoinLobby(lobbyCode, playerData);
    }
}
        