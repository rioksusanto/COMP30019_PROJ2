using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    public static bool gameEnded = false;
    public static string winText = "You win!";
    public static string loseText = "You lose!";
    public static string endGameText = "Click 'P' to play again\n" +
                                "Click 'H' to go home";
    public static string losingPlayer = "Player1";

    public static string getWinText(string currentPlayer) {
        if(string.Compare(currentPlayer, losingPlayer) != 0) {
            return winText;
        } else if(string.Compare(currentPlayer, losingPlayer) != 0) {
            return winText;
        } else {
            return loseText;
        }
    }
}
