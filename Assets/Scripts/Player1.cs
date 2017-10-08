using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour {
    public float thrust;
    public Camera camera;
    public bool speed = false;
    public Color color = Color.green;

    private Rigidbody rb;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule emission;

    // Use this for initialization
    void Start() {
        rb = this.GetComponent<Rigidbody>();
        this.GetComponent<Renderer>().material.color = color;

        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
        emission = particleSystem.emission;
        emission.enabled = false;
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
    
    /* Create sparks upon collision */
    void OnCollisionEnter(Collision col) {
        if (col.gameObject.name == "Sphere2") {
            emission.enabled = true;
            particleSystem.Play();
        }
    }
}
