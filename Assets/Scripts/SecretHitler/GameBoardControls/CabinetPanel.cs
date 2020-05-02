using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CabinetPanel : MonoBehaviour
{
    const string NO_PLACEMENT = "non";
    const string POSSIBLE_CAB = "possible CABINET";
    const string ELECTED_CAB = "ELECTED CABINET"; //asd

    public TextMeshProUGUI _cabinet;

    public TextMeshProUGUI _president;
    public TextMeshProUGUI _chancie;

    public void SetPresident(string playerName)
    {
        _president.text = playerName;
    }

    public void SetChancellor(string playerName)
    {
        _chancie.text = playerName;
    }

    public void ClearPres()
    {
        _president.text = NO_PLACEMENT;
    }
    public void ClearChancie()
    {
        _chancie.text = NO_PLACEMENT;
    }

    public void SetPossibleCabinet()
    {
        _cabinet.text = POSSIBLE_CAB;
    }

    public void SetElectedCab()
    {
        _cabinet.text = ELECTED_CAB;
    }

}
