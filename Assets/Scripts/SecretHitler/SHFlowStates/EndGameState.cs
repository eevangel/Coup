using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;


namespace SHGame
{
    public class EndGameState : IFlowState
    {
        public EndGameScreen _endgame;

        public override FlowState GetFlowState()
        {
            return FlowState.END_GAME;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.END_GAME)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.RESET_GAME)
                    .Goto(FlowState.NO_GAME);
        }

        public override void EnterState()
        {
            switch(_gameState._winCondition)
            {
                case WinCondition.FASCIST_POLICY:
                    _endgame.SetBrag("fascists gently create a love-filled dictatorship by spreading uberviolent fascist love throughout the wonderful world.");
                    _endgame.WinGame(true);

                    GameTitle.Instance.EditTitle("FASCISTS WIN");
                    break;
                case WinCondition.LIBERAL_POLICY:
                    _endgame.SetBrag("libs conquer all benevolently by passing all the destructive liberal nonsense policies amongst the sheeple");
                    _endgame.WinGame(false);
                    GameTitle.Instance.EditTitle("LIBERAL WIN");
                    break;
                case WinCondition.HITLER_CHANCELLOR:
                    _endgame.SetBrag("you've just made hitler chancellor. What the hell were you thinking? I hope you're happy.");
                    _endgame.WinGame(true);
                    GameTitle.Instance.EditTitle("FASCISTS WIN");
                    break;
                case WinCondition.KILL_HITLER:
                    _endgame.SetBrag("you've made everyone that much less fascist by killing one man. I hope you're happy. his family isn't.");
                    _endgame.WinGame(false);
                    GameTitle.Instance.EditTitle("LIBERAL WIN");
                    break;
            }

        }

        public override void ExitState()
        {
            _endgame.Show(false);
        }
    }
}
