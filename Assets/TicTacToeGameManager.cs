using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TicTacToeGameManager : MonoBehaviourPunCallbacks
{
    // Player prefabs for X and O symbols
    public GameObject playerXPrefab;
    public GameObject playerOPrefab;
    public TicTacToeUI ticTacToeUI;

    // Reference to the player GameObject of the local player
    private GameObject localPlayer;


  
   
    public void OnCellClicked(int row, int col)
    {
        if (photonView.IsMine)
        {
            // Get the symbol of the local player
            TicTacToePlayer playerScript = localPlayer.GetComponent<TicTacToePlayer>();
           var symbol = playerScript.GetSymbol();

            // Mark the clicked cell with the symbol
            photonView.RPC("MarkCell", RpcTarget.AllBuffered, row, col, (int)symbol);
        }
    }

    // RPC method to mark the cell with the symbol across the network
    [PunRPC]
    private void MarkCell(int row, int col, int symbol)
    {
        // Convert the integer symbol to PlayerSymbol enum
        TicTacToePlayer.PlayerSymbol playerSymbol = (TicTacToePlayer.PlayerSymbol)symbol;

        // Update the UI by calling the MarkCell method of TicTacToeUI script
        ticTacToeUI.MarkCell(row, col, playerSymbol);
    }
}
