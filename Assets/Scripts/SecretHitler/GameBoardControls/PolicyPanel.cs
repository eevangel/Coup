using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class  PolicyPanel : MonoBehaviour
{
    const string PRESIDENTIAL = "DISCARD A POLICY";
    const string CHANCELLOR = "ENACT A POLICY";
    const string TOP_THREE = "LOOK AT THE NEXT CARDS";

    public System.Action PolicyHasBeenSelected = () => { };

    public List<PolicyCard> _policyCards = new List<PolicyCard>();

    public GameObject _policyPanel;
    public TextMeshProUGUI _title;

    public PolicyCardTheming _cardTheme;
    public Button _vetoButton;

    List<PolicyType> _policies = new List<PolicyType>();

    bool isPresidential = true;

    private void Start()
    {
        _policyCards[0]._policy.onClick.AddListener(OnOnePressed);
        _policyCards[1]._policy.onClick.AddListener(OnTwoPressed);
        _policyCards[2]._policy.onClick.AddListener(OnThreePressed);
        _vetoButton.gameObject.SetActive(false);
        _vetoButton.onClick.AddListener(OnVetoClicked);
        Show(false);
    }

    public void Show(bool shouldShow)
    {
        _policyPanel.SetActive(shouldShow);
    }

    public void ShowPolicyCards(List<PolicyType>policies)
    {
        _policies = policies;

        ColorBlock fascist = _cardTheme._fascistDiscard;
        ColorBlock liberal = _cardTheme._liberalDiscard;

        _policyCards[2]._policy.gameObject.SetActive(true);
        _title.text = PRESIDENTIAL;
        isPresidential = true;

        if (policies.Count <=2)
        {
            fascist = _cardTheme._fascistKeep;
            liberal = _cardTheme._liberalKeep;
            _policyCards[2]._policy.gameObject.SetActive(false);

            _title.text = CHANCELLOR;
            isPresidential = false;
        }

        for (int i=0; i< policies.Count; i++)
        {
            _policyCards[i]._policy.interactable = true;
            if (policies[i] == PolicyType.Fascist)
            {
                _policyCards[i]._policy.colors = fascist;
                _policyCards[i]._text.text = "FASCIST";
            }
            else
            {
                _policyCards[i]._policy.colors = liberal;
                _policyCards[i]._text.text = "LIBERAL";
            }
        }
    }

    public void ShowInactiveCards(List<PolicyType> policies)
    {
        _policies = policies;

        ColorBlock fascist = _cardTheme._fascistDiscard;
        ColorBlock liberal = _cardTheme._liberalDiscard;

        _policyCards[2]._policy.gameObject.SetActive(true);
        _title.text = TOP_THREE;

        for (int i = 0; i < policies.Count; i++)
        {
            _policyCards[i]._policy.interactable = false;
            if (policies[i] == PolicyType.Fascist)
            {
                _policyCards[i]._policy.colors = fascist;
                _policyCards[i]._text.text = "FASCIST";
            }
            else
            {
                _policyCards[i]._policy.colors = liberal;
                _policyCards[i]._text.text = "LIBERAL";
            }
        }
    }



    int _selectedIndex = 0;
    void OnOnePressed()
    {
        _selectedIndex = 0;
        PolicyHasBeenSelected();
    }

    void OnTwoPressed()
    {
        _selectedIndex = 1;
        PolicyHasBeenSelected();
    }

    void OnThreePressed()
    {
        _selectedIndex = 2;
        PolicyHasBeenSelected();
    }

    public PolicyType SetupResults(ref List<PolicyType> passed)
    {
        passed.Clear();
        PolicyType discardedPolicy = PolicyType.Fascist;

        if (isPresidential)
        {
            for(int i=0; i< _policies.Count; i++)
            {
                if(i == _selectedIndex)
                {
                    discardedPolicy = _policies[i];
                }
                else
                {
                    passed.Add(_policies[i]);

                }
            }
        }
        else
        {
            for (int i = 0; i < _policies.Count; i++)
            {
                if (i == _selectedIndex)
                {
                    passed.Add(_policies[i]);
                }
                else
                {
                    discardedPolicy = _policies[i];
                }
            }
        }

        return discardedPolicy;
    }

    #region veto

    public void ShowVetoButton(bool shouldShow)
    {
        _vetoButton.gameObject.SetActive(shouldShow);
    }

    public Action VetoClicked = ()=>{};
    public void OnVetoClicked()
    {
        VetoClicked();
    }

    #endregion veto

    #region static

    public static void PrintPolicies(List<PolicyType> cards)
    {
        string card = "";
        foreach(PolicyType policy in cards)
        {
            card += policy.ToString() + " . ";
        }
        Debug.Log(card);
    }


    public static byte[] PolicyToByteArray(List<PolicyType> policies)
    {
        List<byte> bytePo = new List<byte>();
        foreach (PolicyType policy in policies)
        {
            bytePo.Add((byte)policy);
        }
        return bytePo.ToArray();
    }

    public static List<PolicyType> ByteArrToPolicyList( byte[] bypo)
    {
        List<PolicyType> policies = new List<PolicyType>();
        List<byte> bytePo = new List<byte>(bypo);
        foreach (byte byPo in bytePo)
        {
            policies.Add((PolicyType)byPo);
        }
        return policies;
    }

    #endregion static
}


[Serializable]
public class PolicyCard
{
    public Button _policy;
    public TextMeshProUGUI _text;
}


[Serializable]
public enum PolicyType
{
    Fascist,
    Liberal
}