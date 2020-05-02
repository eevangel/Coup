using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolicyCardDeck : MonoBehaviour
{
    public SH_RoleDefinition _gameRules;

    public Stack<PolicyType> _DrawPile = new Stack<PolicyType>();
    List<PolicyType> _DiscardPile = new List<PolicyType>();

    public int numShuffles = 12;

    private void Start()
    {
        InitialShuffle();
    }

    public void InitialShuffle()
    {
        _DrawPile.Clear();
        _DiscardPile.Clear();
        for (int i = 0; i < _gameRules._fascistPolicyNum; i++)
        {
            _DrawPile.Push(PolicyType.Fascist);
        }
        for (int i = 0; i < _gameRules._liberalPolicyNum; i++)
        {
            _DrawPile.Push(PolicyType.Liberal);
        }
        ShuffleDeck();
    }


    void ShuffleDeck()
    {
        List<PolicyType> tempDeck = new List<PolicyType>(_DrawPile);
        tempDeck.AddRange(_DiscardPile);

        for (int i = 0; i < numShuffles; i++)
        {
            Shuffle(ref tempDeck);
        }

        _DrawPile.Clear();
        _DrawPile = new Stack<PolicyType>(tempDeck);
    }

    private static System.Random rng = new System.Random(System.DateTime.Now.Millisecond);

    void Shuffle(ref List<PolicyType> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            PolicyType value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public List<PolicyType>DrawThree()
    {
        if(_DrawPile.Count < 3)
        {
            ShuffleDeck();
        }
        List<PolicyType> topthree = new List<PolicyType>();

        topthree.Add(_DrawPile.Pop());
        topthree.Add(_DrawPile.Pop());
        topthree.Add(_DrawPile.Pop());
        
        return topthree;
    }

    public List<PolicyType>PeekThree()
    {        
        if (_DrawPile.Count < 3)
        {
            ShuffleDeck();
        }
        List<PolicyType> topthree = new List<PolicyType>();

        topthree.Add(_DrawPile.Pop());
        topthree.Add(_DrawPile.Pop());
        topthree.Add(_DrawPile.Pop());

        _DrawPile.Push(topthree[2]);
        _DrawPile.Push(topthree[1]);
        _DrawPile.Push(topthree[0]);

        return topthree;
    }

    public PolicyType GetTopPolicy()
    {
        if (_DrawPile.Count < 1)
        {
            ShuffleDeck();
        }

        return _DrawPile.Pop();
    }

    public void DiscardPolicy(PolicyType policy)
    {
        _DiscardPile.Add(policy);
    }

}

