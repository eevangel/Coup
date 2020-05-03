using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class CoupPlayer : MonoBehaviour
{
    
    [SerializeField]
    protected string _name;

    public string Name
    {
        get { return _name; }
    }
    public Player PunPlayer { get; private set; }

    PhotonView _view;

    #region initialization

    public static CoupPlayer LocalInstance;

    void Awake()
    {
        _view = GetComponent<PhotonView>();

        if (_view.IsMine)
        {
            LocalInstance = this;
            _name = PhotonNetwork.LocalPlayer.NickName;
            _data._name = _name;
            PunPlayer = PhotonNetwork.LocalPlayer;
            _view.RPC("RegisterUser", RpcTarget.Others, _name, PunPlayer.ActorNumber);
            CoupPlayerManager.Instance.RegisterNewPlayer(this);
        }
    }


    protected virtual void Start()
    {
        if (!_view.IsMine)
        {
            _view.RPC("UpdateUser", RpcTarget.Others);
        }
    }

    void FindPlayer(int actorNumber)
    {
        Player[] players = PhotonNetwork.PlayerListOthers;
        foreach (Player pl in players)
        {
            if (pl.ActorNumber == actorNumber)
            {
                Debug.Log("found punplayer: " + pl.NickName);
                PunPlayer = pl;
                return;
            }
        }
        Debug.Log("couldn't find new player");
    }


    [PunRPC]
    void RegisterUser(string name, int actorNumber)
    {
        _name = name;
        _data._name = _name;
        gameObject.name = _name;
        FindPlayer(actorNumber);
        CoupPlayerManager.Instance.RegisterNewPlayer(this);
    }

    [PunRPC]
    protected void UpdateUser()
    {
        if (_view.IsMine)
        {
            _view.RPC("RegisterUser", RpcTarget.Others, _name, PunPlayer.ActorNumber);
        }
    }

    #endregion initialization

    #region playerData

    public CoupPlayerData _data = new CoupPlayerData();

    public void ReceiveCharacter(CharacterType charType)
    {
        _view.RPC("StoreCharacter", RpcTarget.All, (byte)charType);
    }

    [PunRPC]
    public void StoreCharacter(byte charData)
    {
        _data.AddCharacter((CharacterType)charData);
    }

    public void AddCoins(int numCoins)
    {
        _data.AddCoins(numCoins);
        _view.RPC("SyncCoins", RpcTarget.All, _data._coins);
    }

    [PunRPC]
    public void SyncCoins(int coins)
    {
        _data._coins = coins;
    }

    #endregion playerData
}
