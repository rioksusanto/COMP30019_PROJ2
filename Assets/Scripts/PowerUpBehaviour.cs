﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour {
    public float spinSpeed;
    private Player player;
    private UnityEngine.Renderer playerRender;
    private Color speedyColor = Color.red;
    private bool taken = false;

    public Transform powerUpParticleEffect;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule emission;
    private float timer = 0;
    private int lifeTime = 20;

    void Start () {

        /* Set particle system to emit up towards positive y-axis */
        powerUpParticleEffect = Instantiate(powerUpParticleEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        particleSystem = powerUpParticleEffect.GetComponent<ParticleSystem>();
        particleSystem.Play();
        Invoke("TimeOut", lifeTime);
    }
	
	void Update () {
        this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, new Vector3(0.0f, 1.0f, 0.0f));
    }

    void OnTriggerEnter(Collider col) {
        player = col.gameObject.GetComponent<Player>();
        if(player != null) {
            player.increasePowerUpCount();
        }

        this.GetComponent<Renderer>().enabled = false;
        Physics.IgnoreCollision(col, this.GetComponent<Collider>());

        Destroy(this.gameObject);
        Destroy(this.powerUpParticleEffect.gameObject);
    }

    void TimeOut() {
        Destroy(this.gameObject);
        Destroy(this.powerUpParticleEffect.gameObject);
    }
}
