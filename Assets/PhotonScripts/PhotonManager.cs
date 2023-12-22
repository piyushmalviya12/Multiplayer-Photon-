using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class PhotonManager : MonoBehaviourPunCallbacks , ILobbyCallbacks 
{
    
    #region Panels , InputFields, Text and Buttons
    public GameObject connectionPanel, lobbyPanel, roomPanel , roomListPanel;
    public Button connectBtn, createRoomBtn, JoinRoomBtn, joinBtn, startBtn, leaveBtn, backToLobbyBtn, leaveLobbyBtn;
    public TMP_Text clientstatetxt, playerName1, playerName2;
    public TMP_InputField playerName;
    public static PhotonManager instance;

    public GameObject roomListPrefab;
    public GameObject roomListParent;
    #endregion

    private Dictionary<string, RoomInfo> roomListData;
    private Dictionary<string, GameObject> roomListObjects;

  
   
    #region Unity Methods
    public void Start()
    {
         instance = this;
        connectBtn.onClick.AddListener(OnClickConnect);
        createRoomBtn.onClick.AddListener(OnClickRoomCreate);
        leaveBtn.onClick.AddListener(OnClickLeaveBtn);
        JoinRoomBtn.onClick.AddListener(OnClickJoinRoomBtn);
        backToLobbyBtn.onClick.AddListener(OnClickBackToLobbyBtn);
        startBtn.onClick.AddListener(OnStartBtnClick);
        PhotonNetwork.AutomaticallySyncScene = true;

      // counterInstance = GetComponent<Counter>();

        roomListData = new Dictionary<string, RoomInfo>();
        roomListObjects = new Dictionary<string, GameObject>();
    }
    public void Update()
    {
        clientstatetxt.text = PhotonNetwork.NetworkClientState.ToString();
    }
    public void OnEnable()
    {
        instance=this;
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }
    public void onDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void RaiseEvt(byte code, object data, ReceiverGroup options)
    {
        RaiseEventOptions receiverOpt = new RaiseEventOptions { Receivers = options };
        PhotonNetwork.RaiseEvent(code, data, receiverOpt, SendOptions.SendReliable);
    }
    #endregion


    #region Static Data for event
    public static byte scene = 100, playerData =101, btnContent = 102;
   
    #endregion
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
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
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

    public void OnClickLeaveLobbyBtn()
    {
        roomListPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        Debug.Log("leave lobby Clicked!!");
    }


    private void RoomJoinFromList(string roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
       /* if (!PhotonNetwork.IsMasterClient)
        {
            roomListPanel.SetActive(false);
            roomPanel.SetActive(true);
            startBtn.gameObject.SetActive(false);
        }*/
        PhotonNetwork.JoinRoom(roomName);
    }
    public void ClearRoomList()
    {
        Debug.Log("Room list clear");
        foreach (var roomObject in roomListObjects.Values)
        {
            Destroy(roomObject);
        }
        roomListObjects.Clear();
    }


    public void OnStartBtnClick()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
       /* string sceneName = "GameScene";
        object[] data =
        {
             PhotonNetwork.LocalPlayer,
             sceneName
        };
        RaiseEvt(scene,data,ReceiverGroup.Others);
        SceneManager.LoadScene(sceneName);
        Debug.Log("RaiseEvt"+ scene);*/
    }

    public void RaiseClickEvent()
    {
       object[] content = new object[] { PhotonNetwork.LocalPlayer.ActorNumber };
       RaiseEvt(btnContent, content, ReceiverGroup.Others);
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

    
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " room Created");
       
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " room Joined");
       
       
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            roomPanel.SetActive(true);
            lobbyPanel.SetActive(false);
            playerName1.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
           //s playerName2.text = PhotonNetwork.LocalPlayer.NickName;
            roomPanel.SetActive(true);
            roomListPanel.SetActive(false);
            startBtn.gameObject.SetActive(false);

            playerName1.text = PhotonNetwork.PlayerList[0].NickName;
            playerName2.text = PhotonNetwork.PlayerList[1].NickName;
            object[] data =
            {
                        PhotonNetwork.LocalPlayer,
                        playerName1,
                        playerName2
                    };
            Debug.Log("player1...>" + playerName1);
            Debug.Log("player2...>" + playerName2);
            Debug.Log("Event Raise " + playerData);
            RaiseEvt(playerData, data, ReceiverGroup.All);
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomList();
        foreach (var rooms in roomList)
        {
            Debug.Log(" Room Name "+rooms.Name);
            if (!rooms.IsOpen || !rooms.IsVisible || rooms.RemovedFromList)
            {
                if (roomListData.ContainsKey(rooms.Name))
                {
                    roomListData.Remove(rooms.Name);
                }
            }
            else
            {
                if (roomListData.ContainsKey(rooms.Name))
                {
                    roomListData[rooms.Name] = rooms;
                }
                else
                {
                    roomListData.Add(rooms.Name, rooms);
                }
            }
        }


        foreach (RoomInfo roomItem in roomListData.Values)
        {
            GameObject roomListItemObject = Instantiate(roomListPrefab);
            roomListItemObject.transform.SetParent(roomListParent.transform);
            // room name  , player, button
            roomListItemObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = roomItem.Name;
            roomListItemObject.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = roomItem.PlayerCount + "/ " + roomItem.MaxPlayers;
            roomListItemObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() => RoomJoinFromList(roomItem.Name));
            roomListItemObject.transform.localScale = Vector3.one;
            roomListObjects.Add(roomItem.Name,roomListItemObject);
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log(" OnJoinedLobby");
    }
    public override void OnLeftLobby()
    {
        Debug.Log(" OnLeftLobby");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "Joined");
        playerName1.text = PhotonNetwork.LocalPlayer.NickName;
        playerName2.text = newPlayer.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + "Left Room");
    }

    #endregion


    #region OnEvent

    public void OnEvent(EventData photonEvent)
    {
        
        Debug.Log(photonEvent.Code + "...EventCode");
       
        if (photonEvent.Code == playerData)
        {
            Debug.Log("Event Recieved " + playerData);
            var receivedData = (object[])photonEvent.CustomData;
            var player = (Player)receivedData[0];
            var player1Name = (string)receivedData[1];
            var player2Name = (string)receivedData[2];
            playerName1.text = player1Name;
            playerName2.text = player2Name;
        }

        if (photonEvent.Code == btnContent) 
        {
            object[] data = (object[])photonEvent.CustomData;
            int actorNumber = (int)data[0];
            if (PhotonNetwork.LocalPlayer.ActorNumber != actorNumber)
            {
                Counter.cInstance.clickBtn.gameObject.SetActive(false);
            }
        }
        /*if (photonEvent.Code == scene)
        {
            Debug.Log("Event Recieved for scene" + scene);
            var receivedData = (object[])photonEvent.CustomData;
            var player = (Player)receivedData[0];
            SceneManager.LoadScene("GameScene");
          
        }*/
    }
    #endregion

}
