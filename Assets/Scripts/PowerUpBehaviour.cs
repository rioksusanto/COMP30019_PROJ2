using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour {
    public float spinSpeed;
    private Player1 playerScript;
    private UnityEngine.Renderer playerRender;
    private Color speedyColor = Color.red;
    private bool taken = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localRotation *= Quaternion.AngleAxis(Time.deltaTime * spinSpeed, new Vector3(0.0f, 1.0f, 0.0f));
    }

    void OnTriggerEnter(Collider other)
    {
        
        playerScript = other.gameObject.GetComponent<Player1>();
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
        
    }

    void testis()
    {
        Debug.Log("test");
    }

    void StopPower()
    {
        playerScript.thrust /= 2;
        playerRender.material.color = playerScript.color;
        playerScript.speed = false;
        Destroy(this.gameObject);
    }
}
