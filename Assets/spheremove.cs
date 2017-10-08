using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMove : MonoBehaviour {

    public float thrust = 0;
    public Rigidbody rb;
    public bool speed = false;
    public Color color = Color.green;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
        this.GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Camera.main.transform.forward * thrust);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-Camera.main.transform.forward * thrust);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-Camera.main.transform.right * thrust);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Camera.main.transform.right * thrust);
        }
    }
}
