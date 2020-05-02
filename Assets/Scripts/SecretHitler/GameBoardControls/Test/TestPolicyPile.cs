using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPolicyPile : MonoBehaviour
{
    PolicyPanel _panel;

    public List<PolicyType> _policies = new List<PolicyType>();

    // Start is called before the first frame update
    void Start()
    {
        _panel = GetComponentInChildren<PolicyPanel>();
        _panel.PolicyHasBeenSelected += OnSelected;
        _panel.VetoClicked += OnVeto;
    }


    public bool showPolicy = false;
    public bool showPolicyCards = false;

    public bool showVeto = false;
    public bool hideVeto = false;

    List<PolicyType> _results = new List<PolicyType>();

    public  void OnSelected()
    {
        List<PolicyType> results = new List<PolicyType>();
        PolicyType discard = _panel.SetupResults(ref results);

        string pass = "";
        foreach(PolicyType result in results)
        {
            pass += result.ToString() + ".";
        }

        Debug.LogFormat("pass: {0} | DISCARD {1}", pass, discard.ToString());
    }

    void OnVeto()
    {
        Debug.Log("VetoClicked");
    }

    // Update is called once per frame
    void Update()
    {
        if(showPolicy)
        {
            _panel.Show(!_panel._policyPanel.activeSelf);
            showPolicy = false;
        }

        if(showPolicyCards)
        {
            _panel.ShowPolicyCards(_policies);
            showPolicyCards = false;
        }

        if (showVeto)
        {
            _panel.ShowVetoButton(true);
            showVeto = false;
        }

        if (hideVeto)
        {
            _panel.ShowVetoButton(false);
            hideVeto = false;
        }
    }
}
