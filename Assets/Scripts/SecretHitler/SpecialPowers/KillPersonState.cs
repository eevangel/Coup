using UnityEngine;
using Appccelerate.StateMachine;
using Photon.Pun;
using TMPro;



namespace SHGame
{
    public class KillPersonState : IFlowState
    {
        public PlayerList _playerList;
        public ChoosePersonPanel _choosePersonPanel;
        public NoticePanel _noticePanel;

        const string PRESIDENT_CHOICE = "Please select a person from the right to NOT EXIST anymore. They will die. but it will be a nice death, with lots of flowers and knives.";
        const string OTHER_CHOICE = "Tell the president who to kill. They will die in a very sexy death. Super sexy. Walter white in tighty whities sexy.";


        const string NOTICE_TITLE = "kill a dude dead";
        const string BODY_1 = "Are you sure you want to kill ";
        const string BODY_2 = "? I mean they barely did anything fascist. Other than lying to your face the whole game. and life. soooooo much in life.";

        public override FlowState GetFlowState()
        {
            return FlowState.KILL_PLAYER;
        }

        public override void SetupState(PassiveStateMachine<FlowState, FlowEvents> sm)
        {
            base.SetupState(sm);

            _stateMachine.In(FlowState.KILL_PLAYER)
                .ExecuteOnEntry(EnterState)
                .ExecuteOnExit(ExitState)
                .On(FlowEvents.PRESIDENT_NOMINATED)
                    .Goto(FlowState.CHOOSE_CABINET)
                .On(FlowEvents.KILLED_HITLER)
                    .Goto(FlowState.END_GAME);
        }

        public override void EnterState()
        {
            Debug.Log("entered kill dude");
            if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(PRESIDENT_CHOICE);

                _playerList.ShowPlayerList(true);
                _playerList.EnablePlayerButtons(true);
                _playerList.DisablePlayer(_gameState.PresidentName);
                _playerList.AddListenerToActivePlayerButtons(OnPlayerSelected);
                GameTitle.Instance.EditTitle("CHOOSE A PERSON TO DIE");
            }
            else
            {
                _choosePersonPanel.Show(true);
                _choosePersonPanel.SetText(OTHER_CHOICE);
                GameTitle.Instance.EditTitle("the president will choose a person to be executed");
            }
        }

        string _possibleDeadDude = "";
        void OnPlayerSelected(string playerName)
        {
            _choosePersonPanel.Show(false);
            Debug.Log("confirmar");
            _possibleDeadDude = playerName;

            _noticePanel.SetText(NOTICE_TITLE, BODY_1 + _possibleDeadDude + BODY_2);
            _noticePanel.RemoveAllListeners();
            _noticePanel.AddListeners(OnKillConfirmed, OnKillRejected);
            _noticePanel.Show(true);
        }

        void OnKillConfirmed()
        {
            Debug.Log("confirmar");
            _noticePanel.Show(false);

            _playerList.EnablePlayerButtons(false);
            SHPlayer deadDude = PlayerManager.Instance.FindPlayer(_possibleDeadDude);

            deadDude.Kill();

            if (deadDude.IsHitler)
            {
                _gameState.SendKilledHitler();
            }
            else
            {
                _gameState.RequestNewPresidentOrder();
                _gameState.RequestAnnounceKill(_possibleDeadDude);
            }
        }

        void OnKillRejected()
        {
            _noticePanel.Show(false);
            Debug.Log("non confirmar");

            _choosePersonPanel.Show(true);
            _playerList.ShowPlayerList(true);
        }


        const string OTHER_ANNOUNCE = "The benevolent president has chosen to kill the following person through suffocation under a mound of mashed beets: ";

        const string OTHER_VETO = ". Also the Cabinet may veto policies now if they don't like them. Both President and Chancellor must agree when choosing a policy.";

        const string ANNOUNCE_TITLE = "you totally killed a dude";
        const string ANNOUNCE_BODY_1 = "You've announced that you've killed";
        const string ANNOUNCE_BODY_2 = ". Confirm that everyone has finished mourning their sweet, sweet demise.";
        const string ANNOUNCE_BODY_VETO = "And now the cabinet can veto policies.";

        public void AnnounceKill(string deadName)
        {
            if (SHPlayer.LocalInstance.IsPresident || (PhotonNetwork.IsMasterClient && _gameState.IsPresidentDummy()))
            {
                if (_gameState.IsVetoUnlocked)
                {
                    _noticePanel.SetText(ANNOUNCE_TITLE, ANNOUNCE_BODY_1 + deadName + ANNOUNCE_BODY_2 + ANNOUNCE_BODY_VETO);
                }
                else
                {
                    _noticePanel.SetText(ANNOUNCE_TITLE, ANNOUNCE_BODY_1 + deadName + ANNOUNCE_BODY_2);
                }

                _noticePanel.RemoveAllListeners();
                _noticePanel.AddListeners(OnFinishMourning, OnNotFinishedMourning);
                _noticePanel.Show(true);
            }
            else
            {
                _choosePersonPanel.Show(true);
                if (_gameState.IsVetoUnlocked)
                {
                    _choosePersonPanel.SetText(OTHER_ANNOUNCE + deadName + OTHER_VETO);
                }
                else
                {
                    _choosePersonPanel.SetText(OTHER_ANNOUNCE + deadName);
                }
            }
        }

        public void OnFinishMourning()
        {
            _gameState.SetNextPresident();
        }

        public void OnNotFinishedMourning()
        {
        }

        public override void ExitState()
        {
            _gameState._specialPower = PresidentialSpecialPowerType.NONE;
            _choosePersonPanel.Show(false);
            _noticePanel.RemoveAllListeners();
            _noticePanel.Show(false);

            _playerList.EnablePlayerButtons(false);
        }
    }

}
