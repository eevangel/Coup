using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VotingManager : MonoBehaviour
{
    public PlayerList _playerList;
    public System.Action OnAllVotesEntered = () => { };

    private void Start()
    {
        PlayerManager.Instance.OnNewPlayerRegistered += OnNewPlayerRegistered;
    }

    void OnNewPlayerRegistered(SHPlayer player)
    {
        player.OnVoteUpdated += OnVoteUpdated;
    }

    void OnVoteUpdated(string player)
    {
        _playerList.ShowThatPlayerVoted(player);

        if(PlayerManager.Instance.HaveAllPlayersVoted())
        {
            OnAllVotesEntered();
        }
    }

    public void ShowAllFinalizedVotes()
    {
        List<SHPlayer> players = PlayerManager.Instance.Players;

        foreach(SHPlayer player in players)
        {
            _playerList.ShowPlayerVote(player.Name, player.Vote);
        }

        _playerList.ShowPlayerList(true);
    }

    public bool CalculateVoteOutcome()
    {
        List<SHPlayer> players = PlayerManager.Instance.Players;

        int numVotes = 0;
        int numJas = 0;
        foreach (SHPlayer player in players)
        {
            if (!player.IsKilled)
            {
                if (player.Vote == InsertedVote.Ja)
                {
                    numJas++;
                }
                numVotes++;
            }
        }
        Debug.LogFormat("ja: {0} | na: {1}",numJas.ToString(), numVotes.ToString());
        numVotes -= numJas;
        Debug.LogFormat("ja: {0} | na/2: {1}  => {2}", numJas.ToString(), numVotes.ToString(), (numJas >= numVotes).ToString());

        return numJas >= numVotes;
    }

}
