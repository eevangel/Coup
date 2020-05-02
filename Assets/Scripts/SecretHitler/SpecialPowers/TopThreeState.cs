using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;



namespace SHGame
{
    public class TopThreeState : IFlowState
    {

        public ChoosePersonPanel _choosePersonPanel;
        public NoticePanel _noticePanel;
        public PolicyPanel _policyPanel;

        const string PRESIDENT_NOTICE_TITLE = "view the top three cards";
        const string PRESIDENT_NOTICE_BODY = "Have you looked at the top three cards and memorized them?";
        const string OTHER_CHOICE = "PLEASE WAIT WHILE THE PRESIDENT SECRETLY LOOKS AT THE NEXT THREE CARDS... THEN TOTALLY GOSSIP ABOUT THEIR WIERD NOSE.";


        public override FlowState GetFlowState()
        {
            return FlowState.INVESTIGATE_PARTY;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.TOP_THREE)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_NOMINATED)
                    .Goto(FlowState.CHOOSE_CABINET);
        }

        public override void EnterState()
        {
            Debug.Log("entered choose cabinet");
            if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                _gameState.RecieveTopThree = RecievedNextCards;
                _gameState.RequestTopThree();
                GameTitle.Instance.EditTitle("VIEW THE NEXT THREE CARDS");
            }
            else
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(OTHER_CHOICE);
                GameTitle.Instance.EditTitle("president will now peek at the next three policies");
            }
        }

        public void RecievedNextCards(List<PolicyType>nextCards)
        {
            _policyPanel.ShowInactiveCards(nextCards);
            _policyPanel.Show(true);

            _noticePanel.RemoveAllListeners();
            _noticePanel.AddListeners(OnSeenCards, OnNotSeenCards);
            _noticePanel.SetText(PRESIDENT_NOTICE_TITLE, PRESIDENT_NOTICE_BODY);
            _noticePanel.Show(true);
        }

        void OnSeenCards()
        {
            Debug.Log("confirmar");
            _noticePanel.Show(false);
            _policyPanel.Show(false);

            _gameState.SetNextPresident();
        }

        void OnNotSeenCards()
        {
            Debug.Log("non confirmar");
        }
        

        public override void ExitState()
        {
            _gameState._specialPower = PresidentialSpecialPowerType.NONE;
            _gameState.RecieveTopThree = (topThree) => { };

            _policyPanel.Show(false);
            _choosePersonPanel.Show(false);
            _noticePanel.RemoveAllListeners();
            _noticePanel.Show(false);
        }
    }
}