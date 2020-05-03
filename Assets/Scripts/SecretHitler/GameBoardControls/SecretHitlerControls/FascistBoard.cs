using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FascistBoard : MonoBehaviour
{
    public int NumPolicies { get; private set; }

    public Color _nonActiveSub2;
    public Color _enactedSub2;
    public Color _nonActivePOST2;
    public Color _enactedInPOST2;
    public FascistBoardSetup _setupData;

    public List<FascistBoardTile> _fascistTiles = new List<FascistBoardTile>();
    public TextMeshProUGUI _instructions;
    public TextMeshProUGUI _hoverText;
    public GameObject _hoverPanel;

    public GameObject _investigate1;
    public GameObject _investigate2;
    public GameObject _topThree;
    public GameObject _pick;


    private void Start()
    {
        ResetBoard();
    }

    public void SetupBoard(int numPlayers)
    {
        FascistBoardDefinition boardDefinition = _setupData.GetFascistBoardDefinition(numPlayers);
        _instructions.text = boardDefinition._instructions;
        
        EnableIcons(boardDefinition._minPlayers);

        for (int i=0; i<boardDefinition._specialTileData.Count; i++ )
        {
            _fascistTiles[i].SetupHoverData(boardDefinition._specialTileData[i]);
            _fascistTiles[i]._hoverText = _hoverText;
            _fascistTiles[i]._hoverPanel = _hoverPanel;
            
        }
    }

    void EnableIcons(int minPlayers)
    {
        switch(minPlayers)
        {
            case 5:
                _investigate1.SetActive(false);
                _investigate2.SetActive(false);
                _topThree.SetActive(true);
                _pick.SetActive(false);
                break;
            case 7:
                _investigate1.SetActive(false);
                _investigate2.SetActive(true);
                _topThree.SetActive(false);
                _pick.SetActive(true);
                break;
            case 9:
                _investigate1.SetActive(true);
                _investigate2.SetActive(true);
                _topThree.SetActive(false);
                _pick.SetActive(true);
                break;
        }
    }


    public void ResetBoard()
    {
        NumPolicies = 0;
        for(int i=0; i< 6; i++)
        {
            BoardTileData tile = _fascistTiles[i]._data;
            if (i < 3)
            {
                tile._bkgd.color = _nonActiveSub2;
            }
            else
            {
                tile._bkgd.color = _nonActivePOST2;
            }
            tile.SetText("");
        }
    }

    public PresidentialSpecialPowerType EnactNewPolicy()
    {
        BoardTileData tile = _fascistTiles[NumPolicies]._data;
        if (NumPolicies < 3)
        {
            tile._bkgd.color = _enactedSub2;
        }
        else
        {
            tile._bkgd.color = _enactedInPOST2;
        }
        tile.SetText("FASCIST");

        return _fascistTiles[NumPolicies++].GetPower();
    }
}
