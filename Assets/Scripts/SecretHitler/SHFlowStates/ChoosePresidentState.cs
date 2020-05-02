using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using TMPro;


namespace SHGame
{
    public class ChoosePresidentState : IFlowState
    {
        public GameObject _ChoosePresidentPanel;
        public PlayerList _playerList;
        public TextMeshProUGUI _PanelText;
        public NoticePanel _noticePanel;

        const string HOST_CHOICE = "Please choose a person from the right to be president. Ask around who wants it most!";
        const string OTHER_CHOICE = "First to lick their nose gets to be president!";

        public override FlowState GetFlowState()
        {
            return FlowState.CHOOSE_PRESIDENT;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.CHOOSE_PRESIDENT)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_NOMINATED)
                    .Goto(FlowState.CHOOSE_CABINET);
        }

        public override void EnterState()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                _PanelText.text = HOST_CHOICE;
                _playerList.ShowPlayerList(true);
                _playerList.EnablePlayerButtons(true);
                _playerList.AddListenerToActivePlayerButtons(OnPlayerChosen);
                GameTitle.Instance.EditTitle("CHOOSE THE STARTING PRESIDENT");
            }
            else
            {
                _PanelText.text = OTHER_CHOICE;
                _playerList.ShowPlayerList(true);
                _playerList.EnablePlayerButtons(false);
                GameTitle.Instance.EditTitle("talk about who will choose the starting president");
            }
            _ChoosePresidentPanel.SetActive(true);
        }

        string _possiblePresident;
        const string NOTICE_TITLE = "choose a president";
        const string BODY_1 = "Are you sure you'd like to nominate ";
        const string BODY_2 = " as the president?";

        void OnPlayerChosen(string playerName)
        {
            _possiblePresident = playerName;
            _ChoosePresidentPanel.SetActive(false);

            _noticePanel.SetText(NOTICE_TITLE, BODY_1 + _possiblePresident + BODY_2);
            _noticePanel.RemoveAllListeners();
            _noticePanel.AddListeners(OnPresidentConfirmed, OnPresidentNotConfirmed);
            _noticePanel.Show(true);
        }

        void OnPresidentConfirmed()
        {
            Debug.Log("confirmar");
            _noticePanel.Show(false);

            if (!_gameState.ConfirmPresident(_possiblePresident))
            {
                Debug.LogWarning("player not found");
                OnPresidentNotConfirmed();
            }
        }

        void OnPresidentNotConfirmed()
        {
            _noticePanel.Show(false);
            Debug.Log("non confirmar");

            _ChoosePresidentPanel.SetActive(true);
        }

        public override void ExitState()
        {
            _playerList.ShowPlayerList(false);
            _playerList.EnablePlayerButtons(false);
            _ChoosePresidentPanel.SetActive(false);
            Debug.Log("exiting choosing a president");
        }
    }
}