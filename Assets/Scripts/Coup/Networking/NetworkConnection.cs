// --------------------------------------------------------------------------------------------------------------------
//based off of "ConnectAndJoinRandom.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities, 
// </copyright>
// <summary>
//  Simple component to call ConnectUsingSettings and to get into a PUN room easily.
// </summary>
// <remarks>
//  A custom inspector provides a button to connect in PlayMode, should AutoConnect be false.
//  </remarks>                                                                                               
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace ee.Pun
{
    /// <summary>Simple component to call ConnectUsingSettings and to get into a PUN room easily.</summary>
    /// <remarks>A custom inspector provides a button to connect in PlayMode, should AutoConnect be false.</remarks>
    public class NetworkConnection : MonoBehaviourPunCallbacks
    {
        /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
        public bool AutoConnect = true;

        /// <summary>Used as PhotonNetwork.GameVersion.</summary>
        public byte Version = 1;

        public string _roomName = "edgar";
        public bool _connectToRandom = true;

        public delegate void InvokeJoinedRoom();
        InvokeJoinedRoom _OnJoinedRoomHandlers = () => { };

        public void AddJoinedRoomListener(InvokeJoinedRoom joinedRoom)
        {
            _OnJoinedRoomHandlers += joinedRoom;
        }

        public static NetworkConnection Instance;

        private void Awake()
        {
            if(Instance != null)
            {
                DestroyImmediate(this);
            }
            else
            {
                Instance = this;
            }
        }

        public void Start()
        {
            if (this.AutoConnect)
            {
                this.ConnectNow();
            }
        }

        public void ConnectNow()
        {
            Debug.Log("ConnectAndJoinRandom.ConnectNow() will now call: PhotonNetwork.ConnectUsingSettings().");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.Version + "." + SceneManagerHelper.ActiveSceneBuildIndex;
        }


        // below, we implement some callbacks of the Photon Realtime API.
        // Being a MonoBehaviourPunCallbacks means, we can override the few methods which are needed here.


        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN. This client is now connected to Master Server in region [" + PhotonNetwork.CloudRegion +
                "] and can join a room. Calling: PhotonNetwork.JoinRandomRoom();");
            if (_connectToRandom)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby(). This client is now connected to Relay in region [" + PhotonNetwork.CloudRegion + "]. This script now calls: PhotonNetwork.JoinRandomRoom();");
            if (_connectToRandom)
            {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed() was called by PUN. No random room available in region [" + PhotonNetwork.CloudRegion + "], so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 8 }, null);
        }

        public void JoinDefinedRoom(string roomName)
        {
            _roomName = roomName;
            PhotonNetwork.JoinRoom(_roomName);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogFormat("joined room {0} failed. " + message + ". You will host", _roomName);
            PhotonNetwork.CreateRoom(_roomName, new RoomOptions() { MaxPlayers = 8 }, null);
        }

        // the following methods are implemented to give you some context. re-implement them as needed.
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room in region [" + PhotonNetwork.CloudRegion + "]. Game is now running.");
            _OnJoinedRoomHandlers();
        }

        public void EnterName(string name)
        {
            PhotonNetwork.LocalPlayer.NickName = name;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("player {0} entered room", newPlayer.NickName);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("player {0} left room", otherPlayer.NickName);
            PlayerManager.Instance.RemovePlayer(otherPlayer);


        }
    }

}
