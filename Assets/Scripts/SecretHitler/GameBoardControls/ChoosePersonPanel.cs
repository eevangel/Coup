using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoosePersonPanel : MonoBehaviour
{
    public TextMeshProUGUI _body;
    public Button _hideButton;

    private void Awake()
    {
        _hideButton.onClick.AddListener(HideNotice);
    }

    void HideNotice()
    {
        gameObject.SetActive(false);
    }

    public void Show(bool ShouldShow)
    {
        gameObject.SetActive(ShouldShow);
    }

    public void SetText( string body)
    {
        _body.text = body;
    }

}
