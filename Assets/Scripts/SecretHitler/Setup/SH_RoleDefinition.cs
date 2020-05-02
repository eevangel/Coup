using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnRoleDefinition", order = 1)]
public class SH_RoleDefinition : ScriptableObject
{

    public List<PlayerRoles> _roleDefinition = new List<PlayerRoles>();
    public int _fascistPolicyNum;
    public int _liberalPolicyNum;

    public PlayerRoles GetRoles(int numPlayers)
    {
        foreach(PlayerRoles roles in _roleDefinition)
        {
            if(numPlayers == roles._players)
            {
                return roles;
            }
        }
        Debug.LogWarning("Couldn't find roles for number of players");
        return null;
    } 
}


[Serializable]
public class PlayerRoles
{    
    public int _players;
    public int _liberals;
    public int _fascists;
}
