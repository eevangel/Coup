using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using Photon.Realtime;


namespace SHGame
{
    public class ChooseCabinetState : IFlowState
    {
        public PlayerList _playerList;
        public ChoosePersonPanel _choosePersonPanel;
        public NoticePanel _noticePanel;
        public CabinetPanel _cabinet;

        const string PRESIDENT_CHOICE = "Please choose a person from the right to be your chancellor. Choose wisely! Listen to the others if you dare!\n" + "You can't pick someone from the previous cabinet though ... even if you're besties...";
        const string OTHER_CHOICE = "Influence the fool in the president's seat to make who you want chancellor. Be the puppet master...";


        const string NOTICE_TITLE = "choose your chancellor";
        const string BODY_1 = "Are you sure you'd like to nominate ";
        const string BODY_2 = " as your uber special chancellor?";

        public override FlowState GetFlowState()
        {
            return FlowState.CHOOSE_CABINET;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.CHOOSE_CABINET)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.CHANCELLOR_NOMINATED)
                    .Goto(FlowState.VOTE_CABINET);
        }

        public override void EnterState()
        {
            Debug.Log("entered choose cabinet");
            if(SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(PRESIDENT_CHOICE);

                _playerList.ShowPlayerList(true);
                _playerList.EnablePlayerButtons(true);
                _playerList.DisableListOfPlayers(_gameState.PastCabinet);                
                _playerList.DisablePlayer(_gameState.PresidentName); ;
                _playerList.AddListenerToActivePlayerButtons(OnPlayerSelected);
                GameTitle.Instance.EditTitle("SELECT A CHANCELLOR");
            }
            else
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(OTHER_CHOICE);
                _playerList.ShowPlayerList(false);
                GameTitle.Instance.EditTitle("president selects a chancellor");
            }
        }

        public override void ExitState()
        {
            _choosePersonPanel.Show(false);
            _playerList.ShowPlayerList(false);
            _noticePanel.RemoveAllListeners();
            _noticePanel.Show(false);
            _cabinet.SetPossibleCabinet();
        }

        string _possibleChancellor;

        void OnPlayerSelected(string playerName)
        {
            _possibleChancellor = playerName;
            Debug.Log("chancellor selected: " + playerName);

            _choosePersonPanel.Show(false);

            _noticePanel.SetText(NOTICE_TITLE, BODY_1 + _possibleChancellor + BODY_2);
            _noticePanel.RemoveAllListeners();
            _noticePanel.AddListeners(OnChancellorConfirmed, OnBigCNotConfirmed);
            _noticePanel.Show(true);
        }
        
        void OnChancellorConfirmed()
        {
            Debug.Log("confirmar");
            _noticePanel.Show(false);

            _playerList.EnablePlayerButtons(false);
            if (!_gameState.ConfirmChancellor(_possibleChancellor))
            {
                Debug.Log("player not found");
                OnBigCNotConfirmed();
            }
        }

        void OnBigCNotConfirmed()
        {
            _noticePanel.Show(false);
            Debug.Log("non confirmar");

            _choosePersonPanel.Show(true);
        }
    }
}

