using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {
    public float thrust;
    public Camera camera;

    private Rigidbody rb;

    void Start() {
        rb = this.GetComponent<Rigidbody>();
    }

    /* Update is called once per frame */
    void Update() {
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0;
        if (Input.GetKey(KeyCode.UpArrow)) {
            rb.AddForce(cameraForward * thrust);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            rb.AddForce(-cameraForward * thrust);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            rb.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            rb.transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);
        }
    }
}
