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
    public GameObject[] powerUpsIndicator;
    public Text fps;
    public Text winState;
    public Text endGameDescription;

    /* Stores value used to detect at which y value a player is indicated as dead (fall off plane) */
    private int deathThreshold = -10;

    /* Stores current number of power ups */
    private int powerUpCount = 0; 

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

        /* Hide all the power up icons first */
        foreach(GameObject powerUp in powerUpsIndicator) {
            powerUp.SetActive(false);
        }
        fps.text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();
        winState.text = "";
        endGameDescription.text = "";
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
                if (Input.GetKeyDown(KeyCode.Space) && powerUpCount > 0) {
                    boostPowerUp(cameraForward);
                }
                //Debug.Log("Speed: " + rb.velocity + " | force: " + cameraForward*thrust);
            }
            else {
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
                if (Input.GetKeyDown(KeyCode.KeypadEnter) && powerUpCount > 0) {
                    boostPowerUp(cameraForward);
                }
            }
        } else {
            showEndGameInformation();
        }

        /* Check for options after game ended. P to play again and H to go home/main screen. */
        if (Input.GetKey(KeyCode.P)) {
            GameState.gameEnded = false;
            winState.text = "";
            endGameDescription.text = "";
        }
        if (Input.GetKey(KeyCode.H)) {
            SceneManager.LoadScene("Start");
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
            particleSystem.Play();
        }
    }

    /* Power up effect */
    private void boostPowerUp(Vector3 cameraForward) {
        powerUpCount--;
        powerUpsIndicator[powerUpCount].SetActive(false);
        this.maxVelocity = 300f;
        rb.AddForce(cameraForward * thrust * 100);
    }

    /* Restarts the game if player falls off */
    private void checkIfDead() {
        if (transform.position.y < deathThreshold) {
            SceneManager.LoadScene("purification");
            GameState.gameEnded = true;
            GameState.losingPlayer = this.gameObject.name;
        }
    }

    private void showEndGameInformation() {
        winState.text = GameState.getWinText(this.gameObject.name);
        endGameDescription.text = GameState.endGameText;
    }

    /* Called by power up prefab when player collides with it */
    public void increasePowerUpCount() {
        if(powerUpCount < 3) {
            powerUpsIndicator[powerUpCount].SetActive(true);
            powerUpCount++;
        }
    }

    public Color getColor() {
        return this.color;
    }
}
