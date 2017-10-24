/*
 * This script manage the interactions of the Start scene.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		
	}

    /* Launch the game */
    public void StartGame() {
        GameState.gameEnded = false;
        SceneManager.LoadScene("Game");
    }

    /* Launch help screen */
    public void StartHelp() {
        SceneManager.LoadScene("Help");
    }
}
