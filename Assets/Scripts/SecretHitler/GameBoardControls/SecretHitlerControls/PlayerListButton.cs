using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerListButton : MonoBehaviour
{
    public System.Action<string> OnButtonClicked = (str) => { };
    public TextMeshProUGUI _vote;

    public void ShowVote(bool shouldShow)
    {
        _vote.gameObject.SetActive(shouldShow);
    }

    public void ShowThatUserEnteredVote()
    {
        _vote.text = "V";
    }

    public void ShowUserVote(InsertedVote vote)
    {
        _vote.text = vote.ToString();
    }


    public void OnClick()
    {
        Debug.Log("player" + gameObject.name + "was clicked");
        OnButtonClicked(gameObject.name);
    }

}
