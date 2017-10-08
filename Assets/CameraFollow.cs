using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject cam;
    private int dist = 5;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        float x = cam.transform.position.x + dist;
        float y = cam.transform.position.y + dist;
        float z = cam.transform.position.z + dist;
        this.transform.position = new Vector3(x, y, z);
    }
}
