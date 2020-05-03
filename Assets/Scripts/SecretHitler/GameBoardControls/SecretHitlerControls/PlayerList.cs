using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerList : MonoBehaviour
{
    public TextMeshProUGUI _playerList;
    public List<Button> _playerButtons;
    public List<PlayerListButton> _activePlayerButtons;
    public GameObject _listParent;
    public Button _playerListButton;
    public Button _closeButton;
    Image _panelImage;

    private void Start()
    {
        _playerListButton.onClick.AddListener(OnPlayerButtonClicked);
        _closeButton.onClick.AddListener(OnCloseClicked);
        _panelImage = GetComponent<Image>();
        ShowPlayerList(false);

        foreach(Button playerButton in _playerButtons)
        {
            PlayerListButton listComponent = playerButton.GetComponent<PlayerListButton>();
            playerButton.onClick.AddListener(listComponent.OnClick);
        }
    }

    public void RefreshPlayerList(List<string> playerList)
    {        
        int index = 0;
        _activePlayerButtons.Clear();
        for (index = 0; index < playerList.Count; index++)
        {
            _playerButtons[index].GetComponentInChildren<TextMeshProUGUI>().text = playerList[index];
            _playerButtons[index].transform.parent.gameObject.SetActive(true);
            _playerButtons[index].gameObject.name = playerList[index];
            _playerButtons[index].interactable = false;
            _activePlayerButtons.Add(_playerButtons[index].GetComponent<PlayerListButton>());
        }
        for(; index < _playerButtons.Count; index++)
        {
            _playerButtons[index].transform.parent.gameObject.SetActive(false);
        }
    }

    public void EnablePlayerButtons(bool shouldEnable)
    {
        foreach(Button playerButton in _playerButtons)
        {
            if (playerButton.IsActive())
            {
                playerButton.interactable = shouldEnable;
            }
        }
    }
    
    public void DisableListOfPlayers(List<string> playerList)
    {
        if(playerList.Count > 0)
        {
            foreach(Button player in _playerButtons)
            {
                foreach(string playerName in playerList)
                {
                    if(player.gameObject.name == playerName)
                    {
                        player.interactable = false;
                    }
                }
            }
        }
    }
       
    public void DisablePlayer(string disableMe)
    {
        foreach (Button player in _playerButtons)
        {
            if (player.gameObject.name == disableMe)
            {
                player.interactable = false;
            }
        }
    }

    public void ShowPlayerList(bool shouldShow)
    {
        _listParent.SetActive(shouldShow);
        _panelImage.enabled = shouldShow;
        _playerListButton.gameObject.SetActive(!shouldShow);
    }

    public void OnPlayerButtonClicked()
    {
        ShowPlayerList(true);
    }

    public void OnCloseClicked()
    {
        ShowPlayerList(false);
    }

    public void AddListenerToActivePlayerButtons(System.Action<string> newListener)
    {
        foreach(PlayerListButton player in _activePlayerButtons)
        {
            player.OnButtonClicked = newListener;
        }
    }
    
    public void ShowPossibleVotes(bool shouldShow)
    {
        foreach(PlayerListButton player in _activePlayerButtons)
        {
            player.ShowVote(shouldShow);
        }
    }

    public void ShowThatPlayerVoted(string playerName)
    {
        foreach(PlayerListButton player in _activePlayerButtons)
        {
            if (player.name == playerName)
            {
                player.ShowThatUserEnteredVote();
            }
        }
    }

    public void ShowPlayerVote(string playerName, InsertedVote vote)
    {
        foreach (PlayerListButton player in _activePlayerButtons)
        {
            if (player.name == playerName)
            {
                player.ShowUserVote(vote);
            }
        }
    }

}
