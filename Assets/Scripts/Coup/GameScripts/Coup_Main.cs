using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using Photon.Realtime;


[RequireComponent(typeof(PhotonView))]
public class Coup_Main : MonoBehaviour
{
    public enum MainState
    {
        ROOM,
        GAME_SETUP,
        IN_GAME
    }

    public enum MainEvents
    {
        START_GAME,
        GAME_SETUP_COMPLETE, GAME_SETUP_FAILED,
        END_GAME
    }

    PassiveStateMachine<MainState, MainEvents> _stateMachine = new PassiveStateMachine<MainState, MainEvents>();
    PhotonView _view;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        SetupStatemMachine();
        _stateMachine.Start();
    }

    void SetupStatemMachine()
    {
        SetupRoomState();
        SetupGame_SetupState();
        InGame_SetupState();

        _stateMachine.Initialize(MainState.ROOM);
    }

    #region room state
    public RoomSelectionUI _RoomUI;

    void SetupRoomState()
    {
        _stateMachine.In(MainState.ROOM)
            .ExecuteOnEntry(EnterRoom)
            .ExecuteOnExit(ExitRoom)
            .On(MainEvents.START_GAME)
                .Goto(MainState.GAME_SETUP);
        _RoomUI.StartGameInvoker += TryStartGame;
    }

    void EnterRoom()
    {
        _RoomUI.gameObject.SetActive(true);
        _RoomUI.EnterRoomState();
    }
    void ExitRoom()
    {
        _RoomUI.gameObject.SetActive(false);
    }

    public void TryStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _view.RPC("StartGame", RpcTarget.All);
        }
    }

    [PunRPC]
    void StartGame()
    {
        Debug.Log("start game");
        _stateMachine.Fire(MainEvents.START_GAME);
    }

    #endregion room state

    #region game setup state

    //public GameSetupUI _gameSetupUI;
    //public SH_PlayerSetup _PlayerSetup;
    public CharacterDeckManager _deck;
    public PlayerLayout _layout;    


    void SetupGame_SetupState()
    {
        _stateMachine.In(MainState.GAME_SETUP)
            .ExecuteOnEntry(EnterGameSetup)
            .ExecuteOnExit(ExitGameSetup)
            .On(MainEvents.GAME_SETUP_COMPLETE)
                .Goto(MainState.IN_GAME);

        CoupPlayerManager.Instance.OnRecievingPlayerOrder = SetupLayout;
    }

    void EnterGameSetup()
    {
        Debug.Log("Enter game Setup");

        if(PhotonNetwork.IsMasterClient)
        {
            List<CoupPlayer> players = CoupPlayerManager.Instance._activePlayers;
            foreach (CoupPlayer player in players)
            {
                player.ReceiveCharacter(_deck.DrawOne());
                player.ReceiveCharacter(_deck.DrawOne());
                player.AddCoins(2);
            }
            CoupPlayerManager.Instance.CreatePlayerOrder();
        }
    }

    void SetupLayout()
    {
        _layout.LayoutPlayers(CoupPlayerManager.Instance.ConstructOtherPlayerOrder());
        _layout.Show(true);

        _layout.SetupLocalPlayer(CoupPlayer.LocalInstance._data);
    }

    void OnRolesAssigned()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _view.RPC("SetupPostRolesAssigned", RpcTarget.All);
        }
    }

    [PunRPC]
    void SetupPostRolesAssigned()
    {
        Debug.Log("SetupPostRolesAssigned");
        //_gameSetupUI.ShowPostRolesAssigned(PhotonNetwork.IsMasterClient);
    }

    void OnGameSetupComplete()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _view.RPC("GameSetupCompleted", RpcTarget.All);
        }
    }

    [PunRPC]
    void GameSetupCompleted()
    {
        _stateMachine.Fire(MainEvents.GAME_SETUP_COMPLETE);
    }

    void ExitGameSetup()
    {
        //_gameSetupUI.Hide();
    }
    #endregion game setup state

    #region in game  state
    
    void InGame_SetupState()
    {
        _stateMachine.In(MainState.IN_GAME)
            .ExecuteOnEntry(EnterInGame)
            .ExecuteOnExit(ExitInGame)
            .On(MainEvents.END_GAME)
                .Goto(MainState.GAME_SETUP);

    }

    void EnterInGame()
    {
        //_gameFlow.SendStartGame();
    }

    void ExitInGame()
    {

    }

    #endregion in game  state

}
