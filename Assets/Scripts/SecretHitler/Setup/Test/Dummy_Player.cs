using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Player : SHPlayer
{
    // Start is called before the first frame update
    protected override void Start()
    {
        PlayerManager.Instance.RegisterNewPlayer(this);
        gameObject.name = _name;
    }

    public InsertedVote _dummyVote = InsertedVote.Ja;

    public override InsertedVote Vote
    {
        get
        {
            return _dummyVote;
        }
        set { }                    
    }
}
