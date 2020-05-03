using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;


[RequireComponent(typeof(PhotonView))]
public class CoupPlayerManager : MonoBehaviour
{
    public static CoupPlayerManager Instance;


    Dictionary<string, CoupPlayer> _players = new Dictionary<string, CoupPlayer>();
    List<CoupPlayer> _deadPlayers = new List<CoupPlayer>();
    public List<CoupPlayer> _activePlayers = new List<CoupPlayer>();

    public int NumPlayers { get { return _players.Count; } }
    public int NumActive { get { return _activePlayers.Count; } }


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


    public void RegisterNewPlayer(CoupPlayer player)
    {
        Debug.Log("new player registered: " + player.Name);
        if (!_players.ContainsKey(player.Name))
        {
            _players.Add(player.Name, player);

            Debug.Log("new player added: " + player.Name);
            //_refreshPlayers(new List<string>(_players.Keys));
            //OnNewPlayerRegistered(player);
        }

        player.transform.parent = transform;
        player.gameObject.name = player.Name;
    }
}
