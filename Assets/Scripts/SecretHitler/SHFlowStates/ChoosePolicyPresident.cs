using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using Photon.Realtime;


namespace SHGame
{
    public class ChoosePolicyPresident : IFlowState
    {
        public NoticePanel _notice;
        public PolicyPanel _policies;
        public ChoosePersonPanel _choosePanel;
        public ElectionTracker _electionTracker;

        const string NOTICE_TITLE = "select a discarded policy";
        const string NOTICE_BODY_1 = "Are you sure you'd like to pass the policies ";
        const string NOTICE_BODY_2 = " to your chancellor? How much do you trust that ... that ... ew\n\nAnd you'll discard a ";
        const string NOTICE_BODY_3 = " policy. Best make sure now, while you have a choice.";


        const string CHOOSE_PRESIDENT = "Select the policy below that you'd like to discard. Choose wisely or perish mediocre-ly.\nAlso NO TALKING. SHUSH.";
        const string CHOOSE_NON_PRESIDENT = " is your FAAAABULOUS president / DICTATOR for life / PRIMO MINESTRONE of all things / MUFASA / ALPHA and the OMEGA!  \n...at least until this turn ends. \nMake sure they choose wisely by making a burn book about them.";
        public override FlowState GetFlowState()
        {
            return FlowState.CHOOSE_POLICY_PRESIDENT;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);
            _stateMachine.In(FlowState.CHOOSE_POLICY_PRESIDENT)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_CHOSE)
                    .Goto(FlowState.CHOOSE_POLICY_CHANCELLOR);
        }

        public override void EnterState()
        {
            if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                Debug.Log("choose policy president state");
                GameTitle.Instance.EditTitle("CHOOSE TO DISCARD A POLICY");

                _gameState.PresidentRequestPolicies();
            }
            else
            {

                GameTitle.Instance.EditTitle("the president will draw policies and pass 2 to the chancellor");
                string choose = _gameState.PresidentName + CHOOSE_NON_PRESIDENT;
                _choosePanel.SetText(choose);
                _choosePanel.Show(true);
            }
            _electionTracker.ResetElectionTracker();
        }

        public void OnReceivedDrawCards(List<PolicyType> drawnCards)
        {
            _policies.ShowPolicyCards(drawnCards);
            _policies.Show(true);
            _policies.PolicyHasBeenSelected = OnPolicySelected;

            _notice.RemoveAllListeners();
            _notice.AddListeners(OnPoliciesConfirmed, OnPoliciesNotConfirmed);

            _choosePanel.SetText(CHOOSE_PRESIDENT);
            _choosePanel.Show(true);
        }


        List<PolicyType> _passedPolicies = new List<PolicyType>();
        PolicyType _discarded;
        void OnPolicySelected()
        {
            _passedPolicies  = new List<PolicyType>();
            _discarded = _policies.SetupResults(ref _passedPolicies);

            string bodyText = NOTICE_BODY_1 + _passedPolicies[0].ToString().ToUpper() + " and " + _passedPolicies[1].ToString().ToUpper();
            bodyText += NOTICE_BODY_2 + _discarded.ToString().ToUpper() + NOTICE_BODY_3;

            _notice.SetText(NOTICE_TITLE, bodyText);
            _notice.Show(true);
            _choosePanel.Show(false);

        }

        void OnPoliciesConfirmed()
        {
            Debug.Log("confirmar");
            _notice.Show(false);
            _policies.Show(false);

            _gameState.PresidentChosePolicy(_passedPolicies, _discarded);
        }

        void OnPoliciesNotConfirmed()
        {
            Debug.Log("non confirmar");
            _notice.Show(false);
            _choosePanel.Show(true);
        }


        public override void ExitState()
        {
            _policies.Show(false);

            _choosePanel.Show(false);
        }
    }
}

