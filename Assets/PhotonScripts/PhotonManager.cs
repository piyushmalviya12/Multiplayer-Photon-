using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    
    #region Panels , InputFields and Buttons
    public GameObject connectionPanel, lobbyPanel, roomPanel , roomListPanel;
    public Button connectBtn, createRoomBtn, JoinRoomBtn, joinBtn, startBtn, leaveBtn, backToLobbyBtn;
    public TMP_Text clientstatetxt, playerName1, playerName2;
    public TMP_InputField playerName;
    public PhotonManager instance;
    #endregion

    private Dictionary<string, RoomInfo> roomListData;


    public void Start()
    {
         instance = this;
        connectBtn.onClick.AddListener(OnClickConnect);
        createRoomBtn.onClick.AddListener(OnClickRoomCreate);
        leaveBtn.onClick.AddListener(OnClickLeaveBtn);
        JoinRoomBtn.onClick.AddListener(OnClickJoinRoomBtn);
        backToLobbyBtn.onClick.AddListener(OnClickBackToLobbyBtn);

        roomListData = new Dictionary<string, RoomInfo>();
    }
    public void Update()
    {
        clientstatetxt.text = PhotonNetwork.NetworkClientState.ToString();
    }
    public void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public void onDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #region UIMethods

    public void OnClickConnect()
    {
        var nameText = playerName.text;
        if (!string.IsNullOrEmpty(nameText))
        {
            PhotonNetwork.LocalPlayer.NickName = nameText;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void OnClickRoomCreate()
    {
       var roomName = Random.Range(0, 1000).ToString();
        var roomOptions = new RoomOptions
        {
            MaxPlayers = 2,
            IsVisible = true
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }
    public void OnClickLeaveBtn()
    {
        PhotonNetwork.Disconnect();
    }

    public void OnClickJoinRoomBtn()
    {
        
        roomListPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        Debug.Log("Join Room Btn Clicked!!");
    }


    public void OnClickBackToLobbyBtn()
    {
        roomListPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        Debug.Log("Back To Lobby Btn Clicked!!");
    }





    #endregion



    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnected()
    {
        Debug.Log("Connected To Internet()");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster()");
        connectionPanel.SetActive(false);
        lobbyPanel.SetActive(true);
       
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected");
        roomPanel.SetActive(false);
        connectionPanel.SetActive(true);
        Debug.LogWarningFormat("OnDisconnected(){0}" + cause);

    }

    #endregion
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " room Created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " room Joined");
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            playerName1.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            playerName2.text = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var rooms in roomList)
        {
            Debug.Log(" Room Name "+rooms.Name);
            roomListData.Add(rooms.Name, rooms);
        }
        
    }

    /*public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        // PhotonNetwork.CreateRoom(, new RoomOptions(MaxPlayers = 2));
    }
*/
    /* public override void OnJoinedRoom()
     {
         Debug.Log("Room Joined );
             roomPanel.SetActive(true);
         lobbyPanel.SetActive(false);
     }*/
}
