using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoupPlayer_UI : MonoBehaviour
{
    public CoupPlayerData _data;

    public TextMeshProUGUI _name;
    public Action_UI _action;
    public Character_UI _characters;
    public TextMeshProUGUI _money;

    public void Show(bool shouldShow)
    {
        gameObject.SetActive(shouldShow);
    }

    public void SetData(CoupPlayerData data)
    {
        _data = data;
        gameObject.name = _data._name;
        _name.text = _data._name;

        _characters.SetupCharacters(_data._characters, true);
        _money.text = _data._coins.ToString();
    }

    //refresh money, characters,actions
    
}
