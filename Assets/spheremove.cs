using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spheremove : MonoBehaviour {

    public float thrust;
    public Rigidbody rb;

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody>();
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
