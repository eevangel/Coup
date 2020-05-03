using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTitle : MonoBehaviour
{
    public static GameTitle Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }

    }


    public TextMeshProUGUI _text;

    public void EditTitle(string text)
    {
        _text.text = text;
    }
}
