using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;


namespace SHGame
{
    public class EnactPolicyState : IFlowState
    {
        public FascistBoard _fascist;
        public LiberalBoard _liberal;

        public override FlowState GetFlowState()
        {
            return FlowState.ENACT_POLICY;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.ENACT_POLICY)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_NOMINATED)
                    .Goto(FlowState.CHOOSE_CABINET)
                .On(FlowEvents.LIBERALS_WIN)
                    .Goto(FlowState.END_GAME)
                .On(FlowEvents.FASCISTS_WIN)
                    .Goto(FlowState.END_GAME)
            //special powers------------------------
                .On(FlowEvents.TOP_THREE)
                    .Goto(FlowState.TOP_THREE)
                .On(FlowEvents.INVESTIGATE_PARTY)
                    .Goto(FlowState.INVESTIGATE_PARTY)
                .On(FlowEvents.PICK_PRESIDENT)
                    .Goto(FlowState.PICK_PRESIDENT)
                .On(FlowEvents.KILL_PLAYER)
                    .Goto(FlowState.KILL_PLAYER);

        }

        public override void EnterState()
        {
            Debug.Log("enter ENACT_POLICY state");
            if (_gameState._enactPolicy == PolicyType.Fascist)
            {
                _gameState._specialPower = _fascist.EnactNewPolicy();
                GameTitle.Instance.EditTitle("A FASCIST POLICY IS BORN!");
            }
            else
            {
                _liberal.EnactNewPolicy();
                GameTitle.Instance.EditTitle("A LIBERAL POLICY HAS PASSED!");
            }

            checkEndGameState();
        }

        void checkEndGameState()
        {
            //if no new president
            if (_fascist.NumPolicies >= 6)
            {
                _gameState.SendFascistsWin();
                return;
            }

            if (_liberal.NumPolicies >= 6)
            {
                _gameState.SendLiberalsWin();
                return;
            }

            _gameState.SetPastCabinet();
            _gameState.SendClearChancellor();

            if (_gameState._specialPower == PresidentialSpecialPowerType.NONE)
            {
                _gameState.SetNextPresident();
            }
            else
            {
                _gameState.SendPresidentGetsPower();
            }
        }

        public override void ExitState()
        {
            Debug.Log("leaving ENACT_POLICY state");

        }
    }
}