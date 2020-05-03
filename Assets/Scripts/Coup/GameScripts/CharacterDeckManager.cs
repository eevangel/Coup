using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeckManager : MonoBehaviour
{
    const int NUM_PER_CHARACTER = 3;

    public Stack<CharacterType> _DrawPile = new Stack<CharacterType>();
    List<CharacterType> _DiscardPile = new List<CharacterType>();


    public int numShuffles = 12;

    private void Start()
    {
        InitialShuffle();
    }

    #region shuffle
    void InitialShuffle()
    {
        for(int i=0; i < NUM_PER_CHARACTER; i++)
        {
            _DrawPile.Push(CharacterType.AMBASSADOR);
            _DrawPile.Push(CharacterType.ASSASSIN);
            _DrawPile.Push(CharacterType.CAPTAIN);
            _DrawPile.Push(CharacterType.CONTESSA);
            _DrawPile.Push(CharacterType.DUKE);
        }
        ShuffleDeck();
    }

    void ShuffleDeck()
    {
        List<CharacterType> tempDeck = new List<CharacterType>(_DrawPile);
        tempDeck.AddRange(_DiscardPile);

        for (int i = 0; i < numShuffles; i++)
        {
            Shuffle(ref tempDeck);
        }

        _DrawPile.Clear();
        _DrawPile = new Stack<CharacterType>(tempDeck);
    }

    private static System.Random rng = new System.Random(System.DateTime.Now.Millisecond);

    void Shuffle(ref List<CharacterType> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CharacterType value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    #endregion shuffle

    #region deal

    public CharacterType DrawOne()
    {
        return _DrawPile.Pop();
    }

    public void DiscardOne(CharacterType discard)
    {
        _DrawPile.Push(discard);
        ShuffleDeck();
    }

    #endregion deal
}
