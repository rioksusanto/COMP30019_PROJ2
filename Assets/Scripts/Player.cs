using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float thrust;
    public Camera camera;
    public bool isPlayer1 = false;
    public bool speed = false;
    public Transform sparksParticleEffect;

    private Color color;
    private Rigidbody rb;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule particleEmitter;

    // Use this for initialization
    void Start() {
        rb = this.GetComponent<Rigidbody>();
        color = this.GetComponent<Renderer>().material.color;

        sparksParticleEffect = Instantiate(sparksParticleEffect, transform.position, transform.rotation);
        particleSystem = sparksParticleEffect.GetComponent<ParticleSystem>();
        particleEmitter = particleSystem.emission;
        particleEmitter.enabled = true;
    }

    // Update is called once per frame
    void Update() {
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0;

        if (isPlayer1) {
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
        } else {
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

    /* Create sparks upon collision */
    void OnCollisionEnter(Collision col) {
        sparksParticleEffect.transform.position = col.contacts[0].point;

        if (col.gameObject.tag == "Player") {
            particleSystem.Play();
        }
    }

    public Color getColor() {
        return this.color;
    }
}
