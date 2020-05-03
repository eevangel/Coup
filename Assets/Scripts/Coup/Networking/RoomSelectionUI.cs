
using System;
using UnityEngine;
using UnityEngine.UI;
using ee.Pun;
using TMPro;
using Photon.Pun;


public class RoomSelectionUI : MonoBehaviour
{
    public GameObject _RoomMenu;
    public GameObject _NameEntry;

    public TextMeshProUGUI _roomMessage;
    public TMP_InputField _nameField;
    public Button _nameButton;
    public TMP_InputField _roomField;
    public Button _roomButton;

    public Button _startGameButton;

    public NetworkConnection _network;

    public Button _credits;
    public GameObject _creditsPanel;
    public Button _closeCredits;

    const string NAME_KEY = "PLAYER_NAME";
    const string ROOM_KEY= "ROOM_NAME";

    const string MASTER_POST_JOIN = "Click Start when everyone is in.";
    const string OTHER_POST_JOIN = "Tell everyone a big lie so they trust you.";

    bool _hasChosenName = false;

    private void Start()
    {
        if(PlayerPrefs.HasKey(NAME_KEY))
        {
            _nameField.text = PlayerPrefs.GetString(NAME_KEY);
        }

        if (PlayerPrefs.HasKey(ROOM_KEY))
        {
            _roomField.text = PlayerPrefs.GetString(ROOM_KEY);
        }
        _RoomMenu.SetActive(false);
        _NameEntry.SetActive(true);
        _startGameButton.gameObject.SetActive(false);

        _nameButton.onClick.AddListener(OnNameEntrySubmit);
        _roomButton.onClick.AddListener(OnRoomEntrySubmit);
        _startGameButton.onClick.AddListener(OnStartGameClicked);

        _credits.onClick.AddListener(ShowCredits);
        _closeCredits.onClick.AddListener(HideCredits);
        _creditsPanel.SetActive(false);

        NetworkConnection.Instance.AddJoinedRoomListener(OnJoinedRoom);
    }

    void ShowCredits()
    {
        _creditsPanel.SetActive(true);
    }

    void HideCredits()
    {
        _creditsPanel.SetActive(false);
    }

    void OnNameEntrySubmit()
    {
        Debug.Log("chose " + _nameField.text);
        PlayerPrefs.SetString(NAME_KEY, _nameField.text);

        _RoomMenu.SetActive(true);
        _NameEntry.SetActive(false);

        _network.EnterName(_nameField.text);
        _hasChosenName = true;
    }

    void OnRoomEntrySubmit()
    {
        Debug.Log("chose " + _roomField.text);
        PlayerPrefs.SetString(ROOM_KEY, _roomField.text);

        _network.JoinDefinedRoom(_roomField.text);
    }

    void OnJoinedRoom()
    {
        _RoomMenu.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            _startGameButton.gameObject.SetActive(true);
            _roomMessage.text = MASTER_POST_JOIN;
        }
        else
        {
            _roomMessage.text = OTHER_POST_JOIN;
        }
    }

    public void EnterRoomState()
    {
        if (_hasChosenName)
        {
            _RoomMenu.SetActive(true);
        }
        else
        {
            _NameEntry.SetActive(true);
        }

        _startGameButton.gameObject.SetActive(false);
    }

    public Action StartGameInvoker = () => { };

    void OnStartGameClicked()
    {
        _startGameButton.gameObject.SetActive(false);

        StartGameInvoker();
    }

    

}
