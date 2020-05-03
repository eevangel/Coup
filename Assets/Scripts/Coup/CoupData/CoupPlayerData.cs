using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoupPlayerData
{
    public string _name;
    public List<CoupCharacterData> _characters = new List<CoupCharacterData>();
    public int _coins = 0;

    public List<CharacterType> CharacterTypes
    {
        get
        {
            List<CharacterType> types = new List<CharacterType>();
            foreach(CoupCharacterData character in _characters)
            {
                types.Add(character.GetCharacter());
            }
            return types;
        }
    }
    public bool isFullyRevealed
    {
        get
        {
            foreach(CoupCharacterData character in _characters)
            {
                if(!character.isRevealed)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public CoupPlayerData() { }

    public void ConstructCharacters(List<CharacterType> characters)
    {
        foreach (CharacterType character in characters)
        {
            AddCharacter(character);
        }
    }

    public void AddCharacter(CharacterType car)
    {
        switch (car)
        {
            case CharacterType.AMBASSADOR:
                _characters.Add(new AmbassadorData());
                break;
            case CharacterType.ASSASSIN:
                _characters.Add(new AssassinData());
                break;
            case CharacterType.CAPTAIN:
                _characters.Add(new CaptainData());
                break;
            case CharacterType.DUKE:
                _characters.Add(new DukeData());
                break;
            case CharacterType.CONTESSA:
                _characters.Add(new ContessaData());
                break;
        }
    }

    public bool RemoveCharacter(CharacterType car)
    {
       foreach(CoupCharacterData carData in _characters)
        {
            if(carData.GetCharacter() == car)
            {
                _characters.Remove(carData);
                return true;
            }
        }
        return false;
    }
    
    public void AddCoins(int numCoins)
    {
        _coins += numCoins;
        _coins = Mathf.Max(_coins, 0);
    }
}
