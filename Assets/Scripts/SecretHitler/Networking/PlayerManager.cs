using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    Dictionary<string, SHPlayer> _players = new Dictionary<string, SHPlayer>();
    Action<List<string>> _refreshPlayers = (playerList) => { };

    public List<SHPlayer> Players
    {
        get { return new List<SHPlayer>(_players.Values); }
    }

    public string FindInvestigatedParties()
    {
        foreach(KeyValuePair<string, SHPlayer> playerPair in _players)
        {
            if(playerPair.Value.Investigated)
            {
                return playerPair.Key;
            }
        }
        return "";
    }

    public int NumPlayers { get { return _players.Count; } }

    public List<string> PlayerNames
    {
        get { return new List<string>(_players.Keys); }
    }
    
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void AddPlayerRefreshListener(Action<List<string>> listener)
    {
        _refreshPlayers += listener;
    }

    public Action<SHPlayer> OnNewPlayerRegistered = (player) => { };

    public void RegisterNewPlayer(SHPlayer player)
    {
        Debug.Log("new player registered: " + player.Name);
        if (!_players.ContainsKey(player.Name))
        {
            _players.Add(player.Name, player);

            Debug.Log("new player added: " + player.Name);
            _refreshPlayers(new List<string>(_players.Keys));
            OnNewPlayerRegistered(player);
        }

        player.transform.parent = transform;
        player.gameObject.name = player.Name;
    }

    public void RemovePlayer(Player player)
    {
        if (_players.ContainsKey(player.NickName))
        {
            _players.Remove(player.NickName);
            _refreshPlayers(new List<string>(_players.Keys));
        }
    }

    public SHPlayer FindPlayer(string playerName)
    {
        if(_players.ContainsKey(playerName))
        {
            return _players[playerName];
        }
        return null;
    }

    public bool HaveAllPlayersVoted()
    {
        bool allVoted = true;
        foreach(KeyValuePair<string, SHPlayer>playerPair in _players)
        {
            if(playerPair.Value.Vote == InsertedVote.NONE && !playerPair.Value.IsKilled)
            {
                return false;
            }
        }
        return allVoted;
    }

    //TODO: if player disconnects and reconnects, we should store their old information and give them one of the informations if it is the same name

}
