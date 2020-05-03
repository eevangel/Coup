using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeckManager : MonoBehaviour
{
    const int NUM_PER_CHARACTER = 3;

    public Stack<CoupCharacterData> _DrawPile = new Stack<CoupCharacterData>();
    List<CoupCharacterData> _DiscardPile = new List<CoupCharacterData>();


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
            _DrawPile.Push(new ContessaData());
            _DrawPile.Push(new DukeData());
            _DrawPile.Push(new AmbassadorData());
            _DrawPile.Push(new CaptainData());
            _DrawPile.Push(new AssassinData());
        }
        ShuffleDeck();
    }

    void ShuffleDeck()
    {
        List<CoupCharacterData> tempDeck = new List<CoupCharacterData>(_DrawPile);
        tempDeck.AddRange(_DiscardPile);

        for (int i = 0; i < numShuffles; i++)
        {
            Shuffle(ref tempDeck);
        }

        _DrawPile.Clear();
        _DrawPile = new Stack<CoupCharacterData>(tempDeck);
    }

    private static System.Random rng = new System.Random(System.DateTime.Now.Millisecond);

    void Shuffle(ref List<CoupCharacterData> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CoupCharacterData value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    #endregion shuffle

    #region deal

    public CoupCharacterData DrawOne()
    {
        return _DrawPile.Pop();
    }

    public void DiscardOne(CoupCharacterData discard)
    {
        _DrawPile.Push(discard);
        ShuffleDeck();
    }

    #endregion deal
}
