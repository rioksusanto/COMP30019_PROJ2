/*
 * This script detects collisions between player and power ups
 * and activates that power up on the player.
 */ 

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour {
    public float spinSpeed;
    private Player player;
    private UnityEngine.Renderer playerRender;
    private bool taken = false;

    public Transform powerUpParticleEffect;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule emission;
    private float timer = 0;
    private int lifeTime = 20;
    private int type;

    void Start () {

        /* Set particle system to emit up towards positive y-axis */
        powerUpParticleEffect = Instantiate(powerUpParticleEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        particleSystem = powerUpParticleEffect.GetComponent<ParticleSystem>();
        particleSystem.Play();

        /* Configure current power up (ratio of spawn of Speed Boost:Condense:Invisible = 7:1:2) */
        type = Random.Range(0, 10);
        if (type < 7) {
            type = 1;
            this.GetComponent<Renderer>().material.color = Color.yellow;
        } else if (type < 8) {
            type = 2;
            this.GetComponent<Renderer>().material.color = Color.blue;
        } else {
            type = 3;
            this.GetComponent<Renderer>().material.color = Color.green;
        }

        Invoke("TimeOut", lifeTime);
    }
	
	void Update () {
        this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, new Vector3(0.0f, 1.0f, 0.0f));
    }

    /* Applies effect of power up to player */
    void OnTriggerEnter(Collider col) {
        player = col.gameObject.GetComponent<Player>();
        if (player != null) {
            if (type == 1) {
                player.IncreaseSpeedyCount();
            } else if (type == 2) {
                if (!player.GetBig()) {
                    player.MassUp();
                }
            } else {
                if (!player.GetHiding() || !player.plane.hider) {
                    player.Hide();
                }
            }
        }

        this.GetComponent<Renderer>().enabled = false;
        Physics.IgnoreCollision(col, this.GetComponent<Collider>());

        Destroy(this.gameObject);
        Destroy(this.powerUpParticleEffect.gameObject);
    }

    /* Removes power up */
    void TimeOut() {
        Destroy(this.gameObject);
        Destroy(this.powerUpParticleEffect.gameObject);
    }
}
