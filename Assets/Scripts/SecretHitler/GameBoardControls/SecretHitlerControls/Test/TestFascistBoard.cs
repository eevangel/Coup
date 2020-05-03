using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFascistBoard : MonoBehaviour
{
    public FascistBoard _board;

    public bool _resetBoard = false;
    public bool _enactPolicy = false;
    public int _numPlayers=5;
    
    // Update is called once per frame
    void Update()
    {
        if (_resetBoard)
        {
            _board.ResetBoard();
            _board.SetupBoard(_numPlayers);
            _resetBoard = false;
        }
        if (_enactPolicy)
        {
            PresidentialSpecialPowerType power = _board.EnactNewPolicy();
            Debug.Log("numpolicies " + _board.NumPolicies.ToString() + "|" + power.ToString());
            _enactPolicy = false;
        }
    }
}
