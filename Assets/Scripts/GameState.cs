using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

    public static string player1Name = "Player1";
    public static string player2Name = "Player2";

    public static bool gameEnded = false;
    public static string winText = "You win!";
    public static string loseText = "You lose!";
    public static string endGameText = "Click 'P' to play again\n" +
                                "Click 'H' to go home";
    public static string losingPlayer = "Player1";

    public static string scoreTextPrefix = "Wins: ";
    public static int scorePlayer1 = 0;
    public static int scorePlayer2 = 0;

    /* Gets text of win/lose description */
    public static string getWinText(string currentPlayer) {
        if(string.Compare(currentPlayer, losingPlayer) != 0) {
            return winText;
        } else if(string.Compare(currentPlayer, losingPlayer) != 0) {
            return winText;
        } else {
            return loseText;
        }
    }

    /* Updates score of the two players */
    public static void updateScores() {
        if (string.Compare(losingPlayer, player1Name) == 0) {
            Debug.Log(losingPlayer);
            scorePlayer2++;
        } else {
            scorePlayer1++;
        }
    }

    /* Returns score of the player passed to the function */
    public static string getScoreText(string currentPlayer) {
        if (string.Compare(currentPlayer, player1Name) == 0) {
            return scoreTextPrefix + scorePlayer1;
        }
        else {
            return scoreTextPrefix + scorePlayer2;
        }
    }
}
