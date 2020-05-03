using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayout : MonoBehaviour
{
    public PlayerLayoutDefinition _definition;

    public List<CoupPlayer_UI> _playerPositions = new List<CoupPlayer_UI>();
    public CoupPlayer_UI _localPlayer;

    public void Awake()
    {
        _definition.CreateDictionary();
    }

    private void Start()
    {
        Show(false);
    }

    public void Show(bool shouldShow)
    {
        gameObject.SetActive(shouldShow);
    }

    public void SetupLocalPlayer(CoupPlayerData player)
    {
        _localPlayer.SetData(player);
    }


    public void LayoutPlayers(List<CoupPlayerData>players)
    {
        foreach(CoupPlayer_UI playerUI in _playerPositions)
        {
            playerUI.Show(false);
        }

        List<int> layout=_definition.LayoutForPlayers(players.Count);
        int playerDataIndex = 0;

        foreach(int layoutIndex in layout)
        {
            _playerPositions[layoutIndex].SetData(players[playerDataIndex++]);
            _playerPositions[layoutIndex].Show(true);
        }
    }

}
