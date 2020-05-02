using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using TMPro;



namespace SHGame
{
    public class InvestigateState : IFlowState
    {

        public ChoosePersonPanel _choosePersonPanel;
        public NoticePanel _noticePanel;
        public PlayerList _playerList;


        const string PRESIDENT_CHOICE = "Select a player on the right whose party membership you'd like to investigate";
        const string OTHER_CHOICE = "Please wait while the President asks their mommy what party the other player is from...";
                
        const string NOTICE_TITLE = "investigate a slimy person";
        const string BODY_1 = "Are you sure you'd like to investigate ";
        const string BODY_2 = " and find all their salacious secrets?";


        const string INVESTIGATE_TITLE = "investigation complete";
        const string INVESTIGATE_BODY_1 = " is a member of the ";
        const string INVESTIGATE_BODY_2 = " party ... MIND blown";

        public override FlowState GetFlowState()
        {
            return FlowState.INVESTIGATE_PARTY;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.INVESTIGATE_PARTY)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_NOMINATED)
                    .Goto(FlowState.CHOOSE_CABINET);
        }

        public override void EnterState()
        {
            Debug.Log("entered InvestigateState");
            if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(PRESIDENT_CHOICE);
                
                _playerList.ShowPlayerList(true);
                _playerList.EnablePlayerButtons(true);

                string investigatedPlayerName = PlayerManager.Instance.FindInvestigatedParties();
                if (investigatedPlayerName != "")
                {
                    _playerList.DisablePlayer(investigatedPlayerName);
                }

                _playerList.DisablePlayer(_gameState.PresidentName);
                _playerList.AddListenerToActivePlayerButtons(OnPlayerSelected);
                GameTitle.Instance.EditTitle("INVESTIGATE A PERSON'S PARTY");
            }
            else
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(OTHER_CHOICE);

                _playerList.EnablePlayerButtons(false);
                _playerList.ShowPlayerList(true);
                GameTitle.Instance.EditTitle("the president may peek at a person's party membership");
            }
        }

        string _possibleInvestigatedParty ="";
        void OnPlayerSelected(string playerName)
        {
            _choosePersonPanel.Show(false);
            Debug.Log("confirmar");
            _possibleInvestigatedParty = playerName;
            Debug.Log("investigate: " + playerName);

            _noticePanel.SetText(NOTICE_TITLE, BODY_1 + _possibleInvestigatedParty + BODY_2);
            _noticePanel.RemoveAllListeners();
            _noticePanel.AddListeners(OnInvestigationConfirmed, OnInvestigationHazy);
            _noticePanel.Show(true);
        }

        void OnInvestigationConfirmed()
        {
            _choosePersonPanel.Show(false);
            _playerList.EnablePlayerButtons(false);

            SHPlayer investigatedParty = PlayerManager.Instance.FindPlayer(_possibleInvestigatedParty);
            investigatedParty.SetInvestigated();

            string partyName = "FASCIST";
            if(investigatedParty.IsLiberal)
            {
                partyName = "LIBERAL";
            }

            string noticeText = _possibleInvestigatedParty + INVESTIGATE_BODY_1 + partyName + INVESTIGATE_BODY_2;
            _noticePanel.SetText(INVESTIGATE_TITLE, noticeText);

            _noticePanel.RemoveAllListeners();
            _noticePanel.AddListeners(OnFinishedInvestigation, OnNotFinishedInvestigation);
            _noticePanel.Show(true);
        }

        void OnInvestigationHazy()
        {
            Debug.Log("non confirmar");
            _noticePanel.Show(false);
            _choosePersonPanel.Show(true);
            _playerList.ShowPlayerList(true);
            _playerList.EnablePlayerButtons(true);
            _playerList.DisablePlayer(_gameState.PresidentName);
        }


        void OnFinishedInvestigation()
        {
            _noticePanel.Show(false);
            Debug.Log("confirmar");
            _gameState.SetNextPresident();
        }

        void OnNotFinishedInvestigation()
        {
            Debug.Log("non confirmar");
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