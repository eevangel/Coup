
using UnityEngine;
using Appccelerate.StateMachine;

namespace SHGame
{
    public abstract class IFlowState : MonoBehaviour
    {
        protected PassiveStateMachine<FlowState, FlowEvents> _stateMachine;
        protected SHFlow _gameState;

        public void SetSHFlow(SHFlow flow)
        {
            _gameState = flow;
        }

        public virtual FlowState GetFlowState() { return FlowState.NO_GAME; }

        public virtual void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            _stateMachine = sm;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }

    }


    public enum FlowState
    {
        NO_GAME,
        CHOOSE_PRESIDENT,
        CHOOSE_CABINET,
        VOTES_FAILED,
        VOTE_CABINET,
        CHOOSE_POLICY_PRESIDENT,
        CHOOSE_POLICY_CHANCELLOR,
        ENACT_POLICY,
        //SPECIAL_POWERS
        TOP_THREE, INVESTIGATE_PARTY, PICK_PRESIDENT, KILL_PLAYER,
        END_GAME
    }

    public enum FlowEvents
    {
        START_GAME,
        PRESIDENT_NOMINATED,
        CHANCELLOR_NOMINATED, CABINET_CHOSEN,
        VOTE_SUCCEED, ENOUGH_VOTES_FAILED, HITLER_CHANCELLOR, KILLED_HITLER,
        PRESIDENT_CHOSE,
        PASSING_POLICY, VETO_SUCCEEDED,
        TOP_THREE, INVESTIGATE_PARTY, PICK_PRESIDENT, KILL_PLAYER,
        LIBERALS_WIN, FASCISTS_WIN,
        RESET_GAME
    }
}

