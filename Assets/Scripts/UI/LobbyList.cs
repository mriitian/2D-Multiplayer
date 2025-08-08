using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbyList : MonoBehaviour
{
    [SerializeField] LobbyItem lobbyItemPrefab;
    [SerializeField] Transform lobbyItemParent;
    private bool IsJoining;
    private bool IsRefreshing;

    private void OnEnable()
    {
        RefreshList();
    }

    public async void RefreshList()
    {
        if(IsRefreshing) return;
        IsRefreshing = true;


        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 25;
            options.Filters = new List<QueryFilter>()
            {
            new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"
                ),
            new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"
                )
            };

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync( options );

            foreach(Transform child in lobbyItemParent)
            {
                Destroy(child.gameObject);
            }
            foreach(Lobby lobby in queryResponse.Results)
            {
                LobbyItem lobbyItem = Instantiate(lobbyItemPrefab, lobbyItemParent);
                lobbyItem.Initialise(this, lobby);
            }

        }
        catch(LobbyServiceException e)
        {
            Debug.LogError(e);
        }
        

        IsRefreshing = false;
    }
    public async void JoinAsync(Lobby lobby)
    {
        if(IsJoining) return;
        IsJoining = true;
        try
        {
            Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joinedLobby.Data["JoinCode"].Value;

            await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }

        IsJoining=false;
    }
}
