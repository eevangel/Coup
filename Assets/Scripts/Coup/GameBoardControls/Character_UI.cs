using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character_UI : MonoBehaviour
{
    List<CoupCharacterData> _characters = new List<CoupCharacterData>();
    public TextMeshProUGUI _char1;
    public TextMeshProUGUI _char2;

    public void SetupCharacters(List<CoupCharacterData> characters, bool ShouldShow)
    {
        _characters = characters;

        if(ShouldShow)
        {
            _char1.text = _characters[0]._character.ToString();
            _char2.text = _characters[1]._character.ToString();
        }
    }


}
