﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


[RequireComponent(typeof(PhotonView))]
public class CoupPlayerManager : MonoBehaviour
{
    public static CoupPlayerManager Instance;


    Dictionary<string, CoupPlayer> _players = new Dictionary<string, CoupPlayer>();

    public List<CoupPlayer> _activePlayers = new List<CoupPlayer>();

    public int NumPlayers { get { return _players.Count; } }
    public int NumActive { get { return _activePlayers.Count; } }

    PhotonView _view;

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
        _view = GetComponent<PhotonView>();
    }


    public void RegisterNewPlayer(CoupPlayer player)
    {
        Debug.Log("new player registered: " + player.Name);
        if (!_players.ContainsKey(player.Name))
        {
            _players.Add(player.Name, player);
            _activePlayers.Add(player);
            Debug.Log("new player added: " + player.Name);
            //_refreshPlayers(new List<string>(_players.Keys));
            //OnNewPlayerRegistered(player);
        }

        player.transform.parent = transform;
        player.gameObject.name = player.Name;
    }

    public void CreatePlayerOrder()
    {
        List<string> order = new List<string>();
        foreach(CoupPlayer player in _activePlayers)
        {
            order.Add(player.Name);
            Debug.Log(player.Name);
        }

        _view.RPC("SetPlayerOrder", RpcTarget.All, order.ToArray());
    }

    public System.Action OnRecievingPlayerOrder = () => { };

    [PunRPC]
    void SetPlayerOrder(string[] order)
    {
        _activePlayers.Clear();
        
        foreach(string name in order)
        {
            Debug.Log(name);
            _activePlayers.Add(_players[name]);
        }
        OnRecievingPlayerOrder();
    }

    public List<CoupPlayerData> ConstructOtherPlayerOrder()
    {
        List<CoupPlayerData> playerData = new List<CoupPlayerData>();

        List<CoupPlayerData> beforeFound= new List<CoupPlayerData>();

        bool foundLocalPlayer = false;
        foreach(CoupPlayer player in _activePlayers)
        {
            if(player.name == CoupPlayer.LocalInstance.Name)
            {
                foundLocalPlayer = true;
                continue;
            }
            if(foundLocalPlayer)
            {
                playerData.Add(player._data);
            }
            else
            {
                beforeFound.Add(player._data);
            }
        }
        playerData.AddRange(beforeFound);


        string order = "ORDER: "; 
        foreach(CoupPlayerData player in playerData)
        {
            order += player._name + " . ";
        }
        Debug.Log(order);

        return playerData;
    }
}
