using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEndGame : MonoBehaviour
{
    public EndGameScreen _endgame;

    public bool libwin = false;
    public bool faswin = false;
    public bool hide = false;

    public void Update()
    {
        if(libwin)
        {
            _endgame.SetBrag("libs conquer all benevolently");
            _endgame.WinGame(false);
            libwin = false;
        }
        if (faswin)
        {
            _endgame.SetBrag("fascists gently create a love-filled dictatorship");
            _endgame.WinGame(true);
            faswin = false;
        }
        if(hide)
        {
            _endgame.Show(false);
            hide = false;
        }
    }
}
