using System.Collections.Generic;
using UnityEngine;

public class SH_PlayerSetup : MonoBehaviour
{
    public SH_RoleDefinition _roleDefinition;
    PlayerRoles _roles;

    public PlayerManager _playerManager;

    public System.Action _rolesAssigned = () => { };

    //needs to assign roles, and make someone president first
    public void AssignRoles()
    {
        //get players
        List<SHPlayer> players = _playerManager.Players;
        
        //get num players
        int numPlayersUnassigned = players.Count;

        //get role definition
        PlayerRoles roles = _roleDefinition.GetRoles(numPlayersUnassigned);


        //randomly        
        int randomIndex = Random.Range(0, players.Count);
        players[randomIndex].SetHitler();
        players[randomIndex].BroadcastRole();
        players.RemoveAt(randomIndex);        
        
        //assign fascists
        for(int i=0; i< roles._fascists; i++)
        {
            randomIndex = Random.Range(0, players.Count);
            players[randomIndex].SetFascist();
            players[randomIndex].BroadcastRole();
            players.RemoveAt(randomIndex);
        }

        //set the rest to liberal
        foreach(SHPlayer player in players)
        {
            player.SetLiberal();
            player.BroadcastRole();
        }

        Debug.Log("roles are assigned");
        _rolesAssigned();
    }


}
