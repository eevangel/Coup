using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRoleUI : MonoBehaviour
{
    public TextMeshProUGUI _role;  
    public TextMeshProUGUI _party;
    public Image _roleImage;

    public RoleCardDefinition _roleLibrary;
    public Button _openUI;
    public Button _closeUI;
    public Image _bkgd;
    public GameObject _rolePanel;

    public Color _libColor;
    public Color _fasColor;
        
    private void Start()
    {
        _openUI.onClick.AddListener(OpenClicked);
        _closeUI.onClick.AddListener(CloseClicked);

        _role.text = "Kaiser Soze";
        _party.text = "unaffiliated";
        Show(false);
    }

    public void Show(bool shouldShow)
    {
        _rolePanel.SetActive(shouldShow);
        _openUI.gameObject.SetActive(!shouldShow);            
    }

    void OpenClicked()
    {
        Show(true);
    }

    void CloseClicked()
    {
        Show(false);
    }

    public void UpdateData(bool isLiberal, bool isHitler)
    {
        if(isLiberal)
        {
            _role.text = "LIBERAL";
            _party.text = "LIBERAL";
            _roleImage.sprite= _roleLibrary.GetRandomLib();
            _bkgd.color = _libColor;
        }
        else
        {
            _role.text = "fascist";
            _party.text = "FASCIST";
            _bkgd.color = _fasColor;
            if (isHitler)
            {
                _role.text = "HITLER";
                _roleImage.sprite = _roleLibrary._hitler;
            }
            else
            {
                _roleImage.sprite = _roleLibrary.GetRandomFas();

            }
        }
        Show(true);
    }

    
    
}
