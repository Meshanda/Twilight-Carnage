using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameLobbyEvents
{
    public delegate void LobbyUpdated();

    public static LobbyUpdated OnLobbyUpdated;
}
