using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyName;
    [SerializeField] private TMP_Text lobbyPlayers;
    private LobbyList LobbyList;
    private Lobby lobby;

    public void Initialise(LobbyList lobbyList ,Lobby lobby)
    {
        this.LobbyList = lobbyList;
        this.lobby = lobby;
        lobbyName.text = lobby.Name;
        lobbyPlayers.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
    }


    public void Join()
    {
        LobbyList.JoinAsync(lobby);
    }
}
