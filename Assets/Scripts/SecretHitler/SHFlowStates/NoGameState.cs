using UnityEngine;
using Appccelerate.StateMachine;


namespace SHGame
{
    public class NoGameState : IFlowState
    {
        public PolicyCardDeck _drawPile;
        public GameObject _flowPanel;
        public ElectionTracker _electionTracker;

        public FascistBoard _fascistBoard;
        public LiberalBoard _liberalBoard;

        public override FlowState GetFlowState()
        {
            return FlowState.NO_GAME;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.NO_GAME)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.START_GAME)
                    .Goto(FlowState.CHOOSE_PRESIDENT);
        }

        public override void EnterState()
        {
            _flowPanel.SetActive(false);
        }

        public override void ExitState()
        {
            ResetGameBoard();

            _flowPanel.SetActive(true);
            Debug.Log("show game board");

        }

        void ResetGameBoard()
        {
            _fascistBoard.ResetBoard();
            _fascistBoard.SetupBoard(PlayerManager.Instance.NumPlayers);
            _liberalBoard.ResetBoard();

            _electionTracker.ResetElectionTracker();
            _drawPile.InitialShuffle();
        }
    }
}

