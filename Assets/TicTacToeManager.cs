using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.SceneManagement;

public class TicTacToeManager : MonoBehaviourPun
{
    public Button[] buttons;
    private int[] board;
    private bool gameEnded;

    

    public TMP_Text youWonText;
    public Button replayBtn;

    #region STATIC BYTE
    public static byte MasterClient_Byte = 1;
    public static byte otherPlayer_Byte = 2;
    public static byte Replay_Game_Byet = 3;
    #endregion

    private void Awake()
    {
        board = new int[9];
        gameEnded = false;


        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => OnButtonClick(index));
        }

        // Replay button
        replayBtn.onClick.AddListener(() => Replay());

        PhotonNetwork.AutomaticallySyncScene = true;

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

        int currentPlayer = base.photonView.IsMine ? PhotonNetwork.LocalPlayer.ActorNumber : PhotonNetwork.MasterClient.ActorNumber;

        buttons[index].GetComponentInChildren<TMP_Text>().text = currentPlayer == PhotonNetwork.LocalPlayer.ActorNumber ? "X" : "O";
        buttons[index].GetComponent<Button>().interactable = false;

        if (CheckWin(currentPlayer))
        {
            string winnerName = currentPlayer == PhotonNetwork.LocalPlayer.ActorNumber ? PhotonNetwork.LocalPlayer.NickName : PhotonNetwork.MasterClient.NickName;
            Debug.Log("Player " + winnerName + " wins!");
            youWonText.text = "Player " + winnerName + " wins!";
            gameEnded = true;
        }

        if (IsBoardFull() && !gameEnded)
        {
            Debug.Log("It's a draw!");
            youWonText.text = "It's a draw!";
            gameEnded = true;
        }

        SendGameState(index);
    }

    public bool CheckWin(int player)
    {
        bool isWin = (board[0] == player && board[1] == player && board[2] == player) ||
                     (board[3] == player && board[4] == player && board[5] == player) ||
                     (board[6] == player && board[7] == player && board[8] == player) ||
                     (board[0] == player && board[3] == player && board[6] == player) ||
                     (board[1] == player && board[4] == player && board[7] == player) ||
                     (board[2] == player && board[5] == player && board[8] == player) ||
                     (board[0] == player && board[4] == player && board[8] == player) ||
                     (board[2] == player && board[4] == player && board[6] == player);

        Debug.Log("CheckWin for player " + player + ": " + isWin);
        Debug.Log("Board: " + string.Join(", ", board));

        return isWin;
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

    public void Replay()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        PhotonNetwork.LoadLevel(currentScene.buildIndex);
       
       
    }

    void SendGameState(int index)
    {
        if (base.photonView.IsMine)
        {
            object[] data = new object[] { board, index, gameEnded };
            PhotonNetwork.RaiseEvent(MasterClient_Byte, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
        else
        {
            object[] data = new object[] { board, index, gameEnded };
            PhotonNetwork.RaiseEvent(otherPlayer_Byte, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }
    }

    private void OnEvent(EventData obj)
    {
        if (obj.Code == MasterClient_Byte)
        {
            object[] datas = (object[])obj.CustomData;
            board = (int[])datas[0];
            int index = (int)datas[1];
            gameEnded = (bool)datas[2];
            buttons[index].GetComponentInChildren<TMP_Text>().text = "X";
            buttons[index].GetComponent<Button>().interactable = false;
        }

        if (obj.Code == otherPlayer_Byte)
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




