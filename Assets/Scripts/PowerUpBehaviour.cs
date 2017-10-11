﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour {
    public float spinSpeed;
    private Player playerScript;
    private UnityEngine.Renderer playerRender;
    private Color speedyColor = Color.red;
    private bool taken = false;

    public Transform powerUpParticleEffect;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule emission;

    void Start () {

        /* Set particle system to emit up towards positive y-axis */
        powerUpParticleEffect = Instantiate(powerUpParticleEffect, transform.position, Quaternion.Euler(-90, 0, 0));
        particleSystem = powerUpParticleEffect.GetComponent<ParticleSystem>();
        particleSystem.Play();
    }
	
	void Update () {
        this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, new Vector3(0.0f, 1.0f, 0.0f));
    }

    void OnTriggerEnter(Collider other)
    {
        
        playerScript = other.gameObject.GetComponent<Player>();
        playerRender = other.gameObject.GetComponent<Renderer>();
        if (playerScript && !taken && playerScript.speed)
        {
            Destroy(this.gameObject);
        }

        else if (playerScript && !taken)
        {
            playerScript.thrust *= 1.5f;
            playerRender.material.color = speedyColor;
            playerScript.speed = true;
            Invoke("StopPower", 5);
            Invoke("testis", 2);
            taken = true;
        }
        this.GetComponent<Renderer>().enabled = false;
        Physics.IgnoreCollision(other, this.GetComponent<Collider>());

        Destroy(this.powerUpParticleEffect.gameObject);

    }

    void testis()
    {
        Debug.Log("test");
    }

    void StopPower()
    {
        playerScript.thrust /= 1.5f;
        playerRender.material.color = playerScript.getColor();
        playerScript.speed = false;
        Destroy(this.gameObject);
    }
}
