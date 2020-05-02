using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiberalBoard : MonoBehaviour
{
    public int NumPolicies { get; private set; }
    
    public Color _nonActive;
    public Color _enacted;

    public List<BoardTileData> _tiles = new List<BoardTileData>();

    private void Start()
    {
        ResetBoard();
    }

    public void ResetBoard()
    {
        NumPolicies = 0;
        foreach(BoardTileData tile in _tiles)
        {
            tile._bkgd.color = _nonActive;
            tile.SetText("");
        }
    }

    public void EnactNewPolicy()
    {
        _tiles[NumPolicies]._bkgd.color = _enacted;
        _tiles[NumPolicies].SetText("LIBERAL");

        NumPolicies++;
    }
}

[Serializable]
public class BoardTileData
{
    public Image _bkgd;
    public TextMeshProUGUI _text;
   
    public string _hoverText;

    public BoardTileData()
    {
        _hoverText = "";
    }

    public void SetText(string text)
    {
        _text.text = text;
    }

    public void SetHoverOverText(string text)
    {
        _hoverText = text;
    }
}
