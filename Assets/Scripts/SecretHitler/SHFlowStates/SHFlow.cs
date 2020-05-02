using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using Photon.Realtime;

namespace SHGame
{
    [RequireComponent(typeof(PhotonView))]

    public class SHFlow : MonoBehaviour
    {

        PassiveStateMachine<FlowState, FlowEvents> _stateMachine = new PassiveStateMachine<FlowState, FlowEvents>();

        //CREATE IFLOWSTATE INTERFACE FOR ENTERING AND EXITING STATE
        PhotonView _view;

        public PlayerList _playerList;

        private void Awake()
        {
            _view = GetComponent<PhotonView>();
            SetupStates();
        }

        private void Start()
        {
            PlayerManager.Instance.AddPlayerRefreshListener(_playerList.RefreshPlayerList);
            _stateMachine.Start();
        }

        #region state setup

        [Header("STATES")]
        public NoGameState _noGameState;
        public ChoosePresidentState _choosePresidentState;
        public ChooseCabinetState _cabinetState;
        public VoteCabinetState _voteCabState;
        public VotesFailedState _voteFailedState;
        public ChoosePolicyChancellor _chancellorPolicyState;
        public ChoosePolicyPresident _presidentPolicyState;
        public EnactPolicyState _enactPolicyState;
        public EndGameState _endGameState;

        [Header("special_power_states")]
        public TopThreeState _topThreeState;
        public PickPresidentState _pickPresidentPowerState;
        public KillPersonState _killPersonState;
        public InvestigateState _investigateState;

        void SetupStates()
        {
            _noGameState.SetSHFlow(this);
            _choosePresidentState.SetSHFlow(this);
            _cabinetState.SetSHFlow(this);
            _voteCabState.SetSHFlow(this);
            _voteFailedState.SetSHFlow(this);
            _presidentPolicyState.SetSHFlow(this);
            _chancellorPolicyState.SetSHFlow(this);
            _enactPolicyState.SetSHFlow(this);
            _endGameState.SetSHFlow(this);

            _topThreeState.SetSHFlow(this);
            _pickPresidentPowerState.SetSHFlow(this);
            _killPersonState.SetSHFlow(this);
            _investigateState.SetSHFlow(this);


            _noGameState.SetupState(_stateMachine);
            _choosePresidentState.SetupState(_stateMachine);
            _cabinetState.SetupState(_stateMachine);
            _voteCabState.SetupState(_stateMachine);
            _voteFailedState.SetupState(_stateMachine);
            _presidentPolicyState.SetupState(_stateMachine);
            _chancellorPolicyState.SetupState(_stateMachine);
            _enactPolicyState.SetupState(_stateMachine);
            _endGameState.SetupState(_stateMachine);

            _topThreeState.SetupState(_stateMachine);
            _pickPresidentPowerState.SetupState(_stateMachine);
            _killPersonState.SetupState(_stateMachine);
            _investigateState.SetupState(_stateMachine);

            _stateMachine.Initialize(FlowState.NO_GAME);
        }

        #endregion state setup

        #region start game
        public void SendStartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ConstructAndSendPresidentOrder();

