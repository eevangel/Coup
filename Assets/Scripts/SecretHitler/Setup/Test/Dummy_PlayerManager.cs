using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_PlayerManager : PlayerManager
{
    public List<SHPlayer> _dummyPlayers = new List<SHPlayer>();

    private void Start()
    {
        foreach(SHPlayer player in _dummyPlayers )
        {
            RegisterNewPlayer(player);
        }    
    }
}
