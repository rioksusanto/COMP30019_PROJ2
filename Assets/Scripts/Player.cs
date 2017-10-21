using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public float thrust;
    public float maxVelocity = 150f;
    public Camera camera;
    public bool isPlayer1 = false;
    public bool speed = false;
    public Transform sparksParticleEffect;

    /* UI elements */
    public GameObject[] SpeedyIndicator;
    public Text fps;
    public Text winState;
    public Text endGameDescription;
    public Text score;

    /* Stores value used to detect at which y value a player is indicated as dead (fall off plane) */
    private int deathThreshold = -10;

    /* Stores current number of power ups */
    private int powerUpCount = 0; 
    private int speedyCount = 0; 

    private Color color;
    private Rigidbody rb;
    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule particleEmitter;
    private bool big = false;
    private bool hiding = false;
    public PlaneController plane;
    GameObject ring;

    public bool getBig()
    {
        return this.big;
    }

    public bool getHiding()
    {
        return this.hiding;
    }

    // Use this for initialization
    void Start() {
        rb = this.GetComponent<Rigidbody>();
        color = this.GetComponent<Renderer>().material.color;
        ring = this.transform.GetChild(0).gameObject;

        sparksParticleEffect = Instantiate(sparksParticleEffect, transform.position, transform.rotation);
        particleSystem = sparksParticleEffect.GetComponent<ParticleSystem>();
        particleEmitter = particleSystem.emission;
        particleEmitter.enabled = true;
        
        /* Hide all the power up icons first */
        foreach (GameObject powerUp in SpeedyIndicator) {
            powerUp.SetActive(false);
        }
        fps.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();
        winState.text = "";
        endGameDescription.text = "";
        score.text = GameState.getScoreText(this.gameObject.name);
    }

    // Update is called once per frame
    void Update() {
        checkIfDead();
        fps.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();

        this.maxVelocity = 150f;
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0;

        /* Allow movement controls only when game has started */
        if (!GameState.gameEnded) {
            cleanScreenText();

            if (isPlayer1) {
                if (Input.GetKey(KeyCode.W)) {
                    rb.AddForce(cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    rb.AddForce(-cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    rb.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    rb.transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);
                }
                if (Input.GetKeyDown(KeyCode.Space) && speedyCount > 0)
                {
                    boostPowerUp(cameraForward);
                }
                //Debug.Log("Speed: " + rb.velocity + " | force: " + cameraForward*thrust);
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    rb.AddForce(cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    rb.AddForce(-cameraForward * thrust);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rb.transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rb.transform.Rotate(new Vector3(0, -30, 0) * Time.deltaTime);
                }
                if (Input.GetKeyDown(KeyCode.KeypadEnter) && speedyCount > 0)
                {
                    boostPowerUp(cameraForward);
                }
            }
        } else {
            showEndGameInformation();
            score.text = GameState.getScoreText(this.gameObject.name);

            /* Check for options after game ended. P to play again and H to go home/main screen. */
            if (Input.GetKey(KeyCode.P)) {
                GameState.gameEnded = false;
            }
            if (Input.GetKey(KeyCode.H)) {
                SceneManager.LoadScene("Start");
            }
        }
    }

    /* Set limit to player's maximum speed */
    void FixedUpdate() {
        if (rb.velocity.magnitude > maxVelocity) {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    /* Create sparks upon collision */
    void OnCollisionEnter(Collision col) {
        sparksParticleEffect.transform.position = col.contacts[0].point;

        if (col.gameObject.tag == "Player") {
            Debug.Log(col.relativeVelocity.magnitude);
            particleSystem.Play();
        }
    }

    /* Power up effect */
    private void boostPowerUp(Vector3 cameraForward) {
        speedyCount--;
        SpeedyIndicator[speedyCount].SetActive(false);
        this.maxVelocity *= 10f;
        /*if (this.transform.position.x > plane.getScale_x() + 2 || this.transform.position.x < -plane.getScale_x() - 2
            || this.transform.position.z > plane.getScale_z() + 2 || this.transform.position.z < -plane.getScale_z() - 2)
        {
            Vector3 cameraUpward = camera.transform.up;
            cameraUpward.x = 0;
            cameraUpward.z = 0;
            rb.AddForce(cameraUpward * thrust * 10);
        }*/
        rb.AddForce(cameraForward * thrust * 50);
        
    }

    /* Restarts the game if player falls off */
    private void checkIfDead() {
        if (transform.position.y < deathThreshold) {
            SceneManager.LoadScene("purification");
            GameState.gameEnded = true;
            GameState.losingPlayer = this.gameObject.name;
            GameState.updateScores();
        }
    }

    public void cleanScreenText() {
        winState.text = "";
        endGameDescription.text = "";
    }

    private void showEndGameInformation() {
        winState.text = GameState.getWinText(this.gameObject.name);
        endGameDescription.text = GameState.endGameText;
    }

    /* Called by power up prefab when player collides with it */
    public void increaseSpeedyCount() {
        if(speedyCount < 3) {
            SpeedyIndicator[speedyCount].SetActive(true);
            speedyCount++;
        }
    }

    public void clearScreenText()
    {
        winState.text = "";
        endGameDescription.text = "";
    }

    public Color getColor() {
        return this.color;
    }

    public void massUp()
    {
        big = true;
        this.GetComponent<Rigidbody>().mass *= 30;
        this.thrust *= 30;
        this.maxVelocity = this.maxVelocity * 30;
        this.GetComponent<Renderer>().material.color = Color.grey;
        Invoke("massDown", 10);
        plane.hider = true;
    }

    public void massDown()
    {
        big = false;
        this.GetComponent<Rigidbody>().mass /= 30;
        this.thrust /= 30;
        this.maxVelocity = this.maxVelocity / 30;
        this.GetComponent<Renderer>().material.color = color;
        plane.hider = false;
    }

    public void hide()
    {
        this.hiding = true;
        gameObject.layer = LayerMask.NameToLayer("Invisibility");
        ring.layer = LayerMask.NameToLayer("Invisibility");
        camera.cullingMask |= 1 << LayerMask.NameToLayer("Invisibility");
        Invoke("unhide", 10);
    }

    public void unhide()
    {
        this.hiding = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        ring.layer = LayerMask.NameToLayer("Default");
        camera.cullingMask &= ~(1 << LayerMask.NameToLayer("Invisibility"));
    }
}
