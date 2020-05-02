using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPolicyDeck : MonoBehaviour
{
    public PolicyCardDeck _deck;

    public bool drawThree = false;
    public bool peekThree = false;
    public bool drawTop = false;
    // Update is called once per frame
    void Update()
    {
        if(drawThree)
        {
            List<PolicyType> draw = _deck.DrawThree();
            string cards = "";
            foreach(PolicyType pol in draw)
            {
                cards += pol.ToString() + " . ";
                _deck.DiscardPolicy(pol);
            }
            Debug.Log("DRAW THREEL " + cards);
            drawThree = false;
        }
        if (peekThree)
        {
            List<PolicyType> draw = _deck.PeekThree();
            string cards = "";
            foreach (PolicyType pol in draw)
            {
                cards += pol.ToString() + " . ";
            }
            Debug.Log("Peek THREE " + cards);
            peekThree = false;
        }
        if (drawTop)
        {
            PolicyType draw = _deck.GetTopPolicy();
                        
            Debug.Log("DRAW THREEL " + draw);

            _deck.DiscardPolicy(draw);
            drawTop = false;
        }

    }
}
