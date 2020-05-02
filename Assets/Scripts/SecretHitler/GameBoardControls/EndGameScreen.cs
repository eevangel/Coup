
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameScreen : MonoBehaviour
{
    public Image _background;
    public TextMeshProUGUI _winnerText;
    public TextMeshProUGUI _winnerBrag;

    public Color _fascistBackgroundColor;
    public Color _fascistTextColor;
    public Color _liberalBackgroundColor;
    public Color _liberalTexColort;

    public void Start()
    {
        Show(false);
    }

    public void SetBrag(string text)
    {
        _winnerBrag.text = text;
    }

    public void WinGame(bool fascistsWin)
    {
        Show(true);
        if(fascistsWin)
        {
            _background.color = _fascistBackgroundColor;
            _winnerText.color = _fascistTextColor;
            _winnerBrag.color = _fascistTextColor;

            _winnerText.text = "): FASCISTS WIN :(";
        }
        else
        {
            _background.color = _liberalBackgroundColor;
            _winnerText.color = _liberalTexColort;
            _winnerBrag.color = _liberalTexColort;
            
            _winnerText.text = "(: LIBERALS SUCCEED :)";
        }
    }

    public void Show(bool shouldShow)
    {
        gameObject.SetActive(shouldShow);
    }

}
