using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public abstract class CoupCharacterData 
{
    public CharacterType _character;
    public Sprite _characterLook;
    public string _text;

    public virtual CharacterType GetCharacter() { return _character; }
}

public class ContessaData : CoupCharacterData
{
    public ContessaData()
    {
        _character = CharacterType.CONTESSA;
        _text = "Blocks Assassination";    
    }
}

public class DukeData : CoupCharacterData
{
    public DukeData()
    {
        _character = CharacterType.DUKE;
        _text = "Take 3 coins from Treasury. Blocks Foreign Aid";
    }
}

public class AmbassadorData : CoupCharacterData
{
    public AmbassadorData()
    {
        _character = CharacterType.AMBASSADOR;
        _text = "Exchange cards with Court Deck. Blocks stealing";
    }
}

public class CaptainData : CoupCharacterData
{
    public CaptainData()
    {
        _character = CharacterType.CAPTAIN;
        _text = "Steal 2 coins from another player. Blocks stealing";
    }
}

public class AssassinData : CoupCharacterData
{
    public AssassinData()
    {
        _character = CharacterType.ASSASSIN;
        _text = "Pay 3 coins to assassinate another player.";
    }
}



public enum CharacterType
{
    CONTESSA,
    ASSASSIN,
    DUKE,
    CAPTAIN,
    AMBASSADOR
}
