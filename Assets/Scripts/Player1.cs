using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {
    public float thrust;
    public Camera camera;
    public bool speed = false;
    public Color color = Color.green;

    private Rigidbody rb;

    // Use this for initialization
    void Start() {
        rb = this.GetComponent<Rigidbody>();
        this.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update() {
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0;
        if (Input.GetKey(KeyCode.W)) {
            rb.AddForce(cameraForward * thrust);
        }
        if (Input.GetKey(KeyCode.S)) {
            rb.AddForce(-cameraForward * thrust);
        }
        if (Input.GetKey(KeyCode.A)) {
            rb.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            rb.transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);
        }
    }
}
