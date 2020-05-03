using System;
using UnityEngine;
using UnityEngine.UI;

public class VotingPanel : MonoBehaviour
{
    public Button _ja;
    public Button _nein;

    public Action<InsertedVote> OnVoteEntered;

    private void Start()
    {
        _ja.onClick.AddListener(OnJaPressed);
        _nein.onClick.AddListener(OnNeinPressed);
    }

    public void ShouldEnable(bool enable)
    {
        _ja.interactable = enable;
        _nein.interactable = enable;
    }

    void OnJaPressed()
    {
        OnVoteEntered(InsertedVote.Ja);
    }

    void OnNeinPressed()
    {
        OnVoteEntered(InsertedVote.Nein);
    }

}

public enum InsertedVote
{
    Ja,
    Nein,
    NONE
}
