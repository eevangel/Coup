using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using Photon.Realtime;


namespace SHGame
{
    public class VotesFailedState : IFlowState
    {
        public ChoosePersonPanel _noticePanel;
        public ElectionTracker _electionTracker;

        const string FAILURE_1 = "Votes failed! Elections are useless! Let's change the president to ";
        const string FAILURE_2 = ".\n Let's see if this nominee is smart enough to nominate an electable chancellor!";


        public override FlowState GetFlowState()
        {
            return FlowState.VOTES_FAILED;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.VOTES_FAILED)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_NOMINATED)
                    .Goto(FlowState.CHOOSE_CABINET)
                .On(FlowEvents.PASSING_POLICY)
                    .Goto(FlowState.ENACT_POLICY);

            _electionTracker.OnElectionFailure += OnElectionFailure;
        }

        public override void EnterState()
        {
            Debug.Log("entervotes failed state");

            _electionTracker.IncrementElectionFailure();
                       
            _gameState.SendClearChancellor();
            _gameState.SetNextPresident();

            _noticePanel.SetText(FAILURE_1 + _gameState.PresidentName + FAILURE_2);
            _noticePanel.Show(true);
            GameTitle.Instance.EditTitle("CABINET FAILED TO GET THE VOTES!");

        }

        void OnElectionFailure()
        {
            Debug.Log("elections failed");
            _gameState.ElectionsFailed();
        }

        public override void ExitState()
        {
            Debug.Log("leaving votes failed state");

        }
    }
}

