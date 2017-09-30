using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    public float thrust;
    public Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(Camera.main.transform.forward * thrust);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(-Camera.main.transform.forward * thrust);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.AddForce(-Camera.main.transform.right * thrust);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.AddForce(Camera.main.transform.right * thrust);
        }
    }
}
