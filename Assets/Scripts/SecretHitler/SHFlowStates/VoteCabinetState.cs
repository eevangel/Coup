using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using Photon.Realtime;


namespace SHGame
{
    public class VoteCabinetState : IFlowState
    {
        public VotingPanel _voting;
        public VotingManager _voteManager;
        public PlayerList _players;
        public NoticePanel _noticePanel;
        
        const string NOTICE_TITLE = "Finalize Voting";
        const string BODY_1 = "Are you sure you'd like all votes finalized?";
        const string NOTICE_TITLE_2 = "hurry up, buddy";
        const string BODY_2 = "Well hit Ja when you are sure";

        public override FlowState GetFlowState()
        {
            return FlowState.VOTE_CABINET;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);
                       
            _stateMachine.In(FlowState.VOTE_CABINET)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.VOTE_SUCCEED)
                    .Goto(FlowState.CHOOSE_POLICY_PRESIDENT)
                .On(FlowEvents.ENOUGH_VOTES_FAILED)
                    .Goto(FlowState.VOTES_FAILED)
                .On(FlowEvents.HITLER_CHANCELLOR)
                    .Goto(FlowState.END_GAME);

            _voting.OnVoteEntered += OnVoteEntered;
            _voteManager.OnAllVotesEntered += OnAllVotesEntered;
        }

        public override void EnterState()
        {
            _voting.ShouldEnable(true);
            SHPlayer.LocalInstance.Vote = InsertedVote.NONE;

            _players.EnablePlayerButtons(false);
            _players.ShowPlayerList(true);
            _players.ShowPossibleVotes(true);
            GameTitle.Instance.EditTitle("VOTE ON THE CABINET MEMBERS");
        }

        void OnVoteEntered(InsertedVote vote)
        {
            Debug.Log("chose Vote " + vote.ToString());
            SHPlayer.LocalInstance.Vote = vote;
        }

        void OnAllVotesEntered()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                _noticePanel.Show(true);
                _noticePanel.SetText(NOTICE_TITLE, BODY_1);
                _noticePanel.AddListeners(OnVotesFinalized, OnVotesNotFinalized);
            }
        }

        void OnVotesFinalized()
        {
            _noticePanel.Show(false);

            bool voteOutcome = _voteManager.CalculateVoteOutcome();
            bool isHitlerEndGame = false;
            if(voteOutcome)
            {
                isHitlerEndGame = _gameState.isHitlerVotedChancellor();                
            }
            if (!isHitlerEndGame)
            {
                _gameState.SendCabinetElectionOutcome(voteOutcome);
            }
            else
            {
                _gameState.SendHitlerChancellorEndGame();
            }
        }

        void OnVotesNotFinalized()
        {
            _noticePanel.SetText(NOTICE_TITLE_2, BODY_2);
        }

        public override void ExitState()
        {
            _voting.ShouldEnable(false);
            _voteManager.ShowAllFinalizedVotes();
        }
    }
}

