using UnityEngine;
using Photon.Pun;

public class TicTacToePlayer : MonoBehaviourPun
{
    // Enum to represent the player's symbol
    public enum PlayerSymbol { X, O }

    // Current player's symbol
    private PlayerSymbol symbol;

    // Method to get the player's symbol
    public PlayerSymbol GetSymbol()
    {
        return symbol;
    }

    // Method to set the player's symbol
    public void SetSymbol(PlayerSymbol newSymbol)
    {
        symbol = newSymbol;
    }

    // Additional methods and functionalities can be added here
}
