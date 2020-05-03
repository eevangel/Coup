using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class NoticePanel : MonoBehaviour
{
    public TextMeshProUGUI _title;
    public TextMeshProUGUI _body;
    public Button _yes;
    public Button _no;

    public delegate void NoticeButtonListener();

    public void SetText(string title, string body)
    {
        _title.text = title;
        _body.text = body;
    }

    public void RemoveAllListeners()
    {
        _yes.onClick.RemoveAllListeners();
        _no.onClick.RemoveAllListeners();
        _yes.interactable = false;
        _no.interactable = false;
    }

    public void AddListeners(UnityAction yesListener, UnityAction noListener)
    {
        _yes.onClick.AddListener(yesListener);
        _no.onClick.AddListener(noListener);
        _yes.interactable = true;
        _no.interactable = true;
    }

    public void Show(bool shouldShow)
    {
        gameObject.SetActive(shouldShow);
        
    }
}
