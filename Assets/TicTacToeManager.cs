using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;
using TMPro;

public class TicTacToeManager : MonoBehaviourPun
{
    public Button[] buttons;
    private int[] board;
    private bool gameEnded;

    public TMP_Text youWonText;

    private void Awake()
    {
        board = new int[9];
        gameEnded = false;


        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }
    }
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }



    public void OnDisable()
    {

        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    void OnButtonClick(int index)
    {

        if (gameEnded || board[index] != 0)
            return;

        buttons[index].GetComponentInChildren<TMP_Text>().text = base.photonView.IsMine ? "X" : "O";
        buttons[index].GetComponent<Button>().interactable = false;


        if (CheckForWin(PhotonNetwork.MasterClient.ActorNumber))
        {
            Debug.Log("Player " + PhotonNetwork.MasterClient.NickName + " wins!");
            youWonText.text = "Player " + PhotonNetwork.MasterClient.NickName + " wins!";
            gameEnded = true;
        }


        if (IsBoardFull())
        {
            Debug.Log("It's a draw!");
            youWonText.text = "It's a draw!";
            gameEnded = true;
            return;
        }


        SendGameState(index);
    }

    bool CheckForWin(int player)
    {

        if (((board[0] == player && board[1] == player && board[2] == player) ||
            (board[3] == player && board[4] == player && board[5] == player) ||
            (board[6] == player && board[7] == player && board[8] == player) ||
            (board[0] == player && board[3] == player && board[6] == player) ||
            (board[1] == player && board[4] == player && board[7] == player) ||
            (board[2] == player && board[5] == player && board[8] == player) ||
            (board[0] == player && board[4] == player && board[8] == player) ||
            (board[2] == player && board[4] == player && board[6] == player)) && base.photonView)
        {
            return true;
        }
        return false;
    }

    bool IsBoardFull()
    {
        foreach (int cell in board)
        {
            if (cell == 0)
                return false;
        }
        return true;
    }

    void SendGameState(int index)
    {
        if (base.photonView.IsMine)
        {
            object[] data = new object[] { board, index, gameEnded };
            PhotonNetwork.RaiseEvent(1, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
        else
        {
            object[] data = new object[] { board, index, gameEnded };
            PhotonNetwork.RaiseEvent(2, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    private void OnEvent(EventData obj)
    {
        if (obj.Code == 1)
        {
            object[] datas = (object[])obj.CustomData;
            board = (int[])datas[0];
            int index = (int)datas[1];
            gameEnded = (bool)datas[2];
            buttons[index].GetComponentInChildren<TMP_Text>().text = "X";
            buttons[index].GetComponent<Button>().interactable = false;
        }

        if (obj.Code == 2)
        {
            object[] datas = (object[])obj.CustomData;
            board = (int[])datas[0];
            int index = (int)datas[1];
            gameEnded = (bool)datas[2];
            buttons[index].GetComponentInChildren<TMP_Text>().text = "O";
            buttons[index].GetComponent<Button>().interactable = false;
        }
    }


}




/*using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using TMPro;
using Photon.Realtime;

public class TicTacToeManager : MonoBehaviourPun
{
    public Button[] buttons;
    private int[] board;
    private bool gameEnded;

    public TMP_Text youWonText;
    int index1;
    private void Awake()
    {
        board = new int[9];
        gameEnded = false;

        for (int i = 0; i < buttons.Length; i++)
        {
            int index2 = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index2));
        }
    }
    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }



    public void OnDisable()
    {

        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }
    void OnButtonClick(int index)
    {
        index1 = index;
        if (gameEnded || board[index] != 0)
            return;

        
        CheckGameState();
    }

   *//* void UpdateBoard(int index, int player)
    {
        
        board[index] = player;
        buttons[index].GetComponentInChildren<TMP_Text>().text = player == 1 ? "X" : "O";
        buttons[index].interactable = false;
        
    }*//*

    void CheckGameState()
    {
        if (CheckForWin(PhotonNetwork.IsMasterClient ? 1 : 2))
        {
            Debug.Log("Player " + (PhotonNetwork.IsMasterClient ? PhotonNetwork.MasterClient.NickName: PhotonNetwork.LocalPlayer.NickName) + " wins!");
            youWonText.text = "Player " + (PhotonNetwork.IsMasterClient ? PhotonNetwork.MasterClient.NickName : PhotonNetwork.LocalPlayer.NickName) + " wins!";
            gameEnded = true;
        }
        else if (IsBoardFull())
        {
            Debug.Log("It's a draw!");
            youWonText.text = "It's a draw!";
            gameEnded = true;
        }
        else
        {
            SendGameState(index1);
        }
    }

    bool CheckForWin(int player)
    {
        if ((board[0] == player && board[1] == player && board[2] == player) ||
            (board[3] == player && board[4] == player && board[5] == player) ||
            (board[6] == player && board[7] == player && board[8] == player) ||
            (board[0] == player && board[3] == player && board[6] == player) ||
            (board[1] == player && board[4] == player && board[7] == player) ||
            (board[2] == player && board[5] == player && board[8] == player) ||
            (board[0] == player && board[4] == player && board[8] == player) ||
            (board[2] == player && board[4] == player && board[6] == player))
        {
            return true;
        }
        return false;
    }

    bool IsBoardFull()
    {
        foreach (int cell in board)
        {
            if (cell == 0)
                return false;
        }
        return true;
    }

    void SendGameState(int index)
    {
        object[] data = new object[] { board, index, gameEnded };
        
        PhotonNetwork.RaiseEvent(1, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
    }

    public void OnEvent(EventData obj)
    {
        

       
        if (obj.Code == 1)
        {
            object[] datas = (object[])obj.CustomData;
            board = (int[])datas[0];
            int index = (int)datas[1];
            gameEnded = (bool)datas[2];
            buttons[index].GetComponentInChildren<TMP_Text>().text = "X";
            buttons[index].GetComponent<Button>().interactable = false;
            youWonText.text = "Player " + (PhotonNetwork.IsMasterClient ? PhotonNetwork.MasterClient.NickName : PhotonNetwork.LocalPlayer.NickName) + " wins!";
        }

        if (obj.Code == 2)
        {
            object[] datas = (object[])obj.CustomData;
            board = (int[])datas[0];
            int index = (int)datas[1];
            gameEnded = (bool)datas[2];
            buttons[index].GetComponentInChildren<TMP_Text>().text = "O";
            buttons[index].GetComponent<Button>().interactable = false;
            youWonText.text = "Player " + (PhotonNetwork.IsMasterClient ? PhotonNetwork.MasterClient.NickName : PhotonNetwork.LocalPlayer.NickName) + " wins!";
        }
        CheckGameState();
    }
}*/
