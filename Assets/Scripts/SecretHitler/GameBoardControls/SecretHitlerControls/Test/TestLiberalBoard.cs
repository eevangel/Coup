using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLiberalBoard : MonoBehaviour
{
    public LiberalBoard _board;

    public bool _resetBoard = false;
    public bool _enactPolicy = false;

    // Update is called once per frame
    void Update()
    {
        if(_resetBoard)
        {
            _board.ResetBoard();
            _resetBoard = false;
        }
        if (_enactPolicy)
        {
            _board.EnactNewPolicy();
            Debug.Log("numpolicies " + _board.NumPolicies.ToString());
            _enactPolicy = false;
        }
    }
}
