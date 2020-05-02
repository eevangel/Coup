using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSetupUI : MonoBehaviour
{
    public TextMeshProUGUI _setupMessage;
    public Button _SetupCompleteButton;
    public Button _RedoSetup;

    const string MASTER_START_TEXT = "Assign the roles.";
    const string OTHER_START_TEXT = "Accuse each other of being Otto von Wilhelms and useless bureaucrats until roles have actually been assigned.";

    const string MASTER_POST_ROLES_TEXT = "Run the setup audio first. Then let the plebians wallow in their own filth for awhile";
    const string OTHER_POST_ROLES_TEXT = "Check your role. And then act like a filthy liberal. . . Yes... \"Act\"... Also complain to the host if the role assignment isn't what you're looking for.";

    public SH_PlayerSetup _PlayerSetup;
    public PlayerRoleUI _roleUI;

    public System.Action _onSetupComplete = () => { };


    private void Start()
    {
        _SetupCompleteButton.onClick.AddListener(SetupCompleteClicked);
        _RedoSetup.onClick.AddListener(RedoSetupClicked);
    }

    public void StartSetup(bool isMasterClient)
    {
        gameObject.SetActive(true);
        _SetupCompleteButton.gameObject.SetActive(false);
        _RedoSetup.gameObject.SetActive(false);
        if (isMasterClient)
        {
            _setupMessage.text = MASTER_START_TEXT;
            _PlayerSetup.AssignRoles();
        }
        else
        {
            _setupMessage.text = OTHER_START_TEXT;
        }
    }

    public void ShowPostRolesAssigned( bool isMasterClient)
    {
        if (isMasterClient)
        {
            _setupMessage.text = MASTER_POST_ROLES_TEXT;
            _SetupCompleteButton.gameObject.SetActive(true);
            _RedoSetup.gameObject.SetActive(true);
        }
        else
        {
            _setupMessage.text = OTHER_POST_ROLES_TEXT;
        }
        _roleUI.UpdateData(SHPlayer.LocalInstance.IsLiberal, SHPlayer.LocalInstance.IsHitler);
    }
   

    void SetupCompleteClicked()
    {
        _onSetupComplete();
    }

    void RedoSetupClicked()
    {
        _PlayerSetup.AssignRoles();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
