using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


[RequireComponent(typeof(PhotonView))]
public class SHPlayer : MonoBehaviour
{
    public static SHPlayer LocalInstance;

    [SerializeField] protected bool _isFascist;
    [SerializeField] protected bool _isHitler;


    [SerializeField]
    protected string _name;

    public string Name
    {
        get { return _name; }
    }
    public Player PunPlayer { get; private set; }

    PhotonView _view;


    void Awake()
    {
        _view = GetComponent<PhotonView>();

        if (_view.IsMine)
        {
            LocalInstance = this;
            _name = PhotonNetwork.LocalPlayer.NickName;
            Debug.Log("local instace name " + _name);
            PunPlayer = PhotonNetwork.LocalPlayer;
            _view.RPC("RegisterUser", RpcTarget.Others, _name, PunPlayer.ActorNumber);
            PlayerManager.Instance.RegisterNewPlayer(this);
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

    #region role and party

    public bool IsPresident { get; private set; }

    public bool IsChancellor { get; private set; }

    public bool IsLiberal
    {
        get { return !_isFascist; }
    }

    public bool IsHitler
    {
        get { return _isHitler; }
    }

    public void SetHitler()
    {
        _isFascist = true;
        _isHitler = true;
    }

    public void SetFascist()
    {
        _isFascist = true;
        _isHitler = false;
    }

    public void SetLiberal()
    {
        _isFascist = false;
        _isHitler = false;
    }

    public void BroadcastRole()
    {
        Debug.LogFormat("set player {0} to hitler: {1} fascist {2} liberal {3}", _name, _isHitler.ToString(), _isFascist.ToString(), (!_isHitler && !_isFascist));
        if (PhotonNetwork.IsMasterClient)
        {
            _view.RPC("UpdateRole", RpcTarget.All, _isFascist, _isHitler);
        }
    }

    public bool Investigated { get; private set; } = false;

    public void SetInvestigated()
    {
        Investigated = true;

        _view.RPC("SendInvestigated", RpcTarget.All);
    }

    [PunRPC]
    protected void SendInvestigated()
    {
        Investigated = true;
    }

    #endregion role and party

    #region cabinet

    public void SetPresident(bool isPresident)
    {
        IsPresident = isPresident;
        _view.RPC("SendPresidentialRole", RpcTarget.All, isPresident);
    }

    [PunRPC]
    protected void SendPresidentialRole(bool isPresident)
    {
        IsPresident = isPresident;
    }



    public void SetChancellor(bool isChancie)
    {
        IsChancellor = isChancie;
        _view.RPC("SendChancellorRole", RpcTarget.All, isChancie);
    }

    [PunRPC]
    protected void SendChancellorRole(bool isChancie)
    {
        IsChancellor = isChancie;
    }

    #endregion cabinet

    #region voting

    InsertedVote _vote;
    public virtual InsertedVote Vote
    {
        get
        {
            return _vote;
        }
        set
        {
            _vote = value;

            if (_view.IsMine)
            {
                _view.RPC("SendVote", RpcTarget.All, _vote);
            }
        }
    }

    public System.Action <string>OnVoteUpdated = (player) => { };

    [PunRPC]
    protected virtual void SendVote(InsertedVote vote)
    {
        _vote = vote;
        OnVoteUpdated(Name);
    }

    #endregion voting

    #region RPCS
    [PunRPC]
    void RegisterUser(string name, int actorNumber)
    {
        _name = name;
        gameObject.name = _name;
        FindPlayer(actorNumber);
        PlayerManager.Instance.RegisterNewPlayer(this);
    }

    [PunRPC]
    protected void UpdateUser()
    {
        if (_view.IsMine)
        {
            _view.RPC("RegisterUser", RpcTarget.Others, _name, PunPlayer.ActorNumber);
        }
    }

    [PunRPC]
    protected void UpdateRole(bool fascist, bool hitler)
    {
        _isFascist = fascist;
        _isHitler = hitler;
    }

    #endregion

    #region kill

    public bool IsKilled { get; private set; } = false;

    public void Kill()
    {
        IsKilled = true;

        _view.RPC("SendKilled", RpcTarget.All);
    }

    [PunRPC]
    protected void SendKilled()
    {
        IsKilled = true;
    }


    #endregion  kill


}
