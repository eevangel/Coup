using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using TMPro;



namespace SHGame
{
    public class PickPresidentState : IFlowState
    {

        public PlayerList _playerList;
        public ChoosePersonPanel _choosePersonPanel;
        public NoticePanel _noticePanel;

        const string PRESIDENT_CHOICE = "Please select a person from the right to be the next president. The normal president order will resume after this one.";
        const string OTHER_CHOICE = "The current president gets to select a not dead person to be president next. After they are president, normal elections will ensue...";

        const string NOTICE_TITLE = "choose the next president";
        const string BODY_1 = "Are you sure you want to make ";
        const string BODY_2 = " the next president? I mean its a big decision...";


        public override FlowState GetFlowState()
        {
            return FlowState.PICK_PRESIDENT;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.PICK_PRESIDENT)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_NOMINATED)
                    .Goto(FlowState.CHOOSE_CABINET);
        }

        public override void EnterState()
        {
            Debug.Log("entered PickPresidentState");
            if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(PRESIDENT_CHOICE);

                _playerList.ShowPlayerList(true);
                _playerList.EnablePlayerButtons(true);
                _playerList.DisablePlayer(_gameState.PresidentName);
                _playerList.AddListenerToActivePlayerButtons(OnPlayerSelected);
                GameTitle.Instance.EditTitle("SELECT THE NEXT PRESIDENTIAL CANDIDATE");

            }
            else
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(OTHER_CHOICE);
                GameTitle.Instance.EditTitle("the seated president may now select the following president");
            }
        }


        string _possiblePresident = "";
        void OnPlayerSelected(string playerName)
        {
            _choosePersonPanel.Show(false);
            Debug.Log("confirmar");
            _possiblePresident = playerName;

            _noticePanel.SetText(NOTICE_TITLE, BODY_1 + _possiblePresident + BODY_2);
            _noticePanel.RemoveAllListeners();
            _noticePanel.AddListeners(OnPresidentConfirmed, OnPresidentNotConfirmed);
            _noticePanel.Show(true);
        }

        void OnPresidentConfirmed()
        {
            Debug.Log("confirmar");
            _noticePanel.Show(false);

            if (!_gameState.ConfirmPresident(_possiblePresident, false))
            {
                Debug.LogWarning("player not found");
                OnPresidentNotConfirmed();
            }
        }

        void OnPresidentNotConfirmed()
        {
            _noticePanel.Show(false);
            Debug.Log("non confirmar");

            _choosePersonPanel.Show(true);
            _playerList.ShowPlayerList(true);
        }

        public override void ExitState()
        {
            _gameState._specialPower = PresidentialSpecialPowerType.NONE;
            _choosePersonPanel.Show(false);
            _noticePanel.RemoveAllListeners();
            _noticePanel.Show(false);
        }
    }
}