using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ElectionTracker : MonoBehaviour
{
    public TextMeshProUGUI _fail1;
    public TextMeshProUGUI _fail2;
    public TextMeshProUGUI _fail3;

    int _electionFailures = 0;

    public System.Action OnElectionFailure = () => { };

    public void ResetElectionTracker()
    {
        _electionFailures = 0;

        _fail1.text = ".";
        _fail2.text = ".";
        _fail3.text = ".";
    }

    public void IncrementElectionFailure()
    {
        _electionFailures++;

        switch (_electionFailures)
        {
            case 1:
                _fail1.text = "O";
                _fail2.text = ".";
                _fail3.text = ".";
                break;
            case 2:
                _fail1.text = ".";
                _fail2.text = "O";
                _fail3.text = ".";
                break;
            case 3:
                _fail1.text = ".";
                _fail2.text = ".";
                _fail3.text = "O";
                break;
            case 4:
                _fail1.text = "X";
                _fail2.text = "X";
                _fail3.text = "X";
                _electionFailures = 0;
                OnElectionFailure();
                break;
        }

    }
}
