using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLayoutDef", menuName = "CoupSO/PlayerLayout", order = 1)]
public class PlayerLayoutDefinition : ScriptableObject
{
    Dictionary<int, List<int>> _layoutDict = new Dictionary<int, List<int>>();
    public List<PlayerLayoutByPlayers> _layoutPlayers;

    public void CreateDictionary()
    {
        foreach(PlayerLayoutByPlayers layout in _layoutPlayers)
        {
            _layoutDict.Add(layout.numPlayers, layout.PlayerPos);
        }
    }

    public List<int> LayoutForPlayers(int num)
    {
        if(_layoutDict.ContainsKey(num))
        {
            return _layoutDict[num];
        }
        return null;
    }
}

[System.Serializable]
public class PlayerLayoutByPlayers
{
    public int numPlayers;
    public List<int> PlayerPos;
}
