using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class FascistBoardTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]    public GameObject _hoverPanel;
    [HideInInspector]    public TextMeshProUGUI _hoverText;
    public BoardTileData _data;
    public FascistSpecialTile _specialTileData;

    public bool _enableHover = false;

    public void SetupHoverData(FascistSpecialTile specialData)
    {
        _specialTileData = specialData;
        _enableHover = _specialTileData._hoverText != "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_enableHover)
        {
            _hoverText.text = _specialTileData._hoverText;
            _hoverPanel.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_enableHover)
        {
            _hoverPanel.SetActive(false);
            _hoverText.text = "";
        }
    }

    public PresidentialSpecialPowerType GetPower()
    {
        return _specialTileData._type;
    }
}