                _view.RPC("StartGame", RpcTarget.All);
            }
        }

        [PunRPC]
        void StartGame()
        {
            ResetGameState();

            _stateMachine.Fire(FlowEvents.START_GAME);
        }
        
        void ConstructAndSendPresidentOrder()
        {
            _presidentOrder = PlayerManager.Instance.Players;

            //TODO Can randomize order here or later

            List<string> order = new List<string>();
            foreach (SHPlayer player in _presidentOrder)
            {
                if (!player.IsKilled)
                {
                    order.Add(player.Name);
                }
            }
            _view.RPC("SendPresidentOrder", RpcTarget.All, order.ToArray());
        }

        [PunRPC]
        void SendPresidentOrder(string[] order)
        {
            _presidentOrder.Clear();
            foreach (string name in order)
            {
                _presidentOrder.Add(PlayerManager.Instance.FindPlayer(name));
            }

            _playerList.RefreshPlayerList(new List<string>(order));
        }

        #endregion start game

        #region game controls
        [Header("GAME CONTROLS")]
        public CabinetPanel _cabinet;

        #endregion gameControls

        #region GAME STATE
        SHPlayer _currentPresident = null;
        SHPlayer _currentChancellor = null;

        List<SHPlayer> _presidentOrder = new List<SHPlayer>();
        int _currentPresidentIndex = 0;

        public FascistBoard _fascistBoard;

        List<SHPlayer> _pastCabinet = new List<SHPlayer>(); //should only be set after policy has been enacted
        public List<string> PastCabinet
        {
            get
            {
                List<string> pastCab = new List<string>();
                foreach (SHPlayer player in _pastCabinet)
                {
                    pastCab.Add(player.Name);
                }
                return pastCab;
            }
        }

        void ResetGameState()
        {
            IsVetoUnlocked = false;
            _currentChancellor = null;
            _currentPresident = null;
            _pastCabinet.Clear();
        }

        #region dummy 

        public bool IsPresidentDummy()
        {
            if (_currentPresident != null)
            {
                if (_currentPresident is Dummy_Player)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsChancellorDummy()
        {
            if (_currentChancellor != null)
            {
                if (_currentChancellor is Dummy_Player)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion dummy 

        #region confirm president
        public bool ConfirmPresident(string playerName, bool changeIndex = true)
        {
            SHPlayer possiblePres = PlayerManager.Instance.FindPlayer(playerName);
            if (possiblePres != null)
            {
                if (_currentPresident != null)
                {
                    _currentPresident.SetPresident(false);
                }

                _currentPresident = possiblePres;
                _currentPresident.SetPresident(true);

                _view.RPC("SendConfirmedPresident", RpcTarget.All, playerName, changeIndex);

                return true;
            }
            return false;
        }

        [PunRPC]
        void SendConfirmedPresident(string playerName, bool changeIndex)
        {
            _currentPresident = PlayerManager.Instance.FindPlayer(playerName);

            if(changeIndex)
            {
                SetPresidentIndex(playerName);
            }

            //update cabinet panel with name
            _cabinet.SetPresident(playerName);
            _stateMachine.Fire(FlowEvents.PRESIDENT_NOMINATED);
        }

        void SetPresidentIndex(string playerName)
        {
            for (int i = 0; i < _presidentOrder.Count; i++)
            {
                if (_presidentOrder[i].Name == playerName)
                {
                    _currentPresidentIndex = i;
                    _currentPresident = _presidentOrder[i];

                    return;
                }
            }
        }

        public string PresidentName
        {
            get
            {
                if (_currentPresident != null)
                {
                    return _currentPresident.Name;
                }
                return "non";
            }
        }

        #endregion confirm president
        
        #region confirm chancellor

        public bool ConfirmChancellor(string playerName)
        {
            SHPlayer possibleChancellor = PlayerManager.Instance.FindPlayer(playerName);
            if (possibleChancellor != null)
            {
                if (_currentChancellor != null)
                {
                    _currentChancellor.SetChancellor(false);
                }

                _currentChancellor = possibleChancellor;
                _currentChancellor.SetChancellor(true);

                _view.RPC("SendConfirmedChancellor", RpcTarget.All, playerName);

                return true;
            }
            return false;
        }

        [PunRPC]
        void SendConfirmedChancellor(string playerName)
        {
            _currentChancellor = PlayerManager.Instance.FindPlayer(playerName);
            _cabinet.SetChancellor(playerName);
            _stateMachine.Fire(FlowEvents.CHANCELLOR_NOMINATED);
        }
        #endregion confirm chancellor

        #region past cabinet tracking

        public void SetPastCabinet()
        {
            _pastCabinet.Clear();
            _pastCabinet.Add(_currentPresident);
            _pastCabinet.Add(_currentChancellor);
        }

        #endregion past cabinet tracking
        
        #region confirm election outcome

        public bool isHitlerVotedChancellor()
        {
            return _fascistBoard.NumPolicies >= 3 && _currentChancellor.IsHitler;
        }

        public void SendHitlerChancellorEndGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _view.RPC("HitlerChancellorEndgame", RpcTarget.All);
            }
        }


        [PunRPC]
        void HitlerChancellorEndgame()
        {
            Debug.Log("hitler chancellor endgame");
            _winCondition = WinCondition.HITLER_CHANCELLOR;
            _stateMachine.Fire(FlowEvents.HITLER_CHANCELLOR);
        }

        public void SendCabinetElectionOutcome(bool isElected)
        {
            _view.RPC("CabinetVoteOutcome", RpcTarget.All, isElected);
        }

        [PunRPC]
        void CabinetVoteOutcome(bool isElected)
        {
            if (isElected)
            {
                _stateMachine.Fire(FlowEvents.VOTE_SUCCEED);
            }
            else
            {
                _stateMachine.Fire(FlowEvents.ENOUGH_VOTES_FAILED);
            }
        }

        public void ElectionsFailed()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _enactPolicy = _drawPile.GetTopPolicy();
                _view.RPC("EnactPassedPolicy", RpcTarget.All, (byte)_enactPolicy);
            }
        }

        #endregion confirm election outcome
        
        #region set next president

        public void SetNextPresident()
        {
            _currentPresidentIndex++;

            if (_currentPresidentIndex >= _presidentOrder.Count)
            {
                _currentPresidentIndex = 0;
            }

            ConfirmPresident(_presidentOrder[_currentPresidentIndex].name);
        }

        public void SendClearChancellor()
        {
            _view.RPC("ClearChancellor", RpcTarget.All);
        }

        [PunRPC]
        void ClearChancellor()
        {
            if (_currentChancellor != null)
            {
                _currentPresident.SetChancellor(false);
            }
            _currentChancellor = null;
            _cabinet.ClearChancie();
        }


        #endregion set next president

        #region set policies

        public PolicyCardDeck _drawPile;

        public void PresidentRequestPolicies()
        {
            _view.RPC("RequestPolicyCards", RpcTarget.MasterClient, true);

        }

        public void ChancellorRequestPolicies()
        {
            _view.RPC("RequestPolicyCards", RpcTarget.MasterClient, false);
        }

        [PunRPC]
        void RequestPolicyCards(bool forPresident)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (forPresident)
                {
                    List<PolicyType> drawnCards = _drawPile.DrawThree();
                    if (!IsPresidentDummy())
                    {
                        _view.RPC("GiveCardsTo", _currentPresident.PunPlayer, forPresident, PolicyPanel.PolicyToByteArray(drawnCards));
                    }
                    else
                    {
                        _view.RPC("GiveCardsTo", RpcTarget.MasterClient, forPresident, PolicyPanel.PolicyToByteArray(drawnCards));
                    }
                }
                else
                {
                    if (!IsChancellorDummy())
                    {
                        Debug.Log("giving cards to chancellor: " + _currentChancellor.Name);
                        if (_currentChancellor.PunPlayer == null)
                        {
                            Debug.Log("issue with current chancellor pun hookup");
                        }
                        _view.RPC("GiveCardsTo", _currentChancellor.PunPlayer, forPresident, PolicyPanel.PolicyToByteArray(_presidentialPoliciesToChancellor));
                    }
                    else
                    {
                        _view.RPC("GiveCardsTo", RpcTarget.MasterClient, forPresident, PolicyPanel.PolicyToByteArray(_presidentialPoliciesToChancellor));

                    }
                }
            }
        }

        [PunRPC]
        void GiveCardsTo(bool forPresident, byte[] drawnCardsByteArray)
        {
            if (forPresident)
            {
                if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && IsPresidentDummy()))
                {
                    _presidentPolicyState.OnReceivedDrawCards(PolicyPanel.ByteArrToPolicyList(drawnCardsByteArray));
                }
                else
                {
                    Debug.Log("something went wrong not pres");
                }
            }
            else
            {
                if (SHPlayer.LocalInstance.IsChancellor || (PhotonNetwork.IsMasterClient && IsChancellorDummy()))
                {
                    _chancellorPolicyState.OnReceivedDrawCards(PolicyPanel.ByteArrToPolicyList(drawnCardsByteArray));
                }
                else
                {
                    Debug.Log("something went wrong not chancie");
                }
            }
        }

        public void PresidentChosePolicy(List<PolicyType> passed, PolicyType discarded)
        {
            _view.RPC("PassPresidentialPolicies", RpcTarget.MasterClient, PolicyPanel.PolicyToByteArray(passed), (byte)discarded);
        }

        List<PolicyType> _presidentialPoliciesToChancellor = new List<PolicyType>();
        [PunRPC]
        void PassPresidentialPolicies(byte[] passed, byte discarded)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _presidentialPoliciesToChancellor.Clear();
                _presidentialPoliciesToChancellor.AddRange(PolicyPanel.ByteArrToPolicyList(passed));

                PolicyPanel.PrintPolicies(_presidentialPoliciesToChancellor);

                _drawPile.DiscardPolicy((PolicyType)discarded);

                _view.RPC("EveryoneFireEvent", RpcTarget.All, (byte)FlowEvents.PRESIDENT_CHOSE);
            }
        }

        [PunRPC]
        void EveryoneFireEvent(byte flowEventByte)
        {
            FlowEvents flowEvent = (FlowEvents)flowEventByte;
            Debug.Log("recieved message to fire " + flowEvent.ToString());
            _stateMachine.Fire(flowEvent);
        }

        public void ChancellorChosePolicy(PolicyType passed, PolicyType discarded)
        {
            _view.RPC("EnactChancellorPolicy", RpcTarget.MasterClient, (byte)passed, (byte)discarded);
        }

        [PunRPC]
        void EnactChancellorPolicy(byte pass, byte discarded)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("ENACT " + ((PolicyType)pass).ToString());

                _drawPile.DiscardPolicy((PolicyType)discarded);

                //TODO SOMETHIGN WITH THE ENACTED PASSED POLICY
                _view.RPC("EnactPassedPolicy", RpcTarget.All, pass);
            }
        }

        public PolicyType _enactPolicy;
        public PresidentialSpecialPowerType _specialPower = PresidentialSpecialPowerType.NONE;
        [PunRPC]
        void EnactPassedPolicy(byte passed)
        {
            _enactPolicy = (PolicyType)passed;
            _stateMachine.Fire(FlowEvents.PASSING_POLICY);
        }

        #endregion set policies

        #region end game

        public WinCondition _winCondition;

        public void SendFascistsWin()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _view.RPC("FascistsWin", RpcTarget.All);
            }
        }

        [PunRPC]
        void FascistsWin()
        {
            Debug.Log("FascistsWin");
            _winCondition = WinCondition.FASCIST_POLICY;
            _stateMachine.Fire(FlowEvents.FASCISTS_WIN);
        }

        public void SendLiberalsWin()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _view.RPC("LiberalsWin", RpcTarget.All);
            }

        }

        [PunRPC]
        void LiberalsWin()
        {
            Debug.Log("LibsWin");
            _winCondition = WinCondition.LIBERAL_POLICY;
            _stateMachine.Fire(FlowEvents.LIBERALS_WIN);
        }

        public void SendKilledHitler()
        {
            _view.RPC("KilledHitler", RpcTarget.All);
        }

        [PunRPC]
        void KilledHitler()
        {
            Debug.Log("Killed hitler");
            _winCondition = WinCondition.KILL_HITLER;
            _stateMachine.Fire(FlowEvents.KILLED_HITLER);
        }


        #endregion end game

        #region special power

        public bool IsVetoUnlocked;//{ get; set; }


        public void SendPresidentGetsPower()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _view.RPC("PresidentGetsPower", RpcTarget.All, (byte)_specialPower);
            }
        }

        [PunRPC]
        void PresidentGetsPower(byte powerByte)
        {
            PresidentialSpecialPowerType power = (PresidentialSpecialPowerType)powerByte;

            switch (power)
            {
                case PresidentialSpecialPowerType.TOP_THREE:
                    _stateMachine.Fire(FlowEvents.TOP_THREE);
                    break;
                case PresidentialSpecialPowerType.INVESTIGATE:
                    _stateMachine.Fire(FlowEvents.INVESTIGATE_PARTY);
                    break;
                case PresidentialSpecialPowerType.PICK_NEXT_PRESIDENT:
                    _stateMachine.Fire(FlowEvents.PICK_PRESIDENT);
                    break;
                case PresidentialSpecialPowerType.KILL:
                    _stateMachine.Fire(FlowEvents.KILL_PLAYER);
                    break;
                case PresidentialSpecialPowerType.KILL_AND_VETO:
                    IsVetoUnlocked = true;
                    _stateMachine.Fire(FlowEvents.KILL_PLAYER);
                    break;
            }
        }

        #endregion special policy

        #region top three 

        public System.Action<List<PolicyType>> RecieveTopThree = (topThree) => {};
        public void RequestTopThree()
        {
            _view.RPC("PeekTopThreePolicyCards", RpcTarget.MasterClient);
        }

        [PunRPC]
        void PeekTopThreePolicyCards()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                List<PolicyType> drawnCards = _drawPile.PeekThree();
                if (!IsPresidentDummy())
                {
                    _view.RPC("DeliverTopThreeCards", _currentPresident.PunPlayer, PolicyPanel.PolicyToByteArray(drawnCards));
                }
                else
                {
                    _view.RPC("DeliverTopThreeCards", RpcTarget.MasterClient, PolicyPanel.PolicyToByteArray(drawnCards));
                }
            }
        }

        [PunRPC]
        void DeliverTopThreeCards(byte[] drawBytes)
        {
            List<PolicyType> drawnCards = PolicyPanel.ByteArrToPolicyList(drawBytes);

            RecieveTopThree(drawnCards);
        }




        #endregion top three 

        #region kill

        public void RequestNewPresidentOrder()
        {
            _view.RPC("NewPresidentOrder", RpcTarget.MasterClient);
        }

        [PunRPC]
        void NewPresidentOrder()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ConstructAndSendPresidentOrder();
            }
        }

        public void RequestAnnounceKill(string killedDude)
        {
            _view.RPC("AnnounceKill", RpcTarget.All, killedDude);
        }

        [PunRPC]
        void AnnounceKill(string killedDude)
        {
            _killPersonState.AnnounceKill(killedDude);
        }
        #endregion kill

        #region veto

        public void SendVetoRequest()
        {
            _view.RPC("VetoRequest", _currentPresident.PunPlayer);
        }

        [PunRPC]
        void VetoRequest()
        {            
            _chancellorPolicyState.OnVetoRequest();
        }

        public void SendVetoResponse(bool vetoSucceed)
        {
            if(vetoSucceed)
            {
                _view.RPC("VetoSucceeded", RpcTarget.All);
            }
            else
            {
                _view.RPC("VetoDenial", _currentChancellor.PunPlayer);
            }
        }

        [PunRPC]
        void VetoSucceeded()
        {
            _stateMachine.Fire(FlowEvents.VETO_SUCCEEDED);
        }

        [PunRPC]
        void VetoDenial()
        {
            _chancellorPolicyState.OnRecievedVetoDenial();
        }

        

        #endregion veto

        #endregion GAMESTATE
    }


    public enum WinCondition
    {
        FASCIST_POLICY,
        LIBERAL_POLICY,
        HITLER_CHANCELLOR,
        KILL_HITLER
    }
}

