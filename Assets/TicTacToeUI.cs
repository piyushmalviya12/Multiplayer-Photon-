using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TicTacToeUI : MonoBehaviour
{
    // List of buttons representing the cells
    public List<Button> cellButtons = new List<Button>();
    
    // Method to mark the cell with the symbol
    public void MarkCell(int row, int col, TicTacToePlayer.PlayerSymbol symbol)
    {
        // Get the index of the button in the list based on row and column
        int index = row * 3 + col;

        // Ensure the index is valid
        if (index >= 0 && index < cellButtons.Count)
        {
            // Update the text of the button to display the player's symbol (X or O)
            cellButtons[index].GetComponentInChildren<TMP_Text>().text = (symbol == TicTacToePlayer.PlayerSymbol.X) ? "X" : "O";

            // Optionally, you can disable the button after it has been marked
            cellButtons[index].interactable = false;
        }
        else
        {
            Debug.LogError("Invalid cell index.");
        }
    }
}
