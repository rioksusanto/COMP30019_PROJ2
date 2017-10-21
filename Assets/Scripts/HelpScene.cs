using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpScene : MonoBehaviour {

    public GameObject canvas;

	// Use this for initialization
	void Start () {
        //canvas.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(1000, 1000);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GoHome() {
        SceneManager.LoadScene("Start");
    }
}
