using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FascistBoardSetup", order = 1)]
public class FascistBoardSetup : ScriptableObject
{
    public List<FascistBoardDefinition> _boardDefinitions = new List<FascistBoardDefinition>();

    public FascistBoardDefinition GetFascistBoardDefinition(int numPlayers)
    {
        foreach(FascistBoardDefinition definition in _boardDefinitions)
        {
            if(definition._minPlayers <= numPlayers && numPlayers <= definition._maxPlayers)
            {
                return definition;
            }
        }
        return null;
    }
}

[Serializable]
public class FascistBoardDefinition
{
    public int _minPlayers;
    public int _maxPlayers;

    public string _instructions;

    public List<FascistSpecialTile> _specialTileData = new List<FascistSpecialTile>();
}

[Serializable]
public class FascistSpecialTile
{
    public string _hoverText;
    public PresidentialSpecialPowerType _type;
}

public enum PresidentialSpecialPowerType
{
    NONE,
    TOP_THREE,
    KILL,
    KILL_AND_VETO,
    INVESTIGATE,
    PICK_NEXT_PRESIDENT
}
