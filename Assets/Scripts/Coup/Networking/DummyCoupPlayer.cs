using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyCoupPlayer : CoupPlayer
{
    // Start is called before the first frame update
    protected override void Start()
    {
        CoupPlayerManager.Instance.RegisterNewPlayer(this);
        gameObject.name = _name;
        _data._name = _name;
    }
}

