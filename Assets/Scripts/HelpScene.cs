/*
 * This script manage the interactions of the Help scene.
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpScene : MonoBehaviour {

	void Start () {

    }
	
	void Update () {
		
	}

    /* Returns to Start scene */
    public void GoHome() {
        SceneManager.LoadScene("Start");
    }
}
