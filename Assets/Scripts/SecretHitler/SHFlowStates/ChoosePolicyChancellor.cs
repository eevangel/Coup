using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using Photon.Realtime;


namespace SHGame
{
    public class ChoosePolicyChancellor : IFlowState
    {
        public NoticePanel _notice;
        public PolicyPanel _policies;
        public ChoosePersonPanel _choosePanel;

        const string NOTICE_TITLE = "enact a policy";
        const string NOTICE_BODY_1 = "Are you sure you'd like to ENACT a ";
        const string NOTICE_BODY_2 = " policy? This is like, for sure-sure, forever ever-ever policy. No takesy-backsies.";

        const string CHOOSE_CHANCELLOR = "Select a policy that WILL be ENACTED. NO TALKING. Be mute. now. ... shush... \n\n there you go...";
        const string CHOOSE_NON_CHANCELLOR = " is the chancellor who will enact a policy. How much do you trust this person? Remember what they did in second grade. That was totally messed up. They'll probably mess this up too.";

        public override FlowState GetFlowState()
        {
            return FlowState.CHOOSE_POLICY_CHANCELLOR;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);
            _stateMachine.In(FlowState.CHOOSE_POLICY_CHANCELLOR)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PASSING_POLICY)
                    .Goto(FlowState.ENACT_POLICY)
                .On(FlowEvents.VETO_SUCCEEDED)
                    .Goto(FlowState.VOTES_FAILED);

            _policies.VetoClicked = OnVetoClicked;
        }

        public override void EnterState()
        {
            if (SHPlayer.LocalInstance.IsChancellor || (PhotonNetwork.IsMasterClient && _gameState.IsChancellorDummy()))
            {
                Debug.Log("choose policy chancellor state");

                _gameState.ChancellorRequestPolicies();
                GameTitle.Instance.EditTitle("SELECT A POLICY");
            }
            else
            {
                string choose = _gameState.PresidentName + CHOOSE_NON_CHANCELLOR;
                _choosePanel.SetText(choose);
                _choosePanel.Show(true);

                GameTitle.Instance.EditTitle("chancellor will select a policy");
            }

        }

        public void OnReceivedDrawCards(List<PolicyType> drawnCards)
        {
            if(_gameState.IsVetoUnlocked)
            {
                _policies.ShowVetoButton(true);
            }
            _policies.ShowPolicyCards(drawnCards);
            _policies.Show(true);
            _policies.PolicyHasBeenSelected = OnPolicySelected;


            _choosePanel.SetText(CHOOSE_CHANCELLOR);
            _choosePanel.Show(true);
        }


        List<PolicyType> _passedPolicies = new List<PolicyType>();
        PolicyType _discarded;
        void OnPolicySelected()
        {
            _passedPolicies = new List<PolicyType>();
            _discarded = _policies.SetupResults(ref _passedPolicies);

            string bodyText = NOTICE_BODY_1 + _passedPolicies[0].ToString().ToUpper() + NOTICE_BODY_2;

            _notice.RemoveAllListeners();
            _notice.AddListeners(OnPoliciesConfirmed, OnPoliciesNotConfirmed);

            _notice.SetText(NOTICE_TITLE, bodyText);
            _notice.Show(true);
            _choosePanel.Show(false);
        }

        void OnPoliciesConfirmed()
        {
            Debug.Log("confirmar");
            _notice.Show(false);
            _policies.Show(false);
            _gameState.ChancellorChosePolicy(_passedPolicies[0], _discarded);
        }

        void OnPoliciesNotConfirmed()
        {
            Debug.Log("non confirmar");
        }


        #region veto

        void OnVetoClicked()
        {
            Debug.Log("veto clicked");
            _gameState.SendVetoRequest();
        }

        const string NOTICE_VETO_TITLE = "veto please?";
        const string NOTICE_VETO_BODY = "The Chancellor has formally requested a veto. Approve?" ;

        public void OnVetoRequest()
        {
            if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                Debug.Log("veto requested");
                _notice.SetText(NOTICE_VETO_TITLE, NOTICE_VETO_BODY);
                _notice.RemoveAllListeners();
                _notice.AddListeners(OnVetoSucceed, OnVetoDeny);
                _notice.Show(true);
                _choosePanel.Show(false);

            }
        }

        void OnVetoSucceed()
        {
            _gameState.SendVetoResponse(true);
        }

        void OnVetoDeny()
        {
            _gameState.SendVetoResponse(false);
            _notice.Show(false);
        }

        public void OnRecievedVetoDenial()
        {
            _policies.ShowVetoButton(false);
        }

        public override void ExitState()
        {
            _notice.Show(false);
            _policies.Show(false);
            _choosePanel.Show(false);
            _policies.ShowVetoButton(false);
        }

        #endregion veto
    }
}

