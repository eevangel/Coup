using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public SH_PlayerSetup _setup;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            _setup.AssignRoles();
        }
    }
}
